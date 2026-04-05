using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using ALPackage;
using CommonEnum;
using UnityEngine;

namespace Hotfix
{
    public class HotfixGCommon
    {
        /*
         * 主工程解析函数用不了的大概原因，不一定对哈哈哈
         * 
         * 下面函数用来打印Hotfix里面收到的服务端协议，主工程的NPGCommon.GetInfoPropertys用不了的原因是因为
         * 我们热更工程回包协议结构都是继承_IALProtocolStructure。这个接口在主工程我们热更调用用_IALProtocolStructureAdapter来实现
         * 所以如果用主工程的NPGCommon.GetInfoPropertys解析会认为收到的对象是认为adapter，这边会有两个问题
         * 1.adapter反射找不到真正的"get"开头的业务函数方法；只能找到_IALProtocolStructure接口的通用方法，所以数据是空的；
         * 2.Hotfix底层所有adapter的方法都会有“get_”的底层函数，又因为NPGCommon.GetInfoPropertys的实现原理是遍历所有“get”打头的方法去解析，
         * 所以取到的method都是adapter的“get_XXXX”函数，get_ILInstance, get_Assembly等，导致会一直递归下去死循环
         */
        /// <summary>
        /// Hotfix里面输出协议函数
        /// </summary>
        /// <param name="objInfo"></param>
        /// <param name="strB"></param>
        /// <returns></returns>
        public static string GetInfoPropertys(object objInfo, System.Text.StringBuilder strB = null)
        {
            try
            {
                if (null == strB)
                {
                    strB = new System.Text.StringBuilder();
                }

                if (objInfo == null)
                    return strB.ToString();

                Type tInfo = objInfo.GetType();
                
                if (objInfo is ICollection ilist)
                {
                    strB.AppendFormat("size:{0}", ilist.Count);
                    foreach (object obj in ilist)
                    {
                        GetInfoPropertys(obj, strB);
                    }
                    return strB.ToString();
                }

                strB.AppendFormat("[");

                if (tInfo.IsValueType || (objInfo is string))
                {
                    strB.Append(objInfo.ToString() + "]");
                    return strB.ToString();
                }

                MethodInfo[] pInfos = tInfo.GetMethods();
                for (int i = 0, max = pInfos.Length; i < max; i++)
                {
                    MethodInfo pTemp = pInfos[i];
                    string Pname = pTemp.Name;
                    if (!Pname.StartsWith("get") || Pname.EndsWith("MainOrder") || Pname.EndsWith("SubOrder"))
                    {
                        continue;
                    }
                    string pTypeName = pTemp.ReturnType.Name;
                    object Pvalue = null;
                    
                    //旧版本hotfix反射枚举会有问题，暂时看不到数据，临时先解决
                    if(pTemp.ReturnType.IsEnum)
                    {
                        Pvalue = "暂不支持";
                    }
                    else
                    {
                        Pvalue = pTemp.Invoke(objInfo, null);
                    }

                    if (pTemp.ReturnType.IsValueType || pTemp.ReturnType.Name.StartsWith("String"))
                    {
                        string value = (Pvalue == null ? "NULL" : Pvalue.ToString());
                        //strB.AppendFormat("属性名：{0}，属性类型：{1}，属性值：{2}<br/>", Pname, pTypeName, value);
                        strB.AppendFormat("{0}: {1}, ", Pname.Substring(3, Pname.Length - 3), value);
                    }
                    else
                    {
                        //string value = Pvalue == null ? "NULL" : Pvalue.ToString();
                        strB.AppendFormat("({0}){1}:[", pTypeName, Pname.Substring(3, Pname.Length - 3));
                        GetInfoPropertys(Pvalue, strB);
                        strB.Append("]");
                    }
                }
                strB.AppendLine("],");
                return strB.ToString();
            }
            catch (System.Exception ex)
            {
                Debug.LogError(ex.ToString_ILRuntime());
                return "";
            }
        }
    }
}