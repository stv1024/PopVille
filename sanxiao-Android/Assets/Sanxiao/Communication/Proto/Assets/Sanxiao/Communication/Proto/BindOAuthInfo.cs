using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 客户端绑定OAuth信息。
	/// </summary>
	public class BindOAuthInfo : ISendable, IReceiveable
	{
		private bool HasType{get;set;}
		private int type;
		/// <summary>
		/// OAuth的类型。
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

		private bool HasAuthorizeCode{get;set;}
		private string authorizeCode;
		/// <summary>
		/// oauth的授权code。
		/// </summary>
		public string AuthorizeCode
		{
			get
			{
				return authorizeCode;
			}
			set
			{
				if(value != null)
				{
					HasAuthorizeCode = true;
					authorizeCode = value;
				}
			}
		}

		private bool HasDeviceId{get;set;}
		private string deviceId;
		/// <summary>
		/// 上传绑定信息设备的id。
		/// </summary>
		public string DeviceId
		{
			get
			{
				return deviceId;
			}
			set
			{
				if(value != null)
				{
					HasDeviceId = true;
					deviceId = value;
				}
			}
		}

		public bool HasUid{get;private set;}
		private string uid;
		/// <summary>
		/// 绑定腾讯微博，需要在这里上传openId。
		/// </summary>
		public string Uid
		{
			get
			{
				return uid;
			}
			set
			{
				if(value != null)
				{
					HasUid = true;
					uid = value;
				}
			}
		}

		/// <summary>
		/// 客户端绑定OAuth信息。
		/// </summary>
		public BindOAuthInfo()
		{
		}

		/// <summary>
		/// 客户端绑定OAuth信息。
		/// </summary>
		public BindOAuthInfo
		(
			int type,
			string authorizeCode,
			string deviceId
		):this()
		{
			Type = type;
			AuthorizeCode = authorizeCode;
			DeviceId = deviceId;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,Type);
			writer.Write(2,AuthorizeCode);
			writer.Write(3,DeviceId);
			if(HasUid)
			{
				writer.Write(4,Uid);
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
						Type = obj.Value;
						break;
					case 2:
						AuthorizeCode = obj.Value;
						break;
					case 3:
						DeviceId = obj.Value;
						break;
					case 4:
						Uid = obj.Value;
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
			sb.Append("AuthorizeCode : " + AuthorizeCode + ",");
			sb.Append("DeviceId : " + DeviceId + ",");
			if(HasUid)
			{
				sb.Append("Uid : " + Uid);
			}
			sb.Append("}");
			return sb.ToString();
		}
	}
}
