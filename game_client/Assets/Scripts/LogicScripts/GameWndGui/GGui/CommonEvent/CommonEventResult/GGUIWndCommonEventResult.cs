using System;
using System.Collections.Generic;
using ALPackage;
using CommonEnum;
using NPCommon;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 事件处理结果
    /// </summary>
    public class GGUIWndCommonEventResult : _AGGUIWndEventResult<GGUIMonoCommonEventResult>
    {
        public static GGUIWndCommonEventResult _g_instance;
        public static GGUIWndCommonEventResult instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GGUIWndCommonEventResult();
                return _g_instance;
            }
        }

        private const int _m_iPlayerExpItemType = (int) ENPItemType.CURRENCY;
        private const long _m_lPlayerExpItemSubId = (long) ECurrency.P_EXP;

        private GGUIWndPlayerExpAddSld _m_wPlayerExpAddSld;//玩家经验增加进度条
        private NPGGUIWndCommonItemContainer _m_wRewardContainer;//奖励列表
        private NPGGUIWndCommonCountDown _m_wAutoCloseCountDown;//自动关闭倒计时

        private CommonEventShowResultInfo _m_CommonEventShowInfo;//事件结果显示信息
        private List<NPCommon.NPCommon_ItemInfo> _m_lRewardList;//奖励列表
        private NPCommon.NPCommon_ItemInfo _m_iExpItem;//经验

        private long _m_lCloseWndDelayTaskSerializeId;
        private Action _m_aDoCloseWnd;//关闭窗口操作
        
        public GGUIWndCommonEventResult() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoCommonEventResult.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoCommonEventResult.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        protected override void _onInitDoneSub()
        {
            if (wnd == null)
                return;

            if (wnd.monoPlayerExpAddSld != null)
                _m_wPlayerExpAddSld = new GGUIWndPlayerExpAddSld(wnd.monoPlayerExpAddSld);

            if (wnd.monoRewardContainer != null)
                _m_wRewardContainer = new NPGGUIWndCommonItemContainer(wnd.monoRewardContainer);

            if (wnd.autoCloseCountDownMono != null)
                _m_wAutoCloseCountDown = new NPGGUIWndCommonCountDown(wnd.autoCloseCountDownMono);
        }

        protected override void _onDiscardSub()
        {
            _m_wRewardContainer?.discard();
            _m_wRewardContainer = null;
            
            _m_wPlayerExpAddSld?.discard();
            _m_wPlayerExpAddSld = null;
            
            _m_wAutoCloseCountDown?.discard();
            _m_wAutoCloseCountDown = null;
        }

        protected override void _onShowWndSub()
        {
        }

        protected override void _onHideWndSub()
        {
            _m_wRewardContainer?.hideWnd();
            _m_wPlayerExpAddSld?.hideWnd();
            _m_wAutoCloseCountDown?.hideWnd();

            _m_lCloseWndDelayTaskSerializeId = ALSerializeOpMgr.next();
        }

        protected override void _onResetSub()
        {
            _m_wRewardContainer?.resetWnd();
            _m_wPlayerExpAddSld?.resetWnd();
            _m_wAutoCloseCountDown?.resetWnd();
        }
        
        protected override void _onSetEventData()
        {
            if (_m_iEventResultShowInfo is CommonEventShowResultInfo)
                _m_CommonEventShowInfo = _m_iEventResultShowInfo as CommonEventShowResultInfo;
            else
                _m_CommonEventShowInfo = null;
            
            _m_lRewardList = _m_CommonEventShowInfo?.rewardList;
            _filterExp();
        }

        public void setData(_IEventResultShowInfo _resultShowInfo, Action _doCloseWnd)
        {
            setData(_resultShowInfo);
            _m_aDoCloseWnd = _doCloseWnd;

            // 【优化-1】关卡-通用事件完成窗口-上浮提示奖励可以去掉了
            // https://www.teambition.com/task/6699e5ca1a51f8787769aaa7
            // if (_resultShowInfo != null)
            // {
            //     if (wnd == null || wnd.rewardTipShowDelay <= 0 || wnd.rewardTipShowDelay > wnd.autoCloseCountDown * 2)
            //     {
            //         GCommon.showGainRewardTip(_resultShowInfo.rewardList, true);
            //     }
            //     else
            //     {
            //         ALCommonTaskController.CommonActionAddMonoTask(() =>
            //         {
            //             GCommon.showGainRewardTip(_resultShowInfo.rewardList, true);
            //         }, wnd.rewardTipShowDelay);
            //     }
            // }
        }
        
        /// <summary>
        /// 过滤经验道具
        /// </summary>
        private void _filterExp()
        {
            if(_m_lRewardList == null)
                return;

            if (_m_iExpItem == null)
                _m_iExpItem = new NPCommon_ItemInfo();
            _m_iExpItem.setItemType(_m_iPlayerExpItemType);
            _m_iExpItem.setSubId(_m_lPlayerExpItemSubId);
            _m_iExpItem.setCount(0);
            
            NPCommon.NPCommon_ItemInfo item = null;
            for (int i = _m_lRewardList.Count - 1; i >= 0; i--)
            {
                item = _m_lRewardList[i];
                if (item == null)
                {
                    _m_lRewardList.RemoveAt(i);
                    continue;
                }

                if (item.getItemType() == _m_iPlayerExpItemType && item.getSubId() == _m_lPlayerExpItemSubId)
                {
                    _m_iExpItem.setCount(_m_iExpItem.getCount() + item.getCount());
                }
            }
        }

        protected override void _refreshWndSub()
        {
            if (wnd == null || wnd.autoCloseCountDown <= 0)//关闭窗口倒计时小于0, 直接关闭
            {
                _closeBtnClick(null);
                return;
            }
            
            if (_m_CommonEventShowInfo != null && wnd != null)
            {
                ALUGUICommon.setLabelTxt(wnd.txtResultTitle, TextTranslate.instance.getLanguage(_m_CommonEventShowInfo.resultTitle));
                ALUGUICommon.setLabelTxt(wnd.txtResultDesc, TextTranslate.instance.getLanguage(_m_CommonEventShowInfo.resultDesc));
            }

            if (_m_wPlayerExpAddSld != null)
            {
                _m_wPlayerExpAddSld.showWnd();
                _m_wPlayerExpAddSld.setAddExpWithSldChg(_m_iExpItem?.getCount() ?? 0);
            }
            
            if (_m_wRewardContainer != null && _m_lRewardList != null)
            {
                _m_wRewardContainer.showWnd();
                _m_wRewardContainer.showItemList(NPCommonCostItem.switchList(_m_lRewardList));
            }

            
            if (_m_wAutoCloseCountDown != null)
            {
                _m_wAutoCloseCountDown.showWnd();
                _m_wAutoCloseCountDown.setInfo(wnd.autoCloseCountDown, TransKeyConst.common_closeCountDown_tip, () =>
                {
                    _closeBtnClick(null);
                });
            }
            else
            {
                long serializeId = _m_lCloseWndDelayTaskSerializeId;
                ALCommonTaskController.CommonActionAddMonoTask(() =>
                {
                    if(serializeId != _m_lCloseWndDelayTaskSerializeId)
                        return;
                    
                    _closeBtnClick(null);
                }, wnd.autoCloseCountDown);
            }
        }

        protected override void _closeBtnClick(GameObject _go)
        {
            if(_m_aDoCloseWnd == null)
                QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_CommonEvent.C_COMMON_EVENT_RESULT_NODE);
            else
                _m_aDoCloseWnd?.Invoke();
        }

        public static void showCommonEventResultNotice(CommonEventShowResultInfo _resultShowInfo, Action _startShowResultWnd, Action _onShowDone, bool _onNoNormalRewardNeedShowResultWnd)
        {
            if(_resultShowInfo == null)
            {
                _onShowDone?.Invoke();
                return;
            }
            CommonRewardDealer.showSpecial(_resultShowInfo.gainItemFilterData, null);
            
            // 若有普通奖励需要显示 或 没有普通奖励但是也要显示结果弹窗时，展示结果弹窗
            if((_resultShowInfo.rewardList != null && _resultShowInfo.rewardList.Count > 0) || _onNoNormalRewardNeedShowResultWnd)
            {
                NPUINoticeMgr.instance.addDealer(new NPNoticeDealer_CommonEventResult(_resultShowInfo, _startShowResultWnd, null));
            }
            
            // Notice展示都完成后调用
            NPUINoticeMgr.instance.addDoneDelegate(() =>
            {
                _onShowDone?.Invoke();
            });
        }
    }
}