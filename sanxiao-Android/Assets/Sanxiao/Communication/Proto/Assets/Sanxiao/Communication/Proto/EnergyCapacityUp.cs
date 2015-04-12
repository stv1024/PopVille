using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 蓄力槽升级。
	/// </summary>
	public class EnergyCapacityUp : ISendable, IReceiveable
	{
		private bool HasFromLevel{get;set;}
		private int fromLevel;
		/// <summary>
		/// 升级前的等级。
		/// </summary>
		public int FromLevel
		{
			get
			{
				return fromLevel;
			}
			set
			{
				HasFromLevel = true;
				fromLevel = value;
			}
		}

		private bool HasToLevel{get;set;}
		private int toLevel;
		/// <summary>
		/// 升级后的等级。
		/// </summary>
		public int ToLevel
		{
			get
			{
				return toLevel;
			}
			set
			{
				HasToLevel = true;
				toLevel = value;
			}
		}

		private bool HasFromCapacity{get;set;}
		private int fromCapacity;
		/// <summary>
		/// 升级之前的容量。
		/// </summary>
		public int FromCapacity
		{
			get
			{
				return fromCapacity;
			}
			set
			{
				HasFromCapacity = true;
				fromCapacity = value;
			}
		}

		private bool HasToCapacity{get;set;}
		private int toCapacity;
		/// <summary>
		/// 升级后的蓄力槽容量。
		/// </summary>
		public int ToCapacity
		{
			get
			{
				return toCapacity;
			}
			set
			{
				HasToCapacity = true;
				toCapacity = value;
			}
		}

		/// <summary>
		/// 蓄力槽升级。
		/// </summary>
		public EnergyCapacityUp()
		{
		}

		/// <summary>
		/// 蓄力槽升级。
		/// </summary>
		public EnergyCapacityUp
		(
			int fromLevel,
			int toLevel,
			int fromCapacity,
			int toCapacity
		):this()
		{
			FromLevel = fromLevel;
			ToLevel = toLevel;
			FromCapacity = fromCapacity;
			ToCapacity = toCapacity;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,FromLevel);
			writer.Write(2,ToLevel);
			writer.Write(3,FromCapacity);
			writer.Write(4,ToCapacity);
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
						FromLevel = obj.Value;
						break;
					case 2:
						ToLevel = obj.Value;
						break;
					case 3:
						FromCapacity = obj.Value;
						break;
					case 4:
						ToCapacity = obj.Value;
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
			sb.Append("FromLevel : " + FromLevel + ",");
			sb.Append("ToLevel : " + ToLevel + ",");
			sb.Append("FromCapacity : " + FromCapacity + ",");
			sb.Append("ToCapacity : " + ToCapacity);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
