using Assets.Sanxiao.Communication.Proto;
using Assets.Scripts;
using UnityEngine;

namespace Assets.Sanxiao.UI.Panel.Reinforce
{
    public class Reinforce1Slot : MonoBehaviour
    {
        public delegate void SelectFriendMethod(Reinforce1Slot reinforce1Slot);
        private SelectFriendMethod _selectFriend;
        public RandomTeamMember FriendInfo;

        public UILabel LblNickname;
        public GameObject Tick;

        public UITexture TxrHeadIcon;

        public void SetAndRefresh(RandomTeamMember friendInfo, SelectFriendMethod selectFriend, Texture headIcon)
        {
            FriendInfo = friendInfo;
            _selectFriend = selectFriend;
            TxrHeadIcon.enabled = false;

            if (FriendInfo == null) //空槽
            {
                LblNickname.text = null;
            }
            else
            {
                LblNickname.text = FriendInfo.Nickname;
                if (headIcon)
                {
                    RefreshHeadIcon(headIcon);
                }
                else
                {
                    ImageResourcesManager.LoadImageAndWait("http://tp4.sinaimg.cn/1631220107/50/40042081476/1",
                                                           RefreshHeadIcon); //下载完成后会激活TxrHeadIcon
                }
            }
        }
        void RefreshHeadIcon(Texture texture)
        {
            if (TxrHeadIcon)
            {
                TxrHeadIcon.mainTexture = texture;
                TxrHeadIcon.enabled = true;
            }
        }

        public void ToggleSelected(bool selected)
        {
            if (Tick) Tick.SetActive(selected);
        }

        void OnClick()
        {
            if (_selectFriend != null) _selectFriend(this);
        }
    }
}