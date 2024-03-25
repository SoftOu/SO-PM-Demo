using iTextSharp.text;
using iTextSharp.text.pdf;
using SOPasswordManager.Repo.DataService;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOPasswordManager.Repo.GridService
{
    public class GridFunctions
    {
        public string TableName { get; set; }
        public string ColumnsName { get; set; }
        public string SortColumn { get; set; }
        public string SortOrder { get; set; }
        public int PageNumber { get; set; }
        public int RecordPerPage { get; set; }
        public string WhereClause { get; set; }
        //public Task<Gdata> JsonData { get; set; }
        public string ExportedColumns { get; set; }
        public string ExportedFileName { get; set; }
        public string Mode { get; set; }
        public NameValueCollection HttpContext { get; set; }
        public GridFunctions()
        {
            HttpContext = new NameValueCollection();
        }
        public GridFunctions(NameValueCollection qry)
        {
            HttpContext = qry;
        }
        public string GetColumns()
        {
            string columns = "";
            for (int i = 0; ; i++)
            {
                if (!string.IsNullOrEmpty(HttpContext["columns[" + i + "][data]"]))
                {
                    if (Convert.ToBoolean(HttpContext["columns[" + i + "][searchable]"]))
                    {
                        if (string.IsNullOrEmpty(Convert.ToString(HttpContext["columns[" + i + "][name]"])))
                        {
                            string c = Convert.ToString(HttpContext["columns[" + i + "][data]"]);
                            columns += columns == "" ? c : "," + c;
                        }
                        else
                        {
                            string columnName = Convert.ToString(HttpContext["columns[" + i + "][name]"]);
                            columns += columns == "" ? columnName : "," + columnName;
                        }
                    }
                }
                else
                    break;
            }
            if (!string.IsNullOrEmpty(HttpContext["Columns"]))
                columns = Convert.ToString(HttpContext["Columns"]);
            return columns;
        }

        public string GetSortColumn(string defaultColName)
        {

            //return defaultColName;
            if (!string.IsNullOrEmpty(HttpContext["order[0][column]"]))
            {
                string index = Convert.ToString(HttpContext["order[0][column]"]);
                string ColName = Convert.ToString(HttpContext["columns[" + index + "][data]"]);
                if (string.IsNullOrEmpty(ColName))
                    ColName = defaultColName;
                return ColName;
            }
            else
                return defaultColName;
        }

        public string GetSortOrder(string defaultColName)
        {

            //return defaultColName;
            if (!string.IsNullOrEmpty(HttpContext["order[0][dir]"]))
            {
                string order = Convert.ToString(HttpContext["order[0][dir]"]);
                if (string.IsNullOrEmpty(order))
                    order = "asc";
                return order;
            }
            else
                return "asc";
        }

        public string GetWhereClause(string w = "")
        {
            string where = w;
            if (!string.IsNullOrEmpty(HttpContext["FixClause"]))
            {
                string fix = Convert.ToString(HttpContext["FixClause"]);
                if (fix != "")
                    where += where == "" ? fix : " AND " + fix;
            }
            if (!string.IsNullOrEmpty(HttpContext["search[value]"]))
            {
                string val = Convert.ToString(HttpContext["search[value]"]);
                if (val != "")
                {
                    string whereforall = "";
                    string[] columns = GetColumns().Split(',');
                    foreach (string col in columns)
                    {
                        if (!string.IsNullOrEmpty(col) && col.ToLower() != "rownumber" && col.ToLower() != "image" && col.ToLower() != "id_image")
                            whereforall += whereforall == "" ? col + " LIKE N'%" + val + "%'" : " OR " + col + " LIKE N'%" + val + "%'";
                    }

                    where += where == "" ? "(" + whereforall + ")" : " AND (" + whereforall + ")";
                }
            }
            return where;
        }

        public int GetPageNumber(int Pageno)
        {
            //return Pageno;
            if (!string.IsNullOrEmpty(HttpContext["start"]))
                return Convert.ToInt32(Convert.ToString(HttpContext["start"]));
            else
                return 1;
        }

        public int GetRecordPerPage()
        {
            if (!string.IsNullOrEmpty(HttpContext["length"]))
                return Convert.ToInt32(Convert.ToString(HttpContext["length"]));
            else
                return 10;
        }

        public SqlParameter[] GenerateParamsForSp()
        {
            SqlParameter[] para = new SqlParameter[7];
            para[0] = new SqlParameter().CreateParameter("@TableName", this.TableName, -1);
            para[1] = new SqlParameter().CreateParameter("@ColumnsName", this.ColumnsName, -1);
            para[2] = new SqlParameter().CreateParameter("@SortOrder", GetSortOrder(this.SortOrder));
            para[3] = new SqlParameter().CreateParameter("@SortColumn", GetSortColumn(this.SortColumn));
            para[4] = new SqlParameter().CreateParameter("@PageNumber", GetPageNumber(this.PageNumber));
            para[5] = new SqlParameter().CreateParameter("@RecordPerPage", GetRecordPerPage());
            para[6] = new SqlParameter().CreateParameter("@WhereClause", GetWhereClause(this.WhereClause), -1);
            return para;
        }


        private async Task<Tuple<int, List<Dictionary<string, object>>>> GetDataTable()
        {
            try
            {
                Tuple<int, List<Dictionary<string, object>>> dt = await new dalc().GetGridDataAsync(this.TableName, this.ColumnsName, GetSortOrder(this.SortOrder), GetSortColumn(this.SortColumn), GetPageNumber(this.PageNumber), GetRecordPerPage(), GetWhereClause(this.WhereClause));

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<GridDTO> GetJson()
        {
            var dt = await GetDataTable();
            GridDTO obj = new GridDTO();
            obj.data = dt.Item2;
            obj.draw = HttpContext["draw"];
            obj.recordsFiltered = dt.Item1.ToString();// dt.Rows.Count == 0 ? "0" : dt.Rows[0]["TotalRows"].ToString();
            obj.recordsTotal = dt.Item1.ToString();// dt.Rows.Count == 0 ? "0" : dt.Rows[0]["TotalRows"].ToString();
                                                   // return new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(obj);
            return obj;//  dt.GetJsonForDataTableJS();
        }

        public async Task<byte[]> Export()
        {
            //DataTable dt =await GetDataTable(oGrid);
            DataTable dt = await new dalc().GetExportGridDataAsync(this.TableName, this.ColumnsName, GetSortOrder(this.SortOrder), GetSortColumn(this.SortColumn), GetPageNumber(this.PageNumber), GetRecordPerPage(), GetWhereClause(this.WhereClause));
            this.ExportedColumns = Convert.ToString(HttpContext["Columns"]);
            if (this.ExportedColumns != null)
            {
                List<string> c = this.ExportedColumns.Split(',').ToList();
                string[] s = this.ExportedColumns.Split(',');
                for (int i = 0; i < c.Count; i++)
                {
                    c[i] = c[i].Split(' ')[0].ToString();
                }

                DataTable dtTemp = dt.Copy();
                int j = 0;
                foreach (DataColumn dc in dtTemp.Columns)
                {
                    if (!c.Contains(dc.ColumnName))
                        dt.Columns.Remove(dc.ColumnName);
                    else
                    {
                        dt.Columns[s[j].Split(' ')[0].ToString()].SetOrdinal(j);
                        dt.Columns[s[j].Split(' ')[0].ToString()].ColumnName = s[j].Split('[').Length > 1 ? s[j].Split('[')[1].Replace("]", "").ToString() : s[j];
                        j++;
                    }
                }

            }
            if (dt.Columns.Contains("TotalRows"))
                dt.Columns.Remove("TotalRows");



            if (HttpContext["type"] == "excel")
            {
                return ExportToExcel1(dt);
            }
            if (HttpContext["type"] == "pdf")
            {
                return ExportPDF(dt);

            }
            if (HttpContext["type"] == "csv")
            {
                return ExportTocsv(dt);
            }

            return null;

        }
        public byte[] ExportTocsv(DataTable dt)
        {
            byte[] outputBuffer = null;

            using (MemoryStream tempStream = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(tempStream))
                {
                    Rfc4180Writer.WriteDataTable(dt, writer, true);
                }

                outputBuffer = tempStream.ToArray();
            }

            return outputBuffer;
        }

        public byte[] ExportToExcel1(DataTable sourceTable)
        {


            byte[] outputBuffer = null;

            using (MemoryStream tempStream = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(tempStream))
                {
                    Rfc4180Writer.WriteDataTable(sourceTable, writer, true);
                }

                outputBuffer = tempStream.ToArray();
            }

            return outputBuffer;
        }

        public static class Rfc4180Writer
        {
            public static void WriteDataTable(DataTable sourceTable, TextWriter writer, bool includeHeaders)
            {
                if (includeHeaders)
                {
                    IEnumerable<String> headerValues = sourceTable.Columns
                        .OfType<DataColumn>()
                        .Select(column => QuoteValue(column.ColumnName));

                    writer.WriteLine(String.Join(",", headerValues));
                }

                IEnumerable<String> items = null;

                foreach (DataRow row in sourceTable.Rows)
                {
                    items = row.ItemArray.Select(o => QuoteValue(o?.ToString() ?? String.Empty));
                    writer.WriteLine(String.Join(",", items));
                }
                writer.Flush();
            }
            private static string QuoteValue(string value)
            {
                return String.Concat("\"",
                value.Replace("\"", "\"\""), "\"");
            }
        }




        private byte[] ExportPDF(DataTable dt)
        {
            string heading = "PDF";
            var document = new iTextSharp.text.Document();
            var outputMS = new MemoryStream();
            var writer = PdfWriter.GetInstance(document, outputMS);
            document.Open();
            var font5 = FontFactory.GetFont(FontFactory.HELVETICA, 11);

            document.Add(new Phrase(Environment.NewLine));


            var count = dt.Columns.Count; //  columnsHeader.Count;
            var table = new PdfPTable(count);
            float[] widths = new float[] { 2f, 4f, 5f, 4f, 4f };

            //table.SetWidths(widths);

            table.WidthPercentage = 100;
            var cell = new PdfPCell(new Phrase(heading));
            cell.Colspan = count;

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                var headerCell = new PdfPCell(new Phrase(dt.Columns[i].ToString(), font5));
                headerCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                table.AddCell(headerCell);
            }



            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    table.AddCell(new Phrase(dt.Rows[i][dt.Columns[j]].ToString(), font5));
                }
            }


            document.Add(table);
            document.Close();
            var result = outputMS.ToArray();

            return result;
        }


    }
}
