using ALPackage;
using GC2GS.p038_MarsOp;
using GS2GC.p002_InitOp;
using GS2GC.p038_MarsOp;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// Mars前往子组件
    /// </summary>
    public class MarsGoToSubComponent : _AMarsSubComponent
    {
        //当前到达火星节点信息
        private MarsStageInfo _m_curArrivedMarsStageInfo;
        //开始前往火星时间
        private long _m_lStartTimeMs;
        //是否到达火星
        private bool _m_bIsArriveMars;
        //日志列表
        private List<MarsStageLogInfo> _m_lLogInfoList;
        //初始化完成回调
        private Action<bool> _m_initCompleteCallback;
        //任务刷新定时器
        private ALCommonEnableTaskController _m_tcTickTaskController;

        /// <summary>
        /// 是否到达火星
        /// </summary>
        public bool isArriveMars { get { return _m_bIsArriveMars; } }
        /// <summary>
        /// 是否已经开始前往火星
        /// </summary>
        public bool isStartGoToMars { get { return _m_lStartTimeMs > 0; } }
        /// <summary>
        /// 当前火星节点信息
        /// </summary>
        public MarsStageInfo curArrivedMarsStageInfo { get { return _m_curArrivedMarsStageInfo; } }
        /// <summary>
        /// 第一个节点到达时间
        /// </summary>
        public long firstStageArrivedTimeMs { get { return _m_lStartTimeMs; } }

        public MarsGoToSubComponent([NotNull] MarsComponent _parentComp) : base(_parentComp)
        {
        }

        public override void init(Action<bool> _complete)
        {
            _m_initCompleteCallback = _complete;
            reqMarsGoRouteInit();
        }

        public override void discard()
        {
            _m_curArrivedMarsStageInfo = null;
            _m_lLogInfoList?.Clear();
            _m_tcTickTaskController.setDisable();
        }

        /// <summary>
        /// 获取已触发的日志信息列表
        /// </summary>
        /// <returns></returns>
        public List<MarsStageLogInfo> getAlreadyTriggerLogInfoList()
        {
            if (_m_lLogInfoList == null || _m_curArrivedMarsStageInfo == null || _m_curArrivedMarsStageInfo.marsGoRouteRef == null)
                return null;

            long curServerTimeMs = FpsAndPingMgr.instance.serverTimeTag;
            List<MarsStageLogInfo> logInfoList = new List<MarsStageLogInfo>();
            for (int i = 0; i < _m_lLogInfoList.Count; i++)
            {
                if (_m_lLogInfoList[i] == null)
                    continue;

                if (_m_lLogInfoList[i] != null && _m_lLogInfoList[i].triggerTimeMs <= curServerTimeMs && _m_curArrivedMarsStageInfo.marsGoRouteRef.stage_id >= _m_lLogInfoList[i].stageId)
                    logInfoList.Add(_m_lLogInfoList[i]);
            }

            return logInfoList;
        }

        /// <summary>
        /// 获取当前日志信息
        /// </summary>
        /// <returns></returns>
        public MarsStageLogInfo getCurLogInfo()
        {
            if (_m_curArrivedMarsStageInfo == null || _m_curArrivedMarsStageInfo.marsGoRouteRef == null ||_m_lLogInfoList == null)
                return null;

            long curServerTimeMs = FpsAndPingMgr.instance.serverTimeTag;
            MarsStageLogInfo targetInfo = null;
            for (int i = 0; i < _m_lLogInfoList.Count; i++)
            {
                MarsStageLogInfo tempInfo = _m_lLogInfoList[i];
                if (tempInfo == null)
                    continue;

                if (tempInfo.stageId == _m_curArrivedMarsStageInfo.marsGoRouteRef.stage_id && 
                    (targetInfo == null || (tempInfo.triggerTimeMs < curServerTimeMs && tempInfo.triggerTimeMs > targetInfo.triggerTimeMs)))
                    targetInfo = tempInfo;
            }

            return targetInfo;
        }

        /// <summary>
        /// 初始化日志列表
        /// </summary>
        private void _initLogList()
        {
            _m_lLogInfoList = new List<MarsStageLogInfo>();
            List<MarsGoRouteRefObj> refList = new List<MarsGoRouteRefObj>();
            refList.AddRange(GRefdataCoreMgr.instance.marsGoRouteRefCore.refList);
            refList.Sort((_a,_b)=>_a.stage_id.CompareTo(_b.stage_id));

            for (int i = 0; i < refList.Count; i++)
            {
                MarsGoRouteRefObj routeRef = refList[i];
                if(routeRef == null || routeRef.log_id_list == null || routeRef.log_id_list.Count <= 0)
                    continue;

                for (int j = 0; j < routeRef.log_id_list.Count; j++)
                {
                    MarsStageLogInfo logInfo = new MarsStageLogInfo(routeRef.stage_id, routeRef.log_id_list[j], j, routeRef.log_id_list.Count);
                    _m_lLogInfoList.Add(logInfo);
                }
            }
        }

        /// <summary>
        /// 刷新前往火星红点
        /// </summary>
        private void _refreshGoToRedTip()
        {
            long redTipCount = 0;

            //未到达火星
            if (!_m_bIsArriveMars)
            {
                //当前阶段已抵达
                if (_m_curArrivedMarsStageInfo != null && _m_curArrivedMarsStageInfo.endTimeMs - FpsAndPingMgr.instance.serverTimeTag <= 0)
                    redTipCount = 1;
            }
            else
            {
                //登录火星条件通过并且未选择登陆区域
                if (GCommon.isFuncUnlock(ENPFunctionType.LAND_MARS) && !NPPlayer.instance.playerInfoComp.clientDataRemarkInfo.isSelectLandingMarsArea())
                    redTipCount = 1;
            }

            //设置红点
            RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_MARS_GO_TO_NEXT, redTipCount);

            //如果已经到达并且选择了登陆区域，关闭刷新火星红点任务
            if (_m_bIsArriveMars && NPPlayer.instance.playerInfoComp.clientDataRemarkInfo.isSelectLandingMarsArea())
                _m_tcTickTaskController.setDisable();
        }

        #region S2C

        /// <summary>
        /// 前往火星数据初始化
        /// </summary>
        public void retMarsGoRouteInit(GS2GC_002_072_RetMarsGoRouteInit _msg)
        {
            _m_parentComponent.dealPreInitFunc(() =>
            {
                if (_msg == null)
                {
                    _m_initCompleteCallback?.Invoke(false);
                    return;
                }

                _m_bIsArriveMars = _msg.getIsAllDone();
                _m_lStartTimeMs = _msg.getStartMs();
                if (_msg.getStage() > 0)
                    _m_curArrivedMarsStageInfo = new MarsStageInfo(_msg.getStage(), _msg.getStageStartMs());

                //初始化日志列表
                if (_m_lStartTimeMs > 0 && (_m_lLogInfoList == null || _m_lLogInfoList.Count <= 0))
                    _initLogList();

                //开启任务刷新定时器
                _m_tcTickTaskController.setDisable();
                _m_tcTickTaskController = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(_refreshGoToRedTip, 1f);

                //初始化完成回调
                _m_initCompleteCallback?.Invoke(true);
            });
        }

        /// <summary>
        /// 前往火星-到达新阶段
        /// </summary>
        /// <param name="_msg"></param>
        public void onGoToStageArrived(GS2GC_038_050_OnGoToStageArrived _msg)
        {
            if (_msg == null)
                return;

            _m_lStartTimeMs = _msg.getStartMs();
            if (_m_curArrivedMarsStageInfo != null)
                _m_curArrivedMarsStageInfo.updateInfo(_msg.getStage(), _msg.getStageStartMs());
            else
                _m_curArrivedMarsStageInfo = new MarsStageInfo(_msg.getStage(), _msg.getStageStartMs());

            //初始化日志列表
            if (_m_lStartTimeMs > 0 && (_m_lLogInfoList == null || _m_lLogInfoList.Count <= 0))
                _initLogList();

            //发送埋点-到达火星新阶段
            GCommon.sendStepReport(TraceConst.GO_TO_MARS_ARRIVED_SRAGE.setMarkParam(_msg.getStage()));

            WinMsg.SendMsg(WinMsgType.ON_MARS_ARRIVED_NEW_STAGE, _msg.getStage());
        }

        /// <summary>
        /// 前往火星所有阶段完成
        /// </summary>
        public void onGoToAllStageDone()
        {
            _m_bIsArriveMars = true;

            //发送埋点-到达火星
            GCommon.sendStepReport(TraceConst.ARRIVED_MARS);
            //刷新显示
            GCommon.reloadCustomLoadPrefab();
        }

        #endregion

        #region C2S

        /// <summary>
        /// 请求初始化前往火星数据
        /// </summary>
        public void reqMarsGoRouteInit()
        {
            NPGSClientListener.sendMsgByLog(GSWriter_002_InitOp.make_002_072_ReqMarsGoRouteInit());
        }

        /// <summary>
        /// 请求开始前往火星
        /// </summary>
        /// <param name="_callback"></param>
        public void reqStartToGoMars(Action<bool> _callback = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_038_001_ReqStartToGoMars(),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_038_001_RetStartToGoMars>((_isSuc, _msg) =>
                {
                    if (_callback != null)
                        _callback(_isSuc);
                }));
        }

        /// <summary>
        /// 请求到达火星指定阶段
        /// </summary>
        /// <param name="_stage"></param>
        /// <param name="_callback"></param>
        public void reqArriveMarsStage(int _stage, Action<bool, GS2GC_038_002_RetArriveMarsStage> _callback = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_038_002_ReqArriveMarsStage(_stage),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_038_002_RetArriveMarsStage>((_isSuc, _msg) =>
                {
                    if (_callback != null)
                        _callback(_isSuc, _msg);
                }));
        }

        /// <summary>
        /// 发送登录成功跑马灯
        /// </summary>
        public void reqSendDoneMarquee()
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_038_003_ReqSendDoneMarquee(),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_038_003_RetSendDoneMarquee>(null));
        }

        /// <summary>
        /// 请求发送阶段留言
        /// </summary>
        public void reqSendStageMsg(int _stage, string _content)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_038_004_ReqSendStageMsg(_stage, _content),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_038_004_RetSendStageMsg>(null));
        }

        /// <summary>
        /// 请求阶段留言列表
        /// </summary>
        public void reqGetStageMsgList(int _stage, Action<GS2GC_038_005_RetGetStageMsgList> _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_038_005_ReqGetStageMsgList(_stage),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_038_005_RetGetStageMsgList>((_isSuc, _msg) =>
                {
                    if(_isSuc)
                        _callback?.Invoke(_msg);
                }));
        }

        #endregion
    }
}