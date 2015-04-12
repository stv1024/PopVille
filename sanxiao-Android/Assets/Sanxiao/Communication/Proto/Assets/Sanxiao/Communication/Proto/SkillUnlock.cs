using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 技能解锁。
	/// </summary>
	public class SkillUnlock : ISendable, IReceiveable
	{
		private bool HasSkillCode{get;set;}
		private int skillCode;
		/// <summary>
		/// 技能代码。
		/// </summary>
		public int SkillCode
		{
			get
			{
				return skillCode;
			}
			set
			{
				HasSkillCode = true;
				skillCode = value;
			}
		}

		/// <summary>
		/// 技能解锁。
		/// </summary>
		public SkillUnlock()
		{
		}

		/// <summary>
		/// 技能解锁。
		/// </summary>
		public SkillUnlock
		(
			int skillCode
		):this()
		{
			SkillCode = skillCode;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,SkillCode);
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
						SkillCode = obj.Value;
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
			sb.Append("SkillCode : " + SkillCode);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
