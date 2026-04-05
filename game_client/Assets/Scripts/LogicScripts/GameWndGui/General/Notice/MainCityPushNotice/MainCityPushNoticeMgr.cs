using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using NPEnum;

namespace GOE
{
    public class MainCityPushNoticeMgr
    {
        private static MainCityPushNoticeMgr _m_instance;
        public static MainCityPushNoticeMgr instance { get { return _m_instance ??= new MainCityPushNoticeMgr(); } }

        private bool _m_bIsFirstShow;//是否首次展示, 首次展示代表是登录触发
        private bool _m_bIsNoticeDisable;//是否在处理中禁用了Notice展示
        private int _m_iNoticeDisableSerialize = -1;//Notice禁用的序列号
        private bool _m_bIsAddingNotice;//是否正在添加Notice
        private Action _m_aOnAllNoticeAddDone;//所有Notice添加完成回调
        private long _m_lAddNoticeSerialize;//添加Notice的序列号
        
        [NotNull] private List<NPUINoticeMgr._ANPUINoticeDealer> _m_lTmpBeforMainCityPushNoticeList = new List<NPUINoticeMgr._ANPUINoticeDealer>();
        [NotNull] private List<NPUINoticeMgr._ANPUINoticeDealer> _m_lTmpAfterMainCityPushNoticeList = new List<NPUINoticeMgr._ANPUINoticeDealer>();

        protected MainCityPushNoticeMgr()
        {
            _m_bIsFirstShow = true;
            _m_bIsNoticeDisable = false;
            _m_iNoticeDisableSerialize = -1;
            _m_bIsAddingNotice = false;
            _m_aOnAllNoticeAddDone = null;
            _m_lAddNoticeSerialize = 0;
        }
        
        public long addNoticeSerialize { get { return _m_lAddNoticeSerialize; } }
        
        public void reset()
        {
            _m_bIsFirstShow = true;
            _m_bIsAddingNotice = false;
            _m_aOnAllNoticeAddDone = null;
            _m_lAddNoticeSerialize = ALSerializeOpMgr.next();
            
            _m_lTmpBeforMainCityPushNoticeList.Clear();
            _m_lTmpAfterMainCityPushNoticeList.Clear();
            
            // 移除所有主城推送弹窗Notice
            _removeAllMainCityPushNotice(null);
            
            enableNoticeShow();//恢复Notice展示
        }

        /// <summary>
        /// 暂停Notice展示
        /// </summary>
        public void disableNoticeShow()
        {
            int preDisableSerialize = _m_iNoticeDisableSerialize;
            _m_iNoticeDisableSerialize = NPUINoticeMgr.instance.setNoticeDisable();//先禁用Notice
            if (_m_bIsNoticeDisable)//若已经处于禁用状态, 则恢复之前的禁用状态
            {
                NPUINoticeMgr.instance.setNoticeEnable(preDisableSerialize);
            }
            else
            {
                _m_bIsNoticeDisable = true;
            }
        }
        
        /// <summary>
        /// 恢复Notice展示
        /// </summary>
        public void enableNoticeShow()
        {
            NPUINoticeMgr.instance.setNoticeEnable(_m_iNoticeDisableSerialize);
            _m_bIsNoticeDisable = false;
        }
        
        public void showMainCityPushNotice(Action _onAllNoticeAddDone)
        {
            _m_aOnAllNoticeAddDone += _onAllNoticeAddDone;
            if (_m_bIsAddingNotice)
                return;
            
            bool isLogin = _m_bIsFirstShow;
            _m_bIsFirstShow = false;
            _m_bIsAddingNotice = true;
            long serialize = _m_lAddNoticeSerialize = ALSerializeOpMgr.next();
            
            disableNoticeShow();//先禁用Notice，防止下面加Notice时直接弹出
            _removeAllMainCityPushNotice(_m_lTmpBeforMainCityPushNoticeList);//在下面添加主城推送弹窗前, 先移除之前未展示完的主城推送弹窗, 这部分弹窗是不需要参与排序的

            _addMainCityPushNotice(serialize, isLogin, () =>
            {
                if (serialize != _m_lAddNoticeSerialize)
                {
                    // 因为_m_lAddNoticeSerialize只有可能在reset和showMainCityPushNotice时变化, 但是showMainCityPushNotice中会判断_m_bIsAddingNotice,
                    // 已经处于添加Notice中时不会重复调用showMainCityPushNotice, 所以在这个回调内出现serialize != _m_lAddNoticeSerialize的情况, 只有可能是reset调用导致的
                    // 在reset中已经将需要恢复的数据全部重置, 所以这里直接返回即可
                    return;
                }
                
                // 将新增的所有主城推送弹窗Notice移除
                _removeAllMainCityPushNotice(_m_lTmpAfterMainCityPushNoticeList);
                // 对新增的主城推送弹窗Notice进行排序
                _m_lTmpAfterMainCityPushNoticeList.Sort(_sortMainCityPushNotice);
            
                // // 重新添加主城推送弹窗Notice, 主城弹窗Notice需要在最前面展示
                // // 因为是从头插入, 所以先插入的反而会后显示, 所以这里先插入后面添加的Notice, 在插入前面未展示完的Notice
                // NPUINoticeMgr.instance.addDealerListFront(_m_lTmpAfterMainCityPushNoticeList);
                // //因为_m_lTmpBeforMainCityPushNoticeList列表是通过_removeAllMainCityPushNotice方法获取的, 在remove中是倒序移除, 所以_m_lTmpBeforMainCityPushNoticeList中dealer顺序是与原来顺序相反的, 所以这里要再倒序加入
                // NPUINoticeMgr.instance.addDealerListFront(_m_lTmpBeforMainCityPushNoticeList, true);
                // _m_lTmpAfterMainCityPushNoticeList.Clear();
                // _m_lTmpBeforMainCityPushNoticeList.Clear();
                
                NPUINoticeMgr.instance.addDealerList(_m_lTmpBeforMainCityPushNoticeList, true);
                NPUINoticeMgr.instance.addDealerList(_m_lTmpAfterMainCityPushNoticeList);
                
                _m_bIsAddingNotice = false;
                enableNoticeShow(); //Notice恢复展示
                Action action = _m_aOnAllNoticeAddDone;
                _m_aOnAllNoticeAddDone = null;
                action?.Invoke();
            });
        }

        private void _addMainCityPushNotice(long _addNoticeSerialize, bool _isLogin, Action _allNoticeAddDone)
        {
            // 添加进入主城需要的主城推送弹窗Notice
            ALProcess popCityAndRoom = ALProcess.CreateProcess("_addMainCityPushNotice");
            //周卡结算信息推送弹窗
            popCityAndRoom.addDelegateProcess((_delegateComplete) =>
            {
                addMainCityPushNotice(_addNoticeSerialize, _isLogin, _delegateComplete, _addWeekCardGetSettleNotice);
            });
            //展示运营公告
            // popCityAndRoom.addDelegateProcess((_delegateComplete) =>
            // {
            //     addMainCityPushNotice(_addNoticeSerialize, _isLogin, _delegateComplete, _addAnnouncementNotice);
            // });
            //离线金币收益弹窗
            popCityAndRoom.addDelegateProcess((_delegateComplete) =>
            {
                addMainCityPushNotice(_addNoticeSerialize, _isLogin, _delegateComplete, _addOfflineGoldEarningsNotice);
            });
            //检查倒计时事件是否需要展示新事件对话或者重置事件对话
            popCityAndRoom.addDelegateProcess((_delegateComplete) =>
            {
                addMainCityPushNotice(_addNoticeSerialize, _isLogin ? EMainCityPushNoticeTriggerType.LOGIN : EMainCityPushNoticeTriggerType.OTHER, _delegateComplete, NPPlayer.instance.countdownEventComp.checkDialogueShow);
            });
            //添加活动合并展示推送弹窗Notice
            popCityAndRoom.addDelegateProcess((_delegateComplete) =>
            {
                addMainCityPushNotice(_addNoticeSerialize, _isLogin, _delegateComplete, _addActivityMergeShowPushNotice);
            });
            
            
            ///////////////////////需要添加notice在上面添加/////////////////////////
            
            // 添加热更工程主城推送弹窗
            popCityAndRoom.addDelegateProcess((_delegateComplete) =>
            {
                if (_m_lAddNoticeSerialize == _addNoticeSerialize)
                {
                    HotfixStaticFunc.addMainCityPushNotice(_addNoticeSerialize, _isLogin, _delegateComplete);
                }
                else
                {
                    _delegateComplete?.Invoke();
                }
            });
            
            popCityAndRoom.addProcess(() =>
            {
                _allNoticeAddDone?.Invoke();                
            });
            
            popCityAndRoom.deal();
        }
        /// <summary>
        /// 移除所有主城推送弹窗Notice
        /// </summary>
        /// <param name="_removeNoticeList"></param>
        private void _removeAllMainCityPushNotice(List<NPUINoticeMgr._ANPUINoticeDealer> _removeNoticeList)
        {
            _removeNoticeList?.Clear();
            NPUINoticeMgr.instance.removeDealer((_noticeDealer) =>
            {
                if (_noticeDealer != null && _noticeDealer is _AMainCityPushNotice)
                {
                    _removeNoticeList?.Add(_noticeDealer);
                    return true;
                }

                return false;
            });
        }
        private int _sortMainCityPushNotice(_AMainCityPushNotice _a, _AMainCityPushNotice _b)
        {
            if(_b == null || _b.mainCityPushNoticeRefObj == null) return -1;
            if(_a == null || _a.mainCityPushNoticeRefObj == null) return 1;
            if(ReferenceEquals(_a, _b)) return 0;
            
            return _a.mainCityPushNoticeRefObj.sorting_order.CompareTo(_b.mainCityPushNoticeRefObj.sorting_order);
        }
        
        private int _sortMainCityPushNotice(NPUINoticeMgr._ANPUINoticeDealer _a, NPUINoticeMgr._ANPUINoticeDealer _b)
        {
            if(_b == null || !(_b is _AMainCityPushNotice mainCityPushNoticeB) || mainCityPushNoticeB.mainCityPushNoticeRefObj == null) return -1;
            if(_a == null || !(_a is _AMainCityPushNotice mainCityPushNoticeA) || mainCityPushNoticeA.mainCityPushNoticeRefObj == null) return 1;
            if(ReferenceEquals(_a, _b)) return 0;
            
            return mainCityPushNoticeA.mainCityPushNoticeRefObj.sorting_order.CompareTo(mainCityPushNoticeB.mainCityPushNoticeRefObj.sorting_order);
        }

        #region 添加主城推送弹窗Notice

        public void addMainCityPushNotice(long _addNoticeSerialize, bool _isLogin, Action _addDoneAction, Action<bool, Action> _addNoticeAction)
        {
            if (_m_lAddNoticeSerialize != _addNoticeSerialize || _addNoticeAction == null)
            {
                _addDoneAction?.Invoke();
                return;
            }

            _addNoticeAction(_isLogin, _addDoneAction);
        }
        
        public void addMainCityPushNotice(long _addNoticeSerialize, EMainCityPushNoticeTriggerType _noticeTriggerType, Action _addDoneAction, Action<EMainCityPushNoticeTriggerType, Action> _addNoticeAction)
        {
            if (_m_lAddNoticeSerialize != _addNoticeSerialize || _addNoticeAction == null)
            {
                _addDoneAction?.Invoke();
                return;
            }

            _addNoticeAction(_noticeTriggerType, _addDoneAction);
        }
        
        /// <summary>
        /// 周卡结算信息推送弹窗
        /// </summary>
        /// <param name="_isLogin"></param>
        /// <param name="_addDone"></param>
        private void _addWeekCardGetSettleNotice(bool _isLogin, Action _addDone)
        {
            long offlineMs = NPPlayer.instance.playerInfo.getValue(ENPPlayerParam.OFFLINE_PERIOD_REWARD_DURATION_MS);
            if (offlineMs == 0)
            {
                _addDone?.Invoke();
                return;
            }
            NPPlayer.instance.weekCardComp.reqWeekCardSettleInfo((_info)=>
                {
                    //没有周卡的结算信息
                    // else if (_info.getSettleInfo().getDetailList().Count == 0 && _info.getSettleInfo().getItemList().Count == 0)
                    // {
                    //     
                    // }
                
                    NPUINoticeMgr.instance.addDealer(new NoticeDealer_WeekCardGetSettleInfo(_info, offlineMs, _isLogin ? EMainCityPushNoticeTriggerType.LOGIN : EMainCityPushNoticeTriggerType.OTHER));
                    _addDone?.Invoke();
                },
                () =>
                {
                    _addDone?.Invoke();
                });
        }
        
        /// <summary>
        /// 运营公告弹窗
        /// </summary>
        /// <param name="_isLogin"></param>
        /// <param name="_addDone"></param>
        private void _addAnnouncementNotice(bool _isLogin, Action _addDone)
        {
            //展示运营公告
            AnnouncementMgr.instance.checkCanShowNotice(_isLogin ? EMainCityPushNoticeTriggerType.LOGIN : EMainCityPushNoticeTriggerType.OTHER, (_hasAdd) =>
            {
                _addDone?.Invoke();
            });
        }
        
        /// <summary>
        /// 离线金币收益弹窗
        /// </summary>
        /// <param name="_isLogin"></param>
        /// <param name="_addDone"></param>
        private void _addOfflineGoldEarningsNotice(bool _isLogin, Action _addDone)
        {
            _NPPlayerConditionSerializeInfo showOfflineGoldEarningsCond = GRefdataCoreMgr.instance.npGeneral.show_offline_gold_earnings_cond;
            if (showOfflineGoldEarningsCond != null && !showOfflineGoldEarningsCond.isEmpty && !showOfflineGoldEarningsCond.IsEnable(null))
            {
                NPPlayer.instance.specialItemComp.goldData.setIsShowedOfflineEarnings();
                _addDone?.Invoke();
                return;
            }

            OfflineGoldData offlineData = NPPlayer.instance.specialItemComp.goldData.offlineData;
            // 显示离线金币收益
            if (offlineData.count > 0 && !NPPlayer.instance.specialItemComp.goldData.isShowedOfflineEarnings)
            {
                NPUINoticeMgr.instance.addDealer(new NoticeDealer_OfflineGoldEarnings(offlineData, _isLogin ? EMainCityPushNoticeTriggerType.LOGIN : EMainCityPushNoticeTriggerType.OTHER));
                NPPlayer.instance.specialItemComp.goldData.setIsShowedOfflineEarnings();
                _addDone?.Invoke();
            }
            else
            {
                _addDone?.Invoke();
            }
        }
        
        /// <summary>
        /// 添加活动合并展示推送弹窗Notice
        /// </summary>
        /// <param name="_isLogin"></param>
        private void _addActivityMergeShowPushNotice(bool _isLogin, Action _addDone)
        {
            if (_isLogin)//只有登录时需要展示
            {
                NPUINoticeMgr.instance.addDealer(new NoticeDealer_ActivityMergeShow(EMainCityPushNoticeTriggerType.LOGIN));
                _addDone?.Invoke();
            }
            else
            {
                _addDone?.Invoke();
            }
        }

        #endregion
        
    }
}