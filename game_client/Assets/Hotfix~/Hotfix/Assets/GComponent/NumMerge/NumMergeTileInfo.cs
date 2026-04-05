using Hotfix.Common.NumMergeObj;
using UnityEngine;

namespace Hotfix
{
    /// <summary>
    /// 2048游戏单个格子信息
    /// </summary>
    public class NumMergeTileInfo
    {
        // 网格位置（固定）
        private readonly Vector2Int _m_gridPos;
        
        // 格子等级，0 为空
        private int _m_level;
        // 格子等级配置
        private NumMergeBlockRefObj _m_blockRefObj;
        // buff 步数标记
        private int _m_buffStep;


        public NumMergeTileInfo(Vector2Int _gridPos)
        {
            _m_gridPos = _gridPos;
            _m_level = 0;
            _m_blockRefObj = null;
            _m_buffStep = 0;
        }


        public NumMergeBlockRefObj blockRefObj { get { return _m_blockRefObj; } }
        public int level { get { return _m_level; } }
        public Vector2Int gridPos { get { return _m_gridPos; } }
        public int x { get { return _m_gridPos.x; } }
        public int y { get { return _m_gridPos.y; } }
        public int buffStep { get { return _m_buffStep; } }
        public bool isEmpty { get { return _m_level == 0; } }
        public bool isLevelMax { get { return HotfixRefdataCoreMgr.instance.isNumMergeBlockLevelMax(_m_level); } }


        public void clear()
        {
            _m_level = 0;
            _m_buffStep = 0;
        }


        public void updateFromBlockBase(NumMerge_BlockBase _blockBase)
        {
            if (_blockBase == null)
            {
                clear();
                return;
            }

            _m_level = _blockBase.getLevel();
            _m_blockRefObj = HotfixRefdataCoreMgr.instance.numMergeBlockRefCore.getRef(_m_level);
            _m_buffStep = _blockBase.getBuffStep();
        }
        public bool canMergeWith(NumMergeTileInfo _other)
        {
            if (_other == null)
                return false;
            if (isEmpty || _other.isEmpty)
                return false;
            if (isLevelMax || _other.isLevelMax)
                return false;
            if (level != _other.level)
                return false;
            return true;
        }
    }
}