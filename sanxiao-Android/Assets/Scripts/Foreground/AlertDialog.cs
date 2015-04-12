using UnityEngine;
using Fairwood.Math;

public class AlertDialog : MonoBehaviour
{
    private static AlertDialog _instance;

    public static AlertDialog Instance
    {
        get { return _instance; }
        private set
        {
            if (_instance && value)
            {
                Debug.LogError("more than 1 AlertDialog instance now!");
                Destroy(_instance.gameObject);
            }
            _instance = value;
        }
    }

    protected void OnDestroy()
    {
        _instance = null;
    }

    private static GameObject Prefab
    {
        get
        {
            var go = Resources.Load("UI/AlertDialog") as GameObject;
            return go;
        }
    }

    private static AlertDialog Load()
    {
        if (!Instance)
        {
            if (!Prefab) return null;
            Instance = PrefabHelper.InstantiateAndReset<AlertDialog>(Prefab, MainRoot.Instance.ForegroundGUI);
            return Instance;
        }
        return Instance;
    }

    public static void UnloadInterface()
    {
        if (Instance)
        {
            Destroy(Instance.gameObject);
            Instance = null;
        }
    }

    public GameObject Btn0, Btn1, BtnClose;
    public UILabel LblContent, LblButton0, LblButton1;

    public UISprite SlcBg;

    public delegate void OnButtonClickMethod();

    public OnButtonClickMethod OnButton0Click, OnButton1Click;

    public static void Load(string content)
    {
        var ad = Load();
        if (ad)
        {
            ad.LblContent.text = content;

            ad.Btn0.SetActive(false);
            ad.Btn1.SetActive(false);
            ad.BtnClose.SetActive(true);

            ad.Resize();
        }
    }
    public static void Load(string content, string button0Label,
                            OnButtonClickMethod button0ClickCallback, bool showCloseButton = false)
    {
        var ad = Load();
        if (ad)
        {
            ad.LblContent.text = content;
            var btnY = ad.LblContent.transform.localPosition.y - ad.LblContent.transform.localScale.y * ad.LblContent.printedSize.y - 50;

            ad.LblButton0.text = button0Label;
            ad.Btn0.transform.localPosition = new Vector3(0, btnY, 0);
            ad.OnButton0Click = button0ClickCallback;

            ad.Btn1.SetActive(true);
            ad.Btn1.SetActive(false);
            ad.BtnClose.SetActive(showCloseButton);

            ad.Resize();
        }
    }

    public static void Load(string content, string button0Label,
                            OnButtonClickMethod button0ClickCallback, string button1Label, OnButtonClickMethod button1ClickCallback, bool showCloseButton = false)
    {
        var ad = Load();
        if (ad)
        {
            ad.LblContent.text = content;
            ad.LblContent.AssumeNaturalSize();
            var btnY = ad.LblContent.transform.localPosition.y - ad.LblContent.transform.localScale.y * ad.LblContent.height - 50;
            ad.Btn0.SetActive(true);
            ad.LblButton0.text = button0Label;
            ad.Btn0.transform.localPosition = new Vector3(-125, btnY, 0);
            ad.OnButton0Click = button0ClickCallback;
            
            ad.Btn1.SetActive(true);
            ad.LblButton1.text = button1Label;
            ad.Btn1.transform.localPosition = new Vector3(125, btnY, 0);
            ad.OnButton1Click = button1ClickCallback;
            
            ad.BtnClose.SetActive(showCloseButton);
            
            ad.Resize();
        }
    }

    private void Resize()
    {
        var height = 100 + (-LblContent.transform.localPosition.y + LblContent.transform.localScale.y*LblContent.height);
        if (Btn0.activeSelf) height += 20;
        SlcBg.height = Mathf.CeilToInt(height);
        var pY = 113 + height*0.5f;
        transform.localPosition = transform.localPosition.SetV3Y(pY);
    }

    public void OnBtn0Click()
    {
        UnloadInterface();
        if (OnButton0Click != null) OnButton0Click();
    }

    public void OnBtn1Click()
    {
        UnloadInterface();
        if (OnButton1Click != null) OnButton1Click();
    }

    public void OnCloseClick()
    {
        UnloadInterface();
    }

    public void ProcessEscapeEvent()
    {
        if (BtnClose.activeSelf) OnCloseClick();
    }
}