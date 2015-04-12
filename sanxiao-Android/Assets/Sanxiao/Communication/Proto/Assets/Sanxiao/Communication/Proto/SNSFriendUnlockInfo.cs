using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 社交账号好友的解锁信息。
	/// </summary>
	public class SNSFriendUnlockInfo : ISendable, IReceiveable
	{
		private bool HasType{get;set;}
		private int type;
		/// <summary>
		/// 社交账号的类型。
		/// </summary>
		public int Type
		{
			get
			{
				return type;
			}
			set
			{
				HasType = true;
				type = value;
			}
		}

		private bool HasUid{get;set;}
		private string uid;
		/// <summary>
		/// 社交账号的uid。
		/// </summary>
		public string Uid
		{
			get
			{
				return uid;
			}
			set
			{
				if(value != null)
				{
					HasUid = true;
					uid = value;
				}
			}
		}

		private bool HasUserId{get;set;}
		private string userId;
		/// <summary>
		/// 好友的userId。
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

		public bool HasHeadIconUrl{get;private set;}
		private string headIconUrl;
		/// <summary>
		/// 头像url。
		/// </summary>
		public string HeadIconUrl
		{
			get
			{
				return headIconUrl;
			}
			set
			{
				if(value != null)
				{
					HasHeadIconUrl = true;
					headIconUrl = value;
				}
			}
		}

		private bool HasUnlockedMajorLevelId{get;set;}
		private int unlockedMajorLevelId;
		/// <summary>
		/// 好友解锁的大关id。
		/// </summary>
		public int UnlockedMajorLevelId
		{
			get
			{
				return unlockedMajorLevelId;
			}
			set
			{
				HasUnlockedMajorLevelId = true;
				unlockedMajorLevelId = value;
			}
		}

		private bool HasUnlockedSubLevelId{get;set;}
		private int unlockedSubLevelId;
		/// <summary>
		/// 好友解锁的大关的小关id。
		/// </summary>
		public int UnlockedSubLevelId
		{
			get
			{
				return unlockedSubLevelId;
			}
			set
			{
				HasUnlockedSubLevelId = true;
				unlockedSubLevelId = value;
			}
		}

		/// <summary>
		/// 社交账号好友的解锁信息。
		/// </summary>
		public SNSFriendUnlockInfo()
		{
		}

		/// <summary>
		/// 社交账号好友的解锁信息。
		/// </summary>
		public SNSFriendUnlockInfo
		(
			int type,
			string uid,
			string userId,
			string nickname,
			int unlockedMajorLevelId,
			int unlockedSubLevelId
		):this()
		{
			Type = type;
			Uid = uid;
			UserId = userId;
			Nickname = nickname;
			UnlockedMajorLevelId = unlockedMajorLevelId;
			UnlockedSubLevelId = unlockedSubLevelId;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,Type);
			writer.Write(2,Uid);
			writer.Write(3,UserId);
			writer.Write(4,Nickname);
			if(HasHeadIconUrl)
			{
				writer.Write(5,HeadIconUrl);
			}
			writer.Write(6,UnlockedMajorLevelId);
			writer.Write(7,UnlockedSubLevelId);
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
						Type = obj.Value;
						break;
					case 2:
						Uid = obj.Value;
						break;
					case 3:
						UserId = obj.Value;
						break;
					case 4:
						Nickname = obj.Value;
						break;
					case 5:
						HeadIconUrl = obj.Value;
						break;
					case 6:
						UnlockedMajorLevelId = obj.Value;
						break;
					case 7:
						UnlockedSubLevelId = obj.Value;
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
			sb.Append("Type : " + Type + ",");
			sb.Append("Uid : " + Uid + ",");
			sb.Append("UserId : " + UserId + ",");
			sb.Append("Nickname : " + Nickname + ",");
			if(HasHeadIconUrl)
			{
				sb.Append("HeadIconUrl : " + HeadIconUrl +",");
			}
			sb.Append("UnlockedMajorLevelId : " + UnlockedMajorLevelId + ",");
			sb.Append("UnlockedSubLevelId : " + UnlockedSubLevelId);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
