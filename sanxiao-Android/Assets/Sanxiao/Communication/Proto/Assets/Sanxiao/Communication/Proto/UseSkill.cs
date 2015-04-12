using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 使用技能。
	/// </summary>
	public class UseSkill : ISendable, IReceiveable
	{
		private bool HasSkillCode{get;set;}
		private int skillCode;
		/// <summary>
		/// 使用的技能代码。
		/// </summary>
		public int SkillCode
		{
			get
			{
				return skillCode;
			}
			set
			{
				HasSkillCode = true;
				skillCode = value;
			}
		}

		private bool HasPhysicalDamage{get;set;}
		private int physicalDamage;
		/// <summary>
		/// 技能造成的物理伤害。
		/// </summary>
		public int PhysicalDamage
		{
			get
			{
				return physicalDamage;
			}
			set
			{
				HasPhysicalDamage = true;
				physicalDamage = value;
			}
		}

		/// <summary>
		/// 使用技能。
		/// </summary>
		public UseSkill()
		{
		}

		/// <summary>
		/// 使用技能。
		/// </summary>
		public UseSkill
		(
			int skillCode,
			int physicalDamage
		):this()
		{
			SkillCode = skillCode;
			PhysicalDamage = physicalDamage;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,SkillCode);
			writer.Write(2,PhysicalDamage);
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
						SkillCode = obj.Value;
						break;
					case 2:
						PhysicalDamage = obj.Value;
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
			sb.Append("SkillCode : " + SkillCode + ",");
			sb.Append("PhysicalDamage : " + PhysicalDamage);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
