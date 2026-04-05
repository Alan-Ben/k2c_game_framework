using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 自定义添加列表，暂时只控制show，hide，内部的按钮通过effect自定义跳转，
    /// 有需要程序控制再加Mono字段
    /// </summary>
    public class GGUIMonoCustomAddList : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
    }
}