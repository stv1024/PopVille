using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 玩家绑定推送信息。
	/// </summary>
	public class BindUserPushInfo : ISendable, IReceiveable
	{
		public bool HasApnsDeviceToken{get;private set;}
		private string apnsDeviceToken;
		/// <summary>
		/// 玩家ios设备推送用的token。
		/// </summary>
		public string ApnsDeviceToken
		{
			get
			{
				return apnsDeviceToken;
			}
			set
			{
				if(value != null)
				{
					HasApnsDeviceToken = true;
					apnsDeviceToken = value;
				}
			}
		}

		public bool HasBaiduPushInfo{get;private set;}
		private BaiduPushInfo baiduPushInfo;
		/// <summary>
		/// 玩家安卓设备的百度推送信息。
		/// </summary>
		public BaiduPushInfo BaiduPushInfo
		{
			get
			{
				return baiduPushInfo;
			}
			set
			{
				if(value != null)
				{
					HasBaiduPushInfo = true;
					baiduPushInfo = value;
				}
			}
		}

		/// <summary>
		/// 玩家绑定推送信息。
		/// </summary>
		public BindUserPushInfo()
		{
		}

		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			if(HasApnsDeviceToken)
			{
				writer.Write(1,ApnsDeviceToken);
			}
			if(HasBaiduPushInfo)
			{
				writer.Write(2,BaiduPushInfo);
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
						ApnsDeviceToken = obj.Value;
						break;
					case 2:
						BaiduPushInfo = new BaiduPushInfo();
						BaiduPushInfo.ParseFrom(obj.Value);
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
			if(HasApnsDeviceToken)
			{
				sb.Append("ApnsDeviceToken : " + ApnsDeviceToken +",");
			}
			if(HasBaiduPushInfo)
			{
				sb.Append("BaiduPushInfo : " + BaiduPushInfo);
			}
			sb.Append("}");
			return sb.ToString();
		}
	}
}
