using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 使用技能成功。
	/// </summary>
	public class UseSkillOk : ISendable, IReceiveable
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
		/// 服务器端实际采纳的物理伤害值。
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

		public bool HasKo{get;private set;}
		private bool ko;
		/// <summary>
		/// 是否将对方击败（KO存在，并且为true）。
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
		/// 使用技能成功。
		/// </summary>
		public UseSkillOk()
		{
		}

		/// <summary>
		/// 使用技能成功。
		/// </summary>
		public UseSkillOk
		(
			int skillCode,
			int physicalDamage,
			SyncData syncData
		):this()
		{
			SkillCode = skillCode;
			PhysicalDamage = physicalDamage;
			SyncData = syncData;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,SkillCode);
			writer.Write(2,PhysicalDamage);
			writer.Write(3,SyncData);
			if(HasKo)
			{
				writer.Write(4,Ko);
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
						PhysicalDamage = obj.Value;
						break;
					case 3:
						SyncData = new SyncData();
						SyncData.ParseFrom(obj.Value);
						break;
					case 4:
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
			sb.Append("PhysicalDamage : " + PhysicalDamage + ",");
			sb.Append("SyncData : " + SyncData + ",");
			if(HasKo)
			{
				sb.Append("Ko : " + Ko);
			}
			sb.Append("}");
			return sb.ToString();
		}
	}
}
