using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 绑定玩家推送信息成功。
	/// </summary>
	public class BindUserPushInfoOk : ISendable, IReceiveable
	{
		public bool HasApnsOk{get;private set;}
		private bool apnsOk;
		/// <summary>
		/// 绑定ios push成功。
		/// </summary>
		public bool ApnsOk
		{
			get
			{
				return apnsOk;
			}
			set
			{
				HasApnsOk = true;
				apnsOk = value;
			}
		}

		public bool HasBaiduOk{get;private set;}
		private bool baiduOk;
		/// <summary>
		/// 绑定百度推送信息成功。
		/// </summary>
		public bool BaiduOk
		{
			get
			{
				return baiduOk;
			}
			set
			{
				HasBaiduOk = true;
				baiduOk = value;
			}
		}

		/// <summary>
		/// 绑定玩家推送信息成功。
		/// </summary>
		public BindUserPushInfoOk()
		{
		}

		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			if(HasApnsOk)
			{
				writer.Write(1,ApnsOk);
			}
			if(HasBaiduOk)
			{
				writer.Write(2,BaiduOk);
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
						ApnsOk = obj.Value;
						break;
					case 2:
						BaiduOk = obj.Value;
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
			if(HasApnsOk)
			{
				sb.Append("ApnsOk : " + ApnsOk +",");
			}
			if(HasBaiduOk)
			{
				sb.Append("BaiduOk : " + BaiduOk);
			}
			sb.Append("}");
			return sb.ToString();
		}
	}
}
