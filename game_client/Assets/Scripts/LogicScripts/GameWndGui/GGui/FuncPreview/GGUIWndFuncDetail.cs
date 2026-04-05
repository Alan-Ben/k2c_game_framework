using System.Collections.Generic;
using ALPackage;
using DG.Tweening;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 功能详情弹窗
    /// </summary>
    public class GGUIWndFuncDetail : _ANPGGUIBasicWnd<GGUIMonoFuncDetail>
    {
        private static GGUIWndFuncDetail _g_instance;
        public static GGUIWndFuncDetail instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndFuncDetail();
                return _g_instance;
            }
        }

        //功能信息列表
        private List<FuncUnlockInfo> _m_lFuncUnlockList;
        //功能列表
        private GGUIWndFuncDetailGrid _m_WDetailGrid;
        //系统图片
        private NPGGuiWndTexture _m_wBanner;
        //解锁信息
        private FuncUnlockInfo _m_curSelectFuncUnlockInfo;
        //奖励列表
        private GGUIWndCommonRewardContainer _m_wItemContainer;

        protected GGUIWndFuncDetail() : base(EALUIWndLayer.ADDITION)
        {
        }

        /********************
       * 获取资源所在资源加载文件名称
       **/
        protected override string _monoAssetPath { get { return GGUIMonoFuncDetail.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoFuncDetail.objName; } }

        /**************
         * 获取用于加载资源的管理对象
         **/
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        protected override bool isShowAniPlayOnlyOne { get => true; }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsgAct(WinMsgType.ON_FUNC_UNLOCK_GET_REWARD, _onFuncUnlockGetReward);
            _m_lFuncUnlockList = new List<FuncUnlockInfo>();
            NPPlayer.instance.funcUnlockComp.getNotIgnoreFuncUnlock(_m_lFuncUnlockList);
            _m_lFuncUnlockList.Sort(sortInfoList);
            _setContainerList();
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_FUNC_UNLOCK_GET_REWARD, _onFuncUnlockGetReward);
            _m_WDetailGrid?.hideWnd();
            _m_wBanner?.hideWnd();
            _m_wItemContainer?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_WDetailGrid?.resetWnd();
            _m_wBanner?.discardTexture();
            _m_wItemContainer?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_WDetailGrid?.discard();
            _m_WDetailGrid = null;
            _m_wBanner?.discard();
            _m_wBanner = null;
            _m_wItemContainer?.discard();
            _m_wItemContainer = null;
            _m_curSelectFuncUnlockInfo = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.uncombineBtnClick(wnd.btnLast, _onClickLast);
            ALUGUICommon.uncombineBtnClick(wnd.btnNext, _onClickNext);
            ALUGUICommon.uncombineBtnClick(wnd.btnGetReward, _onClickGetReward);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoFuncGrid != null)
            {
                _m_WDetailGrid = new GGUIWndFuncDetailGrid(wnd.monoFuncGrid);
                _m_WDetailGrid.onSelect += _onSelectFunc;
            }

            if (wnd.imgBanner != null)
                _m_wBanner = new NPGGuiWndTexture(wnd.imgBanner);

            if (wnd.monoItemContainer != null)
                _m_wItemContainer = new GGUIWndCommonRewardContainer(wnd.monoItemContainer);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.combineBtnClick(wnd.btnLast, _onClickLast);
            ALUGUICommon.combineBtnClick(wnd.btnNext, _onClickNext);
            ALUGUICommon.combineBtnClick(wnd.btnGetReward, _onClickGetReward);
        }

        //设置功能列表
        private void _setContainerList()
        {
            if (_m_WDetailGrid != null)
            {
                _m_WDetailGrid.showWnd();
                _m_WDetailGrid.showItemList(_m_lFuncUnlockList);

                //如果上次有选中，则继续选中上次的
                if (_m_curSelectFuncUnlockInfo != null)
                {
                    _m_WDetailGrid.setSelectInfo(_m_curSelectFuncUnlockInfo);
                    _refreshWnd();
                }
            }
        }

        //刷新窗口
        private void _refreshWnd()
        {
            _refreshFuncInfo();
            _refreshState();
            _refreshProgress();
        }

        //刷新系统信息
        private void _refreshFuncInfo()
        {
            if (_m_curSelectFuncUnlockInfo == null)
                return;

            //设置图片
            if (_m_wBanner != null)
            {
                _m_wBanner.showWnd();
                _m_wBanner.setTexture(_m_curSelectFuncUnlockInfo.texBanner);
            }
            //设置名称
            ALUGUICommon.setLabelTxt(wnd.txtName, _m_curSelectFuncUnlockInfo.funcName);
            //设置描述
            ALUGUICommon.setLabelTxt(wnd.txtDesc, _m_curSelectFuncUnlockInfo.funcDesc);
        }

        //刷新解锁状态
        private void _refreshState()
        {
            if (_m_curSelectFuncUnlockInfo == null || wnd == null)
                return;

            //是否已领奖
            bool hasGetReward = _m_curSelectFuncUnlockInfo.hasGetReward;
            //是否已解锁
            bool isUnlock = _m_curSelectFuncUnlockInfo.isUnlock;
            ECommonRewardType rewardType;
            if (hasGetReward)
                rewardType = ECommonRewardType.HAS_GET_REWARD;
            else
            {
                if (isUnlock)
                    rewardType = ECommonRewardType.CAN_GET_REWARD;
                else
                    rewardType = ECommonRewardType.NOT_GET_REWARD;
            }

            //设置奖励列表
            if (_m_wItemContainer != null && _m_curSelectFuncUnlockInfo.functionUnlockRef != null)
            {
                _m_wItemContainer.showWnd();
                _m_wItemContainer.setRewardList(_m_curSelectFuncUnlockInfo.functionUnlockRef.gain_item_list, rewardType);
            }

            //解锁条件描述
            NPSimpleUnlockRef simpleUnlockRef = GRefdataCoreMgr.instance.simpleUnlockMap.getRef(_m_curSelectFuncUnlockInfo.functionUnlockRef.simple_unlock_id);
            if (simpleUnlockRef != null)
            {
                string unlockDesc = TextTranslate.instance.getLanguage(TransKeyConst.funcUnlock_unlockCond_str, TextTranslate.instance.getLanguage(simpleUnlockRef.unlock_tip, simpleUnlockRef.unlock_tip_args));
                //根据解锁状态设置颜色
                unlockDesc = GCommon.addColorForRichText(unlockDesc, isUnlock ? wnd.condDescUnlockColor : wnd.condDescLockColor);
                ALUGUICommon.setLabelTxt(wnd.txtUnlockCondDesc, unlockDesc);
            }

            //设置奖励按钮状态
            wnd.btnSstateAniList?.forcePlay(rewardType);

            //设置解锁显示状态
            ALUGUICommon.setGameObjEnable(wnd.goLockShowList, !isUnlock);
            ALUGUICommon.setGameObjEnable(wnd.goUnlockShowList, isUnlock);

            //设置左右按钮显隐
            ALUGUICommon.setGameObjEnable(wnd.btnLast,_m_curSelectFuncUnlockInfo != null && 
                                                      _m_lFuncUnlockList != null && 
                                                      _m_lFuncUnlockList.Count > 0 && 
                                                      _m_curSelectFuncUnlockInfo.funcId != _m_lFuncUnlockList[0]?.funcId);
            ALUGUICommon.setGameObjEnable(wnd.btnNext, _m_curSelectFuncUnlockInfo != null &&
                                                       _m_lFuncUnlockList != null &&
                                                       _m_lFuncUnlockList.Count > 0 &&
                                                       _m_curSelectFuncUnlockInfo.funcId != _m_lFuncUnlockList[_m_lFuncUnlockList.Count - 1]?.funcId);
        }

        //刷新进度条
        private void _refreshProgress()
        {
            if (wnd == null || _m_curSelectFuncUnlockInfo == null)
                return;

            //设置进度
            long curCount = _m_curSelectFuncUnlockInfo.functionUnlockRef.process_cur_num.CalculateVariableResult(null);
            long targetCount = _m_curSelectFuncUnlockInfo.functionUnlockRef.process_max_num;
            bool isFinish = curCount >= targetCount;
            //设置文本
            //获取对应格式进度字符串
            string curCountStr = GCommon.getValueFormatStr(_m_curSelectFuncUnlockInfo.functionUnlockRef.process_num_format, curCount);
            string targetCountStr = GCommon.getValueFormatStr(_m_curSelectFuncUnlockInfo.functionUnlockRef.process_num_format, targetCount);
            string progressStr = TextTranslate.instance.getLanguage(TransKeyConst.common_currentTotalNum_num_num, curCountStr, targetCountStr);
            progressStr = GCommon.addColorForRichText(progressStr, isFinish ? wnd.finishProgressTextColor : wnd.notFinishProgressTextColor);
            ALUGUICommon.setLabelTxt(wnd.txtProgress, TextTranslate.instance.getLanguage(TransKeyConst.quest_questProgress_str, progressStr));

            //设置进度条，如果是关卡类型的不显示进度条
            if (_m_curSelectFuncUnlockInfo.functionUnlockRef.process_num_format == EValueFormatType.PLAYER_CHAPTER)
                ALUGUICommon.setGameObjEnable(wnd.sldProgress, false);
            else
            {
                ALUGUICommon.setGameObjEnable(wnd.sldProgress, true);
                if (targetCount == 0)
                    ALUGUICommon.setSliderScale(wnd.sldProgress, 1);
                else
                    ALUGUICommon.setSliderScale(wnd.sldProgress, 1.0f * curCount / targetCount);
            }
        }

        //显示奖励预览弹窗
        private void _showRewardPreview(RectTransform _toolTipShowRoot, ECommonRewardType _rewardType)
        {
            if (wnd == null || wnd.rewardPreviewResPathId <= 0 || _m_curSelectFuncUnlockInfo == null || _m_curSelectFuncUnlockInfo.functionUnlockRef == null)
                return;

            long uiPathId = wnd.rewardPreviewResPathId;
            string titleStr = string.IsNullOrEmpty(wnd.rewardPreviewTitleStrKey) ? "" : TextTranslate.instance.getLanguage(wnd.rewardPreviewTitleStrKey);

            QueueMgr.instance.AddNode(new GNodeCommonToolTip_Reward(
                uiPathId,
                titleStr,
                string.Empty,
                _m_curSelectFuncUnlockInfo.functionUnlockRef.gain_item_list,
                _toolTipShowRoot,
                wnd.rewardPreviewToolTipOffset,
                _rewardType));
        }

        //排序：可领取奖励>未解锁>已领取，排序id从小到大
        private int sortInfoList(FuncUnlockInfo _a, FuncUnlockInfo _b)
        {
            if (_a == null || _b == null)
                return 0;

            bool isUnlockA = _a.isUnlock;
            bool isUnlockB = _b.isUnlock;
            bool hasGetRewardA = _a.hasGetReward;
            bool hasGetRewardB = _b.hasGetReward;
            if (hasGetRewardA.CompareTo(hasGetRewardB) != 0)
                return hasGetRewardA.CompareTo(hasGetRewardB);
            else if (isUnlockA.CompareTo(isUnlockB) != 0)
                return -(isUnlockA.CompareTo(isUnlockB));
            else
                return _a.sortId.CompareTo(_b.sortId);
        }

        #region 点击事件

        //选择item
        private void _onSelectFunc(FuncUnlockInfo _info, int _index)
        {
            if (_info == null || _m_lFuncUnlockList == null)
                return;

            //点击的不是同一个item刷新界面
            if(_m_curSelectFuncUnlockInfo == null || _m_curSelectFuncUnlockInfo.funcId != _info.funcId)
            {
                _m_curSelectFuncUnlockInfo = _info;
                _refreshWnd();
            }

            //设置超出的item移动到里面
            GCommon.setGridMoveItemWithinRangeInHorizontal(_m_WDetailGrid?.wnd, _index, _m_lFuncUnlockList.Count);
        }

        //点击领取奖励
        private void _onClickGetReward(GameObject _go)
        {
            if(_go == null || _m_curSelectFuncUnlockInfo == null)
                return;

            //是否已领奖
            bool hasGetReward = _m_curSelectFuncUnlockInfo.hasGetReward;
            //是否已解锁
            bool isUnlock = _m_curSelectFuncUnlockInfo.isUnlock;
            ECommonRewardType rewardType;
            if (hasGetReward)
                rewardType = ECommonRewardType.HAS_GET_REWARD;
            else
            {
                if (isUnlock)
                    rewardType = ECommonRewardType.CAN_GET_REWARD;
                else
                    rewardType = ECommonRewardType.NOT_GET_REWARD;
            }

            switch (rewardType)
            {
                case ECommonRewardType.CAN_GET_REWARD:
                    NPPlayer.instance.funcUnlockComp.reqDoneFuncUnlock(_m_curSelectFuncUnlockInfo.funcType, null);
                    break;
                case ECommonRewardType.HAS_GET_REWARD:
                case ECommonRewardType.NOT_GET_REWARD:
                    _showRewardPreview((RectTransform)_go.transform, rewardType);
                    break;
            }
        }

        //点击关闭窗口
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_FUNC_DETAIL);
        }

        //点击上一个按钮
        private void _onClickLast(GameObject _go)
        {
            _m_WDetailGrid?.setSelectLast();
        }

        //点击下一个按钮
        private void _onClickNext(GameObject _go)
        {
            _m_WDetailGrid?.setSelectNext();
        }

        #endregion

        #region 消息事件

        //领取功能解锁奖励
        private void _onFuncUnlockGetReward()
        {
            _m_WDetailGrid?.refreshList();
            _refreshWnd();
        }

        #endregion
    }
}
