using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 藏品分解确认界面
    /// </summary>
    public class GGUIWndEquipRecycleConfirm : _ANPGGUIBasicWnd<GGUIMonoEquipRecycleConfirm>
    {
        private static GGUIWndEquipRecycleConfirm _g_instance;
        public static GGUIWndEquipRecycleConfirm instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndEquipRecycleConfirm();
                return _g_instance;
            }
        }

        //道具列表
        private NPGGUIWndCommonItemContainer _m_wItemContainer;
        //需要分解列表
        private List<long> _m_lDbList;
        //点击确认按钮
        private Action _m_aOnClickConfirm;

        public GGUIWndEquipRecycleConfirm() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoEquipRecycleConfirm.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoEquipRecycleConfirm.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wItemContainer?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wItemContainer?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wItemContainer?.discard();
            _m_wItemContainer = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.uncombineBtnClick(wnd.btnConfirm, _onClickConfirm);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoItemContainer != null)
                _m_wItemContainer = new NPGGUIWndCommonItemContainer(wnd.monoItemContainer);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.combineBtnClick(wnd.btnConfirm, _onClickConfirm);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_dbIdList"></param>
        /// <param name="_costItemList"></param>
        public void setInfo(List<long> _dbIdList, List<NPCommonCostItem> _costItemList, Action _onClickConfirm = null)
        {
            _m_aOnClickConfirm = _onClickConfirm;
            _m_lDbList = new List<long>();
            if(_dbIdList != null)
                _m_lDbList.AddRange(_dbIdList);

            if (_m_wItemContainer != null)
            {
                _m_wItemContainer.showWnd();
                _m_wItemContainer.showItemList(_costItemList);
            }
        }

        //点击关闭
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_EQUIP_RECYCLE_CONFIRM);
        }

        //点击确认
        private void _onClickConfirm(GameObject _go)
        {
            if (_m_lDbList == null || _m_lDbList.Count == 0)
                return;

            NPPlayer.instance.equipComp.reqEquipDisassemble(_m_lDbList);
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_EQUIP_RECYCLE_CONFIRM);

            Action onConfirm = _m_aOnClickConfirm;
            _m_aOnClickConfirm = null;
            onConfirm?.Invoke();
        }
    }
}