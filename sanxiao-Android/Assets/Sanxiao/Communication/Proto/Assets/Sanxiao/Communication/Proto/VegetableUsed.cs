using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 在对战中使用的蔬菜。
	/// </summary>
	public class VegetableUsed : ISendable, IReceiveable
	{
		private bool HasVegetableCode{get;set;}
		private int vegetableCode;
		/// <summary>
		/// 蔬菜的代码。
		/// </summary>
		public int VegetableCode
		{
			get
			{
				return vegetableCode;
			}
			set
			{
				HasVegetableCode = true;
				vegetableCode = value;
			}
		}

		private bool HasLevel{get;set;}
		private int level;
		/// <summary>
		/// 蔬菜当时的等级。
		/// </summary>
		public int Level
		{
			get
			{
				return level;
			}
			set
			{
				HasLevel = true;
				level = value;
			}
		}

		/// <summary>
		/// 在对战中使用的蔬菜。
		/// </summary>
		public VegetableUsed()
		{
		}

		/// <summary>
		/// 在对战中使用的蔬菜。
		/// </summary>
		public VegetableUsed
		(
			int vegetableCode,
			int level
		):this()
		{
			VegetableCode = vegetableCode;
			Level = level;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,VegetableCode);
			writer.Write(2,Level);
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
						VegetableCode = obj.Value;
						break;
					case 2:
						Level = obj.Value;
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
			sb.Append("VegetableCode : " + VegetableCode + ",");
			sb.Append("Level : " + Level);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
