using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星能源建筑详情窗口
    /// </summary>
    public class GGUIWndMarsBuildingEnergyDetail : _ATALBasicUIWnd<GGUIMonoMarsBuildingEnergyDetail>
    {
        [NotNull] public static GGUIWndMarsBuildingEnergyDetail instance { get { return _g_instance ??= new GGUIWndMarsBuildingEnergyDetail(); } }
        private static GGUIWndMarsBuildingEnergyDetail _g_instance;

        // 建筑视图数据
        private _IMarsBuildingView _m_buildingView;
        // 等级属性显示列表数据
        private List<_IPropertyShow> _m_lAllPropertyList;
        private List<MarsLvlPropertyShowInfo> _m_lLevelPropertyInfoList;

        // 等级属性显示Grid
        private GGUIWndMarsLvlPropertyShowItemGrid _m_lvlPropertyShowGridWnd;
        // tick任务控制器
        // private ALCommonEnableTaskController _m_tickTask;


        public GGUIWndMarsBuildingEnergyDetail() : base(EALUIWndLayer.ADDITION)
        {
        }


        protected override string _monoAssetPath { get { return GGUIMonoMarsBuildingEnergyDetail.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMarsBuildingEnergyDetail.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onShowWnd()
        {
            _m_lvlPropertyShowGridWnd?.showWnd();

            // 开启tick任务
            // _m_tickTask = CommonTaskController.CommonEnableDurationActionAddMonoTask(_tick, 60f);

            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            // 关闭tick任务
            // _m_tickTask.setDisable();

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

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClick);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            // 构建等级属性显示Grid
            if (wnd.lvlPropertyShowGrid != null)
                _m_lvlPropertyShowGridWnd = new GGUIWndMarsLvlPropertyShowItemGrid(wnd.lvlPropertyShowGrid);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
        }


        /// <summary>
        /// 设置建筑数据并刷新（带参数版本）
        /// </summary>
        /// <param name="_buildingView">建筑视图数据</param>
        public void setData(_IMarsBuildingView _buildingView)
        {
            _m_buildingView = _buildingView;

            // 初始化属性列表
            if (_m_lAllPropertyList == null)
                _m_lAllPropertyList = new List<_IPropertyShow>();
            _m_lAllPropertyList.Clear();

            if (_m_lLevelPropertyInfoList == null)
                _m_lLevelPropertyInfoList = new List<MarsLvlPropertyShowInfo>();
            _m_lLevelPropertyInfoList.Clear();

            // 获取建筑等级属性显示信息
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

            // 刷新能量产出数据
            _refreshEnergyData();
        }

        /// <summary>
        /// 刷新等级属性显示
        /// </summary>
        private void _refreshLvlPropertyShow()
        {
            if (!_m_bIsShow || _m_lvlPropertyShowGridWnd == null || _m_buildingView == null || _m_buildingView.buildingInfo == null)
                return;

            _m_lvlPropertyShowGridWnd.showWnd();
            // 设置数据到Grid
            _m_lvlPropertyShowGridWnd.setData(_m_lLevelPropertyInfoList, _m_lAllPropertyList, _m_buildingView.buildingInfo.level);
        }

        /// <summary>
        /// 刷新能量相关数据
        /// </summary>
        private void _refreshEnergyData()
        {
            if (wnd == null || !_m_bIsShow || _m_buildingView == null)
                return;

            MarsBuildingInfo buildingInfo = _m_buildingView.buildingInfo;
            if (buildingInfo == null)
                return;

            // 刷新能量产出速度
            long energyYieldSpeed = buildingInfo.energyProperty.value;
            ALUGUICommon.setLabelTxt(wnd.txtOutputSpeed, 
                TextTranslate.instance.getLanguage(TransKeyConst.mars_commonPerMin_num, energyYieldSpeed.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));

            // 刷新能量存储量
            _refreshEnergyStorage();
        }

        /// <summary>
        /// 刷新能量存储量显示
        /// </summary>
        private void _refreshEnergyStorage()
        {
            if (wnd == null || !_m_bIsShow || _m_buildingView == null)
                return;

            MarsBuildingInfo buildingInfo = _m_buildingView.buildingInfo;
            if (buildingInfo == null)
                return;

            // 获取当前存储能量和最大存储量
            long storedEnergy = buildingInfo.energyProperty.storedEnergy;
            long maxStorage = buildingInfo.energyProperty.maxStorage;

            // 设置能量存储文本 (累积总量:{0}/{1})
            ALUGUICommon.setLabelTxt(wnd.txtEnergyStorage,
                TextTranslate.instance.getLanguage(TransKeyConst.common_useNum_num_num, 
                    storedEnergy.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT), 
                    maxStorage.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));
        }

        /// <summary>
        /// tick刷新（用于实时更新能量存储量）
        /// </summary>
        private void _tick()
        {
            _refreshEnergyStorage();
        }


        /// <summary>
        /// 关闭按钮点击
        /// </summary>
        private void _onBtnCloseClick(GameObject _obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Mars.C_MARS_BUILDING_ENERGY_DETAIL);
        }
    }
}
