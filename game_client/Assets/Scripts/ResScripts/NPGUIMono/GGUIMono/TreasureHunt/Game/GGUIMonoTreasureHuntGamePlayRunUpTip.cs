using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 太空寻宝游戏助跑提示窗口
    /// </summary>
    public class GGUIMonoTreasureHuntGamePlayRunUpTip : _AALBasicUIWndMono
    {
        [ALHeader("不同剩余秒数时的显示对象"),
         ALInfo("不用每一秒都配置，当剩余描述小于等于配置的秒数时，就会生效")]
        public List<GGUITreasureHuntGamePlayRunUpData> listRunUpData;
        [ALHeader("当前剩余秒数的文本")]
        public Text txtRunUpTime;


        public void setSeconds(float _seconds)
        {
            GGUITreasureHuntGamePlayRunUpData minSecondsData = null;
            foreach (GGUITreasureHuntGamePlayRunUpData data in listRunUpData)
            {
                if (data == null)
                    continue;
                
                data.hideAll();
                if (data.seconds >= _seconds && 
                    (minSecondsData == null || data.seconds <= minSecondsData.seconds))
                {
                    minSecondsData = data;
                }
            }
            
            if (minSecondsData != null)
                minSecondsData.setEnable();
        }
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6827); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6827); } }
    }
    [Serializable]
    public class GGUITreasureHuntGamePlayRunUpData
    {
        public float seconds; // 剩余助跑时间
        public List<GameObject> listShow;
        public List<GameObject> listHide;


        public void hideAll()
        {
            ALUGUICommon.setGameObjEnable(listShow, false);
            ALUGUICommon.setGameObjEnable(listHide, true);
        }
        public void setEnable()
        {
            ALUGUICommon.setGameObjEnable(listShow, true);
            ALUGUICommon.setGameObjEnable(listHide, false);
        }
    }
}