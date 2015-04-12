using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 绑定用户推送信息失败。
	/// </summary>
	public class BindUserPushInfoFail : ISendable, IReceiveable
	{
		public bool HasApnsFail{get;private set;}
		private bool apnsFail;
		/// <summary>
		/// </summary>
		public bool ApnsFail
		{
			get
			{
				return apnsFail;
			}
			set
			{
				HasApnsFail = true;
				apnsFail = value;
			}
		}

		public bool HasBaiduFail{get;private set;}
		private bool baiduFail;
		/// <summary>
		/// </summary>
		public bool BaiduFail
		{
			get
			{
				return baiduFail;
			}
			set
			{
				HasBaiduFail = true;
				baiduFail = value;
			}
		}

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

		/// <summary>
		/// 绑定用户推送信息失败。
		/// </summary>
		public BindUserPushInfoFail()
		{
		}

		/// <summary>
		/// 绑定用户推送信息失败。
		/// </summary>
		public BindUserPushInfoFail
		(
			MsgResult result
		):this()
		{
			Result = result;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			if(HasApnsFail)
			{
				writer.Write(1,ApnsFail);
			}
			if(HasBaiduFail)
			{
				writer.Write(2,BaiduFail);
			}
			writer.Write(3,Result);
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
						ApnsFail = obj.Value;
						break;
					case 2:
						BaiduFail = obj.Value;
						break;
					case 3:
						Result = new MsgResult();
						Result.ParseFrom(obj.Value);
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
			if(HasApnsFail)
			{
				sb.Append("ApnsFail : " + ApnsFail +",");
			}
			if(HasBaiduFail)
			{
				sb.Append("BaiduFail : " + BaiduFail +",");
			}
			sb.Append("Result : " + Result);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
