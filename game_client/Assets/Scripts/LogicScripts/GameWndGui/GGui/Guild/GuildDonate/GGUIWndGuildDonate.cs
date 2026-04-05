using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟捐赠
    /// </summary>
    public class GGUIWndGuildDonate : _ANPGGUIBasicResBarWnd<GGUIMonoGuildDonate>
    {
        private static GGUIWndGuildDonate _g_instance;
        public static GGUIWndGuildDonate instance { get { return _g_instance ??= new GGUIWndGuildDonate(); } }

        private List<_ISliderRewardItemInfo> _m_lProcessInfoList;//活跃积分进度信息列表
        
        private GGUIWndCommonRewardSlider _m_wDonateProgress;//捐赠进度
        private GGUIWndGuildDonateItemContainer _m_wDonateContainer;//捐赠列表
        
        public GGUIWndGuildDonate() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoGuildDonate.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoGuildDonate.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.monoDonateProgress != null)
            {
                _m_wDonateProgress = new GGUIWndCommonRewardSlider(wnd.monoDonateProgress);
                _m_wDonateProgress.onClickItem += _onDonateProcessRewardItemClick;
            }

            if (wnd.monoDonateContainer != null)
                _m_wDonateContainer = new GGUIWndGuildDonateItemContainer(wnd.monoDonateContainer);
            
            ALUGUICommon.combineBtnClick(wnd.btnDonateProgressSpecification, _onBtnDonateProgressSpecificationClick);
            ALUGUICommon.combineBtnClick(wnd.btnDonateDetailInfo, _onBtnDonateDetailInfoClick);

            // 创建捐献进度奖励数据
            if (_m_lProcessInfoList == null)
                _m_lProcessInfoList = new List<_ISliderRewardItemInfo>();
            foreach (var item in GRefdataCoreMgr.instance.guildConstructRewardRefCore.refList)
            {
                if(item != null)
                    _m_lProcessInfoList.Add(new DailyDonateProcessRewardInfo(item));
            }
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnDonateProgressSpecification, _onBtnDonateProgressSpecificationClick);
                ALUGUICommon.uncombineBtnClick(wnd.btnDonateDetailInfo, _onBtnDonateDetailInfoClick);
            }
            
            _m_lProcessInfoList?.Clear();
            
            if (_m_wDonateProgress != null)
            {
                _m_wDonateProgress.onClickItem -= _onDonateProcessRewardItemClick;
                _m_wDonateProgress.discard();
            }
            _m_wDonateProgress = null;
         
            _m_wDonateContainer?.discard();
            _m_wDonateContainer = null;
        }
        
        protected override void _onShowWnd()
        {
            _refreshWnd();
            
            WinMsg.RegisterMsgAct(WinMsgType.ON_PLAYER_PERMISSION_CHG, _onPermissionsChg);
            WinMsg.RegisterMsgAct(WinMsgType.ON_GUILD_CONSTRUCT_PROGRESS_REWARD_POINT_CHG, _onDonateProgressChg);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_PLAYER_PERMISSION_CHG, _onPermissionsChg);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_GUILD_CONSTRUCT_PROGRESS_REWARD_POINT_CHG, _onDonateProgressChg);

            _m_wDonateProgress?.hideWnd();
            _m_wDonateContainer?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wDonateProgress?.resetWnd();
            _m_wDonateContainer?.resetWnd();
        }

        /// <summary>
        /// 刷新窗口
        /// </summary>
        private void _refreshWnd()
        {
            if(wnd == null)
                return;

            _refreshDonateContainer();
            _refreshDonateProgress();
        }

        /// <summary>
        /// 刷新联盟捐赠列表
        /// </summary>
        private void _refreshDonateContainer()
        {
            if (_m_wDonateContainer != null)
            {
                _m_wDonateContainer.showWnd();
                _m_wDonateContainer.setData(new List<GuildConstructRefObj>(GRefdataCoreMgr.instance.guildConstructRefCore.refList));
            }
        }
        
        /// <summary>
        /// 刷新联盟捐赠进度
        /// </summary>
        private void _refreshDonateProgress()
        {
            if (_m_wDonateProgress != null)
            {
                _m_wDonateProgress.showWnd();
                _m_wDonateProgress.setInfo(NPPlayer.instance.guildComp.guildInfo?.guildConstructList?.rewardPoint ?? 0, _m_lProcessInfoList);
            }
        }

        /// <summary>
        /// 捐赠进度变化
        /// </summary>
        private void _onDonateProgressChg()
        {
            if(_m_wDonateProgress != null)
                _m_wDonateProgress.refreshStateByScore(NPPlayer.instance.guildComp.guildInfo?.guildConstructList?.rewardPoint ?? 0);
        }

        /// <summary>
        /// 玩家权限变化
        /// </summary>
        private void _onPermissionsChg()
        {
            _refreshDonateContainer();
        }
        
        #region 联盟捐赠进度奖励

        /// <summary>
        /// 每日捐赠进度奖励
        /// </summary>
        private class DailyDonateProcessRewardInfo : _ISliderRewardItemInfo
        {
            private GuildConstructRewardRefObj _m_rDonateProcessRewardRefObj;

            public DailyDonateProcessRewardInfo(GuildConstructRewardRefObj _refObj)
            {
                _m_rDonateProcessRewardRefObj = _refObj;
            }
            
            /// <summary>
            /// id
            /// </summary>
            public long id { get { return _m_rDonateProcessRewardRefObj?.num ?? 0; } }
            /// <summary>
            /// 分数
            /// </summary>
            public long score { get { return _m_rDonateProcessRewardRefObj?.num ?? 0; } }

            public EValueFormatType valueFormatType { get { return EValueFormatType.NORMAL; } }

            public string showScore { get { return string.Empty; } }

            /// <summary>
            /// 奖励领取状态
            /// </summary>
            public ESliderRewardState rewardState
            {
                get
                {
                    if (_m_rDonateProcessRewardRefObj == null)
                        return ESliderRewardState.CAN_NOT_GET;

                    // 获取当前分数
                    long curScore = NPPlayer.instance.guildComp.guildInfo?.guildConstructList?.rewardPoint ?? 0;
                    if (curScore >= score)//若当前进度达到本阶段需要进度(当前分数 >= 本阶段分数)
                    {
                        GuildMemberDailyData selfDailyData = NPPlayer.instance.guildComp.selfDailyData;
                        if (selfDailyData != null && selfDailyData.hasDrawConstructReward((int)score))
                        {
                            return ESliderRewardState.ALREADY_GET;
                        }
                        else
                        {
                            return ESliderRewardState.CAN_GET;
                        }
                    }
                    else
                    {
                        return ESliderRewardState.CAN_NOT_GET;
                    }
                }
            }
            /// <summary>
            /// 宝箱图标
            /// </summary>
            public NPGTextureIndex icon { get { return null; } }

            public _IItem showRewardItem { get { return null; } }
        }

        /// <summary>
        /// 显示进度奖励预览弹窗
        /// </summary>
        /// <param name="_itemInfo"></param>
        /// <param name="_notGetIcon"></param>
        /// <param name="_alreadyGetIcon"></param>
        /// <param name="_rewardType"></param>
        private void _showRewardPreview(_ISliderRewardItemInfo _itemInfo, RectTransform _toolTipShowRoot, NPGTextureIndex _notGetIcon, NPGTextureIndex _alreadyGetIcon, ECommonRewardType _rewardType)
        {
            if (wnd == null || _itemInfo == null)
                return;

            GuildConstructRewardRefObj constructRewardRefObj = GRefdataCoreMgr.instance.guildConstructRewardRefCore.getRef(_itemInfo.score);
            if(constructRewardRefObj == null)
                return;
            
            long uiPathId = wnd.donateProgressRewardPreviewToolTipResPathId;
            QueueMgr.instance.AddNode(new GNodeCommonToolTip_Reward(uiPathId, string.Empty, string.Empty, constructRewardRefObj.reward_item_list
                , _toolTipShowRoot, wnd.donateProgressRewardPreviewToolTipOffset, _rewardType));
        }
        
        #endregion
        
        #region 点击按钮

        /// <summary>
        /// 当点击捐赠进度规则按钮时
        /// </summary>
        /// <param name="_go"></param>
        private void _onBtnDonateProgressSpecificationClick(GameObject _go)
        {
            if(wnd == null || _go == null)
                return;

            long uiPathId = wnd.donateProgressSpecificationToolTipResPathId;
            QueueMgr.instance.AddNode(new GNodeCommonToolTip_GuildDonateProgressSpecification(UIResPathAssistant.getAssetPath(uiPathId), UIResPathAssistant.getObjName(uiPathId), (RectTransform)_go.transform, wnd.donateProgressSpecificationToolTipOffset.x, wnd.donateProgressSpecificationToolTipOffset.y));
        }

        /// <summary>
        /// 当捐赠进度奖励item被点击时
        /// </summary>
        private void _onDonateProcessRewardItemClick(GGUIWndCommonRewardSliderContainerItem _itemWnd)
        {
            if (_itemWnd == null || _itemWnd.wnd == null || _itemWnd.itemInfo == null)
                return;

            switch (_itemWnd.itemInfo.rewardState)
            {
                case ESliderRewardState.CAN_GET://可领取
                {
                    // 请求领取进度奖励
                    NPPlayer.instance.guildComp.reqDrawConstructReward((int)_itemWnd.itemInfo.score, (_msg) =>
                    {
                        if (isShow && _m_wDonateProgress != null)
                        {
                            _m_wDonateProgress.refreshAllState();
                        }
                    }, null);
                    break;
                }
                case ESliderRewardState.CAN_NOT_GET://不可领取
                    _showRewardPreview(_itemWnd.itemInfo, _itemWnd.rectTransform, _itemWnd.wnd.notGetBoxIcon, _itemWnd.wnd.alreadyGetBoxIcon, ECommonRewardType.NOT_GET_REWARD);
                    break;
                case ESliderRewardState.ALREADY_GET://已领取
                    _showRewardPreview(_itemWnd.itemInfo, _itemWnd.rectTransform, _itemWnd.wnd.notGetBoxIcon, _itemWnd.wnd.alreadyGetBoxIcon, ECommonRewardType.HAS_GET_REWARD);
                    break;
            }
        }
        
        /// <summary>
        /// 当点击捐赠详情按钮时
        /// </summary>
        /// <param name="_go"></param>
        private void _onBtnDonateDetailInfoClick(GameObject _go)
        {
            if(wnd == null || _go == null)
                return;

            long uiPathId = wnd.donateDetailInfoToolTipResPathId;
            QueueMgr.instance.AddNode(new GNodeCommonToolTip_GuildDonateDetailInfo(UIResPathAssistant.getAssetPath(uiPathId), UIResPathAssistant.getObjName(uiPathId), (RectTransform)_go.transform, wnd.donateDetailInfoToolTipOffset.x, wnd.donateDetailInfoToolTipOffset.y));
        }
        
        #endregion
    }
}