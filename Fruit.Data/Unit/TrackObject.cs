using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fruit.Data
{
    public class TrackObject : ObservableObject
    {
        protected Dictionary<string, TrackInfo> values = new Dictionary<string, TrackInfo>();

        protected object Get(string name)
        {
            if (values.ContainsKey(name))
            {
                return values[name].Value;
            }
            return null;
        }

        protected void Set(string name, object value)
        {
            if (value != Get(name))
            {
                if (values.ContainsKey(name))
                {
                    var trackInfo = values[name];
                    if (trackInfo.Status != TrackStatus.New)
                    {
                        trackInfo.Status = TrackStatus.Changed;
                    }
                    trackInfo.Value = value;
                }
                else
                {
                    var trackInfo = new TrackInfo { Status = TrackStatus.New, Value = value };
                    values[name] = trackInfo;
                }
                OnPropertyChanged(name);
            }
        }

        public virtual void SetUnchange()
        {
            foreach (var kv in values)
            {
                kv.Value.Status = TrackStatus.Unchange;
            }
        }

        private bool bIsDiry = false;

        [Browsable(false), NoColumn]
        public bool IsDiry
        {
            get { return bIsDiry; }
            internal set
            {
                if (bIsDiry != value)
                {
                    bIsDiry = value;
                    OnPropertyChanged("IsDiry");
                }
            }
        }
    }
}
