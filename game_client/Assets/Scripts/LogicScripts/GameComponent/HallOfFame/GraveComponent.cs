using System;
using System.Collections.Generic;
using Common.GraveObj;
using GC2GS.p002_InitOp;
using GC2GS.p004_PlayerOp;
using GS2GC.p002_InitOp;
using GS2GC.p004_PlayerOp;
using JetBrains.Annotations;
using NPCommon;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 杰出者大厅管理组件
    /// </summary>
    public class GraveComponent : _ANPBasicPlayerComponent
    {
        private bool _m_hasGraveNewReward; // 是否有新的杰出者
        private bool _m_canCelebrate ; // 是否可以庆祝杰出者
        public GraveComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
        }

        //属性
        public override bool isMustInit { get { return true; } }
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.GRAVE; } }
        public override ENPPlayerCompType[] dependCompList { get { return null; } }

        /// <summary>
        /// 是否允许提前初始化。提前初始化的意思是在依赖项没有完成初始化之前就进行初始化操作（一般是提前发送消息）
        /// 在初始化结果消息返回的时候，通过特殊的初始化函数dealPreInitFunc进行处理函数注册，再依赖项完成之后才进行初始化处理
        /// </summary>
        public override bool canPreInit { get { return true; } }

        public override void presendInitProtocol()
        {
            //请求初始化
            _reqGraveInit();
        }

        protected override void _dealInit()
        {
        }

        protected override void _onInitDone()
        {
            WinMsg.RegisterMsgAct(WinMsgType.ON_CROSS_DAY, _onCrossDay);
        }

        protected override void _onInitFail()
        {
        }

        protected override void _discard()
        {
            WinMsg.RegisterMsgAct(WinMsgType.ON_CROSS_DAY, _onCrossDay);
        }

        /// <summary>
        /// 是否有新的杰出者
        /// </summary>
        public bool hasGraveNewReward => _m_hasGraveNewReward;
        public bool canCelebrate => _m_canCelebrate;

        private void _refreshRedTip()
        {
            NPPlayerFixedCDInfo cdInfo = NPPlayer.instance.fixedCdComp.getCDInfoByRefId(GRefdataCoreMgr.instance.npGeneral.grave_celebrate_fixed_cd_id);
            int count = _m_hasGraveNewReward ? 1 : 0;
            if (cdInfo != null && _m_canCelebrate)
                count+= cdInfo.getCount();
            RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_GRAVE_MAIN, count);
        }

        private void _onCrossDay()
        {
            _refreshRedTip();
        }
        #region 消息

        /// <summary>
        /// 请求初始化协议
        /// </summary>
        private void _reqGraveInit()
        {
            NPGSClientListener.sendMsgByLog(new GC2GS_002_068_ReqGraveInit());
        }
        
        /// <summary>
        /// 初始化回包
        /// </summary>
        /// <param name="_info"></param>
        public void retGraveInit(GS2GC_002_068_RetGraveInit _info)
        {
            _m_hasGraveNewReward = _info.getHasGraveNewReward();
            _m_canCelebrate = _info.getHasGraveRecord();
            _refreshRedTip();
            setInitDone();
        }

        
        /// <summary>
        /// 请求新晋杰出者列表
        /// </summary>
        /// <param name="_action"></param>
        public void reqGraveNewInfoList(Action<List<GraveObj_NewInfo>> _action)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_004_041_ReqGraveNewInfoList(),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_004_041_RetGraveNewInfoList>(
                    _info =>
                    {
                        if (_action == null || _info == null) return;
                        if (_info.getNewInfoList().Count <= 0)
                            _m_hasGraveNewReward = false; 
                        _action.Invoke(_info.getNewInfoList());
                    }));
        }

        /// <summary>
        /// 请求杰出者数据记录
        /// </summary>
        /// <param name="_typeId"></param>
        /// <param name="_curPage"></param>
        /// <param name="_pageCount"></param>
        /// <param name="_action">总数量，当前页的数据</param>
        public void reqGraveRecordList(int _typeId, int _curPage, int _pageCount, Action<int, List<GraveObj_Record>> _action)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_004_042_ReqGraveRecordList(_typeId, _curPage, _pageCount),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_004_042_RetGraveRecordList>(
                    _info =>
                    {
                        if (_action == null || _info == null) return;
                        _action.Invoke(_info.getCount(), _info.getRecordList());
                    }));
        }

        /// <summary>
        /// 请求杰出者膜拜
        /// </summary>
        /// <param name="_callBack"></param>
        public void reqGraveCelebrate(Action<long, long, List<NPCommon_ItemInfo>> _callBack)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_004_043_ReqGraveCelebrate(),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_004_043_RetGraveCelebrate>(
                    (_suc, _msg) =>
                    {
                        if (_suc && _msg != null)
                        {
                            _callBack?.Invoke(_msg.getCid(), _msg.getBuffId(), _msg.getGainItemList());
                            _refreshRedTip();
                        }
                    }));
        }

        /// <summary>
        /// 可庆祝新晋杰出者列表
        /// </summary>
        /// <param name="_action"></param>
        public void reqGraveConNewInfoList(Action<List<GraveObj_NewInfo>> _action)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_004_044_ReqGraveConNewInfoList(),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_004_044_RetGraveConNewInfoList>(
                    _info =>
                    {
                        if (_action == null || _info == null) return;
                        _action.Invoke(_info.getNewInfoList());
                    }));
        }

        /// <summary>
        /// 庆祝新晋杰出者
        /// </summary>
        /// <param name="_titleId"></param>
        /// <param name="_cid"></param>
        /// <param name="_callBack"></param>
        public void reqGraveConNewInfo(Action<long, List<NPCommon_ItemInfo>> _callBack)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_004_045_ReqGraveConNewInfo(),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_004_045_RetGraveConNewInfo>(
                    (_suc, _msg) =>
                    {
                        if (_suc && _msg != null)
                        {
                            _m_hasGraveNewReward = false;
                            _refreshRedTip();
                            WinMsg.SendMsg(WinMsgType.ON_GRAVE_NEW_CHANGE);
                            _callBack?.Invoke(_msg.getCid(), _msg.getGainItemList());
                        }
                    }));
        }
        
        /// <summary>
        /// 新杰出者奖励通知
        /// </summary>
        /// <param name="_msg"></param>
        public void onGraveNewReward(GS2GC_004_062_OnGraveNewReward _msg)
        {
            if (_msg != null)
            {
                _m_hasGraveNewReward = true;
                _m_canCelebrate = true;
                WinMsg.SendMsg(WinMsgType.ON_GRAVE_NEW_CHANGE);
                _refreshRedTip();
            }
        }
        #endregion
    }
}