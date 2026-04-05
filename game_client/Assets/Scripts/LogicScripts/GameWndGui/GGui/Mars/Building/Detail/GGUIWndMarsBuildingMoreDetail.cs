using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星建筑更多详情窗口
    /// </summary>
    public class GGUIWndMarsBuildingMoreDetail : _ATALBasicUIWnd<GGUIMonoMarsBuildingMoreDetail>
    {
        [NotNull] public static GGUIWndMarsBuildingMoreDetail instance { get { return _g_instance ??= new GGUIWndMarsBuildingMoreDetail(); } }
        private static GGUIWndMarsBuildingMoreDetail _g_instance;

        private _IMarsBuildingView _m_buildingView;
        private List<_IPropertyShow> _m_lAllPropertyList;
        private List<MarsLvlPropertyShowInfo> _m_lLevelPropertyInfoList;
        
        private GGUIWndMarsLvlPropertyShowItemGrid _m_lvlPropertyShowGridWnd;
        
        public GGUIWndMarsBuildingMoreDetail() : base(EALUIWndLayer.ADDITION)
        {
        }


        protected override string _monoAssetPath { get { return GGUIMonoMarsBuildingMoreDetail.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMarsBuildingMoreDetail.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onShowWnd()
        {
            _refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_lvlPropertyShowGridWnd?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_lvlPropertyShowGridWnd?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_lvlPropertyShowGridWnd?.discard();
            _m_lvlPropertyShowGridWnd = null;

            _m_buildingView = null;
            _m_lAllPropertyList?.Clear();
            _m_lAllPropertyList = null;
            _m_lLevelPropertyInfoList?.Clear();
            _m_lLevelPropertyInfoList = null;
            
            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onCloseBtnClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            // 构建等级属性显示列表
            if (wnd.lvlPropertyShowGrid != null)
                _m_lvlPropertyShowGridWnd = new GGUIWndMarsLvlPropertyShowItemGrid(wnd.lvlPropertyShowGrid);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onCloseBtnClick);
        }

        /// <summary>
        /// 刷新窗口（带参数版本）
        /// </summary>
        /// <param name="_buildingView">建筑视图数据</param>
        public void setData(_IMarsBuildingView _buildingView)
        {
            _m_buildingView = _buildingView;
            if (_m_lAllPropertyList == null)
                _m_lAllPropertyList = new List<_IPropertyShow>();
            _m_lAllPropertyList.Clear();
            if(_m_lLevelPropertyInfoList == null)
                _m_lLevelPropertyInfoList = new List<MarsLvlPropertyShowInfo>();
            _m_lLevelPropertyInfoList.Clear();
            if (_m_buildingView != null && _m_buildingView.buildingInfo != null)
            {
                GRefdataCoreMgr.instance.getMarsBuildingLvlPropertyShowInfo(_m_buildingView.buildingInfo.refObj, _m_lLevelPropertyInfoList, _m_lAllPropertyList);
            }
            
            _refreshWnd();
        }
        
        /// <summary>
        /// 刷新窗口
        /// </summary>
        private void _refreshWnd() 
        {
            if (wnd == null || !_m_bIsShow || _m_buildingView == null)
                return;

            MarsBuildingInfo buildingInfo = _m_buildingView.buildingInfo;
            if (buildingInfo == null)
                return;
            
            // 刷新建筑描述
            ALUGUICommon.setLabelTxt(wnd.txtBuildingDesc, buildingInfo.descTranslated);
            
            // 刷新等级属性显示
            _refreshLvlPropertyShow();
        }


        /// <summary>
        /// 刷新等级属性显示
        /// </summary>
        /// <param name="_buildingInfo">建筑信息</param>
        private void _refreshLvlPropertyShow()
        {
            if (!_m_bIsShow || _m_lvlPropertyShowGridWnd == null || _m_buildingView == null || _m_buildingView.buildingInfo == null)
                return;
            
            _m_lvlPropertyShowGridWnd.showWnd();
            // 设置数据到Grid
            _m_lvlPropertyShowGridWnd.setData(_m_lLevelPropertyInfoList, _m_lAllPropertyList, _m_buildingView.buildingInfo.level);
        }

        /// <summary>
        /// 点击关闭窗口
        /// </summary>
        /// <param name="_go"></param>
        private void _onCloseBtnClick(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Mars.C_MARS_EXPAND_BUILDING_MORE_DETAIL);
        }
    }
}
