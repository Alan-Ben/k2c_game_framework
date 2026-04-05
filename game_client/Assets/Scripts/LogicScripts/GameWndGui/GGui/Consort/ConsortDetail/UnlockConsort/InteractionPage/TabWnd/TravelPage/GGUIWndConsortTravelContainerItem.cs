using System;
using System.Collections.Generic;
using ALPackage;
using Common.ConsortObj;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndConsortTravelContainerItem : _ATALBasicUISubWnd<GGUIMonoConsortTravelContainerItem>
    {
        private GGottenConsortInfo _m_rGottenConsortInfo;//当前妃子信息
        private ConsortTravelRefObj _m_rTravelRefObj;//出游配表数据
        [NotNull] private List<TimesPriceRefObj> _m_lAllTimesPriceRefObjList = new List<TimesPriceRefObj>();//所有的次数价格表数据

        private bool _m_bHasReqTravelCount;//是否已经请求过出游次数
        private long _m_lHasTravelCount;//已经出游次数
        private TimesPriceRefObj _m_rCurTimesPriceRefObj;//当前次数对应的价格表数据
        private NPCommonCostItem _m_curCostItem;//当前消耗道具
        
        private NPGGuiWndTexture _m_wTravelBanner;//出游背景
        private NPGGUIWndCommonItem _m_wCostItemWnd;//消耗道具
        
        private long _m_lShowSerialize;
        
        public GGUIWndConsortTravelContainerItem(GGUIMonoConsortTravelContainerItem _wnd) : base(_wnd)
        {
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.imgTravelBanner != null)
                _m_wTravelBanner = new NPGGuiWndTexture(wnd.imgTravelBanner);

            if (wnd.monoCostItem != null)
                _m_wCostItemWnd = new NPGGUIWndCommonItem(wnd.monoCostItem);
            
            ALUGUICommon.combineBtnClick(wnd.travelBtn, _onTravelBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnDiscountInfo, _onDiscountInfoBtnClick);
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.travelBtn, _onTravelBtnClick);
                ALUGUICommon.uncombineBtnClick(wnd.btnDiscountInfo, _onDiscountInfoBtnClick);
            }
            
            _m_wTravelBanner?.discard();
            _m_wTravelBanner = null;
            
            _m_wCostItemWnd?.discard();
            _m_wCostItemWnd = null;
            
            _m_lAllTimesPriceRefObjList.Clear();
        }
        
        protected override void _onShowWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            
            WinMsg.RegisterMsgAct(WinMsgType.ON_CONSORT_TRAVEL_COUNT_CHG, _onTravelCountChg);
        }

        protected override void _onHideWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            
            WinMsg.UnregisterMsgAct(WinMsgType.ON_CONSORT_TRAVEL_COUNT_CHG, _onTravelCountChg);

            _m_wTravelBanner?.hideWnd();
            _m_wCostItemWnd?.hideWnd();
            
            _m_bHasReqTravelCount = false;
            
            
            _m_lAllTimesPriceRefObjList.Clear();
        }

        protected override void _onReset()
        {
            _m_wTravelBanner?.discardTexture();
            _m_wCostItemWnd?.resetWnd();
        }

        public void setData(GGottenConsortInfo _consortInfo, ConsortTravelRefObj _travelRefObj)
        {
            _m_bHasReqTravelCount = false;
            _m_rGottenConsortInfo = _consortInfo;
            _m_rTravelRefObj = _travelRefObj;
            _m_lAllTimesPriceRefObjList.Clear();
            
            if(_m_rTravelRefObj == null)
                return;

            List<TimesPriceRefObj> allTimesPriceRefObjList = GRefdataCoreMgr.instance.getTimesPriceList(_m_rTravelRefObj.time_price_id);
            if (allTimesPriceRefObjList != null)
            {
                _m_lAllTimesPriceRefObjList.AddRange(allTimesPriceRefObjList);
                _m_lAllTimesPriceRefObjList.Sort((_a, _b) =>
                {
                    if (_b == null)
                        return -1;
                    if (_a == null)
                        return 1;
                    if (ReferenceEquals(_a, _b))
                        return 0;

                    return _a.times.CompareTo(_b.times);//按照次数从小到大排序
                });
            }

            _refreshWnd();
        }

        /// <summary>
        /// 刷新窗口
        /// </summary>
        private void _refreshWnd()
        {
            if(_m_rTravelRefObj == null || wnd == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtTravelName, TextTranslate.instance.getLanguage(_m_rTravelRefObj.name));
            if (_m_wTravelBanner != null)
            {
                _m_wTravelBanner.showWnd();
                _m_wTravelBanner.setTexture(_m_rTravelRefObj.banner);
            }
            
            ALUGUICommon.setLabelTxt(wnd.txtTravelDesc, TextTranslate.instance.getLanguage(_m_rTravelRefObj.desc));

            if (string.IsNullOrEmpty(wnd.gainGainCharmKey))
            {
                ALUGUICommon.setLabelTxt(wnd.txtGainCharm, TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, _m_rTravelRefObj.add_bless));
            }
            else
            {
                ALUGUICommon.setLabelTxt(wnd.txtGainCharm, TextTranslate.instance.getLanguage(wnd.gainGainCharmKey, _m_rTravelRefObj.add_bless));
            }

            if (string.IsNullOrEmpty(wnd.gainGainCharmPointMultipleKey))
            {
                ALUGUICommon.setLabelTxt(wnd.txtGainCharmPointMultiple,
                    TextTranslate.instance.getLanguage(TransKeyConst.common_add_num,
                        _m_rTravelRefObj.bless_point_multiple));
            }
            else
            {
                ALUGUICommon.setLabelTxt(wnd.txtGainCharmPointMultiple,
                    TextTranslate.instance.getLanguage(wnd.gainGainCharmPointMultipleKey,
                        _m_rTravelRefObj.bless_point_multiple));
            }
            
            ALUGUICommon.setGameObjEnable(wnd.supportDiscountShow, !string.IsNullOrEmpty(_m_rTravelRefObj.discount_desc));
            ALUGUICommon.setGameObjEnable(wnd.nonsupportDiscountShow, string.IsNullOrEmpty(_m_rTravelRefObj.discount_desc));

            _refreshTravelCost();

            ALUGUICommon.setLabelTxt(wnd.travelBtnDesc, TextTranslate.instance.getLanguage(_m_rTravelRefObj.btn_desc));
        }

        /// <summary>
        /// 刷新出游消耗(跟出游次数相关, 需要向服务器请求数据)
        /// </summary>
        private void _refreshTravelCost()
        {
            if(_m_rTravelRefObj == null || wnd == null)
                return;
            
            _reqTravelCount(() =>
            {
                if(wnd == null || !isShow || _m_rCurTimesPriceRefObj == null)
                    return;

                ALUGUICommon.setGameObjEnable(wnd.thisTimeTravelHasDiscountShow, _m_rCurTimesPriceRefObj.discount > 0);
                ALUGUICommon.setGameObjEnable(wnd.thisTimeTravelNoDiscountShow, _m_rCurTimesPriceRefObj.discount <= 0);

                if (string.IsNullOrEmpty(wnd.discountKey))
                {
                    ALUGUICommon.setLabelTxt(wnd.txtDiscount, TextTranslate.instance.getLanguage(TransKeyConst.common_reducedPropPer_num, (10000 - _m_rCurTimesPriceRefObj.discount) / 100f));
                }
                else
                {
                    ALUGUICommon.setLabelTxt(wnd.txtDiscount, TextTranslate.instance.getLanguage(wnd.discountKey, (10000 - _m_rCurTimesPriceRefObj.discount) / 100f));
                }

                if (_m_wCostItemWnd != null)
                {
                    _m_wCostItemWnd.showWnd();
                    _m_wCostItemWnd.setItem(_m_curCostItem);
                }
            });
        }
        
        /// <summary>
        /// 请求出游次数
        /// </summary>
        private void _reqTravelCount(Action _reqDone)
        {
            _m_bHasReqTravelCount = false;

            if (_m_rTravelRefObj == null)
            {
                Debug.LogError("GGUIWndConsortTravelContainerItem._reqTravelCount _m_rTravelRefObj is null");
                return;
            }
            
            NPPlayer.instance.consortComp.reqGetRoundTravelCount(_m_rTravelRefObj.id, _msg =>
            {
                if (_msg != null)
                {
                    _m_bHasReqTravelCount = true;
                    _m_lHasTravelCount = _msg.getCounter();
                    
                    _m_rCurTimesPriceRefObj = GRefdataCoreMgr.instance.getTimesPriceRefObj(_m_rTravelRefObj.time_price_id, _m_lHasTravelCount + 1);
                    if (_m_rCurTimesPriceRefObj == null)
                    {
                        Debug.LogError($"GGUIWndConsortTravelContainerItem._reqTravelCount] 找不到 travelId:{_m_rTravelRefObj.id}, times:{_m_lHasTravelCount + 1} 对应的TimePrice配表数据");
                        _m_curCostItem = null;
                        return;
                    }

                    _m_curCostItem = new NPCommonCostItem(_m_rCurTimesPriceRefObj.item, _m_rCurTimesPriceRefObj.cost_item_formula?.CalculateVariableResult(null) ?? 0);
                    
                    _reqDone?.Invoke();
                }
            }, null);
        }

        /// <summary>
        /// 当出游按钮点击
        /// </summary>
        /// <param name="_go"></param>
        private void _onTravelBtnClick(GameObject _go)
        {
            if(!_m_bHasReqTravelCount || _m_rTravelRefObj == null || _m_rCurTimesPriceRefObj == null || _m_rGottenConsortInfo == null)
                return;

            if (_m_curCostItem != null && !GCommon.isItemEnough(_m_curCostItem, true))
                return;

            long consortId = _m_rGottenConsortInfo.consortId;
            long travelId = _m_rTravelRefObj.id;
            long serialize = _m_lShowSerialize;
            NPPlayer.instance.consortComp.reqCallAppoint(consortId, travelId, (_msg) =>
            {
                if(_msg == null || wnd == null || serialize != _m_lShowSerialize || _m_rGottenConsortInfo == null || _m_rTravelRefObj == null)
                    return;
                
                ALProcess process = ALProcess.CreateProcess();
                process
                    // 展示指定邀约表现过程
                    .addDelegateProcess((_processComplete) =>
                    {
                        QueueMgr.instance.addNode_InGame_SingleWnd_OnlyCloseDiscard(GGUIWndConsortAppointCallProcess.instance,
                        () =>
                            {
                                GGUIWndConsortAppointCallProcess.instance.showWnd();
                                GGUIWndConsortAppointCallProcess.instance.setData(_msg, _m_rGottenConsortInfo, _m_rTravelRefObj,
                                    () =>
                                    {
                                        QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_CONSORT_APPOINT_CALL_PROCESS);
                                        _processComplete?.Invoke();
                                    });
                            }, UINodeTagConst.C_CONSORT_APPOINT_CALL_PROCESS);
                    })
                    .addDelegateProcess((_processComplete) =>
                    {
                        GGUIWndConsortRandomInviteReward.instance.setData(new Common.ConsortObj.Consort_CallRes(consortId, 0, _msg.getAddCharmPoint(), false, _msg.getChildId()));
                        QueueMgr.instance.AddNode(new GNodeCommonWndWithCloseFunc_OnlyCloseDiscar(GGUIWndConsortRandomInviteReward.instance,
                            () =>
                            {
                                _processComplete?.Invoke();
                            }, UINodeTagConst.C_CONSORT_RANDOM_INVITE_REWARD));
                    })
                    .addDelegateProcess((_processComplete) =>
                    {
                        long childId = _msg.getChildId();
                        ChildInfo childInfo = NPPlayer.instance.childComp.getChildInfo(childId);
                        if (childInfo != null)
                        {
                            GGUIWndChildGet.instance.refreshWnd(childInfo, () =>
                            {
                                _processComplete?.Invoke();
                            });
                            QueueMgr.instance.AddNode(new GNodeCommonWndWithCloseFunc_OnlyCloseDiscar(GGUIWndChildGet.instance,
                                () =>
                                {
                                    // 若当前没有跳转到子女详情界面，则直接结束流程；若跳转到子女详情界面了，则等子女详情界面关闭后再结束流程
                                    if(!GGUIWndChildGet.instance.isGotoChildInfoWnd)
                                        _processComplete?.Invoke();
                                }, UINodeTagConst_Child.C_CHILD_GET));
                        }
                        else
                        {
                            _processComplete?.Invoke();
                        }
                    })
                    .addProcess(() =>
                    {
                        WinMsg.SendMsg(WinMsgType.CONTROL_TUROTIAL_SETP, ENPTutorialTriggerType.CONSORT_APPOINT_CALL_SHOW_DONE);
                    })
                    .deal();
                
                // 刷新出游消耗
                _refreshTravelCost();
            }, null);
        }

        /// <summary>
        /// 模拟点击当前出游item
        /// </summary>
        public void simulateClickTravelBtn()
        {
            _onTravelBtnClick(null);
        }

        /// <summary>
        /// 折扣信息按钮被点击
        /// </summary>
        /// <param name="_go"></param>
        private void _onDiscountInfoBtnClick(GameObject _go)
        {
            if(wnd == null || _m_rTravelRefObj == null)
                return;

            List<long> descList = new List<long>();
            if (_m_rTravelRefObj.discount_desc_show_discount_left_list != null)
            {
                foreach (long discount in _m_rTravelRefObj.discount_desc_show_discount_left_list)
                {
                    descList.Add(_leftDiscountCount(_m_lHasTravelCount, discount));
                }    
            }

            long uiResId = wnd.discountToolTipUiResId <= 0 ? UIResPathConst.WIN_TOOL_TIP_TEXT : wnd.discountToolTipUiResId;
            QueueMgr.instance.AddNode(new NPGNodeCommonToolTip_Text(
                UIResPathAssistant.getAssetPath(uiResId),
                UIResPathAssistant.getObjName(uiResId),
                TextTranslate.instance.getLanguage(_m_rTravelRefObj.discount_desc, descList),
                _go.GetComponent<RectTransform>(), wnd.discountToolTipInterval.x, wnd.discountToolTipInterval.y));
        }
        
        /// <summary>
        /// 当前收到出游次数变化消息
        /// </summary>
        private void _onTravelCountChg()
        {
            _refreshTravelCost();
        }

        #region 剩余折扣次数
        
        /// <summary>
        /// 剩余折扣次数
        /// </summary>
        /// <param name="_times">当前已购买次数</param>
        /// <param name="_discount">需要查找的折扣万分比</param>
        /// <returns></returns>
        private long _leftDiscountCount(long _times, long _discount)
        {
            if (_m_lAllTimesPriceRefObjList.Count <= 0)
                return 0;

            long nowTravelCount = _times + 1;
            long leftDiscountCount = 0;//剩余折扣次数
            TimesPriceRefObj nowTimesPriceRefObj = null;//当前遍历到的折扣数据
            TimesPriceRefObj nextTimesPriceRefObj = null;//下一个的折扣数据
            
            for (int i = 0; i < _m_lAllTimesPriceRefObjList.Count - 1;)//遍历到列表最后一个元素前的一个元素
            {
                nowTimesPriceRefObj = _m_lAllTimesPriceRefObjList[i];
                //若当前遍历到的折扣数据为null 或 当前折扣不为指定折扣, 直接下一个
                if (nowTimesPriceRefObj == null || nowTimesPriceRefObj.discount != _discount)
                {
                    i++;
                    continue;
                }

                int nextIndex = i + 1;//下一个TimesPriceRefObj下标
                // 找下一个不为null的TimesPriceRefObj
                while (nextIndex < _m_lAllTimesPriceRefObjList.Count)
                {
                    nextTimesPriceRefObj = _m_lAllTimesPriceRefObjList[nextIndex];
                    if(nextTimesPriceRefObj != null)
                        break;
                    
                    nextIndex++;
                }

                // 若找不到下一个TimesPriceRefObj了, 说明当前折扣是最后一个折扣, 剩余折扣次数为无穷大
                if (nextTimesPriceRefObj == null)
                {
                    leftDiscountCount = long.MaxValue;
                    break;
                }

                // 若下一个TimesPriceRefObj的折扣需要的购买次数 比 当前已经购买次数小时, 说明已经过了本次遍历的购买折扣阶段
                if (nextTimesPriceRefObj.times <= nowTravelCount)
                {
                    i = nextIndex;
                    continue;
                }
                
                if (nowTimesPriceRefObj.times > nowTravelCount)
                {
                    leftDiscountCount += nextTimesPriceRefObj.times - nowTimesPriceRefObj.times;
                }
                else
                {
                    leftDiscountCount += nextTimesPriceRefObj.times - nowTravelCount;
                }
                
                i = nextIndex;
            }

            return leftDiscountCount;
        }

        #endregion
    }
}