using UnityEngine;

public class TestLag : MonoBehaviour
{
    public Transform Rot;
    public float Speed;

    void Update()
    {
        var v = Rot.eulerAngles;
        v.z += Time.deltaTime * Speed;
        Rot.eulerAngles = v;
    }
}