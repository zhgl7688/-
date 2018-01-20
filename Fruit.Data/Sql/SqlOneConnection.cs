using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fruit.Data.Sql
{
    class SqlOneConnection : OneConnection, ITableMetadataProvider
    {
        protected override System.Data.Common.DbConnection OpenConnection(string connectionString)
        {
            var conn = new SqlConnection(connectionString);
            conn.Open();
            return conn;
        }

        public override System.Data.DataTable QueryTable(string sql, QueryParams queryParams)
        {
            var cmd = CreateCommand(sql, queryParams);
            var apt = new SqlDataAdapter((SqlCommand)cmd);
            var dataTable = new DataTable();
            apt.Fill(dataTable);
            return dataTable;
        }

        public override System.Data.Common.DbCommand CreateCommand(string sql, QueryParams queryParams)
        {
            var cmd = Connection.CreateCommand();
            cmd.Transaction = Transaction;
            cmd.CommandText = sql;
            if (queryParams != null)
            {
                foreach (var queryParam in queryParams)
                {
                    cmd.Parameters.Add(CreateParameter(queryParam));
                }
            }
            return cmd;
        }

        public override DbParameter CreateParameter(QueryParam queryParam)
        {
            return new SqlParameter(queryParam.Name, queryParam.Value);
        }

        public string[] GetTableNames()
        {
            return Query<string>("select name from sys.tables order by name").ToArray();
        }

        public string[] GetViewNames()
        {
            return Query<string>("select name from sys.views order by name").ToArray();
        }

        public string[] GetTableTypes()
        {
            return Query<string>("select name from sys.table_types order by name").ToArray();
        }

        public string[] GetStoredProcedures()
        {
            return Query<string>("select name from sys.procedures order by name").ToArray();
        }

        ObjectType ITableMetadataProvider.GetType(string name)
        {
            var strType = Query<string>("select type from sys.objects where name = @name", new QueryParam("@name", name)).FirstOrDefault();
            if (strType == null)
                return ObjectType.None;
            switch (strType)
            {
                case "U":
                    return ObjectType.Table;
                case "V":
                    return ObjectType.View;
                case "P":
                    return ObjectType.StoredProcedures;
                default:
                    return ObjectType.Other;
            }
        }

        public Field[] GetTableFields(string table)
        {
            return Query<Field>(@"select c.column_id [index], c.name, t.name fieldType, c.max_length, c.precision, c.scale, c.is_nullable nullable from sys.all_columns c
left join sys.types t on c.system_type_id = t.system_type_id and c.user_type_id = t.user_type_id
where object_id = object_id(@table) order by column_id", new QueryParam("@table", table)).ToArray();
        }

        public Field[] GetTableTypeFields(string tableType)
        {
            return Query<Field>(@"select c.column_id [index], c.name, t.name fieldType, c.max_length, c.precision, c.scale, c.is_nullable nullable from sys.all_columns c
left join sys.types t on c.system_type_id = t.system_type_id and c.user_type_id = t.user_type_id
where object_id = (select top 1 type_table_object_id from sys.table_types where name=@tableType)) order by column_id", new QueryParam("@tableType", tableType)).ToArray();
        }


        public SpParameter[] GetStoredProcedureParameters(string storedProcedure)
        {
            return Query<SpParameter>(@"select name ParameterName, TYPE_NAME(system_type_id) ParameterType, max_length MaxLength, is_output IsOutput from sys.parameters where object_id = OBJECT_ID(@name) order by parameter_id", new QueryParam("@name", storedProcedure)).ToArray();
        }
    }
}
