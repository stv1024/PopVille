using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 购买角色。
	/// </summary>
	public class BuyCharacter : ISendable, IReceiveable
	{
		private bool HasCharacterCode{get;set;}
		private int characterCode;
		/// <summary>
		/// </summary>
		public int CharacterCode
		{
			get
			{
				return characterCode;
			}
			set
			{
				HasCharacterCode = true;
				characterCode = value;
			}
		}

		/// <summary>
		/// 购买角色。
		/// </summary>
		public BuyCharacter()
		{
		}

		/// <summary>
		/// 购买角色。
		/// </summary>
		public BuyCharacter
		(
			int characterCode
		):this()
		{
			CharacterCode = characterCode;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,CharacterCode);
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
						CharacterCode = obj.Value;
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
			sb.Append("CharacterCode : " + CharacterCode);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
