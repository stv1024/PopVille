using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// ---- 推图相关命令 Start ----
	/// 请求一次异步挑战。
	/// </summary>
	public class RequestChallenge : ISendable, IReceiveable
	{
		private bool HasMajorLevelId{get;set;}
		private int majorLevelId;
		/// <summary>
		/// 大关的id。
		/// </summary>
		public int MajorLevelId
		{
			get
			{
				return majorLevelId;
			}
			set
			{
				HasMajorLevelId = true;
				majorLevelId = value;
			}
		}

		private bool HasSubLevelId{get;set;}
		private int subLevelId;
		/// <summary>
		/// 小关的id。
		/// </summary>
		public int SubLevelId
		{
			get
			{
				return subLevelId;
			}
			set
			{
				HasSubLevelId = true;
				subLevelId = value;
			}
		}

		/// <summary>
		/// ---- 推图相关命令 Start ----
		/// 请求一次异步挑战。
		/// </summary>
		public RequestChallenge()
		{
		}

		/// <summary>
		/// ---- 推图相关命令 Start ----
		/// 请求一次异步挑战。
		/// </summary>
		public RequestChallenge
		(
			int majorLevelId,
			int subLevelId
		):this()
		{
			MajorLevelId = majorLevelId;
			SubLevelId = subLevelId;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,MajorLevelId);
			writer.Write(2,SubLevelId);
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
						MajorLevelId = obj.Value;
						break;
					case 2:
						SubLevelId = obj.Value;
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
			sb.Append("MajorLevelId : " + MajorLevelId + ",");
			sb.Append("SubLevelId : " + SubLevelId);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
