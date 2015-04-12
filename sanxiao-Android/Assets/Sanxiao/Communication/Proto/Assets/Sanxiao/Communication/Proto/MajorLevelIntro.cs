using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 大关卡的介绍。
	/// </summary>
	public class MajorLevelIntro : ISendable, IReceiveable
	{
		private bool HasMajorLevelId{get;set;}
		private int majorLevelId;
		/// <summary>
		/// 大关卡的id。
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

		private bool HasIntroContent{get;set;}
		private string introContent;
		/// <summary>
		/// 介绍的文本内容。
		/// </summary>
		public string IntroContent
		{
			get
			{
				return introContent;
			}
			set
			{
				if(value != null)
				{
					HasIntroContent = true;
					introContent = value;
				}
			}
		}

		private List<int> hiddenEquipList;
		/// <summary>
		/// 关卡中可能掉落的装备。
		/// </summary>
		public List<int> HiddenEquipList
		{
			get
			{
				return hiddenEquipList;
			}
			set
			{
				if(value != null)
				{
					hiddenEquipList = value;
				}
			}
		}

		/// <summary>
		/// 大关卡的介绍。
		/// </summary>
		public MajorLevelIntro()
		{
			HiddenEquipList = new List<int>();
		}

		/// <summary>
		/// 大关卡的介绍。
		/// </summary>
		public MajorLevelIntro
		(
			int majorLevelId,
			string introContent
		):this()
		{
			MajorLevelId = majorLevelId;
			IntroContent = introContent;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,MajorLevelId);
			writer.Write(2,IntroContent);
			foreach(int v in HiddenEquipList)
			{
				writer.Write(3,v);
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
						IntroContent = obj.Value;
						break;
					case 3:
						HiddenEquipList.Add(obj.Value);
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
			sb.Append("IntroContent : " + IntroContent + ",");
			sb.Append("HiddenEquipList : [");
			for(int i = 0; i < HiddenEquipList.Count;i ++)
			{
				if(i == HiddenEquipList.Count -1)
				{
					sb.Append(HiddenEquipList[i]);
				}else
				{
					sb.Append(HiddenEquipList[i] + ",");
				}
			}
			sb.Append("]");
			sb.Append("}");
			return sb.ToString();
		}
	}
}
