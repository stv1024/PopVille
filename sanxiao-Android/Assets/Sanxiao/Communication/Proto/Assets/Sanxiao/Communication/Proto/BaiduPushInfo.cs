using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 百度推送的配置信息。
	/// </summary>
	public class BaiduPushInfo : ISendable, IReceiveable
	{
		private bool HasAppId{get;set;}
		private string appId;
		/// <summary>
		/// </summary>
		public string AppId
		{
			get
			{
				return appId;
			}
			set
			{
				if(value != null)
				{
					HasAppId = true;
					appId = value;
				}
			}
		}

		private bool HasChannelId{get;set;}
		private string channelId;
		/// <summary>
		/// </summary>
		public string ChannelId
		{
			get
			{
				return channelId;
			}
			set
			{
				if(value != null)
				{
					HasChannelId = true;
					channelId = value;
				}
			}
		}

		private bool HasUserId{get;set;}
		private string userId;
		/// <summary>
		/// </summary>
		public string UserId
		{
			get
			{
				return userId;
			}
			set
			{
				if(value != null)
				{
					HasUserId = true;
					userId = value;
				}
			}
		}

		/// <summary>
		/// 百度推送的配置信息。
		/// </summary>
		public BaiduPushInfo()
		{
		}

		/// <summary>
		/// 百度推送的配置信息。
		/// </summary>
		public BaiduPushInfo
		(
			string appId,
			string channelId,
			string userId
		):this()
		{
			AppId = appId;
			ChannelId = channelId;
			UserId = userId;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,AppId);
			writer.Write(2,ChannelId);
			writer.Write(3,UserId);
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
						AppId = obj.Value;
						break;
					case 2:
						ChannelId = obj.Value;
						break;
					case 3:
						UserId = obj.Value;
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
			sb.Append("AppId : " + AppId + ",");
			sb.Append("ChannelId : " + ChannelId + ",");
			sb.Append("UserId : " + UserId);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
