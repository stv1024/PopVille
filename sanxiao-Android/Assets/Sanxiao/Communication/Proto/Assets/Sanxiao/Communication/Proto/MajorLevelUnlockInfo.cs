using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// ---- 推图相关配置 Start ----
	/// 玩家解锁大关卡的信息。
	/// </summary>
	public class MajorLevelUnlockInfo : ISendable, IReceiveable
	{
		private bool HasMajorLevelId{get;set;}
		private int majorLevelId;
		/// <summary>
		/// 大关的id。
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

		private List<SubLevelUnlockInfo> subLevelUnlockInfoList;
		/// <summary>
		/// 本大关中的小关解锁的信息。
		/// </summary>
		public List<SubLevelUnlockInfo> SubLevelUnlockInfoList
		{
			get
			{
				return subLevelUnlockInfoList;
			}
			set
			{
				if(value != null)
				{
					subLevelUnlockInfoList = value;
				}
			}
		}

		private bool HasUnlocked{get;set;}
		private bool unlocked;
		/// <summary>
		/// 是否已经解锁。
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
		/// ---- 推图相关配置 Start ----
		/// 玩家解锁大关卡的信息。
		/// </summary>
		public MajorLevelUnlockInfo()
		{
			SubLevelUnlockInfoList = new List<SubLevelUnlockInfo>();
		}

		/// <summary>
		/// ---- 推图相关配置 Start ----
		/// 玩家解锁大关卡的信息。
		/// </summary>
		public MajorLevelUnlockInfo
		(
			int majorLevelId,
			bool unlocked
		):this()
		{
			MajorLevelId = majorLevelId;
			Unlocked = unlocked;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,MajorLevelId);
			foreach(SubLevelUnlockInfo v in SubLevelUnlockInfoList)
			{
				writer.Write(2,v);
			}
			writer.Write(3,Unlocked);
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
						 var subLevelUnlockInfo= new SubLevelUnlockInfo();
						subLevelUnlockInfo.ParseFrom(obj.Value);
						SubLevelUnlockInfoList.Add(subLevelUnlockInfo);
						break;
					case 3:
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
			sb.Append("SubLevelUnlockInfoList : [");
			for(int i = 0; i < SubLevelUnlockInfoList.Count;i ++)
			{
				if(i == SubLevelUnlockInfoList.Count -1)
				{
					sb.Append(SubLevelUnlockInfoList[i]);
				}else
				{
					sb.Append(SubLevelUnlockInfoList[i] + ",");
				}
			}
			sb.Append("],");
			sb.Append("Unlocked : " + Unlocked);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
