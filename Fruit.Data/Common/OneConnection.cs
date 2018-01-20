using Fruit.Data;
using Fruit.Data.Generate;
using Fruit.Metadata;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fruit.Data
{
    public abstract class OneConnection : IDisposable
    {
        protected OneConnection()
        {
            MetadataProvider = this as ITableMetadataProvider;
        }

        protected abstract DbConnection OpenConnection(string connectionString);

        public void Open(string connectionString)
        {
            this.Connection = OpenConnection(connectionString);
        }

        public DbConnection Connection { get; private set; }

        public void BeginTransaction()
        {
            if (Transaction != null)
            {
                throw new Exception("不支持多事物嵌套");
            }
            Transaction = Connection.BeginTransaction();
        }

        public void Commit()
        {
            if (Transaction == null)
            {
                throw new Exception("未开始事物");
            }
            Transaction.Commit();
            Transaction = null;
        }

        public void Rollback()
        {
            if (Transaction == null)
            {
                throw new Exception("未开始事物");
            }
            Transaction.Rollback();
            Transaction = null;
        }

        public ITableMetadataProvider MetadataProvider { get; set; }

        public virtual void Dispose()
        {
            if (this.Connection != null)
            {
                this.Connection.Dispose();
            }
        }

        public virtual string ParameterName(string name)
        {
            return "@" + name;
        }

        public abstract DbParameter CreateParameter(QueryParam queryParam);

        public abstract DbCommand CreateCommand(string sql, QueryParams queryParams = null);
        public virtual DbCommand CreateCommand(string sql, params QueryParam[] queryParams)
        {
            return CreateCommand(sql, new QueryParams(queryParams));
        }


        public virtual object ExecuteScalar(string sql, QueryParams queryParams = null)
        {
            var cmd = CreateCommand(sql, queryParams);
            return cmd.ExecuteScalar();
        }
        public virtual object ExecuteScalar(string sql, params QueryParam[] queryParams)
        {
            return ExecuteScalar(sql, new QueryParams(queryParams));
        }

        public virtual int ExecuteNonQuery(string sql, QueryParams queryParams = null)
        {
            var cmd = CreateCommand(sql, queryParams);
            return cmd.ExecuteNonQuery();
        }
        public virtual int ExecuteNonQuery(string sql, params QueryParam[] queryParams)
        {
            return ExecuteNonQuery(sql, new QueryParams(queryParams));
        }

        public abstract DataTable QueryTable(string sql, QueryParams queryParams = null);
        public virtual DataTable QueryTable(string sql, params QueryParam[] queryParams)
        {
            return QueryTable(sql, new QueryParams(queryParams));
        }

        public virtual DbDataReader QueryReader(string sql, QueryParams queryParams = null)
        {
            var cmd = CreateCommand(sql, queryParams);
            return cmd.ExecuteReader();
        }

        public virtual DbDataReader QueryReader(string sql, params QueryParam[] queryParams)
        {
            return QueryReader(sql, new QueryParams(queryParams));
        }

        public int Insert(string table, DataRow row)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sbValues = new StringBuilder();
            QueryParams p = new QueryParams();
            sb.AppendFormat("insert into {0}(", table);
            foreach (DataColumn column in row.Table.Columns)
            {
                if(row.IsNull(column))continue;
                sb.AppendFormat("{0},", column.ColumnName);
                sbValues.AppendFormat("{0},", ParameterName(column.ColumnName));
                p.Add(new QueryParam(ParameterName(column.ColumnName), row[column]));
            }
            sb.Length--;
            sbValues.Length--;
            sb.Append(")values(");
            sb.Append(sbValues.ToString());
            sb.Append(")");
            return ExecuteNonQuery(sb.ToString(), p);
        }

        public int Update(string table, DataRow row, params string[] keyColumns)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sbKeys = new StringBuilder();
            QueryParams p = new QueryParams();
            sb.AppendFormat("update {0} set ", table);
            foreach (DataColumn column in row.Table.Columns)
            {
                if (keyColumns.Contains(column.ColumnName))
                {
                    if (sbKeys.Length > 0)
                    {
                        sbKeys.Append(" and ");
                    }
                    sbKeys.AppendFormat("{0}={1}", column.ColumnName, this.ParameterName(column.ColumnName));
                    p.Add(new QueryParam(ParameterName(column.ColumnName), row[column, DataRowVersion.Original]));
                }
                else
                {
                    sb.AppendFormat("{0}={1},", column.ColumnName, this.ParameterName(column.ColumnName));
                    p.Add(new QueryParam(ParameterName(column.ColumnName), row[column]));
                }
            }
            sb.Length--;
            sb.Append(" where ");
            sb.Append(sbKeys.ToString());
            return ExecuteNonQuery(sb.ToString(), p);
        }

        public virtual ICollection<TModel> Query<TModel>(string sql, QueryParams queryParams = null)
        {
            List<TModel> list = new List<TModel>();
            var converter = ConverterClassGenerator.GetConverter(typeof(TModel));
            using (var reader = QueryReader(sql, queryParams))
            {
                converter.Init(reader, FModelMetaDataProvider.GetMetaData(typeof(TModel)));
                while (reader.Read())
                {
                    list.Add((TModel)converter.Read(reader));
                }
            }
            return list;
        }
        public virtual ICollection<TModel> Query<TModel>(string sql, params QueryParam[] queryParams)
        {
            return Query<TModel>(sql, new QueryParams(queryParams));
        }

        public ObjectType GetType(string name)
        {
            if (MetadataProvider == null)
            {
                throw new Exception("此数据源不支持操作元数据！");
            }
            return MetadataProvider.GetType(name);
        }

        private TableCollection tables;
        public TableCollection Tables
        {
            get
            {
                if (tables == null)
                {
                    if (MetadataProvider == null)
                    {
                        throw new Exception("此数据源不支持操作元数据！");
                    }
                    tables = new TableCollection(MetadataProvider);
                }
                return tables;
            }
        }

        private ViewCollection views;
        public ViewCollection Views
        {
            get
            {
                if (views == null)
                {
                    if (MetadataProvider == null)
                    {
                        throw new Exception("此数据源不支持操作元数据！");
                    }
                    views = new ViewCollection(MetadataProvider);
                }
                return views;
            }
        }

        private StoredProcedureCollection storedProcedures;
        public StoredProcedureCollection StoredProcedures
        {
            get
            {
                if (storedProcedures == null)
                {
                    if (MetadataProvider == null)
                    {
                        throw new Exception("此数据源不支持操作元数据！");
                    }
                    storedProcedures = new StoredProcedureCollection(MetadataProvider);
                }
                return storedProcedures;
            }
        }

        public DbTransaction Transaction { get; set; }
    }
}
