using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 排行榜数据。
	/// </summary>
	public class LeaderboardItem : ISendable, IReceiveable
	{
		private bool HasGlobalRank{get;set;}
		private long globalRank;
		/// <summary>
		/// 全局排名。
		/// </summary>
		public long GlobalRank
		{
			get
			{
				return globalRank;
			}
			set
			{
				HasGlobalRank = true;
				globalRank = value;
			}
		}

		private bool HasSubRank{get;set;}
		private long subRank;
		/// <summary>
		/// 子榜排名。
		/// </summary>
		public long SubRank
		{
			get
			{
				return subRank;
			}
			set
			{
				HasSubRank = true;
				subRank = value;
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

		private bool HasUserId{get;set;}
		private string userId;
		/// <summary>
		/// 用户的id。
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

		/// <summary>
		/// 排行榜数据。
		/// </summary>
		public LeaderboardItem()
		{
		}

		/// <summary>
		/// 排行榜数据。
		/// </summary>
		public LeaderboardItem
		(
			long globalRank,
			long subRank,
			string nickname,
			string userId
		):this()
		{
			GlobalRank = globalRank;
			SubRank = subRank;
			Nickname = nickname;
			UserId = userId;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,GlobalRank);
			writer.Write(2,SubRank);
			writer.Write(3,Nickname);
			writer.Write(4,UserId);
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
						GlobalRank = obj.Value;
						break;
					case 2:
						SubRank = obj.Value;
						break;
					case 3:
						Nickname = obj.Value;
						break;
					case 4:
						UserId = obj.Value;
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
			sb.Append("GlobalRank : " + GlobalRank + ",");
			sb.Append("SubRank : " + SubRank + ",");
			sb.Append("Nickname : " + Nickname + ",");
			sb.Append("UserId : " + UserId);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
