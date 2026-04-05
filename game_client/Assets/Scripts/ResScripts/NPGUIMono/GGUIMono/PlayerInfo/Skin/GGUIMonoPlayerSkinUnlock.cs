using ALPackage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 解锁玩家皮肤界面
    /// </summary>
    public class GGUIMonoPlayerSkinUnlock : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("前往按钮")]
        public GameObject btnGoTo;
        [ALHeader("形象")]
        public GGUIMonoCommonShowCase monoShowCase;
        [ALHeader("皮肤名称")]
        public Text txtName;
        [ALHeader("亲密度加成")]
        public Text txtIntimacy;
        [ALHeader("加护力加成")]
        public Text txtCharm;
        [ALHeader("有亲密度加成时需要显示的GO列表")]
        public List<GameObject> goHaveIntimacyShowList;
        [ALHeader("有亲密度加成时需要隐藏的GO列表")]
        public List<GameObject> goHaveIntimacyHideList;
        [ALHeader("有加护力加成时需要显示的GO列表")]
        public List<GameObject> goHaveCharmShowList;
        [ALHeader("有加护力加成时需要隐藏的GO列表")]
        public List<GameObject> goHaveCharmHideList;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1724); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1724); } }
    }

}
