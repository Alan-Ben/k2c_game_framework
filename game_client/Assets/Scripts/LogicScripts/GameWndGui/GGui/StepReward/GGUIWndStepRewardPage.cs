using System.Collections.Generic;
using ALPackage;
using Common.ActivityEnum;
using UnityEngine;

namespace GOE
{

    /// <summary>
    /// 阶段奖励页面
    /// </summary>
    public class GGUIWndStepRewardPage : _ATALBasicLoadPrefabSubUIWnd<GGUIMonoStepRewardPage>
    {
        private string _m_sAssetPath;//窗口对象的资源 加载路径
        private string _m_sObjName;//窗口对象的资源 名字
        
        private GGUIWndStepRewardTabItemContainer _m_wTabItemContainer;//tab列表
        private GGUIWndStepRewardItemGrid _m_wRewardGrid;//奖励列表

        private List<ActivityStepRewardInfo> _m_lStepRewardInfoList = new List<ActivityStepRewardInfo>();
        private ActivityStepRewardInfo _m_curStepRewardInfo = null;
        
        private int _m_timeDownSer;

        public GGUIWndStepRewardPage(NPCommonAssetPathInfo _assetPathInfo, Transform _parent) : base(_parent)
        {
            _m_sAssetPath = _assetPathInfo.asset_path;
            _m_sObjName = _assetPathInfo.obj_name;
        }

        protected override string _monoAssetPath { get { return _m_sAssetPath; } }
        protected override string _monoObjName { get { return _m_sObjName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        protected override void _onShowWnd()
        {
            
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ACTIVITY_ADD, _onActivityChg);
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ACTIVITY_UPDATE, _onActivityChg);
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ACTIVITY_STATE_CHG, _onActivityChg);
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ACTIVITY_CLOSE, _onActivityChg);
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ACTIVITY_STEP_REWARD_SCORE_CHG, _onGetStateChg);
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ACTIVITY_STEP_REWARD_GET, _onGetStateChg);
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ACTIVITY_STEP_REWARD_ONE_KEY_GET, _onGetStateChg);
            _m_curStepRewardInfo = null;
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ACTIVITY_ADD, _onActivityChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ACTIVITY_UPDATE, _onActivityChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ACTIVITY_STATE_CHG, _onActivityChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ACTIVITY_CLOSE, _onActivityChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ACTIVITY_STEP_REWARD_SCORE_CHG, _onGetStateChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ACTIVITY_STEP_REWARD_GET, _onGetStateChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ACTIVITY_STEP_REWARD_ONE_KEY_GET, _onGetStateChg);
            _m_curStepRewardInfo = null;
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            _m_wTabItemContainer?.discard();
            _m_wTabItemContainer = null;
            _m_wRewardGrid?.discard();
            _m_wRewardGrid = null;

            if (wnd == null) 
                return;
            ALUGUICommon.combineBtnClick(wnd.btnOneKeyGet, _onBtnOneKeyGet);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            if(wnd.tabItemContainer != null)
                _m_wTabItemContainer = new GGUIWndStepRewardTabItemContainer(wnd.tabItemContainer, _onClickTab);
            if(wnd.itemGrid != null)
                _m_wRewardGrid = new GGUIWndStepRewardItemGrid(wnd.itemGrid, _onClickGoTo, _onClickGet);
            ALUGUICommon.combineBtnClick(wnd.btnOneKeyGet, _onBtnOneKeyGet);
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if(wnd == null)
                return;

            NPPlayer.instance.commonActivityComp.getActivityStepRewardInfoList(_m_lStepRewardInfoList, (_info) =>
            {
                return _info != null && _info.isShowInStepRewardWnd;
            });
            
            if (_m_lStepRewardInfoList.Count > 0)
            {
                //之前选中的阶段奖励是否还存在
                bool isExist = false;
                if (_m_curStepRewardInfo != null)
                {
                    for (int i = 0; i < _m_lStepRewardInfoList.Count; i++)
                    {
                        if (_m_lStepRewardInfoList[i] != null && _m_lStepRewardInfoList[i].stepRewardSetId  == _m_curStepRewardInfo.stepRewardSetId)
                        {
                            isExist = true;
                            break;
                        }
                    }
                }

                // 还没有选中 或者 之前选中的阶段奖励不存在 则选中第一个可领取的阶段奖励
                if (_m_curStepRewardInfo == null || !isExist)
                {
                    for (int i = 0; i < _m_lStepRewardInfoList.Count; i++)
                    {
                        if (_m_lStepRewardInfoList[i] != null && _m_lStepRewardInfoList[i].getCanGetRewardCount() > 0)
                        {
                            _m_curStepRewardInfo = _m_lStepRewardInfoList[i];
                            break;
                        }
                    }
                }

                //如果还是没有有奖励的 则选中第一个
                if (_m_curStepRewardInfo == null)
                    _m_curStepRewardInfo = _m_lStepRewardInfoList[0];
            }
            else
            {
                _m_curStepRewardInfo = null;
            }

            _updatePage(true);
        }
        /// <summary>
        /// 刷新选中的阶段奖励页面
        /// </summary>
        private void _updatePage(bool _needMoveItem = false)
        {
            if(wnd == null)
                return;
            _m_wTabItemContainer?.showWnd();
            _m_wTabItemContainer?.showItemList(_m_lStepRewardInfoList);
            _m_wTabItemContainer?.setSelect(_m_curStepRewardInfo, _needMoveItem);

            bool isShowGoTo = _m_curStepRewardInfo != null &&
                              _m_curStepRewardInfo.stepRewardSetRef != null &&
                              _m_curStepRewardInfo.stepRewardSetRef.go_to != null &&
                              !_m_curStepRewardInfo.stepRewardSetRef.go_to.isEmpty; 
            
            _m_wRewardGrid?.showItemList(_m_curStepRewardInfo, isShowGoTo );
            ALUGUICommon.setGameObjEnable(wnd.showOnCanOneKeyGet, _m_curStepRewardInfo?.getCanGetRewardCount() > 0);
            _m_timeDownSer = ALSerializeOpMgr.next();
            _refreshTimeDown(_m_timeDownSer);
        }

        /// <summary>
        /// 刷新倒计时
        /// </summary>
        private void _refreshTimeDown(int _timeDownSer)
        {
            if (null == wnd)
                return;
            if (null == _m_curStepRewardInfo)
                return;

            _ABaseActivityInfo activityInfo = NPPlayer.instance.commonActivityComp.getActivityInfoByInstanceId(_m_curStepRewardInfo.activityInstanceId);
            if (activityInfo == null)
                return;

            string timeStr = TimeUtil.millisecondsToTime_hms(activityInfo.closeTimeMs - FpsAndPingMgr.instance.serverTimeTag);
        
            ALUGUICommon.setLabelTxt(wnd.txtCountDown, timeStr);
            if (_timeDownSer != _m_timeDownSer)
                return;
            ALCommonTaskController.CommonActionAddMonoTask(() =>
            {
                _refreshTimeDown(_timeDownSer);
            },1f);
        }
        private void _onClickTab(GGUIWndStepRewardTabItem _item)
        {
            if (_item == null || _m_curStepRewardInfo == _item.stepRewardInfo)
                return;
            _m_curStepRewardInfo = _item.stepRewardInfo;
            _updatePage();
        }
        private void _onClickGoTo()
        {
            if (_m_curStepRewardInfo == null || _m_curStepRewardInfo.stepRewardSetRef == null || _m_curStepRewardInfo.stepRewardSetRef.go_to == null || _m_curStepRewardInfo.stepRewardSetRef.go_to.isEmpty)
                return;
            _m_curStepRewardInfo.stepRewardSetRef.go_to.dealEffect();
        }
        private void _onClickGet(GActivityStepRewardRefObj _stepRef)
        {
            if (_m_curStepRewardInfo == null || _stepRef == null || _m_curStepRewardInfo.activityInstanceId <= 0)
                return;
            NPPlayer.instance.commonActivityComp.reqDrawActivityStepReward(_m_curStepRewardInfo.activityInstanceId,
                _m_curStepRewardInfo.stepRewardSetId, _stepRef.step, (_isSuc, _msg) =>
                {
                    _updatePage();
                });

        }

        private void _onBtnOneKeyGet(GameObject _)
        {
            if (_m_curStepRewardInfo == null || _m_curStepRewardInfo.activityInstanceId <= 0)
                return;
            NPPlayer.instance.commonActivityComp.reqAKeyDrawActivityStepReward(_m_curStepRewardInfo.activityInstanceId,
                _m_curStepRewardInfo.stepRewardSetId, (_isSuc, _msg) =>
                {
                    _updatePage();
                });
            _updatePage();
        }
        
        
        //活动变更
        private void _onActivityChg(params object[] _objects)
        {
            if (_objects == null || _objects.Length < 2)
                return;

            long activityId = (long) _objects[0];
            GActivityMainRefObj activityMainRefObj = GRefdataCoreMgr.instance.activityMainRefCore.getRef(activityId);
            
            //是否是冲榜活动
            if(activityMainRefObj != null && activityMainRefObj.step_reward_set_id_list != null && activityMainRefObj.step_reward_set_id_list.Count > 0)
                _refreshWnd();
        }

        private void _onStepRewardScoreChg(params object[] _objects)
        {
            _refreshWnd();
        }
        private void _onGetStateChg(params object[] _objects)
        {
            _updatePage();
        }
    }
}