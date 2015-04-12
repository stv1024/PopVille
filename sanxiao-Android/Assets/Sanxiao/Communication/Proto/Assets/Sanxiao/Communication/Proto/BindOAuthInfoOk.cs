using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 成功绑定OAuth信息。
	/// </summary>
	public class BindOAuthInfoOk : ISendable, IReceiveable
	{
		private bool HasUid{get;set;}
		private string uid;
		/// <summary>
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

		private bool HasAccessToken{get;set;}
		private string accessToken;
		/// <summary>
		/// 账号的accessToken。
		/// </summary>
		public string AccessToken
		{
			get
			{
				return accessToken;
			}
			set
			{
				if(value != null)
				{
					HasAccessToken = true;
					accessToken = value;
				}
			}
		}

		private bool HasExpireTime{get;set;}
		private int expireTime;
		/// <summary>
		/// 过期时间。单位：秒。
		/// </summary>
		public int ExpireTime
		{
			get
			{
				return expireTime;
			}
			set
			{
				HasExpireTime = true;
				expireTime = value;
			}
		}

		/// <summary>
		/// 成功绑定OAuth信息。
		/// </summary>
		public BindOAuthInfoOk()
		{
		}

		/// <summary>
		/// 成功绑定OAuth信息。
		/// </summary>
		public BindOAuthInfoOk
		(
			string uid,
			string accessToken,
			int expireTime
		):this()
		{
			Uid = uid;
			AccessToken = accessToken;
			ExpireTime = expireTime;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,Uid);
			writer.Write(2,AccessToken);
			writer.Write(3,ExpireTime);
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
						Uid = obj.Value;
						break;
					case 2:
						AccessToken = obj.Value;
						break;
					case 3:
						ExpireTime = obj.Value;
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
			sb.Append("Uid : " + Uid + ",");
			sb.Append("AccessToken : " + AccessToken + ",");
			sb.Append("ExpireTime : " + ExpireTime);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
