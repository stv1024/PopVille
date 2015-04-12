using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 蔬菜解锁。
	/// </summary>
	public class VegetableUnlock : ISendable, IReceiveable
	{
		private bool HasVegetableCode{get;set;}
		private int vegetableCode;
		/// <summary>
		/// 蔬菜的id。
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

		private bool HasNewUpgradeLimit{get;set;}
		private int newUpgradeLimit;
		/// <summary>
		/// 新的升级上限。
		/// </summary>
		public int NewUpgradeLimit
		{
			get
			{
				return newUpgradeLimit;
			}
			set
			{
				HasNewUpgradeLimit = true;
				newUpgradeLimit = value;
			}
		}

		/// <summary>
		/// 蔬菜解锁。
		/// </summary>
		public VegetableUnlock()
		{
		}

		/// <summary>
		/// 蔬菜解锁。
		/// </summary>
		public VegetableUnlock
		(
			int vegetableCode,
			int newUpgradeLimit
		):this()
		{
			VegetableCode = vegetableCode;
			NewUpgradeLimit = newUpgradeLimit;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,VegetableCode);
			writer.Write(2,NewUpgradeLimit);
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
						NewUpgradeLimit = obj.Value;
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
			sb.Append("NewUpgradeLimit : " + NewUpgradeLimit);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
