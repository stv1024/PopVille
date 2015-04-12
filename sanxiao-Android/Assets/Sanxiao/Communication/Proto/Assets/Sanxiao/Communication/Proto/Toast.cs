using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 提示。
	/// </summary>
	public class Toast : ISendable, IReceiveable
	{
		private bool HasType{get;set;}
		private int type;
		/// <summary>
		/// 提示的类型。
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

		private bool HasContent{get;set;}
		private string content;
		/// <summary>
		/// 提示的内容。
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
		/// 提示。
		/// </summary>
		public Toast()
		{
		}

		/// <summary>
		/// 提示。
		/// </summary>
		public Toast
		(
			int type,
			string content
		):this()
		{
			Type = type;
			Content = content;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,Type);
			writer.Write(2,Content);
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
			sb.Append("Type : " + Type + ",");
			sb.Append("Content : " + Content);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
