using System.Collections.Generic;
using Fairwood.Math;
using UnityEngine;

/// <summary>
/// 自己写的Tooltip，Show的文本自行控制换行，不会自动换行。是个单例，确保只有一个并且开局时存在并激活，用不销毁和创建
/// </summary>
public class MorlnFloatingToastContainer : MonoBehaviour
{
    public static MorlnFloatingToastContainer Instance { get; private set; }
    void Awake()
    {
        Instance = this;
    }

    public GameObject ToastTemplate;
    GameObject _toast;

    private void _Create(string text)
    {
        Destroy(_toast);
        var mft = PrefabHelper.InstantiateAndReset<MorlnFloatingToast>(ToastTemplate, transform);
        mft.gameObject.SetActive(true);
        mft.SetText(text);
        _toast = mft.gameObject;
        Destroy(_toast, _toast.animation.clip.length);
    }

    public static void Create(string text)
    {
        if (!Instance) return;
        Instance._Create(text);
    }
}