
using System;
using System.Collections;
using System.Collections.Generic;
using ALPackage;
using Common.ActivityObj;
using GC2GS.p002_InitOp;
using GS2GC.p002_InitOp;
using GS2GC.p007_CommOp;
using GS2GC.p017_ActivityOp;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 活动相关热更配表组件
    /// </summary>
    public class CommonActivityHotRefComponent : _ANPBasicPlayerComponent
    {
        [NotNull]private List<ActivityHotRefPatchInfo> _m_activityHotRefPatchInfoList = new List<ActivityHotRefPatchInfo>();
        
        public CommonActivityHotRefComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
            
        }

        protected static ENPPlayerCompType[] _g_DependComp = { ENPPlayerCompType.BASIC_INFO };
        public override bool isMustInit { get { return true; } }
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.COMMON_ACTIVITY_HOT_REF; } }
        public override ENPPlayerCompType[] dependCompList { get { return _g_DependComp; } }
        public override bool canPreInit { get { return true; } }

        public override void presendInitProtocol()
        {
            
        }

        protected override void _dealInit()
        {
            _reqActivityHotRefList();
        }

        protected override void _onInitDone()
        {
            float startTime = Time.realtimeSinceStartup;

            ALCoroutineDealerMgr.instance.addCoroutine(new ALCoroutineWrapper(_patchAll(() =>
            {
                WinMsg.SendMsg(WinMsgType.HOT_REF_PATCH_COMPLETE);
                
                //发送消息热更配表补丁全部完成
                float useTime = Time.realtimeSinceStartup - startTime;
                GCommon.sendStepReport(TraceConst.PATCH_LIST_USE_TIME.setMarkParam(useTime));
            })));
        }

        protected override void _onInitFail()
        {
        }

        protected override void _discard()
        {
            _discardAll();
        }

        private void _discardAll()
        {
            foreach (_ACommonHotRefPatchInfo activityHotRefPatchInfo in _m_activityHotRefPatchInfoList)
            {
                if (activityHotRefPatchInfo != null) 
                    activityHotRefPatchInfo.reset();
            }
            _m_activityHotRefPatchInfoList.Clear();
        }

        private IEnumerator _patchAll(Action _completeCall)
        {
            ALStepCounter stepCounter = new ALStepCounter();
            int max = _m_activityHotRefPatchInfoList.Count;
            if (max == 0)
            {
                if (_completeCall != null) 
                    _completeCall();
                yield break;
            }

            stepCounter.chgTotalStepCount(max);
            stepCounter.regAllDoneDelegate(_completeCall);

            for (int i = 0; i < max; i++)
            {
                ActivityHotRefPatchInfo patchInfo = _m_activityHotRefPatchInfoList[i];
                if (null == patchInfo)
                {
                    stepCounter.addDoneStepCount();
                    continue;
                }
                
                //补丁操作
                patchInfo.startPatch((_isSuc) => { stepCounter.addDoneStepCount(); });
                // 分帧处理，避免多线程密集处理占用资源导致下载异常（下载超时）
                yield return null;
            }
        }

        /// <summary>
        /// 根据活动实例ID打补丁
        /// </summary>
        /// <param name="_activityInstanceId"></param>
        /// <param name="_completeCall"></param>
        public void patchTableByActivityInstanceId(long _activityInstanceId, Action<bool> _completeCall = null)
        {
            ActivityHotRefPatchInfo targetPatchInfo = null;
            foreach (ActivityHotRefPatchInfo activityHotRefPatchInfo in _m_activityHotRefPatchInfoList)
            {
                if (activityHotRefPatchInfo != null && activityHotRefPatchInfo.activityInstanceId == _activityInstanceId)
                {
                    targetPatchInfo = activityHotRefPatchInfo;
                    break;
                }
            }

            if (null == targetPatchInfo)
            {
                if (_completeCall != null)
                    _completeCall(false);
                return;
            }

            //补丁操作
            targetPatchInfo.startPatch(_completeCall);
        }
        
        #region C2S

        /// <summary>
        /// (初始化数据)
        /// </summary>
        private void _reqActivityHotRefList()
        {
            _discardAll();
            
            NPGSClientListener.sendRequestByLog(new GC2GS_002_079_ReqActivityHotRefList(),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_002_079_RetActivityHotRefList>(_info =>
                {
                    if (null == _info)
                        return;
                    
                    foreach (Activity_HotRefInfo itemInfo in _info.getHotRefList())
                    {
                        if(null == itemInfo)
                            continue;
                        
                        ActivityHotRefPatchInfo aCommonHotRefPatch = new ActivityHotRefPatchInfo(
                            itemInfo.getActivityInstanceId(),
                            itemInfo.getActivityId(),
                            itemInfo.getFileName(),
                            itemInfo.getFileMd5(),
                            itemInfo.getFileDir());
                        
                        _m_activityHotRefPatchInfoList.Add(aCommonHotRefPatch);
                    }
                    
                    setInitDone();
                }));
        }

        /// <summary>
        /// 推送热更配表数据修改
        /// </summary>
        /// <param name="_info"></param>
        public void onActivityHotRefInfoChg(GS2GC_017_062_OnActivityHotRefInfoChg _info)
        {
            if(null == _info || null == _info.getHotRefInfo())
                return;
            
            Activity_HotRefInfo itemInfo = _info.getHotRefInfo();
            if(null == itemInfo)
                return;
            
            ActivityHotRefPatchInfo aCommonHotRefPatch = new ActivityHotRefPatchInfo(
                itemInfo.getActivityInstanceId(),
                itemInfo.getActivityId(),
                itemInfo.getFileName(),
                itemInfo.getFileMd5(),
                itemInfo.getFileDir());
            _m_activityHotRefPatchInfoList.Add(aCommonHotRefPatch);
            
            aCommonHotRefPatch.startPatch(null);
        }
        
        #endregion
    }
}