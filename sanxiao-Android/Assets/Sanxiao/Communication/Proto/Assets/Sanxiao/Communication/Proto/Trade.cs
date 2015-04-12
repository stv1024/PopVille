using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 交易。
	/// </summary>
	public class Trade : ISendable, IReceiveable
	{
		private bool HasOutTradeNo{get;set;}
		private string outTradeNo;
		/// <summary>
		/// 外部交易号。
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

		public bool HasTradeNo{get;private set;}
		private string tradeNo;
		/// <summary>
		/// 交易号。
		/// </summary>
		public string TradeNo
		{
			get
			{
				return tradeNo;
			}
			set
			{
				if(value != null)
				{
					HasTradeNo = true;
					tradeNo = value;
				}
			}
		}

		private bool HasRechargeName{get;set;}
		private string rechargeName;
		/// <summary>
		/// 支付包名称。
		/// </summary>
		public string RechargeName
		{
			get
			{
				return rechargeName;
			}
			set
			{
				if(value != null)
				{
					HasRechargeName = true;
					rechargeName = value;
				}
			}
		}

		private bool HasCount{get;set;}
		private int count;
		/// <summary>
		/// 购买的支付包数量。
		/// </summary>
		public int Count
		{
			get
			{
				return count;
			}
			set
			{
				HasCount = true;
				count = value;
			}
		}

		private bool HasTotalFee{get;set;}
		private double totalFee;
		/// <summary>
		/// 总费用。
		/// </summary>
		public double TotalFee
		{
			get
			{
				return totalFee;
			}
			set
			{
				HasTotalFee = true;
				totalFee = value;
			}
		}

		private bool HasPayChannel{get;set;}
		private int payChannel;
		/// <summary>
		/// 支付渠道。
		/// </summary>
		public int PayChannel
		{
			get
			{
				return payChannel;
			}
			set
			{
				HasPayChannel = true;
				payChannel = value;
			}
		}

		private bool HasBuyerUserId{get;set;}
		private string buyerUserId;
		/// <summary>
		/// 购买人的userId。
		/// </summary>
		public string BuyerUserId
		{
			get
			{
				return buyerUserId;
			}
			set
			{
				if(value != null)
				{
					HasBuyerUserId = true;
					buyerUserId = value;
				}
			}
		}

		public bool HasReceiverUserId{get;private set;}
		private string receiverUserId;
		/// <summary>
		/// 收货人的userId。
		/// </summary>
		public string ReceiverUserId
		{
			get
			{
				return receiverUserId;
			}
			set
			{
				if(value != null)
				{
					HasReceiverUserId = true;
					receiverUserId = value;
				}
			}
		}

		private bool HasPayState{get;set;}
		private int payState;
		/// <summary>
		/// 支付状态。
		/// PAY_STATE_CREATED = 1;
		/// PAY_STATE_WAIT_FOR_VALIDATE = 2;
		/// PAY_STATE_PAID = 3;
		/// PAY_STATE_INVALID = 4;
		/// PAY_STATE_CLOSED = 5;
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

		public bool HasPostState{get;private set;}
		private int postState;
		/// <summary>
		/// 发货状态。
		/// POST_STATE_UNPOSTED = 1;
		/// POST_STATE_POSTED = 2;
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

		private bool HasCreateTime{get;set;}
		private long createTime;
		/// <summary>
		/// 创建时间。
		/// </summary>
		public long CreateTime
		{
			get
			{
				return createTime;
			}
			set
			{
				HasCreateTime = true;
				createTime = value;
			}
		}

		public bool HasPayTime{get;private set;}
		private long payTime;
		/// <summary>
		/// 支付时间。
		/// </summary>
		public long PayTime
		{
			get
			{
				return payTime;
			}
			set
			{
				HasPayTime = true;
				payTime = value;
			}
		}

		public bool HasCloseTime{get;private set;}
		private long closeTime;
		/// <summary>
		/// 关闭时间。
		/// </summary>
		public long CloseTime
		{
			get
			{
				return closeTime;
			}
			set
			{
				HasCloseTime = true;
				closeTime = value;
			}
		}

		/// <summary>
		/// 交易。
		/// </summary>
		public Trade()
		{
		}

		/// <summary>
		/// 交易。
		/// </summary>
		public Trade
		(
			string outTradeNo,
			string rechargeName,
			int count,
			double totalFee,
			int payChannel,
			string buyerUserId,
			int payState,
			long createTime
		):this()
		{
			OutTradeNo = outTradeNo;
			RechargeName = rechargeName;
			Count = count;
			TotalFee = totalFee;
			PayChannel = payChannel;
			BuyerUserId = buyerUserId;
			PayState = payState;
			CreateTime = createTime;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,OutTradeNo);
			if(HasTradeNo)
			{
				writer.Write(2,TradeNo);
			}
			writer.Write(3,RechargeName);
			writer.Write(4,Count);
			writer.Write(5,TotalFee);
			writer.Write(6,PayChannel);
			writer.Write(7,BuyerUserId);
			if(HasReceiverUserId)
			{
				writer.Write(8,ReceiverUserId);
			}
			writer.Write(9,PayState);
			if(HasPostState)
			{
				writer.Write(10,PostState);
			}
			writer.Write(11,CreateTime);
			if(HasPayTime)
			{
				writer.Write(12,PayTime);
			}
			if(HasCloseTime)
			{
				writer.Write(13,CloseTime);
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
						TradeNo = obj.Value;
						break;
					case 3:
						RechargeName = obj.Value;
						break;
					case 4:
						Count = obj.Value;
						break;
					case 5:
						TotalFee = obj.Value;
						break;
					case 6:
						PayChannel = obj.Value;
						break;
					case 7:
						BuyerUserId = obj.Value;
						break;
					case 8:
						ReceiverUserId = obj.Value;
						break;
					case 9:
						PayState = obj.Value;
						break;
					case 10:
						PostState = obj.Value;
						break;
					case 11:
						CreateTime = obj.Value;
						break;
					case 12:
						PayTime = obj.Value;
						break;
					case 13:
						CloseTime = obj.Value;
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
			if(HasTradeNo)
			{
				sb.Append("TradeNo : " + TradeNo +",");
			}
			sb.Append("RechargeName : " + RechargeName + ",");
			sb.Append("Count : " + Count + ",");
			sb.Append("TotalFee : " + TotalFee + ",");
			sb.Append("PayChannel : " + PayChannel + ",");
			sb.Append("BuyerUserId : " + BuyerUserId + ",");
			if(HasReceiverUserId)
			{
				sb.Append("ReceiverUserId : " + ReceiverUserId +",");
			}
			sb.Append("PayState : " + PayState + ",");
			if(HasPostState)
			{
				sb.Append("PostState : " + PostState +",");
			}
			sb.Append("CreateTime : " + CreateTime + ",");
			if(HasPayTime)
			{
				sb.Append("PayTime : " + PayTime +",");
			}
			if(HasCloseTime)
			{
				sb.Append("CloseTime : " + CloseTime);
			}
			sb.Append("}");
			return sb.ToString();
		}
	}
}
