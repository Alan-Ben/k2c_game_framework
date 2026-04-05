using ALPackage;
using Common.StageGoalObj;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 阶段目标-时代之巅阶段列表item
    /// </summary>
    public class GGUIWndStageGoalPagePeakGridItem : _ANPGGUIBasicGridItemWnd<GGUIMonoStageGoalPagePeakGridItem>
    {
        // 大阶段数据
        private StageGoalBigStepRefObj _m_bigStepRefObj;
        // banner图
        private NPGGuiWndTexture _m_wBanner;
        // 奖励列表
        private GGUIWndCommonRewardContainer _m_wRewardContainer;
        // 首达玩家头像
        private NPGGUIWndPlayerIcon _m_wPlayerIcon;
        // 显示操作序列号
        private long _m_lShowSerialize;

        /// <summary>
        /// 大阶段数据
        /// </summary>
        public StageGoalBigStepRefObj bigStepRefObj { get { return _m_bigStepRefObj; } }

        public GGUIWndStageGoalPagePeakGridItem(GGUIMonoStageGoalPagePeakGridItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wBanner?.hideWnd();
            _m_wRewardContainer?.hideWnd();
            _m_wPlayerIcon?.hideWnd();
            _m_lShowSerialize = ALSerializeOpMgr.next();
        }

        protected override void _onReset()
        {
            _m_wBanner?.discardTexture();
            _m_wRewardContainer?.resetWnd();
            _m_wPlayerIcon?.resetWnd();
        }

        protected override void _resetGridItem()
        {
        }

        protected override void _onDiscard()
        {
            _m_wBanner?.discard();
            _m_wBanner = null;

            _m_wRewardContainer?.discard();
            _m_wRewardContainer = null;

            _m_wPlayerIcon?.discard();
            _m_wPlayerIcon = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnGetReward, _onClickGetReward);
            ALUGUICommon.uncombineBtnClick(wnd.btnReachDetail, _onClickReachDetail);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if(wnd.imgBigStageBanner != null)
                _m_wBanner = new NPGGuiWndTexture(wnd.imgBigStageBanner);

            if(wnd.monoRewardContainer != null)
                _m_wRewardContainer = new GGUIWndCommonRewardContainer(wnd.monoRewardContainer);

            if(wnd.monoFirstReachPlayerIcon != null)
                _m_wPlayerIcon = new NPGGUIWndPlayerIcon(wnd.monoFirstReachPlayerIcon);

            ALUGUICommon.combineBtnClick(wnd.btnGetReward, _onClickGetReward);
            ALUGUICommon.combineBtnClick(wnd.btnReachDetail, _onClickReachDetail);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_bigStepRef"></param>
        public void setInfo(StageGoalBigStepRefObj _bigStepRef, StageGoalFirstReachInfo _info)
        {
            if(wnd == null || _bigStepRef == null)
                return;

            _m_bigStepRefObj = _bigStepRef;

            // 设置标题、序号
            ALUGUICommon.setLabelTxt(wnd.txtBigStageName, _m_bigStepRefObj.getTitle);
            ALUGUICommon.setLabelTxt(wnd.txtBigStageNum, string.Format("{0:D2}", _m_bigStepRefObj.big_step));
            // banner图
            _m_wBanner?.showWnd();
            _m_wBanner?.setTexture(_bigStepRef.peak_banner);
            // 奖励列表
            ECommonRewardType rewardType = NPPlayer.instance.stageGoalComp.getFirstReachBigStepRewardType(_bigStepRef.big_step);
            _m_wRewardContainer?.showWnd();
            _m_wRewardContainer?.setRewardList(_m_bigStepRefObj.first_reach_reward_item_list, rewardType);
            ALUGUICommon.setGameObjEnable(wnd.goCanGetRewardShowList, rewardType == ECommonRewardType.CAN_GET_REWARD);
            ALUGUICommon.setGameObjEnable(wnd.goCanGetRewardHideList, rewardType != ECommonRewardType.CAN_GET_REWARD);

            // 设置首达信息
            if(_info == null || _info.cid <= 0)
            {
                // 无人首达
                ALUGUICommon.setGameObjEnable(wnd.goHaveReachPlayerShowList, false);
                ALUGUICommon.setGameObjEnable(wnd.goHaveReachPlayerHideList, true);
            }
            else
            {
                // 设置首达时间显示
                ALUGUICommon.setLabelTxt(wnd.txtFirstReachTime, TimeUtil.DateTime2StringMDYHMS(TimeUtil.FromUTCMilliseconds(_info.reachTimeMs)));

                // 先使用已有数据设置显示
                if (_info.simplePlayerInfo != null)
                {
                    ALUGUICommon.setGameObjEnable(wnd.goHaveReachPlayerHideList, false);
                    ALUGUICommon.setGameObjEnable(wnd.goHaveReachPlayerShowList, true);
                    _m_wPlayerIcon?.showWnd();
                    _m_wPlayerIcon?.setPlayerInfo(_info.simplePlayerInfo);
                }

                // 获取最新数据设置首达玩家信息显示
                _m_lShowSerialize = ALSerializeOpMgr.next();
                long curSerialize = _m_lShowSerialize;
                _info.getPlayerInfo(_playerInfo =>
                {
                    if (_playerInfo == null || curSerialize != _m_lShowSerialize || !isShow)
                        return;

                    ALUGUICommon.setGameObjEnable(wnd.goHaveReachPlayerHideList, false);
                    ALUGUICommon.setGameObjEnable(wnd.goHaveReachPlayerShowList, true);
                    _m_wPlayerIcon?.showWnd();
                    _m_wPlayerIcon?.setPlayerInfo(_playerInfo);
                });
            }
        }

        // 点击领取奖励
        private void _onClickGetReward(GameObject _go)
        {
            if (_m_bigStepRefObj == null)
                return;

            NPPlayer.instance.stageGoalComp.reqDrawStageGoalFirstReachReward(_m_bigStepRefObj.big_step);
        }

        // 点击查看达成详情
        private void _onClickReachDetail(GameObject _go)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndStageGoalReachPeakDetail.instance, () =>
            {
                GGUIWndStageGoalReachPeakDetail.instance.showWnd();
                GGUIWndStageGoalReachPeakDetail.instance.setInfo(_m_bigStepRefObj);
            },UINodeTagConst.C_STAGE_GOAL_PEAK_DETAIL);
        }
    }
}
