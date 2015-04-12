using Assets.Sanxiao.Communication.Proto;
using UnityEngine;
using Assets.Scripts;

namespace Assets.Sanxiao.UI.PushLevel
{
    public class Head : MonoBehaviour
    {
        public HeadContainer HeadContainer;

        public int MajorId
        {
            get { return SnsFriendUnlockInfo.UnlockedMajorLevelId; }
        }

        public int SubId
        {
            get { return SnsFriendUnlockInfo.UnlockedSubLevelId; }
        }

        public Transform FollowTarget { get; private set; }
        public SNSFriendUnlockInfo SnsFriendUnlockInfo { get; private set; }

        public UITexture TxrHead;

        private SpringPosition _springPosition;

        public SpringPosition SpringPosition
        {
            get
            {
                if (_springPosition) return _springPosition;
                _springPosition = GetComponent<SpringPosition>();
                if (_springPosition) return _springPosition;
                _springPosition = gameObject.AddComponent<SpringPosition>();
                return _springPosition;
            }
        }

        public void SetAndRefresh(SNSFriendUnlockInfo snsFriendUnlockInfo, Transform followTarget)
        {
            SnsFriendUnlockInfo = snsFriendUnlockInfo;
            FollowTarget = followTarget;

            TxrHead.enabled = false;
            if (snsFriendUnlockInfo.HasHeadIconUrl)
            {
                ImageResourcesManager.LoadImageAndWait(snsFriendUnlockInfo.HeadIconUrl, RefreshTexture);
            }
        }

        void RefreshTexture(Texture texture)
        {
            if (texture)
            {
                TxrHead.enabled = true;
                TxrHead.mainTexture = texture;
            }
            else
            {
                TxrHead.enabled = false;
            }
        }

        private void Update()
        {
            SpringPosition.strength = Mathf.Abs(transform.position.x) > 280 ? 20 : 100;
        }

        void OnClick()
        {
            Debug.Log("Click on:" + SnsFriendUnlockInfo.Nickname);
        }
    }
}