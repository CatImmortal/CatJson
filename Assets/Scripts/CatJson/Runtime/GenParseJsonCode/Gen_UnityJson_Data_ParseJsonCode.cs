using System;
using System.Collections.Generic;


namespace CatJson
{
    public static partial class GenJsonCodes
    {
        private static UnityJson_Data ParseJson_UnityJson_Data()
        {
            if (JsonParser.Lexer.LookNextTokenType() == TokenType.Null)
            {
                JsonParser.Lexer.GetNextToken(out _);
				return null;
            }

            UnityJson_Data obj = new UnityJson_Data();

            JsonParser.ParseJsonObjectProcedure(obj, null, (userdata1, userdata2, key, nextTokenType) =>
            {
                UnityJson_Data temp = (UnityJson_Data)userdata1;
                TokenType tokenType;
                
				if (key.Equals(new RangeString("v2")))
				{
				temp.v2 = ParseJson_UnityEngine_Vector2();
				}
				else if (key.Equals(new RangeString("v3")))
				{
				temp.v3 = ParseJson_UnityEngine_Vector3();
				}
				else if (key.Equals(new RangeString("v4")))
				{
				temp.v4 = ParseJson_UnityEngine_Vector4();
				}
				else if (key.Equals(new RangeString("rotate")))
				{
				temp.rotate = ParseJson_UnityEngine_Quaternion();
				}
				else if (key.Equals(new RangeString("color")))
				{
				temp.color = ParseJson_UnityEngine_Color();
				}
				else if (key.Equals(new RangeString("bounds")))
				{
				temp.bounds = ParseJson_UnityEngine_Bounds();
				}
				else if (key.Equals(new RangeString("rect")))
				{
				temp.rect = ParseJson_UnityEngine_Rect();
				}
				else if (key.Equals(new RangeString("keyframe")))
				{
				temp.keyframe = ParseJson_UnityEngine_Keyframe();
				}
				else if (key.Equals(new RangeString("ac")))
				{
				temp.ac = ParseJson_UnityEngine_AnimationCurve();
				}

                else{
                    JsonParser.ParseJsonValue(nextTokenType);
                }
                
            });


            return obj;
        }
    }

}
