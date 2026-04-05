using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoTreasureHuntGamePlayEndEffect : _AALBasicUIWndMono
    {
        [ALHeader("胜利和失败时显示的内容")]
        public List<GameObject> listWinShow;
        public List<GameObject> listLoseShow;
        [ALHeader("自动关闭时间")]
        public float autoCloseTime = 2.0f; // 自动关闭时间


        public void setWin(bool _isWin)
        {
            ALUGUICommon.setGameObjEnable(listWinShow, false);
            ALUGUICommon.setGameObjEnable(listLoseShow, false);
            ALUGUICommon.setGameObjEnable(_isWin ? listWinShow : listLoseShow, true);
        }
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6830); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6830); } }
    }
}