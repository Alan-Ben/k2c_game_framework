using System;
using System.Collections.Generic;
using System.Linq;
using GC2GS.p002_InitOp;
using GC2GS.p004_PlayerOp;
using GS2GC.p002_InitOp;
using GS2GC.p004_PlayerOp;
using Common.RushExchangeObj;
using JetBrains.Annotations;
using NPEnum;

namespace GOE
{
    public class RushExchangeComponent : _ANPBasicPlayerComponent
    {
        [NotNull]private RushExchangeInfo _m_rushExchangeInfo; // 急速兑换信息


        public ERushExchangeState rushExchangeState => _m_rushExchangeInfo.state;
        public long rushExchangeShowTime => _m_rushExchangeInfo.showTimeMs;
        public long rushExchangeRefId => _m_rushExchangeInfo.refId;

        public RushExchangeComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
            _m_rushExchangeInfo = new RushExchangeInfo();
        }

        //属性
        public override bool isMustInit { get { return true; } }
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.RUSH_EXCHANGE; } }
        public override ENPPlayerCompType[] dependCompList { get { return new []{ENPPlayerCompType.BASIC_INFO}; } }

        /// <summary>
        /// 是否允许提前初始化。提前初始化的意思是在依赖项没有完成初始化之前就进行初始化操作（一般是提前发送消息）
        /// 在初始化结果消息返回的时候，通过特殊的初始化函数dealPreInitFunc进行处理函数注册，再依赖项完成之后才进行初始化处理
        /// </summary>
        public override bool canPreInit { get { return true; } }

        public override void presendInitProtocol()
        {
            //请求初始化
            _reqRushExchangeInit();
        }

        protected override void _dealInit()
        {
            //服务器版本小于0.5 直接完成
            if (MainCameraMono.instance.curServerVersionNum < ServerVersionConst.NEW_SERVER_VERSION_0_5_x)
                setInitDone();
        }

        protected override void _onInitDone()
        {
            _refreshRedTip();
        }

        protected override void _onInitFail()
        {
        }

        protected override void _discard()
        {
        }

        private void _refreshRedTip()
        {
        }
        
        public void checkRefresh()
        {
            _m_rushExchangeInfo.checkRefresh();
        }
        #region 消息

        /// <summary>
        /// 请求初始化协议
        /// </summary>
        private void _reqRushExchangeInit()
        {
            NPGSClientListener.sendMsgByLog(new GC2GS_002_085_ReqRushExchangeInit());
        }

        /// <summary>
        /// 初始化回包
        /// </summary>
        /// <param name="_info"></param>
        public void retRushExchangeInit(GS2GC_002_085_RetRushExchangeInit _info)
        {
            if (_info != null)
            {
                _m_rushExchangeInfo.updateInfo(_info.getInfo());
            }
            
            _refreshRedTip();
            setInitDone();
        }

        /// <summary>
        /// 急速兑换信息变更推送
        /// </summary>
        public void onRushExchangeChg(GS2GC_004_075_OnRushExchangeChg _msg)
        {
            if (_msg == null)
                return;

            _m_rushExchangeInfo.updateInfo(_msg.getInfo());

            // 刷新红点提示
            _refreshRedTip();

            // 发送消息通知UI更新
            WinMsg.SendMsg(WinMsgType.ON_RUSH_EXCHANGE_CHG);
        }


        /// <summary>
        /// 请求急速兑换
        /// </summary>
        /// <param name="_useGemSupplement">是否使用钻石补充不足道具</param>
        /// <param name="_callback">回调，返回是否成功</param>
        public void reqRushExchange(bool _useGemSupplement, Action<bool> _callback = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_004_101_ReqRushExchange(_useGemSupplement),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_004_101_RetRushExchange>((_isSuc, _msg) =>
                {
                    _callback?.Invoke(_isSuc);
                }));
        }

        /// <summary>
        /// 请求刷新急速兑换礼包
        /// </summary>
        /// <param name="_callback">回调，返回是否成功</param>
        public void reqRushExchangeRefresh(Action<bool> _callback = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_004_102_ReqRushExchangeRefresh(),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_004_102_RetRushExchangeRefresh>((_isSuc, _msg) =>
                {
                    _callback?.Invoke(_isSuc);
                }));
        }

        /// <summary>
        /// 领取急速兑换奖励
        /// </summary>
        /// <param name="_callback">回调，返回是否成功</param>
        public void reqRushExchangeReward(Action<bool> _callback = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_004_103_ReqRushExchangeReward(),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_004_103_RetRushExchangeReward>((_isSuc, _msg) =>
                {
                    _refreshRedTip();
                    _callback?.Invoke(_isSuc);
                }));
        }
        #endregion
    }
}