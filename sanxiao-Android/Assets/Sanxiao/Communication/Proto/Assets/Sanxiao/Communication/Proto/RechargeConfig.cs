using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 充值包配置。
	/// </summary>
	public class RechargeConfig : ISendable, IReceiveable
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

		private List<Recharge> rechargeList;
		/// <summary>
		/// </summary>
		public List<Recharge> RechargeList
		{
			get
			{
				return rechargeList;
			}
			set
			{
				if(value != null)
				{
					rechargeList = value;
				}
			}
		}

		/// <summary>
		/// 充值包配置。
		/// </summary>
		public RechargeConfig()
		{
			RechargeList = new List<Recharge>();
		}

		/// <summary>
		/// 充值包配置。
		/// </summary>
		public RechargeConfig
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
			foreach(Recharge v in RechargeList)
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
						 var recharge= new Recharge();
						recharge.ParseFrom(obj.Value);
						RechargeList.Add(recharge);
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
			sb.Append("RechargeList : [");
			for(int i = 0; i < RechargeList.Count;i ++)
			{
				if(i == RechargeList.Count -1)
				{
					sb.Append(RechargeList[i]);
				}else
				{
					sb.Append(RechargeList[i] + ",");
				}
			}
			sb.Append("]");
			sb.Append("}");
			return sb.ToString();
		}
	}
}
