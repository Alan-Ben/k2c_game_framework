using System;
using System.Collections.Generic;
using ALPackage;
using Common.ActivityEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 活动合并展示推送弹窗
    /// </summary>
    public class GGUIWndActivityMergeShowPushNotice : _ANPGGUIBasicWnd<GGUIMonoActivityMergeShowPushNotice>
    {
        private static GGUIWndActivityMergeShowPushNotice _g_instance;
        public static GGUIWndActivityMergeShowPushNotice instance { get { return _g_instance ??= new GGUIWndActivityMergeShowPushNotice(); } }

        // 需要展示的活动列表
        private List<KeyValuePair<PushNoticeActivityMergeRefObj, _ABaseActivityInfo>> _m_lShowActivityList;

        // 活动Item列表Grid
        private GGUIWndActivityMergeShowActivityItemGrid _m_wActivityItemGrid;


        public GGUIWndActivityMergeShowPushNotice() : base(EALUIWndLayer.ADDITION)
        {
        }


        protected override string _monoAssetPath { get { return GGUIMonoActivityMergeShowPushNotice.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoActivityMergeShowPushNotice.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        public event Action dealCloseAction;//关闭窗口操作

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            // 构建活动Item列表Grid
            if (wnd.monoActivityItemGrid != null)
            {
                _m_wActivityItemGrid = new GGUIWndActivityMergeShowActivityItemGrid(wnd.monoActivityItemGrid);
                _m_wActivityItemGrid.onGotoBtnClick += _onActivityItemGotoBtnClick;
            }

            // 绑定关闭按钮点击事件
            ALUGUICommon.combineBtnClick(wnd.closeBtn, _onCloseBtnClick);
        }

        protected override void _onDiscard()
        {
            // 解绑关闭按钮点击事件
            if (wnd != null)
                ALUGUICommon.uncombineBtnClick(wnd.closeBtn, _onCloseBtnClick);

            // 释放活动Item列表Grid
            if (_m_wActivityItemGrid != null)
            {
                _m_wActivityItemGrid.onGotoBtnClick -= _onActivityItemGotoBtnClick;
                _m_wActivityItemGrid.discard();
                _m_wActivityItemGrid = null;
            }

            dealCloseAction = null;
            
            _m_lShowActivityList?.Clear();
            _m_lShowActivityList = null;
        }

        protected override void _onShowWnd()
        {
            _updateNeedShowActivityInfo();
            _refreshWnd();
            
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ACTIVITY_ADD, _onActivityChg);
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ACTIVITY_UPDATE, _onActivityChg);
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ACTIVITY_STATE_CHG, _onActivityChg);
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ACTIVITY_CLOSE, _onActivityChg);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ACTIVITY_ADD, _onActivityChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ACTIVITY_UPDATE, _onActivityChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ACTIVITY_STATE_CHG, _onActivityChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ACTIVITY_CLOSE, _onActivityChg);
            
            _m_wActivityItemGrid?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wActivityItemGrid?.resetWnd();
        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            // 刷新活动Item列表
            _m_wActivityItemGrid?.showWnd();
            _m_wActivityItemGrid?.setData(_m_lShowActivityList);
        }

        /// <summary>
        /// 更新需要展示的活动信息
        /// </summary>
        private void _updateNeedShowActivityInfo()
        {
            if (_m_lShowActivityList == null)
                _m_lShowActivityList = new List<KeyValuePair<PushNoticeActivityMergeRefObj, _ABaseActivityInfo>>();
            _m_lShowActivityList.Clear();

            _ABaseActivityInfo activityInfo = null;
            foreach (var pushNoticeActivityMergeRefObj in GRefdataCoreMgr.instance.pushNoticeActivityMergeRefCore.refList)
            {
                if (pushNoticeActivityMergeRefObj == null)
                    continue;

                activityInfo = NPPlayer.instance.commonActivityComp.getValidActivityInfoByActivityId(pushNoticeActivityMergeRefObj.activity_id);
                if (activityInfo != null && activityInfo.isEnable)
                    _m_lShowActivityList.Add(new KeyValuePair<PushNoticeActivityMergeRefObj, _ABaseActivityInfo>(pushNoticeActivityMergeRefObj, activityInfo));
            }

            // 按sorting_order升序排序
            _m_lShowActivityList.Sort((_a, _b) =>
            {
                if (_b.Key == null || _b.Value == null) return -1;
                if (_a.Key == null || _a.Value == null) return 1;

                int stateSort = _getActivityStateSortOrder(_a.Value.activityState).CompareTo(_getActivityStateSortOrder(_b.Value.activityState));
                if(stateSort != 0)
                    return stateSort;
                // 到这里时, 两个活动的状态一定相同

                //若活动处于未开启, 根据到开启的时间排序
                if (_a.Value.activityState == EActivityState.PLAN)
                {
                    int activityStartTimeCompare = _a.Value.startTimeMs.CompareTo(_b.Value.startTimeMs);
                    if(activityStartTimeCompare != 0)
                        return activityStartTimeCompare;
                }
                
                // 若活动处于进行中, 根据到结算的时间排序
                if (_a.Value.activityState == EActivityState.PLAYING)
                {
                    int activityEndTimeCompare = _a.Value.settleTimeMs.CompareTo(_b.Value.settleTimeMs);
                    if(activityEndTimeCompare != 0)
                        return activityEndTimeCompare;
                }
                
                // 若活动处于结算中, 根据到领奖期的时间排序
                if (_a.Value.activityState == EActivityState.SETTLING)
                {
                    int activityEndTimeCompare = _a.Value.endTimeMs.CompareTo(_b.Value.endTimeMs);
                    if(activityEndTimeCompare != 0)
                        return activityEndTimeCompare;
                }
                
                // 若活动处于领奖中, 根据到结束的时间排序
                if (_a.Value.activityState == EActivityState.REWARDING)
                {
                    int activityEndTimeCompare = _a.Value.closeTimeMs.CompareTo(_b.Value.closeTimeMs);
                    if(activityEndTimeCompare != 0)
                        return activityEndTimeCompare;
                }

                return _a.Key.sorting_order.CompareTo(_b.Key.sorting_order);
            });
        }

        /// <summary>
        /// 关闭按钮点击回调
        /// </summary>
        private void _onCloseBtnClick(GameObject _)
        {
            dealCloseAction?.Invoke();
        }

        /// <summary>
        /// 活动Item前往按钮点击回调
        /// </summary>
        private void _onActivityItemGotoBtnClick(GGUIWndActivityMergeShowActivityItem _itemWnd)
        {
            if (_itemWnd == null)
                return;

            PushNoticeActivityMergeRefObj refObj = _itemWnd.pushNoticeActivityMergeRefObj;

            if (refObj == null || refObj.go_to_effect == null || refObj.go_to_effect.isEmpty)
                return;
            
            refObj.go_to_effect.dealEffect();//执行前往效果
        }
        
        //活动变更
        private void _onActivityChg(params object[] _objects)
        {
            if (_objects == null || _objects.Length < 1 || !(_objects[0] is long activityId))
                return;

            foreach (var pushNoticeActivityMergeRefObj in GRefdataCoreMgr.instance.pushNoticeActivityMergeRefCore.refList)
            {
                if(pushNoticeActivityMergeRefObj != null && pushNoticeActivityMergeRefObj.activity_id == activityId)
                {
                    _updateNeedShowActivityInfo();
                    _refreshWnd();
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.activity_stateUpdateTip_none);
                    break;
                }
            }
        }
        
        private int _getActivityStateSortOrder(EActivityState _state)
        {
            switch (_state)
            {
                case EActivityState.PLAYING:
                    return 0;
                case EActivityState.REWARDING:
                    return 1;
                case EActivityState.SETTLING:
                    return 2;
                default:
                    return 999;
            }
        }
    }
}