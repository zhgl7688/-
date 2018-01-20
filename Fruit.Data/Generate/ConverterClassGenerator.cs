using Fruit.Metadata;
using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fruit.Data.Generate
{
    /// <summary>
    /// 用于生成从 DbDataReader 到指定类型的数据处理方法
    /// </summary>
    public static class ConverterClassGenerator
    {
        private static Dictionary<Type, Type> converters = new Dictionary<Type, Type>();

        static ConverterClassGenerator()
        {
            converters.Add(typeof(string), typeof(ConverterDataReaderString));
        }

        public static ConverterDataReader GetConverter(Type modelType)
        {
            if (converters.ContainsKey(modelType))
            {
                return (ConverterDataReader)Activator.CreateInstance(converters[modelType]);
            }

            var modelTypeName = modelType.Name;
            var convertTypeName = modelType.Name + "ConverterDataReader";
            var convertFullTypeName = "Fruit.Data.Generate." + convertTypeName;
            var metaData = FModelMetaDataProvider.GetMetaData(modelType);

            StringBuilder sbCode = new StringBuilder();
            sbCode.AppendLine("using System;");
            sbCode.AppendLine("using System.Data.Common;");
            sbCode.AppendLine("namespace Fruit.Data.Generate {");

            sbCode.AppendLine(string.Format("public class {0} : ConverterDataReader {{", convertTypeName));

            sbCode.AppendLine("public override object Read(DbDataReader reader) {");
            
            sbCode.AppendLine(string.Format("var model = new {0}();", modelTypeName));
            sbCode.AppendLine("int ordinal;");

            foreach(var prop in metaData.Properties){
                if (!prop.CanWrite || prop.HasAttribute<NoColumnAttribute>()) continue;
                sbCode.AppendLine(string.Format("if(fieldOrdinalMap.ContainsKey(\"{0}\")){{", prop.ColumnName));

                sbCode.AppendLine(string.Format("ordinal = fieldOrdinalMap[\"{0}\"];", prop.ColumnName));
                sbCode.AppendLine("if(!reader.IsDBNull(ordinal)){");
                sbCode.AppendLine(string.Format("model.{0} = ({1})reader[ordinal];", prop.Name, ToTypeName(prop.Type)));
                // if not DBNull
                sbCode.AppendLine("}");
                // if ContainsKey
                sbCode.AppendLine("}");
            }

            sbCode.AppendLine("return model;");

            //  object Read(DbDataReader reader)
            sbCode.AppendLine("}");

            // class
            sbCode.AppendLine("}");

            // namespace
            sbCode.AppendLine("}");

            var code = sbCode.ToString();

            CodeDomProvider compiler = new CSharpCodeProvider();
            CompilerParameters options = new CompilerParameters();
            options.GenerateInMemory = true;
            options.GenerateExecutable = false;
            options.OutputAssembly = "Fruit.Data.Generate." + modelType.Name + ".dll";
#if DEBUG
            options.IncludeDebugInformation = true;
#endif
            options.ReferencedAssemblies.Add("System.dll");
            options.ReferencedAssemblies.Add("System.Data.dll");
            options.ReferencedAssemblies.Add("Fruit.Data.dll");
            var result = compiler.CompileAssemblyFromSource(options, code);
            if (result.CompiledAssembly != null)
            {
                converters[modelType] = result.CompiledAssembly.GetType(convertFullTypeName);
                return (ConverterDataReader)Activator.CreateInstance(converters[modelType]);
            }
            throw new CompilerException(result);
        }

        private static string ToTypeName(Type type)
        {
            if (type.Name == "Nullable`1")
            {
//                return type.GenericTypeArguments[0].Name + "?";
                return type.GenericTypeArguments[0].Name;
            }
            return type.Name;
        }
    }

    public abstract class ConverterDataReader
    {
        protected Dictionary<string, int> fieldOrdinalMap = new Dictionary<string, int>();
        public void Init(DbDataReader reader, FModelMetaData metaData)
        {
            int count = reader.FieldCount;
            for (int i = 0; i < count; i++)
            {
                var field = reader.GetName(i);
                var metaProp = metaData.Properties.Find(field);
                if (metaProp != null)
                {
                    fieldOrdinalMap[metaProp.ColumnName] = i;
                }
            }
        }

        public abstract object Read(DbDataReader reader);
    }

    public class ConverterDataReaderString : ConverterDataReader
    {

        public override object Read(DbDataReader reader)
        {
            return reader.GetString(0);
        }
    }
}
