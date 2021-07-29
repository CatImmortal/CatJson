using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CatJson
{
    public static partial class GenCodes
    {
        private static void ToJson_ToJson1_Data(ToJson1_Data obj,int depth)
        {
            Util.AppendLine("{");

            Util.AppendJsonKey("b", depth);
            if (obj.b == true)
            {
                Util.Append("true");
            }
            else
            {
                Util.Append("false");
            }
            Util.AppendLine(",");

            Util.AppendJsonKey("n", depth);
            Util.Append(obj.n.ToString());
            Util.AppendLine(",");

            Util.AppendJsonKey("s", depth);
            Util.Append(obj.s);
            Util.AppendLine(",");

            Util.AppendJsonKey("intDict",depth);


            Util.Append("}", depth - 1);
        }
    }

}
