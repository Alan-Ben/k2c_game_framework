using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using IFix;

[Configure]
public class InterpertConfig {
    [IFix]
    static IEnumerable<Type> ToProcess
    {
        get
        {
            return (from type in Assembly.Load("Assembly-CSharp").GetTypes()
                where type.Namespace != null && (type.Namespace.Contains("GOE")) && !type.Name.Contains("<")//官方实例就这么写的，说是不支持泛型基类
                select type);
        }
    }
}