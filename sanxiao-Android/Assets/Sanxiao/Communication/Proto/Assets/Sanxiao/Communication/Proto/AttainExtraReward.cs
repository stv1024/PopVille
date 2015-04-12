using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 玩家在游戏的过程中，请求奖励。比如见到金币之类的。
	/// </summary>
	public class AttainExtraReward : ISendable, IReceiveable
	{
		private List<Currency> extraRewardList;
		/// <summary>
		/// 玩家请求的奖励列表。
		/// </summary>
		public List<Currency> ExtraRewardList
		{
			get
			{
				return extraRewardList;
			}
			set
			{
				if(value != null)
				{
					extraRewardList = value;
				}
			}
		}

		/// <summary>
		/// 玩家在游戏的过程中，请求奖励。比如见到金币之类的。
		/// </summary>
		public AttainExtraReward()
		{
			ExtraRewardList = new List<Currency>();
		}

		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			foreach(Currency v in ExtraRewardList)
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
						 var extraReward= new Currency();
						extraReward.ParseFrom(obj.Value);
						ExtraRewardList.Add(extraReward);
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
			sb.Append("ExtraRewardList : [");
			for(int i = 0; i < ExtraRewardList.Count;i ++)
			{
				if(i == ExtraRewardList.Count -1)
				{
					sb.Append(ExtraRewardList[i]);
				}else
				{
					sb.Append(ExtraRewardList[i] + ",");
				}
			}
			sb.Append("]");
			sb.Append("}");
			return sb.ToString();
		}
	}
}
