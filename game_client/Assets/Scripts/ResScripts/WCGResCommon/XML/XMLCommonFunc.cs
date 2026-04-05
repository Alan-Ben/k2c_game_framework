﻿using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;

using ALPackage;

public class XMLCommonFunc
{
    public delegate bool ReadingNodeFunc<T>(T _argObj, XmlNode _node, string _name);
    public delegate void ReadingNodeValueFunc<T>(T _argObj, string _name, string _value);

    /*************************
     * 查询指定目录下的所有xml后缀名文件
     * 
     * @author alzq.z
     * @time   Jun 26, 2013 11:47:46 PM
     */
    public static List<string> findAllChildXMLFiles(string _rootFolderPath)
    {
        return _findAllChildXMLFiles(_rootFolderPath);
    }
    protected static List<string> _findAllChildXMLFiles(string _rootFolderPath)
    {
        if (null == _rootFolderPath)
        {
            return null;
        }

        string[] folderPathArr = Directory.GetDirectories(_rootFolderPath);
        string[] filePathArr = Directory.GetFiles(_rootFolderPath);

        List<string> listFiles = new List<string>();

        foreach (string filePath in filePathArr)
        {
            if (filePath.ToLowerInvariant().EndsWith(".xml"))
            {
                listFiles.Add(filePath);
            }
        }

        foreach (string folderPath in folderPathArr)
        {
            //过滤一些以.开头的特殊文件，如SVN文件或Git文件
            if (!folderPath.StartsWith("."))
            {
                listFiles.AddRange(_findAllChildXMLFiles(folderPath));
            }
        }

        return listFiles;
    }

    /**************************
     * 搜索XML文件，查询出所有指定路径的子节点队列，路径使用:进行分隔
     * 
     * @author alzq.z
     * @time   Jun 26, 2013 11:57:03 PM
     */
    public static List<XmlNode> findAllChildNode(string _filePath, string _nodePath)
    {
        //创建并读取文件
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(_filePath);

        List<XmlNode> retList = new List<XmlNode>();

        //解析路径
        string[] pathArr = _nodePath.Split(':');

        try
        {
            //比较根节点名称与第一个节点名称
            if (!xmlDoc.DocumentElement.Name.Equals(pathArr[0], StringComparison.OrdinalIgnoreCase))
                return retList;

            //遍历子节点操作
            XmlNodeList childrenNodes = xmlDoc.ChildNodes;
            if (null == childrenNodes)
                return retList;

            for (int i = 0; i < childrenNodes.Count; i++)
            {
                //开始解析XML文件
                XmlNode rootNode = childrenNodes[i];

                //进行下一层级的处理
                _findAllChildNode(rootNode, _filePath, pathArr, 1, retList);
            }
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError("Read File: " + _filePath + " Error!\n" + ex.Message);
        }

        return retList;
    }
    /*************************
     * 根据节点路径，将所有符合的子节点对象都返回
     * 
     * @author alzq.z
     * @time   Aug 12, 2013 10:24:10 PM
     */
    protected static void _findAllChildNode(
            XmlNode _rootNode, string _filePath, string[] _pathArr, int _pathDeep, List<XmlNode> _recNodeList)
    {
        if (null == _rootNode)
            return;

        //遍历子节点操作
        XmlNodeList childrenNodes = _rootNode.ChildNodes;
        for (int i = 0; i < childrenNodes.Count; i++)
        {
            XmlNode childNode = childrenNodes[i];
            if (!isNodeEnable(childNode))
                continue;

            //比对节点名称与节点路径
            string nodeName = childNode.Name;
            if (nodeName.Equals(_pathArr[_pathDeep], StringComparison.OrdinalIgnoreCase))
            {
                int nextDeep = _pathDeep + 1;
                if (nextDeep >= _pathArr.Length)
                {
                    //到底直接加入队列
                    _recNodeList.Add(childNode);
                }
                else
                {
                    //继续搜索下一层
                    _findAllChildNode(childNode, _filePath, _pathArr, nextDeep, _recNodeList);
                }
            }
        }
    }

    /******************
     * 根据存储的XML进行数据的读取，根节点不进行读取
     **/
    public static void readXML<T>(string _xmlStr, T _argObj, ReadingNodeFunc<T> _readingNodeFunc, ReadingNodeValueFunc<T> _readingValueFunc)
    {
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(_xmlStr);

        if (null == doc)
            return;

        XmlNodeList childList = doc.DocumentElement.ChildNodes;
        for (int i = 0; i < childList.Count; i++)
        {
            XmlNode childNode = childList[i];

            if (!isNodeEnable(childNode))
                continue;

            string name = childNode.Name;

            //尝试进行节点处理，未处理则进行后续的内容处理
            if (null != _readingNodeFunc && _readingNodeFunc(_argObj, childNode, name))
                continue;

            string value = childNode.InnerText;

            if (null == value || value.Length <= 0)
                continue;

            //读取具体的信息内容
            if (null != _readingValueFunc)
                _readingValueFunc(_argObj, name, value);
        }
    }
    /*********************
     * 读取某个节点下的内容的相关操作
     **/
    public static void readXML<T>(XmlNode _rootNode, T _argObj, ReadingNodeFunc<T> _readingNodeFunc, ReadingNodeValueFunc<T> _readingValueFunc)
    {
        if (null == _rootNode)
            return;

        XmlNodeList childList = _rootNode.ChildNodes;
        for (int i = 0; i < childList.Count; i++)
        {
            XmlNode childNode = childList[i];

            if (!isNodeEnable(childNode))
                continue;

            string name = childNode.Name;

            //尝试进行节点处理，未处理则进行后续的内容处理
            if (null != _readingNodeFunc && _readingNodeFunc(_argObj, childNode, name))
                continue;

            string value = childNode.InnerText;

            if (null == value || value.Length <= 0)
                continue;

            //读取具体的信息内容
            if (null != _readingValueFunc)
                _readingValueFunc(_argObj, name, value);
        }
    }

    /******************
     * 判断节点是否有效
     * 
     * @author alzq.z
     * @time   Jun 27, 2013 12:35:17 AM
     */
    public static bool isNodeEnable(XmlNode _node)
    {
        if (null == _node || _node.NodeType != XmlNodeType.Element)
            return false;

        return true;
    }

    /********************
     * 读取指定标识作为名称的整形列表的对象
     * 
     * @author alzq.z
     * @time   Jun 27, 2013 12:29:59 AM
     */
    public static List<int> readIntList(XmlNode _rootNode, string _nodeName)
    {
        List<int> intList = new List<int>();

        //调用读取函数，动态实例化处理对象
        readXML(
                _rootNode
                , intList
                , (List<int> _intList, XmlNode _node, string _name) =>
                {
                    return false;
                }
                , (List<int> _intList, string _name, string _value) =>
                {
                    if (_name.Equals(_nodeName, StringComparison.OrdinalIgnoreCase))
                    {
                        _intList.Add(Int32.Parse(_value));
                    }
                }
                );

        return intList;
    }
}
