using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 玩家的技能事件，放在防御数据里。
	/// </summary>
	public class UseSkillEvent : ISendable, IReceiveable
	{
		private bool HasTimeDelta{get;set;}
		private long timeDelta;
		/// <summary>
		/// 与上一个技能使用事件或者开局事件发生的时间间隔。
		/// </summary>
		public long TimeDelta
		{
			get
			{
				return timeDelta;
			}
			set
			{
				HasTimeDelta = true;
				timeDelta = value;
			}
		}

		private bool HasSkillCode{get;set;}
		private int skillCode;
		/// <summary>
		/// 技能的代码。
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

		private bool HasSkillLevel{get;set;}
		private int skillLevel;
		/// <summary>
		/// 技能的等级。
		/// </summary>
		public int SkillLevel
		{
			get
			{
				return skillLevel;
			}
			set
			{
				HasSkillLevel = true;
				skillLevel = value;
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
		/// 玩家的技能事件，放在防御数据里。
		/// </summary>
		public UseSkillEvent()
		{
		}

		/// <summary>
		/// 玩家的技能事件，放在防御数据里。
		/// </summary>
		public UseSkillEvent
		(
			long timeDelta,
			int skillCode,
			int skillLevel,
			int physicalDamage
		):this()
		{
			TimeDelta = timeDelta;
			SkillCode = skillCode;
			SkillLevel = skillLevel;
			PhysicalDamage = physicalDamage;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,TimeDelta);
			writer.Write(2,SkillCode);
			writer.Write(3,SkillLevel);
			writer.Write(4,PhysicalDamage);
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
						TimeDelta = obj.Value;
						break;
					case 2:
						SkillCode = obj.Value;
						break;
					case 3:
						SkillLevel = obj.Value;
						break;
					case 4:
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
			sb.Append("TimeDelta : " + TimeDelta + ",");
			sb.Append("SkillCode : " + SkillCode + ",");
			sb.Append("SkillLevel : " + SkillLevel + ",");
			sb.Append("PhysicalDamage : " + PhysicalDamage);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
