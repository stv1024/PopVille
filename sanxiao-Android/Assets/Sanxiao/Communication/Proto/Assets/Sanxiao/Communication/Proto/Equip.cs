using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 装备。
	/// </summary>
	public class Equip : ISendable, IReceiveable
	{
		private bool HasEquipCode{get;set;}
		private int equipCode;
		/// <summary>
		/// 装备的code。
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

		private bool HasType{get;set;}
		private int type;
		/// <summary>
		/// 装备的类型：0：头盔，1：盔甲，2：武器，3：盾，4：鞋子。
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

		private bool HasHealthAdd{get;set;}
		private long healthAdd;
		/// <summary>
		/// 血量加成。
		/// </summary>
		public long HealthAdd
		{
			get
			{
				return healthAdd;
			}
			set
			{
				HasHealthAdd = true;
				healthAdd = value;
			}
		}

		private bool HasAttackAdd{get;set;}
		private long attackAdd;
		/// <summary>
		/// 攻击加成。
		/// </summary>
		public long AttackAdd
		{
			get
			{
				return attackAdd;
			}
			set
			{
				HasAttackAdd = true;
				attackAdd = value;
			}
		}

		private bool HasCriticalStrikeRate{get;set;}
		private int criticalStrikeRate;
		/// <summary>
		/// 暴击概率。
		/// </summary>
		public int CriticalStrikeRate
		{
			get
			{
				return criticalStrikeRate;
			}
			set
			{
				HasCriticalStrikeRate = true;
				criticalStrikeRate = value;
			}
		}

		private bool HasDodgeRate{get;set;}
		private int dodgeRate;
		/// <summary>
		/// 闪避概率。
		/// </summary>
		public int DodgeRate
		{
			get
			{
				return dodgeRate;
			}
			set
			{
				HasDodgeRate = true;
				dodgeRate = value;
			}
		}

		/// <summary>
		/// 装备。
		/// </summary>
		public Equip()
		{
		}

		/// <summary>
		/// 装备。
		/// </summary>
		public Equip
		(
			int equipCode,
			int type,
			long healthAdd,
			long attackAdd,
			int criticalStrikeRate,
			int dodgeRate
		):this()
		{
			EquipCode = equipCode;
			Type = type;
			HealthAdd = healthAdd;
			AttackAdd = attackAdd;
			CriticalStrikeRate = criticalStrikeRate;
			DodgeRate = dodgeRate;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,EquipCode);
			writer.Write(2,Type);
			writer.Write(3,HealthAdd);
			writer.Write(4,AttackAdd);
			writer.Write(5,CriticalStrikeRate);
			writer.Write(6,DodgeRate);
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
						Type = obj.Value;
						break;
					case 3:
						HealthAdd = obj.Value;
						break;
					case 4:
						AttackAdd = obj.Value;
						break;
					case 5:
						CriticalStrikeRate = obj.Value;
						break;
					case 6:
						DodgeRate = obj.Value;
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
			sb.Append("Type : " + Type + ",");
			sb.Append("HealthAdd : " + HealthAdd + ",");
			sb.Append("AttackAdd : " + AttackAdd + ",");
			sb.Append("CriticalStrikeRate : " + CriticalStrikeRate + ",");
			sb.Append("DodgeRate : " + DodgeRate);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
