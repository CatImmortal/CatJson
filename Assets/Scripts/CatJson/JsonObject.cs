using System.Collections;
using System.Collections.Generic;

namespace CatJson
{
    /// <summary>
    /// json对象
    /// </summary>
    public class JsonObject
    {
        private Dictionary<string, JsonValue> values;


        public JsonValue this[string key]
        {
            get
            {
                if (values == null)
                {
                    return default;
                }

                return values[key];
            }

            set
            {
                if (values == null)
                {
                    values = new Dictionary<string, JsonValue>();
                }
                values[key] = value;
            }
        }

        public override string ToString()
        {
            if (values == null)
            {
                return "{} ";
            }
            string str = "{";
            int count = 0;
            foreach (KeyValuePair<string, JsonValue> item in values)
            {
                count++;
                str += "\"" + item.Key + "\"" + " : " + item.Value;
                if (count < values.Count)
                {
                    str += ", ";
                }
            }
            str += "} ";
            return str;
        }
    
        public bool TryGetValue(string key,out JsonValue value)
        {
            value = null;
            if (values == null)
            {
                return false;
            }

            return values.TryGetValue(key, out value);
        }
    }
}

