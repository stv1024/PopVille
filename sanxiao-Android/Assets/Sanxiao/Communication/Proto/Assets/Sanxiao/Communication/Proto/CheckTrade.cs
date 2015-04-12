using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 检查订单。
	/// </summary>
	public class CheckTrade : ISendable, IReceiveable
	{
		private bool HasOutTradeNo{get;set;}
		private string outTradeNo;
		/// <summary>
		/// 订单的outTradeNo。
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

		public bool HasTradeNo{get;private set;}
		private string tradeNo;
		/// <summary>
		/// 订单的tradeNo。
		/// </summary>
		public string TradeNo
		{
			get
			{
				return tradeNo;
			}
			set
			{
				if(value != null)
				{
					HasTradeNo = true;
					tradeNo = value;
				}
			}
		}

		public bool HasExtra{get;private set;}
		private string extra;
		/// <summary>
		/// 订单附带的extra信息。
		/// </summary>
		public string Extra
		{
			get
			{
				return extra;
			}
			set
			{
				if(value != null)
				{
					HasExtra = true;
					extra = value;
				}
			}
		}

		/// <summary>
		/// 检查订单。
		/// </summary>
		public CheckTrade()
		{
		}

		/// <summary>
		/// 检查订单。
		/// </summary>
		public CheckTrade
		(
			string outTradeNo
		):this()
		{
			OutTradeNo = outTradeNo;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,OutTradeNo);
			if(HasTradeNo)
			{
				writer.Write(2,TradeNo);
			}
			if(HasExtra)
			{
				writer.Write(3,Extra);
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
						OutTradeNo = obj.Value;
						break;
					case 2:
						TradeNo = obj.Value;
						break;
					case 3:
						Extra = obj.Value;
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
			if(HasTradeNo)
			{
				sb.Append("TradeNo : " + TradeNo +",");
			}
			if(HasExtra)
			{
				sb.Append("Extra : " + Extra);
			}
			sb.Append("}");
			return sb.ToString();
		}
	}
}
