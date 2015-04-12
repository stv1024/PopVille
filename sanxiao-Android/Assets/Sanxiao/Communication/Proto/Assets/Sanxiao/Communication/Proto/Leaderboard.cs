using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 排行榜数据。
	/// </summary>
	public class Leaderboard : ISendable, IReceiveable
	{
		private bool HasType{get;set;}
		private int type;
		/// <summary>
		/// 排行榜的类型。
		/// </summary>
		public int Type
		{
			get
			{
				return type;
			}
			set
			{
				HasType = true;
				type = value;
			}
		}

		private List<LeaderboardItem> itemList;
		/// <summary>
		/// 排行榜中的数据。
		/// </summary>
		public List<LeaderboardItem> ItemList
		{
			get
			{
				return itemList;
			}
			set
			{
				if(value != null)
				{
					itemList = value;
				}
			}
		}

		private bool HasMyItem{get;set;}
		private LeaderboardItem myItem;
		/// <summary>
		/// 我单独的数据。
		/// </summary>
		public LeaderboardItem MyItem
		{
			get
			{
				return myItem;
			}
			set
			{
				if(value != null)
				{
					HasMyItem = true;
					myItem = value;
				}
			}
		}

		/// <summary>
		/// 排行榜数据。
		/// </summary>
		public Leaderboard()
		{
			ItemList = new List<LeaderboardItem>();
		}

		/// <summary>
		/// 排行榜数据。
		/// </summary>
		public Leaderboard
		(
			int type,
			LeaderboardItem myItem
		):this()
		{
			Type = type;
			MyItem = myItem;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,Type);
			foreach(LeaderboardItem v in ItemList)
			{
				writer.Write(2,v);
			}
			writer.Write(3,MyItem);
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
						Type = obj.Value;
						break;
					case 2:
						 var item= new LeaderboardItem();
						item.ParseFrom(obj.Value);
						ItemList.Add(item);
						break;
					case 3:
						MyItem = new LeaderboardItem();
						MyItem.ParseFrom(obj.Value);
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
			sb.Append("Type : " + Type + ",");
			sb.Append("ItemList : [");
			for(int i = 0; i < ItemList.Count;i ++)
			{
				if(i == ItemList.Count -1)
				{
					sb.Append(ItemList[i]);
				}else
				{
					sb.Append(ItemList[i] + ",");
				}
			}
			sb.Append("],");
			sb.Append("MyItem : " + MyItem);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
