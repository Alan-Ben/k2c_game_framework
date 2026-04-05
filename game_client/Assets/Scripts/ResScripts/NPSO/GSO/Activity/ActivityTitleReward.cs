using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

/// <summary>
/// 活动称号奖励
/// </summary>
[System.Serializable]
public class ActivityTitleReward
{
    public long title_reward_id;
    public long liftTs;

    /// <summary>
    /// 读取字符串
    /// </summary>
    /// <param name="_str"></param>
    /// <returns></returns>
    public static ActivityTitleReward readFromStr(string _str)
    {
        //拆分字符串后进行读取
        string[] strs = _str.Split(new string[] { ":","-" }, StringSplitOptions.RemoveEmptyEntries);

        if (strs.Length < 2)
        {
            UnityEngine.Debug.LogWarning("没有配置 对象名!");
            return null;
        }

        ActivityTitleReward ret = new ActivityTitleReward();

        ret.title_reward_id = long.Parse(strs[0]);
        ret.liftTs = long.Parse(strs[1]);

        return ret;
    }


    /// <summary>
    /// 读取队列
    /// </summary>
    /// <param name="_str"></param>
    /// <returns></returns>
    public static List<ActivityTitleReward> readList(string _str)
    {
        List<ActivityTitleReward> list = new List<ActivityTitleReward>();
        if (null == _str || _str.Length <= 0)
            return list;

        //string[] strs = _str.Split('|');
        string[] strs = _str.Split(new string[] { "|", ";" }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < strs.Length; i++)
        {
            ActivityTitleReward newItem = ActivityTitleReward.readFromStr(strs[i]);
            if(null == newItem)
                continue;

            list.Add(newItem);
        }
        return list;
    }

    public override string ToString()
    {
        return string.Format("{0}:{1}", title_reward_id, liftTs);
    }
    
    /// <summary>
    /// 为自动导出写的
    /// </summary>
    public void ParseFromString(string _str)
    {
        //拆分字符串后进行读取
        string[] strs = _str.Split(new string[] { "-", ":" }, StringSplitOptions.RemoveEmptyEntries);

        if (strs.Length < 1)
        {
            UnityEngine.Debug.LogWarning("没有配置 对象名!");
            return;
        }

        //需要支持物品类型没有配置的情况
        this.title_reward_id = long.Parse(strs[0]);    
        this.liftTs = long.Parse(strs[1]);
    }

    /// <summary>
    /// 为自动导出写的
    /// </summary>
    public static List<ActivityTitleReward> MakeListFromString(string _str)
    {
        return readList(_str);
    }

    /// <summary>
    /// 为自动导出写的
    /// </summary>
    public static ActivityTitleReward[] MakeArrayFromString(string _str)
    {
        return readList(_str).ToArray();
    }
}