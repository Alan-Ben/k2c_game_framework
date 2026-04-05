using System;
using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;

namespace Hotfix
{
    public class GGUISubWndTileMatchStepReward : _AHotfixBaseSubWnd<GGUISubMonoTileMatchStepReward>
    {
        private long _m_lShowSerializeId;
        
        private TileMatchModeRefObj _m_rGameModeRefObj;

        private TileMatchStepRewardRefObj _m_rCurStepRewardRefObj;//当前所处阶段奖励配表数据
        private long _m_lCurStageScore;//当前阶段分数
        private long _m_lNeedAddStageScore;//需要增加的阶段分数
        private bool _m_bIsDealAddStageScoreShow;//是否正在进行增加阶段分数表现
        
        private long _m_lActivityTotalScore;//活动总积分
        
        private NPGGUIWndProgress _m_wStageScoreSlider;//阶段分数进度条
        
        private GGUIWndTileMatchStepRewardBox _m_wStepRewardBox;//阶段奖励宝箱
        private GGUIWndCommonSimpleItem _m_wActivityExchangeTokensItem;//活动兑换币item
        
        private NPGGUICommonTipDealerMgr _m_tipMgr;//tip管理器
        
        private NPCenterTipsRefObj _m_rAddActivityScoreCenterTipsRefObj;//增加活动分数tip配表数据
        
        public GGUISubWndTileMatchStepReward(GGUIHotfixCommonMono _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDoneHotfix()
        {
            if(hotfixWnd == null)
                return;

            if (hotfixWnd.monoScoreSlider != null)
                _m_wStageScoreSlider = new NPGGUIWndProgress(hotfixWnd.monoScoreSlider);

            if (hotfixWnd.monoActivityExchangeTokensItem != null)
                _m_wActivityExchangeTokensItem = new GGUIWndCommonSimpleItem(hotfixWnd.monoActivityExchangeTokensItem);

            if (hotfixWnd.tipMgrParent != null)
                _m_tipMgr = new NPGGUICommonTipDealerMgr(hotfixWnd.tipMgrParent);

            _m_rAddActivityScoreCenterTipsRefObj = GRefdataCoreMgr.instance.tipMap.getRef(hotfixWnd.addActivityScoreTipId);
            
            ALUGUICommon.combineBtnClick(hotfixWnd.btnRewardPreview, _onBtnRewardPreviewClick);
        }
        
        protected override void _onDiscard()
        {
            if (hotfixWnd != null)
            {
                ALUGUICommon.uncombineBtnClick(hotfixWnd.btnRewardPreview, _onBtnRewardPreviewClick);
            }
            
            _m_lNeedAddStageScore = 0;
            _m_bIsDealAddStageScoreShow = false;
            
            _m_wStageScoreSlider?.discard();
            _m_wStageScoreSlider = null;

            if (_m_wStepRewardBox != null)
            {
                _m_wStepRewardBox.onDrawBtnClick -= _onDrawRewardBtnClick;
                _m_wStepRewardBox.discard();
                _m_wStepRewardBox = null;    
            }
            
            _m_wActivityExchangeTokensItem?.discard();
            _m_wActivityExchangeTokensItem = null;
            
            _m_tipMgr?.clear();
            _m_tipMgr = null;

            _m_rAddActivityScoreCenterTipsRefObj = null;

            _m_rGameModeRefObj = null;
        }
        
        protected override void _onShowWnd()
        {
            _m_lShowSerializeId = ALSerializeOpMgr.next();

            refreshWnd();
            
            _m_tipMgr?.start();
            
            ALMsgSys.RegisterMsgAct(HotfixMsgType.ON_TILEMATCH_CAN_DRAW_STEP_REWARD_INFO_CHG, _onTileMatchCanDrawStepRewardChg);
            ALMsgSys.RegisterMsg(HotfixMsgType.ON_A_TILEMATCH_LOGIC_PROCESS_DEAL_DONE, _onATilematchLogicProcessDealDone);
            WinMsg.RegisterMsg(WinMsgType.ON_ACTIVITY_CURRENCY_CHG, _onActivityCurrencyChg);
            ALMsgSys.RegisterMsg(HotfixMsgType.ON_TILEMATCH_SAME_SERIALIZE_LOGIC_PROCESS_DEAL_DONE, _onSameSerializeTileMatchLogicProcessStartDeal);
        }

        protected override void _onHideWnd()
        {
            _m_lShowSerializeId = ALSerializeOpMgr.next();

            ALMsgSys.UnregisterMsgAct(HotfixMsgType.ON_TILEMATCH_CAN_DRAW_STEP_REWARD_INFO_CHG, _onTileMatchCanDrawStepRewardChg);
            ALMsgSys.UnregisterMsg(HotfixMsgType.ON_A_TILEMATCH_LOGIC_PROCESS_DEAL_DONE, _onATilematchLogicProcessDealDone);
            WinMsg.UnregisterMsg(WinMsgType.ON_ACTIVITY_CURRENCY_CHG, _onActivityCurrencyChg);
            ALMsgSys.UnregisterMsg(HotfixMsgType.ON_TILEMATCH_SAME_SERIALIZE_LOGIC_PROCESS_DEAL_DONE, _onSameSerializeTileMatchLogicProcessStartDeal);

            _m_lNeedAddStageScore = 0;
            _m_bIsDealAddStageScoreShow = false;
            
            _m_wStageScoreSlider?.hideWnd();
            _m_wStepRewardBox?.hideWnd();
            _m_wActivityExchangeTokensItem?.hideWnd();
            
            _m_tipMgr?.clear();
        }

        protected override void _onReset()
        {
            _m_lNeedAddStageScore = 0;
            _m_bIsDealAddStageScoreShow = false;
            
            _m_wStageScoreSlider?.resetWnd();
            _m_wStepRewardBox?.resetWnd();
            _m_wActivityExchangeTokensItem?.resetWnd();
            
            _m_tipMgr?.clear();
        }

        public void updateGameModel()
        {
            _m_rGameModeRefObj = HotfixRefdataCoreMgr.instance.tileMatchModeRefCore.getRef((long) HotfixAccountSettingMgr.instance.hotfixAccountSetting.getTileMatchModelType());
        }
        
        /// <summary>
        /// 从component中获取阶段奖励信息
        /// </summary>
        private void _updateCueStepInfoFormComponent()
        {
            _m_lNeedAddStageScore = 0;

            _m_rCurStepRewardRefObj = HotfixNPPlayer.instance.tileMatchComponent.stepRewardInfo?.stepRewardRefObj;
            _m_lCurStageScore = HotfixNPPlayer.instance.tileMatchComponent.stepRewardInfo?.curScore ?? 0;
            if(_m_rCurStepRewardRefObj == null)
                return;

            _updateStepInfo();
        }

        /// <summary>
        /// 更新阶段奖励信息
        /// </summary>
        private void _updateStepInfo()
        {
            _m_lCurStageScore += _m_lNeedAddStageScore;
            _m_lNeedAddStageScore = 0;
            while (_m_rCurStepRewardRefObj != null && _m_rCurStepRewardRefObj.goal <= _m_lCurStageScore)
            {
                _m_lCurStageScore -= _m_rCurStepRewardRefObj.goal;// 减去当前阶段的目标分数
                TileMatchStepRewardRefObj nextStepRewardRefObj = HotfixRefdataCoreMgr.instance.tileMatchStepRewardRefCore.getRef(_m_rCurStepRewardRefObj.step + 1);
                if (nextStepRewardRefObj != null)
                    _m_rCurStepRewardRefObj = nextStepRewardRefObj;
            }
        }
        
        /// <summary>
        /// 刷新窗口内容
        /// </summary>
        public void refreshWnd()
        {
            updateGameModel();

            _refreshCurStepRewardInfo();
            _refreshStepRewardBox();

            //刷新活动兑换币道具
            _refreshActivityExchangeTokensItem();

            // 刷新活动总积分
            _refreshActivityTotalScore();
        }
        
        /// <summary>
        /// 刷新当前阶段奖励信息
        /// </summary>
        private void _refreshCurStepRewardInfo()
        {
            _updateCueStepInfoFormComponent();
            
            _refreshScoreSld();
            
            _m_bIsDealAddStageScoreShow = false;
        }

        private void _refreshScoreSld()
        {
            if (_m_wStageScoreSlider != null)
            {
                _m_wStageScoreSlider.showWnd();
                _m_wStageScoreSlider.setProgress(_m_lCurStageScore, _m_rCurStepRewardRefObj?.goal ?? 0, EValueFormatType.NORMAL_NOT_LARGE_STR, HotfixTransKeyConst.tilematch_stepRewardScoreProgress_num2);
            }
        }

        /// <summary>
        /// 刷新活动兑换币道具
        /// </summary>
        private void _refreshActivityExchangeTokensItem()
        {
            if (_m_wActivityExchangeTokensItem != null)
            {
                _m_wActivityExchangeTokensItem.showWnd();
                NPCommonItem exchangeTokensItem = HotfixRefdataCoreMgr.instance.tileMatchOtherRefObj.tilematch_currency_item;
                NPCommonCostItem exchangeTokensCostItem = new NPCommonCostItem(exchangeTokensItem, GCommon.getItemCount(exchangeTokensItem));
                _m_wActivityExchangeTokensItem.setItem(exchangeTokensCostItem);
            }
        }

        /// <summary>
        /// 刷新活动总积分
        /// </summary>
        private void _refreshActivityTotalScore()
        {
            _m_lActivityTotalScore = HotfixNPPlayer.instance.tileMatchComponent.activityTotalScore;

            if (hotfixWnd != null)
                ALUGUICommon.setLabelTxt(hotfixWnd.txtActivityScore, TextTranslate.instance.getLanguage(HotfixTransKeyConst.tilematch_activityTotalScore_num, 
                    _m_lActivityTotalScore.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));
        }

        public void addActivityTotalScore(long _addScore)
        {
            if(_addScore == 0)
                return;
            
            _m_lActivityTotalScore += _addScore;
            
            if (hotfixWnd != null)
                ALUGUICommon.setLabelTxt(hotfixWnd.txtActivityScore, TextTranslate.instance.getLanguage(HotfixTransKeyConst.tilematch_activityTotalScore_num, 
                    _m_lActivityTotalScore.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));

            // 展示获取活动积分tip
            if (_m_tipMgr != null && _m_rAddActivityScoreCenterTipsRefObj != null)
            {
                List<string> tipTextList = ListPool<string>.Get();
                tipTextList.Add(TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, _addScore.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));
                _m_tipMgr.addTip(new NPTextTipDealer(tipTextList, _m_rAddActivityScoreCenterTipsRefObj, (_tipWnd)=>
                {
                    ListPool<string>.Release(tipTextList);
                }));
            }
        }
        
        /// <summary>
        /// 增加分数
        /// </summary>
        /// <param name="_score"></param>
        public void addStageScore(long _score)
        {
            if(_score == 0)
                return;
            
            _m_lNeedAddStageScore += _score;
            // // 同步增加活动总分数
            // _addActivityTotalScore(_score);
            
            if(_m_bIsDealAddStageScoreShow)//若正在进行增加分数表现，则直接返回
                return;

            _dealAddScoreShow(20);
        }

        /// <summary>
        /// 进行分数增加表现
        /// </summary>
        /// <param name="_leftCanShowChgCount">剩余的可显示变化的次数, 因为增加的分数有可能导致阶段多次变化, 所以这里限制可变化阶段的展示次数, 防止出错导致的递归无限调用</param>
        private void _dealAddScoreShow(int _leftCanShowChgCount)
        {
            if (hotfixWnd == null || !isShow)
            {
                _m_bIsDealAddStageScoreShow = false;
                return;
            }

            // 若没有需要增加的分数 或 当前阶段奖励配表数据为null, 则直接刷新分数进度条显示
            if (_m_lNeedAddStageScore <= 0 || _m_rCurStepRewardRefObj == null)
            {
                _refreshScoreSld();
                _m_bIsDealAddStageScoreShow = false;
                return;
            }
            
            // 若超出了最多可以显示的分数变化次数，则直接当前阶段奖励信息
            if (_leftCanShowChgCount <= 0)
            {
                _refreshCurStepRewardInfo();
                _m_bIsDealAddStageScoreShow = false;
                return;
            }
            
            _m_bIsDealAddStageScoreShow = true;
            
            _m_lCurStageScore += _m_lNeedAddStageScore;//更新总分数值
            _m_lNeedAddStageScore = 0;//需要增加的分数清0
            
            long serializeId = _m_lShowSerializeId;
            // 若当前阶段需要的分数 小于 当前拥有的分数, 表示当前阶段完成, 进行当前阶段完成表现
            if (_m_rCurStepRewardRefObj.goal <= _m_lCurStageScore)
            {
                TileMatchStepRewardRefObj completeStep = _m_rCurStepRewardRefObj;// 记录已完成的阶段配表数据
                // 更新当前所处阶段以及 当前阶段分数 和 需要增加的分数值(因为若存在多个阶段变化, 需要一个一个阶段表现, 所以下面的_m_lNeedAddStageScore和_m_lCurStageScore需要这样赋值)
                TileMatchStepRewardRefObj nextStep = HotfixRefdataCoreMgr.instance.tileMatchStepRewardRefCore.getRef(_m_rCurStepRewardRefObj.step + 1);
                if (nextStep != null)
                    _m_rCurStepRewardRefObj = nextStep;
                _m_lNeedAddStageScore = _m_lCurStageScore - completeStep.goal;
                _m_lCurStageScore = 0;

                if (_m_wStageScoreSlider == null)
                {
                    _dealAddScoreShow(_leftCanShowChgCount - 1);
                }
                else
                {
                    // 完整阶段的进度条展示
                    _m_wStageScoreSlider.setProgressChg(completeStep.goal, completeStep.goal, hotfixWnd.sldChgTime, EValueFormatType.NORMAL,
                        (_curValueStr, _totalValueStr) =>
                        {
                            return TextTranslate.instance.getLanguage(HotfixTransKeyConst.tilematch_stepRewardScoreProgress_num2, _curValueStr, _totalValueStr);
                        }, ()=>
                        {
                            if(serializeId != _m_lShowSerializeId && hotfixWnd != null)
                                return;
                           
                            // 进度条拉满后, 播放当前阶段完成动画
                            _playAnimation(hotfixWnd.curStepCompleteAnimName, () =>
                            {
                                if(serializeId != _m_lShowSerializeId)
                                    return;
                                
                                // 到了新阶段, 进度值从0开始
                                _m_wStageScoreSlider?.setProgress(0, nextStep?.goal ?? 0, EValueFormatType.NORMAL, HotfixTransKeyConst.tilematch_stepRewardScoreProgress_num2);
                                _dealAddScoreShow(_leftCanShowChgCount - 1);
                            });
                        });
                }
            }
            else
            {
                if (_m_wStageScoreSlider == null)
                {
                    _dealAddScoreShow(_leftCanShowChgCount - 1);
                }
                else
                {
                    _m_wStageScoreSlider.setProgressChg(_m_lCurStageScore, _m_rCurStepRewardRefObj.goal, hotfixWnd.sldChgTime, EValueFormatType.NORMAL,
                        (_curValueStr, _totalValueStr) =>
                        {
                            return TextTranslate.instance.getLanguage(HotfixTransKeyConst.tilematch_stepRewardScoreProgress_num2, _curValueStr, _totalValueStr);
                        }, ()=>
                        {
                            if(serializeId != _m_lShowSerializeId)
                                return;
                            
                            _dealAddScoreShow(_leftCanShowChgCount - 1);
                        });
                }
            }
        }
        
        /// <summary>
        /// 刷新阶段奖励宝箱
        /// </summary>
        private void _refreshStepRewardBox()
        {
            if(hotfixWnd == null || hotfixWnd.rewardBoxParent == null)
                return;
            
            // 最小可领取奖励阶段
            int minCanDrawRewardStep = HotfixNPPlayer.instance.tileMatchComponent.minCanDrawRewardStep;
            TileMatchStepRewardRefObj stepRewardRefObj = HotfixRefdataCoreMgr.instance.tileMatchStepRewardRefCore.getRef(minCanDrawRewardStep);
            if (stepRewardRefObj == null)
            {
                stepRewardRefObj = HotfixNPPlayer.instance.tileMatchComponent.stepRewardInfo?.stepRewardRefObj;
            }

            if (stepRewardRefObj == null || stepRewardRefObj.box_prefab_asset_path == null || !stepRewardRefObj.box_prefab_asset_path.enable)
            {
                _m_wStepRewardBox?.hideWnd();
                return;
            }

            // 若已存在宝箱窗口, 但是窗口的资源路径与当前需要的路径的不同，则需要重新创建
            if (_m_wStepRewardBox != null && _m_wStepRewardBox.boxAssetPathInfo != stepRewardRefObj.box_prefab_asset_path)
                _m_wStepRewardBox.discard();

            if (_m_wStepRewardBox == null)
            {
                _m_wStepRewardBox = new GGUIWndTileMatchStepRewardBox(stepRewardRefObj.box_prefab_asset_path, hotfixWnd.rewardBoxParent);
                _m_wStepRewardBox.onDrawBtnClick += _onDrawRewardBtnClick;
            }
            
            if(!_m_wStepRewardBox.isLoaded)
                _m_wStepRewardBox.load();
            
            _m_wStepRewardBox.regLoadDoneDelegate(() =>
            {
                _m_wStepRewardBox.showWnd();
            });
        }

        #region 消息监听

        /// <summary>
        /// 三消可领取阶段奖励信息发生变化
        /// </summary>
        private void _onTileMatchCanDrawStepRewardChg()
        {
            _refreshStepRewardBox();
        }

        /// <summary>
        /// 当一个三消逻辑过程处理完成时
        /// </summary>
        private void _onATilematchLogicProcessDealDone(params object[] _objs)
        {
            if(_objs == null || _objs.Length <= 0 || !(_objs[0] is TileMatchGameLogic._ATileMatchProcessLogicAgent _processLogicAgent))
                return;

            // if(_processLogicAgent.addScore > 0)
            //     Debug.LogError($"===========[_onATilematchLogicProcessDealDone] 处理完成, 增加分数addScore: {_processLogicAgent.addScore} _processLogicAgent:{_processLogicAgent}");
            
            addStageScore(_m_rGameModeRefObj == null ? _processLogicAgent.addScore : _m_rGameModeRefObj.multiple * _processLogicAgent.addScore);
        }
        
        /// <summary>
        /// 兑换券变更
        /// </summary>
        /// <param name="_objects"></param>
        private void _onActivityCurrencyChg(params object[] _objects)
        {
            if (_objects == null || _objects.Length == 0 || !(_objects[0] is long _activityCurrencyId))
                return;

            NPCommonItem exchangeTokens = HotfixRefdataCoreMgr.instance.tileMatchOtherRefObj.tilematch_currency_item;
            if(exchangeTokens == null || _activityCurrencyId != exchangeTokens.itemId)
                return;
            
            //刷新兑换券
            _refreshActivityExchangeTokensItem();
        }

        /// <summary>
        /// 当同一个序列号的三消逻辑处理开始时
        /// </summary>
        private void _onSameSerializeTileMatchLogicProcessStartDeal(params object[] _objs)
        {
            if(_objs == null || _objs.Length <= 0 || !(_objs[0] is List<TileMatchGameLogic._ATileMatchProcessLogicAgent> _processLogicAgentList))
                return;

            int addScore = 0;
            TileMatchGameLogic._ATileMatchProcessLogicAgent agent = null;
            for (int i = 0, count = _processLogicAgentList.Count; i < count; i++)
            {
                agent = _processLogicAgentList[i];
                if (agent != null)
                    addScore += agent.addScore;
            }

            addScore *= (_m_rGameModeRefObj?.multiple ?? 0);
            addActivityTotalScore(addScore);
        }
        
        #endregion

        /// <summary>
        /// 当奖励预览按钮被点击
        /// </summary>
        /// <param name="_go"></param>
        private void _onBtnRewardPreviewClick(GameObject _go)
        {
            if(_m_rCurStepRewardRefObj == null)
                return;
            
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndTileMatchStepRewardProbability.instance, () =>
            {
                GGUIWndTileMatchStepRewardProbability.instance.showWnd();
                GGUIWndTileMatchStepRewardProbability.instance.setData(_m_rCurStepRewardRefObj.jackpot_group_id);
            }, HotfixUINodeTagConst.TILEMATCH_STAGE_REWARD_PREVIEW);
        }
        
        /// <summary>
        /// 领取奖励按钮被点击
        /// </summary>
        /// <param name="_go"></param>
        private void _onDrawRewardBtnClick()
        {
            // 没有可领取的奖励, 直接返回
            if (HotfixNPPlayer.instance.tileMatchComponent.canDrawStepRewardCount <= 0)
            {
                long scoreGap = (_m_rCurStepRewardRefObj?.goal ?? 0) - _m_lCurStageScore;
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(HotfixTransKeyConst.tilematch_stepRewardCannotDrawTip_none, scoreGap));
                return;
            }
            
            HotfixNPPlayer.instance.tileMatchComponent.reqTileMatchDrawStepReward(null);
        }
        
        #region 动画

        /// <summary>
        /// 播放动画
        /// </summary>
        private void _playAnimation(string _aniName, Action _playDone)
        {
            if (hotfixWnd == null || hotfixWnd.ani == null || string.IsNullOrEmpty(_aniName))
            {
                _playDone?.Invoke();
                return;
            }

            hotfixWnd.ani.Play(_aniName, _playDone);
        }

        private void _sample(string _aniName, float _normalizedTime)
        {
            if (hotfixWnd == null || hotfixWnd.ani == null || string.IsNullOrEmpty(_aniName))
            {
                return;
            }
            
            hotfixWnd.ani.Sample(_aniName, _normalizedTime);
        }

        #endregion
    }
}