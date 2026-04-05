using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 游戏设置切换语言列表item
    /// </summary>
    public class NPGGUIMonoGameSettingSwitchLanguageContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("语言类型")]
        public Text texLanguage;
        [ALHeader("选择按钮")]
        public GameObject btnClick;
        [ALHeader("是当前语言展示的GO列表")]
        public List<GameObject> goCurShowGOList;
        [ALHeader("不是当前语言展示的GO列表")]
        public List<GameObject> goNotCurShowGOList;
        [ALHeader("是当前语言时文本颜色")]
        public Color curTextColor = Color.black;
        [ALHeader("不是当前语言时文本颜色")]
        public Color notCurTextColor = Color.black;
    }
}
