using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 上传OAuth账户信息失败。
	/// </summary>
	public class BindOAuthInfoFail : ISendable, IReceiveable
	{
		private bool HasResult{get;set;}
		private MsgResult result;
		/// <summary>
		/// </summary>
		public MsgResult Result
		{
			get
			{
				return result;
			}
			set
			{
				if(value != null)
				{
					HasResult = true;
					result = value;
				}
			}
		}

		/// <summary>
		/// 上传OAuth账户信息失败。
		/// </summary>
		public BindOAuthInfoFail()
		{
		}

		/// <summary>
		/// 上传OAuth账户信息失败。
		/// </summary>
		public BindOAuthInfoFail
		(
			MsgResult result
		):this()
		{
			Result = result;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,Result);
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
						Result = new MsgResult();
						Result.ParseFrom(obj.Value);
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
			sb.Append("Result : " + Result);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
