using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星科技等级属性窗口
    /// </summary>
    public class GGUIWndMarsTechnologyLvlProperty : _ANPGGUIBasicWnd<GGUIMonoMarsTechnologyLvlProperty>
    {
        private static GGUIWndMarsTechnologyLvlProperty _g_instance;
        public static GGUIWndMarsTechnologyLvlProperty instance { get { return _g_instance ??= new GGUIWndMarsTechnologyLvlProperty(); } }

        private MarsTechnologyRefObj _m_technologyRefObj;
        private int _m_curLvl;
        private List<_IPropertyShow> _m_lAllPropertyList;
        private List<MarsLvlPropertyShowInfo> _m_lLevelPropertyInfoList;
        
        private GGUIWndMarsLvlPropertyShowItemGrid _m_wPropertyListGrid;

        public GGUIWndMarsTechnologyLvlProperty() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoMarsTechnologyLvlProperty.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMarsTechnologyLvlProperty.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            // 初始化属性列表Grid
            if (wnd.monoPropertyListGrid != null)
                _m_wPropertyListGrid = new GGUIWndMarsLvlPropertyShowItemGrid(wnd.monoPropertyListGrid);

            // 绑定关闭按钮
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onCloseBtnClick);
        }

        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onCloseBtnClick);
            }

            // 销毁属性列表Grid
            _m_wPropertyListGrid?.discard();
            _m_wPropertyListGrid = null;

            _m_technologyRefObj = null;
            
            _m_lAllPropertyList?.Clear();
            _m_lAllPropertyList = null;
            
            _m_lLevelPropertyInfoList?.Clear();
            _m_lLevelPropertyInfoList = null;
        }

        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            // 隐藏子窗口
            _m_wPropertyListGrid?.hideWnd();
        }

        protected override void _onReset()
        {
            // 重置子窗口
            _m_wPropertyListGrid?.resetWnd();
        }


        public void setData(MarsTechnologyRefObj _technologyRefObj, int _curLvl)
        {
            _m_technologyRefObj = _technologyRefObj;
            _m_curLvl = _curLvl;
            
            if(_m_lAllPropertyList == null)
                _m_lAllPropertyList = new List<_IPropertyShow>();
            _m_lAllPropertyList.Clear();
            if(_m_lLevelPropertyInfoList == null)
                _m_lLevelPropertyInfoList = new List<MarsLvlPropertyShowInfo>();
            _m_lLevelPropertyInfoList.Clear();
            GRefdataCoreMgr.instance.getMarsTechnologyLvlPropertyShowInfo(_m_technologyRefObj, _m_lLevelPropertyInfoList, _m_lAllPropertyList);
            
            _refreshWnd();
        }
        
        /// <summary>
        /// 刷新窗口
        /// </summary>
        private void _refreshWnd()
        {
            if (wnd == null || _m_technologyRefObj == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtTechnologyName, _m_technologyRefObj.transName);

            if (_m_wPropertyListGrid != null)
            {
                _m_wPropertyListGrid.showWnd();
                _m_wPropertyListGrid.setData(_m_lLevelPropertyInfoList, _m_lAllPropertyList, _m_curLvl);
            }
        }

        /// <summary>
        /// 关闭按钮点击
        /// </summary>
        private void _onCloseBtnClick(GameObject _obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MARS_TECHNOLOGY_LVL_PROPERTY);
        }
    }
}
