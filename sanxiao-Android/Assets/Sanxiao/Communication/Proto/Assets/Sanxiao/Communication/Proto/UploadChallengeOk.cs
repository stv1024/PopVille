using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 成功上传挑战数据。
	/// </summary>
	public class UploadChallengeOk : ISendable, IReceiveable
	{
		private bool HasChallengeId{get;set;}
		private string challengeId;
		/// <summary>
		/// 成功上传的挑战的ID。
		/// </summary>
		public string ChallengeId
		{
			get
			{
				return challengeId;
			}
			set
			{
				if(value != null)
				{
					HasChallengeId = true;
					challengeId = value;
				}
			}
		}

		private bool HasStarCount{get;set;}
		private int starCount;
		/// <summary>
		/// 本局获得的评星数量。
		/// </summary>
		public int StarCount
		{
			get
			{
				return starCount;
			}
			set
			{
				HasStarCount = true;
				starCount = value;
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

		public bool HasUnlockElement{get;private set;}
		private UnlockElement unlockElement;
		/// <summary>
		/// 如果出现，则解锁了一些元素。
		/// </summary>
		public UnlockElement UnlockElement
		{
			get
			{
				return unlockElement;
			}
			set
			{
				if(value != null)
				{
					HasUnlockElement = true;
					unlockElement = value;
				}
			}
		}

		/// <summary>
		/// 成功上传挑战数据。
		/// </summary>
		public UploadChallengeOk()
		{
			RoundRewardList = new List<Currency>();
		}

		/// <summary>
		/// 成功上传挑战数据。
		/// </summary>
		public UploadChallengeOk
		(
			string challengeId,
			int starCount
		):this()
		{
			ChallengeId = challengeId;
			StarCount = starCount;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,ChallengeId);
			writer.Write(2,StarCount);
			foreach(Currency v in RoundRewardList)
			{
				writer.Write(3,v);
			}
			if(HasUnlockElement)
			{
				writer.Write(4,UnlockElement);
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
						ChallengeId = obj.Value;
						break;
					case 2:
						StarCount = obj.Value;
						break;
					case 3:
						 var roundReward= new Currency();
						roundReward.ParseFrom(obj.Value);
						RoundRewardList.Add(roundReward);
						break;
					case 4:
						UnlockElement = new UnlockElement();
						UnlockElement.ParseFrom(obj.Value);
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
			sb.Append("ChallengeId : " + ChallengeId + ",");
			sb.Append("StarCount : " + StarCount + ",");
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
			sb.Append("],");
			if(HasUnlockElement)
			{
				sb.Append("UnlockElement : " + UnlockElement);
			}
			sb.Append("}");
			return sb.ToString();
		}
	}
}
