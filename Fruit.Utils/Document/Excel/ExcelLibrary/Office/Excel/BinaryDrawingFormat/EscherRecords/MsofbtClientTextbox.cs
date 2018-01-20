﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Fruit.Utils.ExcelLibrary.BinaryDrawingFormat
{
	public partial class MsofbtClientTextbox : EscherRecord
	{
		public MsofbtClientTextbox(EscherRecord record) : base(record) { }

		public MsofbtClientTextbox()
		{
			this.Type = EscherRecordType.MsofbtClientTextbox;
		}

	}
}
