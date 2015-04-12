using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 检查订单失败。
	/// </summary>
	public class CheckTradeFail : ISendable, IReceiveable
	{
		private bool HasOutTradeNo{get;set;}
		private string outTradeNo;
		/// <summary>
		/// </summary>
		public string OutTradeNo
		{
			get
			{
				return outTradeNo;
			}
			set
			{
				if(value != null)
				{
					HasOutTradeNo = true;
					outTradeNo = value;
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
		/// 检查订单失败。
		/// </summary>
		public CheckTradeFail()
		{
		}

		/// <summary>
		/// 检查订单失败。
		/// </summary>
		public CheckTradeFail
		(
			string outTradeNo,
			MsgResult result
		):this()
		{
			OutTradeNo = outTradeNo;
			Result = result;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,OutTradeNo);
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
						OutTradeNo = obj.Value;
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
			sb.Append("OutTradeNo : " + OutTradeNo + ",");
			sb.Append("Result : " + Result);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
