using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 关卡选择事件页面
    /// </summary>
    public class GGUIMonoCommonSimpleChoiceEvent : _AGGUIMonoCommonSimpleEvent
    {
        [ALHeader("事件图片")]
        public RawImage monoEventImg;

        [ALHeader("选项Container")]
        public GGUIMonoCommonChoiceEventOptionContainer monoOptionContainer;

        [ALHeader("关闭按钮")]
        public GameObject btnClose;
    }
}