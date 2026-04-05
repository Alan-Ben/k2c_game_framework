using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoConsortCGGridItem : _TALUGUIMonoGridItem
    {
        [ALHeader("CG图标")]
        public RawImage cg_icon;

        [ALHeader("CG名字")]
        public TextEx txtCgName;

        [ALHeader("所属妃子名")]
        public TextEx txtOwnerName;

        [ALHeader("点击按钮")]
        public GameObject btnClick;

        [ALHeader("分享按钮")]
        public GameObject btnShare;
        
        [ALHeader("解锁状态显示配置")]
        public List<NPCommonEnumStatInfo<EGameCommonUnlockRewardType>> statInfos;

        /// <summary>
        /// 设置状态
        /// </summary>
        public void setState(EGameCommonUnlockRewardType _state)
        {
            if(statInfos == null)
                return;
            
            NPCommonEnumStatInfo<EGameCommonUnlockRewardType>.setStat(statInfos, _state);
        }
    }
}