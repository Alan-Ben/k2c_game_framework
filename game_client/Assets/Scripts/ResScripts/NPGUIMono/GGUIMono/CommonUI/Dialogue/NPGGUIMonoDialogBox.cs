using ALPackage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 对话框
    /// </summary>
    public class NPGGUIMonoDialogBox : _AALBasicUIWndMono
    {
        [ALHeader("名称")]
        public Text txtName;
        [ALHeader("对话内容")]
        public Text txtContent;
        [ALHeader("头像图标")]
        public RawImage imgIcon;
        [ALHeader("继续按钮")]
        public GameObject btnNext;
        [ALHeader("当前句结束时 显示的物体")]
        public List<GameObject> goListShowOnSentenceOver;
        [ALHeader("选项窗体")]
        public NPGGUIMonoDialogueOptionContainer monoOptionContainer;
        [ALHeader("有选项时需要展示的GO列表")]
        public List<GameObject> goHaveOptionShowList;
        [ALHeader("有选项时需要隐藏的GO列表")]
        public List<GameObject> goHaveOptionHideList;

        [ALInfo("↓↓↓右边对话样式额外配置，左边对话样式不需要配置↓↓↓")]
        [ALHeader("右边样式修正文字布局的mono")]
        public TextSizeFixMonoV2 monoSizeFix;
    }
}
