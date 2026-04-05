using ALPackage;
using Common.ActivityEnum;
using Common.ActivityObj;
using CommonEnum;
using GOE;
using Hotfix.GC2GS.p200_HotSimpleActivityOp;
using Hotfix.GS2GC.p200_HotSimpleActivityOp;
using JetBrains.Annotations;
using NPCommon;
using System;
using System.Collections.Generic;

namespace Hotfix
{
    /// <summary>
    /// 万能活动组件
    /// </summary>
    public class RegularEventComponent : HotfixBaseComponent
    {
        //万能活动id列表
        [NotNull] private List<long> _m_activityIdList = new List<long>();
        //万能活动消耗商店列表
        [NotNull] private Dictionary<long, RegularEventShop> _m_lRegularEventShopDic = new Dictionary<long, RegularEventShop>();
        //定时任务
        private ALCommonEnableTaskController _m_iCheckTask;
        //是否使用十次
        private bool _m_bUseTen;

        /// <summary>
        /// 是否使用十次
        /// </summary>
        public bool useTen { get { return _m_bUseTen; } set { _m_bUseTen = value; } }

        public RegularEventComponent()
        {
        }

        protected override void _dealInit()
        {
            //获取万能活动的活动id
            List<GActivityMainRefObj> activityMainRefList = GRefdataCoreMgr.instance.activityMainRefCore.makeNewAllRefList();
            if (activityMainRefList != null)
            {
                for (int i = 0; i < activityMainRefList.Count; i++)
                {
                    if (activityMainRefList[i] != null && activityMainRefList[i].type_id == ECommonActivityType.REGULAR_EVENT)
                        _m_activityIdList.Add(activityMainRefList[i].activity_id);
                }
            }

            //获取万能活动的活动信息列表
            List<_ABaseActivityInfo> activityInfoList = NPPlayer.instance.commonActivityComp.getValidActivityListInfoByType(ECommonActivityType.REGULAR_EVENT);

            _m_lRegularEventShopDic.Clear();
            if (null == activityInfoList || activityInfoList.Count <= 0)
            {
                setInitDone();
            }
            else
            {
                ALStepCounter stepCounter = new ALStepCounter();
                stepCounter.chgTotalStepCount(activityInfoList.Count);
                stepCounter.regAllDoneDelegate(setInitDone);

                for (int i = 0; i < activityInfoList.Count; i++)
                {
                    //请求对应活动的商店信息
                    reqRegularActivityShopInfo(activityInfoList[i], stepCounter.addDoneStepCount);
                }
            }
        }

        protected override void _onInitDone()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ACTIVITY_ADD, _onActivityAdd);
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ACTIVITY_STATE_CHG, _onActivityStateChg);
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ACTIVITY_CLOSE, _onActivityClose);

            //刷新红点
            _refreshWarehouseRedTip();
            _refreshFreeBuyRedTip();
            _startCheckTask();
        }

        protected override void _onDiscard()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ACTIVITY_ADD, _onActivityAdd);
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ACTIVITY_STATE_CHG, _onActivityStateChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ACTIVITY_CLOSE, _onActivityClose);
            _m_activityIdList.Clear();
            _m_lRegularEventShopDic.Clear();
            _stopCheckTask();
        }

        protected override void _onInitFail()
        {
        }

        /// <summary>
        /// 根据活动实例id获取万能活动商店
        /// </summary>
        /// <param name="_activityInstanceId"></param>
        /// <returns></returns>
        public RegularEventShop getRegularEventShopByInstanceId(long _activityInstanceId)
        {
            if (_m_lRegularEventShopDic.TryGetValue(_activityInstanceId, out RegularEventShop _shop))
                return _shop;
            return null;
        }

        /// <summary>
        /// 根据活动id获取万能活动商店
        /// </summary>
        /// <param name="_activityId"></param>
        /// <returns></returns>
        public RegularEventShop getRegularEventShopByActivityId(long _activityId)
        {
            //取第一个活动
            _ABaseActivityInfo activityInfo = NPPlayer.instance.commonActivityComp.getValidActivityInfoByActivityId(_activityId);
            if(activityInfo == null)
                return null;

            if (_m_lRegularEventShopDic.TryGetValue(activityInfo.instanceId, out RegularEventShop _shop))
                return _shop;
            return null;
        }

        /// <summary>
        /// 刷新仓库红点
        /// </summary>
        private void _refreshWarehouseRedTip()
        {
            for (int i = 0; i < _m_activityIdList.Count; i++)
            {
                long redTipCount = 0;
                long activityId = _m_activityIdList[i];
                _ABaseActivityInfo activityInfo = NPPlayer.instance.commonActivityComp.getValidActivityInfoByActivityId(activityId);
                if (activityInfo == null || !activityInfo.isPlaying)
                {
                    RedTipMgr.instance.setCountByRefRedTipId(HotfixRedTipConst.REGULAR_EVENT_WAREHOUSE.replaceActivity(activityId), redTipCount);
                    continue;
                }

                HotfixRefdataCoreMgr.instance.regularEventShopItemRefCore.dealAllRef(_ref =>
                {
                    if (_ref != null && _ref.activity_id == activityId && _ref.item != null &&
                        GCommon.isItemEnough(_ref.item.getItemType(), _ref.item.subId, 1, false))
                        redTipCount++;
                });
                RedTipMgr.instance.setCountByRefRedTipId(HotfixRedTipConst.REGULAR_EVENT_WAREHOUSE.replaceActivity(activityId), redTipCount);
            }
        }

        /// <summary>
        /// 刷新消耗商店免费购买红点
        /// </summary>
        private void _refreshFreeBuyRedTip()
        {
            for (int i = 0; i < _m_activityIdList.Count; i++)
            {
                long redTipCount = 0;
                long activityId = _m_activityIdList[i];
                _ABaseActivityInfo activityInfo = NPPlayer.instance.commonActivityComp.getValidActivityInfoByActivityId(activityId);
                if (activityInfo == null || !activityInfo.isPlaying)
                {
                    RedTipMgr.instance.setCountByRefRedTipId(HotfixRedTipConst.REGULAR_EVENT_FREE_BUY.replaceActivity(activityId), redTipCount);
                    continue;
                }

                if (_m_lRegularEventShopDic.TryGetValue(activityInfo.instanceId, out RegularEventShop _shop))
                {
                    redTipCount = _shop.getFreeBuyCount();
                }
                RedTipMgr.instance.setCountByRefRedTipId(HotfixRedTipConst.REGULAR_EVENT_FREE_BUY.replaceActivity(activityId), redTipCount);
            }
        }

        #region 刷新商店任务

        //开始检查任务
        private void _startCheckTask()
        {
            _m_iCheckTask.setDisable();
            _m_iCheckTask = CommonTaskController.CommonEnableDurationActionAddMonoTask(_checkNeedRefreshShop, 1.0f);
        }

        //停止检查任务
        private void _stopCheckTask()
        {
            _m_iCheckTask.setDisable();
        }

        //检查是否需要刷新商店
        private void _checkNeedRefreshShop()
        {
            foreach (RegularEventShop regularEventShop in _m_lRegularEventShopDic.Values)
            {
                regularEventShop?.checkRefresh();
            }
        }

        #endregion

        #region 消息事件

        /// <summary>
        /// 新增活动
        /// </summary>
        /// <param name="_objects"></param>
        private void _onActivityAdd(params object[] _objects)
        {
            if (_objects == null || _objects.Length < 2)
                return;

            long activityId = (long)_objects[0];
            long activityInstanceId = (long)_objects[1];

            if (_m_activityIdList.Contains(activityId))
            {
                _ABaseActivityInfo activityInfo = NPPlayer.instance.commonActivityComp.getActivityInfoByInstanceId(activityInstanceId);
                reqRegularActivityShopInfo(activityInfo, null);

                //刷新红点
                _refreshWarehouseRedTip();
                _refreshFreeBuyRedTip();
            }
        }

        /// <summary>
        /// 活动状态变更
        /// </summary>
        /// <param name="_objects"></param>
        private void _onActivityStateChg(params object[] _objects)
        {
            if (_objects == null || _objects.Length < 4)
                return;

            long activityId = (long)_objects[0];
            long activityInstanceId = (long)_objects[1];
            EActivityState oriState = (EActivityState)_objects[2];
            EActivityState curState = (EActivityState)_objects[3];

            if (_m_activityIdList.Contains(activityId))
            {
                if(curState == EActivityState.PLAYING)
                {
                    _ABaseActivityInfo activityInfo = NPPlayer.instance.commonActivityComp.getActivityInfoByInstanceId(activityInstanceId);
                    reqRegularActivityShopInfo(activityInfo, null);
                }

                //刷新红点
                _refreshWarehouseRedTip();
                _refreshFreeBuyRedTip();
            }
        }

        /// <summary>
        /// 活动关闭
        /// </summary>
        /// <param name="_objects"></param>
        private void _onActivityClose(params object[] _objects)
        {
            if (_objects == null || _objects.Length < 2)
                return;

            long activityId = (long)_objects[0];
            long activityInstanceId = (long)_objects[1];

            if (_m_lRegularEventShopDic.ContainsKey(activityInstanceId))
            {
                _m_lRegularEventShopDic.Remove(activityInstanceId);

                //刷新红点
                _refreshWarehouseRedTip();
                _refreshFreeBuyRedTip();
            }
        }

        #endregion

        #region S2C

        /// <summary>
        /// 万能活动商店购买记录变更
        /// </summary>
        /// <param name="_msg"></param>
        public void onRegularActivityShopItemBuy(GS2GC_200_101_OnRegularActivityShopItemBuy _msg)
        {
            if (_msg == null)
                return;

            if(_m_lRegularEventShopDic.TryGetValue(_msg.getActivityInstanceId(), out RegularEventShop _shop))
                _shop?.updateBuyRecord(_msg.getBuyRecord());

            //刷新红点
            _refreshFreeBuyRedTip();

            ALMsgSys.SendMsg(HotfixMsgType.ON_REGULAR_EVENT_SHOP_ITEM_CHG, _msg.getActivityInstanceId());
        }

        /// <summary>
        /// 万能活动商店刷新
        /// </summary>
        /// <param name="_msg"></param>
        public void onRegularActivityShopRefresh(GS2GC_200_102_OnRegularActivityShopRefresh _msg)
        {
            if (_msg == null)
                return;

            if (_m_lRegularEventShopDic.TryGetValue(_msg.getActivityInstanceId(), out RegularEventShop _shop))
            {
                _shop?.resetBuyCount();
                _shop?.updateInfo(_msg.getShopInfo());
            }

            //刷新红点
            _refreshFreeBuyRedTip();

            ALMsgSys.SendMsg(HotfixMsgType.ON_REGULAR_EVENT_SHOP_ITEM_REFRESH, _msg.getActivityInstanceId());
        }

        #endregion


        #region C2S

        /// <summary>
        /// 请求初始化
        /// </summary>
        public void reqRegularActivityShopInfo(_ABaseActivityInfo _activityInfo, Action _callback)
        {
            if (_activityInfo == null || (!_activityInfo.isEnable) )
            {
                _callback?.Invoke();
                return;
            }

            long activityId = _activityInfo.activityId;
            long activityInstanceId = _activityInfo.instanceId;
            NPGSClientListener.sendRequestByLog(new GC2GS_200_001_ReqRegularActivityShopInfo(activityInstanceId),
                new HotfixCommonRequestSucFailSameCallbackProtocolDealer<GS2GC_200_001_RetRegularActivityShopInfo>((_isSuc,_msg) =>
                {
                    if (_isSuc)
                        _m_lRegularEventShopDic[activityInstanceId] = new RegularEventShop(activityId, activityInstanceId, _msg.getShopInfo());

                    //刷新红点
                    _refreshFreeBuyRedTip();

                    if (_callback != null)
                        _callback();
                }));
        }

        /// <summary>
        /// 请求万能活动商店购买物品
        /// </summary>
        /// <param name="_activityInstanceId"></param>
        /// <param name="_itemId"></param>
        /// <param name="_num"></param>
        /// <param name="_callback"></param>
        public void reqRegularActivityShopBuyItem(long _activityInstanceId, long _itemId, int _num, Action _callback = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_200_002_ReqRegularActivityShopBuyItem(_activityInstanceId, _itemId, _num),
                new HotfixCommonErrCodeRequestCallbackProtocolDealer<GS2GC_200_002_RetRegularActivityShopBuyItem>(_msg =>
                {
                    //刷新红点
                    _refreshWarehouseRedTip();

                    if (_callback != null)
                        _callback();
                }));
        }

        /// <summary>
        /// 请求万能活动使用物品
        /// </summary>
        /// <param name="_activityInstanceId"></param>
        /// <param name="_itemId"></param>
        /// <param name="_isTen"></param>
        /// <param name="_callback"></param>
        public void reqRegularActivityUseItem(long _activityInstanceId, long _itemId, bool _isTen, Action<bool, List<NPCommon_ItemInfo>> _callback = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_200_003_ReqRegularActivityUseItem(_activityInstanceId, _itemId, _isTen),
                new HotfixCommonRequestSucFailSameCallbackProtocolDealer<GS2GC_200_003_RetRegularActivityUseItem>((_isSuc, _msg) =>
                {
                    //刷新红点
                    _refreshWarehouseRedTip();

                    if (_callback != null)
                        _callback(_isSuc, _msg.getItemList());
                }));
        }

        /// <summary>
        /// 请求万能活动商店刷新
        /// </summary>
        /// <param name="_callback"></param>
        public void reqRegularActivityShopRefresh(long _activityInstanceId, Action _callback = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_200_004_ReqRegularActivityShopRefresh(_activityInstanceId),
                new HotfixCommonRequestSucFailSameCallbackProtocolDealer<GS2GC_200_004_RetRegularActivityShopRefresh>((_isSuc, _msg) =>
                {
                    if (_callback != null)
                        _callback();
                }));
        }

        #endregion

    }
}