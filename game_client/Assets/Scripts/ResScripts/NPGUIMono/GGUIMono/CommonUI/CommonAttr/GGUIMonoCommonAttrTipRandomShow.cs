using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 基础属性tip随机展示子窗口
    /// </summary>
    public class GGUIMonoCommonAttrTipRandomShow : _AALBasicUIWndMono
    {
        // [ALHeader("属性上浮提示父节点配置")]
        // public List<GGUIHeroUpgradeAttrTipPos> attrTipParentList;
        [ALHeader("属性上浮提示ID")]
        public long attrTipId;
        [ALHeader("每两个类型属性间的上浮延时")]
        public float tipDelayShow = 0.02f;
        [ALHeader("单种属性tip最大播放速度")]
        public float tipAccMaxRate = 3f;
        [ALHeader("单种属性tip数量达到这个数则开始加速")]
        public int tipAccMinNum = 1;
        [ALHeader("单种属性tip数量达到这个数则达到最高速")]
        public int tipAccMaxNum = 2;
        [ALHeader("每种属性tip可同时存在最大数量")]
        public long coexistMaxCount = 3;
    }
}
