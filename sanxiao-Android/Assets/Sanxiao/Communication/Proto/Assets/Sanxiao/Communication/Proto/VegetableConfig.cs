using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 蔬菜配置。
	/// </summary>
	public class VegetableConfig : ISendable, IReceiveable
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

		private List<Vegetable> vegetableList;
		/// <summary>
		/// </summary>
		public List<Vegetable> VegetableList
		{
			get
			{
				return vegetableList;
			}
			set
			{
				if(value != null)
				{
					vegetableList = value;
				}
			}
		}

		/// <summary>
		/// 蔬菜配置。
		/// </summary>
		public VegetableConfig()
		{
			VegetableList = new List<Vegetable>();
		}

		/// <summary>
		/// 蔬菜配置。
		/// </summary>
		public VegetableConfig
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
			foreach(Vegetable v in VegetableList)
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
						 var vegetable= new Vegetable();
						vegetable.ParseFrom(obj.Value);
						VegetableList.Add(vegetable);
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
			sb.Append("VegetableList : [");
			for(int i = 0; i < VegetableList.Count;i ++)
			{
				if(i == VegetableList.Count -1)
				{
					sb.Append(VegetableList[i]);
				}else
				{
					sb.Append(VegetableList[i] + ",");
				}
			}
			sb.Append("]");
			sb.Append("}");
			return sb.ToString();
		}
	}
}
