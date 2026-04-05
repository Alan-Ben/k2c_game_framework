using ALPackage;
using NPEnum;
using UnityEngine.Pool;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 建筑红点HUD控制器
    /// </summary>
    public class GGUIWndMarsBuildingRedTipHUDFollowerController : _ATALGGUICommonFollowItemController<GGUIMonoMarsBuildingRedTipHUD, GGUIWndMarsBuildingRedTipHUDFollower>
    {
        private readonly GResPathIndex _m_resIndex;
        private _IMarsBuildingView _m_buildingView;


        public GGUIWndMarsBuildingRedTipHUDFollowerController()
        {
            // TODO: 需要替换为正确的UI资源ID
            _m_resIndex = new GResPathIndex(7193);
        }
        public GGUIWndMarsBuildingRedTipHUDFollowerController(GResPathIndex _resIndex)
        {
            _m_resIndex = _resIndex;
        }


        public override _AALBasicLoadResIndexInfo followItemIndex { get { return _m_resIndex; } }


        protected override GGUIWndMarsBuildingRedTipHUDFollower _createItemWnd(GGUIMonoMarsBuildingRedTipHUD _wndMono)
        {
            GGUIWndMarsBuildingRedTipHUDFollower wnd = new GGUIWndMarsBuildingRedTipHUDFollower(_wndMono);
            wnd.refreshWnd(_m_buildingView);
            wnd.showWnd();
            return wnd;
        }
        public void refreshWnd(_IMarsBuildingView _buildingView)
        {
            _m_buildingView = _buildingView;
            wnd?.refreshWnd(_m_buildingView);
        }
    }

    /// <summary>
    /// 建筑红点HUD窗口
    /// </summary>
    public class GGUIWndMarsBuildingRedTipHUDFollower : _ATALGGUIWndCommonFollowItem<GGUIMonoMarsBuildingRedTipHUD>
    {
        private _IMarsBuildingView _m_buildingView;

        private NPGGUIWndCommonRedTip _m_canBuildRedTipWnd;
        private NPGGUIWndCommonRedTip _m_canUpgradeRedTipWnd;
        private NPGGUIWndCommonRedTip _m_canDispatchRedTipWnd;
        private NPGGUIWndCommonRedTip _m_buildCompleteRedTipWnd;

        private bool _m_hasCommonItemChgRefreshRedTipTask; // 是否有刷新红点任务
        private ObjectPool<NPCommonItem> _m_commonItemPool = new ObjectPool<NPCommonItem>(() => new NPCommonItem(), null, null, null, false, 1); // 通用道具对象池
        private List<NPCommonItem> _m_lChgItemList = new List<NPCommonItem>(); // 变化的道具列表
        
        
        public GGUIWndMarsBuildingRedTipHUDFollower(GGUIMonoMarsBuildingRedTipHUD _wnd) : base(_wnd)
        {
            initWnd();
        }


        protected override void _onShowWnd()
        {
            _registerMsg();
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _unregisterMsg();
        }
        protected override void _onReset()
        {
        }
        protected override void _onDiscard()
        {
            if (_m_canBuildRedTipWnd != null)
            {
                _m_canBuildRedTipWnd.discard();
                _m_canBuildRedTipWnd = null;
            }
            if (_m_canUpgradeRedTipWnd != null)
            {
                _m_canUpgradeRedTipWnd.discard();
                _m_canUpgradeRedTipWnd = null;
            }
            if (_m_canDispatchRedTipWnd != null)
            {
                _m_canDispatchRedTipWnd.discard();
                _m_canDispatchRedTipWnd = null;
            }
            if (_m_buildCompleteRedTipWnd != null)
            {
                _m_buildCompleteRedTipWnd.discard();
                _m_buildCompleteRedTipWnd = null;
            }

            // 清除延迟任务标记和道具列表
            _m_hasCommonItemChgRefreshRedTipTask = false;
            _pushBackAllCommonItem();
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            // 构建红点子窗口
            if (wnd.monoCanBuildRedTip != null)
            {
                _m_canBuildRedTipWnd = new NPGGUIWndCommonRedTip(wnd.monoCanBuildRedTip);
                _m_canBuildRedTipWnd.showWnd();
            }
            
            if (wnd.monoCanUpgradeRedTip != null)
            {
                _m_canUpgradeRedTipWnd = new NPGGUIWndCommonRedTip(wnd.monoCanUpgradeRedTip);
                _m_canUpgradeRedTipWnd.showWnd();
            }
            
            if (wnd.monoCanDispatchRedTip != null)
            {
                _m_canDispatchRedTipWnd = new NPGGUIWndCommonRedTip(wnd.monoCanDispatchRedTip);
                _m_canDispatchRedTipWnd.showWnd();
            }

            if (wnd.monoBuildCompleteRedTip != null)
            {
                _m_buildCompleteRedTipWnd = new NPGGUIWndCommonRedTip(wnd.monoBuildCompleteRedTip);
                _m_buildCompleteRedTipWnd.showWnd();
            }
        }


        public void refreshWnd(_IMarsBuildingView _buildingView)
        {
            _m_buildingView = _buildingView;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || _m_buildingView == null)
                return;
            
            MarsBuildingInfo buildingInfo = _m_buildingView.buildingInfo;
            if (buildingInfo == null)
                return;
            
            _refreshCanBuildRedTip();
            _refreshCanUpgradeRedTip();
            _refreshCanDispatchRedTip();
            _refreshBuildCompleteRedTip();
        }

        /// <summary>
        /// 刷新可建造红点
        /// </summary>
        private void _refreshCanBuildRedTip()
        {
            if (_m_canBuildRedTipWnd == null)
                return;
            
            MarsBuildingInfo buildingInfo = _m_buildingView?.buildingInfo;
            // 只有未建造状态且满足建造条件时显示红点
            bool showRedTip = buildingInfo != null && NPPlayer.instance.marsComp.buildingSubComponent.hasLeisureQueue && 
                              buildingInfo.state == MarsBuildingInfo.StateType.Unbuilt && buildingInfo.checkCanBuild(true);
            
            _m_canBuildRedTipWnd.showRedTipNum(showRedTip ? 1 : 0);
        }
        
        /// <summary>
        /// 刷新可升级红点
        /// </summary>
        private void _refreshCanUpgradeRedTip()
        {
            if (_m_canUpgradeRedTipWnd == null)
                return;
            
            MarsBuildingInfo buildingInfo = _m_buildingView?.buildingInfo;
            // 只有正常状态且满足升级条件时显示红点
            bool showRedTip = buildingInfo != null && NPPlayer.instance.marsComp.buildingSubComponent.hasLeisureQueue && 
                              buildingInfo.equipmentData.equipmentLevelProgressIsComplete && 
                              buildingInfo.state == MarsBuildingInfo.StateType.Normal && buildingInfo.checkCanUpgrade(true);
            
            _m_canUpgradeRedTipWnd.showRedTipNum(showRedTip ? 1 : 0);
        }
        
        /// <summary>
        /// 刷新可派遣红点
        /// </summary>
        private void _refreshCanDispatchRedTip()
        {
            if (_m_canDispatchRedTipWnd == null)
                return;

            MarsBuildingInfo buildingInfo = _m_buildingView?.buildingInfo;
            bool showRedTip = buildingInfo != null && buildingInfo.settleSlotData.isValid() && !buildingInfo.settleSlotData.isMax
                              && NPPlayer.instance.marsComp.peopleSubComponent.idlePeopleNum > 0;

            _m_canDispatchRedTipWnd.showRedTipNum(showRedTip ? 1 : 0);
        }

        /// <summary>
        /// 刷新建筑建造完成红点
        /// </summary>
        private void _refreshBuildCompleteRedTip()
        {
            if (_m_buildCompleteRedTipWnd == null)
                return;

            MarsBuildingInfo buildingInfo = _m_buildingView?.buildingInfo;
            bool showRedTip = buildingInfo != null &&
                AccountSettingMgr.instance.unreadMarsBuildingChangeSaver.isUnread(buildingInfo.buildRefId);

            _m_buildCompleteRedTipWnd.showRedTipNum(showRedTip ? 1 : 0);
        }

        /// <summary>
        /// 注册消息监听
        /// </summary>
        private void _registerMsg()
        {
            NPPlayer.instance.marsComp.buildingSubComponent.onBuildingStateChg += _onBuildingStateChg;
            NPPlayer.instance.marsComp.buildingSubComponent.onEquipmentUpgraded += _onEquipmentUpgraded;
            NPPlayer.instance.marsComp.buildingSubComponent.onBuildingLevelChg += _onBuildingLevelChg;
            // 注册建筑可派遣居民上限变化事件
            NPPlayer.instance.marsComp.onSettleSlotPeopleLimitChanged += _onSettleSlotPeopleLimitChg;
            
            // 注册人口数量变化消息
            WinMsg.RegisterMsgAct(WinMsgType.ON_MARS_PEOPLE_NUM_CHG, _onPeopleNumChg);
            // 注册通用道具数量变化消息（资源变化可能影响可建造/可升级红点）
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ITEM_COUNT_CHG, _onCommonItemChg);
            
            // 注册玩家属性变化事件（建筑队列数量变化）
            NPPlayer.instance.playerPropertyMgr.propertyChgDelegate += _onPropertyChgDelegate;
            // 注册玩家Buff变化事件（建筑队列临时解锁Buff）
            NPPlayer.instance.playerBuffComp.onChgPlayerBuff += _onBuffChgDelegate;

            // 注册未读建筑变化事件
            AccountSettingMgr.instance.unreadMarsBuildingChangeSaver.onUnreadMarsBuildingChanged += _onUnreadMarsBuildingChanged;
        }
        
        /// <summary>
        /// 解除消息监听
        /// </summary>
        private void _unregisterMsg()
        {
            NPPlayer.instance.marsComp.buildingSubComponent.onBuildingStateChg -= _onBuildingStateChg;
            NPPlayer.instance.marsComp.buildingSubComponent.onEquipmentUpgraded -= _onEquipmentUpgraded;
            NPPlayer.instance.marsComp.buildingSubComponent.onBuildingLevelChg -= _onBuildingLevelChg;
            NPPlayer.instance.marsComp.onSettleSlotPeopleLimitChanged -= _onSettleSlotPeopleLimitChg;
            
            WinMsg.UnregisterMsgAct(WinMsgType.ON_MARS_PEOPLE_NUM_CHG, _onPeopleNumChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ITEM_COUNT_CHG, _onCommonItemChg);
            
            // 解除注册玩家属性变化事件
            NPPlayer.instance.playerPropertyMgr.propertyChgDelegate -= _onPropertyChgDelegate;
            // 解除注册玩家Buff变化事件
            NPPlayer.instance.playerBuffComp.onChgPlayerBuff -= _onBuffChgDelegate;

            // 解除注册未读建筑变化事件
            AccountSettingMgr.instance.unreadMarsBuildingChangeSaver.onUnreadMarsBuildingChanged -= _onUnreadMarsBuildingChanged;
        }
        
        /// <summary>
        /// 建筑状态变化事件处理
        /// </summary>
        private void _onBuildingStateChg(long _buildingId, MarsBuildingInfo.StateType _oldState, MarsBuildingInfo.StateType _newState)
        {
            refreshWnd();
        }
        
        /// <summary>
        /// 部件升级事件处理(部件升级可能会影响建筑各个红点)
        /// </summary>
        private void _onEquipmentUpgraded(long _buildingId, long _equipmentId)
        {
            // 部件升级可能影响升级条件 和 可派遣人数
            _refreshCanUpgradeRedTip();
            
            // 不刷新派遣红点是因为派遣人数上限变化有独立推送
            // _refreshCanDispatchRedTip();
        }
        
        /// <summary>
        /// 建筑派遣槽位人数上限变化事件处理
        /// 参数: _buildingId 建筑ID
        /// </summary>
        private void _onSettleSlotPeopleLimitChg(long _buildingId)
        {
            MarsBuildingInfo buildingInfo = _m_buildingView?.buildingInfo;
            if (buildingInfo == null || buildingInfo.buildRefId != _buildingId)
                return;
            
            // 派遣槽位上限变化影响派遣红点
            _refreshCanDispatchRedTip();
        }
        
        /// <summary>
        /// 建筑等级变化事件处理
        /// </summary>
        private void _onBuildingLevelChg(long _buildingId, long _oldValue)
        {
            _refreshCanBuildRedTip();
            _refreshCanUpgradeRedTip();
        }
        
        /// <summary>
        /// 人口数量变化消息处理
        /// </summary>
        private void _onPeopleNumChg()
        {
            _refreshCanDispatchRedTip();
        }
        
        /// <summary>
        /// 通用道具数量变化消息处理
        /// 参数: _objs[0] 为 ENPItemType, _objs[1] 为 long(itemId)
        /// </summary>
        private void _onCommonItemChg(params object[] _objs)
        {
            if (_objs == null || _objs.Length < 2 || !(_objs[0] is ENPItemType itemType) || !(_objs[1] is long subId))
                return;
            
            NPCommonItem chgCommonItem = _m_commonItemPool.Get();
            if (chgCommonItem == null)
                return;
            
            chgCommonItem.itemType = itemType;
            chgCommonItem.itemId = subId;
            _m_lChgItemList.Add(chgCommonItem);
            
            // 使用延迟任务避免频繁刷新红点
            _createCommonItemChgRefreshRedTipTask();
        }
        
        /// <summary>
        /// 创建CommonItem变化刷新红点任务
        /// </summary>
        private void _createCommonItemChgRefreshRedTipTask()
        {
            if (_m_hasCommonItemChgRefreshRedTipTask)
                return;

            _m_hasCommonItemChgRefreshRedTipTask = true;
            ALCommonTaskController.CommonActionAddMonoTask(() =>
            {
                if (!_m_hasCommonItemChgRefreshRedTipTask)
                    return;
                
                _m_hasCommonItemChgRefreshRedTipTask = false;
                
                _refreshCommonItemChgRedTipTask(); // 刷新物品变化相关红点
                
            }, 1f); // 延迟1秒刷新红点，防止频繁刷新
        }
        
        /// <summary>
        /// 刷新物品变化相关红点任务
        /// </summary>
        private void _refreshCommonItemChgRedTipTask()
        {
            if (_m_lChgItemList.Count <= 0)
                return;
            
            MarsBuildingInfo buildingInfo = _m_buildingView?.buildingInfo;
            if (buildingInfo == null)
            {
                _pushBackAllCommonItem();
                return;
            }
            
            // 根据_m_lChgItemList判断是否需要刷新红点
            bool needRefreshCanBuild = false;
            bool needRefreshCanUpgrade = false;
            
            foreach (var chgItem in _m_lChgItemList)
            {
                if (chgItem == null)
                    continue;
                
                // 若循环中判断到所有红点都需要刷新，则提前退出循环
                if (needRefreshCanBuild && needRefreshCanUpgrade)
                    break;
                
                // 检查变化的道具是否是建造消耗
                if (!needRefreshCanBuild && buildingInfo.refObj != null && buildingInfo.refObj.build_cost_list != null)
                {
                    if (buildingInfo.refObj.build_cost_list.getItem(chgItem.itemType, chgItem.itemId) != null)
                    {
                        needRefreshCanBuild = true;
                    }
                }
                
                // 检查变化的道具是否是升级消耗
                if (!needRefreshCanUpgrade && buildingInfo.levelData != null && buildingInfo.levelData.refObj != null && buildingInfo.levelData.refObj.upgrade_cost_list != null)
                {
                    if (buildingInfo.levelData.refObj.upgrade_cost_list.getItem(chgItem.itemType, chgItem.itemId) != null)
                    {
                        needRefreshCanUpgrade = true;
                    }
                }
            }
            _pushBackAllCommonItem();
            
            // 根据判断结果刷新对应的红点
            if (needRefreshCanBuild)
                _refreshCanBuildRedTip();
            
            if (needRefreshCanUpgrade)
                _refreshCanUpgradeRedTip();
        }
        
        /// <summary>
        /// 放回所有通用道具对象
        /// </summary>
        private void _pushBackAllCommonItem()
        {
            foreach (var item in _m_lChgItemList)
            {
                _m_commonItemPool.Release(item);
            }
            _m_lChgItemList.Clear();
        }
        
        /// <summary>
        /// 玩家属性变化事件处理
        /// </summary>
        private void _onPropertyChgDelegate(ENPPlayerPropertyType _type, long _oldValue, long _newValue)
        {
            // 若变化的是火星建筑队列数量属性，则刷新建造和升级红点（队列数量影响是否显示红点）
            if (_type == ENPPlayerPropertyType.MARS_BUILDING_QUEUE_NUM)
            {
                _refreshCanBuildRedTip();
                _refreshCanUpgradeRedTip();
            }
        }
        
        /// <summary>
        /// 玩家Buff变化事件处理
        /// </summary>
        private void _onBuffChgDelegate(NPPlayerBuffInfo _buffInfo, int _layer, long _timeMs)
        {
            if (_buffInfo == null)
                return;

            // 若变化的是火星建筑队列临时解锁Buff，则刷新建造和升级红点
            if (_buffInfo.buffId == GRefdataCoreMgr.instance.npGeneral.mars_temp_building_queue_gain_buff_item.subId)
            {
                _refreshCanBuildRedTip();
                _refreshCanUpgradeRedTip();
            }
        }

        /// <summary>
        /// 未读建筑变化事件处理
        /// </summary>
        private void _onUnreadMarsBuildingChanged()
        {
            _refreshBuildCompleteRedTip();
        }
    }
}
