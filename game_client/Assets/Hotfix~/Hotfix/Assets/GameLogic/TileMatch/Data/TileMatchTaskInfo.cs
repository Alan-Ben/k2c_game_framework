using System.Collections.Generic;
using GOE;
using Hotfix.Common.TileMatchObj;
using IlRuntimeLitJson;

namespace Hotfix
{
    /// <summary>
    /// 三消任务信息
    /// </summary>
    public class TileMatchTaskInfo
    {
        /// <summary>
        /// 任务序列号(变化任务时更新)
        /// </summary>
        private int _m_lSerialId;
        
        /// <summary>
        /// 任务id
        /// </summary>
        private long _m_lTaskId;

        /// <summary>
        /// 任务配表数据
        /// </summary>
        private TileMatchTaskRefObj _m_rTaskRefObj;

        /// <summary>
        /// 已经进行的步数
        /// </summary>
        private int _m_iHadGoStep;
        
        /// <summary>
        /// 任务方块列表
        /// </summary>
        private List<TileMatchTaskBlockInfo> _m_lTaskBlockInfoList;

        #region 数据pool

        // private bool _m_bIsRelease;
        // private ObjectPool<TileMatchTaskInfo> _m_taskInfoPool;
        // private ObjectPool<TileMatchTaskBlockInfo> _m_blockInfoPool;
        // private ObjectPool<List<TileMatchTaskBlockInfo>> _m_blockInfoListPool;

        #endregion

        public TileMatchTaskInfo(TileMatch_TaskInfo _serverTaskInfo)
        {
            _updateAsNewInfo(_serverTaskInfo);
        }
        
        public int serialId => _m_lSerialId;
        public long taskId => _m_lTaskId;
        public TileMatchTaskRefObj taskRefObj => _m_rTaskRefObj;
        public int stepLimit => taskRefObj?.step_limit ?? 0;
        public int hadGoStep => _m_iHadGoStep;
        public bool reachStepLimit => _m_iHadGoStep >= stepLimit;
        public List<TileMatchTaskBlockInfo> taskBLockInfoList => _m_lTaskBlockInfoList;

        /// <summary>
        /// 这个更新只会在任务是相同任务时更新, 若不同的任务则不会更新
        /// </summary>
        /// <param name="_serverTaskInfo"></param>
        public void updateInfo(TileMatch_TaskInfo _serverTaskInfo)
        {
            if(_serverTaskInfo == null)
                return;

            // 任务序列号不同, 说明任务发生变化
            if (_serverTaskInfo.getSerialId() != _m_lSerialId)
            {
                Debug.LogError($"[TileMatchTaskInfo updateInfo] 尝试使用updateInfo方法更新序列号不同的任务数据。 服务器任务序列号: {_serverTaskInfo.getSerialId()}, 当前客户端任务序列号: {_m_lSerialId}");
                return;
            }
            
            if (_serverTaskInfo.getTaskId() != _m_lTaskId)
            {
                Debug.LogError($"[TileMatchTaskInfo updateInfo] 序列号:{_m_lSerialId}的任务id不匹配, 服务器任务id: {_serverTaskInfo.getTaskId()}, 当前任务id: {_m_lTaskId}");
                return;
            }
            
            List<TileMatch_TaskSubInfo> serverTaskBlockInfoList = _serverTaskInfo.getSubList();//服务端任务格子数据
            int serverTaskBlockInfoListCount = serverTaskBlockInfoList?.Count ?? 0;
            int customTaskBlockInfoListCount = _m_lTaskBlockInfoList?.Count ?? 0;
            // 将任务格子数量是否相同作为判断任务格子是否不正确的判断依据，不对任务格子的具体数据判断是否相同
            if(serverTaskBlockInfoListCount == 0 || customTaskBlockInfoListCount == 0 || serverTaskBlockInfoListCount != customTaskBlockInfoListCount)
            {
                Debug.LogError($"[TileMatchTaskInfo updateInfo] 任务:{_m_lTaskId} 服务器给的数据中的任务格子数据:{HotfixGCommon.GetInfoPropertys(serverTaskBlockInfoList)} " +
                               $"与 客户端当前任务的格子数据:{JsonMapper.ToJson(_m_lTaskBlockInfoList)} 数量无法对应");
                
                return;
            }
            
            // 上面判断了数量相同, 且不为0
            for (int i = 0; i < serverTaskBlockInfoListCount; i++)
            {
                TileMatch_TaskSubInfo serverTaskBlockInfo = serverTaskBlockInfoList[i];
                TileMatchTaskBlockInfo customTaskBlockInfo = _m_lTaskBlockInfoList[i];

                if (customTaskBlockInfo == null)
                {
                    customTaskBlockInfo = new TileMatchTaskBlockInfo();
                    _m_lTaskBlockInfoList[i] = customTaskBlockInfo;
                }
                
                customTaskBlockInfo.updateInfo(serverTaskBlockInfo?.getBlockId() ?? 0, serverTaskBlockInfo?.getDoneNum() ?? 0, _m_rTaskRefObj?.chess_pieces_num?.SafeGet(i) ?? 0);
            }

            _m_iHadGoStep = _serverTaskInfo.getHadGoStep();
        }

        /// <summary>
        /// 作为全新的任务信息更新
        /// </summary>
        private void _updateAsNewInfo(TileMatch_TaskInfo _serverTaskInfo)
        {
            if (_serverTaskInfo == null)
            {
                Debug.LogError($"[TileMatchTaskInfo _updateAsNewInfo] _serverTaskInfo is null, cannot update TileMatchTaskInfo");
                return;
            }

            _m_lSerialId = _serverTaskInfo.getSerialId();
            _m_lTaskId = _serverTaskInfo.getTaskId();
            _m_rTaskRefObj = HotfixRefdataCoreMgr.instance.tileMatchTaskRefCore.getRef(_m_lTaskId);
            _m_iHadGoStep = _serverTaskInfo.getHadGoStep();

            if (_m_lTaskBlockInfoList == null)
                _m_lTaskBlockInfoList = new List<TileMatchTaskBlockInfo>();
            _m_lTaskBlockInfoList.Clear();
            
            if (_m_rTaskRefObj == null)
            {
                Debug.LogError($"[TileMatchTaskInfo _updateAsNewInfo] taskId: {_m_lTaskId}对应的TileMatchTaskRefObj(tilematch_task)配表数据找不到");
            }
            else
            {
                List<TileMatch_TaskSubInfo> serverTaskBlockInfoList = _serverTaskInfo.getSubList();
                List<int> blockNeedTotalNumList = _m_rTaskRefObj.chess_pieces_num;
                
                int serverTaskBlockInfoListCount = serverTaskBlockInfoList?.Count ?? 0;
                int blockNeedTotalNumListCount = blockNeedTotalNumList?.Count ?? 0;
                if (serverTaskBlockInfoListCount == 0 || blockNeedTotalNumListCount == 0 || serverTaskBlockInfoListCount != blockNeedTotalNumListCount)
                {
                    Debug.LogError($"[TileMatchTaskInfo _updateAsNewInfo] 任务:{_m_lTaskId} 服务器给的数据中的任务格子数据:{HotfixGCommon.GetInfoPropertys(serverTaskBlockInfoList)} " +
                                   $"与 配表数据(TileMatchTaskRefObj tilematch_task) 的chess_pieces_num字段:{JsonMapper.ToJson(_m_rTaskRefObj.chess_pieces_num)} 数量无法对应");
                }
                else
                {
                    // 上面判断了数量相同, 且不为0
                    for (int i = 0; i < serverTaskBlockInfoListCount; i++)
                    {
                        TileMatch_TaskSubInfo serverTaskBlockInfo = serverTaskBlockInfoList[i];
                        int blockNeedTotalNum = blockNeedTotalNumList[i];
            
                        TileMatchTaskBlockInfo blockInfo = new TileMatchTaskBlockInfo(serverTaskBlockInfo?.getBlockId() ?? 0, serverTaskBlockInfo?.getDoneNum() ?? 0, blockNeedTotalNum);
                        _m_lTaskBlockInfoList.Add(blockInfo);
                    }
                }
            }
        }

        /// <summary>
        /// 任务是否完成
        /// </summary>
        /// <returns></returns>
        public bool isDone()
        {
            if (_m_lTaskBlockInfoList == null)
                return true;

            TileMatchTaskBlockInfo taskBlockInfo = null;
            for (int i = 0, count = _m_lTaskBlockInfoList.Count; i < count; i++)
            {
                taskBlockInfo = _m_lTaskBlockInfoList[i];
                if (taskBlockInfo != null && !taskBlockInfo.isDone())
                    return false;
            }

            return true;
        }

        public override string ToString()
        {
            return $"_m_lSerialId:{_m_lSerialId}  _m_lTaskId:{_m_lTaskId}  _m_iHadGoStep:{_m_iHadGoStep}  stepLimit:{stepLimit}  " +
                   $"isDone:{isDone()}  _m_lTaskBlockInfoList:{JsonMapper.ToJson(_m_lTaskBlockInfoList)}";
        }

        #region pool做法

        // public void onGetFromPool(ObjectPool<TileMatchTaskInfo> _taskInfoPool, ObjectPool<TileMatchTaskBlockInfo> _tileMatchTaskBlockInfoPool, ObjectPool<List<TileMatchTaskBlockInfo>> _tileMatchTaskBlockInfoListPool)
        // {
        //     if (!_m_bIsRelease)
        //     {
        //         Debug.LogError($"[TileMatchTaskInfo onGetFromPool] TileMatchTaskInfo 还未被释放, 但是又再次调用了onGetFromPool, 需要检查逻辑是否出现问题");
        //     }
        //     
        //     _m_bIsRelease = false;
        //     
        //     _m_taskInfoPool = _taskInfoPool;
        //     _m_blockInfoPool = _tileMatchTaskBlockInfoPool;
        //     _m_blockInfoListPool = _tileMatchTaskBlockInfoListPool;
        // }
        //
        // public void updateData(TileMatch_TaskInfo _serverTaskInfo)
        // {
        //     if (_m_bIsRelease)
        //     {
        //         Debug.LogError($"[TileMatchTaskInfo updateData] TileMatchTaskInfo is already released, cannot update data. TaskId: {_m_lTaskId}");
        //         return;
        //     }
        //     
        //     if (_serverTaskInfo == null)
        //     {
        //         Debug.LogError("TileMatchTaskInfo: updateData called with null _serverTaskInfo");
        //         return;
        //     }
        //
        //     _m_lTaskId = _serverTaskInfo.getTaskId();
        //     _m_rTaskRefObj = HotfixRefdataCoreMgr.instance.tileMatchTaskRefCore.getRef(_m_lTaskId);
        //     _m_iHadGoStep = _serverTaskInfo.getHadGoStep();
        //     
        //     if (_m_lTaskBlockInfoList == null)
        //     {
        //         if (_m_blockInfoListPool != null)
        //             _m_lTaskBlockInfoList = _m_blockInfoListPool.Get();
        //         if(_m_lTaskBlockInfoList == null)
        //             _m_lTaskBlockInfoList = new List<TileMatchTaskBlockInfo>();
        //     }
        //     _clearTaskBlockInfoList();
        //     
        //     if (_m_rTaskRefObj == null)
        //     {
        //         Debug.LogError($"TileMatchTaskInfo: TaskRefObj is null, taskId: {_m_lTaskId}");
        //     }
        //     else
        //     {
        //         List<int> blockIdList = _serverTaskInfo.getBlockIdList();
        //         List<int> blockDoneNumList = _serverTaskInfo.getDoneNumList();
        //         List<int> blockNeedTotalNumList = _m_rTaskRefObj.chess_pieces_num;
        //         
        //         int blockIdListCount = blockIdList?.Count ?? 0;
        //         int blockDoneNumListCount = blockDoneNumList?.Count ?? 0;
        //         int blockNeedTotalNumListCount = blockNeedTotalNumList?.Count ?? 0;
        //         if (blockIdListCount == 0 || blockDoneNumListCount == 0 || blockNeedTotalNumListCount == 0 || 
        //             !(blockIdListCount == blockDoneNumListCount && blockDoneNumListCount == blockNeedTotalNumListCount))
        //         {
        //             Debug.LogError($"[TileMatchTaskInfo ctor] 任务:{_m_lTaskId} 服务器给的数据 getBlockIdList():{JsonMapper.ToJson(_serverTaskInfo.getBlockIdList())} 或 getDoneNumList():{JsonMapper.ToJson(_serverTaskInfo.getDoneNumList())} " +
        //                            $"或 配表数据(TileMatchTaskRefObj tilematch_task) 的chess_pieces_num字段:{JsonMapper.ToJson(_m_rTaskRefObj.chess_pieces_num)} 有问题, 三者的任务格子数量无法对应");
        //         }
        //         else
        //         {
        //             // 上面判断了 3个数量相同, 且不为0
        //             for (int i = 0; i < blockIdListCount; i++)
        //             {
        //                 int blockId = blockIdList[i];
        //                 int blockDoneNum = blockDoneNumList[i];
        //                 int blockNeedTotalNum = blockNeedTotalNumList[i];
        //
        //                 TileMatchTaskBlockInfo blockInfo = null;
        //                 if (_m_blockInfoPool != null)
        //                     blockInfo = _m_blockInfoPool.Get();
        //                 if(blockInfo == null)
        //                     blockInfo = new TileMatchTaskBlockInfo();
        //                 
        //                 blockInfo.updateInfo(blockId, blockDoneNum, blockNeedTotalNum);
        //                 _m_lTaskBlockInfoList.Add(blockInfo);
        //             }
        //         }
        //     }
        // }
        //
        // /// <summary>
        // /// 清空任务格子信息列表
        // /// </summary>
        // private void _clearTaskBlockInfoList()
        // {
        //     if (_m_lTaskBlockInfoList != null)
        //     {
        //         TileMatchTaskBlockInfo taskBlockInfo = null;
        //         for(int i = 0, count = _m_lTaskBlockInfoList.Count;i < count; i++)
        //         {
        //             taskBlockInfo = _m_lTaskBlockInfoList[i];
        //             if(taskBlockInfo == null)
        //                 continue;
        //             
        //             if(_m_blockInfoPool != null)
        //                 _m_blockInfoPool.Release(taskBlockInfo);
        //         }
        //         _m_lTaskBlockInfoList.Clear();
        //     }
        // }
        //
        // /// <summary>
        // /// 回收自身
        // /// </summary>
        // public void release()
        // {
        //     if(_m_bIsRelease)
        //         return;
        //
        //     _m_bIsRelease = true;
        //     
        //     _m_rTaskRefObj = null;
        //
        //     _clearTaskBlockInfoList();
        //     if(_m_blockInfoListPool != null)
        //         _m_blockInfoListPool.Release(_m_lTaskBlockInfoList);
        //     _m_lTaskBlockInfoList = null;
        //
        //     ObjectPool<TileMatchTaskInfo> tmpTaskInfoPool = _m_taskInfoPool;
        //     _m_taskInfoPool = null;
        //     _m_blockInfoPool = null;
        //     _m_blockInfoListPool = null;
        //     tmpTaskInfoPool?.Release(this);
        // }

        #endregion
    }

    /// <summary>
    /// 三消任务格子信息
    /// </summary>
    public class TileMatchTaskBlockInfo
    {
        private long _m_lBlockId;//格子id

        private int _m_iDoneNum;//已经完成的数量
        
        private int _m_iNeedTotalNum;//需要完成的总数量

        public TileMatchTaskBlockInfo()
        {
        }
        
        public TileMatchTaskBlockInfo(long _blockId, int _doneNum, int _needTotalNum)
        {
            updateInfo(_blockId, _doneNum, _needTotalNum);
        }
        
        public void updateInfo(long _blockId, int _doneNum, int _needTotalNum)
        {
            _m_lBlockId = _blockId;
            _m_iDoneNum = _doneNum;
            _m_iNeedTotalNum = _needTotalNum;
        }
        
        public long blockId => _m_lBlockId;
        public int doneNum => _m_iDoneNum;
        public int needTotalNum => _m_iNeedTotalNum;
        
        public bool isDone()
        {
            return _m_iDoneNum >= _m_iNeedTotalNum;
        }

        public override string ToString()
        {
            return $"_m_lBlockId:{_m_lBlockId} _m_iDoneNum:{_m_iDoneNum} _m_iNeedTotalNum:{_m_iNeedTotalNum}";
        }
    }
}