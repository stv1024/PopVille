using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 队友加成。
	/// </summary>
	public class TeamAdd : ISendable, IReceiveable
	{
		private bool HasNickname{get;set;}
		private string nickname;
		/// <summary>
		/// 昵称。
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

		private bool HasCharacter{get;set;}
		private int character;
		/// <summary>
		/// 角色。
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

		private bool HasRoundInitHealth{get;set;}
		private int roundInitHealth;
		/// <summary>
		/// 初始健康值。
		/// </summary>
		public int RoundInitHealth
		{
			get
			{
				return roundInitHealth;
			}
			set
			{
				HasRoundInitHealth = true;
				roundInitHealth = value;
			}
		}

		private bool HasAttackAdd{get;set;}
		private int attackAdd;
		/// <summary>
		/// 攻击加成。
		/// </summary>
		public int AttackAdd
		{
			get
			{
				return attackAdd;
			}
			set
			{
				HasAttackAdd = true;
				attackAdd = value;
			}
		}

		/// <summary>
		/// 队友加成。
		/// </summary>
		public TeamAdd()
		{
		}

		/// <summary>
		/// 队友加成。
		/// </summary>
		public TeamAdd
		(
			string nickname,
			int character,
			int roundInitHealth,
			int attackAdd
		):this()
		{
			Nickname = nickname;
			Character = character;
			RoundInitHealth = roundInitHealth;
			AttackAdd = attackAdd;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,Nickname);
			writer.Write(2,Character);
			writer.Write(3,RoundInitHealth);
			writer.Write(4,AttackAdd);
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
						Nickname = obj.Value;
						break;
					case 2:
						Character = obj.Value;
						break;
					case 3:
						RoundInitHealth = obj.Value;
						break;
					case 4:
						AttackAdd = obj.Value;
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
			sb.Append("Nickname : " + Nickname + ",");
			sb.Append("Character : " + Character + ",");
			sb.Append("RoundInitHealth : " + RoundInitHealth + ",");
			sb.Append("AttackAdd : " + AttackAdd);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
