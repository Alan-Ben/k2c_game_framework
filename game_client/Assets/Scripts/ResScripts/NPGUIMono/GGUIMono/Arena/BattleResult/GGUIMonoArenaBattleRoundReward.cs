using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 竞技场回合连胜奖励弹窗
    /// </summary>
    public class GGUIMonoArenaBattleRoundReward : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("奖励宝箱列表")]
        public List<GGUIMonoArenaBattleRoundRewardBox> monoBoxList;
        [ALHeader("已选择宝箱时需要显示的GO列表")]
        public List<GameObject> goSelectShowList;
        [ALHeader("已选择宝箱时需要隐藏的GO列表")]
        public List<GameObject> goSelectHideList;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(5216); } }
        public static string objName { get { return UIResPathAssistant.getObjName(5216); } }
    }
}