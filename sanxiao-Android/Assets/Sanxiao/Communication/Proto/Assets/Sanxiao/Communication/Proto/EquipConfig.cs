using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 装备的配置。
	/// </summary>
	public class EquipConfig : ISendable, IReceiveable
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

		private List<Equip> equipList;
		/// <summary>
		/// </summary>
		public List<Equip> EquipList
		{
			get
			{
				return equipList;
			}
			set
			{
				if(value != null)
				{
					equipList = value;
				}
			}
		}

		/// <summary>
		/// 装备的配置。
		/// </summary>
		public EquipConfig()
		{
			EquipList = new List<Equip>();
		}

		/// <summary>
		/// 装备的配置。
		/// </summary>
		public EquipConfig
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
			foreach(Equip v in EquipList)
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
						 var equip= new Equip();
						equip.ParseFrom(obj.Value);
						EquipList.Add(equip);
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
			sb.Append("EquipList : [");
			for(int i = 0; i < EquipList.Count;i ++)
			{
				if(i == EquipList.Count -1)
				{
					sb.Append(EquipList[i]);
				}else
				{
					sb.Append(EquipList[i] + ",");
				}
			}
			sb.Append("]");
			sb.Append("}");
			return sb.ToString();
		}
	}
}
