using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 技能参数配置。
	/// </summary>
	public class SkillParameterConfig : ISendable, IReceiveable
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

		private List<SkillParameter> skillParameterList;
		/// <summary>
		/// </summary>
		public List<SkillParameter> SkillParameterList
		{
			get
			{
				return skillParameterList;
			}
			set
			{
				if(value != null)
				{
					skillParameterList = value;
				}
			}
		}

		/// <summary>
		/// 技能参数配置。
		/// </summary>
		public SkillParameterConfig()
		{
			SkillParameterList = new List<SkillParameter>();
		}

		/// <summary>
		/// 技能参数配置。
		/// </summary>
		public SkillParameterConfig
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
			foreach(SkillParameter v in SkillParameterList)
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
						 var skillParameter= new SkillParameter();
						skillParameter.ParseFrom(obj.Value);
						SkillParameterList.Add(skillParameter);
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
			sb.Append("SkillParameterList : [");
			for(int i = 0; i < SkillParameterList.Count;i ++)
			{
				if(i == SkillParameterList.Count -1)
				{
					sb.Append(SkillParameterList[i]);
				}else
				{
					sb.Append(SkillParameterList[i] + ",");
				}
			}
			sb.Append("]");
			sb.Append("}");
			return sb.ToString();
		}
	}
}
