using System;
using System.Collections.Generic;


namespace CatJson
{
    public static partial class GenJsonCodes
    {
        private static UnityEngine.AnimationCurve ParseJson_UnityEngine_AnimationCurve()
        {
            if (JsonParser.Lexer.LookNextTokenType() == TokenType.Null)
            {
                JsonParser.Lexer.GetNextToken(out _);
				return null;
            }

            UnityEngine.AnimationCurve obj = new UnityEngine.AnimationCurve();

            JsonParser.ParseJsonObjectProcedure(obj, null, (userdata1, userdata2, key, nextTokenType) =>
            {
                UnityEngine.AnimationCurve temp = (UnityEngine.AnimationCurve)userdata1;
                TokenType tokenType;
                
				if (key.Equals(new RangeString("keys")))
				{
				List<UnityEngine.Keyframe> list = new List<UnityEngine.Keyframe>();
				JsonParser.ParseJsonArrayProcedure(list, null, (userdata11, userdata21, nextTokenType1) =>
				{
				((List<UnityEngine.Keyframe>)userdata11).Add(ParseJson_UnityEngine_Keyframe());
				});
				temp.keys = list.ToArray();
				}

                else{
                    JsonParser.ParseJsonValue(nextTokenType);
                }
                
            });


            return obj;
        }
    }

}
