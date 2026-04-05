using System;
using System.Collections.Generic;

using ALPackage;
using NPEnum;


/// <summary>
/// questTarget表的客户端监听信息字段
/// 在is_client_target有效的情况下，客户端监听增加进度的消息
/// </summary>
/// <typeparam name="E"></typeparam>
[System.Serializable]
public class ClientTriggerMsgInfo
{
    //监听的枚举
    public string msgType;
    //监听消息的自定义参数
    public string msgArgs;
    
    /************
    * 读取字符串
    **/
    public static ClientTriggerMsgInfo readFromStr(string _str)
    {
        //拆分字符串后进行读取
        string[] strs = _str.Split(new char[] { ':' }, 2);

        if (strs.Length < 1)
        {
            UnityEngine.Debug.LogWarning("没有配置 对象名!");
            return null;
        }

        ClientTriggerMsgInfo ret = new ClientTriggerMsgInfo();

        ret.msgType = strs[0];
        ret.msgArgs = strs.Length > 1 ? strs[1] : string.Empty;

        return ret;
    }

    /************
     * 读取队列
     **/
    public static List<ClientTriggerMsgInfo> readList(string _str)
    {
        List<ClientTriggerMsgInfo> list = new List<ClientTriggerMsgInfo>();
        if (null == _str || _str.Length <= 0)
            return list;

        //string[] strs = _str.Split('|');
        string[] strs = _str.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < strs.Length; i++)
        {
            ClientTriggerMsgInfo newItem = ClientTriggerMsgInfo.readFromStr(strs[i]);
            if(null == newItem)
                continue;

            list.Add(newItem);
        }
        return list;
    }

    public override string ToString()
    {
        return string.Format("{0}:{1}", msgType, msgArgs);
    }
    
    /// <summary>
    /// 为自动导出写的
    /// </summary>
    public void ParseFromString(string _str)
    {
        //拆分字符串后进行读取
        string[] strs = _str.Split(new char[] { ':' }, 2);

        if (strs.Length < 1)
        {
            UnityEngine.Debug.LogWarning("没有配置 对象名!");
            return;
        }

        //需要支持物品类型没有配置的情况
        this.msgType = strs[0];
        this.msgArgs = strs.Length > 1 ? strs[1] : string.Empty;
    }

    /// <summary>
    /// 为自动导出写的
    /// </summary>
    public static List<ClientTriggerMsgInfo> MakeListFromString(string _str)
    {
        return readList(_str);
    }

    /// <summary>
    /// 为自动导出写的
    /// </summary>
    public static ClientTriggerMsgInfo[] MakeArrayFromString(string _str)
    {
        return readList(_str).ToArray();
    }
}