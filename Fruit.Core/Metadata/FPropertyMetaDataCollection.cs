using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fruit.Metadata
{
    /// <summary>
    /// 提供放置模型属性集合的对象
    /// </summary>
    public class FPropertyMetaDataCollection : IReadOnlyCollection<FPropertyMetaData>, IReadOnlyDictionary<string, FPropertyMetaData>
    {
        private List<FPropertyMetaData> mPropertiesList = new List<FPropertyMetaData>();
        private Dictionary<String, FPropertyMetaData> mPropertiesDictionary = new Dictionary<string, FPropertyMetaData>();

        internal FPropertyMetaDataCollection(FModelMetaData owner)
        {
            this.ModelMetaData = owner;
        }

        internal void Add(FPropertyMetaData propertyMetaData)
        {
            mPropertiesList.Add(propertyMetaData);
            mPropertiesDictionary.Add(propertyMetaData.Name, propertyMetaData);
        }

        /// <summary>
        /// 属性总数
        /// </summary>
        public int Count
        {
            get { return mPropertiesList.Count; }
        }

        public IEnumerator<FPropertyMetaData> GetEnumerator()
        {
            return mPropertiesList.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool ContainsKey(string key)
        {
            return mPropertiesDictionary.ContainsKey(key);
        }

        public IEnumerable<string> Keys
        {
            get { return mPropertiesDictionary.Keys; }
        }

        public bool TryGetValue(string key, out FPropertyMetaData value)
        {
            return mPropertiesDictionary.TryGetValue(key, out value);
        }

        public IEnumerable<FPropertyMetaData> Values
        {
            get { return mPropertiesDictionary.Values; }
        }

        public FPropertyMetaData this[int index]
        {
            get { return mPropertiesList[index]; }
        }

        public FPropertyMetaData this[string key]
        {
            get {
                if (mPropertiesDictionary.ContainsKey(key))
                {
                    return mPropertiesDictionary[key];
                }
                return null;
            }
        }

        public FPropertyMetaData Find(string name)
        {
            foreach (var pv in mPropertiesDictionary) {
                if (pv.Key.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    return pv.Value;
                }
            }
            return null;
        }

        IEnumerator<KeyValuePair<string, FPropertyMetaData>> IEnumerable<KeyValuePair<string, FPropertyMetaData>>.GetEnumerator()
        {
            return mPropertiesDictionary.GetEnumerator();
        }

        public FModelMetaData ModelMetaData { get; set; }
    }
}
