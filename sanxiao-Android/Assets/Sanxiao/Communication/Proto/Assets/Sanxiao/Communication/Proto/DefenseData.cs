using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 玩家的防御数据。
	/// </summary>
	public class DefenseData : ISendable, IReceiveable
	{
		private bool HasNickname{get;set;}
		private string nickname;
		/// <summary>
		/// 玩家当时的昵称。
		/// </summary>
		public string Nickname
		{
			get
			{
				return nickname;
			}
			set
			{
				if(value != null)
				{
					HasNickname = true;
					nickname = value;
				}
			}
		}

		private bool HasLevel{get;set;}
		private int level;
		/// <summary>
		/// 玩家当时的等级。
		/// </summary>
		public int Level
		{
			get
			{
				return level;
			}
			set
			{
				HasLevel = true;
				level = value;
			}
		}

		private bool HasCharacter{get;set;}
		private int character;
		/// <summary>
		/// 玩家当时使用的角色。
		/// </summary>
		public int Character
		{
			get
			{
				return character;
			}
			set
			{
				HasCharacter = true;
				character = value;
			}
		}

		private List<int> wearEquipList;
		/// <summary>
		/// 角色上使用的装备。
		/// </summary>
		public List<int> WearEquipList
		{
			get
			{
				return wearEquipList;
			}
			set
			{
				if(value != null)
				{
					wearEquipList = value;
				}
			}
		}

		private bool HasRoundInitHealth{get;set;}
		private int roundInitHealth;
		/// <summary>
		/// 玩家当时的血量。
		/// </summary>
		public int RoundInitHealth
		{
			get
			{
				return roundInitHealth;
			}
			set
			{
				HasRoundInitHealth = true;
				roundInitHealth = value;
			}
		}

		private bool HasEnergyCapacity{get;set;}
		private int energyCapacity;
		/// <summary>
		/// 玩家当时的蓄力槽容量。
		/// </summary>
		public int EnergyCapacity
		{
			get
			{
				return energyCapacity;
			}
			set
			{
				HasEnergyCapacity = true;
				energyCapacity = value;
			}
		}

		private List<VegetableUsed> vegetableList;
		/// <summary>
		/// 玩家本局上阵的蔬菜。
		/// </summary>
		public List<VegetableUsed> VegetableList
		{
			get
			{
				return vegetableList;
			}
			set
			{
				if(value != null)
				{
					vegetableList = value;
				}
			}
		}

		private List<UseSkillEvent> skillEventList;
		/// <summary>
		/// 玩家使用的技能序列。
		/// </summary>
		public List<UseSkillEvent> SkillEventList
		{
			get
			{
				return skillEventList;
			}
			set
			{
				if(value != null)
				{
					skillEventList = value;
				}
			}
		}

		/// <summary>
		/// 玩家的防御数据。
		/// </summary>
		public DefenseData()
		{
			WearEquipList = new List<int>();
			VegetableList = new List<VegetableUsed>();
			SkillEventList = new List<UseSkillEvent>();
		}

		/// <summary>
		/// 玩家的防御数据。
		/// </summary>
		public DefenseData
		(
			string nickname,
			int level,
			int character,
			int roundInitHealth,
			int energyCapacity
		):this()
		{
			Nickname = nickname;
			Level = level;
			Character = character;
			RoundInitHealth = roundInitHealth;
			EnergyCapacity = energyCapacity;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,Nickname);
			writer.Write(2,Level);
			writer.Write(3,Character);
			foreach(int v in WearEquipList)
			{
				writer.Write(4,v);
			}
			writer.Write(5,RoundInitHealth);
			writer.Write(6,EnergyCapacity);
			foreach(VegetableUsed v in VegetableList)
			{
				writer.Write(7,v);
			}
			foreach(UseSkillEvent v in SkillEventList)
			{
				writer.Write(10,v);
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
						Nickname = obj.Value;
						break;
					case 2:
						Level = obj.Value;
						break;
					case 3:
						Character = obj.Value;
						break;
					case 4:
						WearEquipList.Add(obj.Value);
						break;
					case 5:
						RoundInitHealth = obj.Value;
						break;
					case 6:
						EnergyCapacity = obj.Value;
						break;
					case 7:
						 var vegetable= new VegetableUsed();
						vegetable.ParseFrom(obj.Value);
						VegetableList.Add(vegetable);
						break;
					case 10:
						 var skillEvent= new UseSkillEvent();
						skillEvent.ParseFrom(obj.Value);
						SkillEventList.Add(skillEvent);
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
			sb.Append("Nickname : " + Nickname + ",");
			sb.Append("Level : " + Level + ",");
			sb.Append("Character : " + Character + ",");
			sb.Append("WearEquipList : [");
			for(int i = 0; i < WearEquipList.Count;i ++)
			{
				if(i == WearEquipList.Count -1)
				{
					sb.Append(WearEquipList[i]);
				}else
				{
					sb.Append(WearEquipList[i] + ",");
				}
			}
			sb.Append("],");
			sb.Append("RoundInitHealth : " + RoundInitHealth + ",");
			sb.Append("EnergyCapacity : " + EnergyCapacity + ",");
			sb.Append("VegetableList : [");
			for(int i = 0; i < VegetableList.Count;i ++)
			{
				if(i == VegetableList.Count -1)
				{
					sb.Append(VegetableList[i]);
				}else
				{
					sb.Append(VegetableList[i] + ",");
				}
			}
			sb.Append("],");
			sb.Append("SkillEventList : [");
			for(int i = 0; i < SkillEventList.Count;i ++)
			{
				if(i == SkillEventList.Count -1)
				{
					sb.Append(SkillEventList[i]);
				}else
				{
					sb.Append(SkillEventList[i] + ",");
				}
			}
			sb.Append("]");
			sb.Append("}");
			return sb.ToString();
		}
	}
}
