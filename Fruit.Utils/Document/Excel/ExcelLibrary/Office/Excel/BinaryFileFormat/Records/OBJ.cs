﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Fruit.Utils.ExcelLibrary.BinaryFileFormat
{
	public partial class OBJ : Record
	{
		public OBJ(Record record) : base(record) { }

		public OBJ()
		{
			this.Type = RecordType.OBJ;
		}

	}
}
