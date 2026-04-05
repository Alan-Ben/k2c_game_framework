using ALPackage;
using Common.GuildCooperateObj;
using System.Collections.Generic;
using Common.GuildEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 公会协作奖励据点详情弹窗
    /// </summary>
    public class GGUIWndGuildCooperateRewardPosDetail : _ANPGGUIBasicWnd<GGUIMonoGuildCooperateRewardPosDetail>
    {
        private static GGUIWndGuildCooperateRewardPosDetail _g_instance;
        public static GGUIWndGuildCooperateRewardPosDetail instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndGuildCooperateRewardPosDetail();
                return _g_instance;
            }
        }

        // 奖励据点信息
        private GuildCooperateRewardPointInfo _m_rewardPointInfo;
        // 据点图标
        private NPGGuiWndTexture _m_wIcon;
        // 必得奖励列表容器
        private NPGGUIWndCommonItemContainer _m_wDefinitelyItemContainer;
        // 随机奖励列表容器
        private NPGGUIWndCommonItemContainer _m_wRandomItemContainer;


        public GGUIWndGuildCooperateRewardPosDetail() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoGuildCooperateRewardPosDetail.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoGuildCooperateRewardPosDetail.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        protected override bool isShowAniPlayOnlyOne { get { return true; } }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_GUILD_COOPERATE_REWARD_POS_GET_REWARD, _onSimulateClickGetReward);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_GUILD_COOPERATE_REWARD_POS_GET_REWARD, _onSimulateClickGetReward);
            _m_wIcon?.hideWnd();
            _m_wDefinitelyItemContainer?.hideWnd();
            _m_wRandomItemContainer?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wIcon?.discardTexture();
            _m_wDefinitelyItemContainer?.resetWnd();
            _m_wRandomItemContainer?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wIcon?.discard();
            _m_wIcon = null;
            _m_wDefinitelyItemContainer?.discard();
            _m_wDefinitelyItemContainer = null;
            _m_wRandomItemContainer?.discard();
            _m_wRandomItemContainer = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.uncombineBtnClick(wnd.btnGetReward, _onClickGetReward);
            ALUGUICommon.uncombineBtnClick(wnd.btnCanNotGetReward, _onClickCanNotGetReward);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgIcon != null)
                _m_wIcon = new NPGGuiWndTexture(wnd.imgIcon);

            if(wnd.monoDefinitelyItemContainer != null)
                _m_wDefinitelyItemContainer = new NPGGUIWndCommonItemContainer(wnd.monoDefinitelyItemContainer);

            if(wnd.monoRandomItemContainer != null)
                _m_wRandomItemContainer = new NPGGUIWndCommonItemContainer(wnd.monoRandomItemContainer);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.combineBtnClick(wnd.btnGetReward, _onClickGetReward);
            ALUGUICommon.combineBtnClick(wnd.btnCanNotGetReward, _onClickCanNotGetReward);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_info"></param>
        public void setInfo(GuildCooperateRewardPointInfo _info)
        {
            if (_info == null)
                return;

            _m_rewardPointInfo = _info;
            _refreshWnd();
        }

        /// <summary>
        /// 刷新窗口
        /// </summary>
        private void _refreshWnd()
        {
            if (wnd == null || _m_rewardPointInfo == null || _m_rewardPointInfo.posRef == null)
                return;

            // 名称描述
            ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(_m_rewardPointInfo.posRef.name));
            ALUGUICommon.setLabelTxt(wnd.txtDesc, TextTranslate.instance.getLanguage(_m_rewardPointInfo.posRef.desc, _m_rewardPointInfo.posRef.desc_args));

            // 图标
            _m_wIcon?.showWnd();
            _m_wIcon?.setTexture(_m_rewardPointInfo.posRef.icon);

            // 必得奖励列表
            bool isLeader = false;
            if (_m_rewardPointInfo.leaderCid > 0)
                isLeader = _m_rewardPointInfo.leaderCid == NPPlayer.instance.playerInfo.CID;
            else
                isLeader = NPPlayer.instance.guildComp.guildInfo != null && NPPlayer.instance.guildComp.guildInfo.getSelfPositionRef()?.type == EGuildPositionType.LEADER;
            List<NPCommonCostItem> certainlyItemList = new List<NPCommonCostItem>();
            if (isLeader && _m_rewardPointInfo.areaRef != null)
            {
                // 如果是盟主，则添加工会财富奖励展示
                NPCommonCostItem guildWealthItem = new NPCommonCostItem(GRefdataCoreMgr.instance.npGeneral.guild_wealth_common_item, _m_rewardPointInfo.areaRef.reward_point_guild_wealth_reward);
                certainlyItemList.Add(guildWealthItem);
            }
            if (_m_rewardPointInfo.areaRef != null && _m_rewardPointInfo.areaRef.reward_point_reward_list != null)
                certainlyItemList.AddRange(_m_rewardPointInfo.areaRef.reward_point_reward_list);
            _m_wDefinitelyItemContainer?.showWnd();
            _m_wDefinitelyItemContainer?.showItemList(certainlyItemList);

            // 随机奖励列表
            List<NPCommonCostItem> randomItemList = new List<NPCommonCostItem>();
            if (_m_rewardPointInfo.areaRef != null && _m_rewardPointInfo.areaRef.reward_point_random_reward_list != null)
                GCommon.itemListTONoRewardItemList(_m_rewardPointInfo.areaRef.reward_point_random_reward_list, randomItemList, 0);
            _m_wRandomItemContainer?.showWnd();
            _m_wRandomItemContainer?.showItemList(randomItemList);

            // 刷新按钮状态
            _refreshButtonState();
        }

        /// <summary>
        /// 刷新按钮状态
        /// </summary>
        private void _refreshButtonState()
        {
            if (wnd == null || _m_rewardPointInfo == null)
                return;

            wnd.rewardTypeShow?.setShowData(_m_rewardPointInfo.getRewardType());
        }

        #region 点击事件

        /// <summary>
        /// 点击关闭按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_GUIlD_COOPERATE_REWARD_POINT_DETAIL);
        }

        /// <summary>
        /// 点击领取奖励按钮
        /// </summary>
        private void _onClickGetReward(GameObject _go)
        {
            if (_m_rewardPointInfo == null || _m_rewardPointInfo.getRewardType() != ECommonRewardType.CAN_GET_REWARD)
                return;

            GuildCooperate_RewardPointPos pointPos = new GuildCooperate_RewardPointPos(_m_rewardPointInfo.areaId, _m_rewardPointInfo.index);
            NPPlayer.instance.guildCooperateComp.reqDrawRewardPointReward(pointPos, _refreshButtonState);
        }

        /// <summary>
        /// 点击不可领取按钮
        /// </summary>
        private void _onClickCanNotGetReward(GameObject _go)
        {
            if (_m_rewardPointInfo == null || _m_rewardPointInfo.getRewardType() != ECommonRewardType.NOT_GET_REWARD)
                return;

            //需要将周围属性据点全部建设完成才可领取
            NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guildCooperate_canNotGetRewardTip_none);
        }

        #endregion

        #region 消息事件

        /// 模拟点击领取奖励按钮
        private void _onSimulateClickGetReward()
        {
            _onClickGetReward(null);
        }

        #endregion
    }
}