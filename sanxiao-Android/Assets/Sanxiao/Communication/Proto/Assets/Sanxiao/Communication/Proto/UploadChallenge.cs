using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 挑战之后上传的挑战ID。
	/// </summary>
	public class UploadChallenge : ISendable, IReceiveable
	{
		private bool HasChallengeId{get;set;}
		private string challengeId;
		/// <summary>
		/// 挑战ID。
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

		private bool HasIsWin{get;set;}
		private bool isWin;
		/// <summary>
		/// 我是否赢了。
		/// </summary>
		public bool IsWin
		{
			get
			{
				return isWin;
			}
			set
			{
				HasIsWin = true;
				isWin = value;
			}
		}

		private bool HasMyDefenseData{get;set;}
		private DefenseData myDefenseData;
		/// <summary>
		/// 我的防御数据。
		/// </summary>
		public DefenseData MyDefenseData
		{
			get
			{
				return myDefenseData;
			}
			set
			{
				if(value != null)
				{
					HasMyDefenseData = true;
					myDefenseData = value;
				}
			}
		}

		/// <summary>
		/// 挑战之后上传的挑战ID。
		/// </summary>
		public UploadChallenge()
		{
		}

		/// <summary>
		/// 挑战之后上传的挑战ID。
		/// </summary>
		public UploadChallenge
		(
			string challengeId,
			bool isWin,
			DefenseData myDefenseData
		):this()
		{
			ChallengeId = challengeId;
			IsWin = isWin;
			MyDefenseData = myDefenseData;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,ChallengeId);
			writer.Write(2,IsWin);
			writer.Write(3,MyDefenseData);
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
						IsWin = obj.Value;
						break;
					case 3:
						MyDefenseData = new DefenseData();
						MyDefenseData.ParseFrom(obj.Value);
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
			sb.Append("IsWin : " + IsWin + ",");
			sb.Append("MyDefenseData : " + MyDefenseData);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
