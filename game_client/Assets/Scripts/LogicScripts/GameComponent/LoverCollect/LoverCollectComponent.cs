using System;
using ALPackage;
using GC2GS.p002_InitOp;
using GC2GS.p004_PlayerOp;
using GS2GC.p002_InitOp;
using GS2GC.p004_PlayerOp;

namespace GOE
{
    /// <summary>
    /// 情人收集组件
    /// </summary>
    public class LoverCollectComponent : _ANPBasicPlayerComponent
    {
        /// <summary>
        /// 当前选中的情人配置ID，0=未选择
        /// </summary>
        private long _m_targetLoverId;
        /// <summary>
        /// 是否已领取当前目标情人
        /// </summary>
        private bool _m_isClaimed;
        
        private int _m_enableSerialize;
        

        public LoverCollectComponent(NPPlayerComponentMgr _compMgr)
            : base(_compMgr)
        {
        }

        protected static ENPPlayerCompType[] _g_DependComp = { ENPPlayerCompType.SPECIAL_ITEM };
        public override bool isMustInit { get { return true; } }
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.LOVER_COLLECT; } }
        public override ENPPlayerCompType[] dependCompList { get { return _g_DependComp; } }
        public override bool canPreInit { get { return true; } }

        /// <summary>
        /// 当前选中的情人配置ID，0=未选择
        /// </summary>
        public long targetLoverId { get { return _m_targetLoverId; } }
        /// <summary>
        /// 是否已领取当前目标情人
        /// </summary>
        public bool isClaimed { get { return _m_isClaimed; } }

        /// <summary>
        /// 数据变更事件
        /// </summary>
        public event Action onDataChg;
        

        public override void presendInitProtocol()
        {
            reqLoverCollectInit();
        }

        protected override void _dealInit()
        {
        }

        protected override void _onInitDone()
        {
            // 不确定 lazy dealer 要多久全部初始化完成，直接延迟几秒注册，简单稳定又好用！
            int serialize = _m_enableSerialize;
            ALCommonActionMonoTask.addMonoTask(() =>
            {
                if (serialize != _m_enableSerialize)
                    return;
                
                long curEarnings = NPPlayer.instance.specialItemComp.goldData.earnings;
                long targetEarnings = GRefdataCoreMgr.instance.npGeneral.lover_collect_need_earn_speed;
                if (curEarnings >= targetEarnings)
                    return;
                
                NPPlayer.instance.specialItemComp.goldData.onEarningsChg += _onEarningsChg;
            }, 3f);
        }

        protected override void _onInitFail()
        {
            ALLog.Error("LoverCollectComponent init fail");
        }

        protected override void _discard()
        {
            _m_enableSerialize = ALSerializeOpMgr.next();
            NPPlayer.instance.specialItemComp.goldData.onEarningsChg -= _onEarningsChg;
            
            _m_targetLoverId = 0;
            _m_isClaimed = false;
        }

        #region S2C

        /// <summary>
        /// 情人收集初始化返回
        /// </summary>
        public void retLoverCollectInit(GS2GC_002_088_RetLoverCollectInit _msg)
        {
            if (_msg == null)
            {
                setInitDone();
                return;
            }

            _m_targetLoverId = _msg.getTargetLoverId();
            _m_isClaimed = _msg.getIsClaimed();
            setInitDone();
        }

        /// <summary>
        /// 情人收集数据变更推送
        /// </summary>
        public void onLoverCollectChg(GS2GC_004_076_OnLoverCollectChg _msg)
        {
            if (_msg == null)
                return;

            _m_targetLoverId = _msg.getTargetLoverId();
            _m_isClaimed = _msg.getIsClaimed();
            onDataChg?.Invoke();
        }

        #endregion

        #region C2S

        /// <summary>
        /// 请求情人收集初始化
        /// </summary>
        public void reqLoverCollectInit()
        {
            NPGSClientListener.sendMsgByLog(GSWriter_002_InitOp.make_002_088_ReqLoverCollectInit());
        }

        /// <summary>
        /// 请求设置目标情人
        /// </summary>
        public void reqSetLoverTarget(long _loverId, Action<bool> _callback = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_004_104_ReqSetLoverTarget(_loverId),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_004_104_RetSetLoverTarget>((_isSuc, _msg) =>
                {
                    _callback?.Invoke(_isSuc);
                }));
        }

        /// <summary>
        /// 请求领取情人
        /// </summary>
        public void reqClaimLover(Action<bool> _callback = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_004_105_ReqClaimLover(),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_004_105_RetClaimLover>((_isSuc, _msg) =>
                {
                    _callback?.Invoke(_isSuc);
                }));
        }

        #endregion

        private void _onEarningsChg()
        {
            if (_m_isClaimed)
                return;

            long curEarnings = NPPlayer.instance.specialItemComp.goldData.earnings;
            long targetEarnings = GRefdataCoreMgr.instance.npGeneral.lover_collect_need_earn_speed;

            if (curEarnings >= targetEarnings)
            {
                NPPlayer.instance.specialItemComp.goldData.onEarningsChg -= _onEarningsChg;
                NPGUIAddSceneCenterTip.instance.showEmptyTip(GRefdataCoreMgr.instance.npGeneral.lover_collect_reach_target_tip_id);
            }
        }
    }
}
