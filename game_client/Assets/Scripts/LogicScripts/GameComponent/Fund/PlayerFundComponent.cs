using System;
using System.Collections.Generic;
using ALPackage;
using Common.ActivityFundObj;
using GC2GS.p002_InitOp;
using GS2GC.p002_InitOp;
using GS2GC.p017_ActivityOp;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 基金数据组件
    /// </summary>
    public class PlayerFundComponent : _ANPBasicPlayerComponent
    {
        [ItemNotNull, NotNull] private readonly List<FundInfo> _m_fundInfoList;
        [NotNull] private readonly ENPPlayerCompType[] _m_dependCompList;
        
        
        public PlayerFundComponent(NPPlayerComponentMgr _compMgr) 
            : base(_compMgr)
        {
            _m_fundInfoList = new List<FundInfo>();
            _m_dependCompList = new ENPPlayerCompType[] { ENPPlayerCompType.BAG, };
        }
        
        
        public override bool isMustInit { get { return true; } }
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.FUND; } }
        public override ENPPlayerCompType[] dependCompList { get { return _m_dependCompList; } }
        /// <summary>
        /// 是否允许提前初始化。提前初始化的意思是在依赖项没有完成初始化之前就进行初始化操作（一般是提前发送消息）
        /// 在初始化结果消息返回的时候，通过特殊的初始化函数dealPreInitFunc进行处理函数注册，再依赖项完成之后才进行初始化处理
        /// </summary>
        public override bool canPreInit { get { return true; } }
        /// <summary>
        /// 当前的基金信息列表（只读）
        /// </summary>
        public ReadOnlyList<FundInfo> fundInfoList { get { return _m_fundInfoList; } }
        
        public event Action<FundInfo> onFundInfoChg;
        public event Action<FundInfo> onFundAdd;
        public event Action<FundInfo> onFundRemove;


        public override void presendInitProtocol()
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_002_083_ReqActivityFundInit(), 
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_002_083_RetActivityFundInit>((_isSuc, _msg) =>
                {
                    dealPreInitFunc(() =>
                    {
                        if (!_isSuc)
                        {
                            _onInitFail();
                            return;
                        }

                        List<ActivityFund_Info> serverFundInfoList = _msg?.getFundList();
                        if (serverFundInfoList != null)
                        {
                            foreach (ActivityFund_Info serverFundInfo in serverFundInfoList)
                            {
                                FundInfo fundInfo = new FundInfo(serverFundInfo);
                                _m_fundInfoList.Add(fundInfo);
                            }
                        }

                        setInitDone();
                    });
                }));
        }
        protected override void _dealInit()
        {
            //服务器版本小于0.5 直接完成
            if(MainCameraMono.instance.curServerVersionNum < ServerVersionConst.NEW_SERVER_VERSION_0_5_x)
                setInitDone();
        }
        protected override void _onInitDone()
        {
            _onBagItemChg();
            WinMsg.RegisterMsgAct(WinMsgType.ON_BAG_ITEM_ADD, _onBagItemChg);
            WinMsg.RegisterMsgAct(WinMsgType.ON_BAG_ITEM_REMOVE, _onBagItemChg);
        }
        protected override void _onInitFail()
        {
            ALLog.Error("PlayerFundComponent init Fail!!!");
        }
        protected override void _discard()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_BAG_ITEM_ADD, _onBagItemChg);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_BAG_ITEM_REMOVE, _onBagItemChg);
            
            _m_fundInfoList.Clear();
        }
        
        
        /// <summary>
        /// 根据实例ID获取基金信息
        /// </summary>
        public FundInfo getFundInfoByFundId(long _fundId)
        {
            foreach (FundInfo fundInfo in _m_fundInfoList)
            {
                if (fundInfo.fundId == _fundId)
                    return fundInfo;
            }

            return null;
        }
        /// <summary>
        /// 根据活动ID获取基金信息
        /// </summary>
        public FundInfo getFundInfoByActivityId(long _activityId)
        {
            foreach (FundInfo fundInfo in _m_fundInfoList)
            {
                if (fundInfo.fundId == _activityId)
                    return fundInfo;
            }

            return null;
        }
        /// <summary>
        /// 检查是否有任意可领取奖励（用于红点）
        /// </summary>
        public bool checkHasAnyRewardCanDraw()
        {
            foreach (FundInfo fundInfo in _m_fundInfoList)
            {
                if (fundInfo.checkHasAnyRewardCanDraw())
                    return true;
            }

            return false;
        }
        /// <summary>
        /// 检查指定基金是否有可领取奖励（用于红点）
        /// </summary>
        public bool checkHasRewardCanDraw(long _instanceId)
        {
            FundInfo fundInfo = getFundInfoByFundId(_instanceId);
            if (fundInfo == null)
                return false;

            return fundInfo.checkHasAnyRewardCanDraw();
        }


        internal void _onFundScoreChg(GS2GC_017_064_OnActivityFundScoreChg _msg)
        {
            if (_msg == null)
                return;

            long fundId = _msg.getFundId();
            long formulaScore = _msg.getFormulaScore();
            long taskScore = _msg.getTaskScore();

            FundInfo fundInfo = getFundInfoByFundId(fundId);
            if (fundInfo != null)
            {
                fundInfo.updateScore(formulaScore, taskScore);
                onFundInfoChg?.Invoke(fundInfo);
                _refreshRedTip();
            }
        }
        internal void _onFundDrawRewardChg(GS2GC_017_066_OnActivityFundDrawRewardChg _msg)
        {
            if (_msg == null)
                return;

            long fundId = _msg.getFundId();
            int hadDrawFreeStep = _msg.getHadDrawFreeStep();
            int hadDrawPayStep = _msg.getHadDrawPayStep();

            FundInfo fundInfo = getFundInfoByFundId(fundId);
            if (fundInfo != null)
            {
                fundInfo.updateDrawReward(hadDrawFreeStep, hadDrawPayStep);
                onFundInfoChg?.Invoke(fundInfo);
                _refreshRedTip();
            }
        }
        internal void _onFundAdd(GS2GC_017_067_OnActivityFundAdd _msg)
        {
            ActivityFund_Info serverFundInfo = _msg?.getFundInfo();
            if (serverFundInfo == null)
                return;
            
            FundInfo fundInfo = new FundInfo(serverFundInfo);
            _m_fundInfoList.Add(fundInfo);
            onFundAdd?.Invoke(fundInfo);
            _refreshRedTip();
        }
        internal void _onFundRemove(GS2GC_017_068_OnActivityFundRemove _msg)
        {
            if (_msg == null)
                return;
            
            long fundId = _msg.getFundId();
            FundInfo fundInfo = getFundInfoByFundId(fundId);
            if (fundInfo != null)
            {
                _m_fundInfoList.Remove(fundInfo);
                onFundRemove?.Invoke(fundInfo);
                _refreshRedTip();
            }
        }


        private void _onBagItemChg()
        {
            foreach (FundInfo fundInfo in _m_fundInfoList)
            {
                if (fundInfo._refreshItemScore())
                    onFundInfoChg?.Invoke(fundInfo);
            }
            
            _refreshRedTip();
        }
        private void _refreshRedTip()
        {
            bool canDraw = checkHasAnyRewardCanDraw();
            RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_FUND, canDraw ? 1 : 0);
        }
    }
}