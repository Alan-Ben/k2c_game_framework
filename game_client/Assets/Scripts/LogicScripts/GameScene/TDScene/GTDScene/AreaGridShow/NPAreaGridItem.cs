using System;
using System.Collections.Generic;
using UnityEngine;

using ALPackage;

namespace GOE
{
    /***************
     * 建造建筑时地盘区域的单个显示对象
     **/
    public class NPAreaGridItem
    {
        private Vector2Int _m_gridPos;

        // 显示对象
        private NPMonoGridView _m_mono;
        private GameObject _m_monoGO;

        public NPAreaGridItem(NPMonoGridView _mono)
        {
            _m_mono = _mono;
            if(null != _m_mono)
                _m_monoGO = _m_mono.gameObject;
        }
        
        /// <summary>
        /// 这个格子的坐标
        /// </summary>
        public Vector2Int gridPos { get { return _m_gridPos; } }
        /// <summary>
        /// 这个格子的 mono 对象
        /// </summary>
        public NPMonoGridView mono { get { return _m_mono; } }
        
        public void setGridPos(Vector2Int _gridPos)
        {
            _m_gridPos = _gridPos;

            if (_m_monoGO != null)
                _m_monoGO.transform.localPosition = new Vector3(_gridPos.x, 0, _gridPos.y);
        }
        public void refreshGridState(ENPAreaGridState _gridState)
        {
            if (_m_mono != null)
                _m_mono.setGridState(_gridState);
        }
    }
}
