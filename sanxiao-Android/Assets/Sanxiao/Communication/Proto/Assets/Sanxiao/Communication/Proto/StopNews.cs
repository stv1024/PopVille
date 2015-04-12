using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 停止消息。
	/// </summary>
	public class StopNews : ISendable, IReceiveable
	{
		private bool HasNewsId{get;set;}
		private string newsId;
		/// <summary>
		/// </summary>
		public string NewsId
		{
			get
			{
				return newsId;
			}
			set
			{
				if(value != null)
				{
					HasNewsId = true;
					newsId = value;
				}
			}
		}

		/// <summary>
		/// 停止消息。
		/// </summary>
		public StopNews()
		{
		}

		/// <summary>
		/// 停止消息。
		/// </summary>
		public StopNews
		(
			string newsId
		):this()
		{
			NewsId = newsId;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,NewsId);
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
						NewsId = obj.Value;
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
			sb.Append("NewsId : " + NewsId);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
