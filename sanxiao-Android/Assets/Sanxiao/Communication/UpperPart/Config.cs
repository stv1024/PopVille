using Assets.Sanxiao.Data;
using UnityEngine;

namespace Assets.Sanxiao.Communication.UpperPart
{
    public class Config : Proto.Config, IUpperReceivedCmd
    {
        public void Execute()
        {
            CommonData.Config = this;

            ConfigManager.Execute(this);

            MainController.Instance.Execute(this);
        }
    }
}