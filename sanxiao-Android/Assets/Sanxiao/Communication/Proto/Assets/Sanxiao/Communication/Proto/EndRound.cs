using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 结束一局。
	/// </summary>
	public class EndRound : ISendable, IReceiveable
	{
		private bool HasMyInfo{get;set;}
		private User myInfo;
		/// <summary>
		/// 我的信息。
		/// </summary>
		public User MyInfo
		{
			get
			{
				return myInfo;
			}
			set
			{
				if(value != null)
				{
					HasMyInfo = true;
					myInfo = value;
				}
			}
		}

		private bool HasRivalInfo{get;set;}
		private User rivalInfo;
		/// <summary>
		/// 对手的信息。
		/// </summary>
		public User RivalInfo
		{
			get
			{
				return rivalInfo;
			}
			set
			{
				if(value != null)
				{
					HasRivalInfo = true;
					rivalInfo = value;
				}
			}
		}

		private bool HasWin{get;set;}
		private bool win;
		/// <summary>
		/// 我是否赢了。
		/// </summary>
		public bool Win
		{
			get
			{
				return win;
			}
			set
			{
				HasWin = true;
				win = value;
			}
		}

		private List<Currency> roundRewardList;
		/// <summary>
		/// 各种奖励。
		/// </summary>
		public List<Currency> RoundRewardList
		{
			get
			{
				return roundRewardList;
			}
			set
			{
				if(value != null)
				{
					roundRewardList = value;
				}
			}
		}

		/// <summary>
		/// 结束一局。
		/// </summary>
		public EndRound()
		{
			RoundRewardList = new List<Currency>();
		}

		/// <summary>
		/// 结束一局。
		/// </summary>
		public EndRound
		(
			User myInfo,
			User rivalInfo,
			bool win
		):this()
		{
			MyInfo = myInfo;
			RivalInfo = rivalInfo;
			Win = win;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,MyInfo);
			writer.Write(2,RivalInfo);
			writer.Write(3,Win);
			foreach(Currency v in RoundRewardList)
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
						MyInfo = new User();
						MyInfo.ParseFrom(obj.Value);
						break;
					case 2:
						RivalInfo = new User();
						RivalInfo.ParseFrom(obj.Value);
						break;
					case 3:
						Win = obj.Value;
						break;
					case 4:
						 var roundReward= new Currency();
						roundReward.ParseFrom(obj.Value);
						RoundRewardList.Add(roundReward);
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
			sb.Append("MyInfo : " + MyInfo + ",");
			sb.Append("RivalInfo : " + RivalInfo + ",");
			sb.Append("Win : " + Win + ",");
			sb.Append("RoundRewardList : [");
			for(int i = 0; i < RoundRewardList.Count;i ++)
			{
				if(i == RoundRewardList.Count -1)
				{
					sb.Append(RoundRewardList[i]);
				}else
				{
					sb.Append(RoundRewardList[i] + ",");
				}
			}
			sb.Append("]");
			sb.Append("}");
			return sb.ToString();
		}
	}
}
