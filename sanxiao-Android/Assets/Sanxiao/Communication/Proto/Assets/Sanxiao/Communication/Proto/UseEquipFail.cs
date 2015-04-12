using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 使用装备失败。
	/// </summary>
	public class UseEquipFail : ISendable, IReceiveable
	{
		private bool HasResult{get;set;}
		private MsgResult result;
		/// <summary>
		/// </summary>
		public MsgResult Result
		{
			get
			{
				return result;
			}
			set
			{
				if(value != null)
				{
					HasResult = true;
					result = value;
				}
			}
		}

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

		private bool HasEquipCode{get;set;}
		private int equipCode;
		/// <summary>
		/// </summary>
		public int EquipCode
		{
			get
			{
				return equipCode;
			}
			set
			{
				HasEquipCode = true;
				equipCode = value;
			}
		}

		private bool HasUseOrNot{get;set;}
		private bool useOrNot;
		/// <summary>
		/// </summary>
		public bool UseOrNot
		{
			get
			{
				return useOrNot;
			}
			set
			{
				HasUseOrNot = true;
				useOrNot = value;
			}
		}

		/// <summary>
		/// 使用装备失败。
		/// </summary>
		public UseEquipFail()
		{
		}

		/// <summary>
		/// 使用装备失败。
		/// </summary>
		public UseEquipFail
		(
			MsgResult result,
			int characterCode,
			int equipCode,
			bool useOrNot
		):this()
		{
			Result = result;
			CharacterCode = characterCode;
			EquipCode = equipCode;
			UseOrNot = useOrNot;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,Result);
			writer.Write(2,CharacterCode);
			writer.Write(3,EquipCode);
			writer.Write(4,UseOrNot);
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
						Result = new MsgResult();
						Result.ParseFrom(obj.Value);
						break;
					case 2:
						CharacterCode = obj.Value;
						break;
					case 3:
						EquipCode = obj.Value;
						break;
					case 4:
						UseOrNot = obj.Value;
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
			sb.Append("Result : " + Result + ",");
			sb.Append("CharacterCode : " + CharacterCode + ",");
			sb.Append("EquipCode : " + EquipCode + ",");
			sb.Append("UseOrNot : " + UseOrNot);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
