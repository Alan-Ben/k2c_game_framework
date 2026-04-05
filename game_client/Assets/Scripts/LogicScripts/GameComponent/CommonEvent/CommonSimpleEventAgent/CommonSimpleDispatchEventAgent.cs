using System;
using System.Collections.Generic;
using Common.EventObj;
using JetBrains.Annotations;

namespace GOE
{
    public class CommonSimpleDispatchEventAgent : _ACommonSimpleEventAgent
    {
        private CommonEventDispatchRefObj _m_rCommonEventDispatchRefObj;
        private CommonEventDispatchShowRefObj _m_rCommonEventDispatchShowRefObj;
        private List<CommonEventDispatchCondRefObj> _m_lDispatchCondRefObjList;//派遣事件条件列表
        private List<CommonEventDispatchResultRefObj> _m_lDispatchResultRefObjList;//派遣事件结果列表
        private EventDealAddInfo _m_iEventDealAddInfo;//事件处理的额外信息

        public CommonEventDispatchRefObj CommonEventDispatchRefObj { get { return _m_rCommonEventDispatchRefObj; } }
        public CommonEventDispatchShowRefObj CommonEventDispatchShowRefObj { get { return _m_rCommonEventDispatchShowRefObj; } }
        public List<CommonEventDispatchCondRefObj> dispatchConditionRefObjList { get { return _m_lDispatchCondRefObjList; } }
        public int dispatchCondtionNum { get { return _m_lDispatchCondRefObjList?.Count ?? 0; } }//派遣条件数量

        private CommonSimpleDispatchEventAgent(long _eventId, byte[] _eventShowInfo, [NotNull] CommonEventRefObj _commonEventRefObj, _ICommonSimpleEventDealExtOp _eventDealExtOp) 
            : base(_eventId, _eventShowInfo, _commonEventRefObj, _eventDealExtOp)
        {
            _m_rCommonEventDispatchRefObj = null;
            _m_rCommonEventDispatchShowRefObj = null;
            _m_lDispatchCondRefObjList = null;
            _m_lDispatchResultRefObjList = null;
            
            if (commonEventRefObj == null || commonEventRefObj.eventInstanceSubRefObj == null)
                return;
            
            try
            {
                _m_rCommonEventDispatchRefObj = (CommonEventDispatchRefObj)commonEventRefObj.eventInstanceSubRefObj;
            }
            catch (Exception e)
            {
                Debug.LogError($"[CommonDispatchEventAgent] 构造时传入的事件类型是:{commonEventRefObj.eventInstanceSubRefObj.eventType}, 但是强制转化为CommonEventDispatchRefObj时错误:{e}");
            }
            
            if (_m_rCommonEventDispatchRefObj != null)
            {
                _m_rCommonEventDispatchShowRefObj = GRefdataCoreMgr.instance.commonEventDispatchShowRefCore.getRef(_m_rCommonEventDispatchRefObj.dispatch_show_id);

                if (_m_rCommonEventDispatchRefObj.lConditionIdList != null)
                {
                    _m_lDispatchCondRefObjList = new List<CommonEventDispatchCondRefObj>();
                    CommonEventDispatchCondRefObj condRefObj = null;
                    foreach (long conditionId in _m_rCommonEventDispatchRefObj.lConditionIdList)
                    {
                        condRefObj = GRefdataCoreMgr.instance.commonEventDispatchCondRefCore.getRef(conditionId);
                        if(condRefObj != null)
                            _m_lDispatchCondRefObjList.Add(condRefObj);
                    }
                }
                
                if (_m_rCommonEventDispatchRefObj.lDispatchResultIdList != null)
                {
                    _m_lDispatchResultRefObjList = new List<CommonEventDispatchResultRefObj>();
                    CommonEventDispatchResultRefObj resultRefObj = null;
                    foreach (long resultId in _m_rCommonEventDispatchRefObj.lDispatchResultIdList)
                    {
                        resultRefObj = GRefdataCoreMgr.instance.commonEventDispatchResultRefCore.getRef(resultId);
                        if(resultRefObj != null)
                            _m_lDispatchResultRefObjList.Add(resultRefObj);
                    }
                }
            }
        }

        public static CommonSimpleDispatchEventAgent createEventAgent(long _eventId, byte[] _eventShowInfo, [NotNull]CommonEventRefObj _commonEventRefObj
            , _ICommonSimpleEventDealExtOp _eventDealExtOp)
        {
            return new CommonSimpleDispatchEventAgent(_eventId, _eventShowInfo, _commonEventRefObj, _eventDealExtOp);
        }
        
        protected override void _onUpdateServerEventShowInfo()
        {
        }

        protected override void _realDealEvent(_IEventDealAddInfo _dealAddInfo, Action _startDeal, Action _dealDone, Action _break)
        {
            _updateEventDealAddInfo(_dealAddInfo);

            QueueMgr.instance.AddNode(new GNodeCommonDispatchEvent(this, _startDeal, ()=>
            {
                if (_m_iEventDealAddInfo != null && _m_iEventDealAddInfo.showResult != null)
                {
                    _m_iEventDealAddInfo.showResult(_makeEventShowResultInfo(eventDoneInfo), _dealDone);
                }
                else
                {
                    if (afterEventDialogRefObj != null && afterEventDialogRefObj.is_need_bk)//若需要显示模糊背景, 先关闭事件处理窗口
                    {
                        _closeEventDealWnd();
                        eventDealDoneGeneralShowProcess(eventDoneInfo, null, null, _dealDone);
                    }
                    else
                    {
                        // 不需要模糊背景时, 大概率事件有自己背景, 要在背景加载完成后退出
                        eventDealDoneGeneralShowProcess(eventDoneInfo, _closeEventDealWnd, null, _dealDone);
                    }
                }
            }, () =>
            {
                if(!isEventDoneDataLevel)// 若在退出事件窗口时事件还未完成, 那么说明是中断退出
                    _break?.Invoke();
                // else // 若在退出事件窗口时事件已经完成, 那么会在事件中进行事件窗口关闭, 这里不需要额外判断了
                // {
                //     if (_m_iEventDealAddInfo != null && _m_iEventDealAddInfo.showResult != null)
                //     {
                //         _m_iEventDealAddInfo.showResult(_makeEventShowResultInfo(eventDoneInfo), _dealDone);
                //     }
                //     else
                //     {
                //         eventDealDoneGeneralShowProcess(eventDoneInfo, null, _dealDone);
                //     }
                // }
            }, _m_EventDealExtOp.dealEventIsOnlyUINode));
        }

        public override bool canAutoDealEvent { get { return true; } }
        protected override void _realAutoDealEvent(_IEventDealAddInfo _dealAddInfo, Action _dealDone, Action _break)
        {
            _updateEventDealAddInfo(_dealAddInfo);

            reqAutoDealEvent((_isSucc, _resMsg) =>
            {
                if (_isSucc)
                {
                    if (eventDoneInfo != null)
                    {
                        if (_m_iEventDealAddInfo != null && _m_iEventDealAddInfo.showResult != null)
                        {
                            _m_iEventDealAddInfo.showResult(_makeEventShowResultInfo(eventDoneInfo), _dealDone);
                        }
                        else
                        {
                            showEventDealResultWnd(eventDoneInfo, null, _dealDone);
                        }
                    }
                    else
                    {
                        _dealDone?.Invoke();
                    }
                }
                else
                    _break?.Invoke();
            });
        }
        
        public override byte[] getEventAutoDealInfoByteArray()
        {
            List<_IHeroCardShow> allHeroInfoList = new List<_IHeroCardShow>();
            NPPlayer.instance.heroComponent.dealAllHero((_heroInfo) =>
            {
                if(_heroInfo != null)
                    allHeroInfoList.Add(_heroInfo);
            });

            List<_IHeroCardShow> bestSchemeHeroList = new List<_IHeroCardShow>();//已经查找到的最优派遣方案
            int bestSchemeSatisfyCondCount = 0;//已经查找到的最优派遣方案满足的条件数量

            findBestDispatchScheme(allHeroInfoList, true, bestSchemeHeroList, false, ref bestSchemeSatisfyCondCount);

            CommonEvent_DealInfo_Dispatch dispatchEventDealInfo = new CommonEvent_DealInfo_Dispatch();
            foreach (var heroInfo in bestSchemeHeroList)
            {
                if(heroInfo != null && heroInfo.heroRefObj != null)
                    dispatchEventDealInfo.addHeroList(heroInfo.heroRefObj.id);
            }
            
            return dispatchEventDealInfo.makePackage();
        }

        protected override CommonEventShowResultInfo _makeEventShowResultInfo(CommonEvent_DoneInfo _eventDoneInfo)
        {
            if (_eventDoneInfo == null || _m_lDispatchResultRefObjList == null || _m_lDispatchResultRefObjList.Count <= 0)
                return null;

            byte[] eventDealInfoByte = _eventDoneInfo.getExtraInfo();
            if (eventDealInfoByte != null && eventDealInfoByte.Length > 0)
            {
                CommonEvent_DealInfo_Dispatch dealInfoDispatch = new CommonEvent_DealInfo_Dispatch();
                dealInfoDispatch.readPackage(eventDealInfoByte);

                int enableConditionNum = getEnableConditionNum(dealInfoDispatch.getHeroList());
                enableConditionNum = Math.Clamp(enableConditionNum, 0, _m_lDispatchResultRefObjList.Count - 1);
                CommonEventDispatchResultRefObj dispatchResultRefObj = _m_lDispatchResultRefObjList.SafeGet(enableConditionNum);
                
                return new CommonEventShowResultInfo(dispatchResultRefObj?.event_result_title, dispatchResultRefObj?.event_result_desc, _eventDoneInfo);
            }
            else
            {
                return new CommonEventShowResultInfo(string.Empty, string.Empty, _eventDoneInfo);
            }
        }

        #region 查找最优派遣方案

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_allHeroSatisfyConditionCountList">所有可选大臣列表</param>
        /// <param name="_allHeroListNeedSortBySatisfyCondCount">所有大臣列表是否需要先按照单个大臣满足条件数量排序</param>
        /// <param name="_bestSchemeHeroList">记录已经查找到的最优派遣方案</param>
        /// <param name="_needFillSchemeHeroList">是否需要填充选择大臣列表</param>
        /// <param name="_bestSchemeSatisfyCondCount">记录已经查找到的最优派遣方案满足的条件数量</param>
        /// <returns>返回true, 代表找到一个方案满足全部条件</returns>
        public bool findBestDispatchScheme([NotNull] List<HeroSatisfyConditionCount> _allHeroSatisfyConditionCountList, bool _allHeroListNeedSortBySatisfyCondCount,
            [NotNull] List<_IHeroCardShow> _bestSchemeHeroList, bool _needFillSchemeHeroList, ref int _bestSchemeSatisfyCondCount)
        {
            if(_allHeroListNeedSortBySatisfyCondCount)
                _allHeroSatisfyConditionCountList.Sort(HeroSatisfyConditionCount.sort);
            
            List<_IHeroCardShow> filterHeroInfoList = new List<_IHeroCardShow>();
            foreach (var item in _allHeroSatisfyConditionCountList)
            {
                if(item.count > 0)//若大臣连一个条件都不满足, 不用加入列表
                    filterHeroInfoList.Add(item.heroShowInfo);
            }
            
            List<_IHeroCardShow> tmpSelectedHeroList = new List<_IHeroCardShow>();//临时选中的大臣列表
            bool findBestScheme = _findBestDispatchScheme(filterHeroInfoList, 0, tmpSelectedHeroList, _bestSchemeHeroList, ref _bestSchemeSatisfyCondCount);
            if (_bestSchemeHeroList.Count < canDispatchMaxHeroNum && _needFillSchemeHeroList)//若已选择大臣小于可派遣大臣数量, 并且需要填充结果方案列表时
            {
                foreach (var heroSatisfyCondition in _allHeroSatisfyConditionCountList)
                {
                    if(_bestSchemeHeroList.Count >= canDispatchMaxHeroNum)//若已经填充满了, 退出循环
                        break;
                    
                    // 查看heroSatisfyCondition是否不在选中列表中, 是的话就添加
                    if (heroSatisfyCondition.heroShowInfo != null && !_bestSchemeHeroList.Contains(heroSatisfyCondition.heroShowInfo))
                    {
                        _bestSchemeHeroList.Add(heroSatisfyCondition.heroShowInfo);
                    }
                }
            }
            return findBestScheme;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_allHeroList">所有可选大臣列表</param>
        /// <param name="_allHeroListNeedSortBySatisfyCondCount">所有大臣列表是否需要先按照单个大臣满足条件数量排序</param>
        /// <param name="_bestSchemeHeroList">记录已经查找到的最优派遣方案</param>
        /// <param name="_needFillSchemeHeroList">是否需要填充选择大臣列表</param>
        /// <param name="_bestSchemeSatisfyCondCount">记录已经查找到的最优派遣方案满足的条件数量</param>
        /// <returns>返回true, 代表找到一个方案满足全部条件</returns>
        public bool findBestDispatchScheme([NotNull] List<_IHeroCardShow> _allHeroList, bool _allHeroListNeedSortBySatisfyCondCount,
            [NotNull] List<_IHeroCardShow> _bestSchemeHeroList, bool _needFillSchemeHeroList, ref int _bestSchemeSatisfyCondCount)
        {
            _bestSchemeHeroList.Clear();
            _bestSchemeSatisfyCondCount = 0;
            if (CommonEventDispatchRefObj == null || dispatchConditionRefObjList == null || dispatchConditionRefObjList.Count <= 0)
                return false;

            List<_IHeroCardShow> tmpSelectedHeroList = new List<_IHeroCardShow>();//临时选中的大臣列表
            if (_allHeroListNeedSortBySatisfyCondCount)// 若需要对所有大臣排序, 先进行数据处理
            {
                List<HeroSatisfyConditionCount> heroSatisfyConditionCount = getHeroSatisfyConditionCountList(_allHeroList);
                if (heroSatisfyConditionCount != null)
                {
                    List<_IHeroCardShow> filterHeroInfoList = new List<_IHeroCardShow>();
                    foreach (var item in heroSatisfyConditionCount)
                    {
                        if(item.count > 0)//若大臣连一个条件都不满足, 不用加入列表
                            filterHeroInfoList.Add(item.heroShowInfo);
                    }
                    
                    bool findBestScheme = _findBestDispatchScheme(filterHeroInfoList, 0, tmpSelectedHeroList, _bestSchemeHeroList, ref _bestSchemeSatisfyCondCount);
                    if (_bestSchemeHeroList.Count < canDispatchMaxHeroNum && _needFillSchemeHeroList)//若已选择大臣小于可派遣大臣数量, 并且需要填充结果方案列表时
                    {
                        foreach (var heroSatisfyCondition in heroSatisfyConditionCount)
                        {
                            if(_bestSchemeHeroList.Count >= canDispatchMaxHeroNum)//若已经填充满了, 退出循环
                                break;
                    
                            // 查看heroSatisfyCondition是否不在选中列表中, 是的话就添加
                            if (heroSatisfyCondition.heroShowInfo != null && !_bestSchemeHeroList.Contains(heroSatisfyCondition.heroShowInfo))
                            {
                                _bestSchemeHeroList.Add(heroSatisfyCondition.heroShowInfo);
                            }
                        }
                    }
                    return findBestScheme;
                }
                else
                {
                    bool findBestScheme = _findBestDispatchScheme(_allHeroList, 0, tmpSelectedHeroList, _bestSchemeHeroList, ref _bestSchemeSatisfyCondCount);
                    if (_bestSchemeHeroList.Count < canDispatchMaxHeroNum && _needFillSchemeHeroList)//若已选择大臣小于可派遣大臣数量, 并且需要填充结果方案列表时
                    {
                        foreach (var heroInfo in _allHeroList)
                        {
                            if(_bestSchemeHeroList.Count >= canDispatchMaxHeroNum)//若已经填充满了, 退出循环
                                break;
                    
                            // 查看heroSatisfyCondition是否不在选中列表中, 是的话就添加
                            if (heroInfo != null && !_bestSchemeHeroList.Contains(heroInfo))
                            {
                                _bestSchemeHeroList.Add(heroInfo);
                            }
                        }
                    }
                    return findBestScheme;
                }
            }
            else// 若不需要对所有大臣排序, 直接开始递归
            {
                bool findBestScheme = _findBestDispatchScheme(_allHeroList, 0, tmpSelectedHeroList, _bestSchemeHeroList, ref _bestSchemeSatisfyCondCount);
                if (_bestSchemeHeroList.Count < canDispatchMaxHeroNum && _needFillSchemeHeroList)//若已选择大臣小于可派遣大臣数量, 并且需要填充结果方案列表时
                {
                    foreach (var heroInfo in _allHeroList)
                    {
                        if(_bestSchemeHeroList.Count >= canDispatchMaxHeroNum)//若已经填充满了, 退出循环
                            break;
                    
                        // 查看heroSatisfyCondition是否不在选中列表中, 是的话就添加
                        if (heroInfo != null && !_bestSchemeHeroList.Contains(heroInfo))
                        {
                            _bestSchemeHeroList.Add(heroInfo);
                        }
                    }
                }
                return findBestScheme;
            }
        }
        
        /// <summary>
        /// 查找最优派遣计划
        /// </summary>
        /// <param name="_allHeroList">所有可选大臣列表</param>
        /// <param name="_startSelectIndex">本次可从下标开始的大臣选择</param>
        /// <param name="_tmpSelectedHeroList">临时选择的大臣列表</param>
        /// <param name="_bestSchemeHeroList">记录已经查找到的最优派遣方案</param>
        /// <param name="_bestSchemeSatisfyCondCount">记录已经查找到的最优派遣方案满足的条件数量</param>
        /// <returns>返回true, 代表找到一个方案满足全部条件</returns>
        private bool _findBestDispatchScheme([NotNull]List<_IHeroCardShow> _allHeroList, int _startSelectIndex, [NotNull]List<_IHeroCardShow> _tmpSelectedHeroList, [NotNull]List<_IHeroCardShow> _bestSchemeHeroList, ref int _bestSchemeSatisfyCondCount)
        {
            if (_tmpSelectedHeroList.Count > canDispatchMaxHeroNum)//若选择的大臣数量超过可选的最多大臣数量, 返回
                return false;

            int satisfyCondCount = getEnableConditionNum(_tmpSelectedHeroList);//获取满足的条件数量
            if (satisfyCondCount >= (dispatchConditionRefObjList?.Count ?? 0))//若满足所有条件, 说明找到了最优选法
            {
                _bestSchemeHeroList.Clear();
                _bestSchemeHeroList.AddRange(_tmpSelectedHeroList);
                _bestSchemeSatisfyCondCount = satisfyCondCount;
                return true;
            }
            else if(satisfyCondCount > _bestSchemeSatisfyCondCount)//若不满足所有条件, 但是已经满足的条件数量大于记录的最多数量, 先记录, 然后继续递归查找
            {
                _bestSchemeHeroList.Clear();
                _bestSchemeHeroList.AddRange(_tmpSelectedHeroList);
                _bestSchemeSatisfyCondCount = satisfyCondCount;
            }

            if (_tmpSelectedHeroList.Count == canDispatchMaxHeroNum)//若选择的大臣数量 == 可选的最多大臣数量, 返回
                return false;
            
            if (_startSelectIndex >= _allHeroList.Count)//若开始下标大于等于所有数据列表的数量, 返回
                return false;

            for (int i = _startSelectIndex; i < _allHeroList.Count; i++)
            {
                if(_allHeroList[i] == null)
                    continue;
                
                _tmpSelectedHeroList.Add(_allHeroList[i]);
                // 递归查找最优方案, 若找到返回true
                if (_findBestDispatchScheme(_allHeroList, i + 1, _tmpSelectedHeroList, _bestSchemeHeroList, ref _bestSchemeSatisfyCondCount))//若找到最优解, 直接返回
                    return true;
                _tmpSelectedHeroList.RemoveLast();
            }

            // 遍历完都没有找到最优方案, 返回false
            return false;
        }

        public HeroSatisfyConditionCount getHeroSatisfyConditionCount(_IHeroCardShow _heroInfo)
        {
            if(_heroInfo == null || dispatchConditionRefObjList == null)
                return new HeroSatisfyConditionCount(){heroShowInfo = _heroInfo, count = 0};
            
            int satisfyCondCount = 0;//满足条件数量
            foreach (var conditionRefObj in dispatchConditionRefObjList)
            {
                // 没有配置条件的情况, 算满足
                if (conditionRefObj == null || conditionRefObj.condition == null ||
                    conditionRefObj.condition.isEmpty || conditionRefObj.condition.IsEnable(_heroInfo.heroRefObj, null))
                {
                    satisfyCondCount++;
                }
            }
            
            return new HeroSatisfyConditionCount(){heroShowInfo = _heroInfo, count = satisfyCondCount};
        }
        
        /// <summary>
        /// 获取HeroSatisfyConditionCount列表
        /// </summary>
        public List<HeroSatisfyConditionCount> getHeroSatisfyConditionCountList(List<_IHeroCardShow> _allHeroList)
        {
            List<HeroSatisfyConditionCount> heroSatisfyConditionCountList = new List<HeroSatisfyConditionCount>();
            getHeroSatisfyConditionCountList(_allHeroList, heroSatisfyConditionCountList);

            return heroSatisfyConditionCountList;
        }
        
        /// <summary>
        /// 获取HeroSatisfyConditionCount列表
        /// </summary>
        public void getHeroSatisfyConditionCountList(List<_IHeroCardShow> _allHeroList, List<HeroSatisfyConditionCount> _heroSatisfyConditionCountList)
        {
            if(_heroSatisfyConditionCountList == null)
                return;
            
            _heroSatisfyConditionCountList.Clear();
            if (_allHeroList != null)
            {
                foreach (var heroShowInfo in _allHeroList)
                {
                    HeroSatisfyConditionCount heroSatisfyConditionCount = getHeroSatisfyConditionCount(heroShowInfo);
                    _heroSatisfyConditionCountList.Add(heroSatisfyConditionCount);
                }
                
                // 按满足条件数量多到少排序
                _heroSatisfyConditionCountList.Sort(HeroSatisfyConditionCount.sort);
            }
        }
        
        #endregion

        /// <summary>
        /// 获得达成条件的数量
        /// </summary>
        /// <returns></returns>
        public int getEnableConditionNum(List<_IHeroCardShow> _heroInfoList)
        {
            if (_m_lDispatchCondRefObjList == null)
                return 0;

            int enableConditionCount = 0;
            foreach (CommonEventDispatchCondRefObj conditionRef in _m_lDispatchCondRefObjList)
            {
                if (GRefdataCoreMgr.checkDispatchEventConditionIsEnable(conditionRef, _heroInfoList))
                    enableConditionCount++;
            }

            return enableConditionCount;
        }

        public int getEnableConditionNum(List<long> _heroInfoList)
        {
            if (_m_lDispatchCondRefObjList == null)
                return 0;

            int enableConditionCount = 0;
            foreach (CommonEventDispatchCondRefObj conditionRef in _m_lDispatchCondRefObjList)
            {
                if (GRefdataCoreMgr.checkDispatchEventConditionIsEnable(conditionRef, _heroInfoList))
                    enableConditionCount++;
            }

            return enableConditionCount;
        }
        
        /// <summary>
        /// 是否所有条件都达成
        /// </summary>
        /// <returns></returns>
        public bool allConditionEnable(List<_IHeroCardShow> _heroInfoList)
        {
            if (_m_lDispatchCondRefObjList == null)
                return true;

            foreach (CommonEventDispatchCondRefObj conditionRef in _m_lDispatchCondRefObjList)
            {
                if (!GRefdataCoreMgr.checkDispatchEventConditionIsEnable(conditionRef, _heroInfoList))//若有条件未达成, 返回false
                    return false;
            }

            return true;
        }
        
        /// <summary>
        /// 是否所有条件都达成
        /// </summary>
        /// <returns></returns>
        public bool allConditionEnable(List<long> _heroIdList)
        {
            if (_m_lDispatchCondRefObjList == null)
                return true;

            foreach (CommonEventDispatchCondRefObj conditionRef in _m_lDispatchCondRefObjList)
            {
                if (!GRefdataCoreMgr.checkDispatchEventConditionIsEnable(conditionRef, _heroIdList))//若有条件未达成, 返回false
                    return false;
            }

            return true;
        }

        private void _closeEventDealWnd()
        {
            QueueMgr.instance.forceCloseNodeByType(typeof(GNodeCommonDispatchEvent));
        }
        
        #region _ICommonEventShowInfo接口方法

        public override string eventName { get { return _m_rCommonEventDispatchShowRefObj?.event_name; } }
        public override string eventSimpleDesc { get { return _m_rCommonEventDispatchShowRefObj?.event_simple_desc; } }
        public override NPGTextureIndex inEventListIcon { get { return _m_rCommonEventDispatchShowRefObj?.event_list_icon; } }
        public override string eventDetailDesc { get { return _m_rCommonEventDispatchShowRefObj?.event_detail_desc; } }

        #endregion
        
        #region 事件具体处理时需要用到的一些方法

        public int canDispatchMaxHeroNum { get { return _m_rCommonEventDispatchRefObj?.hero_num ?? 0; } }
        public long akeyDispatchSimpleUnlockId { get { return GRefdataCoreMgr.instance.npGeneral.commonevent_dispatchevent_akey_dispatch_simple_unlock_id; } }
        public ENPWarningType dispatchEvent_NotAllConditionEnable_WaringTip { get { return ENPWarningType.COMMON_PVE_EVENT_DISPATCH_NOTALLCONDENABLE_TIP; } }
        public void reqDealEvent(List<long> _selectHeroIdList, Action<bool, CommonEvent_DoneInfo> _reqCallBack)
        {
            CommonEvent_DealInfo_Dispatch eventDealInfoDispatch = new CommonEvent_DealInfo_Dispatch(_selectHeroIdList);
            reqDealEvent(eventDealInfoDispatch.makePackage(), (_isSucc, _msg) =>
            {
                _reqCallBack?.Invoke(_isSucc, eventDoneInfo);
            });
        }

        /// <summary>
        /// 强制关闭窗口
        /// </summary>
        public override void forceBreakEventDeal()
        {
            _closeEventDealWnd();
        }

        #endregion
        
        #region 事件对应的_IEventDealAddInfo

        private class EventDealAddInfo : _IEventDealAddInfo
        {
            private Action<CommonEventShowResultInfo, Action> _m_aShowResultAction;//是否在结果窗口展示时完成事件, 若为false, 结果窗口关闭后才算完成
        
            public EventDealAddInfo(Action<CommonEventShowResultInfo, Action> _showResultAction)
            {
                _m_aShowResultAction = _showResultAction;
            }
        
            public Action<CommonEventShowResultInfo, Action> showResult { get { return _m_aShowResultAction; } }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_showResult">CommonEventShowResultInfo 为事件处理结果展示数据, Action为触发事件完成回调_triggerEventDone</param>
        /// <returns></returns>
        public static _IEventDealAddInfo getEventDealAddInfo(Action<CommonEventShowResultInfo, Action> _showResult)
        {
            return new EventDealAddInfo(_showResult);
        }

        private void _updateEventDealAddInfo(_IEventDealAddInfo _dealAddInfo)
        {
            if (_dealAddInfo is EventDealAddInfo)
            {
                _m_iEventDealAddInfo = (EventDealAddInfo) _dealAddInfo;
            }
            else
            {
                _m_iEventDealAddInfo = null;
            }
        }

        #endregion
    }
    
    /// <summary>
    /// 大臣满足的条件数量
    /// </summary>
    public struct HeroSatisfyConditionCount
    {
        public _IHeroCardShow heroShowInfo;//大臣id
        public int count;//数量

        public static int sort(HeroSatisfyConditionCount _a, HeroSatisfyConditionCount _b)
        {
            int compareRes = -_a.count.CompareTo(_b.count);
            if (compareRes != 0)
                return compareRes;

            if (Object.ReferenceEquals(_a.heroShowInfo, _b.heroShowInfo))
                return 0;
            if (_b.heroShowInfo == null)
                return -1;
            if (_a.heroShowInfo == null)
                return 1;

            compareRes = _a.heroShowInfo.level.CompareTo(_b.heroShowInfo.level);
            if (compareRes != 0)
                return -compareRes;
            
            if (Object.ReferenceEquals(_a.heroShowInfo.heroRefObj, _b.heroShowInfo.heroRefObj))
                return 0;
            if (_b.heroShowInfo.heroRefObj == null)
                return -1;
            if (_a.heroShowInfo.heroRefObj == null)
                return 1;

            return _b.heroShowInfo.heroRefObj.id.CompareTo(_a.heroShowInfo.heroRefObj.id);
        }
    }
}