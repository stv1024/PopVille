using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 菜单界面下方显示的滚动消息。
	/// </summary>
	public class News : ISendable, IReceiveable
	{
		private bool HasNewsId{get;set;}
		private string newsId;
		/// <summary>
		/// 新闻的id。
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

		private bool HasContent{get;set;}
		private string content;
		/// <summary>
		/// 新闻的内容。
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
		/// 菜单界面下方显示的滚动消息。
		/// </summary>
		public News()
		{
		}

		/// <summary>
		/// 菜单界面下方显示的滚动消息。
		/// </summary>
		public News
		(
			string newsId,
			string content
		):this()
		{
			NewsId = newsId;
			Content = content;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,NewsId);
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
						NewsId = obj.Value;
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
			sb.Append("NewsId : " + NewsId + ",");
			sb.Append("Content : " + Content);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
