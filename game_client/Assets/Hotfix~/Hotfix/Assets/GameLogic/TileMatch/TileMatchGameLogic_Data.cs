using System;
using System.Collections.Generic;
using ALPackage;
using Hotfix.Common.TileMatchObj;
using UnityEngine;

namespace Hotfix
{
    /// <summary>
    /// 游戏逻辑中的数据部分, 只有对数据的操作, 不涉及表现
    /// </summary>
    public partial class TileMatchGameLogic
    {
        private TileMatchBlockInfo[][] _m_blockInfoArray;

        // 三消任务信息
        private TileMatchTaskInfo _m_iTaskInfo;

        #region 任务pool

        // // 三消任务信息池
        // private ObjectPool<TileMatchTaskInfo> _m_taskInfoPool;
        // // 任务方块信息池
        // private ObjectPool<TileMatchTaskBlockInfo> _m_taskBlockInfoPool;
        // // 任务方块信息列表池
        // private ObjectPool<List<TileMatchTaskBlockInfo>> _m_taskBlockInfoListPool;

        #endregion

        public TileMatchTaskInfo taskInfo => _m_iTaskInfo;

        public event Action onNewTaskInfo;//当三消客户端任务变为新任务时
        public event Action onTaskInfoChg;//当三消客户端任务数据发生变化时
        
        private void _initData()
        {
            _initBlockInfoArray();

            _initTaskInfo();
        }
        
        private void _discardData()
        {
            onNewTaskInfo = null;
            onTaskInfoChg = null;
            
            _discardBlockInfoArray();

            _discardTaskInfo();
        }

        #region 三消格子数据操作

        /// <summary>
        /// 初始化方块信息数组
        /// </summary>
        private void _initBlockInfoArray()
        {
            // 初始化之前先回收一遍
            _discardBlockInfoArray();
            
            _m_blockInfoArray = new TileMatchBlockInfo[TileMatchUtil.column][];
            for (int x = 0; x < TileMatchUtil.column; x++)
            {
                _m_blockInfoArray[x] = new TileMatchBlockInfo[TileMatchUtil.row];

                for (int y = 0; y < TileMatchUtil.row; y++)
                {
                    _m_blockInfoArray[x][y] = new TileMatchBlockInfo(new Vector2Int(x, y));
                }
            }
        }

        private void _discardBlockInfoArray()
        {
            _dealAllBlockInfo_leftDown2RightUp((_blockInfo) =>
            {
                if (_blockInfo == null)
                    return;

                _clearBlock(_blockInfo, false, 0, null);
            });
            
            _m_blockInfoArray = null;
        }

        /// <summary>
        /// 从左下到右上遍历
        /// </summary>
        /// <param name="_action"></param>
        private void _dealAllBlockInfo_leftDown2RightUp(Action<TileMatchBlockInfo> _action)
        {
            if(_action == null || _m_blockInfoArray == null)
                return;

            for (int y = 0; y < TileMatchUtil.row; y++)
            {
                for (int x = 0; x < TileMatchUtil.column; x++)
                {
                    TileMatchBlockInfo blockInfo = _m_blockInfoArray[x][y];
                    _action.Invoke(blockInfo);
                }
            }
        }
        
        /// <summary>
        /// 从右上到左下遍历
        /// </summary>
        /// <param name="_action"></param>
        private void _dealAllBlockInfo_rightUp2LeftDown(Action<TileMatchBlockInfo> _action)
        {
            if(_action == null || _m_blockInfoArray == null)
                return;

            for (int y = TileMatchUtil.row - 1; y >= 0; y--)
            {
                for (int x = TileMatchUtil.column - 1; x >= 0; x--)
                {
                    TileMatchBlockInfo blockInfo = _m_blockInfoArray[x][y];
                    _action.Invoke(blockInfo);
                }
            }
        }
        
        /// <summary>
        /// 判断坐标是否不在棋盘内
        /// </summary>
        /// <param name="_x"></param>
        /// <param name="_y"></param>
        /// <returns></returns>
        private bool _checkPosOutSide(int _x, int _y)
        {
            return (_x < 0 || _x >= TileMatchUtil.column || _y < 0 || _y >= TileMatchUtil.row);
        }

        /// <summary>
        /// 获取格子数据
        /// </summary>
        /// <param name="_x"></param>
        /// <param name="_y"></param>
        /// <returns></returns>
        private TileMatchBlockInfo _getBlockInfoFormArray(int _x, int _y)
        {
            if (_m_blockInfoArray == null || _checkPosOutSide(_x, _y))
                return null;

            return _m_blockInfoArray[_x][_y];
        }
        
        /// <summary>
        /// 获取格子数据
        /// </summary>
        /// <returns></returns>
        private TileMatchBlockInfo _getBlockInfoFormArray(Vector2Int _logicPos)
        {
            return _getBlockInfoFormArray(_logicPos.x, _logicPos.y);
        }

        /// <summary>
        /// 获取格子数据
        /// </summary>
        /// <param name="_posIndex"></param>
        /// <returns></returns>
        private TileMatchBlockInfo _getBlockInfoFormArray(int _posIndex)
        {
            return _getBlockInfoFormArray(TileMatchUtil.indexToVector2(_posIndex));
        }

        /// <summary>
        /// 获取指定方向上的相邻方块信息
        /// </summary>
        /// <returns></returns>
        private TileMatchBlockInfo _getSpecifiedAdjoinedBlockInfo(Vector2Int _specifiedPos, ETileMatchDirection _direction)
        {
            if (_m_blockInfoArray == null)
                return null;

            int x, y;
            switch (_direction)
            {
                case ETileMatchDirection.Left:
                    x = _specifiedPos.x - 1;
                    y = _specifiedPos.y;
                    break;
                
                case ETileMatchDirection.Right:
                    x = _specifiedPos.x + 1;
                    y = _specifiedPos.y;
                    break;
                
                case ETileMatchDirection.Up:
                    x = _specifiedPos.x;
                    y = _specifiedPos.y + 1;
                    break;
                
                case ETileMatchDirection.Down:
                    x = _specifiedPos.x;
                    y = _specifiedPos.y - 1;
                    break;
                default:
                    return null;
            }

            return _getBlockInfoFormArray(x, y);
        }

        
        /// <summary>
        /// 获取某列所有格子数据
        /// </summary>
        /// <param name="_column"></param>
        /// <returns></returns>
        private TileMatchBlockInfo[] _getColumnTileCubeInfos(int _column)
        {
            if (_m_blockInfoArray == null || _column < 0 || _column >= _m_blockInfoArray.Length)
                return null;
            return _m_blockInfoArray[_column];
        }

        private bool _checkIsSpecialBlock(TileMatchBlockInfo _blockInfo)
        {
            if (_blockInfo == null || _blockInfo.tileMatchBlockRefObj == null)
                return false;

            return _blockInfo.tileMatchBlockRefObj.type != TileMatchEnum.ETileMatch_BlockType.NONE;
        }

        /// <summary>
        /// 获取可交换的两个格子, 从右上到左下查找, 优先特殊格消除
        /// </summary>
        private bool _getCanSwitchBlock(out TileMatchBlockInfo _blockInfo1, out TileMatchBlockInfo _blockInfo2)
        {
            // 若没有特殊格子可消除
            if (!_getCanSwitchSpecialBlock(out _blockInfo1, out _blockInfo2) || _blockInfo1 == null || _blockInfo2 == null)
            {
                // 再查找可交换消除的普通格子
                return _getCanSwitchNormalBlock(out _blockInfo1, out _blockInfo2);
            }
            else
            {
                return true;
            }
        }
        
        /// <summary>
        /// 获取可交换的两个格子(其中有一个必须是特殊格)
        /// </summary>
        /// <param name="_"></param>
        private bool _getCanSwitchSpecialBlock(out TileMatchBlockInfo _blockInfo1, out TileMatchBlockInfo _blockInfo2)
        {
            _blockInfo1 = null;
            _blockInfo2 = null;
            
            if(_m_blockInfoArray == null)
                return false;
            
            // 从右上到左下遍历
            for (int y = TileMatchUtil.row - 1; y >= 0; y--)
            {
                for (int x = TileMatchUtil.column - 1; x >= 0; x--)
                {
                    TileMatchBlockInfo blockInfo = _m_blockInfoArray[x][y];
                    // 若格子数据为空 或 不是特殊格子, 跳过
                    if(blockInfo == null || !_checkIsSpecialBlock(blockInfo))
                        continue;

                    // 左边格子
                    TileMatchBlockInfo tmoBlockInfo = _getSpecifiedAdjoinedBlockInfo(blockInfo.logicPos, ETileMatchDirection.Left);
                    if (tmoBlockInfo != null)
                    {
                        // 若左边格子也是特殊格子, 则直接返回
                        if (_checkIsSpecialBlock(tmoBlockInfo))
                        {
                            _blockInfo1 = blockInfo;
                            _blockInfo2 = tmoBlockInfo;
                            return true;
                        }
                        else if(_blockInfo1 == null || _blockInfo2 == null)
                        {
                            _blockInfo1 = blockInfo;
                            _blockInfo2 = tmoBlockInfo;
                        }
                    }

                    // 右边格子
                    tmoBlockInfo = _getSpecifiedAdjoinedBlockInfo(blockInfo.logicPos, ETileMatchDirection.Right);
                    if (tmoBlockInfo != null)
                    {
                        // 若右边格子也是特殊格子, 则直接返回
                        if (_checkIsSpecialBlock(tmoBlockInfo))
                        {
                            _blockInfo1 = blockInfo;
                            _blockInfo2 = tmoBlockInfo;
                            return true;
                        }
                        else if(_blockInfo1 == null || _blockInfo2 == null)
                        {
                            _blockInfo1 = blockInfo;
                            _blockInfo2 = tmoBlockInfo;
                        }
                    }
                    
                    // 上边格子
                    tmoBlockInfo = _getSpecifiedAdjoinedBlockInfo(blockInfo.logicPos, ETileMatchDirection.Up);
                    if (tmoBlockInfo != null)
                    {
                        // 若上边格子也是特殊格子, 则直接返回
                        if (_checkIsSpecialBlock(tmoBlockInfo))
                        {
                            _blockInfo1 = blockInfo;
                            _blockInfo2 = tmoBlockInfo;
                            return true;
                        }
                        else if(_blockInfo1 == null || _blockInfo2 == null)
                        {
                            _blockInfo1 = blockInfo;
                            _blockInfo2 = tmoBlockInfo;
                        }
                    }
                    
                    // 下边格子
                    tmoBlockInfo = _getSpecifiedAdjoinedBlockInfo(blockInfo.logicPos, ETileMatchDirection.Down);
                    if (tmoBlockInfo != null)
                    {
                        // 若下边格子也是特殊格子, 则直接返回
                        if (_checkIsSpecialBlock(tmoBlockInfo))
                        {
                            _blockInfo1 = blockInfo;
                            _blockInfo2 = tmoBlockInfo;
                            return true;
                        }
                        else if(_blockInfo1 == null || _blockInfo2 == null)
                        {
                            _blockInfo1 = blockInfo;
                            _blockInfo2 = tmoBlockInfo;
                        }
                    }
                }
            }

            return _blockInfo1 != null && _blockInfo2 != null;
        }

        /// <summary>
        /// 获取可交换的普通格子, 两个格子都是普通格
        /// </summary>
        /// <param name="_blockInfo1"></param>
        /// <param name="_blockInfo2"></param>
        private bool _getCanSwitchNormalBlock(out TileMatchBlockInfo _blockInfo1, out TileMatchBlockInfo _blockInfo2)
        {
            _blockInfo1 = null;
            _blockInfo2 = null;
            
            if(_m_blockInfoArray == null)
                return false;

            TileMatchBlockInfo nowBlockInfo = null;
            TileMatchBlockInfo left1 = null;// 左边相邻的第1个格子
            TileMatchBlockInfo left2 = null;// 左边相邻的第2个格子
            TileMatchBlockInfo right1 = null;// 右边相邻的第1个格子
            TileMatchBlockInfo right2 = null;// 右边相邻的第2个格子
            TileMatchBlockInfo up1 = null;// 上边相邻的第1个格子
            TileMatchBlockInfo up2 = null;// 上边相邻的第2个格子
            TileMatchBlockInfo down1 = null;// 下边相邻的第1个格子
            TileMatchBlockInfo down2 = null;// 下边相邻的第2个格子
            
            for (int y = TileMatchUtil.row - 1; y >= 0; y--)
            {
                for (int x = TileMatchUtil.column - 1; x >= 0; x--)
                {
                    nowBlockInfo = _getBlockInfoFormArray(x, y);
                    left1 = _getBlockInfoFormArray(x - 1, y);
                    left2 = _getBlockInfoFormArray(x - 2, y);
                    right1 = _getBlockInfoFormArray(x + 1, y);
                    right2 = _getBlockInfoFormArray(x + 2, y);
                    up1 = _getBlockInfoFormArray(x, y + 1);
                    up2 = _getBlockInfoFormArray(x, y + 2);
                    down1 = _getBlockInfoFormArray(x, y - 1);
                    down2 = _getBlockInfoFormArray(x, y - 2);
                    
                    // 尝试向左交换 并判断是否可以消除
                    if (_checkCanEliminate(left1, nowBlockInfo, left2, right1, right2, up1, up2, down1, down2))
                    {
                        _blockInfo1 = nowBlockInfo;
                        _blockInfo2 = left1;
                        return true;
                    }
                    
                    // 尝试向右交换 并判断是否可以消除
                    if (_checkCanEliminate(right1, left1, left2, nowBlockInfo, right2, up1, up2, down1, down2))
                    {
                        _blockInfo1 = nowBlockInfo;
                        _blockInfo2 = right1;
                        return true;
                    }
                    
                    // 尝试向上交换 并判断是否可以消除
                    if (_checkCanEliminate(up1, left1, left2, right1, right2, nowBlockInfo, up2, down1, down2))
                    {
                        _blockInfo1 = nowBlockInfo;
                        _blockInfo2 = up1;
                        return true;
                    }
                    
                    // 尝试向下交换 并判断是否可以消除
                    if (_checkCanEliminate(down1, left1, left2, right1, right2, up1, up2, nowBlockInfo, down2))
                    {
                        _blockInfo1 = nowBlockInfo;
                        _blockInfo2 = down1;
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 检测是否可以消除
        /// </summary>
        /// <returns></returns>
        private bool _checkCanEliminate(TileMatchBlockInfo _nowBlockInfo, TileMatchBlockInfo _left1, TileMatchBlockInfo _left2, TileMatchBlockInfo _right1
            , TileMatchBlockInfo _right2, TileMatchBlockInfo _up1, TileMatchBlockInfo _up2, TileMatchBlockInfo _down1, TileMatchBlockInfo _down2)
        {
            return (_left2 != null && _left1 != null && _nowBlockInfo != null && _left2.blockRefId == _left1.blockRefId && _left1.blockRefId == _nowBlockInfo.blockRefId) ||
                   (_left1 != null && _nowBlockInfo != null && _right1 != null && _left1.blockRefId == _nowBlockInfo.blockRefId && _nowBlockInfo.blockRefId == _right1.blockRefId) ||
                   (_nowBlockInfo != null && _right1 != null && _right2 != null && _nowBlockInfo.blockRefId == _right1.blockRefId && _right1.blockRefId == _right2.blockRefId) ||
                   (_up2 != null && _up1 != null && _nowBlockInfo != null && _up2.blockRefId == _up1.blockRefId && _up1.blockRefId == _nowBlockInfo.blockRefId) ||
                   (_up1 != null && _nowBlockInfo != null && _down1 != null && _up1.blockRefId == _nowBlockInfo.blockRefId && _nowBlockInfo.blockRefId == _down1.blockRefId) ||
                   (_nowBlockInfo != null && _down1 != null && _down2 != null && _nowBlockInfo.blockRefId == _down1.blockRefId && _down1.blockRefId == _down2.blockRefId);
        }
        
        /// <summary>
        /// 检查全方向是否可以消除
        /// </summary>
        private bool _checkEliminate(int _x, int _y)
        {
            return _checkEliminateHorizontal(_x, _y) || _checkEliminateVertical(_x, _y);
        }
        
        /// <summary>
        /// 检查水平方向是否可以消除
        /// </summary>
        /// <returns></returns>
        private bool _checkEliminateHorizontal(int _x, int _y)
        {
            TileMatchBlockInfo t1, t2, t3;
            for (int i = 0; i < 3; i++)
            {
                t1 = _getBlockInfoFormArray(_x + i - 2, _y);
                t2 = _getBlockInfoFormArray(_x + i - 1, _y);
                t3 = _getBlockInfoFormArray(_x + i, _y);
                
                if (t1 != null && t2 != null && t3 != null && t1.blockRefId == t2.blockRefId && t2.blockRefId == t3.blockRefId)
                    return true;
            }

            return false;
        }
        
        /// <summary>
        /// 检查垂直方向是否可以消除
        /// </summary>
        /// <returns></returns>
        private bool _checkEliminateVertical(int _x, int _y)
        {
            TileMatchBlockInfo t1, t2, t3;
            for (int i = 0; i < 3; i++)
            {
                t1 = _getBlockInfoFormArray(_x, _y + i - 2);
                t2 = _getBlockInfoFormArray(_x, _y + i - 1);
                t3 = _getBlockInfoFormArray(_x, _y + i);
                
                if (t1 != null && t2 != null && t3 != null && t1.blockRefId == t2.blockRefId && t2.blockRefId == t3.blockRefId)
                    return true;
            }

            return false;
        }
        
        #endregion


        #region 三消任务数据操作

        private void _initTaskInfo()
        {
            #region 任务使用pool

            // // 初始化之前先回收一遍
            // _discardTaskInfo();
            //
            // _m_taskBlockInfoListPool = new ObjectPool<List<TileMatchTaskBlockInfo>>(null, _list => _list?.Clear());
            // _m_taskBlockInfoPool = new ObjectPool<TileMatchTaskBlockInfo>(null, null);
            // _m_taskInfoPool = new ObjectPool<TileMatchTaskInfo>((_taskInfo) =>
            // {
            //     _taskInfo?.onGetFromPool(_m_taskInfoPool, _m_taskBlockInfoPool, _m_taskBlockInfoListPool);
            // }, null);

            #endregion
        }
        
        private void _discardTaskInfo()
        {
            #region 任务使用pool

            // _m_iTaskInfo?.release();
            //
            // _m_taskInfoPool = null;
            // _m_taskBlockInfoPool = null;
            // _m_taskBlockInfoListPool = null;

            #endregion

            _m_iTaskInfo = null;
        }

        /// <summary>
        /// 更新三消任务数据
        /// </summary>
        private void _updateTaskInfo(TileMatch_TaskInfo _serverTaskInfo)
        {
            if(_serverTaskInfo == null)
                return;

            // 若当前任务信息不存在 或者 当前任务信息的序列号和服务器的不同, 则创建新的任务信息
            if (_m_iTaskInfo == null || _m_iTaskInfo.serialId != _serverTaskInfo.getSerialId())
            {
                _m_iTaskInfo = new TileMatchTaskInfo(_serverTaskInfo);
                ALMsgSys.SendMsg(HotfixMsgType.ON_NEW_TILEMATCH_TASK_INFO);
                onNewTaskInfo?.Invoke();
            }
            else
            {
                _m_iTaskInfo.updateInfo(_serverTaskInfo);
                ALMsgSys.SendMsg(HotfixMsgType.ON_TILEMATCH_TASK_INFO_CHG);
                onTaskInfoChg?.Invoke();
            }
        }
        
        #endregion
        
    }
}