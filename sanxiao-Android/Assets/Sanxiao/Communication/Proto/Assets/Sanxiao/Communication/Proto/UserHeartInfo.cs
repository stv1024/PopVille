using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 玩家请求自己的疲劳值。
	/// REQUEST_MY_HEART_INFO
	/// 玩家的疲劳值。
	/// </summary>
	public class UserHeartInfo : ISendable, IReceiveable
	{
		private bool HasCount{get;set;}
		private long count;
		/// <summary>
		/// 玩家当前拥有的疲劳值数量。
		/// </summary>
		public long Count
		{
			get
			{
				return count;
			}
			set
			{
				HasCount = true;
				count = value;
			}
		}

		private bool HasNextNeedTime{get;set;}
		private long nextNeedTime;
		/// <summary>
		/// 获得下一个疲劳值需要的时间。单位：毫秒。
		/// 当用户没有下一个疲劳值得时候，next_need_time = -1。
		/// </summary>
		public long NextNeedTime
		{
			get
			{
				return nextNeedTime;
			}
			set
			{
				HasNextNeedTime = true;
				nextNeedTime = value;
			}
		}

		/// <summary>
		/// 玩家请求自己的疲劳值。
		/// REQUEST_MY_HEART_INFO
		/// 玩家的疲劳值。
		/// </summary>
		public UserHeartInfo()
		{
		}

		/// <summary>
		/// 玩家请求自己的疲劳值。
		/// REQUEST_MY_HEART_INFO
		/// 玩家的疲劳值。
		/// </summary>
		public UserHeartInfo
		(
			long count,
			long nextNeedTime
		):this()
		{
			Count = count;
			NextNeedTime = nextNeedTime;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,Count);
			writer.Write(2,NextNeedTime);
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
						Count = obj.Value;
						break;
					case 2:
						NextNeedTime = obj.Value;
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
			sb.Append("Count : " + Count + ",");
			sb.Append("NextNeedTime : " + NextNeedTime);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
