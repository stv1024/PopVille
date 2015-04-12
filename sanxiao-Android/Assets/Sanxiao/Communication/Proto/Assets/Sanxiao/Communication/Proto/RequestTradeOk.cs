using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 请求交易成功。
	/// </summary>
	public class RequestTradeOk : ISendable, IReceiveable
	{
		private bool HasRechargeName{get;set;}
		private string rechargeName;
		/// <summary>
		/// 充值包的名称。
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

		private bool HasOutTradeNo{get;set;}
		private string outTradeNo;
		/// <summary>
		/// 生成的交易订单号。
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

		public bool HasExtra{get;private set;}
		private string extra;
		/// <summary>
		/// 以json的形式存放支付所需的额外参数
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
		/// 请求交易成功。
		/// </summary>
		public RequestTradeOk()
		{
		}

		/// <summary>
		/// 请求交易成功。
		/// </summary>
		public RequestTradeOk
		(
			string rechargeName,
			int count,
			string outTradeNo
		):this()
		{
			RechargeName = rechargeName;
			Count = count;
			OutTradeNo = outTradeNo;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,RechargeName);
			writer.Write(2,Count);
			writer.Write(3,OutTradeNo);
			if(HasExtra)
			{
				writer.Write(4,Extra);
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
						RechargeName = obj.Value;
						break;
					case 2:
						Count = obj.Value;
						break;
					case 3:
						OutTradeNo = obj.Value;
						break;
					case 4:
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
			sb.Append("RechargeName : " + RechargeName + ",");
			sb.Append("Count : " + Count + ",");
			sb.Append("OutTradeNo : " + OutTradeNo + ",");
			if(HasExtra)
			{
				sb.Append("Extra : " + Extra);
			}
			sb.Append("}");
			return sb.ToString();
		}
	}
}
