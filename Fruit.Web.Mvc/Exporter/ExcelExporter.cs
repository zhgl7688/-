using Fruit.Utils.NPOI.HPSF;
using Fruit.Utils.NPOI.HSSF.UserModel;
using Fruit.Utils.NPOI.SS.UserModel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fruit.Web
{
    class ExcelExporter : IDataExporter
    {
        private const int TOTAL_PRE_SHEET = 60000;

        private ExporterSetting setting;
        private int RowCount;
        private int ColumnCount;
        private SortedDictionary<int, string> DataColumns;
        public void Init(ExporterSetting setting)
        {
            this.setting = setting;
            // 计算列头使用总行、列数、列数据对应属性
            this.RowCount = setting.columns.Count;
            for (int r = 0; r < setting.columns.Count; r++)
            {
                int columnCount = 0;
                var rowColumns = setting.columns[r];
                for (int c = 0; c < rowColumns.Count; c++)
                {
                    columnCount += rowColumns[c].colspan;
                }
                this.ColumnCount = Math.Max(this.ColumnCount, columnCount);
            }

            DataColumns = new SortedDictionary<int, string>();
            for (int c = 0; c < ColumnCount; c++)
            {
                for (int r = 0; r < setting.columns.Count; r++)
                {
                    if (setting.columns[r].Count > c && !string.IsNullOrEmpty(setting.columns[r][c].field))
                    {
                        DataColumns.Add(c, setting.columns[r][c].field);
                        break;
                    }
                }
            }
        }

        public void Export(IEnumerable data, System.IO.Stream outStream)
        {
            var workbook = new HSSFWorkbook();
            DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = "上海水蜜桃软件科技有限公司";
            workbook.DocumentSummaryInformation = dsi;

            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            si.Subject = setting.title;
            workbook.SummaryInformation = si;

            ISheet sheet = null;

            int total = 0;
            int rowIndex = 0;

            if (data is JObject)
            {
                foreach (JObject model in ((JObject)data)["rows"])
                {
                    if (total % TOTAL_PRE_SHEET == 0)
                    {
                        sheet = CreateSheet(workbook);
                        rowIndex = RowCount;
                    }
                    ExportRow(model, sheet, rowIndex++);
                    total++;
                }
            }
            else
            {
                foreach (object model in data)
                {
                    if (total % TOTAL_PRE_SHEET == 0)
                    {
                        sheet = CreateSheet(workbook);
                        rowIndex = RowCount;
                    }
                    ExportRow(model, sheet, rowIndex++);
                    total++;
                }
            }
            workbook.Write(outStream);
        }

        private void ExportRow(object model, ISheet sheet, int rowIndex)
        {
            var row = sheet.CreateRow(rowIndex);
            foreach (var kv in DataColumns)
            {
                var prop = model.GetType().GetProperty(kv.Value);
                if (prop == null) continue;
                var cell = row.CreateCell(kv.Key);
                var rprop = model.GetType().GetProperty(kv.Value + "_RefText");
                if (rprop != null)
                {
                    cell.SetCellValue(Convert.ToString(rprop.GetValue(model)));
                }
                else
                {
                    cell.SetCellValue(Convert.ToString(prop.GetValue(model)));
                }
            }
        }

        private void ExportRow(JObject model, ISheet sheet, int rowIndex)
        {
            var row = sheet.CreateRow(rowIndex);
            foreach (var kv in DataColumns)
            {
                var prop = model[kv.Value];
                if (prop == null) continue;
                var cell = row.CreateCell(kv.Key);
                cell.SetCellValue(Convert.ToString(prop));
            }
        }

        private ISheet CreateSheet(HSSFWorkbook workbook)
        {
            var sheet = workbook.CreateSheet();
            for (int r = 0; r < setting.columns.Count; r++)
            {
                var row = sheet.CreateRow(r);
                var rowColumns = setting.columns[r];
                int columnCount = 0;
                for (int c = 0; c < rowColumns.Count; c++)
                {
                    var column = rowColumns[c];
                    if (column.rowspan > 1 || column.colspan > 1)
                    {
                        // 需要合并
                        sheet.AddMergedRegion(new Utils.NPOI.SS.Util.CellRangeAddress(r, r + column.rowspan - 1, columnCount, columnCount + column.colspan - 1));
                    }
                    var cell = row.CreateCell(columnCount);
                    cell.SetCellValue(rowColumns[c].title);


                    columnCount += column.colspan;
                }
            }

            return sheet;
        }
    }
}
