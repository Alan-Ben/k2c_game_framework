using System.Collections.Generic;
using ALPackage;
using System;
using Common.AchieveObj;
using Common.CommonFuncObj;
using CommonEnum;
using GS2GC.p002_InitOp;
using GS2GC.p007_CommOp;
using GS2GC.p021_PlayerInfo;
using JetBrains.Annotations;


namespace GOE
{
    //任务数据管理器
    public class CommonTargetRewardComponent : _ANPBasicPlayerComponent
    {
        //数据列表
        [NotNull] private Dictionary<long, CommonTargetRewardInfo> _m_commonTargetRewardInfoDict;
      
        public CommonTargetRewardComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
            _m_commonTargetRewardInfoDict = new Dictionary<long, CommonTargetRewardInfo>();
        }

        public override bool isMustInit { get { return true; } }

        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.COMMON_TARGET_REWARD; } }

        public override ENPPlayerCompType[] dependCompList { get { return null; } }
        /// <summary>
        /// 是否允许提前初始化。提前初始化的意思是在依赖项没有完成初始化之前就进行初始化操作（一般是提前发送消息）
        /// 在初始化结果消息返回的时候，通过特殊的初始化函数dealPreInitFunc进行处理函数注册，再依赖项完成之后才进行初始化处理
        /// </summary>
        public override bool canPreInit { get { return true; } }
        
        /// <summary>
        /// 发送初始化协议提前申请内容
        /// </summary>
        public override void presendInitProtocol()
        {
            //请求列表
            reqInit();
        }

        protected override void _dealInit()
        {
        }

        //组件加载完成时的调用
        protected override void _onInitDone()
        {
            WinMsg.RegisterMsgAct(WinMsgType.CUSTOM_RELOAD, _refreshAllRedTip);
            WinMsg.RegisterMsgAct(WinMsgType.ON_NODE_CHG, _refreshAllRedTip);
        }

        //组件初始化失败的处理
        protected override void _onInitFail()
        {
            ALLog.Error("CommonTargetRewardComponent init Fail!!!");
        }
        //释放资源函数
        protected override void _discard()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.CUSTOM_RELOAD, _refreshAllRedTip);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_NODE_CHG, _refreshAllRedTip);

            _m_commonTargetRewardInfoDict.Clear();
        }

        //所有组件初始化完以后
        public override void onAllCompInited()
        {
            _refreshAllRedTip();
            
            // 若国力目标已经解锁 且 还未展示过解锁红点
            if (GCommon.isSimpleUnlock(GRefdataCoreMgr.instance.npGeneral.national_power_target_simple_unlock_id) &&
                !AccountSettingMgr.instance.accountSetting.hadShowNationalPowerTargetUnlockRedTip)
            {
                if (RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_NATIONAL_POWER_TARGET_UNLOCK, 1))
                {
                    AccountSettingMgr.instance.accountSetting.setHadShowNationalPowerTargetUnlockRedTip(true);
                }
            }
        }
        
        /// <summary>
        /// 获取对应数据
        /// </summary>
        public CommonTargetRewardInfo getInfoById(long _id)
        {
            if(_m_commonTargetRewardInfoDict.TryGetValue(_id, out CommonTargetRewardInfo _info))
                return _info;
            return null;
        }
        
        /// <summary>
        /// 获取对应id的状态
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public ECommonRewardType getRewardType(long _id)
        {
            CommonTargetRewardInfo commonTargetRewardInfo = getInfoById(_id);
            if (null == commonTargetRewardInfo)
                return ECommonRewardType.NONE;
            
            return commonTargetRewardInfo.getRewardType();
        }
        
        #region S2C

        /// <summary>
        /// 初始化成就信息
        /// </summary>
        /// <param name="_msg"></param>
        public void RetTargetRewardInit(GS2GC_002_034_RetTargetRewardInit _msg)
        {
            if (_msg == null)
                return;
            
            _m_commonTargetRewardInfoDict.Clear();
            
            //服务端都给，以服务端数据为准
            foreach (CommonFunc_TargetReward commonFuncTargetReward in _msg.getInfoList())
            {
                CommonTargetRewardInfo commonTargetRewardInfo = new CommonTargetRewardInfo(commonFuncTargetReward);
                _m_commonTargetRewardInfoDict.Add(commonTargetRewardInfo.id, commonTargetRewardInfo);
            }
            
            setInitDone();
        }

        /// <summary>
        /// 成就信息变化
        /// </summary>
        /// <param name="_msg"></param>
        public void OnTargetRewardUpdate(GS2GC_007_062_OnTargetRewardUpdate _msg)
        {
            if(null == _msg || null == _msg.getInfo())
                return;

            CommonTargetRewardInfo commonTargetRewardInfo = getInfoById(_msg.getInfo().getId());
            if (null == commonTargetRewardInfo)
            {
                commonTargetRewardInfo = new CommonTargetRewardInfo(_msg.getInfo());
                _m_commonTargetRewardInfoDict.Add(commonTargetRewardInfo.id, commonTargetRewardInfo);
            }
            else
            {
                commonTargetRewardInfo.update(_msg.getInfo());
            }

            _refreshRedTip(commonTargetRewardInfo);
        }
        
        #endregion

        /// <summary>
        /// 刷新所有红点
        /// </summary>
        private void _refreshAllRedTip()
        {
            if(!isInited)
                return;
            
            foreach (var rewardInfo in _m_commonTargetRewardInfoDict.Values)
            {
                _refreshRedTip(rewardInfo);
            }
        }
        
        private void _refreshRedTip(CommonTargetRewardInfo _rewardInfo)
        {
            if(_rewardInfo == null || _rewardInfo.targetRewardRefObj == null || !isInited)
                return;

            if (!_rewardInfo.isShowConditionMet())
            {
                RedTipMgr.instance.setCountByRefRedTipId(_rewardInfo.targetRewardRefObj.red_tip_id, 0);
                return;
            }

            ECommonRewardType rewardType = _rewardInfo.getRewardType();
            if (rewardType == ECommonRewardType.CAN_GET_REWARD)
            {
                RedTipMgr.instance.setCountByRefRedTipId(_rewardInfo.targetRewardRefObj.red_tip_id, 1);
            }
            else
            {
                RedTipMgr.instance.setCountByRefRedTipId(_rewardInfo.targetRewardRefObj.red_tip_id, 0);
            }
        }
        
        /// <summary>
        /// 显示条件变动时调用，刷新所有红点
        /// </summary>
        public void onShowConditionChanged()
        {
            _refreshAllRedTip();
        }

        #region C2S

        /// <summary>
        /// 请求成就列表初始化信息
        /// </summary>
        public void reqInit()
        {
            NPGSClientListener.sendMsgByLog(GSWriter_002_InitOp.make_002_034_ReqTargetRewardInit());
        }
        
        /// <summary>
        /// 请求领奖
        /// </summary>
        public void reqDrawTargetReward(long _id, Action _callback)
        {
            NPGSClientListener.sendRequestByLog(GSWriter_007_CommOp.make_007_022_ReqDrawTargetReward(_id),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_007_022_RetDrawTargetReward>((_msg) =>
                {
                    _callback?.Invoke();
                }));
        }

        #endregion
    }
}
