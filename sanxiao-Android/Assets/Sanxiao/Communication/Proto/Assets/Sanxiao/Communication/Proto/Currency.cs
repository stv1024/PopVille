using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 金钱价格。
	/// </summary>
	public class Currency : ISendable, IReceiveable
	{
		private bool HasType{get;set;}
		private int type;
		/// <summary>
		/// 货币类型。
		/// </summary>
		public int Type
		{
			get
			{
				return type;
			}
			set
			{
				HasType = true;
				type = value;
			}
		}

		private bool HasAmount{get;set;}
		private long amount;
		/// <summary>
		/// 货币数量。
		/// </summary>
		public long Amount
		{
			get
			{
				return amount;
			}
			set
			{
				HasAmount = true;
				amount = value;
			}
		}

		/// <summary>
		/// 金钱价格。
		/// </summary>
		public Currency()
		{
		}

		/// <summary>
		/// 金钱价格。
		/// </summary>
		public Currency
		(
			int type,
			long amount
		):this()
		{
			Type = type;
			Amount = amount;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,Type);
			writer.Write(2,Amount);
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
						Type = obj.Value;
						break;
					case 2:
						Amount = obj.Value;
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
			sb.Append("Type : " + Type + ",");
			sb.Append("Amount : " + Amount);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
