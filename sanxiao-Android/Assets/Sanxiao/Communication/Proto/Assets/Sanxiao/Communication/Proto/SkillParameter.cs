using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 技能参数。
	/// </summary>
	public class SkillParameter : ISendable, IReceiveable
	{
		private bool HasSkillCode{get;set;}
		private int skillCode;
		/// <summary>
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

		private List<float> constantList;
		/// <summary>
		/// 技能配置的常量。
		/// </summary>
		public List<float> ConstantList
		{
			get
			{
				return constantList;
			}
			set
			{
				if(value != null)
				{
					constantList = value;
				}
			}
		}

		/// <summary>
		/// 技能参数。
		/// </summary>
		public SkillParameter()
		{
			ConstantList = new List<float>();
		}

		/// <summary>
		/// 技能参数。
		/// </summary>
		public SkillParameter
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
			foreach(float v in ConstantList)
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
						SkillCode = obj.Value;
						break;
					case 2:
						ConstantList.Add(obj.Value);
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
			sb.Append("SkillCode : " + SkillCode + ",");
			sb.Append("ConstantList : [");
			for(int i = 0; i < ConstantList.Count;i ++)
			{
				if(i == ConstantList.Count -1)
				{
					sb.Append(ConstantList[i]);
				}else
				{
					sb.Append(ConstantList[i] + ",");
				}
			}
			sb.Append("]");
			sb.Append("}");
			return sb.ToString();
		}
	}
}
