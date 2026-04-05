using System.Collections.Generic;
using ALPackage;
using System;
using Common.AchieveObj;
using CommonEnum;
using GS2GC.p002_InitOp;
using GS2GC.p021_PlayerInfo;
using JetBrains.Annotations;


namespace GOE
{
    //任务数据管理器
    public class PlayerAchieveComponent : _ANPBasicPlayerComponent
    {
        [NotNull] private List<AchieveInfo> _m_allAchieveList;//成就列表
        [NotNull] private Dictionary<long, long> _m_dAchievePointDic;//不同成就类型的成就点数
        [NotNull] private Dictionary<long, long> _m_dAchievePointMaxDrawStepDic;//不同成就类型成就的点数已领取奖励的最大阶段step_id


        public PlayerAchieveComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
            _m_allAchieveList = new List<AchieveInfo>();
            _m_dAchievePointDic = new Dictionary<long, long>();
            _m_dAchievePointMaxDrawStepDic = new Dictionary<long, long>();
        }

        public override bool isMustInit { get { return true; } }

        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.ACHIEVE; } }

        public override ENPPlayerCompType[] dependCompList { get { return null; } }
        /// <summary>
        /// 是否允许提前初始化。提前初始化的意思是在依赖项没有完成初始化之前就进行初始化操作（一般是提前发送消息）
        /// 在初始化结果消息返回的时候，通过特殊的初始化函数dealPreInitFunc进行处理函数注册，再依赖项完成之后才进行初始化处理
        /// </summary>
        public override bool canPreInit { get { return true; } }

        /// <summary>
        /// 成就列表
        /// </summary>
        public List<AchieveInfo> achieveInfoList { get { return _m_allAchieveList; } }

        /// <summary>
        /// 发送初始化协议提前申请内容
        /// </summary>
        public override void presendInitProtocol()
        {
            _initData();
            //请求列表
            reqAchieveInit();
            
        }

        protected override void _dealInit()
        {
        }

        //组件加载完成时的调用
        protected override void _onInitDone()
        {
        }

        //组件初始化失败的处理
        protected override void _onInitFail()
        {
            ALLog.Error("PlayerAchieveComponent init Fail!!!");
        }
        //释放资源函数
        protected override void _discard()
        {
            clear();
        }

        //所有组件初始化完以后
        public override void onAllCompInited()
        {
            //初始化成就数据上次刷新是否已满标记，由于会引用到其他组件数据去判断，所以需要在所有组件都初始化完之后再初始化
            if (_m_allAchieveList != null)
            {
                for (int i = 0; i < _m_allAchieveList.Count; i++)
                {
                    if(_m_allAchieveList[i] != null)
                        _m_allAchieveList[i].initIsLastFull();
                }
            }

            //刷新红点
            _refreshRedTip();
        }

        private void _initData()
        {
            if(_m_allAchieveList == null)
                _m_allAchieveList = new List<AchieveInfo>();
            else
                _m_allAchieveList.Clear();

            AchieveInfo tmpInfoOld = null;

            GRefdataCoreMgr.instance.achieveMap.dealAllRef((AchieveRefObj _tempRef) =>
                {
                    if(_tempRef == null)
                        return;

                    tmpInfoOld = new AchieveInfo(_tempRef);
                    _m_allAchieveList.Add(tmpInfoOld);
                });
        }
        
        /// <summary>
        /// 根据类型获取成就列表
        /// </summary>
        /// <param name="_type"></param>
        /// <returns></returns>
        public List<AchieveInfo> getInfosByType(EAchieveType _type)
        {
            List<AchieveInfo> infoList = new List<AchieveInfo>();
            for (int i = 0; i < _m_allAchieveList.Count; i++)
            {
                if(_m_allAchieveList[i] != null && _m_allAchieveList[i].achieveType == _type)
                    infoList.Add(_m_allAchieveList[i]);
            }

            return infoList;
        }

        /// <summary>
        /// 获取成就信息
        /// </summary>
        /// <param name="_achieveId"></param>
        /// <returns></returns>
        public AchieveInfo getAchimentInfo(long _achieveId)
        {
            AchieveInfo tempAchieve = null;
            for(int i = 0; i < _m_allAchieveList.Count; ++i)
            {
                tempAchieve = _m_allAchieveList[i];
                if(tempAchieve == null)
                    continue;
                if(tempAchieve.achieveId == _achieveId)
                    return tempAchieve;
            }
            return null;
        }

        //数据清空
        public void clear()
        {
            if (_m_allAchieveList != null)
            {
                for (int i = 0; i < _m_allAchieveList.Count; i++)
                {
                    _m_allAchieveList[i].discard();
                }
                _m_allAchieveList.Clear();
            }

            if(_m_dAchievePointDic != null)
                _m_dAchievePointDic.Clear();

            if(_m_dAchievePointMaxDrawStepDic != null)
                _m_dAchievePointMaxDrawStepDic.Clear();
        }

        /// <summary>
        /// 获取对应的成就点数
        /// </summary>
        /// <param name="_type"></param>
        /// <returns></returns>
        public long getAchievePoint(EAchieveType _type)
        {
            return getAchievePoint((int)_type);
        }

        /// <summary>
        /// 获取对应的成就点数
        /// </summary>
        /// <param name="_type"></param>
        /// <returns></returns>
        public long getAchievePoint(long _type)
        {
            long value = 0;
            _m_dAchievePointDic.TryGetValue(_type, out value);
            return value;
        }

        /// <summary>
        /// 是否已经领取成就点奖励
        /// </summary>
        /// <param name="_type"></param>
        /// <param name="_stepId"></param>
        /// <returns></returns>
        public bool hadGetAchievePoint(long _type, long _stepId)
        {
            if (_m_dAchievePointMaxDrawStepDic.TryGetValue(_type, out long _maxStepId))
                return _stepId <= _maxStepId;
            else
                return false;
        }

        /// <summary>
        /// 获取当前进行中的未领奖成就点阶段配置
        /// </summary>
        /// <returns></returns>
        public AchievePointStepRefObj getCurAchievePointRef(EAchieveType _type)
        {
            AchievePointStepRefObj achievePointStepRef = null;
            for (int i = 0; i < GRefdataCoreMgr.instance.achievePointStepList.refList.Count; i++)
            {
                if (GRefdataCoreMgr.instance.achievePointStepList.refList[i].type == _type &&
                    !hadGetAchievePoint((long) _type, GRefdataCoreMgr.instance.achievePointStepList.refList[i].step_id))
                {
                    achievePointStepRef = GRefdataCoreMgr.instance.achievePointStepList.refList[i];
                    break;
                }
            }

            return achievePointStepRef;
        }

        /// <summary>
        /// 获取当前进行中的未领奖成就点阶段 或 最后一个成就点阶段 配置
        /// </summary>
        /// <param name="_type"></param>
        /// <returns></returns>
        public AchievePointStepRefObj getCurAchievePointOrLastStepRef(EAchieveType _type)
        {
            AchievePointStepRefObj lastAchievePointStepRef = null;

            for (int i = 0; i < GRefdataCoreMgr.instance.achievePointStepList.refList.Count; i++)
            {
                AchievePointStepRefObj item = GRefdataCoreMgr.instance.achievePointStepList.refList[i];
                if(item == null || item.type != _type)
                    continue;

                if (!hadGetAchievePoint((long) _type, item.step_id))
                    return item;

                if (lastAchievePointStepRef == null || lastAchievePointStepRef.step_id < item.step_id)
                {
                    lastAchievePointStepRef = item;
                }
            }

            return lastAchievePointStepRef;
        }

        public ENPCommonGetStat achievePointStepGetStat(AchievePointStepRefObj _refObj)
        {
            if (_refObj == null)
                return ENPCommonGetStat.NONE;

            if (_refObj.need_point != null && !GCommon.isItemEnough(_refObj.need_point, false))
            {
                return ENPCommonGetStat.CAN_NOT_GET;
            }
            else if(!hadGetAchievePoint((long)_refObj.type, _refObj.step_id))
            {
                return ENPCommonGetStat.CAN_GET;
            }
            else
            {
                return ENPCommonGetStat.HAS_GET;
            }
        }

        //刷新红点
        private void _refreshRedTip()
        {

            long[] redCountList = new long[ALCommon.getEnumCount(typeof(EAchieveType))];

            long count = 0;
            //成就是否可领取奖励
            for (int i = 0; i < _m_allAchieveList.Count; i++)
            {
                if(_m_allAchieveList[i] == null)
                    continue;

                //成就是否有可领取奖励
                if (_m_allAchieveList[i].getCurStepRewardState() == ENPCommonGetStat.CAN_GET)
                    redCountList[(int) _m_allAchieveList[i].achieveType]++;
            }

            GRefdataCoreMgr.instance.achieveTypeMap.dealAllRef((_refObj) =>
            {
                //成就点数是否有可领取奖励
                AchievePointStepRefObj achievePointStepRef = getCurAchievePointRef(_refObj.type);
                if (achievePointStepRef != null && GCommon.isItemEnough(achievePointStepRef.need_point, false))
                    redCountList[(int)_refObj.type]++;

                if(_refObj.red_tip_id > 0)
                    RedTipMgr.instance.setCountByRefRedTipId(_refObj.red_tip_id, redCountList[(int)_refObj.type]);
            });
        }

        #region S2C

        /// <summary>
        /// 初始化成就信息
        /// </summary>
        /// <param name="_msg"></param>
        public void retAchieveInit(GS2GC_002_050_RetAchieveInit _msg)
        {
            AchieveInfo achieveInfo = null;
            Achieve_Info serverInfo = null;
            for (int i = 0; i < _msg.getAchieveList().Count; i++)
            {
                serverInfo = _msg.getAchieveList()[i];
                achieveInfo = getAchimentInfo(serverInfo.getAchieveId());
                if (null == achieveInfo)
                {
                    ALLog.Error($"---------成就id：{serverInfo.getAchieveId()}找不到对应的配表信息");
                    continue;
                }
                achieveInfo.refreshData(serverInfo, false);
            }

            Achieve_AchievePointInfo achievePointInfo = null;
            for (int i = 0; i < _msg.getAchievePointList().Count; i++)
            {
                achievePointInfo = _msg.getAchievePointList()[i];
                _m_dAchievePointDic[achievePointInfo.getType()] = achievePointInfo.getCount();
                _m_dAchievePointMaxDrawStepDic[achievePointInfo.getType()] = achievePointInfo.getHadDrawMaxStep();
            }

            setInitDone();
        }

        /// <summary>
        /// 成就信息变化
        /// </summary>
        /// <param name="_msg"></param>
        public void onAchieveChg(GS2GC_021_066_OnAchieveChg _msg)
        {
            AchieveInfo achieveInfo = null;
            Achieve_Info serverInfo = _msg.getAchieve();
            if (null == serverInfo)
                return;
            achieveInfo = getAchimentInfo(serverInfo.getAchieveId());
            if (null == achieveInfo)
            {
                ALLog.Error($"---------成就id：{serverInfo.getAchieveId()}找不到对应的配表信息");
                return;
            }
            achieveInfo.refreshData(serverInfo);

            //刷新红点
            _refreshRedTip();

            WinMsg.SendMsg(WinMsgType.ON_ACHIEVE_INFO_CHG, achieveInfo);
        }

        /// <summary>
        /// 成就点变化
        /// </summary>
        /// <param name="_msg"></param>
        public void onAchievePointChg(GS2GC_021_067_OnAchievePointChg _msg)
        {
            if (_msg == null || _msg.getAchievePointInfo() == null)
                return;

            _m_dAchievePointDic[_msg.getAchievePointInfo().getType()] = _msg.getAchievePointInfo().getCount();
            _m_dAchievePointMaxDrawStepDic[_msg.getAchievePointInfo().getType()] = _msg.getAchievePointInfo().getHadDrawMaxStep();

            //刷新红点
            _refreshRedTip();

            WinMsg.SendMsg(WinMsgType.ON_ACHIEVE_POINT_CHG, _msg.getAchievePointInfo().getType());
        }

        #endregion

        #region C2S

        /// <summary>
        /// 请求成就列表初始化信息
        /// </summary>
        public void reqAchieveInit()
        {
            NPGSClientListener.sendMsgByLog(GSWriter_002_InitOp.make_002_050_ReqAchieveInit());
        }

        /// <summary>
        /// 请求领取成就奖励
        /// </summary>
        /// <param name="_achieveId"></param>
        /// <param name="_step"></param>
        /// <param name="_doneAction"></param>
        public void reqDoneAchieveStep(long _achieveId, int _step, Action _doneAction)
        {
            NPGSClientListener.sendRequestByLog(NPGSWriter_021_PlayerInfoOp.make_025_ReqDoneAchieveStep(_achieveId, _step),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_021_025_RetDoneAchieveStep>((info) =>
                {
                    if (null != _doneAction)
                        _doneAction();
                }));
        }

        /// <summary>
        /// 请求领取成就点数奖励
        /// </summary>
        /// <param name="_achieveStepRewardId"></param>
        public void reqGainAchievePointReward(long _achieveStepRewardId, Action _callback)
        {
            NPGSClientListener.sendRequestByLog(NPGSWriter_021_PlayerInfoOp.make_026_ReqGainAchievePointReward(_achieveStepRewardId),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_021_026_RetGainAchievePointReward>((_msg) =>
                {
                    _callback?.Invoke();
                }));
        }

        #endregion
    }
}
