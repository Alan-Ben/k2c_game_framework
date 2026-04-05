using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoTravelConsortBarEventChooseCost : _AALBasicUIWndMono
    {
        [ALHeader("妃子形象")]
        public GGUIMonoCommonShowCase consortTdShow;
        
        // [ALHeader("消耗道具列表")]
        // public GGUIMonoTravelConsortBarEventChooseCostItemContainer costItemContainer;
        [ALHeader("选择消耗道具窗口列表")]
        public List<GGUIMonoTravelConsortBarEventChooseCostItem> monoChooseCostList;

        [ALHeader("解锁状态配置")]
        public List<NPCommonEnumStatInfo<ETravelConsortUnlockStat>> statInfos;

        [Space(20)]
        [ALHeader("好感度妃子名称文本")]
        public TextEx txtLikeConsortName;
        [ALHeader("好感度妃子名称文本key(一个参数, 妃子名)")]
        public string txtLikeConsortNameKey;
        [ALHeader("好感度值文本")]
        public TextEx txtLikeValue;
        [ALHeader("好感度值key(两个参数, 1.当前好感度, 2.获取所需好感度)")]
        public string txtLikeValueKey;
        
        [Space(20)]
        [ALHeader("亲密度妃子名称文本")]
        public TextEx txtIntimacyConsortName;
        [ALHeader("亲密度妃子名称文本key(一个参数, 妃子名)")]
        public string txtIntimacyConsortNameKey;
        [ALHeader("亲密度值文本")]
        public TextEx txtIntimacyValue;
        [ALHeader("亲密度值文本key(一个参数, 1.当前亲密度)")]
        public string txtIntimacyValueKey;
        
        [Space(20)]
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
    }
}