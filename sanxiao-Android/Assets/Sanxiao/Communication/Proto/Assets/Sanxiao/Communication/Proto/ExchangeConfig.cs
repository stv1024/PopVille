using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 兑换包配置。
	/// </summary>
	public class ExchangeConfig : ISendable, IReceiveable
	{
		private bool HasHash{get;set;}
		private string hash;
		/// <summary>
		/// </summary>
		public string Hash
		{
			get
			{
				return hash;
			}
			set
			{
				if(value != null)
				{
					HasHash = true;
					hash = value;
				}
			}
		}

		private List<Exchange> exchangeList;
		/// <summary>
		/// </summary>
		public List<Exchange> ExchangeList
		{
			get
			{
				return exchangeList;
			}
			set
			{
				if(value != null)
				{
					exchangeList = value;
				}
			}
		}

		/// <summary>
		/// 兑换包配置。
		/// </summary>
		public ExchangeConfig()
		{
			ExchangeList = new List<Exchange>();
		}

		/// <summary>
		/// 兑换包配置。
		/// </summary>
		public ExchangeConfig
		(
			string hash
		):this()
		{
			Hash = hash;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,Hash);
			foreach(Exchange v in ExchangeList)
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
						Hash = obj.Value;
						break;
					case 2:
						 var exchange= new Exchange();
						exchange.ParseFrom(obj.Value);
						ExchangeList.Add(exchange);
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
			sb.Append("Hash : " + Hash + ",");
			sb.Append("ExchangeList : [");
			for(int i = 0; i < ExchangeList.Count;i ++)
			{
				if(i == ExchangeList.Count -1)
				{
					sb.Append(ExchangeList[i]);
				}else
				{
					sb.Append(ExchangeList[i] + ",");
				}
			}
			sb.Append("]");
			sb.Append("}");
			return sb.ToString();
		}
	}
}
