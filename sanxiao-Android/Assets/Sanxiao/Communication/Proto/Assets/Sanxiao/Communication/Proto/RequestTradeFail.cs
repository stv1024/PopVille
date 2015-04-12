using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 请求交易失败。
	/// </summary>
	public class RequestTradeFail : ISendable, IReceiveable
	{
		private bool HasRechargeName{get;set;}
		private string rechargeName;
		/// <summary>
		/// 充值包名称。
		/// </summary>
		public string RechargeName
		{
			get
			{
				return rechargeName;
			}
			set
			{
				if(value != null)
				{
					HasRechargeName = true;
					rechargeName = value;
				}
			}
		}

		private bool HasCount{get;set;}
		private int count;
		/// <summary>
		/// 充值的数量。
		/// </summary>
		public int Count
		{
			get
			{
				return count;
			}
			set
			{
				HasCount = true;
				count = value;
			}
		}

		private bool HasResult{get;set;}
		private MsgResult result;
		/// <summary>
		/// 结果。
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
		/// 请求交易失败。
		/// </summary>
		public RequestTradeFail()
		{
		}

		/// <summary>
		/// 请求交易失败。
		/// </summary>
		public RequestTradeFail
		(
			string rechargeName,
			int count,
			MsgResult result
		):this()
		{
			RechargeName = rechargeName;
			Count = count;
			Result = result;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,RechargeName);
			writer.Write(2,Count);
			writer.Write(3,Result);
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
						RechargeName = obj.Value;
						break;
					case 2:
						Count = obj.Value;
						break;
					case 3:
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
			sb.Append("RechargeName : " + RechargeName + ",");
			sb.Append("Count : " + Count + ",");
			sb.Append("Result : " + Result);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
