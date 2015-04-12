using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 推图关卡的总配置。
	/// </summary>
	public class ChallengeLevelConfig : ISendable, IReceiveable
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

		private List<MajorLevelData> majorLevelList;
		/// <summary>
		/// 大关卡配置。
		/// </summary>
		public List<MajorLevelData> MajorLevelList
		{
			get
			{
				return majorLevelList;
			}
			set
			{
				if(value != null)
				{
					majorLevelList = value;
				}
			}
		}

		/// <summary>
		/// 推图关卡的总配置。
		/// </summary>
		public ChallengeLevelConfig()
		{
			MajorLevelList = new List<MajorLevelData>();
		}

		/// <summary>
		/// 推图关卡的总配置。
		/// </summary>
		public ChallengeLevelConfig
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
			foreach(MajorLevelData v in MajorLevelList)
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
						 var majorLevel= new MajorLevelData();
						majorLevel.ParseFrom(obj.Value);
						MajorLevelList.Add(majorLevel);
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
			sb.Append("MajorLevelList : [");
			for(int i = 0; i < MajorLevelList.Count;i ++)
			{
				if(i == MajorLevelList.Count -1)
				{
					sb.Append(MajorLevelList[i]);
				}else
				{
					sb.Append(MajorLevelList[i] + ",");
				}
			}
			sb.Append("]");
			sb.Append("}");
			return sb.ToString();
		}
	}
}
