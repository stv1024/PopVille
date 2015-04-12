using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 检查订单成功。
	/// </summary>
	public class CheckTradeOk : ISendable, IReceiveable
	{
		private bool HasOutTradeNo{get;set;}
		private string outTradeNo;
		/// <summary>
		/// 订单的outTradeNo。
		/// </summary>
		public string OutTradeNo
		{
			get
			{
				return outTradeNo;
			}
			set
			{
				if(value != null)
				{
					HasOutTradeNo = true;
					outTradeNo = value;
				}
			}
		}

		private bool HasPayState{get;set;}
		private int payState;
		/// <summary>
		/// 订单的支付状态。
		/// </summary>
		public int PayState
		{
			get
			{
				return payState;
			}
			set
			{
				HasPayState = true;
				payState = value;
			}
		}

		private bool HasPostState{get;set;}
		private int postState;
		/// <summary>
		/// 订单的发货状态。
		/// </summary>
		public int PostState
		{
			get
			{
				return postState;
			}
			set
			{
				HasPostState = true;
				postState = value;
			}
		}

		private List<Currency> postedList;
		/// <summary>
		/// 已经发货的项目。
		/// </summary>
		public List<Currency> PostedList
		{
			get
			{
				return postedList;
			}
			set
			{
				if(value != null)
				{
					postedList = value;
				}
			}
		}

		/// <summary>
		/// 检查订单成功。
		/// </summary>
		public CheckTradeOk()
		{
			PostedList = new List<Currency>();
		}

		/// <summary>
		/// 检查订单成功。
		/// </summary>
		public CheckTradeOk
		(
			string outTradeNo,
			int payState,
			int postState
		):this()
		{
			OutTradeNo = outTradeNo;
			PayState = payState;
			PostState = postState;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,OutTradeNo);
			writer.Write(2,PayState);
			writer.Write(3,PostState);
			foreach(Currency v in PostedList)
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
						OutTradeNo = obj.Value;
						break;
					case 2:
						PayState = obj.Value;
						break;
					case 3:
						PostState = obj.Value;
						break;
					case 4:
						 var posted= new Currency();
						posted.ParseFrom(obj.Value);
						PostedList.Add(posted);
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
			sb.Append("OutTradeNo : " + OutTradeNo + ",");
			sb.Append("PayState : " + PayState + ",");
			sb.Append("PostState : " + PostState + ",");
			sb.Append("PostedList : [");
			for(int i = 0; i < PostedList.Count;i ++)
			{
				if(i == PostedList.Count -1)
				{
					sb.Append(PostedList[i]);
				}else
				{
					sb.Append(PostedList[i] + ",");
				}
			}
			sb.Append("]");
			sb.Append("}");
			return sb.ToString();
		}
	}
}
