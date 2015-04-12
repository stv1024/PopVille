using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// </summary>
	public class Packet : ISendable, IReceiveable
	{
		private bool HasType{get;set;}
		private int type;
		/// <summary>
		/// 命令的类型，这次使用int值。
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

		public bool HasContent{get;private set;}
		private byte[] content;
		/// <summary>
		/// 命令的二进制流，有的空命令可以只有type。
		/// </summary>
		public byte[] Content
		{
			get
			{
				return content;
			}
			set
			{
				if(value != null)
				{
					HasContent = true;
					content = value;
				}
			}
		}

		public bool HasEncryption{get;private set;}
		private string encryption;
		/// <summary>
		/// 加密的算法及加密的key等信息。
		/// </summary>
		public string Encryption
		{
			get
			{
				return encryption;
			}
			set
			{
				if(value != null)
				{
					HasEncryption = true;
					encryption = value;
				}
			}
		}

		/// <summary>
		/// </summary>
		public Packet()
		{
		}

		/// <summary>
		/// </summary>
		public Packet
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
			if(HasContent)
			{
				writer.Write(2,Content);
			}
			if(HasEncryption)
			{
				writer.Write(3,Encryption);
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
						Content = obj.Value;
						break;
					case 3:
						Encryption = obj.Value;
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
			if(HasContent)
			{
				sb.Append("Content : " + Content +",");
			}
			if(HasEncryption)
			{
				sb.Append("Encryption : " + Encryption);
			}
			sb.Append("}");
			return sb.ToString();
		}
	}
}
