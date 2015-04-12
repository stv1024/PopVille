using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 蔬菜数据。
	/// </summary>
	public class Vegetable : ISendable, IReceiveable
	{
		private bool HasVegetableCode{get;set;}
		private int vegetableCode;
		/// <summary>
		/// 蔬菜的代码。
		/// </summary>
		public int VegetableCode
		{
			get
			{
				return vegetableCode;
			}
			set
			{
				HasVegetableCode = true;
				vegetableCode = value;
			}
		}

		private bool HasUpgradeLimit{get;set;}
		private int upgradeLimit;
		/// <summary>
		/// 蔬菜的升级上限。
		/// </summary>
		public int UpgradeLimit
		{
			get
			{
				return upgradeLimit;
			}
			set
			{
				HasUpgradeLimit = true;
				upgradeLimit = value;
			}
		}

		private List<int> levelEnergyList;
		/// <summary>
		/// 不同级别蔬菜的蓄力值列表。
		/// </summary>
		public List<int> LevelEnergyList
		{
			get
			{
				return levelEnergyList;
			}
			set
			{
				if(value != null)
				{
					levelEnergyList = value;
				}
			}
		}

		private List<Currency> upgradeCostList;
		/// <summary>
		/// 升级所需要的金币。
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

		/// <summary>
		/// 蔬菜数据。
		/// </summary>
		public Vegetable()
		{
			LevelEnergyList = new List<int>();
			UpgradeCostList = new List<Currency>();
		}

		/// <summary>
		/// 蔬菜数据。
		/// </summary>
		public Vegetable
		(
			int vegetableCode,
			int upgradeLimit
		):this()
		{
			VegetableCode = vegetableCode;
			UpgradeLimit = upgradeLimit;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,VegetableCode);
			writer.Write(2,UpgradeLimit);
			foreach(int v in LevelEnergyList)
			{
				writer.Write(3,v);
			}
			foreach(Currency v in UpgradeCostList)
			{
				writer.Write(4,v);
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
						VegetableCode = obj.Value;
						break;
					case 2:
						UpgradeLimit = obj.Value;
						break;
					case 3:
						LevelEnergyList.Add(obj.Value);
						break;
					case 4:
						 var upgradeCost= new Currency();
						upgradeCost.ParseFrom(obj.Value);
						UpgradeCostList.Add(upgradeCost);
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
			sb.Append("VegetableCode : " + VegetableCode + ",");
			sb.Append("UpgradeLimit : " + UpgradeLimit + ",");
			sb.Append("LevelEnergyList : [");
			for(int i = 0; i < LevelEnergyList.Count;i ++)
			{
				if(i == LevelEnergyList.Count -1)
				{
					sb.Append(LevelEnergyList[i]);
				}else
				{
					sb.Append(LevelEnergyList[i] + ",");
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
			sb.Append("]");
			sb.Append("}");
			return sb.ToString();
		}
	}
}
