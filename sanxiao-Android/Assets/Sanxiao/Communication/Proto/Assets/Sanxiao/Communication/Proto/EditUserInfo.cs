using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 编辑用户信息。
	/// </summary>
	public class EditUserInfo : ISendable, IReceiveable
	{
		public bool HasNewNickname{get;private set;}
		private string newNickname;
		/// <summary>
		/// 新昵称。
		/// </summary>
		public string NewNickname
		{
			get
			{
				return newNickname;
			}
			set
			{
				if(value != null)
				{
					HasNewNickname = true;
					newNickname = value;
				}
			}
		}

		public bool HasNewCharacter{get;private set;}
		private int newCharacter;
		/// <summary>
		/// 新角色。
		/// </summary>
		public int NewCharacter
		{
			get
			{
				return newCharacter;
			}
			set
			{
				HasNewCharacter = true;
				newCharacter = value;
			}
		}

		/// <summary>
		/// 编辑用户信息。
		/// </summary>
		public EditUserInfo()
		{
		}

		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			if(HasNewNickname)
			{
				writer.Write(1,NewNickname);
			}
			if(HasNewCharacter)
			{
				writer.Write(2,NewCharacter);
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
						NewNickname = obj.Value;
						break;
					case 2:
						NewCharacter = obj.Value;
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
			if(HasNewNickname)
			{
				sb.Append("NewNickname : " + NewNickname +",");
			}
			if(HasNewCharacter)
			{
				sb.Append("NewCharacter : " + NewCharacter);
			}
			sb.Append("}");
			return sb.ToString();
		}
	}
}
