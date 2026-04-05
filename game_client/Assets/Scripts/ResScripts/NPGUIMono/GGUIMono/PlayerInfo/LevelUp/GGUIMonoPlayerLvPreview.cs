using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 玩家等级预览
    /// </summary>
    public class GGUIMonoPlayerLvPreview : _AALBasicUIWndMono
    {
        [ALHeader("等级grid")]
        public GGUIMonoPlayerLvPreviewGrid lvPreviewGridMono;

        [ALHeader("当前等级显示的文本颜色")]
        public Color curLvShowColor;

        [ALHeader("其他等级显示的文本颜色")]
        public Color otherLvShowColor;

        [ALHeader("其他等级需要置灰的列表")]
        public List<MaskableGraphic> grayImgList;

        [ALHeader("等级基础信息")]
        public GGUIMonoPlayerLv lvMono;

        [ALHeader("达到上限显示的GoList")]
        public List<GameObject> lvMaxShowGoList;

        [ALHeader("仅当前等级显示的GoList-其他等级隐藏")]
        public List<GameObject> curLvShowGoList;

        [ALHeader("关闭按钮")]
        public GameObject closeBtn;

        [ALHeader("奖励列表")]
        public GGUIMonoCommonRewardContainer itemContainer;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1707); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1707); } }
    }
}
