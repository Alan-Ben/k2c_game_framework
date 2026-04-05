using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System;
using System.Xml;
using System.Text.RegularExpressions;
using System.IO;
using Excel;
using System.Reflection;
using System.Data;

/******************
 * excel文件读取通用函数对象
 **/
namespace ALPackage
{
    public class ALXlsCommon
    {
        /******************
         * 读取excel文件的具体函数，带入的回调为对每个格子进行处理的处理函数
         * 回调带入的参数为 定义结构体对象，格子列名称，格子内容
         **/
        public static List<T> readXls<T>(string _path, Func<T> _createDelegate, Action<T, string, string> _readDelegate)
        {
            //判断带入的函数对象是否有效
            if (null == _createDelegate ||  null == _readDelegate)
                return null;

            //开启文件并进行读取
            string tmpPath = _path.Replace(".xlsx", ".tmpX");
            //拷贝一个文件，避免开启excel时无法读取的问题
            File.Copy(_path, tmpPath);
            FileStream stream = File.Open(tmpPath, FileMode.Open, FileAccess.Read, FileShare.None);
            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

            //创建结果队列
            List<T> resList = new List<T>();

            do
            {
                //输出读取的表格名
                Debug.Log("Start Read Sheet: " + excelReader.Name);
                //判断sheet name是否data
                if (excelReader.Name.Equals("data", StringComparison.OrdinalIgnoreCase))
                {
                    //存储列名称,用于分析数据并进行读取
                    List<string> columnNameList = new List<string>();
                    int lineIdx = 0;
                    //对表进行处理
                    while (excelReader.Read())
                    {
                        if (0 == lineIdx)
                        {
                            //取列名
                            for (int i = 0; i < excelReader.FieldCount; i++)
                            {
                                //添加列名称
                                if (excelReader.IsDBNull(i))
                                    columnNameList.Add("");
                                else
                                    columnNameList.Add(excelReader.GetString(i));
                            }
                        }
                        else
                        {
                            //进行行处理
                            //创建新的接受数据节点
                            T dataObj = _createDelegate();

                            //记录本行是否有效
                            bool isLineEnable = false;
                            //没列值进行处理
                            for (int i = 0; i < excelReader.FieldCount; i++)
                            {
                                if (excelReader.IsDBNull(i))
                                    continue;

                                //获取对应值，进行后续处理
                                string value = excelReader.GetString(i);
                                if(i == 1 && (null != value && (value.StartsWith("--") || value.Equals("-C"))))//第二列如果是--开头，或-C，这一行无效
                                {
                                    //设置本行无效
                                    isLineEnable = false;
                                    break;
                                }

                                if (null != value && value.Trim().Length > 0)
                                {
                                    try
                                    {
                                        //处理读取操作
                                        _readDelegate(dataObj, columnNameList[i], value);
                                        //设置本行有效
                                        isLineEnable = true;
                                    }
                                    catch (Exception ex)
                                    {
                                        Debug.LogError(ex.Message + " Error Position:" + "[" + (i + 1) + "," + (lineIdx + 1) + "]" + "----name:" + columnNameList[i] + "   value:" + value);
                                    }
                                }
                            }

                            //添加到结果集合
                            if (isLineEnable)
                                resList.Add(dataObj);
                        }

                        //增加行号
                        lineIdx++;
                    }
                }
            } while (excelReader.NextResult());

            //释放文件资源
            stream.Close();
            stream.Dispose();
            excelReader.Dispose();
            stream = null;
            excelReader = null;

            //删除拷贝的文件
            File.Delete(tmpPath);

            return resList;
        }
        /********************
         * 创建xml节点，返回创建的新的子节点
         */
        public static XmlNode createXmlNode(XmlDocument xmlDoc, XmlNode _fatherNode, string _nodeName)
        {
            if (null == _fatherNode)
                return null;

            XmlNode tmpNode = xmlDoc.CreateElement(_nodeName);
            _fatherNode.AppendChild(tmpNode);
            return tmpNode;
        }
        public static XmlNode createXmlNode(XmlDocument xmlDoc, XmlNode _fatherNode, string _nodeName, bool _value)
        {
            return createXmlNode(xmlDoc, _fatherNode, _nodeName, _value.ToString());
        }
        public static XmlNode createXmlNode(XmlDocument xmlDoc, XmlNode _fatherNode, string _nodeName, long _value)
        {
            return createXmlNode(xmlDoc, _fatherNode, _nodeName, _value.ToString());
        }
        public static XmlNode createXmlNode(XmlDocument xmlDoc, XmlNode _fatherNode, string _nodeName, float _value)
        {
            return createXmlNode(xmlDoc, _fatherNode, _nodeName, _value.ToString());
        }
        public static XmlNode createXmlNode(XmlDocument xmlDoc, XmlNode _fatherNode, string _nodeName, int _value)
        {
            return createXmlNode(xmlDoc, _fatherNode, _nodeName, _value.ToString());
        }
        public static XmlNode createXmlNode(XmlDocument xmlDoc, XmlNode _fatherNode, string _nodeName, string _nodeValue)
        {
            if (null == _fatherNode || null == _nodeValue || _nodeValue.Length <= 0)
                return null;

            XmlNode tmpNode = xmlDoc.CreateElement(_nodeName);
            tmpNode.InnerText = _nodeValue;
            _fatherNode.AppendChild(tmpNode);
            return tmpNode;
        }

        
        /*****************
         * 将结构体自动转化为字符串的处理函数
         **/
        public static System.Text.StringBuilder makeObjStr(object objInfo)
        {
            System.Text.StringBuilder strB = new System.Text.StringBuilder();

            try
            {
                if (objInfo == null)
                    return strB;

                Type tInfo = objInfo.GetType();

                if (objInfo is ICollection)
                {
                    ICollection Ilist = objInfo as ICollection;
                    strB.Append("\n");
                    strB.AppendFormat("size:{0}", Ilist.Count);
                    strB.Append("\n");
                    foreach (object obj in Ilist)
                    {
                        strB.AppendFormat("{0}:", obj.GetType().ToString());
                        _makeObjStr(obj, strB, "-");
                    }
                    return strB;
                }
                
                if (tInfo.IsValueType || (objInfo is string))
                {
                    strB.Append(objInfo.ToString());
                    strB.Append("\n");
                    return strB;
                }

                //逐个成员处理
                FieldInfo[] mems = tInfo.GetFields();
                for (int i = 0; i < mems.Length; i++)
                {
                    FieldInfo memInfo = mems[i];
                    if (null == memInfo)
                        continue;
                    if (memInfo.Name.StartsWith("_g") || memInfo.Name.StartsWith("g_"))
                        continue;
                    
                    strB.AppendFormat("{0}:", memInfo.Name);

                    object obj = memInfo.GetValue(objInfo);
                    _makeObjStr(obj, strB, "-");
                }

                return strB;
            }
            catch (System.Exception ex)
            {
                Debug.LogError(ex.Message);
                return strB;
            }
        }
        protected static void _makeObjStr(object objInfo, System.Text.StringBuilder _recStrB, string _deepStr)
        {
            try
            {
                if (objInfo == null)
                {
                    _recStrB.Append("null\n");
                    return;
                }

                if (_recStrB == null)
                    _recStrB = new System.Text.StringBuilder();

                Type tInfo = objInfo.GetType();

                if (objInfo is ICollection)
                {
                    ICollection Ilist = objInfo as ICollection;
                    _recStrB.Append("\n");
                    _recStrB.Append(_deepStr);
                    _recStrB.AppendFormat("size:{0}", Ilist.Count);
                    _recStrB.Append("\n");
                    foreach (object obj in Ilist)
                    {
                        _recStrB.Append(_deepStr);
                        _recStrB.AppendFormat("{0}:", obj.GetType().ToString());
                        _makeObjStr(obj, _recStrB, _deepStr + "-");
                    }
                    return ;
                }
                
                if (tInfo.IsValueType || (objInfo is string))
                {
                    _recStrB.Append(objInfo.ToString());
                    _recStrB.Append("\n");
                    return ;
                }

                //结构体先换行
                _recStrB.Append("\n");
                //逐个成员处理
                FieldInfo[] mems = tInfo.GetFields();
                for (int i = 0; i < mems.Length; i++)
                {
                    FieldInfo memInfo = mems[i];
                    if (null == memInfo)
                        continue;
                    if (memInfo.Name.StartsWith("_g") || memInfo.Name.StartsWith("g_"))
                        continue;

                    _recStrB.Append(_deepStr);
                    _recStrB.AppendFormat("{0}:", memInfo.Name);

                    object obj = memInfo.GetValue(objInfo);
                    _makeObjStr(obj, _recStrB, _deepStr + "-");
                }

                return ;
            }
            catch (System.Exception ex)
            {
                Debug.LogError(objInfo.GetType() + ":\n" + ex.Message);
                return ;
            }
        }

        /****************
         * 根据字符串，使用冒号分割后读取整形队列
         **/
        public static List<int> readIntList(string _str)
        {
            List<int> list = new List<int>();

            if (null != _str && _str.Length != 0 && "0" != _str)
            {
                string[] strs = _str.Split(':');

                for (int i = 0; i < strs.Length; i++)
                {
                    list.Add(int.Parse(strs[i]));
                }
            }

            return list;
        }
        public static List<long> readLongList(string _str)
        {
            List<long> list = new List<long>();

            if (null != _str && _str.Length != 0 && "0" != _str)
            {
                string[] strs = _str.Split(':');

                for (int i = 0; i < strs.Length; i++)
                {
                    list.Add(long.Parse(strs[i]));
                }
            }

            return list;
        }
        public static List<string> readStringList(string _str)
        {
            List<string> list = new List<string>();

            if (null != _str && _str.Length != 0 && "0" != _str)
            {
                string[] strs = _str.Split(':');

                list.AddRange(strs);
            }

            return list;
        }
    }
}