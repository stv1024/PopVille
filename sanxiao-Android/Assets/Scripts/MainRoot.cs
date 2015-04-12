using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Sanxiao.UI;
using UnityEngine;
using Fairwood.Math;

/// <summary>
/// UI Root手动调节高度
/// </summary>
public class MainRoot : MonoBehaviour
{
    public static MainRoot Instance { get; private set; }

    public const int MainLayer = 8;
    public List<Camera> Cameras;

    public Transform ForegroundGUI;
    public Transform Background;

    public Sprite SprSplash, SprBg;

    void Awake()
    {
        Instance = this;
        Debug.Log("屏幕分辨率:" + Screen.width + "*" + Screen.height + "; Ratio:" + (Screen.height*1f/Screen.width));
        if (Screen.height*1f/Screen.width > 1.5f)//如果屏幕太高，就在上下增加白边
        {
// ReSharper disable PossibleLossOfFraction
            foreach (var myCamera in Cameras)
            {
                myCamera.orthographicSize = 320*Screen.height/Screen.width;
            }
// ReSharper restore PossibleLossOfFraction
            Background.localScale = new Vector3(1, Screen.height /1.5f / Screen.width, 1);
        }
        else if (Screen.height * 1f / Screen.width < 1.5f)
        {
            Background.localScale = new Vector3(Screen.width * 1.5f/ Screen.height, 1, 1);
        }
    }

    public static Vector2 StdCameraRange
    {
        get
        {
            var range = new Vector2(640, 960);
            if (Screen.height * 1f / Screen.width > 1.5f)//如果屏幕太高，就在上下增加白边
            {
                range.y *= Screen.height / 1.5f / Screen.width;
            }
            else if (Screen.height * 1f / Screen.width < 1.5f)
            {
                range.x *= Screen.width * 1.5f / Screen.height;
            }
            return range;
        }
    }

    public enum UIStateName
    {
        Entrance = 0,
        Menu = 1,
        Match = 2,
        Game = 3,
        PushLevel = 4,
        EndRound = 5,
    }

    readonly BaseUI[] _baseUIs = new BaseUI[16];
    private bool _isSwitching;

    UIStateName CurrentUIState
    {
        get
        {
            var ind = -1;
            for (int i = 0; i < _baseUIs.Length; i++)
            {
                if (_baseUIs[i])
                {
                    ind = i;
                    break;
                }
            }
            return (UIStateName) ind;
        }
    }
    private UIStateName _targetUIState;

    /// <summary>
    /// 确保进入某种界面，不会重复加载
    /// </summary>
    /// <param name="uiState"></param>
    public static void Goto(UIStateName uiState)
    {
        if (Instance)
        {
            Instance._targetUIState = uiState;
            if (!Instance._isSwitching)
            {
                Instance.StartCoroutine(Instance._Goto());
            }
        }
    }
    
    IEnumerator _Goto()
    {
        _isSwitching = true;
        if (GetNeedLoadingMask())
        {
            LoadingMask.StartLoading();
            yield return new WaitForSeconds(LoadingMask.Instance.TimeToUnload);
        }

        BaseUI newUI = null;
        switch (_targetUIState)
        {
            case UIStateName.Entrance:
                newUI = EntranceUI.EnterStage();
                break;
            case UIStateName.Menu:
                newUI = MenuUI.EnterStage();
                break;
            case UIStateName.Match:
                newUI = MatchUI.EnterStage();
                break;
            case UIStateName.Game:
                newUI = GameUI.EnterStage();
                break;
            case UIStateName.PushLevel:
                newUI = PushLevelUI.EnterStage();
                break;
            case UIStateName.EndRound:
                newUI = EndRoundUI.EnterStage();
                break;
            default:
                Debug.LogError("ERROR uiState:" + _targetUIState);
                break;
        }
        switch (_targetUIState)
        {
            case UIStateName.Entrance:
                var sr = Background.GetComponent<SpriteRenderer>();
                sr.sprite = SprSplash;
                break;
            case UIStateName.PushLevel:
                //Background.gameObject.SetActive(false);
                break;
            default:
                //Background.gameObject.SetActive(true);
                sr = Background.GetComponent<SpriteRenderer>();
                sr.sprite = SprBg;
                break;
        }

        if (!newUI)
        {
            Debug.LogError("GotoUI却没有成功创建UI:" + _targetUIState);
        }

        for (int i = 0; i < _baseUIs.Length; i++)
        {
            if (i == (int) _targetUIState)
            {
                _baseUIs[i] = newUI;
            }
            else
            {
                if (_baseUIs[i])
                {
                    _baseUIs[i].OffStage();
                    _baseUIs[i] = null;
                    Resources.UnloadUnusedAssets();
                }
            }
        }

        LoadingMask.EndLoading();
        _isSwitching = false;
    }

    bool GetNeedLoadingMask()
    {
        var ind = -1;
        for (int i = 0; i < _baseUIs.Length; i++)
        {
            if (_baseUIs[i])
            {
                ind = i;
                break;
            }
        }
        if ((UIStateName) ind == _targetUIState) return false;
        if (_targetUIState == UIStateName.Game) return true;
        if (_targetUIState == UIStateName.PushLevel) return true;
        switch ((UIStateName)ind)
        {
            case UIStateName.Entrance:
                return true;
            case UIStateName.Menu:
                break;
            case UIStateName.Match:
                break;
            case UIStateName.Game:
                return true;
            case UIStateName.PushLevel:
                return true;
            case UIStateName.EndRound:
                return true;
        }
        return false;
    }

    public static Transform UIParent
    {
        get { return Instance ? Instance.transform : null; }
    }
    private readonly List<BaseTempSingletonPanel> _baseTempSingletonPanels = new List<BaseTempSingletonPanel>();
    public static void ShowPanel(GameObject prefab)
    {
        foreach (var baseTempSingletonPanel in Instance._baseTempSingletonPanels)
        {
            baseTempSingletonPanel.gameObject.SetActive(false);//已存在的面板都失活
        }
        Instance._baseTempSingletonPanels.Add(PrefabHelper.InstantiateAndReset<BaseTempSingletonPanel>(prefab,
                                                                                                       Instance
                                                                                                           .transform));
        foreach (var ui in Instance._baseUIs.Where(x=>x))
        {
            ui.gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// 将Panel列表中指定的一个提到最前
    /// </summary>
    /// <param name="panel"></param>
    public static void FocusPanel(BaseTempSingletonPanel panel)
    {
        if (panel == null)
        {
            Debug.LogException(new NullReferenceException("panel"));
            return;
        }
        Instance._baseTempSingletonPanels.Remove(panel);
        foreach (var baseTempSingletonPanel in Instance._baseTempSingletonPanels.Where(baseTempSingletonPanel => baseTempSingletonPanel != panel))
        {
            baseTempSingletonPanel.gameObject.SetActive(false);
        }
        Instance._baseTempSingletonPanels.Add(panel);//将panel提到最上
        panel.gameObject.SetActive(true);//激活面板
    }
    public static void DidDestroyPanel(BaseTempSingletonPanel panel)
    {
        if (!Instance._baseTempSingletonPanels.Remove(panel)) Debug.LogError("怎么可能不在列表里，请检查bug隐患");
        if (Instance._baseTempSingletonPanels.Count > 0)
        {
            Instance._baseTempSingletonPanels.Last().gameObject.SetActive(true);//激活最上层Panel
        }
        else
        {
            foreach (var ui in Instance._baseUIs.Where(x => x))//激活UI
            {
                ui.gameObject.SetActive(true);
            }
        }
    }

    /// <summary>
    /// 当前UI/Panel状态，用于数据统计
    /// </summary>
    public string CurrentViewStateName
    {
        get
        {
            if (_baseTempSingletonPanels.Count > 0)
            {
                return _baseTempSingletonPanels.Last().GetNameForEventStats();
            }
            return CurrentUIState.ToString();
        }
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            OnEscapeClick();
        }
    }
    void OnEscapeClick()
    {
        //TODO 先处理Dialogs
        if (MorlnTooltip.IsShowing)
        {
            MorlnTooltip.ForceHide();
            return;
        }

        if (AlertDialog.Instance)
        {
            AlertDialog.Instance.ProcessEscapeEvent();
            return;
        }

        //再处理Panels
        for (var i = _baseTempSingletonPanels.Count - 1; i >= 0; i--)
        {
            if (_baseTempSingletonPanels[i].OnEscapeClick()) return;
        }

        //最后处理UIs
        foreach (var baseUI in _baseUIs.Where(x=>x))
        {
            if (baseUI.OnEscapeClick()) return;
        }
    }

    public static Vector3 InverseTransformPoint(Vector3 position)
    {
        return Instance.transform.InverseTransformPoint(position);
    }
}