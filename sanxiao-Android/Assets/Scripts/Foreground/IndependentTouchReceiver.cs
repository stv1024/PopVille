using UnityEngine;

/// <summary>
/// 不阻挡UICamera事件的独立接收器，用于单例面板的底部，当点击在/不在碰撞器上则发送事件
/// </summary>
public class IndependentTouchReceiver : MonoBehaviour
{
    public GameObject EventReceiver;
    public string FunctionName;
    public bool SendWhenInside = false;

    private void Update()
    {
        if (!UICamera.currentCamera || !GetComponent<Collider>() || !EventReceiver || string.IsNullOrEmpty(FunctionName)) return;
        Vector2 inPos;
        if (Input.GetMouseButton(0))
        {
            inPos = Input.mousePosition;
        }
        else if (Input.touchCount > 0)
        {
            inPos = Input.touches[0].position;
        }
        else
        {
            return;
        }
        var ray = UICamera.currentCamera.ScreenPointToRay(inPos);

        var dist = UICamera.currentCamera.farClipPlane - UICamera.currentCamera.nearClipPlane;
        RaycastHit hit;
        if (GetComponent<Collider>().Raycast(ray, out hit, dist) == SendWhenInside)
        {
            EventReceiver.SendMessage(FunctionName, SendMessageOptions.DontRequireReceiver);
        }
    }
}