﻿using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 装备介绍。
	/// </summary>
	public class EquipIntro : ISendable, IReceiveable
	{
		private bool HasEquipCode{get;set;}
		private int equipCode;
		/// <summary>
		/// 装备的code。
		/// </summary>
		public int EquipCode
		{
			get
			{
				return equipCode;
			}
			set
			{
				HasEquipCode = true;
				equipCode = value;
			}
		}

		private bool HasIntroContent{get;set;}
		private string introContent;
		/// <summary>
		/// 介绍文本的内容。
		/// </summary>
		public string IntroContent
		{
			get
			{
				return introContent;
			}
			set
			{
				if(value != null)
				{
					HasIntroContent = true;
					introContent = value;
				}
			}
		}

		/// <summary>
		/// 装备介绍。
		/// </summary>
		public EquipIntro()
		{
		}

		/// <summary>
		/// 装备介绍。
		/// </summary>
		public EquipIntro
		(
			int equipCode,
			string introContent
		):this()
		{
			EquipCode = equipCode;
			IntroContent = introContent;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,EquipCode);
			writer.Write(2,IntroContent);
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
						EquipCode = obj.Value;
						break;
					case 2:
						IntroContent = obj.Value;
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
			sb.Append("EquipCode : " + EquipCode + ",");
			sb.Append("IntroContent : " + IntroContent);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
