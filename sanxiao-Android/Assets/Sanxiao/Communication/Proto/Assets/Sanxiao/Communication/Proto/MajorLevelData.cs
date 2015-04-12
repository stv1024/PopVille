using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 大关卡的配置信息。
	/// </summary>
	public class MajorLevelData : ISendable, IReceiveable
	{
		private bool HasMajorLevelId{get;set;}
		private int majorLevelId;
		/// <summary>
		/// 大关的id。
		/// </summary>
		public int MajorLevelId
		{
			get
			{
				return majorLevelId;
			}
			set
			{
				HasMajorLevelId = true;
				majorLevelId = value;
			}
		}

		private bool HasTitle{get;set;}
		private string title;
		/// <summary>
		/// 标题。
		/// </summary>
		public string Title
		{
			get
			{
				return title;
			}
			set
			{
				if(value != null)
				{
					HasTitle = true;
					title = value;
				}
			}
		}

		public bool HasDescription{get;private set;}
		private string description;
		/// <summary>
		/// 描述。
		/// </summary>
		public string Description
		{
			get
			{
				return description;
			}
			set
			{
				if(value != null)
				{
					HasDescription = true;
					description = value;
				}
			}
		}

		private List<SubLevelData> subLevelList;
		/// <summary>
		/// 小关的配置。
		/// </summary>
		public List<SubLevelData> SubLevelList
		{
			get
			{
				return subLevelList;
			}
			set
			{
				if(value != null)
				{
					subLevelList = value;
				}
			}
		}

		/// <summary>
		/// 大关卡的配置信息。
		/// </summary>
		public MajorLevelData()
		{
			SubLevelList = new List<SubLevelData>();
		}

		/// <summary>
		/// 大关卡的配置信息。
		/// </summary>
		public MajorLevelData
		(
			int majorLevelId,
			string title
		):this()
		{
			MajorLevelId = majorLevelId;
			Title = title;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,MajorLevelId);
			writer.Write(2,Title);
			if(HasDescription)
			{
				writer.Write(3,Description);
			}
			foreach(SubLevelData v in SubLevelList)
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
						MajorLevelId = obj.Value;
						break;
					case 2:
						Title = obj.Value;
						break;
					case 3:
						Description = obj.Value;
						break;
					case 4:
						 var subLevel= new SubLevelData();
						subLevel.ParseFrom(obj.Value);
						SubLevelList.Add(subLevel);
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
			sb.Append("MajorLevelId : " + MajorLevelId + ",");
			sb.Append("Title : " + Title + ",");
			if(HasDescription)
			{
				sb.Append("Description : " + Description +",");
			}
			sb.Append("SubLevelList : [");
			for(int i = 0; i < SubLevelList.Count;i ++)
			{
				if(i == SubLevelList.Count -1)
				{
					sb.Append(SubLevelList[i]);
				}else
				{
					sb.Append(SubLevelList[i] + ",");
				}
			}
			sb.Append("]");
			sb.Append("}");
			return sb.ToString();
		}
	}
}
