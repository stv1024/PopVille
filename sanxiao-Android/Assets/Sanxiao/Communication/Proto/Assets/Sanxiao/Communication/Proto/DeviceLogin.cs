using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 游客设备登陆。
	/// </summary>
	public class DeviceLogin : ISendable, IReceiveable
	{
		private bool HasDeviceUid{get;set;}
		private string deviceUid;
		/// <summary>
		/// 设备的唯一识别ID。
		/// </summary>
		public string DeviceUid
		{
			get
			{
				return deviceUid;
			}
			set
			{
				if(value != null)
				{
					HasDeviceUid = true;
					deviceUid = value;
				}
			}
		}

		private bool HasClientInfo{get;set;}
		private ClientInfo clientInfo;
		/// <summary>
		/// 客户端信息。
		/// </summary>
		public ClientInfo ClientInfo
		{
			get
			{
				return clientInfo;
			}
			set
			{
				if(value != null)
				{
					HasClientInfo = true;
					clientInfo = value;
				}
			}
		}

		/// <summary>
		/// 游客设备登陆。
		/// </summary>
		public DeviceLogin()
		{
		}

		/// <summary>
		/// 游客设备登陆。
		/// </summary>
		public DeviceLogin
		(
			string deviceUid,
			ClientInfo clientInfo
		):this()
		{
			DeviceUid = deviceUid;
			ClientInfo = clientInfo;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,DeviceUid);
			writer.Write(2,ClientInfo);
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
						DeviceUid = obj.Value;
						break;
					case 2:
						ClientInfo = new ClientInfo();
						ClientInfo.ParseFrom(obj.Value);
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
			sb.Append("DeviceUid : " + DeviceUid + ",");
			sb.Append("ClientInfo : " + ClientInfo);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
