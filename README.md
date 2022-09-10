# 简介
为Unity开发者量身打造的Json库，内置ILRuntime支持

在内存分配，CPU耗时上都达到了一个较好的水准

源码架构十分精简易读，可方便的按需求进行扩展修改

QQ交流群：762036315

# 性能对比

反序列化Json文本为自定义数据类型：

![](https://github.com/CatImmortal/CatJson/raw/main/ImageRes/ParseJsonByType.png)

将自定义数据类型序列化为Json文本：

![](https://github.com/CatImmortal/CatJson/raw/main/ImageRes/ToJsonByType.png)



# 功能介绍

- **支持Json文本与通用Json对象间的转换**

  ```csharp
  JsonObject jo = JsonParser.Default.ParseJson<JsonObject>(jsonText);
  string jsonText = JsonParser.Default.ToJson(jo);
  
  //或者使用扩展方法的形式
  JsonObject jo = jsonText.ParseJson<JsonObject>();
  string jsonText = jo.ToJson();
  ```

- **支持Json文本与自定义数据对象间的转换**

  ```csharp
  CustomData data = JsonParser.Default.ParseJson<CustomData>(jsonText);
  string jsonText = JsonParser.Default.ToJson<CustomData>(data);
  
  //或者使用扩展方法的形式
  CustomData data = jsonText.ParseJson<CustomData>();
  string jsonText = data.ToJson();
  ```

- **基础数据类型支持byte,sbyte,short,ushort,int,uint,long,ulong,float,double,decimal,char**

- **支持自定义枚举类型，会作为string进行转换**

- **数据容器类型支持字典、数组、List，其中字典key的类型除了string外还支持int与枚举类型**

  *注意：支持三者的互相嵌套，但对于数组套数组的情况只支持锯齿数组不支持多维数组*

- **支持Json文本与数据容器类型间的转换**

  ```csharp
  int[] nums =  JsonParser.Default.ParseJson<int[]>("[1,2,3]");
  ```

- **支持各种Unity特有数据类型，如Vector2/3/4，Quaternion,Color,Bounds,Rect,Keyframe,AnimationCurve等**

- **序列化为Json文本时支持格式化**

  可通过设置`JsonParser.Default.IsFormat = false`关闭格式化序列化

- **支持多态序列化/反序列化**

  可通过设置`JsonParser.Default.IsPolymorphic = false`关闭多态序列化/反序列化

- **支持使用者通过自定义JsonFormatter指定某个类型的转换方式**

  调用`JsonParser.AddCustomJsonFormatter(Type type, IJsonFormatter formatter)`即可注册指定类型的自定义Formatter

- **支持使用JsonIgnore特性标记想要忽略的字段/属性**

  对于无法通过修改源码进行标记的字段/属性，可通过调用`JsonParser.AddIgnoreMember(Type type, string memberName)`进行忽略

- **支持使用JsonKey特性自定义序列化/反序列化时的Key名称**

  对于无法通过修改源码进行自定义JsonKey的字段/属性，可通过调用`JsonParser.SetJsonKey(Type type, string key, FieldInfo fi)`和`JsonParser.SetJsonKey(Type type, string key, PropertyInfo fi)`进行自定义

- **定义了IJsonParserCallbackReceiver接口，为使用者提供序列化前的回调OnToJsonStart和反序列化后的回调OnParseJsonEnd，以处理其他特殊情况**

- **支持使用ILRuntime时对于热更层类型的序列化/反序列化，使用FUCK_LUA宏即可一键开启对ILRuntime的适配**

  *注意：如果要在热更层调用ToJson或ParseJson的泛型版本，需要在ILRuntime初始化时调用ILRuntimeHelper.RegisterILRuntimeCLRRedirection注册CatJson的CLR重定向*

# 注意事项

- `JsonParser.Default`表示默认Json解析器对象，若需要创建不同Parser实例，可通过`JsonParser jsonParserObject = new JsonParser()`创建，不同实例不共享配置设置
- 默认只会对公有的实例字段/属性(且属性必须同时具有get/set)进行序列化/反序列化，可通过调用`JsonParser.SetBindingFlags(BindingFlags bindingFlags)`来修改BindingFlags
- 对于字段/属性而言，如果其值为null,false或0，那么为了性能考虑将不会对该字段/属性进行序列化（可以通过`JsonParser.Default.IgnoreDefaultValue = false`全局关闭此项设置，或针对指定类型标记`JsonCareDefaultValue`特性进行局部关闭）

# 相关文章

[CatJson开发总结](http://cathole.top/2021/12/05/catjson-dev-summary/)

[CatJsonV2开发总结](http://cathole.top/2022/08/17/catjson-v2-dev-summary/)

