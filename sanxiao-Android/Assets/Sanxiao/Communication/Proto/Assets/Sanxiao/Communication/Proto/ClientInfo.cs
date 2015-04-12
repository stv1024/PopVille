using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 客户端向服务器发送的版本信息。
	/// </summary>
	public class ClientInfo : ISendable, IReceiveable
	{
		private bool HasClientVersion{get;set;}
		private int clientVersion;
		/// <summary>
		/// 客户端当前的版本号。
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

		private bool HasSaleChannel{get;set;}
		private string saleChannel;
		/// <summary>
		/// 客户端的渠道。
		/// </summary>
		public string SaleChannel
		{
			get
			{
				return saleChannel;
			}
			set
			{
				if(value != null)
				{
					HasSaleChannel = true;
					saleChannel = value;
				}
			}
		}

		private bool HasOs{get;set;}
		private string os;
		/// <summary>
		/// 客户端的操作系统。(ios android)
		/// </summary>
		public string Os
		{
			get
			{
				return os;
			}
			set
			{
				if(value != null)
				{
					HasOs = true;
					os = value;
				}
			}
		}

		/// <summary>
		/// 客户端向服务器发送的版本信息。
		/// </summary>
		public ClientInfo()
		{
		}

		/// <summary>
		/// 客户端向服务器发送的版本信息。
		/// </summary>
		public ClientInfo
		(
			int clientVersion,
			string saleChannel,
			string os
		):this()
		{
			ClientVersion = clientVersion;
			SaleChannel = saleChannel;
			Os = os;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,ClientVersion);
			writer.Write(2,SaleChannel);
			writer.Write(3,Os);
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
						ClientVersion = obj.Value;
						break;
					case 2:
						SaleChannel = obj.Value;
						break;
					case 3:
						Os = obj.Value;
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
			sb.Append("ClientVersion : " + ClientVersion + ",");
			sb.Append("SaleChannel : " + SaleChannel + ",");
			sb.Append("Os : " + Os);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
