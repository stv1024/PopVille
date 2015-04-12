using Assets.Sanxiao.Game;
using Assets.Sanxiao.UI.Panel;

namespace Assets.Sanxiao.Communication.UpperPart
{
    public class NicknameProvided : Proto.NicknameProvided, IUpperReceivedCmd
    {
        public void Execute()
        {
            foreach (var nickname in NicknameList)
            {
                SelectNicknamePanel.NicknameQueue.Enqueue(nickname);
            }
        }
    }
}