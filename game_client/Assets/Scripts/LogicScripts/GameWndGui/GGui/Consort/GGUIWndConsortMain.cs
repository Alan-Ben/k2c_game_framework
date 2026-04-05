using System.Collections.Generic;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 家人系统主页面
    /// </summary>
    public class GGUIWndConsortMain : _ATALBasicLoadPrefabSubUIWnd<GGUIMonoConsortMain>
    {
        private GGUIWndConsortListGrid _m_wConsortListGrid;//妃子列表
        private NPGGUIWndCommonToggleEx _m_wAkeyInviteToggle;//一键邀请Toggle
        private GGUIWndCommonLazyCDCountResume _m_wStrengthLazyCdLazyCD;//体力CD恢复
        private GGUISubWndConsortInviteBuff _m_wBuffWnd;//邀约buff窗口
        
        private List<GGottenConsortInfo> _m_lUnlockConsortInfoList = new List<GGottenConsortInfo>();//已解锁的妃子卡牌列表
        private List<ConsortRefShowInfo> _m_lLockConsortInfoList = new List<ConsortRefShowInfo>();//未解锁的妃子卡牌列表

        private bool _m_bIsInviting;//是否在邀约中
        private long _m_lShowSerialize;

        /// <summary>
        /// 是否正在邀约中
        /// </summary>
        public bool isInviting { get { return _m_bIsInviting; } }
        
        public GGUIWndConsortMain(Transform _parent) : base(_parent)
        {
        }

        ///妃子列表上次列表滚动位置
        public float consortGridPreVerticalNormalizedPosition { get; set; }
        
        protected override string _monoAssetPath { get { return GGUIMonoConsortMain.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoConsortMain.objName; } }

        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.consortList != null)
            {
                _m_wConsortListGrid = new GGUIWndConsortListGrid(wnd.consortList);
                _m_wConsortListGrid.clickDelegate += _onOnConsortItemClick;
                _m_wConsortListGrid.onVerticalNormalizedPositionChg += _onConsortGridVerticalNormalizedPositionChg;
            }

            if (wnd.monoBuff != null)
                _m_wBuffWnd = new GGUISubWndConsortInviteBuff(wnd.monoBuff);
            
            if (wnd.AkeyInviteToggle != null)
            {
                _m_wAkeyInviteToggle = new NPGGUIWndCommonToggleEx(wnd.AkeyInviteToggle);
                _m_wAkeyInviteToggle.clickDelegate += _onClickAkeyInviteToggle;
            }

            if (wnd.monoStrengthLazyCd != null)
                _m_wStrengthLazyCdLazyCD = new GGUIWndCommonLazyCDCountResume(wnd.monoStrengthLazyCd);
         
            ALUGUICommon.combineBtnClick(wnd.btnInvite, _onClickInviteBtn);
            ALUGUICommon.combineBtnClick(wnd.btnAKeyInvite, _onClickAkeyInviteBtn);
            ALUGUICommon.combineBtnClick(wnd.btnReturn, _onReturnBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnCg, _onCgBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnChgEntranceConsort, _onChgEntranceConsortBtnClick);
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnInvite, _onClickInviteBtn);
                ALUGUICommon.uncombineBtnClick(wnd.btnAKeyInvite, _onClickAkeyInviteBtn);   
                ALUGUICommon.uncombineBtnClick(wnd.btnReturn, _onReturnBtnClick);
                ALUGUICommon.uncombineBtnClick(wnd.btnCg, _onCgBtnClick);
                ALUGUICommon.uncombineBtnClick(wnd.btnChgEntranceConsort, _onChgEntranceConsortBtnClick);
            }

            if (_m_wConsortListGrid != null)
            {
                _m_wConsortListGrid.clickDelegate -= _onOnConsortItemClick;
                _m_wConsortListGrid.onVerticalNormalizedPositionChg -= _onConsortGridVerticalNormalizedPositionChg;
                _m_wConsortListGrid.discard();
                _m_wConsortListGrid = null;                
            }

            _m_wBuffWnd?.discard();
            _m_wBuffWnd = null;
            
            if (_m_wAkeyInviteToggle != null)
            {
                _m_wAkeyInviteToggle.clickDelegate -= _onClickAkeyInviteToggle;
                _m_wAkeyInviteToggle.discard();
            }
            _m_wAkeyInviteToggle = null;
            
            _m_wStrengthLazyCdLazyCD?.discard();
            _m_wStrengthLazyCdLazyCD = null;

            _m_lUnlockConsortInfoList?.Clear();
            _m_lUnlockConsortInfoList = null;
            _m_lLockConsortInfoList?.Clear();
            _m_lLockConsortInfoList = null;
        }
        
        protected override void _onShowWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();

            WinMsg.RegisterMsg(WinMsgType.SIMULATE_OPEN_CONSORT_INFO_BY_INDEX, _simulateOpenConsortInfoByIndex);//模拟点击打开情人信息界面，item下标从0开始
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_CONSORT_RANDOM_GREET, _simulateClickConsortRandomGreet);//模拟点击情人随机邀约
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_CONSORT_ONE_KEY_INVITE_TOGGLE, _simulateClickOneKeyInviteToggle);//模拟点击情人一键邀约开关
            WinMsg.RegisterMsg(WinMsgType.CONSORT_MAIN_SCROLL_MOVE_TO_CONSORT, _onScrollMoveToConsort);//妃子主界面滚动到可升级加护技能的妃子
            _m_wBuffWnd?.showWnd();
            _refreshWnd(false);
        }

        protected override void _onHideWnd()
        {
            _m_bIsInviting = false;
            _m_lShowSerialize = ALSerializeOpMgr.next();
            
            WinMsg.UnregisterMsg(WinMsgType.SIMULATE_OPEN_CONSORT_INFO_BY_INDEX, _simulateOpenConsortInfoByIndex);//模拟点击打开情人信息界面，item下标从0开始
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_CONSORT_RANDOM_GREET, _simulateClickConsortRandomGreet);//模拟点击情人随机邀约
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_CONSORT_ONE_KEY_INVITE_TOGGLE, _simulateClickOneKeyInviteToggle);//模拟点击情人一键邀约开关
            WinMsg.UnregisterMsg(WinMsgType.CONSORT_MAIN_SCROLL_MOVE_TO_CONSORT, _onScrollMoveToConsort);//妃子主界面滚动到可升级加护技能的妃子

            _m_wConsortListGrid?.hideWnd();
            _m_wBuffWnd?.hideWnd();
            _m_wAkeyInviteToggle?.hideWnd();
            _m_wStrengthLazyCdLazyCD?.hideWnd();
            
            _m_lUnlockConsortInfoList?.Clear();
            _m_lLockConsortInfoList?.Clear();
        }

        protected override void _onReset()
        {
            _m_wConsortListGrid?.resetWnd();
            _m_wBuffWnd?.resetWnd();
            _m_wAkeyInviteToggle?.resetWnd();
            _m_wStrengthLazyCdLazyCD?.resetWnd();
        }

        private void _refreshWnd(bool _needMoveScrollRect = false)
        {
            _refreshTotalConsortInfo(_needMoveScrollRect);
            _refreshAkeyInvite();
            _refreshStrengthLazyCdLazyCD();
        }
        
        /// <summary>
        /// 刷新所有妃子信息
        /// </summary>
        private void _refreshTotalConsortInfo(bool _needMoveScrollRect = false)
        {
            if(wnd == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtTotalIntimacy, NPPlayer.instance.consortComp.allIntimacyNum);
            ALUGUICommon.setLabelTxt(wnd.txtTotalCharm, NPPlayer.instance.consortComp.allCharmNum);
            ALUGUICommon.setLabelTxt(wnd.totalConsortNum, TextTranslate.instance.getLanguage(TransKeyConst.hero_ownNum_num_num, 
                NPPlayer.instance.consortComp.getConsortCount(),
                GRefdataCoreMgr.instance.consortRefCore.refList.Count));
            
            _m_lUnlockConsortInfoList = ConsortUtil.getUnlockConsortList(false, null);
            _m_lLockConsortInfoList = ConsortUtil.getLockConsortList(null);
            
            if (_m_wConsortListGrid != null)
            {
                _m_wConsortListGrid.showWnd();
                _m_wConsortListGrid.showConsortList(_m_lUnlockConsortInfoList, _m_lLockConsortInfoList, consortGridPreVerticalNormalizedPosition, _needMoveScrollRect);
            }
        }

        private void _refreshStrengthLazyCdLazyCD()
        {
            if (_m_wStrengthLazyCdLazyCD != null)
            {
                _m_wStrengthLazyCdLazyCD.showWnd();
                _m_wStrengthLazyCdLazyCD.setInfo(GRefdataCoreMgr.instance.npGeneral.consort_rand_call_cd);
            }
        }
        
        /// <summary>
        /// 刷新一键邀约
        /// </summary>
        private void _refreshAkeyInvite()
        {
            EGameCommonUnlockType AKeyUnlock = GCommon.isSimpleUnlock(GRefdataCoreMgr.instance.npGeneral.consort_a_key_call_simple_unlock_id)
                ?EGameCommonUnlockType.UNLOCK:EGameCommonUnlockType.LOCK;//判断一键邀约功能是否解锁
            if(wnd  != null)
                NPCommonEnumStatInfo<EGameCommonUnlockType>.setStat(wnd.statAKeyInvite, AKeyUnlock);
            
            if (_m_wAkeyInviteToggle != null)
            {
                _m_wAkeyInviteToggle.showWnd();
                _m_wAkeyInviteToggle.setSelected(AccountSettingMgr.instance.accountSetting.isAkeyGreeting, true);
            }
        }

        //打开家人详情窗口
        private void _openConsortWnd(_IConsortShowInfo _info)
        {
            if (_info == null)
                return;

            if (_m_lUnlockConsortInfoList != null)
            {
                for (int i = 0; i < _m_lUnlockConsortInfoList.Count; i++)
                {
                    GGottenConsortInfo getConsortInfo = _m_lUnlockConsortInfoList[i];
                    if (getConsortInfo != null && getConsortInfo.consortId == _info.consortId)
                    {

                        GNodeUnLockConsortDetail.addConsortNode(new List<GGottenConsortInfo>(_m_lUnlockConsortInfoList), i);
                        return;
                    }
                }
            }

            if (_m_lLockConsortInfoList != null)
            {
                for (int i = 0; i < _m_lLockConsortInfoList.Count; i++)
                {
                    ConsortRefShowInfo lockConsortInfo = _m_lLockConsortInfoList[i];
                    if (lockConsortInfo != null && lockConsortInfo.consortId == _info.consortId)
                    {
                        QueueMgr.instance.AddNode(new GNodeLockConsortDetail(_m_lLockConsortInfoList.ConvertAll((_consortShoInfo) => (_IConsortShowInfo)_consortShoInfo), i));
                        return;
                    }
                }
            }
        }
        
        /// <summary>
        /// 当点击了一键邀约Toggle
        /// </summary>
        /// <param name="_wnd"></param>
        private void _onClickAkeyInviteToggle(NPGGUIWndCommonToggleEx _wnd)
        {
            if (_wnd == null)
                return;

            bool isOn = !_wnd.isOn;
            // 若想要勾选一键邀约，但是一键邀约功能未解锁，进行提示, 并直接返回
            if(isOn && !GCommon.isSimpleUnlock(GRefdataCoreMgr.instance.npGeneral.consort_a_key_call_simple_unlock_id,true))
                return;
            
            AccountSettingMgr.instance.accountSetting.setIsAkeyGreeting(isOn);
            _refreshAkeyInvite();
        }

        /// <summary>
        /// 模拟点击家人打开家人信息界面
        /// </summary>
        /// <param name="_objects"></param>
        private void _simulateOpenConsortInfoByIndex(params object[] _objects)
        {
            if (null == _objects || _objects.Length == 0)
                return;

            long index = (long)_objects[0];
            int targetIndex = (int) index;

            if(_m_lUnlockConsortInfoList != null && _m_lUnlockConsortInfoList.Count > targetIndex)
                _openConsortWnd(_m_lUnlockConsortInfoList[targetIndex]);
            else if (_m_lLockConsortInfoList != null)
            {
                if (_m_lUnlockConsortInfoList != null)
                    targetIndex = targetIndex - _m_lUnlockConsortInfoList.Count;

                if (_m_lLockConsortInfoList.Count > targetIndex)
                    _openConsortWnd(_m_lLockConsortInfoList[targetIndex]);
            }
        }

        //模拟点击妃子随机宠幸
        private void _simulateClickConsortRandomGreet()
        {
            if (_m_wAkeyInviteToggle != null && _m_wAkeyInviteToggle.isOn)
                _onClickAkeyInviteBtn(null);
            else
                _onClickInviteBtn(null);
        }

        //模拟点击情人一键邀约开关
        private void _simulateClickOneKeyInviteToggle()
        {
            _onClickAkeyInviteToggle(_m_wAkeyInviteToggle);
        }

        /// <summary>
        /// 当妃子列表滚动位置发生变化时
        /// </summary>
        /// <param name="_value"></param>
        private void _onConsortGridVerticalNormalizedPositionChg(float _value)
        {
            consortGridPreVerticalNormalizedPosition = _value;
        }
        
        #region 点击事件

        /// <summary>
        /// 点击邀约按钮
        /// </summary>
        /// <param name="_gameObject"></param>
        private void _onClickInviteBtn(GameObject _gameObject)
        {
            if(_m_bIsInviting)
                return;
            
            if(!GCommon.lazycdEnough(GRefdataCoreMgr.instance.npGeneral.consort_rand_call_cd, 1, true))
                return;

            _m_bIsInviting = true;
            
            //是否首次随机宠幸
            bool isFirstRandomCall = NPPlayer.instance.recordComp.getValue(ENPPlayerRecordParam.CONSORT_RND_CALL_TIMES) <= 0;
            
            long serialize = _m_lShowSerialize;
            NPPlayer.instance.consortComp.reqCallRand((_msg) =>
            {
                if (_msg == null || _msg.getRes() == null || wnd == null || serialize != _m_lShowSerialize)
                {
                    _m_bIsInviting = false;
                    return;
                }

                // Grid列表移动
                // https://www.teambition.com/task/68e8bc496ef38a30fbe6a686 改成不用滚动
                // NPUINoticeMgr.instance.addDealer(new NoticeDealer_CustomAction((_onComplete) =>
                // {
                //     if (_m_wConsortListGrid != null && serialize == _m_lShowSerialize)
                //     {
                //         _m_wConsortListGrid.setByInviteConsortId(_msg.getRes().getConsortId(), true, ()=>
                //         {
                //             _onComplete?.Invoke();
                //         });
                //     }
                //     else
                //     {
                //         _onComplete?.Invoke();
                //     }
                // }, NPNoticeType.g_AllTypeArr));

                NPUINoticeMgr.instance.addDealer(new NoticeDealer_ConsortRandCallProcess(() => serialize == _m_lShowSerialize, _msg.getRes(), 0, () =>
                {
                    GGUIWndConsortListGridItem consortItem = null;
                    if (_m_wConsortListGrid != null)
                    {
                        _m_wConsortListGrid.refreshAllItem((_itemWnd, _index) =>
                        {
                            if (_itemWnd != null && _itemWnd.consortShowInfo != null && _itemWnd.consortShowInfo.consortId == _msg.getRes().getConsortId())
                                consortItem = _itemWnd;
                        });
                    }

                    Vector3 consortItemPosition = consortItem?.consortHeadIconCenterWorldPos ?? Vector3.zero;
                    return consortItemPosition;
                }, null,
                    () =>
                    {
                        _m_wConsortListGrid?.setByInviteConsortId(0, false);//设置为0表示没有被邀约
                    }));

                // 若获取到CG, 展示获取CG弹窗
                if (_msg.getRes().getIsGainCg())
                {
                    ConsortStoryRefObj consortStoryRefObj = GRefdataCoreMgr.instance.consortStoryRefCore.getRef(_msg.getRes().getConsortStoryId());
                    ConsortCGRefObj consortCgRefObj = GRefdataCoreMgr.instance.consortCGRefCore.getRef(consortStoryRefObj?.unlock_cg ?? 0);
                    if(consortCgRefObj != null)
                        NPUINoticeMgr.instance.addDealer(new NoticeDealer_ConsortGetCG(() => serialize == _m_lShowSerialize, consortCgRefObj));
                }
                
                // 展示邀约奖励窗口
                NPUINoticeMgr.instance.addDealer(new NoticeDealer_ConsortRandCallResult(() => serialize == _m_lShowSerialize, _msg,
                    () =>
                    {
                        _refreshWnd();//邀约成功后, 可能会有亲密度和体力变化, 所以这里整个页面都刷新一下
                        _m_bIsInviting = false;
                    }));

                // 若有获取到子嗣, 展示获取子嗣弹窗
                ChildInfo getChildInfo = NPPlayer.instance.childComp.getChildInfo(_msg.getRes().getChildId());
                if(getChildInfo != null)
                    NPUINoticeMgr.instance.addDealer(new NoticeDealer_ChildGet(getChildInfo));

                NPUINoticeMgr.instance.addDoneDelegate(() =>
                {
                    WinMsg.SendMsg(WinMsgType.ON_CONSORT_INVITE_SHOW_DONE);
                });
                
            }, null);
        }

        /// <summary>
        /// 点击一键邀约按钮
        /// </summary>
        /// <param name="_gameObject"></param>
        private void _onClickAkeyInviteBtn(GameObject _gameObject)
        {
            if(_m_bIsInviting)
                return;
            
            // 若一键邀约功能未解锁, 直接返回
            if(!GCommon.isSimpleUnlock(GRefdataCoreMgr.instance.npGeneral.consort_a_key_call_simple_unlock_id,true))
                return;
         
            if(!GCommon.lazycdEnough(GRefdataCoreMgr.instance.npGeneral.consort_rand_call_cd, 1, true))
                return;
         
            _m_bIsInviting = true;
            
            NPPlayer.instance.consortComp.reqCallAkey((_msg) =>
            {
                if (_msg == null)
                {
                    _m_bIsInviting = false;
                    return;
                }
                
                // 展示邀约奖励窗口
                NPUINoticeMgr.instance.addDealer(new NoticeDealer_ConsortAKeyCallResult(_msg));
                
                ALStepCounter stepCounter = new ALStepCounter();
                stepCounter.chgTotalStepCount(1);
                stepCounter.regAllDoneDelegate(() =>
                {
                    _m_bIsInviting = false;
                });
                
                List<ChildInfo> getChildInfoList = new List<ChildInfo>();//获取到的子嗣列表
                
                // 若有妃子获取CG, 展示获取CG弹窗
                if (_msg.getResList() != null)
                {
                    foreach (var callRes in _msg.getResList())
                    {
                        if(callRes == null)
                            continue;

                        if (callRes.getIsGainCg())
                        {
                            ConsortStoryRefObj consortStoryRefObj = GRefdataCoreMgr.instance.consortStoryRefCore.getRef(callRes.getConsortStoryId());
                            ConsortCGRefObj consortCgRefObj = GRefdataCoreMgr.instance.consortCGRefCore.getRef(consortStoryRefObj?.unlock_cg ?? 0);
                            if (consortCgRefObj != null)
                            {
                                NPUINoticeMgr.instance.addDealer(new NoticeDealer_ConsortRandCallProcess(()=>true, callRes, 0, null, null));
                        
                                stepCounter.chgTotalStepCount(1);
                                NPUINoticeMgr.instance.addDealer(new NoticeDealer_ConsortGetCG(() => true, consortCgRefObj, stepCounter.addDoneStepCount));
                            }
                        }
                        
                        ChildInfo getChildInfo = NPPlayer.instance.childComp.getChildInfo(callRes.getChildId());
                        if(getChildInfo != null)
                            getChildInfoList.Add(getChildInfo);
                    }
                }
                
                stepCounter.addDoneStepCount();

                foreach (var getChildInfo in getChildInfoList)
                {
                    if(getChildInfo != null)
                        NPUINoticeMgr.instance.addDealer(new NoticeDealer_ChildGet(getChildInfo));
                }
                
                _refreshWnd();//邀约成功后, 可能会有亲密度和体力变化, 所以这里整个页面都刷新一下
                NPUINoticeMgr.instance.addDoneDelegate(() =>
                {
                    WinMsg.SendMsg(WinMsgType.ON_CONSORT_INVITE_SHOW_DONE);
                });
                
            }, null);

        }

        /// <summary>
        /// 返回按钮被点击
        /// </summary>
        private void _onReturnBtnClick(GameObject _gameObject)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_CONSORT_MAIN);
        }

        /// <summary>
        /// 当妃子列表item被点击时 
        /// </summary>
        private void _onOnConsortItemClick(GGUIWndConsortListGridItem _itemWnd)
        {
            if(_itemWnd == null || _itemWnd.consortShowInfo == null || _m_bIsInviting)
                return;

            _openConsortWnd(_itemWnd.consortShowInfo);
        }

        /// <summary>
        /// cg按钮被点击
        /// </summary>
        /// <param name="_go"></param>
        private void _onCgBtnClick(GameObject _go)
        {
            QueueMgr.instance.AddNode(new GNodeConsortCGMain(EConsortCGType.NONE));
        }
        
        /// <summary>
        /// 切换入口妃子按钮被点击
        /// </summary>
        /// <param name="_go"></param>
        private void _onChgEntranceConsortBtnClick(GameObject _go)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndChangeEntranceConsort.instance, () =>
            {
                GGUIWndChangeEntranceConsort.instance.showWnd();
            }, UINodeTagConst.C_CHANGE_ENTRANCE_CONSORT);
        }
        
        #endregion

        #region Scroll滚动到指定妃子

        /// <summary>
        /// 滚动到指定类型的妃子位置
        /// 参数：_objs[0] 为 EConsortMainTargetConsortType 枚举（已由发送方解析）
        /// 参数：_objs[1] 为移动时间（float，已由发送方解析）
        /// 参数：_objs[2] 为妃子ID字符串（string，仅当 SPECIFIED_ID 时有效，由本方解析）
        /// </summary>
        private void _onScrollMoveToConsort(params object[] _objs)
        {
            if (_objs == null || _objs.Length < 1 || _objs[0] == null || _m_wConsortListGrid == null)
                return;

            EConsortMainTargetConsortType targetConsortType;
            if (_objs[0] is EConsortMainTargetConsortType)
                targetConsortType = (EConsortMainTargetConsortType) _objs[0];
            else if (_objs[0] is string s && ALCommon.TryEnumParse(typeof(EConsortMainTargetConsortType), s, out targetConsortType))
            { }
            else
                return;

            string param = string.Empty;
            if (_objs.Length >= 2 && _objs[1] is string)
                param = (string) _objs[1];
            
            float moveTime = 0.25f;
            if (_objs.Length >= 3 && _objs[2] is float t)
                moveTime = t;

            _m_wConsortListGrid.scrollMoveToTargetConsort(targetConsortType, param, moveTime, null);
        }

        #endregion

        #region 获取RectTransform

        /// <summary>
        /// 根据目标类型获取对应妃子的 RectTransform
        /// </summary>
        public RectTransform getTargetConsortRectTransform(EConsortMainTargetConsortType _type, string _params)
        {
            if (_m_wConsortListGrid == null)
                return null;

            return _m_wConsortListGrid.getTargetConsortRectTransform(_type, _params);
        }

        #endregion
    }
}