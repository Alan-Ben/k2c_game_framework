using System.Collections.Generic;
using System;
using ALPackage;
using Common.RankGiftPackObj;
using Common.TravelObj;
using Common.WeekCardObj;
using CommonEnum;
using GC2GS.p002_InitOp;
using GC2GS.p004_PlayerOp;
using JetBrains.Annotations;
using GS2GC.p002_InitOp;
using GS2GC.p004_PlayerOp;
using GS2GC.p021_PlayerInfo;
using NPEnum;

namespace GOE
{
    // 周卡模块管理类
    public class RankGiftPackComponent : _ANPBasicPlayerComponent
    {
        //当前礼包信息
        private RankGiftPackInfo _m_rankGiftPackInfo;
        
        //构造函数
        public RankGiftPackComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
            
        }

        public RankGiftPackInfo rankGiftPackInfo
        {
            get { return _m_rankGiftPackInfo; }
        }

        #region override
        public override bool isMustInit { get { return true; } }
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.RANK_GIFT_PACK; } }
        public override ENPPlayerCompType[] dependCompList { get { return null; } }
        #endregion
        
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
            NPGSClientListener.sendRequestByLog(new GC2GS_002_084_ReqRankGiftPackInit(), 
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_002_084_RetRankGiftPackInit>((_isSuc, _msg) =>
                {
                    dealPreInitFunc(() =>
                    {
                        if (!_isSuc || null == _msg)
                        {
                            _onInitFail();
                            return;
                        }

                        _m_rankGiftPackInfo = new RankGiftPackInfo(_msg.getGiftPackInfo());

                        setInitDone();
                    });
                }));
        }

        protected override void _dealInit()
        {

        }

        //组件加载完成时的调用
        protected override void _onInitDone()
        {
            _refreshRed();
        }

        private void _refreshRed()
        {
            
        }

        //组件初始化失败的处理
        protected override void _onInitFail()
        {
            ALLog.Error("RankGiftPackComponent init Fail!!!");
        }

        //释放资源函数
        protected override void _discard()
        {
            
        }
        
        public void OnRankGiftPackChg(GS2GC_004_064_OnRankGiftPackChg _msg)
        {
            if(null == _msg)
                return;
            
            if(null == _m_rankGiftPackInfo)
                _m_rankGiftPackInfo = new RankGiftPackInfo(_msg.getGiftPackInfo());
            else
            {
                _m_rankGiftPackInfo.update(_msg.getGiftPackInfo());
            }
            
            WinMsg.SendMsg(WinMsgType.ON_RANK_GIFT_CHG);
        }
        
        /// <summary>
        /// 请求请求购买
        /// </summary>
        public void reqBuyRankGiftPack(long _dbId, Action<GS2GC_004_039_RetBuyRankGiftPack> _succAction = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_004_039_ReqBuyRankGiftPack(_dbId),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_004_039_RetBuyRankGiftPack>((info) =>
                {
                    if (null != _succAction)
                        _succAction(info);
                }));
        }

    }
}
