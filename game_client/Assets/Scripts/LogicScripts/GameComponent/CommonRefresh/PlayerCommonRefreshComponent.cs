
using System.Collections.Generic;
using ALPackage;
using Common.CommonFuncObj;
using GC2GS.p002_InitOp;
using GC2GS.p021_PlayerInfo;
using GS2GC.p002_InitOp;
using GS2GC.p021_PlayerInfo;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 经营事件组件
    /// </summary>
    public class PlayerCommonRefreshComponent : _ANPBasicPlayerComponent
    {
        private const float REFRESH_INTERVAL_S = 1f;
        
        [ItemNotNull, NotNull] private readonly List<CommonRefreshInfo> _m_commonRefreshList;
        [NotNull] private readonly HashSet<long> _m_refreshingIdSet;
        private ALCommonEnableTaskController _m_tickController;
        
        
        public PlayerCommonRefreshComponent(NPPlayerComponentMgr _compMgr) 
            : base(_compMgr)
        {
            _m_commonRefreshList = new List<CommonRefreshInfo>();
            _m_refreshingIdSet = new HashSet<long>();
        }
        

        public override bool isMustInit { get { return true; } }
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.COMMON_REFRESH; } }
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
            NPGSClientListener.sendRequestByLog(new GC2GS_002_035_ReqCommonRefreshInit(),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_002_035_RetCommonRefreshInit>((_isSuc, _msg) =>
                {
                    if (!_isSuc)
                    {
                        setInitFail();
                        return;
                    }

                    _initData(_msg);
                }));
        }
        protected override void _dealInit()
        {
        }
        //组件加载完成时的调用
        protected override void _onInitDone()
        {
            _m_tickController = ALCommonEnableDurationActionMonoTask.addMonoTask(_tickTask, REFRESH_INTERVAL_S);
        }
        protected override void _onInitFail()
        {
            ALLog.Error("PlayerCommonRefreshComponent init Fail!!!");
        }
        protected override void _discard()
        {
            _m_commonRefreshList.Clear();
            _m_tickController.setDisable();
        }


        private void _initData(GS2GC_002_035_RetCommonRefreshInit _msg)
        {
            List<CommonFunc_Refresh> serverList =_msg?.getInfoList();
            if (serverList == null)
            {
                setInitFail();
                return;
            }

            foreach (CommonFunc_Refresh serverInfo in serverList)
            {
                if (serverInfo == null)
                    continue;
                
                _m_commonRefreshList.Add(new CommonRefreshInfo(serverInfo));
            }
            _m_commonRefreshList.Sort(_sortFunc);
            
            setInitDone();
        }
        private void _tickTask()
        {
            long curTimeMs = FpsAndPingMgr.instance.serverTimeTag;
            foreach (CommonRefreshInfo info in _m_commonRefreshList)
            {
                // 如果刷新时间还没到，后面的也不用判断了
                if (info.refreshTimeMs > curTimeMs)
                    break;

                long id = info.id;
                if (_m_refreshingIdSet.Contains(id))
                    continue;
                
                _m_refreshingIdSet.Add(id);
                NPGSClientListener.sendRequestByLog(new GC2GS_021_041_ReqDealCommonRefresh(id),
                    new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_021_041_RetDealCommonRefresh>(
                        (_isSuc, _msg) => _m_refreshingIdSet.Remove(id)));
            }
        }
        private int _sortFunc(CommonRefreshInfo _a, CommonRefreshInfo _b)
        {
            return _a.refreshTimeMs.CompareTo(_b.refreshTimeMs);
        }
        
        internal void _onCommonRefreshChg(GS2GC_021_073_OnCommonRefreshChg _msg)
        {
            CommonFunc_Refresh serverInfo = _msg?.getRefreshData();
            if (serverInfo == null)
                return;
            
            foreach (CommonRefreshInfo info in _m_commonRefreshList)
            {
                if (info.id == serverInfo.getId())
                {
                    info._updateData(serverInfo.getNextRefreshTimeMs());
                    return;
                }
            }
            _m_commonRefreshList.Sort(_sortFunc);
        }
    }
}
