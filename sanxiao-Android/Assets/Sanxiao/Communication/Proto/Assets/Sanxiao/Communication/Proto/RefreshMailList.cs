using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 刷新邮件列表。
	/// </summary>
	public class RefreshMailList : ISendable, IReceiveable
	{
		private bool HasReadState{get;set;}
		private int readState;
		/// <summary>
		/// 邮件的状态：0 查询未读，1 查询已读，2 查询全部。
		/// </summary>
		public int ReadState
		{
			get
			{
				return readState;
			}
			set
			{
				HasReadState = true;
				readState = value;
			}
		}

		/// <summary>
		/// 刷新邮件列表。
		/// </summary>
		public RefreshMailList()
		{
		}

		/// <summary>
		/// 刷新邮件列表。
		/// </summary>
		public RefreshMailList
		(
			int readState
		):this()
		{
			ReadState = readState;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,ReadState);
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
						ReadState = obj.Value;
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
			sb.Append("ReadState : " + ReadState);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
