using ALPackage;
using Common.ActivityEnum;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 充值返利主页面
    /// </summary>
    public class GGUIWndRechargeRebatePage : _ATALBasicLoadPrefabSubUIWnd<GGUIMonoRechargeRebatePage>
    {
        //资源id
        private long _m_lUIResId;
        //充值返利组列表
        private List<RechargeRebateInfo> _m_lRechargeRebateInfoList;
        //当前选中的充值返利组
        private RechargeRebateInfo _m_curSelectInfo;
        //横幅图片
        private NPGGuiWndTexture _m_wBanner;
        //定时任务
        private ALCommonEnableTaskController _m_iTickTask;
        //礼包组标题
        private GGUIWndSubCashGiftPackGroupTitle _m_wGiftPackGroupTitle;
        //页签列表
        private GGUIWndRechargeRebatePageTabContainer _m_wTabContainer;
        //步骤列表
        private GGUIWndRechargeRebatePageStepContainer _m_wStepContainer;
        //当前标题资源id
        private long _m_lCurTitleResId;

        public GGUIWndRechargeRebatePage(long _uiResId, Transform _parent) : base(_parent)
        {
            _m_lUIResId = _uiResId;
        }

        protected override string _monoAssetPath { get { return UIResPathAssistant.getAssetPath(_m_lUIResId); } }
        protected override string _monoObjName { get { return UIResPathAssistant.getObjName(_m_lUIResId); } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_RECHARGE_REBATE_COUNT_CHG, _onCountChg);
            WinMsg.RegisterMsg(WinMsgType.ON_RECHARGE_REBATE_DRAW_CHG, _onDrawRewardChg);
            WinMsg.RegisterMsg(WinMsgType.ON_RECHARGE_REBATE_GROUP_CHG, _onGroupInfoChg);
            WinMsg.RegisterMsg(WinMsgType.ON_RECHARGE_REBATE_ADD, _onAddRemoveRechargeRebate);
            WinMsg.RegisterMsg(WinMsgType.ON_RECHARGE_REBATE_REMOVE, _onAddRemoveRechargeRebate);
            _refreshTabList();
        }
        
        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_RECHARGE_REBATE_COUNT_CHG, _onCountChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_RECHARGE_REBATE_DRAW_CHG, _onDrawRewardChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_RECHARGE_REBATE_GROUP_CHG, _onGroupInfoChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_RECHARGE_REBATE_ADD, _onAddRemoveRechargeRebate);
            WinMsg.UnregisterMsg(WinMsgType.ON_RECHARGE_REBATE_REMOVE, _onAddRemoveRechargeRebate);
            _m_wTabContainer?.hideWnd();
            _m_wStepContainer?.hideWnd();
            _m_wBanner?.hideWnd();
            _m_iTickTask.setDisable();
            _pushBackTitle();
        }
        
        protected override void _onReset()
        {
            _m_wTabContainer?.resetWnd();
            _m_wStepContainer?.resetWnd();
            _m_wBanner?.discardShowTexture();
        }
        
        protected override void _onDiscard()
        {
            _m_wTabContainer?.discard();
            _m_wTabContainer = null;
            _m_wStepContainer?.discard();
            _m_wStepContainer = null;
            _m_wBanner?.discard();
            _m_wBanner = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoTabContainer != null)
            {
                _m_wTabContainer = new GGUIWndRechargeRebatePageTabContainer(wnd.monoTabContainer);
                _m_wTabContainer.onClickItem += _onClickTabItem;
            }

            if (wnd.monoStepContainer != null)
                _m_wStepContainer = new GGUIWndRechargeRebatePageStepContainer(wnd.monoStepContainer);

            if (wnd.imgBanner != null)
                _m_wBanner = new NPGGuiWndTexture(wnd.imgBanner);
        }

        //刷新页签列表
        private void _refreshTabList()
        {
            if (wnd == null)
                return;

            if (_m_lRechargeRebateInfoList == null)
                _m_lRechargeRebateInfoList = new List<RechargeRebateInfo>();
            _m_lRechargeRebateInfoList.Clear();

            //获取可显示的充值返利组列表
            _m_lRechargeRebateInfoList = NPPlayer.instance.rechargeRebateComp.getRechargeRebateInfoList();
            //按id排序
            _m_lRechargeRebateInfoList.Sort((_a,_b)=>_a.groupId.CompareTo(_b.groupId));
            //展示页签列表
            _m_wTabContainer?.showWnd();
            _m_wTabContainer?.showItemList(_m_lRechargeRebateInfoList);
        }

        //刷新页签红点
        private void _refreshTabRedTip()
        {
            _m_wTabContainer?.refreshRedTip();
        }

        //刷新窗口
        private void _refreshWnd(bool _moveToTop)
        {
            _refreshTitle();
            _refreshCount();
            _refreshStepList(_moveToTop);
            _refreshCDTask();
        }

        //刷新标题
        private void _refreshTitle()
        {
            if (wnd == null || _m_curSelectInfo == null || _m_curSelectInfo.groupRefObj == null)
                return;

            //横幅图
            _m_wBanner?.showWnd();
            _m_wBanner?.setTexture(_m_curSelectInfo.groupRefObj.tex_banner);

            //回收标题
            _pushBackTitle();
            //加载新标题
            long titleUiResId = _m_curSelectInfo.groupRefObj.title_ui_res_id;
            if (titleUiResId > 0 && wnd.goTitleParent != null)
            {
                GCashGiftPackGroupTitleCacheMgr.instance.popItem(titleUiResId, wnd.goTitleParent,
                    _item =>
                    {
                        if (!isShow || wnd == null)
                        {
                            GCashGiftPackGroupTitleCacheMgr.instance.pushBackCacheItem(titleUiResId, _item);
                            return;
                        }
                        _item?.showWnd();
                        _item?.setInfo(_m_curSelectInfo.groupRefObj.name);
                        _m_wGiftPackGroupTitle = _item;
                        _m_lCurTitleResId = titleUiResId;
                    });
            }

            //描述
            ALUGUICommon.setLabelTxt(wnd.txtDesc, TextTranslate.instance.getLanguage(_m_curSelectInfo.groupRefObj.desc));
        }

        //刷新计数
        private void _refreshCount()
        {
            if (wnd == null || _m_curSelectInfo == null || _m_curSelectInfo.groupRefObj == null)
                return;

            //获取计数
            long curCount = _m_curSelectInfo.curCount;
            ALUGUICommon.setLabelTxt(wnd.txtCountDesc, TextTranslate.instance.getLanguage(_m_curSelectInfo.groupRefObj.count_desc, curCount));
        }

        //刷新步骤列表
        private void _refreshStepList(bool _moveToTop)
        {
            if (wnd == null || _m_curSelectInfo == null || _m_curSelectInfo.groupRefObj == null)
                return;

            _m_wStepContainer?.showWnd();
            _m_wStepContainer?.showItemList(_m_curSelectInfo);
            if(_moveToTop)
                _m_wStepContainer?.moveToTop();
        }

        //回收标题
        private void _pushBackTitle()
        {
            if (_m_lCurTitleResId > 0)
                GCashGiftPackGroupTitleCacheMgr.instance.pushBackCacheItem(_m_lCurTitleResId, _m_wGiftPackGroupTitle);
            _m_lCurTitleResId = 0;
            _m_wGiftPackGroupTitle = null;
        }

        //刷新倒计时任务
        private void _refreshCDTask()
        {
            _m_iTickTask.setDisable();
            _m_iTickTask = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(_tick, 0.2f);
        }

        //每秒倒计时
        private void _tick()
        {
            if (wnd == null || _m_curSelectInfo == null)
                return;

            long leftTimeMs;

            //获取活动剩余时间
            _ABaseActivityInfo activityInfo = NPPlayer.instance.commonActivityComp.getActivityInfoByInstanceId(_m_curSelectInfo.activityInstanceId);
            if (activityInfo == null || !activityInfo.isPlaying)
                return;
            else
                leftTimeMs = activityInfo.endTimeMs - FpsAndPingMgr.instance.serverTimeTag;

            if (leftTimeMs < 0)
                ALUGUICommon.setLabelTxt(wnd.txtLeftTime, "");
            else
                ALUGUICommon.setLabelTxt(wnd.txtLeftTime, TimeUtil.millisecondsToTime_hms(leftTimeMs, TransKeyConst.giftPack_leftTime_num_num_num));
        }

        //点击页签
        private void _onClickTabItem(GGUIWndRechargeRebatePageTabContainerItem _item, bool _moveToTop)
        {
            if (_item == null || _item.rechargeRebateInfo == null)
                return;

            _m_curSelectInfo = _item.rechargeRebateInfo;
            _refreshWnd(_moveToTop);
        }

        #region 消息事件

        //新增移除充值返利
        private void _onAddRemoveRechargeRebate(params object[] _objects)
        {
            _refreshTabList();
        }
        
        //领取奖励变化
        private void _onDrawRewardChg(params object[] _objects)
        {
            if (_m_curSelectInfo == null || _objects == null || _objects.Length < 2)
                return;

            long groupId = (long)_objects[1];

            if (_m_curSelectInfo.groupId == groupId)
            {
                _refreshStepList(false);
                _refreshTabRedTip();
            }
        }

        //计数变化
        private void _onCountChg(params object[] _objects)
        {
            if (_m_curSelectInfo == null || _objects == null || _objects.Length < 3)
                return;

            long groupId = (long)_objects[1];

            if (_m_curSelectInfo.groupId == groupId)
            {
                _refreshCount();
                _refreshTabRedTip();
            }
        }

        //组信息变化
        private void _onGroupInfoChg(params object[] _objects)
        {
            if (_m_curSelectInfo == null || _objects == null || _objects.Length < 2)
                return;

            long groupId = (long)_objects[1];

            if (_m_curSelectInfo.groupId == groupId)
            {
                _refreshWnd(true);
                _refreshTabRedTip();
            }
        }

        #endregion
    }
}
