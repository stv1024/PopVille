using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 心跳响应。
	/// </summary>
	public class HBRes : ISendable, IReceiveable
	{
		public bool HasSerialId{get;private set;}
		private long serialId;
		/// <summary>
		/// </summary>
		public long SerialId
		{
			get
			{
				return serialId;
			}
			set
			{
				HasSerialId = true;
				serialId = value;
			}
		}

		/// <summary>
		/// 心跳响应。
		/// </summary>
		public HBRes()
		{
		}

		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			if(HasSerialId)
			{
				writer.Write(2,SerialId);
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
					case 2:
						SerialId = obj.Value;
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
			if(HasSerialId)
			{
				sb.Append("SerialId : " + SerialId);
			}
			sb.Append("}");
			return sb.ToString();
		}
	}
}
