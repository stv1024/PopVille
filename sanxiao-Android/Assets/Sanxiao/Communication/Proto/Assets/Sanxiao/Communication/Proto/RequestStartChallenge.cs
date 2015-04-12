using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 请求开始挑战。
	/// </summary>
	public class RequestStartChallenge : ISendable, IReceiveable
	{
		private bool HasChallengeId{get;set;}
		private string challengeId;
		/// <summary>
		/// 挑战的id。
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

		private List<string> friendUserIdList;
		/// <summary>
		/// 请求组队的好友的userId。
		/// </summary>
		public List<string> FriendUserIdList
		{
			get
			{
				return friendUserIdList;
			}
			set
			{
				if(value != null)
				{
					friendUserIdList = value;
				}
			}
		}

		/// <summary>
		/// 请求开始挑战。
		/// </summary>
		public RequestStartChallenge()
		{
			FriendUserIdList = new List<string>();
		}

		/// <summary>
		/// 请求开始挑战。
		/// </summary>
		public RequestStartChallenge
		(
			string challengeId
		):this()
		{
			ChallengeId = challengeId;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,ChallengeId);
			foreach(string v in FriendUserIdList)
			{
				writer.Write(2,v);
			}
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
						FriendUserIdList.Add(obj.Value);
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
			sb.Append("FriendUserIdList : [");
			for(int i = 0; i < FriendUserIdList.Count;i ++)
			{
				if(i == FriendUserIdList.Count -1)
				{
					sb.Append(FriendUserIdList[i]);
				}else
				{
					sb.Append(FriendUserIdList[i] + ",");
				}
			}
			sb.Append("]");
			sb.Append("}");
			return sb.ToString();
		}
	}
}
