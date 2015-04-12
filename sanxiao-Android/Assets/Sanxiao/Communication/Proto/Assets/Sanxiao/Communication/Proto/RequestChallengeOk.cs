using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 请求挑战成功。
	/// </summary>
	public class RequestChallengeOk : ISendable, IReceiveable
	{
		private bool HasMajorLevelId{get;set;}
		private int majorLevelId;
		/// <summary>
		/// 大关的id。
		/// </summary>
		public int MajorLevelId
		{
			get
			{
				return majorLevelId;
			}
			set
			{
				HasMajorLevelId = true;
				majorLevelId = value;
			}
		}

		private bool HasSubLevelId{get;set;}
		private int subLevelId;
		/// <summary>
		/// 小关的id。
		/// </summary>
		public int SubLevelId
		{
			get
			{
				return subLevelId;
			}
			set
			{
				HasSubLevelId = true;
				subLevelId = value;
			}
		}

		private bool HasChallengeId{get;set;}
		private string challengeId;
		/// <summary>
		/// 生成的挑战id。
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

		private bool HasBossData{get;set;}
		private DefenseData bossData;
		/// <summary>
		/// 对方老大的数据。
		/// </summary>
		public DefenseData BossData
		{
			get
			{
				return bossData;
			}
			set
			{
				if(value != null)
				{
					HasBossData = true;
					bossData = value;
				}
			}
		}

		private List<TeamAdd> fellowDataList;
		/// <summary>
		/// 对方小弟的辅助数据。
		/// </summary>
		public List<TeamAdd> FellowDataList
		{
			get
			{
				return fellowDataList;
			}
			set
			{
				if(value != null)
				{
					fellowDataList = value;
				}
			}
		}

		/// <summary>
		/// 请求挑战成功。
		/// </summary>
		public RequestChallengeOk()
		{
			FellowDataList = new List<TeamAdd>();
		}

		/// <summary>
		/// 请求挑战成功。
		/// </summary>
		public RequestChallengeOk
		(
			int majorLevelId,
			int subLevelId,
			string challengeId,
			DefenseData bossData
		):this()
		{
			MajorLevelId = majorLevelId;
			SubLevelId = subLevelId;
			ChallengeId = challengeId;
			BossData = bossData;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,MajorLevelId);
			writer.Write(2,SubLevelId);
			writer.Write(3,ChallengeId);
			writer.Write(4,BossData);
			foreach(TeamAdd v in FellowDataList)
			{
				writer.Write(5,v);
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
						MajorLevelId = obj.Value;
						break;
					case 2:
						SubLevelId = obj.Value;
						break;
					case 3:
						ChallengeId = obj.Value;
						break;
					case 4:
						BossData = new DefenseData();
						BossData.ParseFrom(obj.Value);
						break;
					case 5:
						 var fellowData= new TeamAdd();
						fellowData.ParseFrom(obj.Value);
						FellowDataList.Add(fellowData);
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
			sb.Append("MajorLevelId : " + MajorLevelId + ",");
			sb.Append("SubLevelId : " + SubLevelId + ",");
			sb.Append("ChallengeId : " + ChallengeId + ",");
			sb.Append("BossData : " + BossData + ",");
			sb.Append("FellowDataList : [");
			for(int i = 0; i < FellowDataList.Count;i ++)
			{
				if(i == FellowDataList.Count -1)
				{
					sb.Append(FellowDataList[i]);
				}else
				{
					sb.Append(FellowDataList[i] + ",");
				}
			}
			sb.Append("]");
			sb.Append("}");
			return sb.ToString();
		}
	}
}
