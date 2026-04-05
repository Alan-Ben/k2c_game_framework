using System;
using System.Collections.Generic;
using GOE;

/// <summary>
/// 物品数量区间对应粒子数量配置
/// </summary>
[System.Serializable]
public class ParticleNumRangeInfo
{
    public long miniCount;//最小数量
    public long maxCount;//最大数量
    public int particleNum;//粒子数量

    public ParticleNumRangeInfo()
    {
    }

    /// <summary>
    /// 获取目标需要展示的粒子数量
    /// </summary>
    /// <param name="_itemCount"></param>
    /// <returns></returns>
    public static int getTargetParticleNum(long _itemCount)
    {
#if NP_GAME
        List<ParticleNumRangeInfo> rangeList = GRefdataCoreMgr.instance.npGeneral.currency_particle_num_range_list;
        if (rangeList == null || rangeList.Count == 0)
            return 0;

        for (int i = 0; i < rangeList.Count; i++)
        {
            ParticleNumRangeInfo temp = rangeList[i];
            if(temp == null)
                continue;

            if (WCGLongRange.inRange(_itemCount, temp.miniCount, temp.maxCount))
                return temp.particleNum;
        }  
#endif
        return 0;
    }

#if NP_GAME
    public static int getTargetParticleNum(List<ParticleNumRangeInfo> _rangeList, long _itemCount)
    {
        if (_rangeList == null || _rangeList.Count == 0)
            return 0;

        for (int i = 0; i < _rangeList.Count; i++)
        {
            ParticleNumRangeInfo temp = _rangeList[i];
            if (temp == null)
                continue;

            if (WCGLongRange.inRange(_itemCount, temp.miniCount, temp.maxCount))
                return temp.particleNum;
        }
        return 0;
    }
#endif
    
    /************
    * 读取字符串
    **/
    public static ParticleNumRangeInfo readFromStr(string _str)
    {
        //拆分字符串后进行读取
        string[] strs = _str.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);

        if (strs.Length < 3)
        {
            UnityEngine.Debug.LogWarning("参数数量配置错误！");
            return null;
        }

        ParticleNumRangeInfo ret = new ParticleNumRangeInfo();

        ret.miniCount = long.Parse(strs[0]);
        ret.maxCount = long.Parse(strs[1]);
        ret.particleNum = int.Parse(strs[2]);

        return ret;
    }

    /************
     * 读取队列
     **/
    public static List<ParticleNumRangeInfo> readList(string _str)
    {
        List<ParticleNumRangeInfo> list = new List<ParticleNumRangeInfo>();
        if (null == _str || _str.Length <= 0)
            return list;

        //string[] strs = _str.Split('|');
        string[] strs = _str.Split(new string[] { "|", ";" }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < strs.Length; i++)
        {
            ParticleNumRangeInfo newItem = ParticleNumRangeInfo.readFromStr(strs[i]);
            if (null == newItem)
                continue;

            list.Add(newItem);
        }
        return list;
    }

    public override string ToString()
    {
        return string.Format("{0}:{1}:{2}", miniCount, maxCount, particleNum);
    }

    /// <summary>
    /// 为自动导出写的
    /// </summary>
    public void ParseFromString(string _str)
    {
        //拆分字符串后进行读取
        string[] strs = _str.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);

        if (strs.Length < 1)
        {
            UnityEngine.Debug.LogWarning("参数数量配置错误!");
            return;
        }

        this.miniCount = long.Parse(strs[0]);
        this.maxCount = long.Parse(strs[1]);
        this.particleNum = int.Parse(strs[2]);
    }

    /// <summary>
    /// 为自动导出写的
    /// </summary>
    public static List<ParticleNumRangeInfo> MakeListFromString(string _str)
    {
        return readList(_str);
    }

    /// <summary>
    /// 为自动导出写的
    /// </summary>
    public static ParticleNumRangeInfo[] MakeArrayFromString(string _str)
    {
        return readList(_str).ToArray();
    }
}