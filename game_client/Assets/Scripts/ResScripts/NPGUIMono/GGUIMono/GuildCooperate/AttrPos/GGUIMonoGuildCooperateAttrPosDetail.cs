using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 公会协作属性据点详情
    /// </summary>
    public class GGUIMonoGuildCooperateAttrPosDetail : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("属性据点名称")]
        public Text txtAttrPosName;
        [ALHeader("派遣伙伴相性加成提示")]
        public Text txtDispatchDesc;
        [ALHeader("奖励据点图标")]
        public RawImage imgPosIcon;
        [ALHeader("属性图标")]
        public RawImage imgAttrIcon;
        [ALHeader("血量进度条")]
        public GGUIMonoCommonBlood monoHP;
        [ALHeader("筛选子窗口")]
        public GGUIMonoHeroMainFilter monoFilter;
        [ALHeader("伙伴列表")]
        public GGUIMonoGuildCooperateSelectHeroContainer monoHeroContainer;
        [ALHeader("派遣按钮")]
        public GameObject btnDispatch;
        [ALHeader("恢复按钮")]
        public GameObject btnRecover;
        [ALHeader("停止自动派遣按钮")]
        public GameObject btnStopAuto;
        [ALHeader("恢复消耗的道具")]
        public NPGGUIMonoCommonItem monoRecoverItem;
        [ALHeader("自动派遣开关")]
        public NPGGUIMonoCommonToggleEx monoAutoToggle;

        [ALHeader("自动派遣时显示的GO列表")]
        public List<GameObject> goAutoDispatchShowList;
        [ALHeader("自动派遣时隐藏的GO列表")]
        public List<GameObject> goAutoDispatchHideList;

        [ALHeader("建设值上浮center_tip id")]
        public long constructCenterTipID;
        [ALHeader("建设值上浮提示展示父节点")]
        public Transform constructCenterTipParent;

        [ALInfo("======下面状态列表只显示一种======")]
        [ALHeader("可派遣时显示的GO列表")]
        public List<GameObject> goCanDispatchShowList;
        [ALHeader("不可派遣可恢复时显示的GO列表")]
        public List<GameObject> goCanRecoverShowList;
        [ALHeader("不可派遣不可恢复时显示的GO列表")]
        public List<GameObject> goCanNotRecoverShowList;

        [ALInfo("======建设特效======")]
        [ALHeader("建设特效父节点")]
        public Transform sfxParent;
        [ALHeader("建设特效id")]
        public long sfxId;
        [ALHeader("建设特效同时存在最大数量")]
        public long maxSfxCount = 5;
        [ALHeader("延时展示建设特效时间（相对于开始派遣）")]
        public float delayShowSfxTime = 0.1f;

        [ALInfo("======进度条特效======")]
        [ALHeader("进度条特效父节点")]
        public Transform progressSfxParent;
        [ALHeader("进度条特效id")]
        public long progressSfxId;
        [ALHeader("进度条特效同时存在最大数量")]
        public long progressMaxSfxCount = 5;
        [ALHeader("延时展示进度条特效时间（相对于开始派遣）")]
        public float delayShowProgressSfxTime = 0.2f;

        /// <summary>
        /// 设置显示状态
        /// </summary>
        /// <param name="_canUse"></param>
        /// <param name="_canRecover"></param>
        public void setShowState(bool _canUse, bool _canRecover)
        {
            ALUGUICommon.setGameObjEnable(goCanDispatchShowList, false);
            ALUGUICommon.setGameObjEnable(goCanRecoverShowList, false);
            ALUGUICommon.setGameObjEnable(goCanNotRecoverShowList, false);

            if(_canUse)
                ALUGUICommon.setGameObjEnable(goCanDispatchShowList, true);
            else if(_canRecover)
                ALUGUICommon.setGameObjEnable(goCanRecoverShowList, true);
            else
                ALUGUICommon.setGameObjEnable(goCanNotRecoverShowList, true);
        }


        public static string assetPath { get { return UIResPathAssistant.getAssetPath(4935); } }
        public static string objName { get { return UIResPathAssistant.getObjName(4935); } }
    }
}