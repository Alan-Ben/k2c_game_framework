using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 跟随窗口
    /// </summary>
    public class GGUIWndCommonToolTip_TowerState : _ATNPGGUIWndCommonItemToolTip<GGUIMonoCommonToolTip_TowerState>
    {
        public GGUIWndCommonToolTip_TowerState(string _assetPath, string _assetName) : base(_assetPath, _assetName)
        {
        }

        protected override void _onDiscard()
        {
            base._onDiscard();
        }

        protected override void _onWndInitDone()
        {
            base._onWndInitDone();
            if (null == wnd)
                return;
        }

        protected override void _onClose()
        {
            QueueMgr.instance.forceCloseNodeByType(typeof(GNodeCommonToolTip_TowerState));
        }
        
        public void setInfo(RectTransform _targetTransRoot, float _intervalX,float _intervalY)
        {
            _refreshWnd();
            setPos(_targetTransRoot,_intervalX, _intervalY);
        }
        
        private void _refreshWnd()
        {
            if(null == wnd)
                return;
            TowerChapterRefObj newChapter = GRefdataCoreMgr.instance.towerChapterRefCore?.getRef(NPPlayer.instance.towerComp.curChapterId);
            int newLevel = NPPlayer.instance.towerComp.curLevel;
            if (newChapter != null)
            {
                ALUGUICommon.setLabelTxt(wnd.txtEarnBonus, TextTranslate.instance.getLanguage(TransKeyConst.tower_earn_bonus_add_num, NPPlayer.instance.towerComp.towerEarningAddPer/100f));
                ALUGUICommon.setLabelTxt(wnd.txtLevelName, newChapter.getLevelName(newLevel));
                ALUGUICommon.setLabelTxt(wnd.txtDailyCoins, TextTranslate.instance.getLanguage(TransKeyConst.tower_daily_coin_get_num, newChapter.getTowerCoinCount(newLevel)));
            }
            
        }
    }
}