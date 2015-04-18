using Assets.Scripts;
using UnityEngine;

/// <summary>
/// 背景音乐管理器
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// 音效播放时的音量，由设置面板所控制的
    /// </summary>
    public static bool AudioOn
    {
        get { return SystemSettings.AudioOn; }
        set
        {
            SystemSettings.AudioOn = value;
            if (!Instance) return;
            if (!SystemSettings.AudioOn)
            {
                Instance.GetComponent<AudioSource>().enabled = false;
            }
            else
            {
                if (!Instance.GetComponent<AudioSource>().enabled) Instance.GetComponent<AudioSource>().enabled = true;
                Instance.GetComponent<AudioSource>().volume = 1;
            }
        }
    }

    public static void PlayOneShot(AudioClip clip, float volumeScale = 1)
    {
        if (!Instance || !clip || !AudioOn || volumeScale <= 0) return;
        Instance.GetComponent<AudioSource>().PlayOneShot(clip, volumeScale);
    }
    /// <summary>
    /// 随机播放一个音效
    /// </summary>
    /// <param name="volumeScale"></param>
    /// <param name="clips"></param>
    public static void PlayRandomOneShot(float volumeScale = 1,params AudioClip[] clips)
    {
        if (!Instance || clips == null || !AudioOn || volumeScale <= 0 || clips.Length == 0) return;
        Instance.GetComponent<AudioSource>().PlayOneShot(clips[Random.Range(0, clips.Length)], volumeScale);
    }
}