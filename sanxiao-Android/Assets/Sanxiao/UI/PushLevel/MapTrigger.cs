using UnityEngine;

namespace Assets.Sanxiao.UI.PushLevel
{
    /// <summary>
    /// 地图上的小物件,玩家点击后可以触发一些小动画
    /// </summary>
    public class MapTrigger : MonoBehaviour
    {
        /// <summary>
        /// 美术填写朝哪个Animator上发送Trigger
        /// </summary>
        public Animator TargetAnimator;
        /// <summary>
        /// 美术填写Trigger名称
        /// </summary>
        public string Trigger;
        void OnClick()
        {
            if (TargetAnimator) TargetAnimator.SetTrigger(Trigger);
        }
    }
}