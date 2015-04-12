using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 社交账号的好友信息。
	/// </summary>
	public class SNSFriendInfo : ISendable, IReceiveable
	{
		private bool HasUid{get;set;}
		private string uid;
		/// <summary>
		/// 社交账号uid。
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
		/// 玩家的userId。
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

		private bool HasSex{get;set;}
		private int sex;
		/// <summary>
		/// 性别。
		/// </summary>
		public int Sex
		{
			get
			{
				return sex;
			}
			set
			{
				HasSex = true;
				sex = value;
			}
		}

		private bool HasNickname{get;set;}
		private string nickname;
		/// <summary>
		/// 玩家的昵称。
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
		/// 好友头像的url。
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

		private bool HasLeftTeamCount{get;set;}
		private int leftTeamCount;
		/// <summary>
		/// 剩余的可组队的次数。
		/// </summary>
		public int LeftTeamCount
		{
			get
			{
				return leftTeamCount;
			}
			set
			{
				HasLeftTeamCount = true;
				leftTeamCount = value;
			}
		}

		/// <summary>
		/// 社交账号的好友信息。
		/// </summary>
		public SNSFriendInfo()
		{
		}

		/// <summary>
		/// 社交账号的好友信息。
		/// </summary>
		public SNSFriendInfo
		(
			string uid,
			string userId,
			int sex,
			string nickname,
			int leftTeamCount
		):this()
		{
			Uid = uid;
			UserId = userId;
			Sex = sex;
			Nickname = nickname;
			LeftTeamCount = leftTeamCount;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,Uid);
			writer.Write(2,UserId);
			writer.Write(3,Sex);
			writer.Write(4,Nickname);
			if(HasHeadIconUrl)
			{
				writer.Write(5,HeadIconUrl);
			}
			writer.Write(6,LeftTeamCount);
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
						Uid = obj.Value;
						break;
					case 2:
						UserId = obj.Value;
						break;
					case 3:
						Sex = obj.Value;
						break;
					case 4:
						Nickname = obj.Value;
						break;
					case 5:
						HeadIconUrl = obj.Value;
						break;
					case 6:
						LeftTeamCount = obj.Value;
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
			sb.Append("Uid : " + Uid + ",");
			sb.Append("UserId : " + UserId + ",");
			sb.Append("Sex : " + Sex + ",");
			sb.Append("Nickname : " + Nickname + ",");
			if(HasHeadIconUrl)
			{
				sb.Append("HeadIconUrl : " + HeadIconUrl +",");
			}
			sb.Append("LeftTeamCount : " + LeftTeamCount);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
