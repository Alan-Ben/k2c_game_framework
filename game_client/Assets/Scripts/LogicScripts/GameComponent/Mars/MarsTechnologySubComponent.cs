using System;
using System.Collections.Generic;
using ALPackage;
using Common.MarsObj;
using GC2GS.p002_InitOp;
using GS2GC.p002_InitOp;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 火星科技子组件
    /// </summary>
    public class MarsTechnologySubComponent : _AMarsSubComponent
    {
        [NotNull] private readonly MarsPropertyContainer _m_marsPropertyContainer;
        [NotNull] private readonly NPPlayerPropertyContainer _m_playerPropertyContainer;
        private long _m_lMarsPower;//火星实力加成
        [ItemNotNull, NotNull] private readonly List<MarsTechnologyInfo> _m_lTechnologyInfoList;
        
        private MarsTechnologyInfo _m_iNowdUpgradingOrUpgradedTechnologyInfo;//当前正在升级中或升级完成的科技信息
        private ALCommonEnableTaskController _m_upgradeTechCountDownTaskController;//科技升级任务倒计时

        public MarsTechnologySubComponent([NotNull] MarsComponent _parentComp) : base(_parentComp)
        {
            _m_marsPropertyContainer = new MarsPropertyContainer();
            _m_playerPropertyContainer = new NPPlayerPropertyContainer();
            _m_lMarsPower = 0;
            _m_lTechnologyInfoList = new List<MarsTechnologyInfo>();
        }

        /// <summary>
        /// 火星属性容器
        /// </summary>
        [NotNull] public MarsPropertyContainer marsPropertyContainer { get { return _m_marsPropertyContainer; } }
        /// <summary>
        /// 玩家属性容器
        /// </summary>
        [NotNull] public NPPlayerPropertyContainer playerPropertyContainer { get { return _m_playerPropertyContainer; } }

        public long marsPower { get { return _m_lMarsPower; } }

        /// <summary>
        /// 当前正在升级中或已升级完成的科技信息
        /// </summary>
        public MarsTechnologyInfo nowdUpgradingOrUpgradedTechnologyInfo { get { return _m_iNowdUpgradingOrUpgradedTechnologyInfo; } }

        public IReadOnlyList<MarsTechnologyInfo> technologyInfoList { get { return _m_lTechnologyInfoList; } }
        
        /// <summary>
        /// 当前升级中科技倒计时变化
        /// </summary>
        public event Action upgradingTechnologyCountDownChg;

        public override void init(Action<bool> _complete)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_002_078_ReqMarsTechInit(), 
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_002_078_RetMarsTechInit>((_isSuc, _msg) =>
                {
                    _m_parentComponent.dealPreInitFunc(() =>
                    {
                        if (!_isSuc || _msg == null)
                        {
                            _complete?.Invoke(false);
                            return;
                        }

                        List<Mars_Technology> serverTechnologyList = _msg.getTechnologyList();
                        MarsTechnologyInfo technologyInfo = null;
                        _m_lTechnologyInfoList.Clear();
                        if (serverTechnologyList != null)
                        {
                            foreach (Mars_Technology serverTechnology in serverTechnologyList)
                            {
                                if (serverTechnology == null)
                                    continue;

                                technologyInfo = new MarsTechnologyInfo(serverTechnology);
                                _m_lTechnologyInfoList.Add(technologyInfo);
                                
                                // 添加属性加成
                                MarsTechnologyLevelRefObj technologyLevelRefObj = technologyInfo.technologyLvlRefObj;
                                if (technologyLevelRefObj != null)
                                {
                                    _m_marsPropertyContainer.addModifier(technologyLevelRefObj.mars_property);
                                    _m_playerPropertyContainer.addModifier(technologyLevelRefObj.player_property);
                                    _m_lMarsPower += technologyLevelRefObj.mars_power;
                                }
                            }
                        }
                        // 触发火星实力变化
                        _m_parentComponent.needUpdatePower();
                        
                        // 更新正在升级中的科技信息
                        _updateUpgradingTechnologyInfo();

                        _complete?.Invoke(true);
                    });
                }));
        }
        
        public override void discard()
        {
            _discardUpgradeTechCountDownTask();
            upgradingTechnologyCountDownChg = null;
            
            _m_marsPropertyContainer.clear();
            _m_playerPropertyContainer.clear();
            _m_lMarsPower = 0;

            _m_iNowdUpgradingOrUpgradedTechnologyInfo = null;
            _m_lTechnologyInfoList.Clear();
        }

        /// <summary>
        /// 根据科技ID获取科技信息
        /// </summary>
        public MarsTechnologyInfo getTechnologyInfoById(long _technologyId)
        {
            return _m_lTechnologyInfoList.Find((_item) =>
            {
                return _item != null && _item.technologyId == _technologyId;
            });
        }


        private void _updateTechnology(Mars_Technology _serverData)
        {
            if (_serverData == null)
                return;
            
            long technologyId = _serverData.getTechnologyId();
            MarsTechnologyInfo technologyInfo = getTechnologyInfoById(technologyId);
            MarsTechnologyLevelRefObj preTechnologyLevelRefObj = technologyInfo?.technologyLvlRefObj;
            if (technologyInfo != null)
            {
                technologyInfo.update(_serverData);
            }
            else
            {
                technologyInfo = new MarsTechnologyInfo(_serverData);
            }

            // 更新属性加成
            MarsTechnologyLevelRefObj nowTechnologyLevelRefObj = technologyInfo.technologyLvlRefObj;
            if (preTechnologyLevelRefObj != nowTechnologyLevelRefObj)
            {
                if (preTechnologyLevelRefObj != null)
                {
                    _m_marsPropertyContainer.removeModifier(preTechnologyLevelRefObj.mars_property);
                    _m_playerPropertyContainer.removeModifier(preTechnologyLevelRefObj.player_property);
                    _m_lMarsPower -= preTechnologyLevelRefObj.mars_power;
                }

                if (nowTechnologyLevelRefObj != null)
                {
                    _m_marsPropertyContainer.addModifier(nowTechnologyLevelRefObj.mars_property);
                    _m_playerPropertyContainer.addModifier(nowTechnologyLevelRefObj.player_property);
                    _m_lMarsPower += nowTechnologyLevelRefObj.mars_power;
                }
            }

            // 触发火星实力变化
            _m_parentComponent.needUpdatePower();
            
            _updateUpgradingTechnologyInfo();
            
            WinMsg.SendMsg(WinMsgType.ON_MARS_TECHNOLOGY_CHG, technologyInfo);
        }

        internal void onItemHelpSecsChg(long _objId, int _secs)
        {
            MarsTechnologyInfo technologyInfo = getTechnologyInfoById(_objId);
            if (technologyInfo == null)
            {
                ALLog.Error($"[MarsTechnologySubComponent] onItemHelpSecsChg: 未找到对应的科技信息, _objId={_objId}");
                return;
            }
            
            technologyInfo.onItemHelpSecsChg(_secs);
            
            _updateUpgradingTechnologyInfo();
            
            WinMsg.SendMsg(WinMsgType.ON_MARS_TECHNOLOGY_CHG, technologyInfo);
        }

        #region upgrade technology

        /// <summary>
        /// 更新正在升级中的科技信息
        /// </summary>
        private void _updateUpgradingTechnologyInfo()
        {
            MarsTechnologyInfo preTechnologyInfo = _m_iNowdUpgradingOrUpgradedTechnologyInfo;
            _m_iNowdUpgradingOrUpgradedTechnologyInfo = null;
            foreach (MarsTechnologyInfo technologyInfo in _m_lTechnologyInfoList)
            {
                // 跳过不在升级中或已升级完成的科技
                if (!technologyInfo.isUpgradingOrUpgraded)
                    continue;

                if (_m_iNowdUpgradingOrUpgradedTechnologyInfo == null || !_m_iNowdUpgradingOrUpgradedTechnologyInfo.isUpgradingOrUpgraded)
                {
                    _m_iNowdUpgradingOrUpgradedTechnologyInfo = technologyInfo;
                    continue;
                }

                // 优先选择升级中的科技
                if (_m_iNowdUpgradingOrUpgradedTechnologyInfo.isUpgraded && technologyInfo.isUpgrading)
                {
                    _m_iNowdUpgradingOrUpgradedTechnologyInfo = technologyInfo;
                    continue;
                }
                
                // 都在升级中, 优先选择结束时间更早的科技
                if(_m_iNowdUpgradingOrUpgradedTechnologyInfo.isUpgrading && technologyInfo.isUpgrading && technologyInfo.afterReductionEndUpgradeLvlMs < _m_iNowdUpgradingOrUpgradedTechnologyInfo.afterReductionEndUpgradeLvlMs)
                {
                    _m_iNowdUpgradingOrUpgradedTechnologyInfo = technologyInfo;
                    continue;
                }
            }

            if (_m_iNowdUpgradingOrUpgradedTechnologyInfo != null && _m_iNowdUpgradingOrUpgradedTechnologyInfo.isUpgradingOrUpgraded)
            {
                // 当前有正在升级中的科技, 开启倒计时任务
                _initUpgradeTechCountDownTask();
            }
            else
            {
                
                _discardUpgradeTechCountDownTask();
            }
            
            if(_m_iNowdUpgradingOrUpgradedTechnologyInfo != preTechnologyInfo)
                WinMsg.SendMsg(WinMsgType.ON_MARS_UPGRADING_TECHNOLOGY_CHG);
        }
        
        private void _discardUpgradeTechCountDownTask()
        {
            _m_upgradeTechCountDownTaskController.setDisable();
        }

        private void _initUpgradeTechCountDownTask()
        {
            _discardUpgradeTechCountDownTask();
            
            if (_m_iNowdUpgradingOrUpgradedTechnologyInfo == null || !_m_iNowdUpgradingOrUpgradedTechnologyInfo.isUpgradingOrUpgraded)
                return;

            _m_upgradeTechCountDownTaskController = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(
                () =>
                {
                    upgradingTechnologyCountDownChg?.Invoke();
                    
                    // 若当前没有正在升级的科技 或 科技升级完成. 尝试更新正在升级的任务信息
                    if (_m_iNowdUpgradingOrUpgradedTechnologyInfo == null || !_m_iNowdUpgradingOrUpgradedTechnologyInfo.isUpgrading)
                    {
                        // 销毁任务
                        _discardUpgradeTechCountDownTask();
                        
                        // 若科技处于升级完成状态
                        if (_m_iNowdUpgradingOrUpgradedTechnologyInfo != null && _m_iNowdUpgradingOrUpgradedTechnologyInfo.isUpgraded)
                        {
                            // 先直接客户端设置为非升级状态
                            _m_iNowdUpgradingOrUpgradedTechnologyInfo.setTechnologyInUpgradingOrUpgraded(false);
                            
                            // 科技升级完成, 显示CenterTip
                            if (_m_iNowdUpgradingOrUpgradedTechnologyInfo.technologyRefObj != null)
                            {
                                NPGUIAddSceneCenterTip.instance.showIconTextTip(_m_iNowdUpgradingOrUpgradedTechnologyInfo.technologyRefObj.icon, string.Empty, 
                                    GRefdataCoreMgr.instance.npGeneral.mars_technology_research_complete_tip_id);
                            }
                            
                            // 倒计时完成后, 发送一条科技变化消息, 以触发界面刷新
                            WinMsg.SendMsg(WinMsgType.ON_MARS_TECHNOLOGY_CHG, _m_iNowdUpgradingOrUpgradedTechnologyInfo);
                            
                            // 向服务器请求确认科技升级完成
                            reqConfirmUpgradeTechnologyLvl(_m_iNowdUpgradingOrUpgradedTechnologyInfo.technologyId,
                                (_isSucc) =>
                                {
                                    // 若没有成功
                                    if (!_isSucc)
                                    {
                                        // 将数据重置为升级中状态
                                        _m_iNowdUpgradingOrUpgradedTechnologyInfo?.setTechnologyInUpgradingOrUpgraded(true);
                                        // 重新更新正在升级中的科技信息
                                        _updateUpgradingTechnologyInfo();
                                    }
                                });
                        }
                        else
                        {
                            // 更新正在升级中的科技信息
                            _updateUpgradingTechnologyInfo();
                        }
                        return;
                    }
                }, 1f);
        }
        
        #endregion

        #region GS2GC

        /// <summary>
        /// 处理科技数据变化推送
        /// </summary>
        internal void _onTechnologyChg(GS2GC.p039_MarsBuildingOp.GS2GC_039_060_OnMarsTechnologyChg _msg)
        {
            if (_msg == null)
                return;
            
            Mars_Technology serverTechnology = _msg.getInfo();
            if (serverTechnology != null)
                _updateTechnology(serverTechnology);
        }

        #endregion

        #region GC2GS

        /// <summary>
        /// 请求升级科技等级
        /// </summary>
        /// <param name="_technologyId">科技ID</param>
        /// <param name="_complete">完成回调</param>
        public void reqUpgradeTechnologyLvl(long _technologyId, Action _complete = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS.p039_MarsBuildingOp.GC2GS_039_020_ReqUpgradeTechnologyLvl(_technologyId),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC.p039_MarsBuildingOp.GS2GC_039_020_RetUpgradeTechnologyLvl>((_isSuc, _msg) =>
                {
                    _complete?.Invoke();
                }));
        }

        /// <summary>
        /// 请求确认升级科技等级
        /// </summary>
        /// <param name="_technologyId">科技ID</param>
        /// <param name="_complete">完成回调</param>
        public void reqConfirmUpgradeTechnologyLvl(long _technologyId, Action<bool> _complete = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS.p039_MarsBuildingOp.GC2GS_039_021_ReqConfirmUpgradeTechnologyLvl(_technologyId),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC.p039_MarsBuildingOp.GS2GC_039_021_RetConfirmUpgradeTechnologyLvl>((_isSuc, _msg) =>
                {
                    _complete?.Invoke(_isSuc);
                }));
        }

        /// <summary>
        /// 请求取消科技升级
        /// </summary>
        /// <param name="_technologyId">科技ID</param>
        /// <param name="_complete">完成回调</param>
        public void reqCancelTechnologyUpgrade(long _technologyId, Action _complete = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS.p039_MarsBuildingOp.GC2GS_039_022_ReqCancelTechnologyUpgrade(_technologyId),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC.p039_MarsBuildingOp.GS2GC_039_022_RetCancelTechnologyUpgrade>((_isSuc, _msg) =>
                {
                    _complete?.Invoke();
                }));
        }

        /// <summary>
        /// 请求直接升级科技
        /// </summary>
        /// <param name="_technologyId"></param>
        /// <param name="_complete"></param>
        public void reqSetTechnologyDone(long _technologyId, Action _complete = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS.p039_MarsBuildingOp.GC2GS_039_023_ReqSetTechnologyDone(_technologyId),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC.p039_MarsBuildingOp.GS2GC_039_023_RetSetTechnologyDone>((_isSuc, _msg) =>
                {
                    _complete?.Invoke();
                }));
        }
        
        #endregion
    }
}