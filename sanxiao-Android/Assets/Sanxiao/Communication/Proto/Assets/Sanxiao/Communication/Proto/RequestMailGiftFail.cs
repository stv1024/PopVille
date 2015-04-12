using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 请求礼物失败。
	/// </summary>
	public class RequestMailGiftFail : ISendable, IReceiveable
	{
		private bool HasResult{get;set;}
		private MsgResult result;
		/// <summary>
		/// </summary>
		public MsgResult Result
		{
			get
			{
				return result;
			}
			set
			{
				if(value != null)
				{
					HasResult = true;
					result = value;
				}
			}
		}

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
		/// 申请的礼品列表。。
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
		/// 请求礼物失败。
		/// </summary>
		public RequestMailGiftFail()
		{
			SnList = new List<int>();
		}

		/// <summary>
		/// 请求礼物失败。
		/// </summary>
		public RequestMailGiftFail
		(
			MsgResult result,
			string mailId
		):this()
		{
			Result = result;
			MailId = mailId;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,Result);
			writer.Write(2,MailId);
			foreach(int v in SnList)
			{
				writer.Write(3,v);
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
						Result = new MsgResult();
						Result.ParseFrom(obj.Value);
						break;
					case 2:
						MailId = obj.Value;
						break;
					case 3:
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
			sb.Append("Result : " + Result + ",");
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
