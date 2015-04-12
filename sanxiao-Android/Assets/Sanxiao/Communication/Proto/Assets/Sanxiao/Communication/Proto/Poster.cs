using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 小喇叭。
	/// </summary>
	public class Poster : ISendable, IReceiveable
	{
		private bool HasContent{get;set;}
		private string content;
		/// <summary>
		/// 小喇叭的内容。
		/// </summary>
		public string Content
		{
			get
			{
				return content;
			}
			set
			{
				if(value != null)
				{
					HasContent = true;
					content = value;
				}
			}
		}

		/// <summary>
		/// 小喇叭。
		/// </summary>
		public Poster()
		{
		}

		/// <summary>
		/// 小喇叭。
		/// </summary>
		public Poster
		(
			string content
		):this()
		{
			Content = content;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,Content);
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
						Content = obj.Value;
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
			sb.Append("Content : " + Content);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
