using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 玩家的邮件列表。
	/// </summary>
	public class UserMailList : ISendable, IReceiveable
	{
		private List<Mail> mailList;
		/// <summary>
		/// </summary>
		public List<Mail> MailList
		{
			get
			{
				return mailList;
			}
			set
			{
				if(value != null)
				{
					mailList = value;
				}
			}
		}

		/// <summary>
		/// 玩家的邮件列表。
		/// </summary>
		public UserMailList()
		{
			MailList = new List<Mail>();
		}

		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			foreach(Mail v in MailList)
			{
				writer.Write(1,v);
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
						 var mail= new Mail();
						mail.ParseFrom(obj.Value);
						MailList.Add(mail);
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
			sb.Append("MailList : [");
			for(int i = 0; i < MailList.Count;i ++)
			{
				if(i == MailList.Count -1)
				{
					sb.Append(MailList[i]);
				}else
				{
					sb.Append(MailList[i] + ",");
				}
			}
			sb.Append("]");
			sb.Append("}");
			return sb.ToString();
		}
	}
}
