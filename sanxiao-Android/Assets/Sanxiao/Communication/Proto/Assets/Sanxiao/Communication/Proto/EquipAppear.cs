using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 出现装备。
	/// </summary>
	public class EquipAppear : ISendable, IReceiveable
	{
		private List<int> equipCodeList;
		/// <summary>
		/// 装备的代码。
		/// </summary>
		public List<int> EquipCodeList
		{
			get
			{
				return equipCodeList;
			}
			set
			{
				if(value != null)
				{
					equipCodeList = value;
				}
			}
		}

		/// <summary>
		/// 出现装备。
		/// </summary>
		public EquipAppear()
		{
			EquipCodeList = new List<int>();
		}

		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			foreach(int v in EquipCodeList)
			{
				writer.Write(1,v);
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
						EquipCodeList.Add(obj.Value);
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
			sb.Append("EquipCodeList : [");
			for(int i = 0; i < EquipCodeList.Count;i ++)
			{
				if(i == EquipCodeList.Count -1)
				{
					sb.Append(EquipCodeList[i]);
				}else
				{
					sb.Append(EquipCodeList[i] + ",");
				}
			}
			sb.Append("]");
			sb.Append("}");
			return sb.ToString();
		}
	}
}
