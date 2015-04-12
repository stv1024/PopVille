using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 角色配置。
	/// </summary>
	public class CharacterConfig : ISendable, IReceiveable
	{
		private bool HasHash{get;set;}
		private string hash;
		/// <summary>
		/// </summary>
		public string Hash
		{
			get
			{
				return hash;
			}
			set
			{
				if(value != null)
				{
					HasHash = true;
					hash = value;
				}
			}
		}

		private List<Character> characterList;
		/// <summary>
		/// </summary>
		public List<Character> CharacterList
		{
			get
			{
				return characterList;
			}
			set
			{
				if(value != null)
				{
					characterList = value;
				}
			}
		}

		/// <summary>
		/// 角色配置。
		/// </summary>
		public CharacterConfig()
		{
			CharacterList = new List<Character>();
		}

		/// <summary>
		/// 角色配置。
		/// </summary>
		public CharacterConfig
		(
			string hash
		):this()
		{
			Hash = hash;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,Hash);
			foreach(Character v in CharacterList)
			{
				writer.Write(2,v);
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
						Hash = obj.Value;
						break;
					case 2:
						 var character= new Character();
						character.ParseFrom(obj.Value);
						CharacterList.Add(character);
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
			sb.Append("Hash : " + Hash + ",");
			sb.Append("CharacterList : [");
			for(int i = 0; i < CharacterList.Count;i ++)
			{
				if(i == CharacterList.Count -1)
				{
					sb.Append(CharacterList[i]);
				}else
				{
					sb.Append(CharacterList[i] + ",");
				}
			}
			sb.Append("]");
			sb.Append("}");
			return sb.ToString();
		}
	}
}
