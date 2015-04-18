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
                Instance.GetComponent<AudioSource>().enabled = false;
            }
            else
            {
                if (!Instance.GetComponent<AudioSource>().enabled)
                {
                    Instance.GetComponent<AudioSource>().enabled = true;
                    Instance.GetComponent<AudioSource>().volume = 1;
                }
            }
        }
    }

    private const float DurationFrom0ToMax = 1;

    public void CrossFadeIn()
    {
        if (!MusicOn)
        {
            if (GetComponent<AudioSource>().enabled)
            {
                GetComponent<AudioSource>().enabled = false;
            }
            return;
        }
        if (GetComponent<AudioSource>().enabled && GetComponent<AudioSource>().volume >= 1) return;//不重复淡入
        StartCoroutine(_CrossFadeIn());
    }
    IEnumerator _CrossFadeIn()
    {
        if (!GetComponent<AudioSource>().enabled)
        {
            GetComponent<AudioSource>().volume = 0;
            GetComponent<AudioSource>().enabled = true;
        }

        const float speed = 1/DurationFrom0ToMax;
        while (true)
        {
            GetComponent<AudioSource>().volume += speed*Time.deltaTime;
            if (GetComponent<AudioSource>().volume >= 1)
            {
                GetComponent<AudioSource>().volume = 1;
                yield break;
            }
            yield return new WaitForEndOfFrame();
        }
    }
    public void CrossFadeOut()
    {
        if (!MusicOn)
        {
            if (GetComponent<AudioSource>().enabled)
            {
                GetComponent<AudioSource>().enabled = false;
            }
            return;
        }
        if (!GetComponent<AudioSource>().enabled) return;//不重复淡出
        if (GetComponent<AudioSource>().volume <= 0.01f)
        {
            GetComponent<AudioSource>().enabled = false;
            return;
        }
        StartCoroutine(_CrossFadeOut());
    }

    IEnumerator _CrossFadeOut()
    {
        const float speed = 1 / DurationFrom0ToMax;
        while (true)
        {
            GetComponent<AudioSource>().volume -= speed * Time.deltaTime;
            if (GetComponent<AudioSource>().volume <= 0)
            {
                GetComponent<AudioSource>().enabled = false;
                yield break;
            }
            yield return new WaitForEndOfFrame();
        }
    }
}