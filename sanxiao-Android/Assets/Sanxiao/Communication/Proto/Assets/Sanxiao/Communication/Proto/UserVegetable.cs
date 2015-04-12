using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 玩家的蔬菜数据。
	/// </summary>
	public class UserVegetable : ISendable, IReceiveable
	{
		private bool HasVegetableCode{get;set;}
		private int vegetableCode;
		/// <summary>
		/// 蔬菜的id。
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

		private bool HasCurrentUpgradeLimit{get;set;}
		private int currentUpgradeLimit;
		/// <summary>
		/// 蔬菜当前的升级上限。
		/// </summary>
		public int CurrentUpgradeLimit
		{
			get
			{
				return currentUpgradeLimit;
			}
			set
			{
				HasCurrentUpgradeLimit = true;
				currentUpgradeLimit = value;
			}
		}

		private bool HasCurrentLevel{get;set;}
		private int currentLevel;
		/// <summary>
		/// 当前的等级。
		/// </summary>
		public int CurrentLevel
		{
			get
			{
				return currentLevel;
			}
			set
			{
				HasCurrentLevel = true;
				currentLevel = value;
			}
		}

		public bool HasMatureTime{get;private set;}
		private long matureTime;
		/// <summary>
		/// 如果蔬菜在升级中，则mature_time是升级完成所需要的时间。单位：毫秒。
		/// </summary>
		public long MatureTime
		{
			get
			{
				return matureTime;
			}
			set
			{
				HasMatureTime = true;
				matureTime = value;
			}
		}

		/// <summary>
		/// 玩家的蔬菜数据。
		/// </summary>
		public UserVegetable()
		{
		}

		/// <summary>
		/// 玩家的蔬菜数据。
		/// </summary>
		public UserVegetable
		(
			int vegetableCode,
			int currentUpgradeLimit,
			int currentLevel
		):this()
		{
			VegetableCode = vegetableCode;
			CurrentUpgradeLimit = currentUpgradeLimit;
			CurrentLevel = currentLevel;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,VegetableCode);
			writer.Write(2,CurrentUpgradeLimit);
			writer.Write(3,CurrentLevel);
			if(HasMatureTime)
			{
				writer.Write(4,MatureTime);
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
						CurrentUpgradeLimit = obj.Value;
						break;
					case 3:
						CurrentLevel = obj.Value;
						break;
					case 4:
						MatureTime = obj.Value;
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
			sb.Append("CurrentUpgradeLimit : " + CurrentUpgradeLimit + ",");
			sb.Append("CurrentLevel : " + CurrentLevel + ",");
			if(HasMatureTime)
			{
				sb.Append("MatureTime : " + MatureTime);
			}
			sb.Append("}");
			return sb.ToString();
		}
	}
}
