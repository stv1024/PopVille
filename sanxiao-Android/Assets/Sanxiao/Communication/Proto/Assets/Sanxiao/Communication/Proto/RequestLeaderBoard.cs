using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// ---- 排行榜相关命令 Start ----
	/// 请求排行榜数据。
	/// </summary>
	public class RequestLeaderBoard : ISendable, IReceiveable
	{
		private bool HasType{get;set;}
		private int type;
		/// <summary>
		/// 排行榜的类型。
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

		/// <summary>
		/// ---- 排行榜相关命令 Start ----
		/// 请求排行榜数据。
		/// </summary>
		public RequestLeaderBoard()
		{
		}

		/// <summary>
		/// ---- 排行榜相关命令 Start ----
		/// 请求排行榜数据。
		/// </summary>
		public RequestLeaderBoard
		(
			int type
		):this()
		{
			Type = type;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,Type);
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
					default:
						break;
				}
			}
		}
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("{");
			sb.Append("Type : " + Type);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
