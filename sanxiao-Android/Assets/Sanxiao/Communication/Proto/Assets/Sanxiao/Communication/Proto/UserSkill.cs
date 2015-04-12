using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 玩家拥有的技能数据。
	/// </summary>
	public class UserSkill : ISendable, IReceiveable
	{
		private bool HasCode{get;set;}
		private int code;
		/// <summary>
		/// 技能的code。
		/// </summary>
		public int Code
		{
			get
			{
				return code;
			}
			set
			{
				HasCode = true;
				code = value;
			}
		}

		private bool HasLevel{get;set;}
		private int level;
		/// <summary>
		/// 我的技能的当前等级。
		/// 0：未解锁，>0：当前的等级。
		/// </summary>
		public int Level
		{
			get
			{
				return level;
			}
			set
			{
				HasLevel = true;
				level = value;
			}
		}

		/// <summary>
		/// 玩家拥有的技能数据。
		/// </summary>
		public UserSkill()
		{
		}

		/// <summary>
		/// 玩家拥有的技能数据。
		/// </summary>
		public UserSkill
		(
			int code,
			int level
		):this()
		{
			Code = code;
			Level = level;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,Code);
			writer.Write(2,Level);
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
						Code = obj.Value;
						break;
					case 2:
						Level = obj.Value;
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
			sb.Append("Code : " + Code + ",");
			sb.Append("Level : " + Level);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
