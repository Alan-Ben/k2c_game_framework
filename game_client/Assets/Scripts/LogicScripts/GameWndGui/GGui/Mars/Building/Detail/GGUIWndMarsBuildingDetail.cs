using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星建筑详情窗口
    /// </summary>
    public class GGUIWndMarsBuildingDetail : _ATALBasicUIWnd<GGUIMonoMarsBuildingDetail>
    {
        private _IMarsBuildingView _m_buildingView;
        private List<MasrPropertyShowInfo> _m_propertyShowList;
        
        private NPGGuiWndTexture _m_buildingIconWnd;
        private GGUIWndMarsPropertyShowItemContainer _m_propertyShowContainerWnd;

        private string _m_sAssetPath;
        private string _m_sObjName;

        private new bool _m_bIsShow;
        
        public GGUIWndMarsBuildingDetail(string _sAssetPath, string _sObjName) : base(EALUIWndLayer.ADDITION)
        {
            _m_sAssetPath = _sAssetPath;
            _m_sObjName = _sObjName;
        }

        protected override string _monoAssetPath { get { return _m_sAssetPath; } }
        protected override string _monoObjName { get { return _m_sObjName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            _m_bIsShow = true;
            
            _refreshWnd();
            
            _trySelectBuildingView();
        }
        protected override void _onHideWnd()
        {
            _tryUnselectBuildingView();
            
            _m_buildingIconWnd?.hideWnd();
            _m_propertyShowContainerWnd?.hideWnd();
            
            _m_bIsShow = false;
        }
        protected override void _onReset()
        {
            _m_buildingIconWnd?.discardTexture();
            _m_propertyShowContainerWnd?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_buildingIconWnd?.discard();
            _m_buildingIconWnd = null;
            
            _m_propertyShowContainerWnd?.discard();
            _m_propertyShowContainerWnd = null;

            _m_propertyShowList?.Clear();
            _m_propertyShowList = null;
            
            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnReturn, _onBtnCloseClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnCancelUpgrade, _onBtnCancelUpgradeClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnMoreDetail, _onBtnMoreDetailClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            // 构建建筑图标
            if (wnd.buildingIcon != null)
                _m_buildingIconWnd = new NPGGuiWndTexture(wnd.buildingIcon);
            
            // 构建属性显示容器
            if (wnd.monoPropertyShowContainer != null)
                _m_propertyShowContainerWnd = new GGUIWndMarsPropertyShowItemContainer(wnd.monoPropertyShowContainer);
            
            // 绑定按钮点击事件
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
            ALUGUICommon.combineBtnClick(wnd.btnReturn, _onBtnCloseClick);
            ALUGUICommon.combineBtnClick(wnd.btnCancelUpgrade, _onBtnCancelUpgradeClick);
            ALUGUICommon.combineBtnClick(wnd.btnMoreDetail, _onBtnMoreDetailClick);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="_buildingView">建筑视图数据</param>
        public void setData(_IMarsBuildingView _buildingView)
        {
            _m_buildingView = _buildingView;
            if (_m_propertyShowList == null)
                _m_propertyShowList = new List<MasrPropertyShowInfo>();
            _m_propertyShowList.Clear();
            _m_buildingView?.buildingInfo?.levelData.refObj?.dealAllProperty((propertyShow, value) =>
            {
                if(propertyShow == null)
                    return;
                
                _m_propertyShowList.Add(new MasrPropertyShowInfo()
                {
                    propertyShow = propertyShow,
                    nowValue = value,
                });
            });

            _tryUnselectBuildingView();
            
            _refreshWnd();

            _trySelectBuildingView();
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
            
            // 刷新建筑名称
            ALUGUICommon.setLabelTxt(wnd.txtBuildingName, buildingInfo.nameTranslated);
            
            // 刷新建筑等级
            ALUGUICommon.setLabelTxt(wnd.txtLevel, TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, buildingInfo.level));
            
            // 刷新建筑描述
            ALUGUICommon.setLabelTxt(wnd.txtDesc, buildingInfo.descTranslated);

            if (_m_buildingIconWnd != null)
            {
                _m_buildingIconWnd.showWnd();
                _m_buildingIconWnd.setTexture(buildingInfo.refObj.building_icon);
            }

            if (_m_propertyShowContainerWnd != null)
            {
                _m_propertyShowContainerWnd.showWnd();
                _m_propertyShowContainerWnd.setData(_m_propertyShowList);
            }
            
            // 刷新状态显示
            NPCommonEnumStatMutexShowInfo<MarsBuildingInfo.StateType>.setStat(wnd.stateTypeShowList, buildingInfo.state);
        }

        /// <summary>
        /// 关闭按钮点击事件
        /// </summary>
        /// <param name="_obj">点击的GameObject</param>
        private void _onBtnCloseClick(GameObject _obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Mars.C_MARS_EXPAND_BUILDING_DETAIL);
        }
        
        /// <summary>
        /// 取消升级按钮点击事件
        /// </summary>
        /// <param name="_obj">点击的GameObject</param>
        private void _onBtnCancelUpgradeClick(GameObject _obj)
        {
            if (_m_buildingView == null || _m_buildingView.buildingInfo == null || 
                _m_buildingView.buildingInfo.state != MarsBuildingInfo.StateType.Upgrading)
                return;
            
            NPPlayer.instance.marsComp.buildingSubComponent.cancelUpgrade(_m_buildingView.buildingInfo.buildRefId, ()=>
            {
                _refreshWnd();
            });
        }
        /// <summary>
        /// 查看更多详情按钮点击事件
        /// </summary>
        /// <param name="_obj">点击的GameObject</param>
        private void _onBtnMoreDetailClick(GameObject _obj)
        {
            if (_m_buildingView == null)
                return;
            
            // 打开更多详情窗口
            GGUIWndMarsBuildingMoreDetail.instance.setData(_m_buildingView);
            QueueMgr.instance.addNode_InGame_SingleWnd_OnlyCloseDiscard(GGUIWndMarsBuildingMoreDetail.instance, 
                GGUIWndMarsBuildingMoreDetail.instance.showWnd, EUIQueueStageType.MAIN, UINodeTagConst_Mars.C_MARS_EXPAND_BUILDING_MORE_DETAIL, false);
        }
        
        private void _trySelectBuildingView()
        {
            if (wnd == null || !_m_bIsShow || _m_buildingView == null)
                return;

            _m_buildingView.setSelected(true);
            MainAdditionMarsTDScene.instance.focusToPos(_m_buildingView.position, wnd.focusViewportPos, wnd.focusScale, wnd.focusTime);
        }
        private void _tryUnselectBuildingView()
        {
            if (wnd == null || !_m_bIsShow || _m_buildingView == null)
                return;

            _m_buildingView?.setSelected(false);
            MainAdditionMarsTDScene.instance.cancelThePosFocus(wnd.focusTime);
        }
    }
}