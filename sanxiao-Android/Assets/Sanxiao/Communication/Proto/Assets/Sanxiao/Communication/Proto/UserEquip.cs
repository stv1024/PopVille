using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 用户拥有的装备。
	/// </summary>
	public class UserEquip : ISendable, IReceiveable
	{
		private bool HasEquipCode{get;set;}
		private int equipCode;
		/// <summary>
		/// 拥有的装备代码。
		/// </summary>
		public int EquipCode
		{
			get
			{
				return equipCode;
			}
			set
			{
				HasEquipCode = true;
				equipCode = value;
			}
		}

		private bool HasCount{get;set;}
		private int count;
		/// <summary>
		/// 玩家拥有装备的数量。
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

		/// <summary>
		/// 用户拥有的装备。
		/// </summary>
		public UserEquip()
		{
		}

		/// <summary>
		/// 用户拥有的装备。
		/// </summary>
		public UserEquip
		(
			int equipCode,
			int count
		):this()
		{
			EquipCode = equipCode;
			Count = count;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,EquipCode);
			writer.Write(2,Count);
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
						EquipCode = obj.Value;
						break;
					case 2:
						Count = obj.Value;
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
			sb.Append("EquipCode : " + EquipCode + ",");
			sb.Append("Count : " + Count);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
