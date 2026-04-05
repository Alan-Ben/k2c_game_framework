using UnityEngine;
using System;
using System.Collections.Generic;
using ALPackage;
using Common.NpChatObj;
using Common.NpPlayerInfoObj;
using UnityEngine.UI;

using NPEnum;


namespace GOE
{
    public class GGUIWndPlayerLv : _ANPGGUIBasicSubWnd<GGUIMonoPlayerLv>
    {
        // 等级图片
        private NPGGuiWndTexture _m_wLvWnd;

        //等级
        private PlayerLvlRefObj _m_lvRefObj;

        public GGUIWndPlayerLv(GGUIMonoPlayerLv _wnd)
           : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            _refresh();
        }

        protected override void _onHideWnd()
        {

        }

        protected override void _onReset()
        {
            if (null != _m_wLvWnd)
                _m_wLvWnd.discardTexture();
        }

        protected override void _onDiscard()
        {
            if (null != _m_wLvWnd)
                _m_wLvWnd.discard();
            _m_wLvWnd = null;

            ALUGUICommon.uncombineBtnClick(wnd.explainBtn, _explainBtnDidClick);
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            //称号
            if (null != wnd.lvImg)
                _m_wLvWnd = new NPGGuiWndTexture(wnd.lvImg);

            ALUGUICommon.combineBtnClick(wnd.explainBtn, _explainBtnDidClick);
        }

        /// <summary>
        /// 设置称号
        /// </summary>
        /// <param name="_infoTitleId"></param>
        public void setLv(long _lv)
        {
            PlayerLvlRefObj refObj = GRefdataCoreMgr.instance.playerLvlCore.getRef(_lv);
            if (null == refObj)
                return;

            setLvRefObj(refObj);
        }

        public void setLvRefObj(PlayerLvlRefObj _refObj)
        {
            _m_lvRefObj = _refObj;
            _refresh();

        }
        private void _refresh()
        {
            if (null == wnd || null == _m_lvRefObj)
                return;

            if (null != _m_wLvWnd)
                _m_wLvWnd.setTexture(_m_lvRefObj.icon);

            ALUGUICommon.setLabelTxt(wnd.lvTxt, TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, _m_lvRefObj.lvl));
            ALUGUICommon.setLabelTxt(wnd.lvNumTxt, _m_lvRefObj.lvl);
            ALUGUICommon.setLabelTxt(wnd.lvNameTxt, _m_lvRefObj.nameStr);
            ALUGUICommon.setLabelTxt(wnd.lvNumWithNameTxt, TextTranslate.instance.getLanguage(TransKeyConst.common_levelAndLevelName_num_str, TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, _m_lvRefObj.lvl), _m_lvRefObj.nameStr));
        }

        private void _explainBtnDidClick(GameObject _go)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndPlayerLvPreview.instance, GGUIWndPlayerLvPreview.instance.showWnd, EUIQueueStageType.MAIN, UINodeTagConst_PlayerInfo.C_ADD_PLAYERINFO_LV_PREVIEW_NODE, false, false);
        }
    }
}
