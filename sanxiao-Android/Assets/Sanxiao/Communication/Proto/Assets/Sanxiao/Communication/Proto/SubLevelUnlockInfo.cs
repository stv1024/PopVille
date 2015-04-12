using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 玩家解锁小关的信息。
	/// </summary>
	public class SubLevelUnlockInfo : ISendable, IReceiveable
	{
		private bool HasMajorLevelId{get;set;}
		private int majorLevelId;
		/// <summary>
		/// 大关id。
		/// </summary>
		public int MajorLevelId
		{
			get
			{
				return majorLevelId;
			}
			set
			{
				HasMajorLevelId = true;
				majorLevelId = value;
			}
		}

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

		private bool HasCurrentStar{get;set;}
		private int currentStar;
		/// <summary>
		/// 小关获得的星星数。
		/// </summary>
		public int CurrentStar
		{
			get
			{
				return currentStar;
			}
			set
			{
				HasCurrentStar = true;
				currentStar = value;
			}
		}

		private bool HasUnlocked{get;set;}
		private bool unlocked;
		/// <summary>
		/// 是否解锁。
		/// </summary>
		public bool Unlocked
		{
			get
			{
				return unlocked;
			}
			set
			{
				HasUnlocked = true;
				unlocked = value;
			}
		}

		/// <summary>
		/// 玩家解锁小关的信息。
		/// </summary>
		public SubLevelUnlockInfo()
		{
		}

		/// <summary>
		/// 玩家解锁小关的信息。
		/// </summary>
		public SubLevelUnlockInfo
		(
			int majorLevelId,
			int subLevelId,
			int currentStar,
			bool unlocked
		):this()
		{
			MajorLevelId = majorLevelId;
			SubLevelId = subLevelId;
			CurrentStar = currentStar;
			Unlocked = unlocked;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,MajorLevelId);
			writer.Write(2,SubLevelId);
			writer.Write(3,CurrentStar);
			writer.Write(4,Unlocked);
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
						MajorLevelId = obj.Value;
						break;
					case 2:
						SubLevelId = obj.Value;
						break;
					case 3:
						CurrentStar = obj.Value;
						break;
					case 4:
						Unlocked = obj.Value;
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
			sb.Append("MajorLevelId : " + MajorLevelId + ",");
			sb.Append("SubLevelId : " + SubLevelId + ",");
			sb.Append("CurrentStar : " + CurrentStar + ",");
			sb.Append("Unlocked : " + Unlocked);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
