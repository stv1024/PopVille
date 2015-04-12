using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 用户。
	/// </summary>
	public class User : ISendable, IReceiveable
	{
		private bool HasUserId{get;set;}
		private string userId;
		/// <summary>
		/// // 简版信息 ---------------->
		/// 用户的唯一ID(数据库中的uuid)。
		/// </summary>
		public string UserId
		{
			get
			{
				return userId;
			}
			set
			{
				if(value != null)
				{
					HasUserId = true;
					userId = value;
				}
			}
		}

		private bool HasNickname{get;set;}
		private string nickname;
		/// <summary>
		/// 昵称。
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
		/// 用户当前的等级。
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

		private bool HasCharacterCode{get;set;}
		private int characterCode;
		/// <summary>
		/// 玩家当前使用的角色代码。
		/// </summary>
		public int CharacterCode
		{
			get
			{
				return characterCode;
			}
			set
			{
				HasCharacterCode = true;
				characterCode = value;
			}
		}

		private bool HasRoundInitHealth{get;set;}
		private int roundInitHealth;
		/// <summary>
		/// 每局开始时候拥有的血量。
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
		/// 玩家的蓄力槽容量。
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

		private bool HasRoundCount{get;set;}
		private long roundCount;
		/// <summary>
		/// 一共玩的局数。
		/// </summary>
		public long RoundCount
		{
			get
			{
				return roundCount;
			}
			set
			{
				HasRoundCount = true;
				roundCount = value;
			}
		}

		public bool HasExp{get;private set;}
		private long exp;
		/// <summary>
		/// // 全版信息 ---------------->
		/// 用户的经验值。
		/// </summary>
		public long Exp
		{
			get
			{
				return exp;
			}
			set
			{
				HasExp = true;
				exp = value;
			}
		}

		public bool HasExpFloor{get;private set;}
		private long expFloor;
		/// <summary>
		/// 用户当前经验值下限。
		/// </summary>
		public long ExpFloor
		{
			get
			{
				return expFloor;
			}
			set
			{
				HasExpFloor = true;
				expFloor = value;
			}
		}

		public bool HasExpCeil{get;private set;}
		private long expCeil;
		/// <summary>
		/// 用户当前经验值上限。
		/// </summary>
		public long ExpCeil
		{
			get
			{
				return expCeil;
			}
			set
			{
				HasExpCeil = true;
				expCeil = value;
			}
		}

		public bool HasMoney10{get;private set;}
		private long money10;
		/// <summary>
		/// 游戏内第一货币。
		/// </summary>
		public long Money10
		{
			get
			{
				return money10;
			}
			set
			{
				HasMoney10 = true;
				money10 = value;
			}
		}

		public bool HasMoney1{get;private set;}
		private long money1;
		/// <summary>
		/// 游戏内第二货币。
		/// </summary>
		public long Money1
		{
			get
			{
				return money1;
			}
			set
			{
				HasMoney1 = true;
				money1 = value;
			}
		}

		/// <summary>
		/// 用户。
		/// </summary>
		public User()
		{
		}

		/// <summary>
		/// 用户。
		/// </summary>
		public User
		(
			string userId,
			string nickname,
			int level,
			int characterCode,
			int roundInitHealth,
			int energyCapacity,
			long roundCount
		):this()
		{
			UserId = userId;
			Nickname = nickname;
			Level = level;
			CharacterCode = characterCode;
			RoundInitHealth = roundInitHealth;
			EnergyCapacity = energyCapacity;
			RoundCount = roundCount;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,UserId);
			writer.Write(2,Nickname);
			writer.Write(3,Level);
			writer.Write(11,CharacterCode);
			writer.Write(12,RoundInitHealth);
			writer.Write(13,EnergyCapacity);
			writer.Write(21,RoundCount);
			if(HasExp)
			{
				writer.Write(31,Exp);
			}
			if(HasExpFloor)
			{
				writer.Write(32,ExpFloor);
			}
			if(HasExpCeil)
			{
				writer.Write(33,ExpCeil);
			}
			if(HasMoney10)
			{
				writer.Write(41,Money10);
			}
			if(HasMoney1)
			{
				writer.Write(42,Money1);
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
						UserId = obj.Value;
						break;
					case 2:
						Nickname = obj.Value;
						break;
					case 3:
						Level = obj.Value;
						break;
					case 11:
						CharacterCode = obj.Value;
						break;
					case 12:
						RoundInitHealth = obj.Value;
						break;
					case 13:
						EnergyCapacity = obj.Value;
						break;
					case 21:
						RoundCount = obj.Value;
						break;
					case 31:
						Exp = obj.Value;
						break;
					case 32:
						ExpFloor = obj.Value;
						break;
					case 33:
						ExpCeil = obj.Value;
						break;
					case 41:
						Money10 = obj.Value;
						break;
					case 42:
						Money1 = obj.Value;
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
			sb.Append("UserId : " + UserId + ",");
			sb.Append("Nickname : " + Nickname + ",");
			sb.Append("Level : " + Level + ",");
			sb.Append("CharacterCode : " + CharacterCode + ",");
			sb.Append("RoundInitHealth : " + RoundInitHealth + ",");
			sb.Append("EnergyCapacity : " + EnergyCapacity + ",");
			sb.Append("RoundCount : " + RoundCount + ",");
			if(HasExp)
			{
				sb.Append("Exp : " + Exp +",");
			}
			if(HasExpFloor)
			{
				sb.Append("ExpFloor : " + ExpFloor +",");
			}
			if(HasExpCeil)
			{
				sb.Append("ExpCeil : " + ExpCeil +",");
			}
			if(HasMoney10)
			{
				sb.Append("Money10 : " + Money10 +",");
			}
			if(HasMoney1)
			{
				sb.Append("Money1 : " + Money1);
			}
			sb.Append("}");
			return sb.ToString();
		}
	}
}
