using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// ---- 技能 Start ----
	/// 技能数据。
	/// </summary>
	public class Skill : ISendable, IReceiveable
	{
		private bool HasSkillCode{get;set;}
		private int skillCode;
		/// <summary>
		/// 技能代码。
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

		private List<int> useCostList;
		/// <summary>
		/// 使用一次需要消耗的蓄力值。
		/// </summary>
		public List<int> UseCostList
		{
			get
			{
				return useCostList;
			}
			set
			{
				if(value != null)
				{
					useCostList = value;
				}
			}
		}

		private List<int> unlockLevelList;
		/// <summary>
		/// 解锁技能所需要的玩家等级。
		/// </summary>
		public List<int> UnlockLevelList
		{
			get
			{
				return unlockLevelList;
			}
			set
			{
				if(value != null)
				{
					unlockLevelList = value;
				}
			}
		}

		private List<Currency> upgradeCostList;
		/// <summary>
		/// 升级价格列表。一共有多少级以升级价格列表的个数来确定。
		/// </summary>
		public List<Currency> UpgradeCostList
		{
			get
			{
				return upgradeCostList;
			}
			set
			{
				if(value != null)
				{
					upgradeCostList = value;
				}
			}
		}

		private List<int> physicalDamageList;
		/// <summary>
		/// 每一级的物理伤害列表。
		/// </summary>
		public List<int> PhysicalDamageList
		{
			get
			{
				return physicalDamageList;
			}
			set
			{
				if(value != null)
				{
					physicalDamageList = value;
				}
			}
		}

		public bool HasExtra{get;private set;}
		private string extra;
		/// <summary>
		/// 技能的附加属性。
		/// </summary>
		public string Extra
		{
			get
			{
				return extra;
			}
			set
			{
				if(value != null)
				{
					HasExtra = true;
					extra = value;
				}
			}
		}

		/// <summary>
		/// ---- 技能 Start ----
		/// 技能数据。
		/// </summary>
		public Skill()
		{
			UseCostList = new List<int>();
			UnlockLevelList = new List<int>();
			UpgradeCostList = new List<Currency>();
			PhysicalDamageList = new List<int>();
		}

		/// <summary>
		/// ---- 技能 Start ----
		/// 技能数据。
		/// </summary>
		public Skill
		(
			int skillCode
		):this()
		{
			SkillCode = skillCode;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,SkillCode);
			foreach(int v in UseCostList)
			{
				writer.Write(2,v);
			}
			foreach(int v in UnlockLevelList)
			{
				writer.Write(3,v);
			}
			foreach(Currency v in UpgradeCostList)
			{
				writer.Write(4,v);
			}
			foreach(int v in PhysicalDamageList)
			{
				writer.Write(5,v);
			}
			if(HasExtra)
			{
				writer.Write(10,Extra);
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
						UseCostList.Add(obj.Value);
						break;
					case 3:
						UnlockLevelList.Add(obj.Value);
						break;
					case 4:
						 var upgradeCost= new Currency();
						upgradeCost.ParseFrom(obj.Value);
						UpgradeCostList.Add(upgradeCost);
						break;
					case 5:
						PhysicalDamageList.Add(obj.Value);
						break;
					case 10:
						Extra = obj.Value;
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
			sb.Append("UseCostList : [");
			for(int i = 0; i < UseCostList.Count;i ++)
			{
				if(i == UseCostList.Count -1)
				{
					sb.Append(UseCostList[i]);
				}else
				{
					sb.Append(UseCostList[i] + ",");
				}
			}
			sb.Append("],");
			sb.Append("UnlockLevelList : [");
			for(int i = 0; i < UnlockLevelList.Count;i ++)
			{
				if(i == UnlockLevelList.Count -1)
				{
					sb.Append(UnlockLevelList[i]);
				}else
				{
					sb.Append(UnlockLevelList[i] + ",");
				}
			}
			sb.Append("],");
			sb.Append("UpgradeCostList : [");
			for(int i = 0; i < UpgradeCostList.Count;i ++)
			{
				if(i == UpgradeCostList.Count -1)
				{
					sb.Append(UpgradeCostList[i]);
				}else
				{
					sb.Append(UpgradeCostList[i] + ",");
				}
			}
			sb.Append("],");
			sb.Append("PhysicalDamageList : [");
			for(int i = 0; i < PhysicalDamageList.Count;i ++)
			{
				if(i == PhysicalDamageList.Count -1)
				{
					sb.Append(PhysicalDamageList[i]);
				}else
				{
					sb.Append(PhysicalDamageList[i] + ",");
				}
			}
			sb.Append("],");
			if(HasExtra)
			{
				sb.Append("Extra : " + Extra);
			}
			sb.Append("}");
			return sb.ToString();
		}
	}
}
