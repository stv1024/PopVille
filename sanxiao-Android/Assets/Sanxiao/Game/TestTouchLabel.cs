using Fairwood.Math;
using UnityEngine;

/// <summary>
/// 测试Touches的Label
/// </summary>
public class TestTouchLabel : MonoBehaviour
{
    public int index;
    public UILabel Lbl;
    void Update()
    {
        if (index == -1)
        {
            Lbl.text = "mouse";
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition).SetV3Z(-100);
            return;
        }
        if (index >= Input.touches.Length){ transform.position = new Vector3(0, -480, 0);return;}
        var touch = Input.GetTouch(index);
        Lbl.text = "" + index + ":" + touch.fingerId + ";" + touch.phase;
        transform.position = Camera.main.ScreenToWorldPoint(touch.position).SetV3Z(-100);
    }
}