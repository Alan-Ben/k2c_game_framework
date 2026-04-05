using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoTravelConsortBarEventChooseCostItem : _AALBasicUIWndMono
    {
        [ALHeader("选项配表id")]
        public long optionRefId;
        
        [ALHeader("选项图标")]
        public RawImage optionIcon;

        [ALHeader("选项名")]
        public TextEx txtOptionName;
        
        [ALHeader("选中妃子解锁状态配置")]
        public List<NPCommonEnumStatInfo<ETravelConsortUnlockStat>> statInfos;
        
        [ALHeader("增加的好感度")]
        public TextEx txtAddLike;
        [ALHeader("增加的好感度key(一个参数, 1.增加的好感度)")]
        public string txtAddLikeKey;
        
        [ALHeader("增加的亲密度")]
        public TextEx txtAddIntimacy;
        [ALHeader("增加亲密度key(一个参数, 1.增加的亲密度)")]
        public string txtAddIntimacyKey;
        
        [ALHeader("消耗道具")]
        public NPGGUIMonoCommonItem monoCostItem;
        [ALHeader("有消耗时显示")]
        public List<GameObject> hasCostShowGoList;
        [ALHeader("无消耗时显示")]
        public List<GameObject> freeShowGoList;
        
        [ALHeader("选择按钮")]
        public GameObject btnSelect;
    }
}