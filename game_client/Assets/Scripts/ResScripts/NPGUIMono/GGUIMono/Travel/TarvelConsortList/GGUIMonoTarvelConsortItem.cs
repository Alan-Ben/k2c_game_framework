using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 游历中的知己解锁状态
/// </summary>
public enum ETravelConsortUnlockStat
{
    [InspectorName("已解锁")]
    UNLOCK,
    [InspectorName("未解锁--可游历获得")]
    LOCK_IN_TRAVEL,
    [InspectorName("未解锁--游历不可获得")]
    LOCK,
}

public class ETravelConsortUnlockStatComparer : IComparer<ETravelConsortUnlockStat>
{
    private static readonly Dictionary<ETravelConsortUnlockStat, int> _g_orderDic = new Dictionary<ETravelConsortUnlockStat, int>
    {
        { ETravelConsortUnlockStat.LOCK_IN_TRAVEL, 1 },
        { ETravelConsortUnlockStat.UNLOCK, 2 },
        { ETravelConsortUnlockStat.LOCK, 3 }
    };

    public static int compare(ETravelConsortUnlockStat x, ETravelConsortUnlockStat y)
    {
        return _g_orderDic[x].CompareTo(_g_orderDic[y]);
    }
    
    public int Compare(ETravelConsortUnlockStat x, ETravelConsortUnlockStat y)
    {
        return compare(x, y);
    }
}

//游历妃子列表item
public class GGUIMonoTarvelConsortItem : _AALBasicUIWndMono
{
    [ALHeader("妃子item")]
    public GGUIMonoConsortCardItem consortItem; 
    [ALHeader("好感度进度条")]
    public NPGGUIMonoProgress monoLikeProgress;
    [ALHeader("好感度进度条key(两个参数，第一个是好感度值，第二个是好感度上限值)")]
    public string likeProgressKey;
    
    [ALHeader("最常出现的地点名字")]
    public TextEx txtTravelPos; 
    [ALHeader("最常出现的地点名字key(一个参数, 地点名字)")]
    public string txtTravelPosKey;
    
    [ALHeader("解锁状态配置")]
    public List<NPCommonEnumStatInfo<ETravelConsortUnlockStat>> statInfos;
}