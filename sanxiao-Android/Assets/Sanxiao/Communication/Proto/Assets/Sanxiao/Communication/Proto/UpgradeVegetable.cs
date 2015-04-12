using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 升级蔬菜。
	/// </summary>
	public class UpgradeVegetable : ISendable, IReceiveable
	{
		private bool HasVegetableCode{get;set;}
		private int vegetableCode;
		/// <summary>
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

		/// <summary>
		/// 升级蔬菜。
		/// </summary>
		public UpgradeVegetable()
		{
		}

		/// <summary>
		/// 升级蔬菜。
		/// </summary>
		public UpgradeVegetable
		(
			int vegetableCode
		):this()
		{
			VegetableCode = vegetableCode;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,VegetableCode);
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
					default:
						break;
				}
			}
		}
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("{");
			sb.Append("VegetableCode : " + VegetableCode);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
