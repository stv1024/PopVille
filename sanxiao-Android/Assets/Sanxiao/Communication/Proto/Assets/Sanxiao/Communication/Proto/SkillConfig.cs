using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 技能数据配置。
	/// </summary>
	public class SkillConfig : ISendable, IReceiveable
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

		private List<Skill> skillList;
		/// <summary>
		/// </summary>
		public List<Skill> SkillList
		{
			get
			{
				return skillList;
			}
			set
			{
				if(value != null)
				{
					skillList = value;
				}
			}
		}

		/// <summary>
		/// 技能数据配置。
		/// </summary>
		public SkillConfig()
		{
			SkillList = new List<Skill>();
		}

		/// <summary>
		/// 技能数据配置。
		/// </summary>
		public SkillConfig
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
			foreach(Skill v in SkillList)
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
						 var skill= new Skill();
						skill.ParseFrom(obj.Value);
						SkillList.Add(skill);
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
			sb.Append("SkillList : [");
			for(int i = 0; i < SkillList.Count;i ++)
			{
				if(i == SkillList.Count -1)
				{
					sb.Append(SkillList[i]);
				}else
				{
					sb.Append(SkillList[i] + ",");
				}
			}
			sb.Append("]");
			sb.Append("}");
			return sb.ToString();
		}
	}
}
