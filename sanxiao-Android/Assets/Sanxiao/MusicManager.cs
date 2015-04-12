using System.Collections;
using Assets.Scripts;
using UnityEngine;

/// <summary>
/// 背景音乐管理器
/// </summary>
public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }
    void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// 音乐播放时的音量，由设置面板所控制的
    /// </summary>
    public static bool MusicOn
    {
        get { return SystemSettings.MusicOn; }
        set
        {
            SystemSettings.MusicOn = value;
            if (!Instance) return;
            if (!SystemSettings.MusicOn)
            {
                Instance.audio.enabled = false;
            }
            else
            {
                if (!Instance.audio.enabled)
                {
                    Instance.audio.enabled = true;
                    Instance.audio.volume = 1;
                }
            }
        }
    }

    private const float DurationFrom0ToMax = 1;

    public void CrossFadeIn()
    {
        if (!MusicOn)
        {
            if (audio.enabled)
            {
                audio.enabled = false;
            }
            return;
        }
        if (audio.enabled && audio.volume >= 1) return;//不重复淡入
        StartCoroutine(_CrossFadeIn());
    }
    IEnumerator _CrossFadeIn()
    {
        if (!audio.enabled)
        {
            audio.volume = 0;
            audio.enabled = true;
        }

        const float speed = 1/DurationFrom0ToMax;
        while (true)
        {
            audio.volume += speed*Time.deltaTime;
            if (audio.volume >= 1)
            {
                audio.volume = 1;
                yield break;
            }
            yield return new WaitForEndOfFrame();
        }
    }
    public void CrossFadeOut()
    {
        if (!MusicOn)
        {
            if (audio.enabled)
            {
                audio.enabled = false;
            }
            return;
        }
        if (!audio.enabled) return;//不重复淡出
        if (audio.volume <= 0.01f)
        {
            audio.enabled = false;
            return;
        }
        StartCoroutine(_CrossFadeOut());
    }

    IEnumerator _CrossFadeOut()
    {
        const float speed = 1 / DurationFrom0ToMax;
        while (true)
        {
            audio.volume -= speed * Time.deltaTime;
            if (audio.volume <= 0)
            {
                audio.enabled = false;
                yield break;
            }
            yield return new WaitForEndOfFrame();
        }
    }
}