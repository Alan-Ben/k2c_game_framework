using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// fixedCD所处状态
    /// </summary>
    public enum EFixedCDState
    {
        [InspectorName("没有次数")]
        NO_COUNT,
        [InspectorName("有次数但未满")]
        HAS_COUNT_UNMET_MAX,
        [InspectorName("次数已满")]
        MAX_COUNT,
    }
    
    /// <summary>
    /// 仿照FixedCdCustomMono写的
    /// </summary>
    public class GGUIMonoCommonFixedCd : _AALBasicUIWndMono
    {
        [ALHeader("颜色配置")]
        public List<NPGGUICommonConditionChgColorInfo> chgColorConfig;

        [ALHeader("次数文本")]
        public TextEx txtNum;
        [ALHeader("是否显示最大数量")]
        public bool isShowMaxCount;

        [ALHeader("刷新数量倒计时文本")]
        public TextEx txtRefreshNumCountDown;
        [ALHeader("刷新数量倒计时文本的key(一个参数, 时间)")]
        public string txtRefreshNumCountDownKey;
        
        [ALHeader("所处状态列表")]
        public List<NPCommonEnumStatInfo<EFixedCDState>> cdStateInfoList;
        
        [ALHeader("刷新间隔（秒）")]
        [Min(0.5f)]
        public float refreshInterval = 1f;

        [ALHeader("查看详情按钮")]
        public GameObject btnInfo;
        [ALHeader("详情弹窗间隔")]
        public float toolTipInterval = 0f;

        [ALHeader("获取按钮")]
        public GameObject btnGet;
    }
}