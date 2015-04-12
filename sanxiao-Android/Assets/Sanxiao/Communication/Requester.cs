using System;
using Assets.Sanxiao.Communication.Proto;
using Assets.Sanxiao.Communication.UpperPart;

namespace Assets.Sanxiao.Communication
{
    public class Requester
    {
        private Requester()
        {
        }

        private static Requester _instance;

        public static Requester Instance
        {
            get { return _instance ?? (_instance = new Requester()); }
        }

        static void Request(Packet packet)
        {
            UnityClient.Instance.WriteCmd(packet);
        }

        /// <summary>
        /// 已catch所有Exception，不可能再向上抛出Exception
        /// </summary>
        /// <param name="cmd"></param>
        public void Send(IUpperSentCmd cmd)
        {
            try
            {
                UnityEngine.Debug.Log(string.Format("{0}:{1}", cmd.GetType().Name, cmd));

                var packet = new Packet(cmd.CmdType){Encryption = ""};

                var content = cmd.GetProtoBufferBytes();
                if (content != null)
                {
                    packet.Content = content;
                }

                Request(packet);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogException(e);
            }
        }
    }
}