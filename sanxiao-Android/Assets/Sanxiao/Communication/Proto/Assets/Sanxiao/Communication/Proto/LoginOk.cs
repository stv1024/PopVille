using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 登录成功。
	/// </summary>
	public class LoginOk : ISendable, IReceiveable
	{
		private bool HasMyUserInfo{get;set;}
		private User myUserInfo;
		/// <summary>
		/// 我的账户信息。
		/// </summary>
		public User MyUserInfo
		{
			get
			{
				return myUserInfo;
			}
			set
			{
				if(value != null)
				{
					HasMyUserInfo = true;
					myUserInfo = value;
				}
			}
		}

		private List<UserSkill> mySkillList;
		/// <summary>
		/// 我拥有的技能列表。
		/// </summary>
		public List<UserSkill> MySkillList
		{
			get
			{
				return mySkillList;
			}
			set
			{
				if(value != null)
				{
					mySkillList = value;
				}
			}
		}

		private List<UserVegetable> myVegetableList;
		/// <summary>
		/// 我的蔬菜列表。
		/// </summary>
		public List<UserVegetable> MyVegetableList
		{
			get
			{
				return myVegetableList;
			}
			set
			{
				if(value != null)
				{
					myVegetableList = value;
				}
			}
		}

		private List<UserCharacter> myCharacterList;
		/// <summary>
		/// 我的角色列表。
		/// </summary>
		public List<UserCharacter> MyCharacterList
		{
			get
			{
				return myCharacterList;
			}
			set
			{
				if(value != null)
				{
					myCharacterList = value;
				}
			}
		}

		private List<UserEquip> myEquipList;
		/// <summary>
		/// 我的装备列表。
		/// </summary>
		public List<UserEquip> MyEquipList
		{
			get
			{
				return myEquipList;
			}
			set
			{
				if(value != null)
				{
					myEquipList = value;
				}
			}
		}

		private List<MajorLevelUnlockInfo> challengeUnlockInfoList;
		/// <summary>
		/// 推图模式的已解锁列表。
		/// </summary>
		public List<MajorLevelUnlockInfo> ChallengeUnlockInfoList
		{
			get
			{
				return challengeUnlockInfoList;
			}
			set
			{
				if(value != null)
				{
					challengeUnlockInfoList = value;
				}
			}
		}

		private bool HasConfigHash{get;set;}
		private ConfigHash configHash;
		/// <summary>
		/// 游戏的配置类。
		/// </summary>
		public ConfigHash ConfigHash
		{
			get
			{
				return configHash;
			}
			set
			{
				if(value != null)
				{
					HasConfigHash = true;
					configHash = value;
				}
			}
		}

		public bool HasFirstTimeGuide{get;private set;}
		private bool firstTimeGuide;
		/// <summary>
		/// 是否需要首次登陆教学。
		/// </summary>
		public bool FirstTimeGuide
		{
			get
			{
				return firstTimeGuide;
			}
			set
			{
				HasFirstTimeGuide = true;
				firstTimeGuide = value;
			}
		}

		private List<SNSFriendUnlockInfo> snsFriendUnlockInfoList;
		/// <summary>
		/// 社交好友的解锁进度。
		/// </summary>
		public List<SNSFriendUnlockInfo> SnsFriendUnlockInfoList
		{
			get
			{
				return snsFriendUnlockInfoList;
			}
			set
			{
				if(value != null)
				{
					snsFriendUnlockInfoList = value;
				}
			}
		}

		public bool HasIsFirstCharge{get;private set;}
		private bool isFirstCharge;
		/// <summary>
		/// 是否可以首冲。
		/// </summary>
		public bool IsFirstCharge
		{
			get
			{
				return isFirstCharge;
			}
			set
			{
				HasIsFirstCharge = true;
				isFirstCharge = value;
			}
		}

		/// <summary>
		/// 登录成功。
		/// </summary>
		public LoginOk()
		{
			MySkillList = new List<UserSkill>();
			MyVegetableList = new List<UserVegetable>();
			MyCharacterList = new List<UserCharacter>();
			MyEquipList = new List<UserEquip>();
			ChallengeUnlockInfoList = new List<MajorLevelUnlockInfo>();
			SnsFriendUnlockInfoList = new List<SNSFriendUnlockInfo>();
		}

		/// <summary>
		/// 登录成功。
		/// </summary>
		public LoginOk
		(
			User myUserInfo,
			ConfigHash configHash
		):this()
		{
			MyUserInfo = myUserInfo;
			ConfigHash = configHash;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,MyUserInfo);
			foreach(UserSkill v in MySkillList)
			{
				writer.Write(2,v);
			}
			foreach(UserVegetable v in MyVegetableList)
			{
				writer.Write(3,v);
			}
			foreach(UserCharacter v in MyCharacterList)
			{
				writer.Write(4,v);
			}
			foreach(UserEquip v in MyEquipList)
			{
				writer.Write(5,v);
			}
			foreach(MajorLevelUnlockInfo v in ChallengeUnlockInfoList)
			{
				writer.Write(20,v);
			}
			writer.Write(30,ConfigHash);
			if(HasFirstTimeGuide)
			{
				writer.Write(40,FirstTimeGuide);
			}
			foreach(SNSFriendUnlockInfo v in SnsFriendUnlockInfoList)
			{
				writer.Write(50,v);
			}
			if(HasIsFirstCharge)
			{
				writer.Write(60,IsFirstCharge);
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
						MyUserInfo = new User();
						MyUserInfo.ParseFrom(obj.Value);
						break;
					case 2:
						 var mySkill= new UserSkill();
						mySkill.ParseFrom(obj.Value);
						MySkillList.Add(mySkill);
						break;
					case 3:
						 var myVegetable= new UserVegetable();
						myVegetable.ParseFrom(obj.Value);
						MyVegetableList.Add(myVegetable);
						break;
					case 4:
						 var myCharacter= new UserCharacter();
						myCharacter.ParseFrom(obj.Value);
						MyCharacterList.Add(myCharacter);
						break;
					case 5:
						 var myEquip= new UserEquip();
						myEquip.ParseFrom(obj.Value);
						MyEquipList.Add(myEquip);
						break;
					case 20:
						 var challengeUnlockInfo= new MajorLevelUnlockInfo();
						challengeUnlockInfo.ParseFrom(obj.Value);
						ChallengeUnlockInfoList.Add(challengeUnlockInfo);
						break;
					case 30:
						ConfigHash = new ConfigHash();
						ConfigHash.ParseFrom(obj.Value);
						break;
					case 40:
						FirstTimeGuide = obj.Value;
						break;
					case 50:
						 var snsFriendUnlockInfo= new SNSFriendUnlockInfo();
						snsFriendUnlockInfo.ParseFrom(obj.Value);
						SnsFriendUnlockInfoList.Add(snsFriendUnlockInfo);
						break;
					case 60:
						IsFirstCharge = obj.Value;
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
			sb.Append("MyUserInfo : " + MyUserInfo + ",");
			sb.Append("MySkillList : [");
			for(int i = 0; i < MySkillList.Count;i ++)
			{
				if(i == MySkillList.Count -1)
				{
					sb.Append(MySkillList[i]);
				}else
				{
					sb.Append(MySkillList[i] + ",");
				}
			}
			sb.Append("],");
			sb.Append("MyVegetableList : [");
			for(int i = 0; i < MyVegetableList.Count;i ++)
			{
				if(i == MyVegetableList.Count -1)
				{
					sb.Append(MyVegetableList[i]);
				}else
				{
					sb.Append(MyVegetableList[i] + ",");
				}
			}
			sb.Append("],");
			sb.Append("MyCharacterList : [");
			for(int i = 0; i < MyCharacterList.Count;i ++)
			{
				if(i == MyCharacterList.Count -1)
				{
					sb.Append(MyCharacterList[i]);
				}else
				{
					sb.Append(MyCharacterList[i] + ",");
				}
			}
			sb.Append("],");
			sb.Append("MyEquipList : [");
			for(int i = 0; i < MyEquipList.Count;i ++)
			{
				if(i == MyEquipList.Count -1)
				{
					sb.Append(MyEquipList[i]);
				}else
				{
					sb.Append(MyEquipList[i] + ",");
				}
			}
			sb.Append("],");
			sb.Append("ChallengeUnlockInfoList : [");
			for(int i = 0; i < ChallengeUnlockInfoList.Count;i ++)
			{
				if(i == ChallengeUnlockInfoList.Count -1)
				{
					sb.Append(ChallengeUnlockInfoList[i]);
				}else
				{
					sb.Append(ChallengeUnlockInfoList[i] + ",");
				}
			}
			sb.Append("],");
			sb.Append("ConfigHash : " + ConfigHash + ",");
			if(HasFirstTimeGuide)
			{
				sb.Append("FirstTimeGuide : " + FirstTimeGuide +",");
			}
			sb.Append("SnsFriendUnlockInfoList : [");
			for(int i = 0; i < SnsFriendUnlockInfoList.Count;i ++)
			{
				if(i == SnsFriendUnlockInfoList.Count -1)
				{
					sb.Append(SnsFriendUnlockInfoList[i]);
				}else
				{
					sb.Append(SnsFriendUnlockInfoList[i] + ",");
				}
			}
			sb.Append("],");
			if(HasIsFirstCharge)
			{
				sb.Append("IsFirstCharge : " + IsFirstCharge);
			}
			sb.Append("}");
			return sb.ToString();
		}
	}
}
