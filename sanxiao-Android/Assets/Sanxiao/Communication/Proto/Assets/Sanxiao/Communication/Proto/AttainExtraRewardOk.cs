using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 玩家在游戏过程中请求奖励成功。
	/// </summary>
	public class AttainExtraRewardOk : ISendable, IReceiveable
	{
		private List<Currency> rewardResultList;
		/// <summary>
		/// 奖励过之后，玩家拥有的当前值。
		/// </summary>
		public List<Currency> RewardResultList
		{
			get
			{
				return rewardResultList;
			}
			set
			{
				if(value != null)
				{
					rewardResultList = value;
				}
			}
		}

		/// <summary>
		/// 玩家在游戏过程中请求奖励成功。
		/// </summary>
		public AttainExtraRewardOk()
		{
			RewardResultList = new List<Currency>();
		}

		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			foreach(Currency v in RewardResultList)
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
					case 2:
						 var rewardResult= new Currency();
						rewardResult.ParseFrom(obj.Value);
						RewardResultList.Add(rewardResult);
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
			sb.Append("RewardResultList : [");
			for(int i = 0; i < RewardResultList.Count;i ++)
			{
				if(i == RewardResultList.Count -1)
				{
					sb.Append(RewardResultList[i]);
				}else
				{
					sb.Append(RewardResultList[i] + ",");
				}
			}
			sb.Append("]");
			sb.Append("}");
			return sb.ToString();
		}
	}
}
