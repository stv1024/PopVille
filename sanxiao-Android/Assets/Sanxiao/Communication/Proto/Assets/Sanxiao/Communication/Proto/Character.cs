using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// ---- 技能 End ----
	/// 角色配置。
	/// </summary>
	public class Character : ISendable, IReceiveable
	{
		private bool HasCharacterCode{get;set;}
		private int characterCode;
		/// <summary>
		/// 角色id。
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

		private bool HasInitEnergy{get;set;}
		private int initEnergy;
		/// <summary>
		/// 角色每局开始时候具有的初始的蓄力值。
		/// </summary>
		public int InitEnergy
		{
			get
			{
				return initEnergy;
			}
			set
			{
				HasInitEnergy = true;
				initEnergy = value;
			}
		}

		public bool HasUnlockCost{get;private set;}
		private Currency unlockCost;
		/// <summary>
		/// 解锁价格。
		/// </summary>
		public Currency UnlockCost
		{
			get
			{
				return unlockCost;
			}
			set
			{
				if(value != null)
				{
					HasUnlockCost = true;
					unlockCost = value;
				}
			}
		}

		/// <summary>
		/// ---- 技能 End ----
		/// 角色配置。
		/// </summary>
		public Character()
		{
		}

		/// <summary>
		/// ---- 技能 End ----
		/// 角色配置。
		/// </summary>
		public Character
		(
			int characterCode,
			int initEnergy
		):this()
		{
			CharacterCode = characterCode;
			InitEnergy = initEnergy;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,CharacterCode);
			writer.Write(2,InitEnergy);
			if(HasUnlockCost)
			{
				writer.Write(3,UnlockCost);
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
						CharacterCode = obj.Value;
						break;
					case 2:
						InitEnergy = obj.Value;
						break;
					case 3:
						UnlockCost = new Currency();
						UnlockCost.ParseFrom(obj.Value);
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
			sb.Append("InitEnergy : " + InitEnergy + ",");
			if(HasUnlockCost)
			{
				sb.Append("UnlockCost : " + UnlockCost);
			}
			sb.Append("}");
			return sb.ToString();
		}
	}
}
