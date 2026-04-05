using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 对话历史回顾选项
    /// </summary>
    public class GGUIMonoDialogueHistoryOptionContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("选项内容")]
        public Text txtContent;
        [ALHeader("选项选中时需要展示的GO列表")]
        public List<GameObject> goSelectShowList;
        [ALHeader("选项选中时需要隐藏的GO列表")]
        public List<GameObject> goSelectHideList;

        [ALInfo("↓↓↓右边选项样式额外配置，左边选项样式不需要配置↓↓↓")]
        [ALHeader("右边样式修正文字布局的mono")]
        public TextSizeFixMonoV2 monoSizeFix;
    }
}
