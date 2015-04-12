using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 请求生成交易。
	/// </summary>
	public class RequestTrade : ISendable, IReceiveable
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

		public bool HasReceiverUserId{get;private set;}
		private string receiverUserId;
		/// <summary>
		/// 如果是代买，则添加一个receiver的user_id。
		/// </summary>
		public string ReceiverUserId
		{
			get
			{
				return receiverUserId;
			}
			set
			{
				if(value != null)
				{
					HasReceiverUserId = true;
					receiverUserId = value;
				}
			}
		}

		private bool HasPayChannel{get;set;}
		private int payChannel;
		/// <summary>
		/// 支付渠道。
		/// </summary>
		public int PayChannel
		{
			get
			{
				return payChannel;
			}
			set
			{
				HasPayChannel = true;
				payChannel = value;
			}
		}

		public bool HasClientVersion{get;private set;}
		private int clientVersion;
		/// <summary>
		/// 客户端版本号。
		/// </summary>
		public int ClientVersion
		{
			get
			{
				return clientVersion;
			}
			set
			{
				HasClientVersion = true;
				clientVersion = value;
			}
		}

		/// <summary>
		/// 请求生成交易。
		/// </summary>
		public RequestTrade()
		{
		}

		/// <summary>
		/// 请求生成交易。
		/// </summary>
		public RequestTrade
		(
			string rechargeName,
			int count,
			int payChannel
		):this()
		{
			RechargeName = rechargeName;
			Count = count;
			PayChannel = payChannel;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,RechargeName);
			writer.Write(2,Count);
			if(HasReceiverUserId)
			{
				writer.Write(3,ReceiverUserId);
			}
			writer.Write(4,PayChannel);
			if(HasClientVersion)
			{
				writer.Write(5,ClientVersion);
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
						ReceiverUserId = obj.Value;
						break;
					case 4:
						PayChannel = obj.Value;
						break;
					case 5:
						ClientVersion = obj.Value;
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
			if(HasReceiverUserId)
			{
				sb.Append("ReceiverUserId : " + ReceiverUserId +",");
			}
			sb.Append("PayChannel : " + PayChannel + ",");
			if(HasClientVersion)
			{
				sb.Append("ClientVersion : " + ClientVersion);
			}
			sb.Append("}");
			return sb.ToString();
		}
	}
}
