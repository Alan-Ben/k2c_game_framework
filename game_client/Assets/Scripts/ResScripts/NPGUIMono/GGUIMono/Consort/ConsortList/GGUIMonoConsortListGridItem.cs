using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 妃子列表item
    /// </summary>
    public class GGUIMonoConsortListGridItem : _TALUGUIMonoGridItem
    {
        [ALHeader("卡牌基础信息item")]
        public GGUIMonoConsortCardItem monoCardItem;
        
        [ALHeader("妃子展示内容父节点(用于进行位置偏移, 不要直接拖自身item)")]
        public RectTransform consortShowContentParent;
        
        [ALHeader("妃子头像中心RectTransform, 不要直接拖自身item")]
        public RectTransform consortIconCenterTransform;

        [ALHeader("被邀约动画")]
        public Animation byInviteConsortItemAnim;
        [ALHeader("被邀约动画名称")]
        public string byInviteConsortItemAnimName;
        
        [ALHeader("红点")]
        public GGUIMonoConsortSystemRedTip redTipMono;

        // [ALHeader("外出游学展示go列表")]
        // public List<GameObject> goOutStudyShowGoList;
    }
}