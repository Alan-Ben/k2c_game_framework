using ALPackage;
using Common.NpChatObj;
using GS2GC.p014_ChildOp;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 子嗣分享组队弹窗
    /// </summary>
    public class GGUIWndShareChildTeamUp : _ANPGGUIBasicWnd<GGUIMonoShareChildTeamUp>
    {
        private static GGUIWndShareChildTeamUp _g_instance;
        public static GGUIWndShareChildTeamUp instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GGUIWndShareChildTeamUp();
                return _g_instance;
            }
        }

        //子嗣分享信息
        private NPCommon_ChatContent_ChildShare _m_childShareInfo;
        //目标子嗣头像
        private NPGGuiWndTexture _m_wTargetIcon;
        //自己未组队子嗣列表
        private GGUISubWndAdultUnmarriedGrid _m_wSelfUnmarriedGrid;
        //未组队子嗣列表
        private List<UnmarriedInfo> _m_unmarriedInfoList;
        //倒计时任务
        private ALCommonEnableTaskController _m_timeTask;
        //正在请求处理的子嗣id列表
        private HashSet<long> _m_lDealingAdultIdList;

        public GGUIWndShareChildTeamUp() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get => GGUIMonoShareChildTeamUp.assetPath; }
        protected override string _monoObjName { get => GGUIMonoShareChildTeamUp.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }

        protected override void _onShowWnd()
        {
            NPPlayer.instance.childComp.onUnmarriedAdultStateChg += _stateChg;
            _m_timeTask = ALCommonEnableDurationActionMonoTask.addMonoTask(_timeTask, 0.2f);
        }

        protected override void _onHideWnd()
        {
            NPPlayer.instance.childComp.onUnmarriedAdultStateChg -= _stateChg;
            _m_timeTask.setDisable();
            _m_lDealingAdultIdList?.Clear();
            _m_lDealingAdultIdList = null;
            _m_wSelfUnmarriedGrid?.hideWnd();
            _m_wTargetIcon?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wSelfUnmarriedGrid?.resetWnd();
            _m_wTargetIcon?.discardTexture();
        }

        protected override void _onDiscard()
        {
            _m_wSelfUnmarriedGrid?.discard();
            _m_wSelfUnmarriedGrid = null;
            _m_wTargetIcon?.discard();
            _m_wTargetIcon = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgTargetIcon != null)
                _m_wTargetIcon = new NPGGuiWndTexture(wnd.imgTargetIcon);

            if (wnd.monoAdultGrid != null)
            {
                _m_wSelfUnmarriedGrid = new GGUISubWndAdultUnmarriedGrid(wnd.monoAdultGrid);
                _m_wSelfUnmarriedGrid.onClickItemApply += _onClickItemApply;
            }

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_info"></param>
        public void setInfo(NPCommon_ChatContent_ChildShare _info)
        {
            _m_childShareInfo = _info;
            _refreshWnd();
        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            _refreshTargetChild();
            _refreshUnmarriedGrid();
        }

        //刷新目标子嗣信息
        private void _refreshTargetChild()
        {
            if (wnd == null || _m_childShareInfo == null)
                return;

            //头像
            ChildResRefObj childResRef = GRefdataCoreMgr.instance.childResCore.getRef(_m_childShareInfo.getChildResId());
            _m_wTargetIcon?.showWnd();
            _m_wTargetIcon?.setTexture(childResRef?.icon);

            //名称
            ALUGUICommon.setLabelTxt(wnd.txtTargetName, 
                string.IsNullOrEmpty(wnd.targetNameKey) ? 
                _m_childShareInfo.getChildName() : 
                TextTranslate.instance.getLanguage(wnd.targetNameKey, _m_childShareInfo.getChildName()));

            //赚速
            ALUGUICommon.setLabelTxt(wnd.txtTargetEarnings, string.IsNullOrEmpty(wnd.targetEarningsKey) ?
                _m_childShareInfo.getEarnings().ToLargeString(PrimitiveExtension.ELargeStringType.GOLD) : 
                TextTranslate.instance.getLanguage(wnd.targetEarningsKey, _m_childShareInfo.getEarnings().ToLargeString(PrimitiveExtension.ELargeStringType.GOLD)));

            //是否卷王
            ALUGUICommon.setGameObjEnable(wnd.listTargetSuperShow, _m_childShareInfo.getIsSuper());
        }

        //刷新自己未组队子嗣列表
        private void _refreshUnmarriedGrid()
        {
            if (_m_unmarriedInfoList == null)
                _m_unmarriedInfoList = new List<UnmarriedInfo>();

            //获取未组队子嗣列表
            _m_unmarriedInfoList.Clear();
            NPPlayer.instance.childComp.getUnmarriedChildListNonAlloc(_m_unmarriedInfoList);
            _m_unmarriedInfoList.Sort((_a, _b) => _a.status.CompareTo(_b.status));

            //展示列表
            _m_wSelfUnmarriedGrid?.showWnd();
            _m_wSelfUnmarriedGrid?.refreshWnd(_m_unmarriedInfoList);
        }

        /// <summary>
        /// 子嗣状态变更
        /// </summary>
        private void _stateChg()
        {
            _refreshUnmarriedGrid();
        }

        /// <summary>
        /// 倒计时任务
        /// </summary>
        private void _timeTask()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            _m_wSelfUnmarriedGrid?.refreshAllItem((_item, _index) => _item.refreshTime());
        }

        #region 点击事件

        /// <summary>
        /// 点击申请组队按钮
        /// </summary>
        /// <param name="_info"></param>
        private void _onClickItemApply(UnmarriedInfo _info)
        {
            if (_info == null || _m_childShareInfo == null)
                return;

            long cid = _m_childShareInfo.getCid();
            long adultId = _info.adultId;

            if (_m_lDealingAdultIdList == null)
                _m_lDealingAdultIdList = new HashSet<long>();

            //如果该子嗣正在请求处理组队，不再处理，避免快速点击
            if (_m_lDealingAdultIdList.Contains(adultId))
                return;

            //添加进正在处理列表
            _m_lDealingAdultIdList.Add(adultId);

            //请求组队
            NPGSClientListener.sendRequestByLog(GSWriter_014_ChildOp.make_013_ReqApplyToPlayer(cid, adultId),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_014_013_RetApplyToPlayer>((_isSuc, _msg) =>
                {
                    //移除正在处理的子嗣id
                    if(_m_lDealingAdultIdList != null && _m_lDealingAdultIdList.Contains(adultId))
                        _m_lDealingAdultIdList.Remove(adultId);

                    //如果对方设置了拒绝组队，弹出提示
                    if (_msg != null && _msg.getIsPlayerRefuseAllRequest())
                    {
                        NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.child_engagedTargetRefuseAllRequest_none);
                        return;
                    }
                }));
        }

        /// <summary>
        /// 点击关闭按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_CHAT_SHARE_CHILD_TEAM_UP);
        }

        #endregion
    }
}