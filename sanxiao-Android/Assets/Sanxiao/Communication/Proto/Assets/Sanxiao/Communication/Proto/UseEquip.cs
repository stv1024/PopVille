using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 使用或者卸下装备。
	/// </summary>
	public class UseEquip : ISendable, IReceiveable
	{
		private bool HasCharacterCode{get;set;}
		private int characterCode;
		/// <summary>
		/// 角色代码。
		/// </summary>
		public int CharacterCode
		{
			get
			{
				return characterCode;
			}
			set
			{
				HasCharacterCode = true;
				characterCode = value;
			}
		}

		private bool HasEquipCode{get;set;}
		private int equipCode;
		/// <summary>
		/// 装备代码。
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

		private bool HasUseOrNot{get;set;}
		private bool useOrNot;
		/// <summary>
		/// 使用还是卸下。
		/// </summary>
		public bool UseOrNot
		{
			get
			{
				return useOrNot;
			}
			set
			{
				HasUseOrNot = true;
				useOrNot = value;
			}
		}

		/// <summary>
		/// 使用或者卸下装备。
		/// </summary>
		public UseEquip()
		{
		}

		/// <summary>
		/// 使用或者卸下装备。
		/// </summary>
		public UseEquip
		(
			int characterCode,
			int equipCode,
			bool useOrNot
		):this()
		{
			CharacterCode = characterCode;
			EquipCode = equipCode;
			UseOrNot = useOrNot;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,CharacterCode);
			writer.Write(2,EquipCode);
			writer.Write(3,UseOrNot);
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
						CharacterCode = obj.Value;
						break;
					case 2:
						EquipCode = obj.Value;
						break;
					case 3:
						UseOrNot = obj.Value;
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
			sb.Append("CharacterCode : " + CharacterCode + ",");
			sb.Append("EquipCode : " + EquipCode + ",");
			sb.Append("UseOrNot : " + UseOrNot);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
