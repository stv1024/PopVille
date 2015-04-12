using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 技能等级详情。
	/// </summary>
	public class SkillLevelDetail : ISendable, IReceiveable
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

		private List<string> levelDetailList;
		/// <summary>
		/// 技能对应的每个等级的详情。
		/// </summary>
		public List<string> LevelDetailList
		{
			get
			{
				return levelDetailList;
			}
			set
			{
				if(value != null)
				{
					levelDetailList = value;
				}
			}
		}

		/// <summary>
		/// 技能等级详情。
		/// </summary>
		public SkillLevelDetail()
		{
			LevelDetailList = new List<string>();
		}

		/// <summary>
		/// 技能等级详情。
		/// </summary>
		public SkillLevelDetail
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
			foreach(string v in LevelDetailList)
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
						LevelDetailList.Add(obj.Value);
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
			sb.Append("LevelDetailList : [");
			for(int i = 0; i < LevelDetailList.Count;i ++)
			{
				if(i == LevelDetailList.Count -1)
				{
					sb.Append(LevelDetailList[i]);
				}else
				{
					sb.Append(LevelDetailList[i] + ",");
				}
			}
			sb.Append("]");
			sb.Append("}");
			return sb.ToString();
		}
	}
}
