using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 对手使用技能。
	/// </summary>
	public class RivalUseSkill : ISendable, IReceiveable
	{
		private bool HasSkillCode{get;set;}
		private int skillCode;
		/// <summary>
		/// 对手使用的技能代码。
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

		private bool HasSyncData{get;set;}
		private SyncData syncData;
		/// <summary>
		/// 当前的同步数据。
		/// </summary>
		public SyncData SyncData
		{
			get
			{
				return syncData;
			}
			set
			{
				if(value != null)
				{
					HasSyncData = true;
					syncData = value;
				}
			}
		}

		private bool HasPhysicalDamage{get;set;}
		private int physicalDamage;
		/// <summary>
		/// 对方此技能造成的物理伤害。
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

		public bool HasKo{get;private set;}
		private bool ko;
		/// <summary>
		/// 是否击败对手。
		/// </summary>
		public bool Ko
		{
			get
			{
				return ko;
			}
			set
			{
				HasKo = true;
				ko = value;
			}
		}

		/// <summary>
		/// 对手使用技能。
		/// </summary>
		public RivalUseSkill()
		{
		}

		/// <summary>
		/// 对手使用技能。
		/// </summary>
		public RivalUseSkill
		(
			int skillCode,
			int skillLevel,
			SyncData syncData,
			int physicalDamage
		):this()
		{
			SkillCode = skillCode;
			SkillLevel = skillLevel;
			SyncData = syncData;
			PhysicalDamage = physicalDamage;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,SkillCode);
			writer.Write(2,SkillLevel);
			writer.Write(3,SyncData);
			writer.Write(4,PhysicalDamage);
			if(HasKo)
			{
				writer.Write(5,Ko);
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
						SkillCode = obj.Value;
						break;
					case 2:
						SkillLevel = obj.Value;
						break;
					case 3:
						SyncData = new SyncData();
						SyncData.ParseFrom(obj.Value);
						break;
					case 4:
						PhysicalDamage = obj.Value;
						break;
					case 5:
						Ko = obj.Value;
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
			sb.Append("SkillLevel : " + SkillLevel + ",");
			sb.Append("SyncData : " + SyncData + ",");
			sb.Append("PhysicalDamage : " + PhysicalDamage + ",");
			if(HasKo)
			{
				sb.Append("Ko : " + Ko);
			}
			sb.Append("}");
			return sb.ToString();
		}
	}
}
