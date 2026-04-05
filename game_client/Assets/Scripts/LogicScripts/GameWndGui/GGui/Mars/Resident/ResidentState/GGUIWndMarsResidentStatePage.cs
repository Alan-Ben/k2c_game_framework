using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星基地 - 居民状态页面
    /// </summary>
    public class GGUIWndMarsResidentStatePage : _ANPGGUIBasicLoadPrefabSubWnd<GGUIMonoMarsResidentStatePage>
    {
        private NPCommonAssetPathInfo _m_iAssetPathInfo;
        // 派遣Item Grid子窗口
        private GGUIWndMarsResidentDispatchItemGrid _m_wDispatchItemGrid;
        
        public GGUIWndMarsResidentStatePage(Transform _parent, NPCommonAssetPathInfo _assetPathInfo) : base(_parent)
        {
            _m_iAssetPathInfo = _assetPathInfo;
        }

        protected override string _monoAssetPath { get { return _m_iAssetPathInfo?.asset_path; } }
        protected override string _monoObjName { get { return _m_iAssetPathInfo?.obj_name; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            // 创建派遣Grid子窗口
            if (wnd.monoDispatchItemGrid != null)
                _m_wDispatchItemGrid = new GGUIWndMarsResidentDispatchItemGrid(wnd.monoDispatchItemGrid);

            ALUGUICommon.combineBtnClick(wnd.btnReplenish, _onClickReplenishBtn);
            ALUGUICommon.combineBtnClick(wnd.btnGoToBuildBuilding, _onClickGoToBuildBuildingBtn);
        }

        protected override void _onDiscard()
        {
            // 解绑按钮
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnReplenish, _onClickReplenishBtn);
                ALUGUICommon.uncombineBtnClick(wnd.btnGoToBuildBuilding, _onClickGoToBuildBuildingBtn);
            }

            // 释放子窗口
            _m_wDispatchItemGrid?.discard();
            _m_wDispatchItemGrid = null;
        }

        protected override void _onShowWnd()
        {
            // 注册消息事件
            WinMsg.RegisterMsgAct(WinMsgType.ON_MARS_PEOPLE_NUM_CHG, _onMarsPeopleNumChg);
            if (NPPlayer.instance?.marsComp != null)
            {
                NPPlayer.instance.marsComp.onPeopleNumLimitChanged += _onPeopleNumLimitChanged;
                NPPlayer.instance.marsComp.onSettleSlotPeopleLimitChanged += _onSettleSlotPeopleLimitChanged;
            }

            refreshWnd();
        }

        protected override void _onHideWnd()
        {
            // 注销消息事件
            WinMsg.UnregisterMsgAct(WinMsgType.ON_MARS_PEOPLE_NUM_CHG, _onMarsPeopleNumChg);
            if (NPPlayer.instance?.marsComp != null)
            {
                NPPlayer.instance.marsComp.onPeopleNumLimitChanged -= _onPeopleNumLimitChanged;
                NPPlayer.instance.marsComp.onSettleSlotPeopleLimitChanged -= _onSettleSlotPeopleLimitChanged;
            }

            // 隐藏子窗口
            _m_wDispatchItemGrid?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wDispatchItemGrid?.resetWnd();
        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        public void refreshWnd()
        {
            _refreshResidentTotalAndLimit();
            _refreshResidentStatusNums();
            _refreshDispatchNums();
            _refreshDispatchOrGotoButtons();
            _refreshDispatchGrid();
        }

        /// <summary>
        /// 刷新居民总数和上限显示
        /// </summary>
        private void _refreshResidentTotalAndLimit()
        {
            if (wnd == null || wnd.txtGalleryful == null || NPPlayer.instance?.marsComp == null)
                return;

            long totalPeopleNum = NPPlayer.instance.marsComp.totalPeopleNum;
            long peopleNumLimit = NPPlayer.instance.marsComp.peopleNumLimit;
            ALUGUICommon.setLabelTxt(wnd.txtGalleryful, TextTranslate.instance.getLanguage(TransKeyConst.mars_resident_canContainNum_num_num, totalPeopleNum, peopleNumLimit));
        }

        /// <summary>
        /// 刷新居民状态数量显示
        /// </summary>
        private void _refreshResidentStatusNums()
        {
            if (wnd == null || NPPlayer.instance?.marsComp == null)
                return;

            MarsPeopleSubComponent peopleComp = NPPlayer.instance.marsComp.peopleSubComponent;
            ALUGUICommon.setLabelTxt(wnd.txtWorkingNum, peopleComp.workingPeopleNum.ToString());
            ALUGUICommon.setLabelTxt(wnd.txtSickNum, peopleComp.sickPeopleNum.ToString());
            ALUGUICommon.setLabelTxt(wnd.txtRestNum, peopleComp.idlePeopleNum.ToString());
        }

        /// <summary>
        /// 刷新可派遣人数显示
        /// </summary>
        private void _refreshDispatchNums()
        {
            if (wnd == null || wnd.canDispatchNum == null || NPPlayer.instance?.marsComp == null)
                return;

            long dispatchedNum = NPPlayer.instance.marsComp.dispatchedPeopleNum;
            long slotLimit = NPPlayer.instance.marsComp.settleSlotPeopleLimit;
            ALUGUICommon.setLabelTxt(wnd.canDispatchNum, TextTranslate.instance.getLanguage(TransKeyConst.mars_resident_canDispatchNum_num_num, dispatchedNum, slotLimit));
        }

        /// <summary>
        /// 刷新派遣或前往建造按钮显示
        /// </summary>
        private void _refreshDispatchOrGotoButtons()
        {
            if (wnd == null || NPPlayer.instance?.marsComp == null)
                return;

            long totalPeopleNum = NPPlayer.instance.marsComp.totalPeopleNum;
            long peopleNumLimit = NPPlayer.instance.marsComp.peopleNumLimit;

            // 是否可补充居民(当前小于上限)
            bool canReplenish = totalPeopleNum < peopleNumLimit;

            ALUGUICommon.setGameObjEnable(wnd.btnReplenish, canReplenish);
            ALUGUICommon.setGameObjEnable(wnd.btnGoToBuildBuilding, !canReplenish);
        }

        /// <summary>
        /// 刷新派遣Item列表显示
        /// </summary>
        private void _refreshDispatchGrid()
        {
            if (_m_wDispatchItemGrid == null)
                return;
            
            _m_wDispatchItemGrid.showWnd();
            _m_wDispatchItemGrid.refreshGrid();
        }

        // 按钮点击事件
        private void _onClickReplenishBtn(GameObject _go)
        {
            if(wnd == null || wnd.btnReplenishClickEffect == null || wnd.btnReplenishClickEffect.isEmpty)
                GCommon.enterUIMainNodeShow(ESysSceneType.MARS_RESIDENT_REPLENISH);
            else
                wnd.btnReplenishClickEffect.dealEffect();
        }

        /// <summary>
        /// 点击前往建筑建造按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickGoToBuildBuildingBtn(GameObject _go)
        {
            MarsBuildingInfo targetLivingBuilding = MarsUtil.getCanBuildOrUpdateLivingBuilding();//聚焦的居住舱
            if(targetLivingBuilding == null)
                return;
            
            // 关闭窗口
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MARS_RESIDENT);
            
            MarsUtil.jumpToBuildingUpgrade(targetLivingBuilding);
        }

        #region 事件回调

        /// <summary>
        /// 当居民数量有变化时
        /// </summary>
        private void _onMarsPeopleNumChg()
        {
            // 刷新总数和上限显示
            _refreshResidentTotalAndLimit();
            // 刷新状态数量显示
            _refreshResidentStatusNums();
            // 刷新可派遣人数显示
            _refreshDispatchNums();
            // 刷新派遣或前往建造按钮显示
            _refreshDispatchOrGotoButtons();
            
            // 不用在这里刷新Grid, 因为Grid内部会监听人数变化消息自行刷新
        }

        /// <summary>
        /// 当可容纳人数上限变化时
        /// </summary>
        /// <param name="_value"></param>
        private void _onPeopleNumLimitChanged(long _value)
        {
            // 刷新总数和上限显示
            _refreshResidentTotalAndLimit();
            // 刷新派遣或前往建造按钮显示
            _refreshDispatchOrGotoButtons();
        }

        /// <summary>
        /// 当可派遣人数上限变化时
        /// </summary>
        /// <param name="_value"></param>
        private void _onSettleSlotPeopleLimitChanged(long _value)
        {
            // 刷新可派遣人数显示
            _refreshDispatchNums();
            
            // 不用在这里刷新Grid, 因为Grid内部会监听变化消息自行刷新
        }

        #endregion
    }
}