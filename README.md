# 简介
为Unity开发者量身打造的功能强大的高性能Json库，内置ILRuntime支持

<br/>

在内存分配，CPU耗时，功能扩展上都达到了一个较好的水准

<br/>

源码十分精简，在支持大量功能的同时Runtime部分源码只有2k行，仅为LitJson的一半

# 性能对比

反序列化Json文本为自定义数据类型：

![](https://github.com/CatImmortal/CatJson/raw/main/ImageRes/ParseJsonByType.png)

将自定义数据类型序列化为Json文本：

![](https://github.com/CatImmortal/CatJson/raw/main/ImageRes/ToJsonByType.png.png)



# 功能介绍

- **支持Json文本与通用Json对象间的转换**

  ```c#
  JsonObject jo = JsonParser.ParseJson(jsonText);
  string jsonText = JsonParser.ToJson(jo);
  ```

  <br/>

- **支持Json文本与自定义数据类型间的转换**

  ```c#
  CustomData data = JsonParser.ParseJson<CustomData>(jsonText);
  string jsonText = JsonParser.ToJson<CustomData>(data);
  ```

  <br/>

- **序列化为Json文本时自带格式化**

  <br/>

- **基础数据类型支持byte,char,bool,string,int,long,float,double**

  <br/>

- **支持自定义枚举类型，会作为int进行转换**

  <br/>

- **容器方面支持字典、数组、List，其中字典key的类型除了string外还支持int类型**

  *注意：支持三者的互相嵌套，但对于数组套数组的情况只支持锯齿数组不支持多维数组*

  <br/>

- **支持各种Unity特有数据类型，如Vector2/3/4，Quaternion,Color,Bounds,Rect,Keyframe,AnimationCurve等**

  <br/>

- **支持预生成序列化/反序列化代码以大幅度提高性能**

  在想要生成代码的自定义数据类上使用**GenJsonCodeRoot**特性进行标记，然后点击上方菜单栏中的`CatJson/预生成Json解析-转换代码`即可生成相应的代码，最后在**ParseJson/ToJson**调用时，第二个参数传入true(默认为false，通过反射运行)即可使用预生成的代码

  *注意：生成代码前需要在JsonCodeGenConfig.cs中根据自己项目的情况进行一些配置工作*

  *注意：在运行时需要先调用一次GenJsonCodesHelper.Init进行初始化工作，以注册生成好的代码*

  *注意：标记根类型即可，其所依赖的类型会自动生成对应代码*

  <br/>

- **支持使用者主动指定某个类型的转换方式**

  以**DateTime**为例，在**JsonParser.Extension.cs**文件中的静态构造方法中写入下述代码，即可将**DateTime**以字符串的形式序列化/反序列化：

  ```C#
              //反序列化DateTime
              ExtensionParseFuncDict.Add(typeof(DateTime), () =>
              {
                  //这里使用了Lexer.GetNextTokenByType(TokenType.String)从Json文本中提取了DateTime类型的字段/属性所对应的字符串值，然后使用DateTime.Parse解析该值，并将结果返回
                  RangeString rs = Lexer.GetNextTokenByType(TokenType.String);
                  return DateTime.Parse(rs.ToString());
              });
  
  
              //序列化DateTime
              ExtensionToJsonFuncDict.Add(typeof(DateTime), (value) =>
              {
                  Util.Append("\"");
                  Util.Append(value.ToString());
                  Util.Append("\"");
              });
  ```

  <br/>

- **支持使用JsonIgnore特性标记想要忽略的字段/属性**

  对于无法通过修改源码进行标记的字段/属性，以**Quaternion**的**eulerAngles**为例，可在**JsonParser.Extension.cs**文件中的静态构造方法中写入下述代码进行忽略：

  ```c#
              IgnoreSet.Add(typeof(Quaternion), new HashSet<string>()
              {
                  nameof(Quaternion.eulerAngles),
              }
  ```

  <br/>

- **定义了IJsonParserCallbackReceiver接口，为使用者提供序列化前的回调OnToJsonStart和反序列化后的回调OnParseJsonEnd，以处理其他特殊情况**

  <br/>

- **支持使用ILRuntime时对于热更层类型的序列化/反序列化，使用FUCK_LUA宏即可一键开启对ILRuntime的适配**

  *注意：对于ILRuntime热更层的类型只能通过反射运行，不支持生成代码*
  
  *注意：如果要在热更层调用ToJson或ParseJson的泛型版本，需要在ILRuntime初始化时调用JsonParser.RegisterILRuntimeCLRRedirection注册CatJson的CLR重定向*

# 注意事项

- 只会对公有的实例字段/属性进行序列化/反序列化，且属性必须同时具有get/set
- 对于字段/属性而言，如果其值为null,false或0，那么为了性能考虑将不会对该字段/属性进行序列化



