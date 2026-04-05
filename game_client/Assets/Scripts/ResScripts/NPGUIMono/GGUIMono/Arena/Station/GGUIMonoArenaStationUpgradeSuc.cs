using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 竞技场贸易站升级成功弹窗
    /// </summary>
    public class GGUIMonoArenaStationUpgradeSuc : _AALBasicUIWndMono
    {
        [ALHeader("等级")]
        public Text txtLevel;
        [ALHeader("上个收益速度")]
        public Text txtLastSpeed;
        [ALHeader("当前收益速度")]
        public Text txtCurSpeed;
        [ALHeader("上个上限")]
        public Text txtLastLimit;
        [ALHeader("当前上限")]
        public Text txtCurLimit;
        [ALHeader("有特权无上限时需要显示的GO列表")]
        public List<GameObject> goHavePrivilegeShowList;
        [ALHeader("有特权无上限时需要隐藏的GO列表")]
        public List<GameObject> goHavePrivilegeHideList;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(5202); } }
        public static string objName { get { return UIResPathAssistant.getObjName(5202); } }
    }
}