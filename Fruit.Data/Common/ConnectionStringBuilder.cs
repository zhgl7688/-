using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fruit.Data
{
    class ConnectionStringBuilder : NameValueCollection
    {
        public ConnectionStringBuilder(string connectionString)
        {
            var reader = new StringReader(connectionString);

            bool readValue = false;
            string name = null;
            int p = -1;
            while ((p = reader.Peek()) > -1)
            {
                if (Char.IsWhiteSpace((char)p))
                {
                    reader.Read();
                    continue;
                }
                if (p == '=')
                {
                    reader.Read();
                    readValue = true;
                    continue;
                }
                if(p == ';')
                {
                    reader.Read();
                    readValue = false;
                    continue;
                }
                if (readValue)
                {
                    this[name] = ReadValue(reader);
                }
                else
                {
                    name = ReadName(reader);
                }
            }
        }

        private string ReadName(StringReader reader)
        {
            StringBuilder sb = new StringBuilder();
            int p = -1;
            while ((p = reader.Peek()) > -1)
            {
                if (p == '=')
                    break;
                sb.Append((char)reader.Read());
            }
            return sb.ToString();
        }

        private string ReadValue(StringReader reader)
        {
            bool strLevel = false;
            StringBuilder sb = new StringBuilder();
            int p = -1;
            while ((p = reader.Peek()) > -1)
            {
                if (p == ';' && !strLevel)
                    break;
                if (p == '"')
                {
                    reader.Read();
                    strLevel = !strLevel;
                    continue;
                }
                sb.Append((char)reader.Read());
            }
            return sb.ToString();
        }
    }
}
