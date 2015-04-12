using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 游戏所有的配置。
	/// </summary>
	public class Config : ISendable, IReceiveable
	{
		public bool HasCoreConfig{get;private set;}
		private CoreConfig coreConfig;
		/// <summary>
		/// 核心配置。
		/// </summary>
		public CoreConfig CoreConfig
		{
			get
			{
				return coreConfig;
			}
			set
			{
				if(value != null)
				{
					HasCoreConfig = true;
					coreConfig = value;
				}
			}
		}

		public bool HasRechargeConfig{get;private set;}
		private RechargeConfig rechargeConfig;
		/// <summary>
		/// 充值配置。
		/// </summary>
		public RechargeConfig RechargeConfig
		{
			get
			{
				return rechargeConfig;
			}
			set
			{
				if(value != null)
				{
					HasRechargeConfig = true;
					rechargeConfig = value;
				}
			}
		}

		public bool HasSkillConfig{get;private set;}
		private SkillConfig skillConfig;
		/// <summary>
		/// 技能配置。
		/// </summary>
		public SkillConfig SkillConfig
		{
			get
			{
				return skillConfig;
			}
			set
			{
				if(value != null)
				{
					HasSkillConfig = true;
					skillConfig = value;
				}
			}
		}

		public bool HasSkillParameterConfig{get;private set;}
		private SkillParameterConfig skillParameterConfig;
		/// <summary>
		/// 技能参数配置。
		/// </summary>
		public SkillParameterConfig SkillParameterConfig
		{
			get
			{
				return skillParameterConfig;
			}
			set
			{
				if(value != null)
				{
					HasSkillParameterConfig = true;
					skillParameterConfig = value;
				}
			}
		}

		public bool HasExchangeConfig{get;private set;}
		private ExchangeConfig exchangeConfig;
		/// <summary>
		/// 兑换包配置。
		/// </summary>
		public ExchangeConfig ExchangeConfig
		{
			get
			{
				return exchangeConfig;
			}
			set
			{
				if(value != null)
				{
					HasExchangeConfig = true;
					exchangeConfig = value;
				}
			}
		}

		public bool HasVegetableConfig{get;private set;}
		private VegetableConfig vegetableConfig;
		/// <summary>
		/// 蔬菜配置。
		/// </summary>
		public VegetableConfig VegetableConfig
		{
			get
			{
				return vegetableConfig;
			}
			set
			{
				if(value != null)
				{
					HasVegetableConfig = true;
					vegetableConfig = value;
				}
			}
		}

		public bool HasChallengeLevelConfig{get;private set;}
		private ChallengeLevelConfig challengeLevelConfig;
		/// <summary>
		/// 推图关卡配置。
		/// </summary>
		public ChallengeLevelConfig ChallengeLevelConfig
		{
			get
			{
				return challengeLevelConfig;
			}
			set
			{
				if(value != null)
				{
					HasChallengeLevelConfig = true;
					challengeLevelConfig = value;
				}
			}
		}

		public bool HasSkillIntroTextConfig{get;private set;}
		private SkillIntroTextConfig skillIntroTextConfig;
		/// <summary>
		/// 技能简介文本。
		/// </summary>
		public SkillIntroTextConfig SkillIntroTextConfig
		{
			get
			{
				return skillIntroTextConfig;
			}
			set
			{
				if(value != null)
				{
					HasSkillIntroTextConfig = true;
					skillIntroTextConfig = value;
				}
			}
		}

		public bool HasSkillLevelDetailTextConfig{get;private set;}
		private SkillLevelDetailTextConfig skillLevelDetailTextConfig;
		/// <summary>
		/// 技能等级详情文本。
		/// </summary>
		public SkillLevelDetailTextConfig SkillLevelDetailTextConfig
		{
			get
			{
				return skillLevelDetailTextConfig;
			}
			set
			{
				if(value != null)
				{
					HasSkillLevelDetailTextConfig = true;
					skillLevelDetailTextConfig = value;
				}
			}
		}

		public bool HasWaitHintTextConfig{get;private set;}
		private WaitHintTextConfig waitHintTextConfig;
		/// <summary>
		/// 等待提示文本。
		/// </summary>
		public WaitHintTextConfig WaitHintTextConfig
		{
			get
			{
				return waitHintTextConfig;
			}
			set
			{
				if(value != null)
				{
					HasWaitHintTextConfig = true;
					waitHintTextConfig = value;
				}
			}
		}

		public bool HasVegetableIntroTextConfig{get;private set;}
		private VegetableIntroTextConfig vegetableIntroTextConfig;
		/// <summary>
		/// 蔬菜介绍文本。
		/// </summary>
		public VegetableIntroTextConfig VegetableIntroTextConfig
		{
			get
			{
				return vegetableIntroTextConfig;
			}
			set
			{
				if(value != null)
				{
					HasVegetableIntroTextConfig = true;
					vegetableIntroTextConfig = value;
				}
			}
		}

		public bool HasCharacterConfig{get;private set;}
		private CharacterConfig characterConfig;
		/// <summary>
		/// 角色配置。
		/// </summary>
		public CharacterConfig CharacterConfig
		{
			get
			{
				return characterConfig;
			}
			set
			{
				if(value != null)
				{
					HasCharacterConfig = true;
					characterConfig = value;
				}
			}
		}

		public bool HasEquipConfig{get;private set;}
		private EquipConfig equipConfig;
		/// <summary>
		/// 装备配置。
		/// </summary>
		public EquipConfig EquipConfig
		{
			get
			{
				return equipConfig;
			}
			set
			{
				if(value != null)
				{
					HasEquipConfig = true;
					equipConfig = value;
				}
			}
		}

		public bool HasMajorLevelIntroTextConfig{get;private set;}
		private MajorLevelIntroTextConfig majorLevelIntroTextConfig;
		/// <summary>
		/// 大关介绍文本。
		/// </summary>
		public MajorLevelIntroTextConfig MajorLevelIntroTextConfig
		{
			get
			{
				return majorLevelIntroTextConfig;
			}
			set
			{
				if(value != null)
				{
					HasMajorLevelIntroTextConfig = true;
					majorLevelIntroTextConfig = value;
				}
			}
		}

		public bool HasEquipIntroTextConfig{get;private set;}
		private EquipIntroTextConfig equipIntroTextConfig;
		/// <summary>
		/// 装备介绍文本。
		/// </summary>
		public EquipIntroTextConfig EquipIntroTextConfig
		{
			get
			{
				return equipIntroTextConfig;
			}
			set
			{
				if(value != null)
				{
					HasEquipIntroTextConfig = true;
					equipIntroTextConfig = value;
				}
			}
		}

		public bool HasCharacterIntroTextConfig{get;private set;}
		private CharacterIntroTextConfig characterIntroTextConfig;
		/// <summary>
		/// 角色介绍文本。
		/// </summary>
		public CharacterIntroTextConfig CharacterIntroTextConfig
		{
			get
			{
				return characterIntroTextConfig;
			}
			set
			{
				if(value != null)
				{
					HasCharacterIntroTextConfig = true;
					characterIntroTextConfig = value;
				}
			}
		}

		public bool HasOauthParamConfig{get;private set;}
		private OAuthParamConfig oauthParamConfig;
		/// <summary>
		/// 社交平台参数配置。
		/// </summary>
		public OAuthParamConfig OauthParamConfig
		{
			get
			{
				return oauthParamConfig;
			}
			set
			{
				if(value != null)
				{
					HasOauthParamConfig = true;
					oauthParamConfig = value;
				}
			}
		}

		/// <summary>
		/// 游戏所有的配置。
		/// </summary>
		public Config()
		{
		}

		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			if(HasCoreConfig)
			{
				writer.Write(1,CoreConfig);
			}
			if(HasRechargeConfig)
			{
				writer.Write(2,RechargeConfig);
			}
			if(HasSkillConfig)
			{
				writer.Write(3,SkillConfig);
			}
			if(HasSkillParameterConfig)
			{
				writer.Write(4,SkillParameterConfig);
			}
			if(HasExchangeConfig)
			{
				writer.Write(5,ExchangeConfig);
			}
			if(HasVegetableConfig)
			{
				writer.Write(6,VegetableConfig);
			}
			if(HasChallengeLevelConfig)
			{
				writer.Write(7,ChallengeLevelConfig);
			}
			if(HasSkillIntroTextConfig)
			{
				writer.Write(8,SkillIntroTextConfig);
			}
			if(HasSkillLevelDetailTextConfig)
			{
				writer.Write(9,SkillLevelDetailTextConfig);
			}
			if(HasWaitHintTextConfig)
			{
				writer.Write(10,WaitHintTextConfig);
			}
			if(HasVegetableIntroTextConfig)
			{
				writer.Write(11,VegetableIntroTextConfig);
			}
			if(HasCharacterConfig)
			{
				writer.Write(12,CharacterConfig);
			}
			if(HasEquipConfig)
			{
				writer.Write(13,EquipConfig);
			}
			if(HasMajorLevelIntroTextConfig)
			{
				writer.Write(14,MajorLevelIntroTextConfig);
			}
			if(HasEquipIntroTextConfig)
			{
				writer.Write(15,EquipIntroTextConfig);
			}
			if(HasCharacterIntroTextConfig)
			{
				writer.Write(16,CharacterIntroTextConfig);
			}
			if(HasOauthParamConfig)
			{
				writer.Write(17,OauthParamConfig);
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
						CoreConfig = new CoreConfig();
						CoreConfig.ParseFrom(obj.Value);
						break;
					case 2:
						RechargeConfig = new RechargeConfig();
						RechargeConfig.ParseFrom(obj.Value);
						break;
					case 3:
						SkillConfig = new SkillConfig();
						SkillConfig.ParseFrom(obj.Value);
						break;
					case 4:
						SkillParameterConfig = new SkillParameterConfig();
						SkillParameterConfig.ParseFrom(obj.Value);
						break;
					case 5:
						ExchangeConfig = new ExchangeConfig();
						ExchangeConfig.ParseFrom(obj.Value);
						break;
					case 6:
						VegetableConfig = new VegetableConfig();
						VegetableConfig.ParseFrom(obj.Value);
						break;
					case 7:
						ChallengeLevelConfig = new ChallengeLevelConfig();
						ChallengeLevelConfig.ParseFrom(obj.Value);
						break;
					case 8:
						SkillIntroTextConfig = new SkillIntroTextConfig();
						SkillIntroTextConfig.ParseFrom(obj.Value);
						break;
					case 9:
						SkillLevelDetailTextConfig = new SkillLevelDetailTextConfig();
						SkillLevelDetailTextConfig.ParseFrom(obj.Value);
						break;
					case 10:
						WaitHintTextConfig = new WaitHintTextConfig();
						WaitHintTextConfig.ParseFrom(obj.Value);
						break;
					case 11:
						VegetableIntroTextConfig = new VegetableIntroTextConfig();
						VegetableIntroTextConfig.ParseFrom(obj.Value);
						break;
					case 12:
						CharacterConfig = new CharacterConfig();
						CharacterConfig.ParseFrom(obj.Value);
						break;
					case 13:
						EquipConfig = new EquipConfig();
						EquipConfig.ParseFrom(obj.Value);
						break;
					case 14:
						MajorLevelIntroTextConfig = new MajorLevelIntroTextConfig();
						MajorLevelIntroTextConfig.ParseFrom(obj.Value);
						break;
					case 15:
						EquipIntroTextConfig = new EquipIntroTextConfig();
						EquipIntroTextConfig.ParseFrom(obj.Value);
						break;
					case 16:
						CharacterIntroTextConfig = new CharacterIntroTextConfig();
						CharacterIntroTextConfig.ParseFrom(obj.Value);
						break;
					case 17:
						OauthParamConfig = new OAuthParamConfig();
						OauthParamConfig.ParseFrom(obj.Value);
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
			if(HasCoreConfig)
			{
				sb.Append("CoreConfig : " + CoreConfig +",");
			}
			if(HasRechargeConfig)
			{
				sb.Append("RechargeConfig : " + RechargeConfig +",");
			}
			if(HasSkillConfig)
			{
				sb.Append("SkillConfig : " + SkillConfig +",");
			}
			if(HasSkillParameterConfig)
			{
				sb.Append("SkillParameterConfig : " + SkillParameterConfig +",");
			}
			if(HasExchangeConfig)
			{
				sb.Append("ExchangeConfig : " + ExchangeConfig +",");
			}
			if(HasVegetableConfig)
			{
				sb.Append("VegetableConfig : " + VegetableConfig +",");
			}
			if(HasChallengeLevelConfig)
			{
				sb.Append("ChallengeLevelConfig : " + ChallengeLevelConfig +",");
			}
			if(HasSkillIntroTextConfig)
			{
				sb.Append("SkillIntroTextConfig : " + SkillIntroTextConfig +",");
			}
			if(HasSkillLevelDetailTextConfig)
			{
				sb.Append("SkillLevelDetailTextConfig : " + SkillLevelDetailTextConfig +",");
			}
			if(HasWaitHintTextConfig)
			{
				sb.Append("WaitHintTextConfig : " + WaitHintTextConfig +",");
			}
			if(HasVegetableIntroTextConfig)
			{
				sb.Append("VegetableIntroTextConfig : " + VegetableIntroTextConfig +",");
			}
			if(HasCharacterConfig)
			{
				sb.Append("CharacterConfig : " + CharacterConfig +",");
			}
			if(HasEquipConfig)
			{
				sb.Append("EquipConfig : " + EquipConfig +",");
			}
			if(HasMajorLevelIntroTextConfig)
			{
				sb.Append("MajorLevelIntroTextConfig : " + MajorLevelIntroTextConfig +",");
			}
			if(HasEquipIntroTextConfig)
			{
				sb.Append("EquipIntroTextConfig : " + EquipIntroTextConfig +",");
			}
			if(HasCharacterIntroTextConfig)
			{
				sb.Append("CharacterIntroTextConfig : " + CharacterIntroTextConfig +",");
			}
			if(HasOauthParamConfig)
			{
				sb.Append("OauthParamConfig : " + OauthParamConfig);
			}
			sb.Append("}");
			return sb.ToString();
		}
	}
}
