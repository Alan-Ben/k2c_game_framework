using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine.Device;

namespace GOE
{
    /// <summary>
    /// 内存统一管理器
    /// </summary>
    public partial class MemoryMgr
    {
        private static MemoryMgr _m_instance;
        [NotNull]public static MemoryMgr instance
        {
            get
            {
                if(_m_instance == null)
                {
                    _m_instance = new MemoryMgr();
                }
                return _m_instance;
            }
        }
        
        private ENPGameMemoryLevel _m_memoryLevel;//当前内存评级
        private int _m_clothesUnitReserveCount;//玩家装扮内存保留个数
        private long _m_clothesUnitWaringCount;//玩家装扮内存检查个数,超过这个个数清理到保留个数
        
        public void init()
        {
            if (SystemInfo.systemMemorySize < 2048)
            {
                _m_memoryLevel = ENPGameMemoryLevel.LOW;
                _setConfigByLow();
            }
            else if (SystemInfo.systemMemorySize < 4096)
            {
                _m_memoryLevel = ENPGameMemoryLevel.NORMAL;
                _setConfigByNormal();
            }
            else
            {
                _m_memoryLevel = ENPGameMemoryLevel.HIGH;
                _setConfigByHigh();
            }

#if UNITY_EDITOR
            //editor下强制最低画质
            _m_memoryLevel = ENPGameMemoryLevel.LOW;
            _setConfigByLow();      
#endif
            
            //主城低内存回调
            Application.lowMemory += onCallBackLowMemory;
            
            //创建一个每帧lateupdate执行任务
            ALCommonTaskController.CommonEnableLateTickActionAddLaterMonoTask(_onLateUpdateTick);
        }
        
        //触发低内存警告回调
        public void onCallBackLowMemory()
        {            
            //发送埋点-触发低内存警告
            GCommon.sendStepReport(TraceConst.LOW_MEMORY_CALL_BACK.setMarkParam(
                UnityEngine.Time.realtimeSinceStartup,
                null == QueueMgr.instance._lastNode ? string.Empty : QueueMgr.instance._lastNode.nodeTag));
            
            //释放内存
            dealReleaseMemory();
        }

        //清理内存
        public void dealReleaseMemory()
        {
            //清理特效缓存
            SfxCache.instance.discardAllUnUseCacheItem();
            
            //清理showcase缓存
            ShowCaseResMgr.instance.discardAllUnUseCacheItems();
            
            //资源回收
            ALMemCollectMgr.instance.collect();
        }
        
        private void _onLateUpdateTick()
        {
        }
        
        private void _setConfigByLow()
        {
            _m_clothesUnitReserveCount = 0;
            _m_clothesUnitWaringCount = 10;
        }
        
        private void _setConfigByNormal()
        {
            _m_clothesUnitReserveCount = 10;
            _m_clothesUnitWaringCount = 20;
        }
        
        private void _setConfigByHigh()
        {
            _m_clothesUnitReserveCount = 30;
            _m_clothesUnitWaringCount = 60;
        }
    }
}