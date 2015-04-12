using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 随机队友信息。
	/// </summary>
	public class RandomTeamMember : ISendable, IReceiveable
	{
		private bool HasUserId{get;set;}
		private string userId;
		/// <summary>
		/// 随机队友的userId。
		/// </summary>
		public string UserId
		{
			get
			{
				return userId;
			}
			set
			{
				if(value != null)
				{
					HasUserId = true;
					userId = value;
				}
			}
		}

		private bool HasCharacter{get;set;}
		private int character;
		/// <summary>
		/// 随机队友的角色。
		/// </summary>
		public int Character
		{
			get
			{
				return character;
			}
			set
			{
				HasCharacter = true;
				character = value;
			}
		}

		private bool HasNickname{get;set;}
		private string nickname;
		/// <summary>
		/// 随机队友的昵称。
		/// </summary>
		public string Nickname
		{
			get
			{
				return nickname;
			}
			set
			{
				if(value != null)
				{
					HasNickname = true;
					nickname = value;
				}
			}
		}

		/// <summary>
		/// 随机队友信息。
		/// </summary>
		public RandomTeamMember()
		{
		}

		/// <summary>
		/// 随机队友信息。
		/// </summary>
		public RandomTeamMember
		(
			string userId,
			int character,
			string nickname
		):this()
		{
			UserId = userId;
			Character = character;
			Nickname = nickname;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,UserId);
			writer.Write(2,Character);
			writer.Write(3,Nickname);
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
						UserId = obj.Value;
						break;
					case 2:
						Character = obj.Value;
						break;
					case 3:
						Nickname = obj.Value;
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
			sb.Append("UserId : " + UserId + ",");
			sb.Append("Character : " + Character + ",");
			sb.Append("Nickname : " + Nickname);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
