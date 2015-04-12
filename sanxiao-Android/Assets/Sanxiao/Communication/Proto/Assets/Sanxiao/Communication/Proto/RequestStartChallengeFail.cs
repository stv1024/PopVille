using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 请求开始挑战失败。
	/// </summary>
	public class RequestStartChallengeFail : ISendable, IReceiveable
	{
		private bool HasChallengeId{get;set;}
		private string challengeId;
		/// <summary>
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

		private bool HasResult{get;set;}
		private MsgResult result;
		/// <summary>
		/// </summary>
		public MsgResult Result
		{
			get
			{
				return result;
			}
			set
			{
				if(value != null)
				{
					HasResult = true;
					result = value;
				}
			}
		}

		/// <summary>
		/// 请求开始挑战失败。
		/// </summary>
		public RequestStartChallengeFail()
		{
		}

		/// <summary>
		/// 请求开始挑战失败。
		/// </summary>
		public RequestStartChallengeFail
		(
			string challengeId,
			MsgResult result
		):this()
		{
			ChallengeId = challengeId;
			Result = result;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,ChallengeId);
			writer.Write(2,Result);
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
						Result = new MsgResult();
						Result.ParseFrom(obj.Value);
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
			sb.Append("Result : " + Result);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
