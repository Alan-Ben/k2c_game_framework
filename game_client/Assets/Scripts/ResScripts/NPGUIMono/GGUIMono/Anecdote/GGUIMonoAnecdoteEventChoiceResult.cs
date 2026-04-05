using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoAnecdoteEventChoiceResult : _AALBasicUIWndMono
    {
        [ALHeader("选项的结果描述")]
        public Text txtChoiceResult;
        [ALHeader("结果的奖励列表")]
        public NPGGUIMonoCommonItemContainer monoItemContainer;
        [ALHeader("关闭按钮")]
        public GameObject btnClose;

        [ALHeader("选择正确和错误显示的内容")]
        public List<GameObject> listRightShow;
        public List<GameObject> listWrongShow;


        public void setChoiceRight(bool _right)
        {
            ALUGUICommon.setGameObjEnable(listRightShow, false);
            ALUGUICommon.setGameObjEnable(listWrongShow, false);
            ALUGUICommon.setGameObjEnable(_right ? listRightShow : listWrongShow, true);
        }


        public static string assetPath { get { return UIResPathAssistant.getAssetPath(3405); } }
        public static string objName { get { return UIResPathAssistant.getObjName(3405); } }
    }
}