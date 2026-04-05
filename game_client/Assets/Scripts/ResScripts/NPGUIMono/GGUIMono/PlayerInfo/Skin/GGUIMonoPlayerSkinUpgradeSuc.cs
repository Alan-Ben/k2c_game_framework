using ALPackage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 玩家皮肤升级成功界面
    /// </summary>
    public class GGUIMonoPlayerSkinUpgradeSuc : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("形象")]
        public GGUIMonoCommonShowCase monoShowCase;
        [ALHeader("皮肤名称")]
        public Text txtName;
        [ALHeader("上个等级")]
        public Text txtLastLevel;
        [ALHeader("上个亲密度加成")]
        public Text txtLastIntimacy;
        [ALHeader("上个加护力加成")]
        public Text txtLastCharm;
        [ALHeader("当前等级")]
        public Text txtCurLevel;
        [ALHeader("当前亲密度加成")]
        public Text txtCurIntimacy;
        [ALHeader("当前加护力加成")]
        public Text txtCurCharm;
        [ALHeader("有亲密度加成时需要显示的GO列表")]
        public List<GameObject> goHaveIntimacyShowList;
        [ALHeader("有亲密度加成时需要隐藏的GO列表")]
        public List<GameObject> goHaveIntimacyHideList;
        [ALHeader("有加护力加成时需要显示的GO列表")]
        public List<GameObject> goHaveCharmShowList;
        [ALHeader("有加护力加成时需要隐藏的GO列表")]
        public List<GameObject> goHaveCharmHideList;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1725); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1725); } }
    }

}
