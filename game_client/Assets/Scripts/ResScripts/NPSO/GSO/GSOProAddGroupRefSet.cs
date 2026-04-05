using System;
using System.Collections.Generic;
using ALPackage;
using GOE;

/// <summary>
/// 加成概率表
/// </summary>
[System.Serializable]
public class ProAddGroupRefObj : _IALBasicRefObj
{
    public long _refId { get { return group_id; } }

    public long group_id;//组id
 
    public List<ProAddRefObj> add_list;//加成概率列表(在导出代码ProAddExportMenu中按照加成从小到大排序了)
    public long total_weight;//总权重

    /// <summary>
    /// 获取最大加成的加成概率配表数据, 因为add_list列表按照加成从小到大排序了，所以最后一个就是最大加成
    /// </summary>
    public ProAddRefObj maxAddProRefObj { get { return add_list?.GetLast(); } }

    /// <summary>
    /// 获取加成提升概率
    /// </summary>
    /// <param name="_nowAdd">当前加成万分比</param>
    /// <returns></returns>
    public float getAddUpPro(long _nowAdd)
    {
        if (add_list == null)
            return 0;

        ProAddRefObj perAddProRefObj = null;//上一级的加成概率
        ProAddRefObj nowAddProRefObj = null;//当前加成概率
        foreach (ProAddRefObj item in add_list)
        {
            if(item == null)
                continue;
        
            nowAddProRefObj = item;
            if(nowAddProRefObj.add >= _nowAdd)
                break;
            
            perAddProRefObj = nowAddProRefObj;
            nowAddProRefObj = null;
        }
        
        if (nowAddProRefObj == null) //找不到当前加成所处的概率区间了, 说明当前加成已经是最大加成了, 继续提升的概率为0
            return 0;

        // 服务端采用的随机方式是 将同一组内的ProAddRefObj按照权重占比方式随机到一个ProAddRefObj, 然后在 (上一ProAddRefObj.add, 当前ProAddRefObj.add] 区间内等概率随机到一个加成
        // 所以给定一个加成，计算提升概率的方式是: 先取到当前加成所在的区间nowAddProRefObj(因为区间按照add从小到大排序过, 所以当前加成所处加成区间配表数据 是第一个ProAddRefObj.add大于当前值的区间)，
        // 算出在全部区间内随机到大于当前区间的比率randomLargeIntervalRate,
        // 算出在全部区间内随机到当前加成区间的比率randomNowAddProRate, 再算出在当前区间内随机到大于当前加成的比率inIntervalLargeRate randomNowAddProRate*inIntervalLargeRate就是在当前区间中随机到大于当前加成的概率
        float randomLargeIntervalRate = 1f * (total_weight - nowAddProRefObj.fromBeginToNowWeight) / total_weight;//在全部区间内随机到大于当前区间的比率
        float randomNowAddProRate = 1f * nowAddProRefObj.random_weight / total_weight;//在全部区间内随机到当前加成区间的比率
        float inIntervalLargeRate = 1f * (nowAddProRefObj.add - _nowAdd) / (nowAddProRefObj.add - (perAddProRefObj?.add ?? 0));//在当前区间内随机到大于当前加成的比率

        return randomLargeIntervalRate + randomNowAddProRate * inIntervalLargeRate;
        
        // float intervalLessNowAddWeight = 0f;//在当前区间内随机到小于当前加成的权重
        // float largeNowAddWeight = 0f;//在全部区间内随机到大于当前加成的权重
        // if (perAddProRefObj == null)//找不到上一级区间加成, 代表当前加成区间是最小加成区间了
        // {
        //     intervalLessNowAddWeight = 1f * _nowAdd / nowAddProRefObj.add * nowAddProRefObj.random_weight;//在当前区间内随机到小于当前加成的权重
        //     largeNowAddWeight = total_weight - intervalLessNowAddWeight;//在全部区间内随机到大于当前加成的权重 = 总权重 - 在当前区间内随机到小于当前加成的权重
        //     return largeNowAddWeight / total_weight;
        // }
        //
        // long addInterval = nowAddProRefObj.add - perAddProRefObj.add;//当前区间加成差值
        // long preNowAddInterval = _nowAdd - perAddProRefObj.add;//当前加成与上一级区间加成的差值
        // intervalLessNowAddWeight = 1f * preNowAddInterval / preNowAddInterval * nowAddProRefObj.random_weight;//在当前区间内随机到小于当前加成的权重
        // largeNowAddWeight = total_weight - perAddProRefObj.fromBeginToNowWeight - intervalLessNowAddWeight;//在全部区间内随机到大于当前加成的权重 = 总权重 - 从开始到上一区间总权重 - 在当前区间内随机到小于当前加成的权重
        // return largeNowAddWeight / total_weight;
    }
}

[System.Serializable]
public class ProAddRefObj
{
    public long id;//唯一id
    public long add;//加成万分比
    public long random_weight;//随机权重

    public long fromBeginToNowWeight; //同组内从开始到当前的随机权重

    public static int sort(ProAddRefObj _a, ProAddRefObj _b)
    {
        if (_b == null)
            return -1;
        if (_a == null)
            return 1;

        if (_a.add.CompareTo(_b.add) != 0)
            return _a.add.CompareTo(_b.add);

        return _a.id.CompareTo(_b.id);
    }
}

[System.Serializable]
public class TmpProAddRefObj
{
    public long id;//唯一id
    public long group_id;//加成组id
    public long add;//加成万分比
    public long random_weight;//随机权重
}

/// <summary>
/// 加成概率表
/// </summary>
public class GSOProAddGroupRefSet : _TALSOBasicRefSet<ProAddGroupRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
    public static string objName { get { return "pro_add"; } }
}