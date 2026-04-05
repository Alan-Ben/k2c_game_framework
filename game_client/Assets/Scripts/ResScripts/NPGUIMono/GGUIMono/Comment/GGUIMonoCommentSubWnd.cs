using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 弹幕字窗口容器
    /// </summary>
    public class GGUIMonoCommentSubWnd : _AALBasicUIWndMono
    {
        [ALHeader("评论容器")] 
        public GGUIMonoCommentContainer monoCommentContainer;
        [ALHeader("初始弹幕数量")]
        public int initCount;
        [ALHeader("最快自动添加时间（秒）")]
        [Range(0.1f, 60)]
        public float addTimeMinS = 0;
        [ALHeader("最慢自动添加时间（秒）")]
        [Range(0.1f, 60)]
        public float addTimeMaxS = 1;
    }
}