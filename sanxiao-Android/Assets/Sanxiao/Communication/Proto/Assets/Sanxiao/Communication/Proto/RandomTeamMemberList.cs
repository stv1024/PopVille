using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 请求随机队友列表。
	/// RequestRandomTeamMemberList
	/// 随机队友列表。
	/// </summary>
	public class RandomTeamMemberList : ISendable, IReceiveable
	{
		private List<RandomTeamMember> teamMemberList;
		/// <summary>
		/// </summary>
		public List<RandomTeamMember> TeamMemberList
		{
			get
			{
				return teamMemberList;
			}
			set
			{
				if(value != null)
				{
					teamMemberList = value;
				}
			}
		}

		/// <summary>
		/// 请求随机队友列表。
		/// RequestRandomTeamMemberList
		/// 随机队友列表。
		/// </summary>
		public RandomTeamMemberList()
		{
			TeamMemberList = new List<RandomTeamMember>();
		}

		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			foreach(RandomTeamMember v in TeamMemberList)
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
						 var teamMember= new RandomTeamMember();
						teamMember.ParseFrom(obj.Value);
						TeamMemberList.Add(teamMember);
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
			sb.Append("TeamMemberList : [");
			for(int i = 0; i < TeamMemberList.Count;i ++)
			{
				if(i == TeamMemberList.Count -1)
				{
					sb.Append(TeamMemberList[i]);
				}else
				{
					sb.Append(TeamMemberList[i] + ",");
				}
			}
			sb.Append("]");
			sb.Append("}");
			return sb.ToString();
		}
	}
}
