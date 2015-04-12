using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 请求礼品。
	/// </summary>
	public class RequestMailGift : ISendable, IReceiveable
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

		private List<int> snList;
		/// <summary>
		/// 礼物的序列号列表。
		/// </summary>
		public List<int> SnList
		{
			get
			{
				return snList;
			}
			set
			{
				if(value != null)
				{
					snList = value;
				}
			}
		}

		/// <summary>
		/// 请求礼品。
		/// </summary>
		public RequestMailGift()
		{
			SnList = new List<int>();
		}

		/// <summary>
		/// 请求礼品。
		/// </summary>
		public RequestMailGift
		(
			string mailId
		):this()
		{
			MailId = mailId;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,MailId);
			foreach(int v in SnList)
			{
				writer.Write(2,v);
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
						SnList.Add(obj.Value);
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
			sb.Append("SnList : [");
			for(int i = 0; i < SnList.Count;i ++)
			{
				if(i == SnList.Count -1)
				{
					sb.Append(SnList[i]);
				}else
				{
					sb.Append(SnList[i] + ",");
				}
			}
			sb.Append("]");
			sb.Append("}");
			return sb.ToString();
		}
	}
}
