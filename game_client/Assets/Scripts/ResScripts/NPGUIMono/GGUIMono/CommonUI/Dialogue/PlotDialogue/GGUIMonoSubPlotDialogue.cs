using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 隐藏对话按钮需要播放的动画类型
    /// </summary>
    public enum EHeroRecommendDialogueAniType
    {
        [InspectorName("SHOW（展示对话列表）")]
        SHOW,
        [InspectorName("HIDE（隐藏对话列表）")]
        HIDE
    }

    /// <summary>
    /// 剧情对话附加窗口
    /// </summary>
    public class GGUIMonoSubPlotDialogue : _AALBasicUIWndMono
    {
        [ALHeader("点击展示下一句按钮")]
        public GameObject btnNext;
        [ALHeader("额外点击展示下一句按钮")]
        public GameObject btnNextEx;
        [ALHeader("选项窗体")]
        public NPGGUIMonoDialogueOptionContainer monoOptionContainer;
        [ALHeader("对话演出showcase")]
        public GGUIMonoCommonShowCase monoShowcase;
        [ALHeader("对话记录加载父节点")]
        public Transform goChatHistoryParent;
        [ALHeader("对话记录列表ScrollRect")]
        public ScrollRect chatHistoryScrollRect;

        [ALHeader("出现选项时需要展示的GO列表")]
        public List<GameObject> goHaveOptionShowList;
        [ALHeader("出现选项时需要隐藏的GO列表")]
        public List<GameObject> goHaveOptionHideList;

        [ALHeader("展示隐藏对话内容按钮")]
        public GameObject btnShowDialogue;
        [ALHeader("点击展示隐藏对话按钮需要播放的动画")]
        public CommonAnimationShowTypeInfo<EHeroRecommendDialogueAniType> clickShowDialogueBtnAni;
        [ALHeader("显示隐藏对话内容按钮的显示动画")]
        public CommonAnimationSingleInfo aniShowDialogueBtn;
    }
}

