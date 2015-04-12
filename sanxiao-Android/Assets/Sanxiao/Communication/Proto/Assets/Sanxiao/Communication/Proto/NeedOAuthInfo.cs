using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 需要客户端重新上传授权。
	/// </summary>
	public class NeedOAuthInfo : ISendable, IReceiveable
	{
		public bool HasType{get;private set;}
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

		public bool HasName{get;private set;}
		private string name;
		/// <summary>
		/// 显示给用户便于用户重新绑定的社交账户名称。
		/// </summary>
		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				if(value != null)
				{
					HasName = true;
					name = value;
				}
			}
		}

		/// <summary>
		/// 需要客户端重新上传授权。
		/// </summary>
		public NeedOAuthInfo()
		{
		}

		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			if(HasType)
			{
				writer.Write(1,Type);
			}
			if(HasName)
			{
				writer.Write(2,Name);
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
						Name = obj.Value;
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
			if(HasType)
			{
				sb.Append("Type : " + Type +",");
			}
			if(HasName)
			{
				sb.Append("Name : " + Name);
			}
			sb.Append("}");
			return sb.ToString();
		}
	}
}
