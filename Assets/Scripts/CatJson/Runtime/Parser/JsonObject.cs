using System.Collections;
using System.Collections.Generic;

namespace CatJson
{
    /// <summary>
    /// json对象
    /// </summary>
    public class JsonObject
    {
        private Dictionary<string, JsonValue> valueDict;

        public JsonValue this[string key]
        {
            get
            {
                if (valueDict == null)
                {
                    return null;
                }

                return valueDict[key];
            }

            set
            {
                if (valueDict == null)
                {
                    valueDict = new Dictionary<string, JsonValue>();
                }
                valueDict[key] = value;
            }
        }

        public override string ToString()
        {
            if (valueDict == null)
            {
                return "{} ";
            }
            string str = "{";
            int count = 0;
            foreach (KeyValuePair<string, JsonValue> item in valueDict)
            {
                count++;
                str += "\"" + item.Key + "\"" + " : " + item.Value;
                if (count < valueDict.Count)
                {
                    str += ", ";
                }
            }
            str += "} ";
            return str;
        }
  
        public void ToJson(int depth)
        {
            Util.AppendLine("{");
            int count = 0;
            foreach (KeyValuePair<string, JsonValue> item in valueDict)
            {
                count++;
                Util.Append("\"", depth);

                Util.Append(item.Key);
                Util.Append("\"");

                //Util.Append(" ");
                Util.Append(":");
                //Util.Append(" ");

                item.Value.ToJson(depth + 1);

                if (count != valueDict.Count)
                {
                    //不是最后一个
                    Util.AppendLine(",");
                }
                else
                {
                    Util.AppendLine(string.Empty);
                }
            }
           
            Util.Append("}", depth - 1);
        }
    }
}

