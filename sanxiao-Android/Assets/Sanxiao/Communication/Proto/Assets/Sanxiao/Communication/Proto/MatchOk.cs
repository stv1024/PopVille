using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 新匹配一局。
	/// NewMatch
	/// 取消匹配。
	/// CancelMatch
	/// 再来一局。
	/// ReMatch
	/// 匹配结果。
	/// </summary>
	public class MatchOk : ISendable, IReceiveable
	{
		private bool HasRivalInfo{get;set;}
		private User rivalInfo;
		/// <summary>
		/// 匹配到的对手的信息。
		/// </summary>
		public User RivalInfo
		{
			get
			{
				return rivalInfo;
			}
			set
			{
				if(value != null)
				{
					HasRivalInfo = true;
					rivalInfo = value;
				}
			}
		}

		private bool HasStartSeconds{get;set;}
		private int startSeconds;
		/// <summary>
		/// 多少秒之后自动开始一局。
		/// </summary>
		public int StartSeconds
		{
			get
			{
				return startSeconds;
			}
			set
			{
				HasStartSeconds = true;
				startSeconds = value;
			}
		}

		/// <summary>
		/// 新匹配一局。
		/// NewMatch
		/// 取消匹配。
		/// CancelMatch
		/// 再来一局。
		/// ReMatch
		/// 匹配结果。
		/// </summary>
		public MatchOk()
		{
		}

		/// <summary>
		/// 新匹配一局。
		/// NewMatch
		/// 取消匹配。
		/// CancelMatch
		/// 再来一局。
		/// ReMatch
		/// 匹配结果。
		/// </summary>
		public MatchOk
		(
			User rivalInfo,
			int startSeconds
		):this()
		{
			RivalInfo = rivalInfo;
			StartSeconds = startSeconds;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,RivalInfo);
			writer.Write(2,StartSeconds);
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
						RivalInfo = new User();
						RivalInfo.ParseFrom(obj.Value);
						break;
					case 2:
						StartSeconds = obj.Value;
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
			sb.Append("RivalInfo : " + RivalInfo + ",");
			sb.Append("StartSeconds : " + StartSeconds);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
