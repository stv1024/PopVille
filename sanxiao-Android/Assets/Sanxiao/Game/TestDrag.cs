using UnityEngine;

/// <summary>
/// 测试Drag
/// </summary>
public class TestDrag : MonoBehaviour
{
    public UILabel LblCur, LblMax;
    
    public float LastRt;
    public float MaxInterval;
    public float MaxFC;

    void Update()
    {
        var curIntv = Time.realtimeSinceStartup - LastRt;
        if (curIntv > MaxInterval) { MaxInterval = curIntv;
            MaxFC = Time.frameCount;
        }
        LblCur.text = "Cur:" + (int)(curIntv * 1000000) + "us";
        LblMax.text = "Max:" + (int) (MaxInterval*1000000) + "us\n@" + MaxFC;
        LastRt = Time.realtimeSinceStartup;
    }

    private void OnClick()
    {
        MaxInterval = 0;
        MaxFC = -1;
    }
}