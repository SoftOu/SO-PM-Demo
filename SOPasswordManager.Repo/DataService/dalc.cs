using Microsoft.EntityFrameworkCore;
using SOPasswordManager.Repo.DbEntities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace SOPasswordManager.Repo.DataService
{
    public class dalc : SOPasswordManagerContext
    {
        DbConnection conn;
        // SqlConnection conn;

        public dalc()
        {
            conn = this.Database.GetDbConnection();

            conn.ConnectionString = DBConnectionString.ConnectionString;
        }


        public async Task<List<Dictionary<string, object>>> GetSPData(string spName, SqlParameter[] para)
        {
            DbConnection connection = conn;
            DbCommand cmd = connection.CreateCommand();
            cmd.CommandTimeout = 0;
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = spName;
            cmd.Parameters.AddRange(para);
            DbDataReader dr;
            try
            {
                await conn.OpenAsync();
                dr = await cmd.ExecuteReaderAsync();
                // conn.Close();
                List<Dictionary<string, object>> lst = new List<Dictionary<string, object>>();
                while (await dr.ReadAsync())
                {
                    Dictionary<string, object> obj = new Dictionary<string, object>();
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        obj[dr.GetName(i)] = dr[i];
                    }
                    lst.Add(obj);
                }

                return lst;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                conn.Close();
                cmd.Parameters.Clear();
                cmd.Dispose();
            }
        }

        public async Task<Tuple<int, List<Dictionary<string, object>>>> GetGridDataAsync(string TableName, string ColumnsName, string SortOrder, string SortColumn, System.Nullable<int> PageNumber, System.Nullable<int> RecordPerPage, string WhereClause)
        {

            DbConnection connection = conn;// this.Database.GetDbConnection();
            bool needClose = false;
            if (connection.State != ConnectionState.Open)
            {

                try
                {
                    await connection.OpenAsync();

                    //connection.Open();

                    needClose = true;
                }
                catch (Exception ex)
                {

                }


            }

            try
            {
                using (DbCommand cmd = connection.CreateCommand())
                {
                    //if (this.Database.GetCommandTimeout().HasValue)
                    //    cmd.CommandTimeout = this.Database.GetCommandTimeout().Value;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = @"GetGridData";

                    DbParameter TableNameParameter = cmd.CreateParameter();
                    TableNameParameter.ParameterName = "p_TableName";
                    TableNameParameter.Direction = ParameterDirection.Input;
                    if (TableName != null)
                    {
                        TableNameParameter.Value = TableName;
                    }
                    else
                    {
                        TableNameParameter.DbType = DbType.String;
                        TableNameParameter.Size = -1;
                        TableNameParameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(TableNameParameter);

                    DbParameter ColumnsNameParameter = cmd.CreateParameter();
                    ColumnsNameParameter.ParameterName = "p_ColumnsName";
                    ColumnsNameParameter.Direction = ParameterDirection.Input;
                    if (ColumnsName != null)
                    {
                        ColumnsNameParameter.Value = ColumnsName;
                    }
                    else
                    {
                        ColumnsNameParameter.DbType = DbType.String;
                        ColumnsNameParameter.Size = -1;
                        ColumnsNameParameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(ColumnsNameParameter);

                    DbParameter SortOrderParameter = cmd.CreateParameter();
                    SortOrderParameter.ParameterName = "p_SortOrder";
                    SortOrderParameter.Direction = ParameterDirection.Input;
                    if (SortOrder != null)
                    {
                        SortOrderParameter.Value = SortOrder;
                    }
                    else
                    {
                        SortOrderParameter.DbType = DbType.String;
                        SortOrderParameter.Size = -1;
                        SortOrderParameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(SortOrderParameter);

                    DbParameter SortColumnParameter = cmd.CreateParameter();
                    SortColumnParameter.ParameterName = "p_SortColumn";
                    SortColumnParameter.Direction = ParameterDirection.Input;
                    if (SortColumn != null)
                    {
                        SortColumnParameter.Value = SortColumn;
                    }
                    else
                    {
                        SortColumnParameter.DbType = DbType.String;
                        SortColumnParameter.Size = -1;
                        SortColumnParameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(SortColumnParameter);

                    DbParameter PageNumberParameter = cmd.CreateParameter();
                    PageNumberParameter.ParameterName = "p_PageNumber";
                    PageNumberParameter.Direction = ParameterDirection.Input;
                    if (PageNumber.HasValue)
                    {
                        PageNumberParameter.Value = PageNumber.Value;
                    }
                    else
                    {
                        PageNumberParameter.DbType = DbType.Int32;
                        PageNumberParameter.Size = -1;
                        PageNumberParameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(PageNumberParameter);

                    DbParameter RecordPerPageParameter = cmd.CreateParameter();
                    RecordPerPageParameter.ParameterName = "p_RecordPerPage";
                    RecordPerPageParameter.Direction = ParameterDirection.Input;
                    if (RecordPerPage.HasValue)
                    {
                        RecordPerPageParameter.Value = RecordPerPage.Value;
                    }
                    else
                    {
                        RecordPerPageParameter.DbType = DbType.Int32;
                        RecordPerPageParameter.Size = -1;
                        RecordPerPageParameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(RecordPerPageParameter);

                    DbParameter WhereClauseParameter = cmd.CreateParameter();
                    WhereClauseParameter.ParameterName = "p_WhereClause";
                    WhereClauseParameter.Direction = ParameterDirection.Input;
                    WhereClauseParameter.Value = !string.IsNullOrEmpty(WhereClause) ? WhereClause : "1=1"; 
                    cmd.Parameters.Add(WhereClauseParameter);

                    //When YES: it will retrieve just the number of rows
                    //When NO: it will fetch all records
                    DbParameter justNumberRows = cmd.CreateParameter();
                    justNumberRows.ParameterName = "p_JustNumberRows";
                    justNumberRows.Direction = ParameterDirection.Input;
                    justNumberRows.DbType = DbType.Boolean;
                    justNumberRows.Value = true;

                    cmd.Parameters.Add(justNumberRows);


                    //await cmd.ExecuteNonQueryAsync();
                    try
                    {
                        DbDataReader dr;
                        // await conn.OpenAsync();
                        //First call to the stored procedure to retrieve the total number of rows
                        int total = 0;
                        using (dr = await cmd.ExecuteReaderAsync())
                        {
                            while (await dr.ReadAsync())
                            {
                                total = Convert.ToInt32(dr[0]);
                            }
                        }

                        //Second call to fetch the records
                        justNumberRows.Value = false;
                        dr = await cmd.ExecuteReaderAsync();
                        
                        List<Dictionary<string, object>> lst = new List<Dictionary<string, object>>();

                        if(total > 0)
                            while (await dr.ReadAsync())
                            {
                                //dr.Read();
                                //while (lst.Count < RecordPerPage)
                                if (lst.Count < RecordPerPage)
                                {
                                    Dictionary<string, object> obj = new Dictionary<string, object>();
                                    for (int i = 0; i < dr.FieldCount; i++)
                                    {
                                        //if (total == 0 && dr.GetName(i).ToLower() == "totalrows")
                                        //{
                                        //    total = Convert.ToInt32(dr[i]);
                                        //}
                                        obj[dr.GetName(i)] = dr[i].GetType().Name == "DBNull" ? null : dr[i];
                                    }

                                    lst.Add(obj);
                                }
                                else
                                {
                                    //break;
                                }
                            }

                        //return  lst;
                        return new Tuple<int, List<Dictionary<string, object>>>(total, lst);
                        //return new Tuple<int, List<Dictionary<string, object>>>(lst.Count, lst);
                    }
                    catch (Exception ex)
                    {
                        return new Tuple<int, List<Dictionary<string, object>>>(0, null);
                    }

                }
            }
            finally
            {
                if (needClose)
                    connection.Close();
            }
        }

        //public async Task<Tuple<int, List<Dictionary<string, object>>>> GetGridDataAsync(string TableName, string ColumnsName, string SortOrder, string SortColumn, System.Nullable<int> PageNumber, System.Nullable<int> RecordPerPage, string WhereClause)
        //{

        //    DbConnection connection = conn;// this.Database.GetDbConnection();
        //    bool needClose = false;
        //    if (connection.State != ConnectionState.Open)
        //    {

        //        try
        //        {
        //            await connection.OpenAsync();

        //            //connection.Open();

        //            needClose = true;
        //        }
        //        catch (Exception ex)
        //        {

        //        }


        //    }

        //    try
        //    {
        //        using (DbCommand cmd = connection.CreateCommand())
        //        {
        //            //if (this.Database.GetCommandTimeout().HasValue)
        //            //    cmd.CommandTimeout = this.Database.GetCommandTimeout().Value;
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.CommandText = @"GetGridData";

        //            DbParameter TableNameParameter = cmd.CreateParameter();
        //            TableNameParameter.ParameterName = "p_TableName";
        //            TableNameParameter.Direction = ParameterDirection.Input;
        //            if (TableName != null)
        //            {
        //                TableNameParameter.Value = TableName;
        //            }
        //            else
        //            {
        //                TableNameParameter.DbType = DbType.String;
        //                TableNameParameter.Size = -1;
        //                TableNameParameter.Value = DBNull.Value;
        //            }
        //            cmd.Parameters.Add(TableNameParameter);

        //            DbParameter ColumnsNameParameter = cmd.CreateParameter();
        //            ColumnsNameParameter.ParameterName = "p_ColumnsName";
        //            ColumnsNameParameter.Direction = ParameterDirection.Input;
        //            if (ColumnsName != null)
        //            {
        //                ColumnsNameParameter.Value = ColumnsName;
        //            }
        //            else
        //            {
        //                ColumnsNameParameter.DbType = DbType.String;
        //                ColumnsNameParameter.Size = -1;
        //                ColumnsNameParameter.Value = DBNull.Value;
        //            }
        //            cmd.Parameters.Add(ColumnsNameParameter);

        //            DbParameter SortOrderParameter = cmd.CreateParameter();
        //            SortOrderParameter.ParameterName = "p_SortOrder";
        //            SortOrderParameter.Direction = ParameterDirection.Input;
        //            if (SortOrder != null)
        //            {
        //                SortOrderParameter.Value = SortOrder;
        //            }
        //            else
        //            {
        //                SortOrderParameter.DbType = DbType.String;
        //                SortOrderParameter.Size = -1;
        //                SortOrderParameter.Value = DBNull.Value;
        //            }
        //            cmd.Parameters.Add(SortOrderParameter);

        //            DbParameter SortColumnParameter = cmd.CreateParameter();
        //            SortColumnParameter.ParameterName = "p_SortColumn";
        //            SortColumnParameter.Direction = ParameterDirection.Input;
        //            if (SortColumn != null)
        //            {
        //                SortColumnParameter.Value = SortColumn;
        //            }
        //            else
        //            {
        //                SortColumnParameter.DbType = DbType.String;
        //                SortColumnParameter.Size = -1;
        //                SortColumnParameter.Value = DBNull.Value;
        //            }
        //            cmd.Parameters.Add(SortColumnParameter);

        //            DbParameter PageNumberParameter = cmd.CreateParameter();
        //            PageNumberParameter.ParameterName = "p_PageNumber";
        //            PageNumberParameter.Direction = ParameterDirection.Input;
        //            if (PageNumber.HasValue)
        //            {
        //                PageNumberParameter.Value = PageNumber.Value;
        //            }
        //            else
        //            {
        //                PageNumberParameter.DbType = DbType.Int32;
        //                PageNumberParameter.Size = -1;
        //                PageNumberParameter.Value = DBNull.Value;
        //            }
        //            cmd.Parameters.Add(PageNumberParameter);

        //            DbParameter RecordPerPageParameter = cmd.CreateParameter();
        //            RecordPerPageParameter.ParameterName = "p_RecordPerPage";
        //            RecordPerPageParameter.Direction = ParameterDirection.Input;
        //            if (RecordPerPage.HasValue)
        //            {
        //                RecordPerPageParameter.Value = RecordPerPage.Value;
        //            }
        //            else
        //            {
        //                RecordPerPageParameter.DbType = DbType.Int32;
        //                RecordPerPageParameter.Size = -1;
        //                RecordPerPageParameter.Value = DBNull.Value;
        //            }
        //            cmd.Parameters.Add(RecordPerPageParameter);

        //            DbParameter WhereClauseParameter = cmd.CreateParameter();
        //            WhereClauseParameter.ParameterName = "p_WhereClause";
        //            WhereClauseParameter.Direction = ParameterDirection.Input;
        //            if (WhereClause != null)
        //            {
        //                WhereClauseParameter.Value = WhereClause;
        //            }
        //            else
        //            {
        //                WhereClauseParameter.DbType = DbType.String;
        //                WhereClauseParameter.Size = -1;
        //                WhereClauseParameter.Value = DBNull.Value;
        //            }
        //            cmd.Parameters.Add(WhereClauseParameter);

        //            //OUTPUT PARAMETER
        //            DbParameter totalRowsParameter = cmd.CreateParameter();
        //            totalRowsParameter.ParameterName = "p_TotalRows";
        //            totalRowsParameter.Direction = ParameterDirection.Output;
        //            totalRowsParameter.DbType = DbType.Int32;

        //            cmd.Parameters.Add(totalRowsParameter);


        //            //await cmd.ExecuteNonQueryAsync();
        //            try
        //            {
        //                DbDataReader dr;
        //                // await conn.OpenAsync();
        //                dr = await cmd.ExecuteReaderAsync();
        //                // conn.Close();
        //                List<Dictionary<string, object>> lst = new List<Dictionary<string, object>>();
        //                int total = 0;
        //                while (await dr.ReadAsync())
        //                {
        //                    Dictionary<string, object> obj = new Dictionary<string, object>();
        //                    for (int i = 0; i < dr.FieldCount; i++)
        //                    {
        //                        if (total == 0 && dr.GetName(i).ToLower() == "totalrows")
        //                        {
        //                            total = Convert.ToInt32(dr[i]);
        //                        }
        //                        obj[dr.GetName(i)] = dr[i].GetType().Name == "DBNull" ? null : dr[i];
        //                    }
        //                    lst.Add(obj);
        //                }
        //                //return  lst;
        //                //return new Tuple<int, List<Dictionary<string, object>>>(total, lst);
        //                return new Tuple<int, List<Dictionary<string, object>>>(lst.Count, lst);
        //            }
        //            catch (Exception ex)
        //            {
        //                return new Tuple<int, List<Dictionary<string, object>>>(0, null);
        //            }

        //        }
        //    }
        //    finally
        //    {
        //        if (needClose)
        //            connection.Close();
        //    }
        //}

        public async Task<DataTable> GetExportGridDataAsync(string TableName, string ColumnsName, string SortOrder, string SortColumn, System.Nullable<int> PageNumber, System.Nullable<int> RecordPerPage, string WhereClause)
        {
            DbConnection connection = conn;// this.Database.GetDbConnection();
            bool needClose = false;
            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync();
                needClose = true;
            }

            try
            {
                using (DbCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = @"GetGridData";

                    DbParameter TableNameParameter = cmd.CreateParameter();
                    TableNameParameter.ParameterName = "TableName";
                    TableNameParameter.Direction = ParameterDirection.Input;
                    if (TableName != null)
                    {
                        TableNameParameter.Value = TableName;
                    }
                    else
                    {
                        TableNameParameter.DbType = DbType.String;
                        TableNameParameter.Size = -1;
                        TableNameParameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(TableNameParameter);

                    DbParameter ColumnsNameParameter = cmd.CreateParameter();
                    ColumnsNameParameter.ParameterName = "ColumnsName";
                    ColumnsNameParameter.Direction = ParameterDirection.Input;
                    if (ColumnsName != null)
                    {
                        ColumnsNameParameter.Value = ColumnsName;
                    }
                    else
                    {
                        ColumnsNameParameter.DbType = DbType.String;
                        ColumnsNameParameter.Size = -1;
                        ColumnsNameParameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(ColumnsNameParameter);

                    DbParameter SortOrderParameter = cmd.CreateParameter();
                    SortOrderParameter.ParameterName = "SortOrder";
                    SortOrderParameter.Direction = ParameterDirection.Input;
                    if (SortOrder != null)
                    {
                        SortOrderParameter.Value = SortOrder;
                    }
                    else
                    {
                        SortOrderParameter.DbType = DbType.String;
                        SortOrderParameter.Size = -1;
                        SortOrderParameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(SortOrderParameter);

                    DbParameter SortColumnParameter = cmd.CreateParameter();
                    SortColumnParameter.ParameterName = "SortColumn";
                    SortColumnParameter.Direction = ParameterDirection.Input;
                    if (SortColumn != null)
                    {
                        SortColumnParameter.Value = SortColumn;
                    }
                    else
                    {
                        SortColumnParameter.DbType = DbType.String;
                        SortColumnParameter.Size = -1;
                        SortColumnParameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(SortColumnParameter);

                    DbParameter PageNumberParameter = cmd.CreateParameter();
                    PageNumberParameter.ParameterName = "PageNumber";
                    PageNumberParameter.Direction = ParameterDirection.Input;
                    if (PageNumber.HasValue)
                    {
                        PageNumberParameter.Value = PageNumber.Value;
                    }
                    else
                    {
                        PageNumberParameter.DbType = DbType.Int32;
                        PageNumberParameter.Size = -1;
                        PageNumberParameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(PageNumberParameter);

                    DbParameter RecordPerPageParameter = cmd.CreateParameter();
                    RecordPerPageParameter.ParameterName = "RecordPerPage";
                    RecordPerPageParameter.Direction = ParameterDirection.Input;
                    if (RecordPerPage.HasValue)
                    {
                        RecordPerPageParameter.Value = RecordPerPage.Value;
                    }
                    else
                    {
                        RecordPerPageParameter.DbType = DbType.Int32;
                        RecordPerPageParameter.Size = -1;
                        RecordPerPageParameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(RecordPerPageParameter);

                    DbParameter WhereClauseParameter = cmd.CreateParameter();
                    WhereClauseParameter.ParameterName = "WhereClause";
                    WhereClauseParameter.Direction = ParameterDirection.Input;
                    if (WhereClause != null)
                    {
                        WhereClauseParameter.Value = WhereClause;
                    }
                    else
                    {
                        WhereClauseParameter.DbType = DbType.String;
                        WhereClauseParameter.Size = -1;
                        WhereClauseParameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(WhereClauseParameter);
                    //await cmd.ExecuteNonQueryAsync();
                    DataTable dt = new DataTable();
                    DbDataReader dr;
                    connection.Close();
                    await conn.OpenAsync();
                    dr = await cmd.ExecuteReaderAsync(CommandBehavior.Default);
                    // conn.Close();
                    dt.Load(dr);
                    return dt;
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (needClose)
                    connection.Close();
            }
        }

        
    }
}
