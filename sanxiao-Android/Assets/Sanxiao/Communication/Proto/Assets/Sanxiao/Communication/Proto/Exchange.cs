using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 兑换包。
	/// </summary>
	public class Exchange : ISendable, IReceiveable
	{
		private bool HasName{get;set;}
		private string name;
		/// <summary>
		/// 兑换包名称。
		/// </summary>
		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				if(value != null)
				{
					HasName = true;
					name = value;
				}
			}
		}

		private bool HasFromType{get;set;}
		private int fromType;
		/// <summary>
		/// 源货币类型。
		/// </summary>
		public int FromType
		{
			get
			{
				return fromType;
			}
			set
			{
				HasFromType = true;
				fromType = value;
			}
		}

		private bool HasToType{get;set;}
		private int toType;
		/// <summary>
		/// 目标货币类型。
		/// </summary>
		public int ToType
		{
			get
			{
				return toType;
			}
			set
			{
				HasToType = true;
				toType = value;
			}
		}

		private bool HasFromAmount{get;set;}
		private long fromAmount;
		/// <summary>
		/// 源货币的数量。
		/// </summary>
		public long FromAmount
		{
			get
			{
				return fromAmount;
			}
			set
			{
				HasFromAmount = true;
				fromAmount = value;
			}
		}

		private bool HasToAmount{get;set;}
		private long toAmount;
		/// <summary>
		/// 目标货币的数量。
		/// </summary>
		public long ToAmount
		{
			get
			{
				return toAmount;
			}
			set
			{
				HasToAmount = true;
				toAmount = value;
			}
		}

		/// <summary>
		/// 兑换包。
		/// </summary>
		public Exchange()
		{
		}

		/// <summary>
		/// 兑换包。
		/// </summary>
		public Exchange
		(
			string name,
			int fromType,
			int toType,
			long fromAmount,
			long toAmount
		):this()
		{
			Name = name;
			FromType = fromType;
			ToType = toType;
			FromAmount = fromAmount;
			ToAmount = toAmount;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,Name);
			writer.Write(2,FromType);
			writer.Write(3,ToType);
			writer.Write(4,FromAmount);
			writer.Write(5,ToAmount);
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
						Name = obj.Value;
						break;
					case 2:
						FromType = obj.Value;
						break;
					case 3:
						ToType = obj.Value;
						break;
					case 4:
						FromAmount = obj.Value;
						break;
					case 5:
						ToAmount = obj.Value;
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
			sb.Append("Name : " + Name + ",");
			sb.Append("FromType : " + FromType + ",");
			sb.Append("ToType : " + ToType + ",");
			sb.Append("FromAmount : " + FromAmount + ",");
			sb.Append("ToAmount : " + ToAmount);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
