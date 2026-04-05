using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 选择大臣页面
    /// </summary>
    public class GGUIWndEveningDungeonFightSelectHero : _ANPGGUIBasicWnd<GGUIMonoEveningDungeonFightSelectHero>
    {
        private static GGUIWndEveningDungeonFightSelectHero _g_instance;
        public static GGUIWndEveningDungeonFightSelectHero instance { get { return _g_instance ??= new GGUIWndEveningDungeonFightSelectHero(); } }

        private List<_IEveningDungeonHeroFightInfo> _m_lHeroFightInfoList;//大臣列表
        private bool _m_bHasCanSelectHero;//是否有可以选择的大臣
        private _IEveningDungeonHeroFightInfo _m_iCurSelectHeroInfo;//当前选中的大臣
        private Action<_IEveningDungeonHeroFightInfo> _m_aOnSureSelectHero;//当确认选中大臣时的回调
        
        private GGUIWndHeroIconNullableItem _m_wSelectHeroIcon;//选中大臣头像
        private GGUIWndEveningDungeonFightSelectHeroItemGrid _m_wSelectHeroGrid;//选择大臣列表
        
        public GGUIWndEveningDungeonFightSelectHero() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoEveningDungeonFightSelectHero.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoEveningDungeonFightSelectHero.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.selectHeroIcon != null)
                _m_wSelectHeroIcon = new GGUIWndHeroIconNullableItem(wnd.selectHeroIcon);

            if (wnd.monoSelectHeroItemGrid != null)
            {
                _m_wSelectHeroGrid = new GGUIWndEveningDungeonFightSelectHeroItemGrid(wnd.monoSelectHeroItemGrid);
                _m_wSelectHeroGrid.clickDelegate += _onSelectHeroItemClick;
            }
            
            ALUGUICommon.combineBtnClick(wnd.btnSure, _onSureBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onCloseBtnClick);
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnSure, _onSureBtnClick);
                ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onCloseBtnClick);
            }
            
            _m_aOnSureSelectHero = null;
            
            _m_wSelectHeroIcon?.discard();
            _m_wSelectHeroIcon = null;

            if (_m_wSelectHeroGrid != null)
            {
                _m_wSelectHeroGrid.clickDelegate -= _onSelectHeroItemClick;
                _m_wSelectHeroGrid.discard();
                _m_wSelectHeroGrid = null;       
            }
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wSelectHeroIcon?.hideWnd();
            _m_wSelectHeroGrid?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wSelectHeroIcon?.resetWnd();
            _m_wSelectHeroGrid?.resetWnd();
        }

        public void setData(List<_IEveningDungeonHeroFightInfo> _heroFightInfoList, _IEveningDungeonHeroFightInfo _curSelectHero, Action<_IEveningDungeonHeroFightInfo> _onSureSelectHero)
        {
            _m_lHeroFightInfoList = _heroFightInfoList;
            _m_bHasCanSelectHero = false;
            if (_m_lHeroFightInfoList != null)
            {
                foreach (var heroFightInfo in _m_lHeroFightInfoList)
                {
                    if (heroFightInfo != null && heroFightInfo.nowFightCount < heroFightInfo.fightMaxCount)
                    {
                        _m_bHasCanSelectHero = true;
                        break;
                    }
                }
            }
            
            _m_iCurSelectHeroInfo = _curSelectHero;
            _m_aOnSureSelectHero = _onSureSelectHero;
            
            _refreshWnd();
        }

        /// <summary>
        /// 刷新窗口
        /// </summary>
        private void _refreshWnd()
        {
            if (_m_wSelectHeroGrid != null)
            {
                _m_wSelectHeroGrid.showWnd();
                _m_wSelectHeroGrid.showHeroList(_m_lHeroFightInfoList);
                _m_wSelectHeroGrid.setSelectByHeroId(_m_iCurSelectHeroInfo?.heroId ?? 0, false);
                _refreshSelectHeroShow();
            }
        }

        /// <summary>
        /// 刷新选中大臣显示信息
        /// </summary>
        private void _refreshSelectHeroShow()
        {
            if(wnd == null)
                return;

            if (_m_wSelectHeroIcon != null)
            {
                _m_wSelectHeroIcon.showWnd();
                _m_wSelectHeroIcon.setData(_m_iCurSelectHeroInfo?.heroCardShow);
            }

            if (_m_iCurSelectHeroInfo != null)
            {
                string selectHeroFightATKKey = string.IsNullOrEmpty(wnd.txtSelectHeroFightATKKey) ? TransKeyConst.common_value : wnd.txtSelectHeroFightATKKey;
                ALUGUICommon.setLabelTxt(wnd.txtSelectHeroFightATK, TextTranslate.instance.getLanguage(selectHeroFightATKKey, _m_iCurSelectHeroInfo.fightATK.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));       
            }
        }
        
        private void _onSelectHeroItemClick(_IEveningDungeonHeroFightInfo _selectHeroFightInfo)
        {
            _m_iCurSelectHeroInfo = _selectHeroFightInfo;

            _refreshSelectHeroShow();

            _onSureBtnClick(null);
        }
        
        /// <summary>
        /// 点击确认按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onSureBtnClick(GameObject _go)
        {
            // 当没有大臣可以选择时
            if (!_m_bHasCanSelectHero)
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.eveningDungeon_noHeroCanFightTip_none);
                return;
            }

            if (_m_iCurSelectHeroInfo == null)
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.eveningDungeon_notYetSelectedHeroTip_none);
                return;
            }
            
            _m_aOnSureSelectHero?.Invoke(_m_iCurSelectHeroInfo);
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_EVENING_DUNGEON_SELECT_HERO);
        }
        
        /// <summary>
        /// 关闭按钮点击事件
        /// </summary>
        /// <param name="_go"></param>
        private void _onCloseBtnClick(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_EVENING_DUNGEON_SELECT_HERO);
        }
    }
}