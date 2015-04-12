using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 开始挑战赛。
	/// </summary>
	public class StartChallenge : ISendable, IReceiveable
	{
		private bool HasChallengeId{get;set;}
		private string challengeId;
		/// <summary>
		/// 生成的挑战ID。
		/// </summary>
		public string ChallengeId
		{
			get
			{
				return challengeId;
			}
			set
			{
				if(value != null)
				{
					HasChallengeId = true;
					challengeId = value;
				}
			}
		}

		private List<TeamAdd> friendDataList;
		/// <summary>
		/// 好友的数据。
		/// </summary>
		public List<TeamAdd> FriendDataList
		{
			get
			{
				return friendDataList;
			}
			set
			{
				if(value != null)
				{
					friendDataList = value;
				}
			}
		}

		private bool HasRoundTimeout{get;set;}
		private int roundTimeout;
		/// <summary>
		/// 本局的时间限制。(超过时间，则认为战斗失败！单位：秒)
		/// </summary>
		public int RoundTimeout
		{
			get
			{
				return roundTimeout;
			}
			set
			{
				HasRoundTimeout = true;
				roundTimeout = value;
			}
		}

		/// <summary>
		/// 开始挑战赛。
		/// </summary>
		public StartChallenge()
		{
			FriendDataList = new List<TeamAdd>();
		}

		/// <summary>
		/// 开始挑战赛。
		/// </summary>
		public StartChallenge
		(
			string challengeId,
			int roundTimeout
		):this()
		{
			ChallengeId = challengeId;
			RoundTimeout = roundTimeout;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,ChallengeId);
			foreach(TeamAdd v in FriendDataList)
			{
				writer.Write(2,v);
			}
			writer.Write(3,RoundTimeout);
			return writer.GetProtoBufferBytes();
		}
		public void ParseFrom(byte[] buffer)
		{
			 ParseFrom(buffer, 0, buffer.Length);
		}
		public void ParseFrom(byte[] buffer, int offset, int size)
		{
			if (buffer == null) return;
			ProtoBufferReader reader = new ProtoBufferReader(buffer,offset,size);
			foreach (ProtoBufferObject obj in reader.ProtoBufferObjs)
			{
				switch (obj.FieldNumber)
				{
					case 1:
						ChallengeId = obj.Value;
						break;
					case 2:
						 var friendData= new TeamAdd();
						friendData.ParseFrom(obj.Value);
						FriendDataList.Add(friendData);
						break;
					case 3:
						RoundTimeout = obj.Value;
						break;
					default:
						break;
				}
			}
		}
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("{");
			sb.Append("ChallengeId : " + ChallengeId + ",");
			sb.Append("FriendDataList : [");
			for(int i = 0; i < FriendDataList.Count;i ++)
			{
				if(i == FriendDataList.Count -1)
				{
					sb.Append(FriendDataList[i]);
				}else
				{
					sb.Append(FriendDataList[i] + ",");
				}
			}
			sb.Append("],");
			sb.Append("RoundTimeout : " + RoundTimeout);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
