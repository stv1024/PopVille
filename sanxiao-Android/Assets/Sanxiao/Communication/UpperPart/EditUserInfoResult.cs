using System.Collections.Generic;
using Assets.Sanxiao.Game;
using Assets.Sanxiao.UI.Panel;

namespace Assets.Sanxiao.Communication.UpperPart
{
    public class EditUserInfoResult : Proto.EditUserInfoResult, IUpperReceivedCmd
    {
        public void Execute()
        {
            if (HasNicknameResult)
            {
                if (NicknameResult.Code == 10000)
                {
                    SelectNicknamePanel.NicknameSelectOk = true;
                }
                else
                {
                    if (NicknameResult.HasMsg) MorlnFloatingToast.Create(NicknameResult.Msg);
                }
            }
        }
    }
}