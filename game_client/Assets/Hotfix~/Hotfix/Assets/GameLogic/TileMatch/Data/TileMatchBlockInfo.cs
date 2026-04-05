using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Hotfix
{
    /// <summary>
    /// 格子信息，格子的位置在初始化完成后就不会再变化了， 会变化的只有在格子上的数据和表现
    /// </summary>
    public class TileMatchBlockInfo
    {
        private Vector2Int _m_vLogicPos; //逻辑坐标

        private long _m_lBlockRefId;
        private TileMatchEnum.ETileMatch_ModeType _m_eModelType;//游戏模式

        private GGUIWndTileMatchChecker _m_blockShow;
        
        private TileMatchBlockRefObj _m_tileMatchBlockRefObj;//三消格子配表数据
        private TileMatchBlockShowRefObj _m_tileMatchBlockShowRefObj;//三消格子表现配表数据
        
        public TileMatchBlockInfo(int _logicPosIndex)
        {
            _m_vLogicPos = TileMatchUtil.indexToVector2(_logicPosIndex);
        }

        public TileMatchBlockInfo(Vector2Int _logicPos)
        {
            _m_vLogicPos = _logicPos;
        }

        public long blockRefId { get { return _m_lBlockRefId; } }
        public Vector2Int logicPos { get { return _m_vLogicPos; } }
        public int logicPosIndex { get { return TileMatchUtil.vector2ToIndex(_m_vLogicPos); } }
        public GGUIWndTileMatchChecker blockShow { get { return _m_blockShow; } }
        public TileMatchBlockRefObj tileMatchBlockRefObj { get { return _m_tileMatchBlockRefObj; } }
        public TileMatchBlockShowRefObj tileMatchBlockShowRefObj { get { return _m_tileMatchBlockShowRefObj; } }

        public NPCommonAssetPathInfo blockPrefabAssetPathInfo { get { return _m_tileMatchBlockShowRefObj?.prefab_asset_path; } }
        
        public void setBlockRefId(long _blockRefId)
        {
            _m_lBlockRefId = _blockRefId;
            
            _m_tileMatchBlockRefObj = HotfixRefdataCoreMgr.instance.tileMatchBlockRefCore.getRef(_blockRefId);
            if (_m_tileMatchBlockRefObj == null)
            {
                Debug.LogError($"[TileMatchBlockInfo] 找不到id:{_blockRefId} 的 格子配表数据TileMatchBlockRefObj");
            }

            _m_tileMatchBlockShowRefObj = HotfixRefdataCoreMgr.instance.getTileMatchBlockShowRefObj(_blockRefId, _m_eModelType);
            if (_m_tileMatchBlockShowRefObj == null)
            {
                Debug.LogError($"[TileMatchBlockInfo] 找不到_blockRefId:{_blockRefId} _modeType:{_m_eModelType} 的 格子表现配表数据TileMatchBlockShowRefObj");
            }
        }

        public void setBlockRefId(TileMatchEnum.ETileMatch_ModeType _matchModel, long _blockRefId)
        {
            _m_eModelType = _matchModel;
            
            setBlockRefId(_blockRefId);
        }
        
        public void resetBlockInfo()
        {
            _m_lBlockRefId = 0;
            _m_tileMatchBlockRefObj = null;
            _m_tileMatchBlockShowRefObj = null;
        }
        
        public void setBlockShow(GGUIWndTileMatchChecker _blockShow)
        {
            _m_blockShow = _blockShow;
            _m_blockShow?.setCheckerLogicPos(_m_vLogicPos);
        }

        public void resetBlockShow()
        {
            _m_blockShow = null;
        }

        public static int sort(TileMatchBlockInfo _a, TileMatchBlockInfo _b)
        {
            if (_a == null || _b == null)
                return 0;
            return _a.logicPosIndex.CompareTo(_b.logicPosIndex);
        }

        /// <summary>
        /// 判断两个格子的方向关系(_blockB在自己的哪个方向上)
        /// </summary>
        public ETileMatchDirection getCubeDirection(TileMatchBlockInfo _blockB)
        {
            if (_blockB == null)
                return ETileMatchDirection.None;

            return TileMatchUtil.getPosDirection(logicPos, _blockB.logicPos);
        }
    }
}