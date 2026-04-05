using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoInnGuestUnlockTipContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("解锁提示文本")]
        public Text txtTip;
        [ALHeader("完成与否的显示区别")]
        public List<GameObject> listCompleteShow;
        public List<GameObject> listUnCompleteShow;
        [ALHeader("未完成的置灰列表")]
        public List<MaskableGraphic> listUnCompleteGray;
        [ALHeader("完成与否的颜色区别")]
        public List<Graphic> listCompleteColorChange;
        public Color completeColor;
        public Color unCompleteColor;
        
        
#if NP_GAME
        public void setComplete(bool _isComplete)
        {
            ALUGUICommon.setGameObjEnable(listCompleteShow, false);
            ALUGUICommon.setGameObjEnable(listUnCompleteShow, false);
            ALUGUICommon.setGameObjEnable(_isComplete ? listCompleteShow : listUnCompleteShow, true);
            if (_isComplete)
                GGameCommonInfo.disgrayImage(listUnCompleteGray);
            else
                GGameCommonInfo.grayImage(listUnCompleteGray);
            Color targetColor = _isComplete ? completeColor : unCompleteColor;
            ALUGUICommon.setUIObjColor(listCompleteColorChange, targetColor);
        }
#endif
    }
}