using System;
using System.Collections.Generic;
using ALPackage;
using Common.DinnerEnum;
using Common.DinnerObj;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 宴会详情界面
    /// </summary>
    public class GGUIWndDinnerMain : _ATALBasicUIWnd<GGUIMonoDinnerMain>
    {
    
        private static GGUIWndDinnerMain _g_instance = new GGUIWndDinnerMain();

        public static GGUIWndDinnerMain instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndDinnerMain();
                return _g_instance;
            }
        }
        private GDinnerInfo _m_dinnerInfo;//宴会信息
        private int _m_timeDownSer;
        private int _m_typeActioinSer;
    
        private NPGGUIWndPlayerIcon _m_playerInfo; //开宴玩家信息
        private NPGGUIWndCommonToggleEx _m_toggleQuickJoin;
        private GGUIWndConsortIconItem _m_consortCardItem;
        private GGUISubWndChildInfo _m_childCardItem;

        private GGUIWndDinnerMainJoinerItemContainer _m_joinerItemContainer; //参加者item容器

        private NPGGUIWndCommonItem _m_costItem;// 消耗

        private GGUIWndDinnerVideo _m_videoWnd1;
        private GGUIWndDinnerVideo _m_videoWnd2;
        private int _m_curPlayVideoWnd = 0;

        private long _m_dinnerInfoSerializeOp;

        public GGUIWndDinnerMain() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get => GGUIMonoDinnerMain.assetPath; }
        protected override string _monoObjName { get => GGUIMonoDinnerMain.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
        public override bool needDiscardOnSwitch => true;

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_CUR_LOOK_DINNER_CHG, _onCurLookDinnerChg);
            WinMsg.RegisterMsg(WinMsgType.ON_MY_DINNER_ADD, onMyDinnerAdd);
        }

        protected override void _onHideWnd()
        {
            _m_timeDownSer = ALSerializeOpMgr.next();
            _m_dinnerInfoSerializeOp = ALSerializeOpMgr.next();

            _m_playerInfo?.hideWnd();

            _m_videoWnd1?.hideWnd();
        
            _m_videoWnd2?.hideWnd();
            _m_curPlayVideoWnd = 0;
            
            WinMsg.UnregisterMsg(WinMsgType.ON_CUR_LOOK_DINNER_CHG, _onCurLookDinnerChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_MY_DINNER_ADD, onMyDinnerAdd);
        }

        protected override void _onReset()
        {
        
        }

        protected override void _onDiscard()
        {
            _m_toggleQuickJoin?.discard();
            _m_toggleQuickJoin = null;
            
            _m_playerInfo?.discard();
            _m_playerInfo = null;
        
            _m_consortCardItem?.discard();
            _m_consortCardItem = null;

            _m_childCardItem?.discard();
            _m_childCardItem = null;
            
            _m_joinerItemContainer?.discard();
            _m_joinerItemContainer = null;
            
            if(_m_videoWnd1 != null)
            {
                _m_videoWnd1.discard();
                _m_videoWnd1 = null;
            }
            if (_m_videoWnd2 != null)
            {
                _m_videoWnd2.discard();
                _m_videoWnd2 = null;
            }

            _m_curPlayVideoWnd = 0;
            
            if(_m_costItem != null)
            {
                _m_costItem.discard();
                _m_costItem = null;
            }
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _clickBtnClose);
            ALUGUICommon.uncombineBtnClick(wnd.btnInvite, _onClickInvite);
            ALUGUICommon.uncombineBtnClick(wnd.btnJoin, _onClickJoin);
            ALUGUICommon.uncombineBtnClick(wnd.btnLast, _onClickPre);
            ALUGUICommon.uncombineBtnClick(wnd.btnNext, _onClickNext);
            ALUGUICommon.uncombineBtnClick(wnd.btnLog, _onClickLog);
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;
            
            if (null != wnd.toggleQuickJoin)
            {
                _m_toggleQuickJoin = new NPGGUIWndCommonToggleEx(wnd.toggleQuickJoin);
                _m_toggleQuickJoin.clickDelegate += _onClickQuickJoin;
                _m_toggleQuickJoin.setSelected(false);
            }
            if (wnd.playerInfo !=null) _m_playerInfo = new NPGGUIWndPlayerIcon(wnd.playerInfo);
        
            if (wnd.consortIconItem != null)
                _m_consortCardItem = new GGUIWndConsortIconItem(wnd.consortIconItem);

            if (wnd.childIconItem != null)
                _m_childCardItem = new GGUISubWndChildInfo(wnd.childIconItem);
            
            if(wnd.joinerItemContainer != null)
                _m_joinerItemContainer = new GGUIWndDinnerMainJoinerItemContainer(wnd.joinerItemContainer);
            
            if(wnd.curQuickJoinCostItem != null)
                _m_costItem = new NPGGUIWndCommonItem(wnd.curQuickJoinCostItem);

            if (wnd.videoMono1 != null)
                _m_videoWnd1 = new GGUIWndDinnerVideo(wnd.videoMono1);
            
            if (wnd.videoMono2 != null)
                _m_videoWnd2 = new GGUIWndDinnerVideo(wnd.videoMono2);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _clickBtnClose);
            ALUGUICommon.combineBtnClick(wnd.btnInvite, _onClickInvite);
            ALUGUICommon.combineBtnClick(wnd.btnJoin, _onClickJoin);
            ALUGUICommon.combineBtnClick(wnd.btnLast, _onClickPre);
            ALUGUICommon.combineBtnClick(wnd.btnNext, _onClickNext);
            ALUGUICommon.combineBtnClick(wnd.btnLog, _onClickLog);
        }
    
        
        /// <summary>
        /// 当前进入的宴会发生变化的时候
        /// </summary>
        /// <param name="_objs"></param>
        private void _onCurLookDinnerChg(object[] _objs)
        {
            if(_objs.Length < 1)
                return;
            long dinnerId = (long) _objs[0];
            if (dinnerId == _m_dinnerInfo.dinnerId)
            {
                _refreshDynamicInfo();
            }
        }
        
        /// <summary>
        /// 关闭界面
        /// </summary>
        /// <param name="obj"></param>
        private void _clickBtnClose(GameObject obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_DINNER_MAIN);
        }

    
        /// <summary>
        /// 邀请
        /// </summary>
        /// <param name="obj"></param>
        private void _onClickInvite(GameObject obj)
        {
            GGUIWndDinnerInviteMain.instance.setInfo(_m_dinnerInfo);
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndDinnerInviteMain.instance, GGUIWndDinnerInviteMain.instance.showWnd, EUIQueueStageType.MAIN, UINodeTagConst.C_DINNER_INVITE, false, false);
        }
    
        /// <summary>
        /// 加入宴会
        /// </summary>
        /// <param name="obj"></param>
        private void _onClickJoin(GameObject obj)
        {
            if (AccountSettingMgr.instance.dinnerSaver.isQuickJoin())
            {
                long costId = AccountSettingMgr.instance.dinnerSaver.getQuickJointCostId();
                NPPlayer.instance.dinnerComp.reqJoinDinner(_m_dinnerInfo.instanceId, costId, (_info) =>
                {
                    joinRefreshWnd(costId, _info.getGainItemList());
                }, () =>
                {
                    AccountSettingMgr.instance.dinnerSaver.setQuickJointCostId(0);
                });
            }
            else
            {
                GGUIWndDinnerCostList.instance.setInfo((costRef) =>
                {
                    if(costRef == null)
                        return;
                
                    NPPlayer.instance.dinnerComp.reqJoinDinner(_m_dinnerInfo.instanceId, costRef.id, (_info) =>
                    {
                        joinRefreshWnd(costRef.id, _info.getGainItemList());
                    } ,() =>
                    {
                        AccountSettingMgr.instance.dinnerSaver.setQuickJointCostId(0);
                    });
                });
                QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndDinnerCostList.instance, GGUIWndDinnerCostList.instance.showWnd, EUIQueueStageType.MAIN, UINodeTagConst.C_DINNER_COST_USE, false, false);
            }
        }
        /// <summary>
        /// 加入宴会
        /// </summary>
        /// <param name="obj"></param>
        private void _onClickQuickJoin(NPGGUIWndCommonToggleEx obj)
        {
            if (null == _m_toggleQuickJoin)
                return;
            if (!_m_toggleQuickJoin.isOn)
            {
                GGUIWndDinnerCostList.instance.setQuickJoin((cost) =>
                {        
                    if(cost == null)
                        return;
                    AccountSettingMgr.instance.dinnerSaver.setQuickJointCostId(cost.id);
                    _refreshQuickJoinShow();
                });
                QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndDinnerCostList.instance, GGUIWndDinnerCostList.instance.showWnd, EUIQueueStageType.MAIN, UINodeTagConst.C_DINNER_COST_USE, false, false);
            }
            else
            {
                AccountSettingMgr.instance.dinnerSaver.setQuickJointCostId(0);
                _refreshQuickJoinShow();
            }
        }
        /// <summary>
        /// 上一个宴会
        /// </summary>
        /// <param name="obj"></param>
        private void _onClickPre(GameObject obj)
        {
            NPPlayer.instance.dinnerComp.reqGetPreDinnerInfo(_m_dinnerInfo.instanceId, _m_dinnerInfo.listIdx, (_info) =>
            {
                if (_info == null)
                    return;
                _m_dinnerInfo = _info;
                _refreshWnd();
            });
        }
    
        /// <summary>
        /// 下一个宴会
        /// </summary>
        /// <param name="obj"></param>
        private void _onClickNext(GameObject obj)
        {
            NPPlayer.instance.dinnerComp.reqGetNextDinnerInfo(_m_dinnerInfo.instanceId, _m_dinnerInfo.listIdx, (_info) =>
            {
                if (_info == null)
                    return;
                _m_dinnerInfo = _info;
                _m_dinnerInfo.regDetailInfo((_dinnerInfo) =>
                {
                    _m_dinnerInfo = _dinnerInfo;
                    _refreshWnd();
                }, () =>
                {
                    NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.dinner_is_over_tip));
                    QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_DINNER_MAIN);
                });
            
            });
        }

        private void _onClickLog(GameObject _)
        {
            GGUIWndDinnerDetailLog.instance.setInfo(_m_dinnerInfo);
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndDinnerDetailLog.instance, GGUIWndDinnerDetailLog.instance.showWnd, EUIQueueStageType.MAIN, UINodeTagConst.C_DINNER_DETAIL_LOG, false, false);
        }

        /// <summary>
        /// 设置显示信息
        /// </summary>
        /// <param name="_dinnerInfo"></param>
        public void setInfo(GDinnerInfo _dinnerInfo, Action _onLoadDone)
        {
            _m_dinnerInfo = _dinnerInfo;
            _m_dinnerInfoSerializeOp = ALSerializeOpMgr.next();
            _refreshWnd(_onLoadDone);
        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd(Action _onLoadDone = null)
        {
            if(null == wnd)
                return;
            _refreshStaticInfo();
            _refreshDynamicInfo(_onLoadDone);
        }

        /// <summary>
        /// 刷新这个宴会的不会变化的信息
        /// </summary>
        private void _refreshStaticInfo()
        {
            if (_m_dinnerInfo == null || _m_dinnerInfo.dinnerTypeRef == null)
                return;
            
            if (null != _m_playerInfo)
            {
                _m_playerInfo.showWnd();
                _m_playerInfo.setPlayer(_m_dinnerInfo.ownerCid);
            }
            ALUGUICommon.setLabelTxt(wnd.txtDinnerName, TextTranslate.instance.getLanguage(_m_dinnerInfo.dinnerTypeRef.name_spe));
                
                
            if (_m_dinnerInfo.isConsortDinner())
            {
                if (wnd.showTypeAnim != null) wnd.showTypeAnim.Play(wnd.consortAnimName);
                _m_consortCardItem?.showWnd();
                _m_consortCardItem?.setInfo(new ConsortInfo(_m_dinnerInfo.getPermitConsortId()), 0);
                _m_childCardItem?.hideWnd();
            }
            else if (_m_dinnerInfo.isGiftdeChildCeleDinner())
            {
                if (wnd.showTypeAnim != null && !string.IsNullOrEmpty(wnd.childCeleAnimName)) wnd.showTypeAnim.Play(wnd.childCeleAnimName);
                _m_consortCardItem?.hideWnd();
                _m_childCardItem?.hideWnd();
                long serializeOp = _m_dinnerInfoSerializeOp;
                NPPlayer.instance.childComp.getAdultChildInfoById(_m_dinnerInfo.getPermitChildId(), (_childInfo) =>
                {
                    if (serializeOp != _m_dinnerInfoSerializeOp)
                        return;
                    if (_childInfo == null)
                    {
                        _m_childCardItem?.hideWnd();
                        return;
                    }
                    _m_childCardItem?.showWnd();
                    _m_childCardItem?.refreshWnd(_childInfo);
                });
            }
            else
            {
                if (wnd.showTypeAnim != null) wnd.showTypeAnim.Play(wnd.defaultAnimName);
                _m_consortCardItem?.hideWnd();
                _m_childCardItem?.hideWnd();
            }
    
            ALUGUICommon.setGameObjEnable(wnd.consortDinnerShowGos, _m_dinnerInfo.isConsortDinner());
            ALUGUICommon.setGameObjEnable(wnd.childCeleDinnerShowGos, _m_dinnerInfo.isGiftdeChildCeleDinner());
            ALUGUICommon.setUIObjColor(wnd.txtColorGraphics, _m_dinnerInfo.dinnerTypeRef.txt_color);
            ALUGUICommon.setUIObjColor(wnd.bgColorGraphics, _m_dinnerInfo.dinnerTypeRef.bg_color);
                
            if (_m_joinerItemContainer != null)
            {
                _m_joinerItemContainer.showWnd();
                _m_joinerItemContainer.showItemList(_m_dinnerInfo.joinerInfoList, _m_dinnerInfo.dinnerTypeRef.default_seat_num);
            }
            _m_timeDownSer = ALSerializeOpMgr.next();
            _refreshTimeDown(_m_timeDownSer);
            _m_typeActioinSer = ALSerializeOpMgr.next();
            int typingActionSer = _m_typeActioinSer;
            _showRandomTalk(typingActionSer);
        }
        /// <summary>
        /// 刷新会变更的信息
        /// </summary>
        private void _refreshDynamicInfo(Action _onLoadDone = null)
        {
            if (null == wnd || null == _m_dinnerInfo)
            {
                _onLoadDone?.Invoke();
                return;
            }
            _refreshQuickJoinShow();
            ALUGUICommon.setGameObjEnable(wnd.btnLast, _m_dinnerInfo.hasPre);
            ALUGUICommon.setGameObjEnable(wnd.btnNext, _m_dinnerInfo.hasNext);
            ALUGUICommon.setLabelTxt(wnd.txtDinnerScore, TextTranslate.instance.getLanguage(TransKeyConst.dinner_main_score, _m_dinnerInfo.score));
            int joinerCount = _m_dinnerInfo.joinerCount;
            int maxJoinerCount = _m_dinnerInfo.dinnerTypeRef.default_seat_num;
            ALUGUICommon.setLabelTxt(wnd.txtDinnerSeat,TextTranslate.instance.getLanguage(TransKeyConst.dinner_seatNum_num_num, joinerCount, maxJoinerCount));
            
            // 显示宴会最新log
            DinnerDetailLogInfo lastLog = _m_dinnerInfo.getLastDetailLog();
            if (lastLog != null)
                GCommon.reqPlayerInfo(lastLog.joinerId, _info =>
                {
                    if (wnd == null)
                        return;

                    ALUGUICommon.setLabelTxt(wnd.txtLogMini, lastLog?.makeLogContent(_info?.name));
                });
                
            EDinnerPlayerStat stat = _m_dinnerInfo.getPlayerStat();
            NPCommonEnumStatInfo<EDinnerPlayerStat>.setStat(wnd.statInfos, stat);

            if (_m_videoWnd1 != null && _m_videoWnd2 != null)
            {
                GVideoClipIndex videoClip = _m_dinnerInfo.getDinnerVideoInfo(_m_dinnerInfo.joinerCount);

                // 如果当前宴会有视频，则需要播放hide和show动画
                if(_m_curPlayVideoWnd == 1)
                {
                    if (_m_videoWnd1.videoClip != null && _m_videoWnd1.videoClip.Equals(videoClip))
                    {
                        _onLoadDone?.Invoke();
                    }
                    else
                    {
                        _m_videoWnd1.hideWnd();
                        _m_videoWnd2.showWndVideo(videoClip, _onLoadDone);
                        _m_curPlayVideoWnd = 2;
                    }
                }
                else if (_m_curPlayVideoWnd == 2)
                {
                    if (_m_videoWnd2.videoClip != null && _m_videoWnd2.videoClip.Equals(videoClip))
                    {
                        _onLoadDone?.Invoke();
                    }
                    else
                    {
                        _m_videoWnd2.hideWnd();
                        _m_videoWnd1.showWndVideo(videoClip, _onLoadDone);
                        _m_curPlayVideoWnd = 1;
                    }
                }
                // 如果没有播放视频，则直接显示视频，不播动画
                else
                {
                   
                    _m_curPlayVideoWnd = 1;
                    if (_m_videoWnd1 != null)
                    {
                        _m_videoWnd1.showWndVideo(videoClip, _onLoadDone);
                    }
                    else
                    {
                        _onLoadDone?.Invoke();
                    }
                }
            }

            bool isSeatFull = joinerCount >= maxJoinerCount;
            ALUGUICommon.setGameObjEnable(wnd.goHideInSeatFullList, !isSeatFull);
            ALUGUICommon.setGameObjEnable(wnd.goShowInSeatFullList, isSeatFull);

        }

        private void _refreshQuickJoinShow()
        {
            if (_m_toggleQuickJoin != null)
                _m_toggleQuickJoin.setSelected(AccountSettingMgr.instance.dinnerSaver.isQuickJoin());
            if (_m_costItem != null)
            {
                long costId = AccountSettingMgr.instance.dinnerSaver.getQuickJointCostId();
                if (costId <= 0)
                {
                    _m_costItem.hideWnd();
                }
                else
                {
                    GDinnerJoinCostRefObj costInfo = GRefdataCoreMgr.instance.dinnerJoinCostRefCore.getRef(costId);
                    if (costInfo != null)
                    {
                        _m_costItem.setItem(costInfo.cost_item);
                        _m_costItem.showWnd();
                    }
                    else
                    {
                        _m_costItem.hideWnd();
                    }
                }
           
            }
        }

        /// <summary>
        /// 刷新倒计时
        /// </summary>
        private void _refreshTimeDown(int _timeDownSer)
        {
            if (null == wnd)
                return;
            if (null == _m_dinnerInfo)
                return;
            long leftTimeMs = _m_dinnerInfo.getRemainTimeMs();
        
            long hour = leftTimeMs / (1000 * 60 * 60);
            long minute = (leftTimeMs / (1000 * 60)) % 60;
            long second = (leftTimeMs / 1000) % 60;
            string cdTxt =  TextTranslate.instance.getLanguage(TransKeyConst.dinner_detail_timestamp_h_m_s, hour, minute, second);

            if (wnd.txtDinnerTimeDown != null)
                foreach (Text txtTime in wnd.txtDinnerTimeDown)
                {
                    ALUGUICommon.setLabelTxt(txtTime, cdTxt);
                }

            if (_timeDownSer != _m_timeDownSer)
                return;
            ALCommonTaskController.CommonActionAddMonoTask(() =>
            {
                _refreshTimeDown(_timeDownSer);
            },1f);
        }

        private void _showJoinDinner(GDinnerJoinerInfo _joiner, Action _onComplete)
        {
            _m_joinerItemContainer?.addItem(_joiner);
            _showWelcome(_joiner, _onComplete);
        }

        private void _showWelcome(GDinnerJoinerInfo _joiner, Action _onComplete = null)
        {
            if (_joiner != null)
                _joiner.regDetailInfo(_info =>
                {
                    if (wnd == null)
                        return;

                    if (_m_dinnerInfo != null)
                        ALUGUICommon.setLabelTxt(wnd.txtWelcome,
                            TextTranslate.instance.getLanguage(_m_dinnerInfo.dinnerTypeRef?.welcome_dialogue_keys?.GetRandomItem(), _joiner?.name));
                    // 更改序列表，结束随机氛围对话回调
                    _m_typeActioinSer = ALSerializeOpMgr.next();
                    int typingActionSer = _m_typeActioinSer;

                    if (wnd.welcomeAnim != null)
                        wnd.welcomeAnim.Play(wnd.welcomeShowAnimName, () =>
                        {
                            _onComplete?.Invoke();
                            _showRandomTalk(typingActionSer);
                        });
                });
            else
            {
                _onComplete?.Invoke();
            }
        }

        private void _showRandomTalk(long _typingActionSer)
        {
            if(_typingActionSer != _m_typeActioinSer)
                return;
            if (_m_dinnerInfo != null)
                _m_joinerItemContainer?.showRandomTalk(TextTranslate.instance.getLanguage(_m_dinnerInfo.dinnerTypeRef.common_dialogue_keys.GetRandomItem()));

            if (wnd != null)
                ALCommonTaskController.CommonActionAddMonoTask(() =>
                    {
                        _showRandomTalk(_typingActionSer);
                    }, wnd.randomTalkShowTimeRange.getRandomValue());
        }

        /// <summary>
        /// 当我的宴会有玩家加入
        /// </summary>
        private void onMyDinnerAdd(object[] _objs)
        {
            if(_objs == null || _objs.Length < 1 || _objs[0] is not long joinerCid)
                return;
            
            if (_m_dinnerInfo.getPlayerStat() == EDinnerPlayerStat.OWNER)
            {
                _m_dinnerInfoSerializeOp = ALSerializeOpMgr.next();
                long serializeOp = _m_dinnerInfoSerializeOp;
                _m_dinnerInfo.regDetailInfo((_dinnerInfo) =>
                {
                    if(serializeOp != _m_dinnerInfoSerializeOp)
                        return;
                    _refreshDynamicInfo();
                    GDinnerJoinerInfo joinerInfo = _m_dinnerInfo.getJoinerInfo(joinerCid);
                    _showJoinDinner(joinerInfo, () =>
                    {
                        if(serializeOp != _m_dinnerInfoSerializeOp)
                            return;
                        if (_m_dinnerInfo != null && _m_dinnerInfo.getPlayerStat() == EDinnerPlayerStat.OWNER && _m_dinnerInfo.dinnerTypeRef != null && _m_dinnerInfo.joinerCount >= _m_dinnerInfo.dinnerTypeRef.default_seat_num)
                        {
                            if (!NPPlayer.instance.dinnerComp.hasOwenrReward)
                                return;
                            NPPlayer.instance.dinnerComp.reqTakeOpenReward((_info) =>
                            {
                                NPUINoticeMgr.instance.addDealer(new NPNoticeDealer_DinnerCreateResult(_info.getResult()));
                            });
                        }
                    });
                }, () =>
                {
                    NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.dinner_is_over_tip));
                    QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_DINNER_MAIN);
                },true);
            }
        }

        private void joinRefreshWnd(long _costId, List<NPCommon.NPCommon_ItemInfo> _gainItems)
        {
            GDinnerJoinCostRefObj costRef = GRefdataCoreMgr.instance.dinnerJoinCostRefCore.getRef(_costId);
        
            _m_dinnerInfo.regDetailInfo((_dinnerInfo) =>
            {
                GDinnerJoinerInfo joinerInfo = _dinnerInfo.getJoinerInfo(NPPlayer.instance.playerInfo.CID);
                NPNoticeDealer_DinnerJoinResult result = new NPNoticeDealer_DinnerJoinResult(_gainItems, joinerInfo.score, _m_dinnerInfo, null,
                    () =>
                    {
                        _refreshDynamicInfo();
                        _showJoinDinner(joinerInfo, null);
                    });
                NPUINoticeMgr.instance.addDealer(result);
            }, () =>
            {
                GDinnerJoinerInfo joinerInfo = new GDinnerJoinerInfo(EDinnerJoinerType.PLAYER, NPPlayer.instance.playerInfo.CID, costRef.join_gain_score, FpsAndPingMgr.instance.serverTimeTag, _costId);
                NPNoticeDealer_DinnerJoinResult result = new NPNoticeDealer_DinnerJoinResult(_gainItems, joinerInfo.score, _m_dinnerInfo, null, () =>
                {
                    // 如果已经结束，则播放一下参加表现，然后提示关闭宴会
                    _showJoinDinner(joinerInfo, () =>
                    {
                        NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.dinner_is_over_tip));
                        QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_DINNER_MAIN);
                    });
                });
                NPUINoticeMgr.instance.addDealer(result);
              
            },true);
        }
    }
}