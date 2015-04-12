using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 技能等级详情的文本。
	/// </summary>
	public class SkillLevelDetailTextConfig : ISendable, IReceiveable
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

		private List<SkillLevelDetail> detailList;
		/// <summary>
		/// 技能等级详情列表。
		/// </summary>
		public List<SkillLevelDetail> DetailList
		{
			get
			{
				return detailList;
			}
			set
			{
				if(value != null)
				{
					detailList = value;
				}
			}
		}

		/// <summary>
		/// 技能等级详情的文本。
		/// </summary>
		public SkillLevelDetailTextConfig()
		{
			DetailList = new List<SkillLevelDetail>();
		}

		/// <summary>
		/// 技能等级详情的文本。
		/// </summary>
		public SkillLevelDetailTextConfig
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
			foreach(SkillLevelDetail v in DetailList)
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
						 var detail= new SkillLevelDetail();
						detail.ParseFrom(obj.Value);
						DetailList.Add(detail);
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
			sb.Append("DetailList : [");
			for(int i = 0; i < DetailList.Count;i ++)
			{
				if(i == DetailList.Count -1)
				{
					sb.Append(DetailList[i]);
				}else
				{
					sb.Append(DetailList[i] + ",");
				}
			}
			sb.Append("]");
			sb.Append("}");
			return sb.ToString();
		}
	}
}
