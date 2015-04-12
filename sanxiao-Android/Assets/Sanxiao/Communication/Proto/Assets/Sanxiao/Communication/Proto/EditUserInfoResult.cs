using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 编辑用户信息结果。
	/// </summary>
	public class EditUserInfoResult : ISendable, IReceiveable
	{
		public bool HasNicknameResult{get;private set;}
		private MsgResult nicknameResult;
		/// <summary>
		/// 编辑昵称的结果。
		/// </summary>
		public MsgResult NicknameResult
		{
			get
			{
				return nicknameResult;
			}
			set
			{
				if(value != null)
				{
					HasNicknameResult = true;
					nicknameResult = value;
				}
			}
		}

		public bool HasCharacterResult{get;private set;}
		private MsgResult characterResult;
		/// <summary>
		/// 编辑角色的结果。
		/// </summary>
		public MsgResult CharacterResult
		{
			get
			{
				return characterResult;
			}
			set
			{
				if(value != null)
				{
					HasCharacterResult = true;
					characterResult = value;
				}
			}
		}

		public bool HasCurrentUser{get;private set;}
		private User currentUser;
		/// <summary>
		/// 当前最新的用户信息。
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

		/// <summary>
		/// 编辑用户信息结果。
		/// </summary>
		public EditUserInfoResult()
		{
		}

		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			if(HasNicknameResult)
			{
				writer.Write(1,NicknameResult);
			}
			if(HasCharacterResult)
			{
				writer.Write(2,CharacterResult);
			}
			if(HasCurrentUser)
			{
				writer.Write(100,CurrentUser);
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
						NicknameResult = new MsgResult();
						NicknameResult.ParseFrom(obj.Value);
						break;
					case 2:
						CharacterResult = new MsgResult();
						CharacterResult.ParseFrom(obj.Value);
						break;
					case 100:
						CurrentUser = new User();
						CurrentUser.ParseFrom(obj.Value);
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
			if(HasNicknameResult)
			{
				sb.Append("NicknameResult : " + NicknameResult +",");
			}
			if(HasCharacterResult)
			{
				sb.Append("CharacterResult : " + CharacterResult +",");
			}
			if(HasCurrentUser)
			{
				sb.Append("CurrentUser : " + CurrentUser);
			}
			sb.Append("}");
			return sb.ToString();
		}
	}
}
