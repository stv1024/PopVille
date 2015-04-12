using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 充值包。
	/// </summary>
	public class Recharge : ISendable, IReceiveable
	{
		private bool HasName{get;set;}
		private string name;
		/// <summary>
		/// 充值包的名称。
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

		private bool HasPrice{get;set;}
		private float price;
		/// <summary>
		/// 充值包的价格。
		/// </summary>
		public float Price
		{
			get
			{
				return price;
			}
			set
			{
				HasPrice = true;
				price = value;
			}
		}

		private bool HasTargetType{get;set;}
		private int targetType;
		/// <summary>
		/// 购买的目标货币类型。
		/// </summary>
		public int TargetType
		{
			get
			{
				return targetType;
			}
			set
			{
				HasTargetType = true;
				targetType = value;
			}
		}

		private bool HasTargetAmount{get;set;}
		private int targetAmount;
		/// <summary>
		/// 购买的目标货币数量。
		/// </summary>
		public int TargetAmount
		{
			get
			{
				return targetAmount;
			}
			set
			{
				HasTargetAmount = true;
				targetAmount = value;
			}
		}

		public bool HasExtra{get;private set;}
		private string extra;
		/// <summary>
		/// 附加属性，通常是iap的productId，移动的计费代码。
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
		/// 充值包。
		/// </summary>
		public Recharge()
		{
		}

		/// <summary>
		/// 充值包。
		/// </summary>
		public Recharge
		(
			string name,
			float price,
			int targetType,
			int targetAmount
		):this()
		{
			Name = name;
			Price = price;
			TargetType = targetType;
			TargetAmount = targetAmount;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,Name);
			writer.Write(2,Price);
			writer.Write(4,TargetType);
			writer.Write(5,TargetAmount);
			if(HasExtra)
			{
				writer.Write(6,Extra);
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
						Name = obj.Value;
						break;
					case 2:
						Price = obj.Value;
						break;
					case 4:
						TargetType = obj.Value;
						break;
					case 5:
						TargetAmount = obj.Value;
						break;
					case 6:
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
			sb.Append("Name : " + Name + ",");
			sb.Append("Price : " + Price + ",");
			sb.Append("TargetType : " + TargetType + ",");
			sb.Append("TargetAmount : " + TargetAmount + ",");
			if(HasExtra)
			{
				sb.Append("Extra : " + Extra);
			}
			sb.Append("}");
			return sb.ToString();
		}
	}
}
