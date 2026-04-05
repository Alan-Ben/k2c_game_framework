using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 周卡委派--选择骑士界面
    /// </summary>
    public class GGUIWndWeekCardSelectedHero : _ANPGGUIBasicWnd<GGUIMonoWeekCardSelectedHero>
    {
        private static GGUIWndWeekCardSelectedHero _g_instance = new GGUIWndWeekCardSelectedHero();
        public static GGUIWndWeekCardSelectedHero instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new  GGUIWndWeekCardSelectedHero();
                return _g_instance;
            }
        }
        
        private GGUIWndWeekCardSelectedHeroGrid _m_itemGrid;
        public event Action<List<long>> onSelectedConfirm;//点击确定的回调
        
        public GGUIWndWeekCardSelectedHero() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get => GGUIMonoWeekCardSelectedHero.assetPath; }
        protected override string _monoObjName { get => GGUIMonoWeekCardSelectedHero.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }

        protected override void _onShowWnd()
        {
            
        }

        protected override void _onHideWnd()
        {
            
        }

        protected override void _onReset()
        {
            _m_itemGrid?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_itemGrid?.discard();
            _m_itemGrid = null;
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            ALUGUICommon.combineBtnClick(wnd.btnClose, _clickClose);
            ALUGUICommon.combineBtnClick(wnd.btnConfirm, _clickConfirm);
            if (null != wnd.itemGrid)
            {
                _m_itemGrid = new GGUIWndWeekCardSelectedHeroGrid(wnd.itemGrid);
                _m_itemGrid.onSelectedCountChg += _onSelectedCountChg;
            }
        }

        /// <summary>
        /// 选中数量变化
        /// </summary>
        /// <param name="_selectedCount"></param>
        /// <param name="_max"></param>
        private void _onSelectedCountChg(int _selectedCount,int _max)
        {
            if(null == wnd)
                return;
            ALUGUICommon.setLabelTxt(wnd.txtSelectedCount,
                TextTranslate.instance.getLanguage(TransKeyConst.week_card_selected_hero_count, _selectedCount, _max));
        }

        /// <summary>
        /// 设置显示信息
        /// </summary>
        /// <param name="_allHeroInfo"></param>
        /// <param name="_defaultSelectedHeroList"></param>
        /// <param name="_maxLimit"></param>
        public void setInfo(List<HeroInfo> _allHeroInfo, List<long> _defaultSelectedHeroList, int _maxLimit)
        {
            if(null == wnd)
                return;
            showWnd();
            _m_itemGrid?.showWnd();
            _m_itemGrid?.setInfo(_allHeroInfo, _defaultSelectedHeroList, _maxLimit);
        }
        
        /// <summary>
        /// 点击关闭
        /// </summary>
        /// <param name="obj"></param>
        private void _clickClose(GameObject obj)
        {
            QueueMgr.instance.DoUIRollBackByEsc();
        }

        /// <summary>
        /// 点击确定
        /// </summary>
        /// <param name="obj"></param>
        private void _clickConfirm(GameObject obj)
        {
            List<long> selectedList = new List<long>();
            _m_itemGrid?.getSelectedList(selectedList);

            onSelectedConfirm?.Invoke(selectedList);
            QueueMgr.instance.DoUIRollBackByEsc();
        }
    }
}