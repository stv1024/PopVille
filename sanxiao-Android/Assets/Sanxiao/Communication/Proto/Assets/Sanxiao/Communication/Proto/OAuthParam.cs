using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 社交平台参数。
	/// </summary>
	public class OAuthParam : ISendable, IReceiveable
	{
		private bool HasType{get;set;}
		private int type;
		/// <summary>
		/// 社交平台的类型。
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

		public bool HasAppKey{get;private set;}
		private string appKey;
		/// <summary>
		/// </summary>
		public string AppKey
		{
			get
			{
				return appKey;
			}
			set
			{
				if(value != null)
				{
					HasAppKey = true;
					appKey = value;
				}
			}
		}

		public bool HasAppSecret{get;private set;}
		private string appSecret;
		/// <summary>
		/// </summary>
		public string AppSecret
		{
			get
			{
				return appSecret;
			}
			set
			{
				if(value != null)
				{
					HasAppSecret = true;
					appSecret = value;
				}
			}
		}

		public bool HasRedirectUrl{get;private set;}
		private string redirectUrl;
		/// <summary>
		/// </summary>
		public string RedirectUrl
		{
			get
			{
				return redirectUrl;
			}
			set
			{
				if(value != null)
				{
					HasRedirectUrl = true;
					redirectUrl = value;
				}
			}
		}

		/// <summary>
		/// 社交平台参数。
		/// </summary>
		public OAuthParam()
		{
		}

		/// <summary>
		/// 社交平台参数。
		/// </summary>
		public OAuthParam
		(
			int type
		):this()
		{
			Type = type;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,Type);
			if(HasAppKey)
			{
				writer.Write(2,AppKey);
			}
			if(HasAppSecret)
			{
				writer.Write(3,AppSecret);
			}
			if(HasRedirectUrl)
			{
				writer.Write(4,RedirectUrl);
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
						AppKey = obj.Value;
						break;
					case 3:
						AppSecret = obj.Value;
						break;
					case 4:
						RedirectUrl = obj.Value;
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
			if(HasAppKey)
			{
				sb.Append("AppKey : " + AppKey +",");
			}
			if(HasAppSecret)
			{
				sb.Append("AppSecret : " + AppSecret +",");
			}
			if(HasRedirectUrl)
			{
				sb.Append("RedirectUrl : " + RedirectUrl);
			}
			sb.Append("}");
			return sb.ToString();
		}
	}
}
