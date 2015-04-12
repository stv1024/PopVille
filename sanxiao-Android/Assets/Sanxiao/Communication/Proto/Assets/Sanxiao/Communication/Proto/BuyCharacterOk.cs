using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 购买角色成功。
	/// </summary>
	public class BuyCharacterOk : ISendable, IReceiveable
	{
		private bool HasCurrentUser{get;set;}
		private User currentUser;
		/// <summary>
		/// 购买成功后的玩家数据。
		/// </summary>
		public User CurrentUser
		{
			get
			{
				return currentUser;
			}
			set
			{
				if(value != null)
				{
					HasCurrentUser = true;
					currentUser = value;
				}
			}
		}

		private bool HasNewCharacter{get;set;}
		private UserCharacter newCharacter;
		/// <summary>
		/// 刚买的角色数据。
		/// </summary>
		public UserCharacter NewCharacter
		{
			get
			{
				return newCharacter;
			}
			set
			{
				if(value != null)
				{
					HasNewCharacter = true;
					newCharacter = value;
				}
			}
		}

		/// <summary>
		/// 购买角色成功。
		/// </summary>
		public BuyCharacterOk()
		{
		}

		/// <summary>
		/// 购买角色成功。
		/// </summary>
		public BuyCharacterOk
		(
			User currentUser,
			UserCharacter newCharacter
		):this()
		{
			CurrentUser = currentUser;
			NewCharacter = newCharacter;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,CurrentUser);
			writer.Write(2,NewCharacter);
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
						CurrentUser = new User();
						CurrentUser.ParseFrom(obj.Value);
						break;
					case 2:
						NewCharacter = new UserCharacter();
						NewCharacter.ParseFrom(obj.Value);
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
			sb.Append("CurrentUser : " + CurrentUser + ",");
			sb.Append("NewCharacter : " + NewCharacter);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
