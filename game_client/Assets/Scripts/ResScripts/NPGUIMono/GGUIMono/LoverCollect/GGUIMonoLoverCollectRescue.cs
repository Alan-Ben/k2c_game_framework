using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    [Serializable]
    public class GGUIMonoLoverCollectDialogData
    {
        [ALHeader("对话气泡按钮")]
        public GameObject btnDialogBubble;
        [ALHeader("对话文本")]
        public Text txtDialog;
        [ALHeader("对话气泡动画")]
        public Animation animDialogBubble;
        public string animNameDialogBubble;
        [ALHeader("没有操作的话自动播放对话气泡动画的时间（秒）")]
        public float autoPlayDialogBubbleAnimTime = 3f;
        [ALHeader("对话的 key 内容")]
        public List<GGUIMonoLoverCollectDialogKeys> listDialogKeys;
    }
    [Serializable]
    public class GGUIMonoLoverCollectDialogKeys
    {
        [ALHeader("使用条件")]
        public _NPPlayerConditionSerializeInfo use_condition;
        [ALHeader("还不可以解救时的对话 key 内容")]
        public List<string> listDialogKeyNotCanRescue;
         [ALHeader("可以解救时的对话 key 内容")]
        public List<string> listDialogKeyCanRescue;
    }
    public class GGUIMonoLoverCollectRescue : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("赚速进度条")]
        public Slider sliderProgress;
        [ALHeader("赚速进度文本")]
        public Text txtProgress;
        [ALHeader("距离目标赚速差值文本")]
        public Text txtDiffEarnings;
        [ALHeader("解救按钮")]
        public GameObject btnRescue;
        [ALHeader("前往获取赚速按钮")]
        public GameObject btnGoEarn;
        [ALHeader("进度完成时显示")]
        public List<GameObject> listCanRescueShow;
        [ALHeader("进度未完成时显示")]
        public List<GameObject> listCanRescueHide;
        [ALHeader("解救后的表演组ID")]
        public long performGroupId;
        [ALHeader("对话数据")]
        public GGUIMonoLoverCollectDialogData dialogData;


#if NP_GAME
        public void setCanRescueState(bool _canRescue)
        {
            ALUGUICommon.setGameObjEnable(listCanRescueShow, false);
            ALUGUICommon.setGameObjEnable(listCanRescueHide, false);
            ALUGUICommon.setGameObjEnable(_canRescue ? listCanRescueShow : listCanRescueHide, true);
        }
#endif


        public static string assetPath { get { return UIResPathAssistant.getAssetPath(9101); } }
        public static string objName { get { return UIResPathAssistant.getObjName(9101); } }
    }
}
