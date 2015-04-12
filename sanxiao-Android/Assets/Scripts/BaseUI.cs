using System.Collections;
using UnityEngine;

public abstract class BaseUI : BaseHasAtlasFlags
{
    #region UI通用部分

    private const float TransitionDuration = 0.0f;
    protected void OnStage()
    {
        //var uiPanels = GetComponentsInChildren<UIPanel>();
        //StartCoroutine(OnStageEffectCoroutine(uiPanels));
    }

    private IEnumerator OnStageEffectCoroutine(UIPanel[] uiPanels)
    {
        float startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < startTime + TransitionDuration)
        {
            foreach (var uiPanel in uiPanels)
            {
                uiPanel.alpha = (Time.realtimeSinceStartup - startTime)/TransitionDuration;
            }
            yield return new WaitForEndOfFrame();
        }
        foreach (var uiPanel in uiPanels)
        {
            uiPanel.alpha = 1;
        }
    }

    /// <summary>
    /// 成员方法，退场
    /// </summary>
    public virtual void OffStage()
    {
        DestroySelfAfterOffStage();
        //var uiPanels = GetComponentsInChildren<UIPanel>();
        //StartCoroutine(OffStageEffectCoroutine(uiPanels));
    }

    private IEnumerator OffStageEffectCoroutine(UIPanel[] uiPanels)
    {
        float startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < startTime + TransitionDuration)
        {
            foreach (var uiPanel in uiPanels)
            {
                uiPanel.alpha = 1 - (Time.realtimeSinceStartup - startTime) / TransitionDuration;
            }
            yield return new WaitForEndOfFrame();
        }
        foreach (var uiPanel in uiPanels)
        {
            uiPanel.alpha = 0;
        }
    }

    /// <summary>
    /// 退场，一段时间后再销毁自己
    /// </summary>
    protected void DestroySelfAfterOffStage()
    {
        Invoke("DestroySelf", TransitionDuration);//下场过渡长度
    }

    /// <summary>
    /// 释放资源
    /// </summary>
    protected abstract void Release();

    protected void DestroySelf()
    {
        Release();//释放资源
        Destroy(gameObject);//帧末销毁自己。因为已经孤立，所以无所谓在一帧的什么时间点销毁
    }

    /// <summary>
    /// 按下返回键的处理。返回false则事件会继续传递给下面的面板
    /// </summary>
    /// <returns></returns>
    public virtual bool OnEscapeClick()
    {
        return false;
    }
    #endregion
}