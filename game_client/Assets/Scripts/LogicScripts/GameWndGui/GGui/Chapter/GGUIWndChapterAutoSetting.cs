using System;
using ALPackage;
using NPEnum;
using System.Collections.Generic;
using System.Linq;
using Common.ChapterEnum;
using CommonEnum;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GOE
{
    /// <summary>
    /// 关卡自动挑战设定
    /// </summary>
    public class GGUIWndChapterAutoSetting : _ATALBasicUIWnd<GGUIMonoChapterAutoSetting>
    {
        private static GGUIWndChapterAutoSetting _g_instance = new GGUIWndChapterAutoSetting();
        public static GGUIWndChapterAutoSetting instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndChapterAutoSetting();
                return _g_instance;
            }
        }
        
        //金币鼓舞
        private NPGGUIWndCommonItemNumSlider _m_goldSliderWnd;
        //砖石鼓舞
        private NPGGUIWndCommonItemNumSlider _m_crystalSliderWnd;
        //道具鼓舞
        private NPGGUIWndCommonItemNumSlider _m_itemSliderWnd;
   
        
        public GGUIWndChapterAutoSetting() : base(EALUIWndLayer.ADDITION)
        {

        }

        protected override string _monoAssetPath { get { return GGUIMonoChapterAutoSetting.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoChapterAutoSetting.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {

        }

        protected override void _onReset()
        {
            if (_m_goldSliderWnd != null) 
                _m_goldSliderWnd.resetWnd();
            if (_m_crystalSliderWnd != null) 
                _m_crystalSliderWnd.resetWnd();
            if (_m_itemSliderWnd != null) 
                _m_itemSliderWnd.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (null == wnd)
                return;

            if (_m_goldSliderWnd != null) 
                _m_goldSliderWnd.discard();
            if (_m_crystalSliderWnd != null)
                _m_crystalSliderWnd.discard();
            if (_m_itemSliderWnd != null) 
                _m_itemSliderWnd.discard();

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.uncombineBtnClick(wnd.screenBtnClose, _onClickClose);
            ALUGUICommon.uncombineBtnClick(wnd.btnSure, _onClickAuto);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.goldInfo != null) _m_goldSliderWnd = new NPGGUIWndCommonItemNumSlider(wnd.goldInfo);
            if (wnd.crystalInfo != null) _m_crystalSliderWnd = new NPGGUIWndCommonItemNumSlider(wnd.crystalInfo);
            if (wnd.itemInfo != null) _m_itemSliderWnd = new NPGGUIWndCommonItemNumSlider(wnd.itemInfo);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.combineBtnClick(wnd.screenBtnClose, _onClickClose);
            ALUGUICommon.combineBtnClick(wnd.btnSure, _onClickAuto);
        }
     
        private void _refreshWnd()
        {
            if (null != _m_goldSliderWnd)
            {
                _m_goldSliderWnd.setInfo(new NPCommonCostItem(ENPItemType.CURRENCY, (int)ECurrency.SILVER, 1));
                _m_goldSliderWnd.setSliderLimit(0, 0,GRefdataCoreMgr.instance.npGeneral.auto_forward_gold_inspire_count_limit);
            }
            if (null != _m_crystalSliderWnd)
            {
                _m_crystalSliderWnd.setInfo(GRefdataCoreMgr.instance.npGeneral.crystal_inspire_fixed_cost);
                _m_crystalSliderWnd.setSliderLimit(0, 0,GRefdataCoreMgr.instance.npGeneral.auto_forward_crystal_inspire_count_limit);
            }
            if (null != _m_itemSliderWnd)
            {
                _m_itemSliderWnd.setInfo(GRefdataCoreMgr.instance.npGeneral.item_inspire_cost);
                _m_itemSliderWnd.setSliderLimit(0, 0,GRefdataCoreMgr.instance.npGeneral.auto_forward_item_inspire_count_limit);
            }
        }
        
        //点击关闭
        private void _onClickClose(GameObject _obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Chapter.C_CHAPTER_AUTO_SETTING);
        }
        
        //点击自动
        private void _onClickAuto(GameObject _obj)
        {
            Dictionary<EChapterInspireType, int> inspireInfoDic = new Dictionary<EChapterInspireType, int>();
            inspireInfoDic.Add(EChapterInspireType.GOLD, _m_goldSliderWnd == null ? 0 : (int)_m_goldSliderWnd.curValue);
            inspireInfoDic.Add(EChapterInspireType.CRYSTAL, _m_crystalSliderWnd == null ? 0 : (int)_m_crystalSliderWnd.curValue);
            inspireInfoDic.Add(EChapterInspireType.ITEM, _m_itemSliderWnd == null ? 0 : (int)_m_itemSliderWnd.curValue);
            
            NPPlayer.instance.chapterComp.forwardLogicMgr.startAutoForwardToChapter(inspireInfoDic);
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Chapter.C_CHAPTER_AUTO_SETTING);
        }
    }
}