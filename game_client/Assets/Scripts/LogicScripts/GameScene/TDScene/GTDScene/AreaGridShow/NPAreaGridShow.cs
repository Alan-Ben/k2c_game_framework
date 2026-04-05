using UnityEngine;
using System.Collections.Generic;
using System;

using ALPackage;
using JetBrains.Annotations;
using Object = UnityEngine.Object;


namespace GOE
{
    /// <summary>
    /// 区域格子显示对象
    /// </summary>
    public class NPAreaGridShow
    {
        // 区域内的所有标记列表  
        [ItemNotNull][NotNull] private readonly List<NPAreaGridItem> _m_gridItemList;
        
        // 根节点对象
        private GameObject _m_sRootGo;
        // 缓存 RootGo 的 Transform 组件，访问快 10%
        private Transform _m_tTrans;
        // 这个区域的长度，用来计算 list 的下标
        private int _m_length;
        
        public NPAreaGridShow()
        {
            _m_gridItemList = new List<NPAreaGridItem>();
        }

        /// <summary>
        /// 初始化这个格子显示对象
        /// </summary>
        public void init(Vector3 _gridZeroPos, Quaternion _gridRotation, int _width = 0, int _length = 0, Func<Vector2Int, ENPAreaGridState> _judgeData = null)
        {
            if (_m_sRootGo != null)
                return;

            // 创建一个空 GO 来存放对应的格子 item
            _m_sRootGo = new GameObject { name = "area_tip_group_root" };
            _m_tTrans = _m_sRootGo.transform;
            _m_tTrans.position = _gridZeroPos;
            _m_tTrans.rotation = _gridRotation;
            
            // 刷新一遍所有的格子对象
            refreshSize(_width, _length, _judgeData);
        }
        /// <summary>
        /// 销毁这个格子显示对象
        /// </summary>
        public void discard()
        {
            // 清空所有网格
            _clearAllGridItem();
            
            // 删除根节点
            Object.Destroy(_m_sRootGo);
            _m_sRootGo = null;
        }
        /// <summary>
        /// 显示这个对象
        /// </summary>
        public void show()
        {
            ALUGUICommon.setGameObjEnable(_m_sRootGo);
        }
        /// <summary>
        /// 隐藏这个对象
        /// </summary>
        public void hide()
        {
            ALUGUICommon.setGameObjDisable(_m_sRootGo);
        }
        /// <summary>
        /// 刷新当前显示的所有格子
        /// </summary>
        public void refresh(Func<Vector2Int, ENPAreaGridState> _judgeData)
        {
            if (_judgeData == null)
                return;
            
            foreach (NPAreaGridItem gridItem in _m_gridItemList)
            {
                gridItem.refreshGridState(_judgeData.Invoke(gridItem.gridPos));
            }
        }
        /// <summary>
        /// 刷新指定范围的 gridItem
        /// </summary>
        public void refresh(Vector2Int _startPos, int _width, int _length, Func<Vector2Int, ENPAreaGridState> _judgeData)
        {
            if (_judgeData == null)
                return;

            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _length; j++)
                {
                    Vector2Int gridPos = _startPos + new Vector2Int(i, j);
                    NPAreaGridItem gridItem = _getGridItem(gridPos.x, gridPos.y);
                    gridItem?.refreshGridState(_judgeData.Invoke(gridPos));
                }
            }
        }
        /// <summary>
        /// 更新格子尺寸
        /// </summary>
        public void refreshSize(int _width, int _length, Func<Vector2Int, ENPAreaGridState> _judgeData = null)
        {
            _clearAllGridItem();

            _m_length = _length;
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _length; j++)
                {
                    NPAreaGridItem gridItem = NPAreaGridShowGOCache.instance.popItem(_m_tTrans);
                    if (gridItem == null)
                        return;

                    Vector2Int gridPos = new Vector2Int(i, j);
                    gridItem.setGridPos(gridPos);
                    if (_judgeData != null)
                        gridItem.refreshGridState(_judgeData.Invoke(gridPos));
                    _m_gridItemList.Add(gridItem);
                }
            }
        }
        
        // 清空网格
        private void _clearAllGridItem()
        {
            for (int i = 0; i < _m_gridItemList.Count; i++)
            {
                NPAreaGridShowGOCache.instance.pushBackCacheItem(_m_gridItemList[i]);
            }
            _m_gridItemList.Clear();
        }
        // 获取对应格子的 item
        private NPAreaGridItem _getGridItem(int _x, int _z)
        {
            int idx = _x * _m_length + _z;
            if (idx < 0 || idx >= _m_gridItemList.Count)
                return null;

            return _m_gridItemList[idx];
        }
        /// <summary>
        /// 更新根节点坐标
        /// </summary>
        public void refreshRootPos(Vector3 _position, Quaternion _rotation)
        {
            if (_m_tTrans == null)
                return;
            
            _m_tTrans.position = _position;
            _m_tTrans.rotation = _rotation;
        }
    }
}
