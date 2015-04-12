using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 玩家切换角色。
	/// </summary>
	public class ChangeCharacter : ISendable, IReceiveable
	{
		private bool HasNewCharacterCode{get;set;}
		private int newCharacterCode;
		/// <summary>
		/// 新使用的角色代码。
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
		/// 玩家切换角色。
		/// </summary>
		public ChangeCharacter()
		{
		}

		/// <summary>
		/// 玩家切换角色。
		/// </summary>
		public ChangeCharacter
		(
			int newCharacterCode
		):this()
		{
			NewCharacterCode = newCharacterCode;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,NewCharacterCode);
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
			sb.Append("NewCharacterCode : " + NewCharacterCode);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
