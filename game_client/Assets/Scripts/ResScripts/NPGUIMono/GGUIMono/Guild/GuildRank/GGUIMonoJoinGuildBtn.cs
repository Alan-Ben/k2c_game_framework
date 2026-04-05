using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 联盟申请状态
    /// </summary>
    public enum EGuildApplyState
    {
        [InspectorName("没有数据时")]
        NO_DATA,
        [InspectorName("拒绝加入(不是申请被拒绝, 是盟主将联盟设置为拒绝加入状态)")]
        DECLINE_JOIN,
        [InspectorName("可申请加入")]
        CAN_APPLY_JOIN,
        [InspectorName("不可申请加入(联盟的加入方式为申请加入时, 客户端层面判断不可以申请时)")]
        CANNOT_APPLY_JOIN,
        [InspectorName("可自由加入")]
        CAN_FREE_JOIN,
        [InspectorName("不可自由加入(联盟的加入方式为自由加入时, 客户端层面判断不可以加入时)")]
        CANNOT_FREE_JOIN,
        [InspectorName("正在申请中")]
        APPLYING,
    }
    
    /// <summary>
    /// 加入联盟按钮
    /// </summary>
    public class GGUIMonoJoinGuildBtn : _AALBasicUIWndMono
    {
        [ALHeader("加入按钮(若和btnApply共用一个按钮时, 只要拖一个, 别拖两个一样的)")]
        public GameObject btnJoin;
        [ALHeader("申请加入按钮(若和btnJoin共用一个按钮时, 只要拖一个, 别拖两个一样的)")]
        public GameObject btnApply;
        [ALHeader("取消申请加入按钮")]
        public GameObject btnCancelJoin;
        
        [ALHeader("不同申请状态下的展示")]
        public MultiStateShow<EGuildApplyState> applyStateShow;

        [ALHeader("不可加入时的置灰物体")]
        public List<MaskableGraphic> onCannotJoinGaryList;
    }
}