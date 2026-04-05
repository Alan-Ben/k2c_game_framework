using ALPackage;
using Common.ActivityEnum;
using Common.RechargeRebateObj;
using CommonEnum;
using GC2GS.p033_SimpleActivityOp;
using GS2GC.p033_SimpleActivityOp;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 充值返利组件
    /// </summary>
    public class RechargeRebateComponent : _ANPBasicPlayerComponent
    {
        //充值返利信息字典，<活动实例id，充值返利信息列表>
        [NotNull] private Dictionary<long, List<RechargeRebateInfo>> _m_dRechargeRebateInfoDic = new Dictionary<long, List<RechargeRebateInfo>>();

        //构造函数
        public RechargeRebateComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
        }

        protected static ENPPlayerCompType[] _g_DependComp = { ENPPlayerCompType.COMMON_ACTIVITY };
        public override bool isMustInit { get { return true; } }
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.RECHARGE_REBATE; } }
        public override ENPPlayerCompType[] dependCompList { get { return _g_DependComp; } }

        /// <summary>
        /// 是否允许提前初始化。提前初始化的意思是在依赖项没有完成初始化之前就进行初始化操作（一般是提前发送消息）
        /// 在初始化结果消息返回的时候，通过特殊的初始化函数dealPreInitFunc进行处理函数注册，再依赖项完成之后才进行初始化处理
        /// </summary>
        public override bool canPreInit { get { return false; } }

        /// <summary>
        /// 发送初始化协议提前申请内容
        /// </summary>
        public override void presendInitProtocol()
        {
        }

        protected override void _dealInit()
        {
            _ABaseActivityInfo activityInfo = NPPlayer.instance.commonActivityComp.getValidActivityInfoByType(ECommonActivityType.RECHARGE_REBATE);
            if (activityInfo == null || !activityInfo.isPlaying)
            {
                setInitDone();
                return;
            }

            reqRechargeRebateInfo(activityInfo.instanceId, (_infoList) =>
            {
                _addRechargeRebateInfoList(activityInfo.instanceId, _infoList);
                setInitDone();
            });
        }

        //组件加载完成时的调用
        protected override void _onInitDone()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ACTIVITY_ADD, _onActivityAdd);
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ACTIVITY_STATE_CHG, _onActivityStateChg);
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ACTIVITY_CLOSE, _onActivityRemove);
            refreshRechargeRebateRedTip();
        }

        //组件初始化失败的处理
        protected override void _onInitFail()
        {
            ALLog.Error("RechargeRebateComponent init Fail!!!");
        }

        //释放资源函数
        protected override void _discard()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ACTIVITY_ADD, _onActivityAdd);
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ACTIVITY_STATE_CHG, _onActivityStateChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ACTIVITY_CLOSE, _onActivityRemove);
            _clear();
        }

        //析构函数
        private void _clear()
        {
            _m_dRechargeRebateInfoDic.Clear();
        }

        /// <summary>
        /// 获取充值返利信息列表
        /// </summary>
        /// <returns></returns>
        public List<RechargeRebateInfo> getRechargeRebateInfoList()
        {
            List<RechargeRebateInfo> infoList = new List<RechargeRebateInfo>();
            foreach (List<RechargeRebateInfo> rechargeRebateInfos in _m_dRechargeRebateInfoDic.Values)
            {
                if(rechargeRebateInfos != null)
                    infoList.AddRange(rechargeRebateInfos);
            }
            return infoList;
        }

        /// <summary>
        /// 刷新充值返利红点提示
        /// </summary>
        public void refreshRechargeRebateRedTip()
        {
            long redTipCount = 0;
            foreach (KeyValuePair<long, List<RechargeRebateInfo>> keyValuePair in _m_dRechargeRebateInfoDic)
            {
                List<RechargeRebateInfo> rechargeRebateInfoList = keyValuePair.Value;
                if (rechargeRebateInfoList == null || rechargeRebateInfoList.Count <= 0)
                    continue;

                for (int i = 0; i < rechargeRebateInfoList.Count; i++)
                {
                    if (rechargeRebateInfoList[i] != null && rechargeRebateInfoList[i].haveStepCanGetReward())
                        redTipCount++;
                }
            }
            RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_RECHARGE_REBATE, redTipCount);
        }

        /// <summary>
        /// 添加充值返利信息列表
        /// </summary>
        /// <param name="_instanceId"></param>
        /// <param name="_infoList"></param>
        private void _addRechargeRebateInfoList(long _instanceId, List<RechargeRebate_GroupInfo> _infoList)
        {
            List<RechargeRebateInfo> rechargeRebateList = new List<RechargeRebateInfo>();
            for (int i = 0; i < _infoList.Count; i++)
            {
                rechargeRebateList.Add(new RechargeRebateInfo(_instanceId, _infoList[i]));
            }
            _m_dRechargeRebateInfoDic[_instanceId] = rechargeRebateList;
            //刷新红点
            refreshRechargeRebateRedTip();
            //发送消息
            WinMsg.SendMsg(WinMsgType.ON_RECHARGE_REBATE_ADD, _instanceId);
        }

        /// <summary>
        /// 移除充值返利信息
        /// </summary>
        /// <param name="_instanceId"></param>
        private void _removeRechargeRebateInfo(long _instanceId)
        {
            if (_m_dRechargeRebateInfoDic.ContainsKey(_instanceId))
            {
                _m_dRechargeRebateInfoDic.Remove(_instanceId);
                //刷新红点
                refreshRechargeRebateRedTip();
                //发送消息
                WinMsg.SendMsg(WinMsgType.ON_RECHARGE_REBATE_REMOVE, _instanceId);
            }
        }

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
            long instanceId = (long)_objects[1];
            GActivityMainRefObj activityMainRef = GRefdataCoreMgr.instance.activityMainRefCore.getRef(activityId);
            _ABaseActivityInfo activityInfo = NPPlayer.instance.commonActivityComp.getActivityInfoByInstanceId(instanceId);
            if(activityInfo == null || activityMainRef == null || activityMainRef.type_id != ECommonActivityType.RECHARGE_REBATE || !activityInfo.isPlaying)
                return;

            //请求充值返利信息
            reqRechargeRebateInfo(instanceId, _infoList =>
            {
                _addRechargeRebateInfoList(instanceId, _infoList);
            });
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
            long instanceId = (long)_objects[1];
            EActivityState oriState = (EActivityState) _objects[2];
            EActivityState curState = (EActivityState) _objects[3];

            GActivityMainRefObj activityMainRef = GRefdataCoreMgr.instance.activityMainRefCore.getRef(activityId);
            if (activityMainRef != null && activityMainRef.type_id == ECommonActivityType.RECHARGE_REBATE && curState == EActivityState.PLAYING)
            {
                //请求充值返利信息
                reqRechargeRebateInfo(instanceId, _infoList =>
                {
                    _addRechargeRebateInfoList(instanceId, _infoList);
                });
            }
            else
                _removeRechargeRebateInfo(instanceId);
        }

        /// <summary>
        /// 活动移除
        /// </summary>
        /// <param name="_objects"></param>
        private void _onActivityRemove(params object[] _objects)
        {
            if (_objects == null || _objects.Length < 2)
                return;

            long activityId = (long)_objects[0];
            long instanceId = (long)_objects[1];

            GActivityMainRefObj activityMainRef = GRefdataCoreMgr.instance.activityMainRefCore.getRef(activityId);
            if (activityMainRef != null && activityMainRef.type_id == ECommonActivityType.RECHARGE_REBATE)
            {
                _removeRechargeRebateInfo(instanceId);
            }
        }

        #endregion

        #region S2C

        /// <summary>
        /// 充值返利计数变化
        /// </summary>
        /// <param name="_msg"></param>
        public void onRechargeRebateCountChg(GS2GC_033_109_OnRechargeRebateCountChg _msg)
        {
            if (_msg == null)
                return;

            if (_m_dRechargeRebateInfoDic.TryGetValue(_msg.getActivityInstanceId(), out List<RechargeRebateInfo> _infoList))
            {
                if (_infoList != null)
                {
                    for (int i = 0; i < _infoList.Count; i++)
                    {
                        if (_infoList[i] != null && _infoList[i].groupId == _msg.getGroupId())
                        {
                            _infoList[i].updateCount(_msg.getCount()); ;
                            break;
                        }
                    }
                }
            }
            //刷新红点
            refreshRechargeRebateRedTip();
            //发送消息
            WinMsg.SendMsg(WinMsgType.ON_RECHARGE_REBATE_COUNT_CHG, _msg.getActivityInstanceId(), _msg.getGroupId(), _msg.getCount());
        }

        /// <summary>
        /// 充值返利奖励领取变更
        /// </summary>
        /// <param name="_msg"></param>
        public void onRechargeRebateRewardDraw(GS2GC_033_110_OnRechargeRebateRewardDraw _msg)
        {
            if (_msg == null)
                return;

            if (_m_dRechargeRebateInfoDic.TryGetValue(_msg.getActivityInstanceId(), out List<RechargeRebateInfo> _infoList))
            {
                if (_infoList != null)
                {
                    for (int i = 0; i < _infoList.Count; i++)
                    {
                        if (_infoList[i] != null && _infoList[i].groupId == _msg.getGroupId())
                        {
                            _infoList[i].addHadDrawStepList(_msg.getStepId()); ;
                            break;
                        }
                    }
                }
            }
            //刷新红点
            refreshRechargeRebateRedTip();
            //发送消息
            WinMsg.SendMsg(WinMsgType.ON_RECHARGE_REBATE_DRAW_CHG, _msg.getActivityInstanceId(), _msg.getGroupId());
        }

        /// <summary>
        /// 充值返利组信息变化
        /// </summary>
        /// <param name="_msg"></param>
        public void onRechargeRebateGroupInfoChg(GS2GC_033_111_OnRechargeRebateGroupInfoChg _msg)
        {
            if (_msg == null)
                return;

            if (_m_dRechargeRebateInfoDic.TryGetValue(_msg.getActivityInstanceId(), out List<RechargeRebateInfo> _infoList))
            {
                if (_infoList != null)
                {
                    for (int i = 0; i < _infoList.Count; i++)
                    {
                        if (_infoList[i] != null && _infoList[i].groupId == _msg.getGroupInfo().getGroupId())
                        {
                            _infoList[i].updateInfo(_msg.getActivityInstanceId(), _msg.getGroupInfo());
                            break;
                        }
                    }
                }
            }
            //刷新红点
            refreshRechargeRebateRedTip();
            //发送消息
            WinMsg.SendMsg(WinMsgType.ON_RECHARGE_REBATE_GROUP_CHG, _msg.getActivityInstanceId(), _msg.getGroupInfo().getGroupId());
        }

        #endregion

        #region C2S

        /// <summary>
        /// 请求充值返利信息列表
        /// </summary>
        public void reqRechargeRebateInfo(long _instanceId, Action<List<RechargeRebate_GroupInfo>> _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_033_007_ReqRechargeRebateInfo(_instanceId),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_033_007_RetRechargeRebateInfo>((_isSuc, _msg) =>
                {
                    if (_isSuc)
                        _callback?.Invoke(_msg.getGroupInfoList());
                }));
        }

        /// <summary>
        /// 请求领取充值返利奖励
        /// </summary>
        /// <param name="_instanceId"></param>
        /// <param name="_groupId"></param>
        /// <param name="_stepId"></param>
        /// <param name="_callback"></param>
        public void reqRechargeRebateDrawReward(long _instanceId, long _groupId, long _stepId, Action _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_033_008_ReqRechargeRebateDrawReward(_instanceId, _groupId, _stepId),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_033_008_RetRechargeRebateDrawReward>((_isSuc, _msg) =>
                {
                    if (_isSuc)
                        _callback?.Invoke();
                }));
        }

        #endregion
    }
}
