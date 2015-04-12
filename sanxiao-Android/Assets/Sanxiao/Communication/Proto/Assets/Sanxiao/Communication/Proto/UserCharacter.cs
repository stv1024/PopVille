using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 玩家的角色列表。
	/// </summary>
	public class UserCharacter : ISendable, IReceiveable
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

		private List<int> wearEquipList;
		/// <summary>
		/// 角色身上穿的装备的code。
		/// </summary>
		public List<int> WearEquipList
		{
			get
			{
				return wearEquipList;
			}
			set
			{
				if(value != null)
				{
					wearEquipList = value;
				}
			}
		}

		/// <summary>
		/// 玩家的角色列表。
		/// </summary>
		public UserCharacter()
		{
			WearEquipList = new List<int>();
		}

		/// <summary>
		/// 玩家的角色列表。
		/// </summary>
		public UserCharacter
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
			foreach(int v in WearEquipList)
			{
				writer.Write(10,v);
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
						CharacterCode = obj.Value;
						break;
					case 10:
						WearEquipList.Add(obj.Value);
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
			sb.Append("WearEquipList : [");
			for(int i = 0; i < WearEquipList.Count;i ++)
			{
				if(i == WearEquipList.Count -1)
				{
					sb.Append(WearEquipList[i]);
				}else
				{
					sb.Append(WearEquipList[i] + ",");
				}
			}
			sb.Append("]");
			sb.Append("}");
			return sb.ToString();
		}
	}
}
