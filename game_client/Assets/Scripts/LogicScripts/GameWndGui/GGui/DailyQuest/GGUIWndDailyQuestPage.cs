using System;
using System.Collections.Generic;
using ALPackage;
using Common.QuestEnum;
using Common.QuestObj;
using CommonEnum;
using GS2GC.p028_QuestOp;
using NPCommon;
using NPEnum;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 每日任务页面
    /// </summary>
    public class GGUIWndDailyQuestPage : _ATALBasicLoadPrefabSubUIWnd<GGUIMonoDailyQuestPage>
    {
        private string _m_sAssetPath;//窗口对象的资源 加载路径
        private string _m_sObjName;//窗口对象的资源 名字

        private List<_ISliderRewardItemInfo> _m_lProcessInfoList;//活跃积分进度信息列表
        private DailyQuestGroupItem _m_dailyQuestGroupItem;//任务数据
        private GGUIWndDailyQuestContainer _m_wDailyQuestContainer;//任务列表
        private GGUIWndCommonRewardSlider _m_wRewardSlider;//活跃积分进度
        private ALCommonEnableTaskController _m_tcTickTaskController;//倒计时任务
        private NPGGuiWndTexture _m_wScoreIcon;//积分图标
        private bool _m_bIsPlayingAni;//是否正在领奖
        private NPGGUISubOutSetHarvestWnd _m_harvestWnd;//粒子动画wnd
        private List<CommonUISfxObj> _m_lSfxObjList;//特效列表

        //预览弹窗
        private NPGGUIWndCommonRewardPreview _m_previewWnd;
        
        public GGUIWndDailyQuestPage(NPCommonAssetPathInfo _assetPathInfo, Transform _parent)
            : base(_parent)
        {
            _m_sAssetPath = _assetPathInfo.asset_path;
            _m_sObjName = _assetPathInfo.obj_name;
        }

        protected override string _monoAssetPath { get { return _m_sAssetPath; } }
        protected override string _monoObjName { get { return _m_sObjName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.DAILY_QUEST_CHG, _onDailyQuestChg);//日常任务信息变更
            WinMsg.RegisterMsg(WinMsgType.DAILY_QUEST_REWARD_CHG, _onActiveChg);//已领取活跃奖励变更
            WinMsg.RegisterMsg(WinMsgType.DAILY_QUEST_GROUP_CHG, _onDailyQuestGroupChg);//任务组变动
            
            WinMsg.RegisterMsg(WinMsgType.SIMULATE_CLICK_DAILY_QUEST_REWARD, _simulateClickReward);

            
            _m_bIsPlayingAni = false;

            //初始化活跃积分进度奖励信息列表
            DailyQuestGroupItem groupItem = NPPlayer.instance.dailyQuestComp.getDailyQuestGroupItemByType(EDailyQuestType.DAY);
            if (_m_lProcessInfoList == null)
                _m_lProcessInfoList = new List<_ISliderRewardItemInfo>();
            else
                _m_lProcessInfoList.Clear();

            if (groupItem != null && groupItem.activeRewardList != null)
            {
                for (int i = 0; i < groupItem.activeRewardList.Count; i++)
                {
                    if (groupItem.activeRewardList[i] != null)
                        _m_lProcessInfoList.Add(new DailyQuestProcessInfo(groupItem.activeRewardList[i]));
                }
            }

            //停止原先任务
            _m_tcTickTaskController.setDisable();
            //开启任务进行数据逻辑的处理
            _m_tcTickTaskController = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(_tick1Sec, 1f);


            _refreshWnd(true);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.DAILY_QUEST_CHG, _onDailyQuestChg);//日常任务信息变更
            WinMsg.UnregisterMsg(WinMsgType.DAILY_QUEST_REWARD_CHG, _onActiveChg);//已领取活跃奖励变更
            WinMsg.UnregisterMsg(WinMsgType.DAILY_QUEST_GROUP_CHG, _onDailyQuestGroupChg);//任务组变动

            WinMsg.UnregisterMsg(WinMsgType.SIMULATE_CLICK_DAILY_QUEST_REWARD, _simulateClickReward);
            
            _m_bIsPlayingAni = false;
            _m_tcTickTaskController.setDisable();

            if (_m_wDailyQuestContainer != null)
                _m_wDailyQuestContainer.hideWnd();

            if (_m_wRewardSlider != null)
                _m_wRewardSlider.hideWnd();

            if (_m_wScoreIcon != null)
                _m_wScoreIcon.hideWnd();

            _discardSfx();
        }

        protected override void _onReset()
        {
            if (_m_wDailyQuestContainer != null)
                _m_wDailyQuestContainer.resetWnd();

            if (_m_wRewardSlider != null)
                _m_wRewardSlider.resetWnd();

            if (_m_wScoreIcon != null)
                _m_wScoreIcon.discardTexture();
        }

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            if (_m_wDailyQuestContainer != null)
                _m_wDailyQuestContainer.discard();
            _m_wDailyQuestContainer = null;

            if (_m_wRewardSlider != null)
                _m_wRewardSlider.discard();
            _m_wRewardSlider = null;

            if (null != _m_previewWnd)
                _m_previewWnd.discard();
            _m_previewWnd = null;

            if (null != _m_wScoreIcon)
                _m_wScoreIcon.discard();
            _m_wScoreIcon = null;

            if (null != _m_harvestWnd)
                _m_harvestWnd.discard();
            _m_harvestWnd = null;

            _discardSfx();
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoDailyQuestContainer != null)
            {
                _m_wDailyQuestContainer = new GGUIWndDailyQuestContainer(wnd.monoDailyQuestContainer);
                _m_wDailyQuestContainer.onClickGetReward += _onClickGetReward;
                _m_wDailyQuestContainer.onClickOnceGetReward += _onClickOnceGetReward;

            }

            if (wnd.monoRewardSlider != null)
            {
                _m_wRewardSlider = new GGUIWndCommonRewardSlider(wnd.monoRewardSlider);
                _m_wRewardSlider.onClickItem += _onClickItem;
            }

            if (wnd.imgScoreIcon != null)
                _m_wScoreIcon = new NPGGuiWndTexture(wnd.imgScoreIcon);

            if (wnd.particleEndRect != null)
            {
                _m_harvestWnd = new NPGGUISubOutSetHarvestWnd(wnd.particleEndRect, EHarvestType.DAILY_QUEST_ACTIVE_POINT, _particleResChgDelegate, _particleStartDelegate, _particleFirstItemDoneDelegate, _particlePerItemDoneDelegate, _particleAllItemDoneDelegate);
                _m_harvestWnd.init();
            }
        }

        //刷新窗口
        private void _refreshWnd(bool _needMoveToTop)
        {
            _m_dailyQuestGroupItem = NPPlayer.instance.dailyQuestComp.getDailyQuestGroupItemByType(EDailyQuestType.DAY);
            if (_m_dailyQuestGroupItem == null)
            {
                Debug.LogError("GGUIWndDailyQuestPage 未获取到每日任务数据");
                return;
            }

            _refreshGrid(_needMoveToTop);
            _refreshActiveSlider();
            _refreshScoreIcon();
            _refreshActiveScore();
            _refreshActiveAllFinishSate();
        }

        //刷新列表
        private void _refreshGrid(bool _needMoveToTop)
        {
            if (_m_dailyQuestGroupItem == null)
                return;

            List<DailyQuestItem> dailyQuestItemList = new List<DailyQuestItem>();
            _m_dailyQuestGroupItem.getAllShowQuestList(dailyQuestItemList);
            List<long> canFinishList = new List<long>();
            _m_dailyQuestGroupItem.getCanFinishQuestList(canFinishList);
            //是否可以展示一键领取按钮
            bool canShowOneKey = GRefdataCoreMgr.instance.npGeneral.daily_quest_one_key_finish_condition != null &&
                                 GRefdataCoreMgr.instance.npGeneral.daily_quest_one_key_finish_condition.IsEnable(null);

            if (_m_wDailyQuestContainer != null)
            {
                _m_wDailyQuestContainer.showWnd();
                _m_wDailyQuestContainer.setShowData(dailyQuestItemList, canFinishList.Count > 1 && canShowOneKey);
                if (_needMoveToTop)
                    _m_wDailyQuestContainer.moveToTop();
            }
        }

        //刷新日常积分进度
        private void _refreshActiveSlider()
        {
            if (wnd == null)
                return;

            long curScore = NPPlayer.instance.rescourceComp.getValue(ECurrency.DAILY_QUEST_ACTIVE_POINT);
            if (_m_wRewardSlider != null)
            {
                _m_wRewardSlider.showWnd();
                _m_wRewardSlider.setInfo(curScore, _m_lProcessInfoList);
            }

            if (null != _m_harvestWnd)
                _m_harvestWnd.setResChg(curScore);
        }

        //刷新积分图标
        private void _refreshScoreIcon()
        {
            if (_m_wScoreIcon != null)
            {
                _m_wScoreIcon.showWnd();
                _m_wScoreIcon.setTexture(GCommon.getItemTexIcon(ENPItemType.CURRENCY,(long)ECurrency.DAILY_QUEST_ACTIVE_POINT));
            }
        }

        //刷新活跃积分
        private void _refreshActiveScore()
        {
            if (wnd == null)
                return;

            long curScore = NPPlayer.instance.rescourceComp.getValue(ECurrency.DAILY_QUEST_ACTIVE_POINT);
            ALUGUICommon.setLabelTxt(wnd.txtScore, curScore.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
        }

        //刷新活跃奖励是否全部领取显隐状态
        private void _refreshActiveAllFinishSate()
        {
            if (wnd == null)
                return;

            bool isAllFinish = true;
            if (_m_lProcessInfoList != null)
            {
                for (int i = 0; i < _m_lProcessInfoList.Count; i++)
                {
                    if (_m_lProcessInfoList[i] != null && _m_lProcessInfoList[i].rewardState != ESliderRewardState.ALREADY_GET)
                    {
                        isAllFinish = false;
                        break;
                    }
                }
            }

            ALUGUICommon.setGameObjEnable(wnd.goActiveRewardAllGetShowList, isAllFinish);
            ALUGUICommon.setGameObjEnable(wnd.goActiveRewardAllGetHideList, !isAllFinish);
        }

        //刷新日常积分状态
        private void _refreshProcessState()
        {
            if (_m_wRewardSlider != null)
            {
                _m_wRewardSlider.showWnd();
                _m_wRewardSlider.refreshAllState();
            }
        }

        //每秒刷新剩余时间
        private void _tick1Sec()
        {
            if (wnd == null)
                return;

            DailyQuestGroupItem groupItem = NPPlayer.instance.dailyQuestComp.getDailyQuestGroupItemByType(EDailyQuestType.DAY);
            if (groupItem == null)
                return;

            long leftTimeMs = groupItem.nextRefreshTimeMs - FpsAndPingMgr.instance.serverTimeTag;
            if (leftTimeMs < 0)
                leftTimeMs = 0;

            string leftTimeMsStr = TimeUtil.millisecondsToTime_hms(leftTimeMs);
            ALUGUICommon.setLabelTxt(wnd.txtTime, TextTranslate.instance.getLanguage(TransKeyConst.dailyQuest_leftResetTime_time, leftTimeMsStr));
        }

        //销毁特效
        private void _discardSfx()
        {
            if (_m_lSfxObjList != null)
            {
                for (int i = 0; i < _m_lSfxObjList.Count; i++)
                {
                    _m_lSfxObjList[i]?.forceDiscard();
                }
                _m_lSfxObjList.Clear();
                _m_lSfxObjList = null;
            }
        }

        #region 点击事件

        //点击进度item
        private void _onClickItem(GGUIWndCommonRewardSliderContainerItem _itemWnd)
        {
            if (_itemWnd == null || _itemWnd.wnd == null || _itemWnd.itemInfo == null)
                return;

            switch (_itemWnd.itemInfo.rewardState)
            {
                case ESliderRewardState.CAN_GET://可领取
                    {
                        //是否一键领取活跃度奖励
                        bool canShowOneKey = GRefdataCoreMgr.instance.npGeneral.daily_quest_one_key_finish_condition != null &&
                                             GRefdataCoreMgr.instance.npGeneral.daily_quest_one_key_finish_condition.IsEnable(null);

                        if (canShowOneKey)
                            NPPlayer.instance.dailyQuestComp.reqDailyQuestAKeyDrawActiveReward(EDailyQuestType.DAY);
                        else
                            NPPlayer.instance.dailyQuestComp.reqDrawActiveReward(EDailyQuestType.DAY, _itemWnd.itemInfo.id);
                        break;
                    }
                case ESliderRewardState.CAN_NOT_GET://不可领取
                    _showRewardPreview(_itemWnd.itemInfo, _itemWnd.rectTransform, ECommonRewardType.NOT_GET_REWARD);
                    break;
                case ESliderRewardState.ALREADY_GET://已领取
                    _showRewardPreview(_itemWnd.itemInfo, _itemWnd.rectTransform, ECommonRewardType.HAS_GET_REWARD);
                    break;
            }
        }

        //显示奖励预览弹窗
        private void _showRewardPreview(_ISliderRewardItemInfo _itemInfo, RectTransform _toolTipShowRoot, ECommonRewardType _rewardType)
        {
            if (wnd == null || _itemInfo == null || wnd.boxRewardPreviewResPathId <= 0)
                return;

            DailyQuestActiveRewardRefObj refObj = GRefdataCoreMgr.instance.dailyQuestRewardListCore.getRef(_itemInfo.id);
            if (refObj == null)
                return;

            List<NPCommonCostItem> itemList = GCommon.getShowListByReawrdId(refObj.reward_id);
            long uiPathId = wnd.boxRewardPreviewResPathId;
            string titleStr = string.IsNullOrEmpty(wnd.boxRewardPreviewTitleStrKey) ? "" : TextTranslate.instance.getLanguage(wnd.boxRewardPreviewTitleStrKey);

            QueueMgr.instance.AddNode(new GNodeCommonToolTip_Reward(
                uiPathId, 
                titleStr, 
                string.Empty, 
                itemList, 
                _toolTipShowRoot, 
                wnd.boxRewardPreviewToolTipOffset, 
                _rewardType));
        }

        //模拟领取奖励
        private void _simulateClickReward(object[] _objs)
        {
            if(null == _m_wDailyQuestContainer)
                return;
            
            if(null == _objs || _objs.Length == 0)
                return;
            
            long index = (long) _objs[0];
            
            GGUIWndDailyQuestContainerItem itemWnd = _m_wDailyQuestContainer.getItemWnd((int)index);
            if(null == itemWnd)
                return;

            itemWnd.simulateClickClickGetReward();
        }

        //点击领取奖励
        private void _onClickGetReward(GGUIWndDailyQuestContainerItem _item, RectTransform _starTransform)
        {
            if (_item == null || _item.dailyQuestInfo == null || _item.dailyQuestInfo.getDailyQuestState() != EDailyQuestState.CAN_GET)
                return;

            if (_m_bIsPlayingAni)
                return;

            _m_bIsPlayingAni = true;
            NPPlayer.instance.dailyQuestComp.reqFinishDailyQuest(EDailyQuestType.DAY, _item.dailyQuestInfo.dailyQuestId, (_itemList) =>
            {
                if (_item == null)
                    return;

                _item.playGetRewardAnimation(() =>
                {
                    _m_bIsPlayingAni = false;
                    _refreshGrid(false);
                });
                _playOnceParticleAni(_item.dailyQuestInfo, _starTransform);
            }, () =>
            {
                _m_bIsPlayingAni = false;
            });
        }

        //点击一键领取每日任务
        private void _onClickOnceGetReward(List<GGUIWndDailyQuestContainerItem> _itemList)
        {
            if (_m_bIsPlayingAni)
                return;

            _m_bIsPlayingAni = true;
            NPPlayer.instance.dailyQuestComp.reqDailyQuestAKeyDrawFinishReward(EDailyQuestType.DAY, () =>
            {
                if (_itemList == null)
                    return;

                GGUIWndDailyQuestContainerItem item = null;
                for (int i = 0; i < _itemList.Count; i++)
                {
                    item = _itemList[i];
                    if (null == item || null == item.wnd)
                        continue;

                    item.playGetRewardAnimation(() =>
                    {
                        _m_bIsPlayingAni = false;
                        _refreshGrid(false);
                    });
                    _playOnceParticleAni(item.dailyQuestInfo, item.wnd.particleStartTrans);
                }
            }, () =>
            {
                _m_bIsPlayingAni = false;
            });
        }

        //播放单个粒子动画
        private void _playOnceParticleAni(DailyQuestItem _item, RectTransform _starTransform)
        {
            if (_item == null || _starTransform == null)
                return;

            //播放粒子动画
            NPCommonCostItem activationItem = _item.dailyQuestRef.activation_item;
            long particleCount = activationItem != null ? activationItem.count : 0;
            GGUIHarvestCore.instance.startHarvestCollection(EHarvestType.DAILY_QUEST_ACTIVE_POINT, _starTransform, (int)particleCount, GRefdataCoreMgr.instance.npGeneral.daily_quest_sore_particle_id, 1);
        }
        #endregion

        #region 消息事件

        //日常任务信息变更
        private void _onDailyQuestChg(params object[] _objects)
        {
            if(!_m_bIsPlayingAni)
                _refreshGrid(false);
        }

        //已领取活跃奖励变更
        private void _onActiveChg(params object[] _objects)
        {
            _refreshProcessState();
            _refreshActiveAllFinishSate();
            if (!_m_bIsPlayingAni) 
                _refreshGrid(false);
        }

        //任务组变动
        private void _onDailyQuestGroupChg(params object[] paramsObjects)
        {
            _refreshWnd(true);
        }

        #endregion

        #region 粒子事件

        /// <summary>
        /// 资源变动回调
        /// </summary>
        private void _particleResChgDelegate()
        {
            _setCurPoint();
        }

        /// <summary>
        /// 粒子动画开始回调
        /// </summary>
        private void _particleStartDelegate()
        {
            if (wnd != null && wnd.particleAni != null)
                wnd.particleAni.play(EAchieveParticleAniType.START_PARTICLE);
        }

        /// <summary>
        /// 第一个粒子结束回调
        /// </summary>
        /// <param name="_itemCount"></param>
        private void _particleFirstItemDoneDelegate(long _itemCount)
        {
            //播放特效
            if (wnd != null && wnd.particleSfxId > 0)
            {
                if (_m_lSfxObjList == null)
                    _m_lSfxObjList = new List<CommonUISfxObj>();

                CommonUISfxObj sfxObj = PlaySfxMgr.instance.playUISfx(wnd.particleSfxId, wnd.particleSfxParent);
                if (sfxObj != null)
                    _m_lSfxObjList.Add(sfxObj);

                //销毁多余的特效
                if (_m_lSfxObjList.Count > wnd.particleSfxMaxNum)
                {
                    while (_m_lSfxObjList.Count > wnd.particleSfxMaxNum)
                    {
                        if (_m_lSfxObjList.Count <= 0)
                            break;

                        _m_lSfxObjList[0]?.forceDiscard();
                        _m_lSfxObjList.RemoveAt(0);
                    }
                }
            }
        }

        /// <summary>
        /// 单个粒子结束回调
        /// </summary>
        private void _particlePerItemDoneDelegate(long _itemCount)
        {
            _setCurPoint();

            if (wnd != null && wnd.particleAni != null)
                wnd.particleAni.play(EAchieveParticleAniType.SINGLE_PARTICLE);
        }

        private void _particleAllItemDoneDelegate()
        {
            //动画结束后刷新，当前阶段变化
            _refreshGrid(false);
            _refreshActiveScore();
            _refreshActiveAllFinishSate();

            //刷新计数
            if (null != _m_harvestWnd)
            {
                long curScore = NPPlayer.instance.rescourceComp.getValue(ECurrency.DAILY_QUEST_ACTIVE_POINT);
                _m_harvestWnd.setResChg(curScore);
            }
        }


        /// <summary>
        /// 设置当前分数
        /// </summary>
        private void _setCurPoint()
        {
            if (wnd == null || _m_harvestWnd == null)
                return;

            //设置进度条
            long curScore = _m_harvestWnd.getCurCount();

            if (_m_wRewardSlider != null)
            {
                _m_wRewardSlider.showWnd();
                _m_wRewardSlider.refreshStateByScore(curScore);
            }
            ALUGUICommon.setLabelTxt(wnd.txtScore, curScore.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
        }

        #endregion

    }

    /// <summary>
    /// 每日任务进度奖励列表信息
    /// </summary>
    public class DailyQuestProcessInfo : _ISliderRewardItemInfo
    {
        private DailyQuestActiveRewardRefObj _m_lDailyQuestActiveRef;

        /// <summary>
        /// id
        /// </summary>
        public long id
        {
            get { return _m_lDailyQuestActiveRef != null ? _m_lDailyQuestActiveRef.id : 0; }
        }
        /// <summary>
        /// 分数
        /// </summary>
        public long score
        {
            get
            {
                return _m_lDailyQuestActiveRef != null && _m_lDailyQuestActiveRef.draw_active_reward_need != null
                    ? _m_lDailyQuestActiveRef.draw_active_reward_need.count
                    : 0;
            }
        }

        public EValueFormatType valueFormatType { get { return EValueFormatType.NORMAL; } }

        public string showScore { get { return string.Empty; } }

        /// <summary>
        /// 奖励领取状态
        /// </summary>
        public ESliderRewardState rewardState
        {
            get
            {
                if (_m_lDailyQuestActiveRef == null || _m_lDailyQuestActiveRef.draw_active_reward_need == null || _m_lDailyQuestActiveRef.draw_active_reward_need.getItemType() == ENPItemType.NONE)
                    return ESliderRewardState.CAN_NOT_GET;

                long curScore = NPPlayer.instance.rescourceComp.getValue(ECurrency.DAILY_QUEST_ACTIVE_POINT);
                DailyQuestGroupItem groupItem = NPPlayer.instance.dailyQuestComp.getDailyQuestGroupItemByType(EDailyQuestType.DAY);
                if (groupItem != null && groupItem.hasTakenActiveRewardRefIdList != null)
                {
                    if (groupItem.hasTakenActiveRewardRefIdList.Contains(_m_lDailyQuestActiveRef.id))
                        return ESliderRewardState.ALREADY_GET;
                    else if (curScore >= _m_lDailyQuestActiveRef.draw_active_reward_need.count)
                        return ESliderRewardState.CAN_GET;
                    else
                        return ESliderRewardState.CAN_NOT_GET;
                }
                else
                    return ESliderRewardState.CAN_NOT_GET;
            }
        }
        /// <summary>
        /// 宝箱图标
        /// </summary>
        public NPGTextureIndex icon 
        {
            get { return null; }
        }

        public _IItem showRewardItem { get { return null; } }

        public DailyQuestProcessInfo(DailyQuestActiveRewardRefObj _refObj)
        {
            _m_lDailyQuestActiveRef = _refObj;
        }
    }
}