using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 切换角色失败。
	/// </summary>
	public class ChangeCharacterFail : ISendable, IReceiveable
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

		private bool HasNewCharacterCode{get;set;}
		private int newCharacterCode;
		/// <summary>
		/// </summary>
		public int NewCharacterCode
		{
			get
			{
				return newCharacterCode;
			}
			set
			{
				HasNewCharacterCode = true;
				newCharacterCode = value;
			}
		}

		/// <summary>
		/// 切换角色失败。
		/// </summary>
		public ChangeCharacterFail()
		{
		}

		/// <summary>
		/// 切换角色失败。
		/// </summary>
		public ChangeCharacterFail
		(
			MsgResult result,
			int newCharacterCode
		):this()
		{
			Result = result;
			NewCharacterCode = newCharacterCode;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,Result);
			writer.Write(2,NewCharacterCode);
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
					case 2:
						NewCharacterCode = obj.Value;
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
			sb.Append("Result : " + Result + ",");
			sb.Append("NewCharacterCode : " + NewCharacterCode);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
