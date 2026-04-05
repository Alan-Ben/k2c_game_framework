using ALPackage;
using UnityEngine;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 通用toogle控件组，直接控制go显隐，不使用ugui的toggle组件
    /// </summary>
    public class NPGGUIMonoCommonToggleEx : MonoBehaviour
    {
        [ALHeader("默认是否选中")]
        public bool isOn;
        [ALHeader("点击按钮")]
        public GameObject btnClick;
        [ALHeader("附加点击按钮")]
        public GameObject btnAdditionClick;
        [ALHeader("选中 状态时显示")]
        public List<GameObject> isOnShow;
        [ALHeader("非选中 状态时显示")]
        public List<GameObject> isOffShow;
        [ALHeader("选中动画")]
        public Animation selectAnimation;
        [ALHeader("选中动画名称")]
        public string selectAniName;
        [ALHeader("隐藏取消选中动画名称")]
        public string disSelectAniName;
    }
}