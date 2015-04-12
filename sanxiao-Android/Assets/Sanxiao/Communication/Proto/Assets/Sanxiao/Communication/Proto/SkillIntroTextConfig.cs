using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 技能简介文本。
	/// </summary>
	public class SkillIntroTextConfig : ISendable, IReceiveable
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

		private List<int> skillCodeList;
		/// <summary>
		/// 技能的代码列表。
		/// </summary>
		public List<int> SkillCodeList
		{
			get
			{
				return skillCodeList;
			}
			set
			{
				if(value != null)
				{
					skillCodeList = value;
				}
			}
		}

		private List<string> displayNameList;
		/// <summary>
		/// 技能的显示名称。
		/// </summary>
		public List<string> DisplayNameList
		{
			get
			{
				return displayNameList;
			}
			set
			{
				if(value != null)
				{
					displayNameList = value;
				}
			}
		}

		private List<string> introList;
		/// <summary>
		/// 技能的介绍。
		/// </summary>
		public List<string> IntroList
		{
			get
			{
				return introList;
			}
			set
			{
				if(value != null)
				{
					introList = value;
				}
			}
		}

		/// <summary>
		/// 技能简介文本。
		/// </summary>
		public SkillIntroTextConfig()
		{
			SkillCodeList = new List<int>();
			DisplayNameList = new List<string>();
			IntroList = new List<string>();
		}

		/// <summary>
		/// 技能简介文本。
		/// </summary>
		public SkillIntroTextConfig
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
			foreach(int v in SkillCodeList)
			{
				writer.Write(2,v);
			}
			foreach(string v in DisplayNameList)
			{
				writer.Write(3,v);
			}
			foreach(string v in IntroList)
			{
				writer.Write(4,v);
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
						SkillCodeList.Add(obj.Value);
						break;
					case 3:
						DisplayNameList.Add(obj.Value);
						break;
					case 4:
						IntroList.Add(obj.Value);
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
			sb.Append("SkillCodeList : [");
			for(int i = 0; i < SkillCodeList.Count;i ++)
			{
				if(i == SkillCodeList.Count -1)
				{
					sb.Append(SkillCodeList[i]);
				}else
				{
					sb.Append(SkillCodeList[i] + ",");
				}
			}
			sb.Append("],");
			sb.Append("DisplayNameList : [");
			for(int i = 0; i < DisplayNameList.Count;i ++)
			{
				if(i == DisplayNameList.Count -1)
				{
					sb.Append(DisplayNameList[i]);
				}else
				{
					sb.Append(DisplayNameList[i] + ",");
				}
			}
			sb.Append("],");
			sb.Append("IntroList : [");
			for(int i = 0; i < IntroList.Count;i ++)
			{
				if(i == IntroList.Count -1)
				{
					sb.Append(IntroList[i]);
				}else
				{
					sb.Append(IntroList[i] + ",");
				}
			}
			sb.Append("]");
			sb.Append("}");
			return sb.ToString();
		}
	}
}
