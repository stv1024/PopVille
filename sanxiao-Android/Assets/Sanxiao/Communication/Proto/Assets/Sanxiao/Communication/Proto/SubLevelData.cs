using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 小关的配置。
	/// </summary>
	public class SubLevelData : ISendable, IReceiveable
	{
		private bool HasSubLevelId{get;set;}
		private int subLevelId;
		/// <summary>
		/// 小关的id。
		/// </summary>
		public int SubLevelId
		{
			get
			{
				return subLevelId;
			}
			set
			{
				HasSubLevelId = true;
				subLevelId = value;
			}
		}

		private bool HasTitle{get;set;}
		private string title;
		/// <summary>
		/// 标题。
		/// </summary>
		public string Title
		{
			get
			{
				return title;
			}
			set
			{
				if(value != null)
				{
					HasTitle = true;
					title = value;
				}
			}
		}

		public bool HasDescription{get;private set;}
		private string description;
		/// <summary>
		/// 描述。
		/// </summary>
		public string Description
		{
			get
			{
				return description;
			}
			set
			{
				if(value != null)
				{
					HasDescription = true;
					description = value;
				}
			}
		}

		private bool HasCanTeam{get;set;}
		private bool canTeam;
		/// <summary>
		/// 是否可以组队打副本。
		/// </summary>
		public bool CanTeam
		{
			get
			{
				return canTeam;
			}
			set
			{
				HasCanTeam = true;
				canTeam = value;
			}
		}

		private bool HasWidth{get;set;}
		private int width;
		/// <summary>
		/// 关卡的宽度。
		/// </summary>
		public int Width
		{
			get
			{
				return width;
			}
			set
			{
				HasWidth = true;
				width = value;
			}
		}

		private bool HasHeight{get;set;}
		private int height;
		/// <summary>
		/// 关卡的长度。
		/// </summary>
		public int Height
		{
			get
			{
				return height;
			}
			set
			{
				HasHeight = true;
				height = value;
			}
		}

		private List<int> gridConfigList;
		/// <summary>
		/// 关卡的格子配置。
		/// </summary>
		public List<int> GridConfigList
		{
			get
			{
				return gridConfigList;
			}
			set
			{
				if(value != null)
				{
					gridConfigList = value;
				}
			}
		}

		public bool HasCoinBagProbability{get;private set;}
		private int coinBagProbability;
		/// <summary>
		/// 关卡中掉落金币袋子的概率。例：60代表10000个糖果中会出现60个金币。
		/// </summary>
		public int CoinBagProbability
		{
			get
			{
				return coinBagProbability;
			}
			set
			{
				HasCoinBagProbability = true;
				coinBagProbability = value;
			}
		}

		public bool HasCoinBagCapacity{get;private set;}
		private int coinBagCapacity;
		/// <summary>
		/// 每个金币袋子包含的金币数量。
		/// </summary>
		public int CoinBagCapacity
		{
			get
			{
				return coinBagCapacity;
			}
			set
			{
				HasCoinBagCapacity = true;
				coinBagCapacity = value;
			}
		}

		public bool HasDiamondBagProbability{get;private set;}
		private int diamondBagProbability;
		/// <summary>
		/// 关卡中掉落钻石的概率。例：60代表10000个糖果中会出现60个钻石。
		/// </summary>
		public int DiamondBagProbability
		{
			get
			{
				return diamondBagProbability;
			}
			set
			{
				HasDiamondBagProbability = true;
				diamondBagProbability = value;
			}
		}

		public bool HasDiamondBagCapacity{get;private set;}
		private int diamondBagCapacity;
		/// <summary>
		/// 每个钻石袋子包含的钻石数量。
		/// </summary>
		public int DiamondBagCapacity
		{
			get
			{
				return diamondBagCapacity;
			}
			set
			{
				HasDiamondBagCapacity = true;
				diamondBagCapacity = value;
			}
		}

		public bool HasHealthBottleProbability{get;private set;}
		private int healthBottleProbability;
		/// <summary>
		/// 关卡中掉落血瓶的概率。例：60代表10000个糖果中会出现60个血瓶。
		/// </summary>
		public int HealthBottleProbability
		{
			get
			{
				return healthBottleProbability;
			}
			set
			{
				HasHealthBottleProbability = true;
				healthBottleProbability = value;
			}
		}

		public bool HasHealthBottleCapacity{get;private set;}
		private int healthBottleCapacity;
		/// <summary>
		/// 血瓶中包含的血值。
		/// </summary>
		public int HealthBottleCapacity
		{
			get
			{
				return healthBottleCapacity;
			}
			set
			{
				HasHealthBottleCapacity = true;
				healthBottleCapacity = value;
			}
		}

		public bool HasEnergyBottleProbability{get;private set;}
		private int energyBottleProbability;
		/// <summary>
		/// 关卡中掉落能量瓶的概率。例：60代表10000个糖果中会出现60个能量瓶。
		/// </summary>
		public int EnergyBottleProbability
		{
			get
			{
				return energyBottleProbability;
			}
			set
			{
				HasEnergyBottleProbability = true;
				energyBottleProbability = value;
			}
		}

		public bool HasEnergyBottleCapacity{get;private set;}
		private int energyBottleCapacity;
		/// <summary>
		/// 能量瓶中包含的能量值。
		/// </summary>
		public int EnergyBottleCapacity
		{
			get
			{
				return energyBottleCapacity;
			}
			set
			{
				HasEnergyBottleCapacity = true;
				energyBottleCapacity = value;
			}
		}

		/// <summary>
		/// 小关的配置。
		/// </summary>
		public SubLevelData()
		{
			GridConfigList = new List<int>();
		}

		/// <summary>
		/// 小关的配置。
		/// </summary>
		public SubLevelData
		(
			int subLevelId,
			string title,
			bool canTeam,
			int width,
			int height
		):this()
		{
			SubLevelId = subLevelId;
			Title = title;
			CanTeam = canTeam;
			Width = width;
			Height = height;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,SubLevelId);
			writer.Write(2,Title);
			if(HasDescription)
			{
				writer.Write(3,Description);
			}
			writer.Write(4,CanTeam);
			writer.Write(5,Width);
			writer.Write(6,Height);
			foreach(int v in GridConfigList)
			{
				writer.Write(7,v);
			}
			if(HasCoinBagProbability)
			{
				writer.Write(10,CoinBagProbability);
			}
			if(HasCoinBagCapacity)
			{
				writer.Write(11,CoinBagCapacity);
			}
			if(HasDiamondBagProbability)
			{
				writer.Write(12,DiamondBagProbability);
			}
			if(HasDiamondBagCapacity)
			{
				writer.Write(13,DiamondBagCapacity);
			}
			if(HasHealthBottleProbability)
			{
				writer.Write(14,HealthBottleProbability);
			}
			if(HasHealthBottleCapacity)
			{
				writer.Write(15,HealthBottleCapacity);
			}
			if(HasEnergyBottleProbability)
			{
				writer.Write(16,EnergyBottleProbability);
			}
			if(HasEnergyBottleCapacity)
			{
				writer.Write(17,EnergyBottleCapacity);
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
						SubLevelId = obj.Value;
						break;
					case 2:
						Title = obj.Value;
						break;
					case 3:
						Description = obj.Value;
						break;
					case 4:
						CanTeam = obj.Value;
						break;
					case 5:
						Width = obj.Value;
						break;
					case 6:
						Height = obj.Value;
						break;
					case 7:
						GridConfigList.Add(obj.Value);
						break;
					case 10:
						CoinBagProbability = obj.Value;
						break;
					case 11:
						CoinBagCapacity = obj.Value;
						break;
					case 12:
						DiamondBagProbability = obj.Value;
						break;
					case 13:
						DiamondBagCapacity = obj.Value;
						break;
					case 14:
						HealthBottleProbability = obj.Value;
						break;
					case 15:
						HealthBottleCapacity = obj.Value;
						break;
					case 16:
						EnergyBottleProbability = obj.Value;
						break;
					case 17:
						EnergyBottleCapacity = obj.Value;
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
			sb.Append("SubLevelId : " + SubLevelId + ",");
			sb.Append("Title : " + Title + ",");
			if(HasDescription)
			{
				sb.Append("Description : " + Description +",");
			}
			sb.Append("CanTeam : " + CanTeam + ",");
			sb.Append("Width : " + Width + ",");
			sb.Append("Height : " + Height + ",");
			sb.Append("GridConfigList : [");
			for(int i = 0; i < GridConfigList.Count;i ++)
			{
				if(i == GridConfigList.Count -1)
				{
					sb.Append(GridConfigList[i]);
				}else
				{
					sb.Append(GridConfigList[i] + ",");
				}
			}
			sb.Append("],");
			if(HasCoinBagProbability)
			{
				sb.Append("CoinBagProbability : " + CoinBagProbability +",");
			}
			if(HasCoinBagCapacity)
			{
				sb.Append("CoinBagCapacity : " + CoinBagCapacity +",");
			}
			if(HasDiamondBagProbability)
			{
				sb.Append("DiamondBagProbability : " + DiamondBagProbability +",");
			}
			if(HasDiamondBagCapacity)
			{
				sb.Append("DiamondBagCapacity : " + DiamondBagCapacity +",");
			}
			if(HasHealthBottleProbability)
			{
				sb.Append("HealthBottleProbability : " + HealthBottleProbability +",");
			}
			if(HasHealthBottleCapacity)
			{
				sb.Append("HealthBottleCapacity : " + HealthBottleCapacity +",");
			}
			if(HasEnergyBottleProbability)
			{
				sb.Append("EnergyBottleProbability : " + EnergyBottleProbability +",");
			}
			if(HasEnergyBottleCapacity)
			{
				sb.Append("EnergyBottleCapacity : " + EnergyBottleCapacity);
			}
			sb.Append("}");
			return sb.ToString();
		}
	}
}
