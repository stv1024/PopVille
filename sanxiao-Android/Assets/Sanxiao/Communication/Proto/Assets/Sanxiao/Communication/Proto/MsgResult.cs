using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 消息结果。
	/// </summary>
	public class MsgResult : ISendable, IReceiveable
	{
		private bool HasCode{get;set;}
		private int code;
		/// <summary>
		/// 结果码。
		/// </summary>
		public int Code
		{
			get
			{
				return code;
			}
			set
			{
				HasCode = true;
				code = value;
			}
		}

		public bool HasMsg{get;private set;}
		private string msg;
		/// <summary>
		/// 对结果的描述。
		/// </summary>
		public string Msg
		{
			get
			{
				return msg;
			}
			set
			{
				if(value != null)
				{
					HasMsg = true;
					msg = value;
				}
			}
		}

		/// <summary>
		/// 消息结果。
		/// </summary>
		public MsgResult()
		{
		}

		/// <summary>
		/// 消息结果。
		/// </summary>
		public MsgResult
		(
			int code
		):this()
		{
			Code = code;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,Code);
			if(HasMsg)
			{
				writer.Write(2,Msg);
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
						Code = obj.Value;
						break;
					case 2:
						Msg = obj.Value;
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
			sb.Append("Code : " + Code + ",");
			if(HasMsg)
			{
				sb.Append("Msg : " + Msg);
			}
			sb.Append("}");
			return sb.ToString();
		}
	}
}
