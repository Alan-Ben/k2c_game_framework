using System.Collections.Generic;
using ALPackage;
using Common.ActivityEnum;
using CommonEnum;
using GOE;
using Hotfix.NumMergeEnum;
using JetBrains.Annotations;
using NPEnum;
using UnityEngine;

namespace Hotfix
{
    /// <summary>
    /// 2048 游戏界面
    /// </summary>
    public class GGUIWndNumMergeGame : _AHotfixBaseWnd<GGUIMonoNumMergeGame>
    {
        [NotNull] public static GGUIWndNumMergeGame instance { get { return _g_instance ??= new GGUIWndNumMergeGame(); } }
        private static GGUIWndNumMergeGame _g_instance;
        

        //奖券道具
        private GGUIWndCommonSimpleItem _m_wTicketItem;
        //体力
        private GGUIWndCommonLazyCDCountResume _m_wStamina;
        //快速模式切换
        private NPGGUIWndCommonToggleEx _m_wToggleAdvanceMode;
        //急速模式切换
        private NPGGUIWndCommonToggleEx _m_wToggleUltraMode;
        //整理道具
        private GGUIWndCommonSimpleItem _m_wOrganizeItem;
        //消除道具
        private GGUIWndCommonSimpleItem _m_wEliminateItem;
        //游戏玩法子窗口
        private GGUIWndNumMergeGamePlay _m_wGamePlay;
        //宝箱奖励子窗口
        private GGUIWndNumMergeBox _m_wBoxReward;
        //tip管理器
        private NPGGUICommonTipDealerMgr _m_tipMgr;
        //上次的体力数量，-1表示初始化显示
        private int _m_lastLazyCDCount = -1;


        private GGUIWndNumMergeGame()
            : base(EALUIWndLayer.NORMAL)
        {
        }


        protected override string _monoAssetPath { get { return UIResPathAssistant.getAssetPath(8601); } }
        protected override string _monoObjName { get { return UIResPathAssistant.getObjName(8601); } }


        protected override void _onShowWnd()
        {
            _m_wTicketItem?.showWnd();
            _m_wStamina?.showWnd();
            _m_wOrganizeItem?.showWnd();
            _m_wEliminateItem?.showWnd();
            _m_wGamePlay?.showWnd();
            _m_wBoxReward?.showWnd();
            _m_tipMgr?.start();

            refreshWnd();

            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ACTIVITY_STATE_CHG, _onActivityStateChg);
            WinMsg.RegisterMsg(WinMsgType.ON_ACTIVITY_CURRENCY_CHG, _onActivityCurrencyChg);
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_ADD, _onBagItemChg);
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_COUNT_ADD, _onBagItemChg);
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_UPDATE, _onBagItemChg);
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_REMOVE, _onBagItemChg);
            ALMsgSys.RegisterMsgAct(HotfixMsgType.SHOW_NUMMERGE_USE_ITEM_TIP, _showUseItemTip);
            WinMsg.RegisterMsg(WinMsgType.ON_LAZY_CD_CHG, _onLazyCDChg);

            HotfixNPPlayer.instance.numMergeComponent.onScoreChg += refreshHighestScore;
            HotfixNPPlayer.instance.numMergeComponent.onBoardDataChg += refreshModeLockState;
        }
        protected override void _onHideWnd()
        {
            HotfixNPPlayer.instance.numMergeComponent.onBoardDataChg -= refreshModeLockState;
            HotfixNPPlayer.instance.numMergeComponent.onScoreChg -= refreshHighestScore;

            WinMsg.UnregisterMsg(WinMsgType.ON_LAZY_CD_CHG, _onLazyCDChg);
            ALMsgSys.UnregisterMsgAct(HotfixMsgType.SHOW_NUMMERGE_USE_ITEM_TIP, _showUseItemTip);
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_ADD, _onBagItemChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_REMOVE, _onBagItemChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_UPDATE, _onBagItemChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_PLAYER_RES_CHANGE, _onBagItemChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_ACTIVITY_CURRENCY_CHG, _onActivityCurrencyChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ACTIVITY_STATE_CHG, _onActivityStateChg);

            _m_lastLazyCDCount = -1;

            _m_wTicketItem?.hideWnd();
            _m_wStamina?.hideWnd();
            _m_wOrganizeItem?.hideWnd();
            _m_wEliminateItem?.hideWnd();
            _m_wGamePlay?.hideWnd();
            _m_wBoxReward?.hideWnd();
            _m_tipMgr?.clear();
        }
        protected override void _onReset()
        {
            _m_wTicketItem?.resetWnd();
            _m_wStamina?.resetWnd();
            _m_wOrganizeItem?.resetWnd();
            _m_wEliminateItem?.resetWnd();
            _m_wGamePlay?.resetWnd();
            _m_wBoxReward?.resetWnd();
        }
        protected override void _onDiscard()
        {
            if (hotfixWnd == null)
                return;

            _m_wTicketItem?.discard();
            _m_wTicketItem = null;
            _m_wStamina?.discard();
            _m_wStamina = null;
            if (_m_wToggleAdvanceMode != null)
            {
                _m_wToggleAdvanceMode.clickDelegate -= _onToggleAdvanceModeChanged;
                _m_wToggleAdvanceMode.discard();
                _m_wToggleAdvanceMode = null;
            }
            if (_m_wToggleUltraMode != null)
            {
                _m_wToggleUltraMode.clickDelegate -= _onToggleUltraModeChanged;
                _m_wToggleUltraMode.discard();
                _m_wToggleUltraMode = null;
            }
            _m_wOrganizeItem?.discard();
            _m_wOrganizeItem = null;
            _m_wEliminateItem?.discard();
            _m_wEliminateItem = null;
            _m_wGamePlay?.discard();
            _m_wGamePlay = null;
            _m_wBoxReward?.discard();
            _m_wBoxReward = null;
            _m_tipMgr?.clear();
            _m_tipMgr = null;

            ALUGUICommon.uncombineBtnClick(hotfixWnd.btnClose, _onClickClose);
            ALUGUICommon.uncombineBtnClick(hotfixWnd.btnScoreDetail, _onClickScoreDetail);
            ALUGUICommon.uncombineBtnClick(hotfixWnd.btnOrganize, _onClickOrganize);
            ALUGUICommon.uncombineBtnClick(hotfixWnd.btnEliminate, _onClickEliminate);
            ALUGUICommon.uncombineBtnClick(hotfixWnd.btnHandbook, _onClickHandbook);
        }
        protected override void _onWndInitDoneHotfix()
        {
            if (hotfixWnd == null)
                return;

            //初始化奖券道具
            if (hotfixWnd.monoTicketItem != null)
                _m_wTicketItem = new GGUIWndCommonSimpleItem(hotfixWnd.monoTicketItem);
            //初始化体力
            if (hotfixWnd.monoStamina != null)
                _m_wStamina = new GGUIWndCommonLazyCDCountResume(hotfixWnd.monoStamina);
            //初始化快速模式切换
            if (hotfixWnd.toggleAdvanceMode != null)
            {
                _m_wToggleAdvanceMode = new NPGGUIWndCommonToggleEx(hotfixWnd.toggleAdvanceMode);
                _m_wToggleAdvanceMode.clickDelegate += _onToggleAdvanceModeChanged; 
            }
            //初始化急速模式切换
            if (hotfixWnd.toggleUltraMode != null)
            {
                _m_wToggleUltraMode = new NPGGUIWndCommonToggleEx(hotfixWnd.toggleUltraMode);
                _m_wToggleUltraMode.clickDelegate += _onToggleUltraModeChanged;
            }
            //初始化整理道具
            if (hotfixWnd.monoOrganizeItem != null)
                _m_wOrganizeItem = new GGUIWndCommonSimpleItem(hotfixWnd.monoOrganizeItem);
            //初始化消除道具
            if (hotfixWnd.monoEliminateItem != null)
                _m_wEliminateItem = new GGUIWndCommonSimpleItem(hotfixWnd.monoEliminateItem);
            //初始化游戏玩法子窗口
            if (hotfixWnd.monoGamePlay != null)
                _m_wGamePlay = new GGUIWndNumMergeGamePlay(hotfixWnd.monoGamePlay);
            //初始化宝箱奖励子窗口
            if (hotfixWnd.monoBoxReward != null)
                _m_wBoxReward = new GGUIWndNumMergeBox(hotfixWnd.monoBoxReward);
            if (hotfixWnd.transTicketAddTipParent)
                _m_tipMgr = new NPGGUICommonTipDealerMgr(hotfixWnd.transTicketAddTipParent);

            ALUGUICommon.combineBtnClick(hotfixWnd.btnClose, _onClickClose);
            ALUGUICommon.combineBtnClick(hotfixWnd.btnScoreDetail, _onClickScoreDetail);
            ALUGUICommon.combineBtnClick(hotfixWnd.btnOrganize, _onClickOrganize);
            ALUGUICommon.combineBtnClick(hotfixWnd.btnEliminate, _onClickEliminate);
            ALUGUICommon.combineBtnClick(hotfixWnd.btnHandbook, _onClickHandbook);
        }


        //刷新界面
        public void refreshWnd()
        {
            if (hotfixWnd == null || !_m_bIsShow)
                return;

            _m_lastLazyCDCount = NPPlayer.instance.lazyCdComp.getCount(HotfixRefdataCoreMgr.instance.numMergeOtherRefObj.num_merge_lazy_cd_id);
            _m_wStamina?.setInfo(HotfixRefdataCoreMgr.instance.numMergeOtherRefObj.num_merge_lazy_cd_id);
            refreshTicketItem();
            refreshOrganizeItem();
            refreshEliminateItem();
            refreshHighestScore(false);
            refreshModeToggle();
            refreshModeLockState();

            if (hotfixWnd.useItemTipAnim != null)
                hotfixWnd.useItemTipAnim.Sample(hotfixWnd.useItemTipAnimName, 1f);
        }
        /// <summary>
        /// 刷新最高得分
        /// </summary>
        public void refreshHighestScore(bool _byBuff)
        {
            if (hotfixWnd == null || !_m_bIsShow)
                return;

            long highestScore = HotfixNPPlayer.instance.numMergeComponent.maxScore;
            ALUGUICommon.setLabelTxt(hotfixWnd.txtHighestScore, highestScore);
        }
        public void refreshTicketItem(bool _showAddTip = false)
        {
            if (hotfixWnd == null || !_m_bIsShow || _m_wTicketItem == null)
                return;

            NPCommonItem ticketItem = HotfixRefdataCoreMgr.instance.numMergeOtherRefObj.num_merge_ticket_item;
            long oldCount = _m_wTicketItem.itemCount;
            long newCount = GCommon.getItemCount(ticketItem);
            _m_wTicketItem.setItem(new CommonItemData(ticketItem, newCount));

            if (_showAddTip && _m_tipMgr != null)
            {
                NPCenterTipsRefObj tipRefObj = GRefdataCoreMgr.instance.tipMap.getRef(hotfixWnd.ticketAddTipId);
                long addCount = newCount - oldCount;
                if (addCount > 0)
                    _m_tipMgr.addTip(new NPTextTipDealer(
                        new List<string> { TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, addCount) }, 
                        tipRefObj, null));
            }
        }
        public void refreshOrganizeItem()
        {
            if (hotfixWnd == null || !_m_bIsShow)
                return;

            NPCommonItem organizeItem = HotfixRefdataCoreMgr.instance.numMergeOtherRefObj.num_merge_organize_item;
            _m_wOrganizeItem?.setItem(new CommonItemData(organizeItem, GCommon.getItemCount(organizeItem)));
        }
        public void refreshEliminateItem()
        {
            if (hotfixWnd == null || !_m_bIsShow)
                return;
            
            NPCommonItem eliminateItem = HotfixRefdataCoreMgr.instance.numMergeOtherRefObj.num_merge_eliminate_item;
            _m_wEliminateItem?.setItem(new CommonItemData(eliminateItem, GCommon.getItemCount(eliminateItem)));
        }
        //刷新模式切换状态
        public void refreshModeToggle()
        {
            if (hotfixWnd == null || !_m_bIsShow)
                return;

            ENumMerge_ModeType modeType = HotfixAccountSettingMgr.instance.hotfixAccountSetting.getNumMergeModeType();
            NumMergeModeRefObj modeRefObj = HotfixRefdataCoreMgr.instance.numMergeModeRefCore.getRef((long) modeType);
            if (modeRefObj == null || !modeRefObj.isModeUnlocked())
                modeType = ENumMerge_ModeType.NORMAL;
            
            HotfixAccountSettingMgr.instance.hotfixAccountSetting.setNumMergeModeType(modeType);
            _m_wToggleAdvanceMode?.setSelected(modeType == ENumMerge_ModeType.ADVANCED);
            _m_wToggleUltraMode?.setSelected(modeType == ENumMerge_ModeType.ULTRA);
            _sampleModeChangeAnimation(modeType, 1f);
        }
        public void refreshModeLockState()
        {
            if (hotfixWnd == null || !_m_bIsShow)
                return;

            NumMergeModeRefObj advanceModeRefObj = HotfixRefdataCoreMgr.instance.numMergeModeRefCore.getRef((long)ENumMerge_ModeType.ADVANCED);
            NumMergeModeRefObj ultraModeRefObj = HotfixRefdataCoreMgr.instance.numMergeModeRefCore.getRef((long)ENumMerge_ModeType.ULTRA);

            bool isAdvanceUnlocked = advanceModeRefObj != null && advanceModeRefObj.isModeUnlocked();
            bool isUltraUnlocked = ultraModeRefObj != null && ultraModeRefObj.isModeUnlocked();

            ALUGUICommon.setGameObjEnable(hotfixWnd.listAdvanceModeLockShow, !isAdvanceUnlocked);
            ALUGUICommon.setGameObjEnable(hotfixWnd.listUltraModeLockShow, !isUltraUnlocked);
        }
        
        
        //快速模式切换事件
        private void _onToggleAdvanceModeChanged(NPGGUIWndCommonToggleEx _isOn)
        {
            _handleModeToggle(ENumMerge_ModeType.ADVANCED);
        }
        //急速模式切换事件
        private void _onToggleUltraModeChanged(NPGGUIWndCommonToggleEx _isOn)
        {
            _handleModeToggle(ENumMerge_ModeType.ULTRA);
        }
        /// <summary>
        /// 处理模式切换（包含动画）
        /// </summary>
        private void _handleModeToggle(ENumMerge_ModeType _targetMode)
        {
            // 检查模式是否解锁
            NumMergeModeRefObj modeRefObj = HotfixRefdataCoreMgr.instance.numMergeModeRefCore.getRef((long) _targetMode);
            if (modeRefObj == null || !modeRefObj.isModeUnlocked(true))
                return;

            // 获取当前模式
            ENumMerge_ModeType currentMode = HotfixAccountSettingMgr.instance.hotfixAccountSetting.getNumMergeModeType();
            // 如果当前已经是该模式，则切换回普通模式；否则切换到目标模式
            ENumMerge_ModeType newMode = currentMode == _targetMode ? ENumMerge_ModeType.NORMAL : _targetMode;
            // 保存新模式
            HotfixAccountSettingMgr.instance.hotfixAccountSetting.setNumMergeModeType(newMode);
            // 更新UI切换状态
            _m_wToggleAdvanceMode?.setSelected(newMode == ENumMerge_ModeType.ADVANCED);
            _m_wToggleUltraMode?.setSelected(newMode == ENumMerge_ModeType.ULTRA);
            // 播放模式切换动画
            _playModeChangeAnimation(newMode);
        }
        /// <summary>
        /// 播放模式切换动画
        /// </summary>
        private void _playModeChangeAnimation(ENumMerge_ModeType _modeType)
        {
            if (hotfixWnd == null || hotfixWnd.modeChangeAnim == null)
                return;

            string animName;
            switch (_modeType)
            {
                case ENumMerge_ModeType.NORMAL:
                    animName = hotfixWnd.normalModeAnimName;
                    break;
                case ENumMerge_ModeType.ADVANCED:
                    animName = hotfixWnd.advanceModeAnimName;
                    break;
                case ENumMerge_ModeType.ULTRA:
                    animName = hotfixWnd.ultraModeAnimName;
                    break;
                default:
                    animName = null;
                    break;
            }

            if (!string.IsNullOrEmpty(animName))
                hotfixWnd.modeChangeAnim.ForcePlay(animName);
        }
        private void _sampleModeChangeAnimation(ENumMerge_ModeType _modeType, float _normalizedTime)
        {
            if (hotfixWnd == null || hotfixWnd.modeChangeAnim == null)
                return;

            string animName;
            switch (_modeType)
            {
                case ENumMerge_ModeType.NORMAL:
                    animName = hotfixWnd.normalModeAnimName;
                    break;
                case ENumMerge_ModeType.ADVANCED:
                    animName = hotfixWnd.advanceModeAnimName;
                    break;
                case ENumMerge_ModeType.ULTRA:
                    animName = hotfixWnd.ultraModeAnimName;
                    break;
                default:
                    animName = null;
                    break;
            }

            if (!string.IsNullOrEmpty(animName))
                hotfixWnd.modeChangeAnim.Sample(animName, _normalizedTime);
        }
        //点击关闭按钮
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(HotfixUINodeTagConst.NUMMERGE_GAME);
        }
        //点击得分详情按钮
        private void _onClickScoreDetail(GameObject _go)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndNumMergeGameScoreDetail.instance, GGUIWndNumMergeGameScoreDetail.instance.showWnd, 
                EUIQueueStageType.MAIN, HotfixUINodeTagConst.NUMMERGE_GAME_SCORE_DETAIL, true, false);
        }
        //点击整理按钮
        private void _onClickOrganize(GameObject _go)
        {
            NPCommonItem organizeItem = HotfixRefdataCoreMgr.instance.numMergeOtherRefObj.num_merge_organize_item;
            if (!GCommon.isItemEnough(new NPCommonCostItem(organizeItem, 1), true))
                return;
            
            NPMesMgr.instance.showTwoBtnMes(TextTranslate.instance.getLanguage(
                    HotfixTransKeyConst.numMerge_useRefresh_des), 
                TextTranslate.instance.getLanguage(TransKeyConst.cancel), 
                    null,
                TextTranslate.instance.getLanguage(TransKeyConst.confirm), 
                () =>
                {
                    _m_wGamePlay?.doOrganize();
                }, true, HotfixTransKeyConst.numMerge_useRefresh_title);
        }
        //点击消除按钮
        private void _onClickEliminate(GameObject _go)
        {
            NPCommonItem eliminateItem = HotfixRefdataCoreMgr.instance.numMergeOtherRefObj.num_merge_eliminate_item;
            if (!GCommon.isItemEnough(new NPCommonCostItem(eliminateItem, 1), true))
                return;
            
            _m_wGamePlay?.startEliminateMode();
        }
        private void _onBagItemChg(params object[] _objs)
        {
            if(_objs == null || _objs.Length < 1 || _objs[0] is not BagItem bagItem)
                return;
            
            NPCommonItem ticketItem = HotfixRefdataCoreMgr.instance.numMergeOtherRefObj.num_merge_ticket_item;
            NPCommonItem organizeItem = HotfixRefdataCoreMgr.instance.numMergeOtherRefObj.num_merge_organize_item;
            NPCommonItem eliminateItem = HotfixRefdataCoreMgr.instance.numMergeOtherRefObj.num_merge_eliminate_item;
            
            if (organizeItem != null && organizeItem.itemType == ENPItemType.BAG_ITEM && organizeItem.itemId == bagItem.itemId)
                refreshOrganizeItem();
            if (eliminateItem != null && eliminateItem.itemType == ENPItemType.BAG_ITEM && eliminateItem.itemId == bagItem.itemId)
                refreshEliminateItem();
            if (ticketItem != null && ticketItem.itemType == ENPItemType.BAG_ITEM && ticketItem.itemId == bagItem.itemId)
                refreshTicketItem(true);
        }
        private void _onActivityCurrencyChg(params object[] _objects)
        {
            if (_objects == null || _objects.Length == 0)
                return;

            long activityCurrencyId = (long)_objects[0];
            
            NPCommonItem ticketItem = HotfixRefdataCoreMgr.instance.numMergeOtherRefObj.num_merge_ticket_item;
            NPCommonItem organizeItem = HotfixRefdataCoreMgr.instance.numMergeOtherRefObj.num_merge_organize_item;
            NPCommonItem eliminateItem = HotfixRefdataCoreMgr.instance.numMergeOtherRefObj.num_merge_eliminate_item;
            
            if (organizeItem != null && organizeItem.itemType == ENPItemType.ACTIVITY_CURRENCY && organizeItem.itemId == activityCurrencyId)
                refreshOrganizeItem();
            if (eliminateItem != null && eliminateItem.itemType == ENPItemType.ACTIVITY_CURRENCY && eliminateItem.itemId == activityCurrencyId)
                refreshEliminateItem();
            if (ticketItem != null && ticketItem.itemType == ENPItemType.ACTIVITY_CURRENCY && ticketItem.itemId == activityCurrencyId)
                refreshTicketItem(true);
        }
        //活动状态变更
        private void _onActivityStateChg(params object[] _objects)
        {
            if (_objects == null || _objects.Length < 4)
                return;

            long activityId = (long)_objects[0];
            GActivityMainRefObj activityRef = GRefdataCoreMgr.instance.activityMainRefCore.getRef(activityId);
            if (activityRef is not { type_id: ECommonActivityType.NUM_MERGE })
                return;

            EActivityState nowActivityState = (EActivityState)_objects[3];
            if (nowActivityState != EActivityState.PLAYING)
            {
                NPMesMgr.instance.showOneBtnMes(
                    TextTranslate.instance.getLanguage(TransKeyConst.common_activity_alreadyEnd_none),
                    TextTranslate.instance.getLanguage(TransKeyConst.confirm),
                    () => QueueMgr.instance.QuitUntilCanStop(_node => _node is { nodeTag: HotfixUINodeTagConst.NUMMERGE_GAME }, true));
            }
        }
        private void _onLazyCDChg(object[] _objs)
        {
            if (_objs is { Length: 0 })
                return;

            long cdId = (long) _objs[0];
            long lazyCDId = HotfixRefdataCoreMgr.instance.numMergeOtherRefObj.num_merge_lazy_cd_id;
            if (cdId != lazyCDId)
                return;

            // 判断体力是否上升
            PlayerLazyCDInfo lazyCdInfo = NPPlayer.instance.lazyCdComp.getLazyCDInfo(lazyCDId);
            if (lazyCdInfo != null)
            {
                int curCount = lazyCdInfo.getCount();
                // 非初始记录，且当前数量大于上次记录，说明体力上升了
                if (_m_lastLazyCDCount < curCount)
                    _showLazyCDAddTip(curCount - _m_lastLazyCDCount);
                _m_lastLazyCDCount = curCount;
            }
        }
        private void _showLazyCDAddTip(int _addCount)
        {
            NPGUIAddSceneCenterTip.instance.showIconTextTip(
                GCommon.getItemTexIcon(ENPItemType.LAZY_CD, HotfixRefdataCoreMgr.instance.numMergeOtherRefObj.num_merge_lazy_cd_id), 
                TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, _addCount));
        }
        private void _showUseItemTip()
        {
            if (hotfixWnd == null || hotfixWnd.useItemTipAnim == null)
                return;

            hotfixWnd.useItemTipAnim.ForcePlay(hotfixWnd.useItemTipAnimName);
        }
        //点击图鉴按钮
        private void _onClickHandbook(GameObject _go)
        {
            //打开图鉴界面
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndNumMergeHandbook.instance, GGUIWndNumMergeHandbook.instance.showWnd, HotfixUINodeTagConst.NUMMERGE_HANDBOOK);
        }
    }
}
