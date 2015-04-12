using Assets.Sanxiao.Game;
using UnityEngine;

namespace Assets.Sanxiao.Communication.UpperPart
{
    public class RivalUseSkill : Proto.RivalUseSkill, IUpperReceivedCmd
    {
        public RivalUseSkill()
        {
        }

        public void Execute()
        {
            Debug.Log("RivalUseSkill(" + this);

            if (GameManager.Instance != null)
            {
                GameManager.Instance.Execute(this);
            }
        }
    }
}