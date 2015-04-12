using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 角色的介绍文本。
	/// </summary>
	public class CharacterIntro : ISendable, IReceiveable
	{
		private bool HasCharacterCode{get;set;}
		private int characterCode;
		/// <summary>
		/// 角色的code。
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

		private bool HasDescription{get;set;}
		private string description;
		/// <summary>
		/// 角色的描述。
		/// </summary>
		public string Description
		{
			get
			{
				return description;
			}
			set
			{
				if(value != null)
				{
					HasDescription = true;
					description = value;
				}
			}
		}

		private bool HasTalentIntro{get;set;}
		private string talentIntro;
		/// <summary>
		/// 角色的天赋技能描述。
		/// </summary>
		public string TalentIntro
		{
			get
			{
				return talentIntro;
			}
			set
			{
				if(value != null)
				{
					HasTalentIntro = true;
					talentIntro = value;
				}
			}
		}

		/// <summary>
		/// 角色的介绍文本。
		/// </summary>
		public CharacterIntro()
		{
		}

		/// <summary>
		/// 角色的介绍文本。
		/// </summary>
		public CharacterIntro
		(
			int characterCode,
			string description,
			string talentIntro
		):this()
		{
			CharacterCode = characterCode;
			Description = description;
			TalentIntro = talentIntro;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,CharacterCode);
			writer.Write(2,Description);
			writer.Write(3,TalentIntro);
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
					case 2:
						Description = obj.Value;
						break;
					case 3:
						TalentIntro = obj.Value;
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
			sb.Append("CharacterCode : " + CharacterCode + ",");
			sb.Append("Description : " + Description + ",");
			sb.Append("TalentIntro : " + TalentIntro);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
