using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 邮件。
	/// </summary>
	public class Mail : ISendable, IReceiveable
	{
		private bool HasMailId{get;set;}
		private string mailId;
		/// <summary>
		/// 邮件的id。
		/// </summary>
		public string MailId
		{
			get
			{
				return mailId;
			}
			set
			{
				if(value != null)
				{
					HasMailId = true;
					mailId = value;
				}
			}
		}

		private bool HasType{get;set;}
		private int type;
		/// <summary>
		/// 邮件的类型。
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

		private bool HasFromUserId{get;set;}
		private string fromUserId;
		/// <summary>
		/// 发件人的id。如果是系统邮件则为："system"
		/// </summary>
		public string FromUserId
		{
			get
			{
				return fromUserId;
			}
			set
			{
				if(value != null)
				{
					HasFromUserId = true;
					fromUserId = value;
				}
			}
		}

		private bool HasFromNickname{get;set;}
		private string fromNickname;
		/// <summary>
		/// 发件人的昵称。
		/// </summary>
		public string FromNickname
		{
			get
			{
				return fromNickname;
			}
			set
			{
				if(value != null)
				{
					HasFromNickname = true;
					fromNickname = value;
				}
			}
		}

		public bool HasTitle{get;private set;}
		private string title;
		/// <summary>
		/// 标题。
		/// </summary>
		public string Title
		{
			get
			{
				return title;
			}
			set
			{
				if(value != null)
				{
					HasTitle = true;
					title = value;
				}
			}
		}

		public bool HasContent{get;private set;}
		private string content;
		/// <summary>
		/// 内容。
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

		private bool HasIsRead{get;set;}
		private bool isRead;
		/// <summary>
		/// 是否已读。
		/// </summary>
		public bool IsRead
		{
			get
			{
				return isRead;
			}
			set
			{
				HasIsRead = true;
				isRead = value;
			}
		}

		private bool HasTimestamp{get;set;}
		private long timestamp;
		/// <summary>
		/// 邮件的创建时间。
		/// </summary>
		public long Timestamp
		{
			get
			{
				return timestamp;
			}
			set
			{
				HasTimestamp = true;
				timestamp = value;
			}
		}

		private List<Gift> giftList;
		/// <summary>
		/// 邮件附带的礼品列表。
		/// </summary>
		public List<Gift> GiftList
		{
			get
			{
				return giftList;
			}
			set
			{
				if(value != null)
				{
					giftList = value;
				}
			}
		}

		/// <summary>
		/// 邮件。
		/// </summary>
		public Mail()
		{
			GiftList = new List<Gift>();
		}

		/// <summary>
		/// 邮件。
		/// </summary>
		public Mail
		(
			string mailId,
			int type,
			string fromUserId,
			string fromNickname,
			bool isRead,
			long timestamp
		):this()
		{
			MailId = mailId;
			Type = type;
			FromUserId = fromUserId;
			FromNickname = fromNickname;
			IsRead = isRead;
			Timestamp = timestamp;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,MailId);
			writer.Write(2,Type);
			writer.Write(3,FromUserId);
			writer.Write(4,FromNickname);
			if(HasTitle)
			{
				writer.Write(5,Title);
			}
			if(HasContent)
			{
				writer.Write(6,Content);
			}
			writer.Write(7,IsRead);
			writer.Write(8,Timestamp);
			foreach(Gift v in GiftList)
			{
				writer.Write(9,v);
			}
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
						MailId = obj.Value;
						break;
					case 2:
						Type = obj.Value;
						break;
					case 3:
						FromUserId = obj.Value;
						break;
					case 4:
						FromNickname = obj.Value;
						break;
					case 5:
						Title = obj.Value;
						break;
					case 6:
						Content = obj.Value;
						break;
					case 7:
						IsRead = obj.Value;
						break;
					case 8:
						Timestamp = obj.Value;
						break;
					case 9:
						 var gift= new Gift();
						gift.ParseFrom(obj.Value);
						GiftList.Add(gift);
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
			sb.Append("MailId : " + MailId + ",");
			sb.Append("Type : " + Type + ",");
			sb.Append("FromUserId : " + FromUserId + ",");
			sb.Append("FromNickname : " + FromNickname + ",");
			if(HasTitle)
			{
				sb.Append("Title : " + Title +",");
			}
			if(HasContent)
			{
				sb.Append("Content : " + Content +",");
			}
			sb.Append("IsRead : " + IsRead + ",");
			sb.Append("Timestamp : " + Timestamp + ",");
			sb.Append("GiftList : [");
			for(int i = 0; i < GiftList.Count;i ++)
			{
				if(i == GiftList.Count -1)
				{
					sb.Append(GiftList[i]);
				}else
				{
					sb.Append(GiftList[i] + ",");
				}
			}
			sb.Append("]");
			sb.Append("}");
			return sb.ToString();
		}
	}
}
