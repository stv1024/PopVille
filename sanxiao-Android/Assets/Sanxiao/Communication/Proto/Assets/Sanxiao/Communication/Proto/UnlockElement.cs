using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 解锁元素。
	/// </summary>
	public class UnlockElement : ISendable, IReceiveable
	{
		public bool HasLevelUp{get;private set;}
		private LevelUp levelUp;
		/// <summary>
		/// 玩家升级。
		/// </summary>
		public LevelUp LevelUp
		{
			get
			{
				return levelUp;
			}
			set
			{
				if(value != null)
				{
					HasLevelUp = true;
					levelUp = value;
				}
			}
		}

		public bool HasEnergyCapacityUp{get;private set;}
		private EnergyCapacityUp energyCapacityUp;
		/// <summary>
		/// 蓄力槽升级。
		/// </summary>
		public EnergyCapacityUp EnergyCapacityUp
		{
			get
			{
				return energyCapacityUp;
			}
			set
			{
				if(value != null)
				{
					HasEnergyCapacityUp = true;
					energyCapacityUp = value;
				}
			}
		}

		private List<VegetableUnlock> vegetableUnlockList;
		/// <summary>
		/// 蔬菜解锁。
		/// </summary>
		public List<VegetableUnlock> VegetableUnlockList
		{
			get
			{
				return vegetableUnlockList;
			}
			set
			{
				if(value != null)
				{
					vegetableUnlockList = value;
				}
			}
		}

		private List<SkillUnlock> skillUnlockList;
		/// <summary>
		/// 技能解锁。
		/// </summary>
		public List<SkillUnlock> SkillUnlockList
		{
			get
			{
				return skillUnlockList;
			}
			set
			{
				if(value != null)
				{
					skillUnlockList = value;
				}
			}
		}

		private List<MajorLevelUnlockInfo> majorLevelUnlockList;
		/// <summary>
		/// 解锁的大关。
		/// </summary>
		public List<MajorLevelUnlockInfo> MajorLevelUnlockList
		{
			get
			{
				return majorLevelUnlockList;
			}
			set
			{
				if(value != null)
				{
					majorLevelUnlockList = value;
				}
			}
		}

		private List<SubLevelUnlockInfo> subLevelUnlockList;
		/// <summary>
		/// 解锁的小关。
		/// </summary>
		public List<SubLevelUnlockInfo> SubLevelUnlockList
		{
			get
			{
				return subLevelUnlockList;
			}
			set
			{
				if(value != null)
				{
					subLevelUnlockList = value;
				}
			}
		}

		public bool HasEquipAppear{get;private set;}
		private EquipAppear equipAppear;
		/// <summary>
		/// 出现装备。
		/// </summary>
		public EquipAppear EquipAppear
		{
			get
			{
				return equipAppear;
			}
			set
			{
				if(value != null)
				{
					HasEquipAppear = true;
					equipAppear = value;
				}
			}
		}

		/// <summary>
		/// 解锁元素。
		/// </summary>
		public UnlockElement()
		{
			VegetableUnlockList = new List<VegetableUnlock>();
			SkillUnlockList = new List<SkillUnlock>();
			MajorLevelUnlockList = new List<MajorLevelUnlockInfo>();
			SubLevelUnlockList = new List<SubLevelUnlockInfo>();
		}

		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			if(HasLevelUp)
			{
				writer.Write(1,LevelUp);
			}
			if(HasEnergyCapacityUp)
			{
				writer.Write(2,EnergyCapacityUp);
			}
			foreach(VegetableUnlock v in VegetableUnlockList)
			{
				writer.Write(3,v);
			}
			foreach(SkillUnlock v in SkillUnlockList)
			{
				writer.Write(4,v);
			}
			foreach(MajorLevelUnlockInfo v in MajorLevelUnlockList)
			{
				writer.Write(5,v);
			}
			foreach(SubLevelUnlockInfo v in SubLevelUnlockList)
			{
				writer.Write(6,v);
			}
			if(HasEquipAppear)
			{
				writer.Write(7,EquipAppear);
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
						LevelUp = new LevelUp();
						LevelUp.ParseFrom(obj.Value);
						break;
					case 2:
						EnergyCapacityUp = new EnergyCapacityUp();
						EnergyCapacityUp.ParseFrom(obj.Value);
						break;
					case 3:
						 var vegetableUnlock= new VegetableUnlock();
						vegetableUnlock.ParseFrom(obj.Value);
						VegetableUnlockList.Add(vegetableUnlock);
						break;
					case 4:
						 var skillUnlock= new SkillUnlock();
						skillUnlock.ParseFrom(obj.Value);
						SkillUnlockList.Add(skillUnlock);
						break;
					case 5:
						 var majorLevelUnlock= new MajorLevelUnlockInfo();
						majorLevelUnlock.ParseFrom(obj.Value);
						MajorLevelUnlockList.Add(majorLevelUnlock);
						break;
					case 6:
						 var subLevelUnlock= new SubLevelUnlockInfo();
						subLevelUnlock.ParseFrom(obj.Value);
						SubLevelUnlockList.Add(subLevelUnlock);
						break;
					case 7:
						EquipAppear = new EquipAppear();
						EquipAppear.ParseFrom(obj.Value);
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
			if(HasLevelUp)
			{
				sb.Append("LevelUp : " + LevelUp +",");
			}
			if(HasEnergyCapacityUp)
			{
				sb.Append("EnergyCapacityUp : " + EnergyCapacityUp +",");
			}
			sb.Append("VegetableUnlockList : [");
			for(int i = 0; i < VegetableUnlockList.Count;i ++)
			{
				if(i == VegetableUnlockList.Count -1)
				{
					sb.Append(VegetableUnlockList[i]);
				}else
				{
					sb.Append(VegetableUnlockList[i] + ",");
				}
			}
			sb.Append("],");
			sb.Append("SkillUnlockList : [");
			for(int i = 0; i < SkillUnlockList.Count;i ++)
			{
				if(i == SkillUnlockList.Count -1)
				{
					sb.Append(SkillUnlockList[i]);
				}else
				{
					sb.Append(SkillUnlockList[i] + ",");
				}
			}
			sb.Append("],");
			sb.Append("MajorLevelUnlockList : [");
			for(int i = 0; i < MajorLevelUnlockList.Count;i ++)
			{
				if(i == MajorLevelUnlockList.Count -1)
				{
					sb.Append(MajorLevelUnlockList[i]);
				}else
				{
					sb.Append(MajorLevelUnlockList[i] + ",");
				}
			}
			sb.Append("],");
			sb.Append("SubLevelUnlockList : [");
			for(int i = 0; i < SubLevelUnlockList.Count;i ++)
			{
				if(i == SubLevelUnlockList.Count -1)
				{
					sb.Append(SubLevelUnlockList[i]);
				}else
				{
					sb.Append(SubLevelUnlockList[i] + ",");
				}
			}
			sb.Append("],");
			if(HasEquipAppear)
			{
				sb.Append("EquipAppear : " + EquipAppear);
			}
			sb.Append("}");
			return sb.ToString();
		}
	}
}
