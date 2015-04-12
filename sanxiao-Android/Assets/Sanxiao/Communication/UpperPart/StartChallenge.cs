using Assets.Sanxiao.Game;
using Assets.Sanxiao.UI;
using Assets.Sanxiao.UI.Panel;
using UnityEngine;

namespace Assets.Sanxiao.Communication.UpperPart
{
    public class StartChallenge : Proto.StartChallenge, IUpperReceivedCmd
    {
        public void Execute()
        {
            MainController.Instance.Execute(this);
        }
    }
}