using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 宴会赴宴消耗弹窗
    /// </summary>
    public class GGUIWndDinnerCostList : _ATALBasicUIWnd<GGUIMonoDinnerCostList>
    {
    
        private static GGUIWndDinnerCostList _g_instance = new GGUIWndDinnerCostList();

        public static GGUIWndDinnerCostList instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndDinnerCostList();
                return _g_instance;
            }
        }

        private GGUIWndDinnerCostItemContainer _m_dinnerTypeContainer;

        private bool _m_isQuickJoin;

        private event Action<GDinnerJoinCostRefObj> _m_joinAction;
        private event Action<GDinnerJoinCostRefObj> _m_saveAction;

        public GGUIWndDinnerCostList() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get => GGUIMonoDinnerCostList.assetPath; }
        protected override string _monoObjName { get => GGUIMonoDinnerCostList.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }

        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
        
        }

        protected override void _onReset()
        {
        
        }

        protected override void _onDiscard()
        {
            _m_dinnerTypeContainer?.discard();
            _m_dinnerTypeContainer = null;

            _m_joinAction = null;
            _m_saveAction = null;
            if (wnd == null)
                return;
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.uncombineBtnClick(wnd.btnClose2, _onClickClose);
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.combineBtnClick(wnd.btnClose2, _onClickClose);
            if (null != wnd.dinnerTypeContainer)
            {
                _m_dinnerTypeContainer = new GGUIWndDinnerCostItemContainer(wnd.dinnerTypeContainer);
                _m_dinnerTypeContainer.onClickJoin += _onClickJoin;
                _m_dinnerTypeContainer.onClickSave += _onClickSave;
            }
        }

        /// <summary>
        /// 设置显示数据
        /// </summary>
        /// <param name="_joinCostList"></param>
        public void setInfo(Action<GDinnerJoinCostRefObj> _joinAction)
        {
            _m_joinAction += _joinAction;
            _m_saveAction = null;
            _m_isQuickJoin = false;
            _refreshWnd();
        }

        public void setQuickJoin(Action<GDinnerJoinCostRefObj> _saveAction)
        {
            _m_saveAction += _saveAction;
            _m_joinAction = null;
            _m_isQuickJoin = true;
            _refreshWnd();
        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd)
                return;
            _m_dinnerTypeContainer?.showWnd();
            _m_dinnerTypeContainer?.showItemList(_m_isQuickJoin);
        }

        /// <summary>
        /// 点击加入
        /// </summary>
        /// <param name="_costRef"></param>
        private void _onClickJoin(GDinnerJoinCostRefObj _costRef)
        {
            _m_joinAction?.Invoke(_costRef);
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_DINNER_COST_USE);
        }

        /// <summary>
        /// 点击加入
        /// </summary>
        /// <param name="_costRef"></param>
        private void _onClickSave(GDinnerJoinCostRefObj _costRef)
        {
            _m_saveAction?.Invoke(_costRef);
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_DINNER_COST_USE);
        }
    
        /// <summary>
        /// 点击关闭按钮
        /// </summary>
        /// <param name="obj"></param>
        private void _onClickClose(GameObject obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_DINNER_COST_USE);
        }
    }
}