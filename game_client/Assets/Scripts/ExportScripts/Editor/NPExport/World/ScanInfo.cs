
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;


namespace GOE
{
	public partial class NPWorldMapExportWnd
	{
	    /// <summary>
	    /// 扫描数据
	    /// </summary>
	    private class ScanInfo
	    {
	        // 扫描的场景名字
	        private string _m_sSceneName;
	        // 扫描的所有物件
	        [NotNull]
	        private readonly List<UnitData> _m_listUnit;
	        // 错误的列表
	        [NotNull]
	        private readonly List<ErrorObj> _m_listError;
	        // 是否Rect初始化了
	        private bool isRectInit;
	        // 扫描的场景包围盒
	        private Rect _m_rectWorld;
	        // 表示什么时候扫描的场景
	        private DateTime _m_scanTime;

	        public ScanInfo(string _sceneName)
	        {
	            _m_sSceneName = _sceneName;
	            _m_scanTime = DateTime.Now;

	            _m_listUnit = new List<UnitData>();
	            _m_listError = new List<ErrorObj>();

	            isRectInit = false;
	        }
        
	        /// <summary>
	        /// 扫描的场景的名字
	        /// </summary>
	        public string sceneName { get { return _m_sSceneName; } }
	        /// <summary>
	        /// 扫描的所有物件的包围盒
	        /// </summary>
	        public Rect worldRect { get { return _m_rectWorld; } }
	        /// <summary>
	        /// 表示什么时候扫描的场景
	        /// </summary>
	        public DateTime scanTime { get { return _m_scanTime; } }
	        /// <summary>
	        /// 对象列表
	        /// </summary>
	        public List<UnitData> unitList { get { return _m_listUnit; } }
	        /// <summary>
	        /// 错误列表
	        /// </summary>
	        public List<ErrorObj> errorList { get { return _m_listError; } }
        
	        public void addErrorObj(ErrorObj _errorObj)
	        {
	            _m_listError.Add(_errorObj);
	        }

	        public void addUnitData(UnitData _unitData)
	        {
	            _m_listUnit.Add(_unitData);
	        }

	        public void Encapsulate(Rect _aabb2D)
	        {
	            if (!isRectInit)
	            {
	                isRectInit = true;
	                _m_rectWorld = _aabb2D;
	            }
	            else
	            {
	                _m_rectWorld.min = Vector2.Min(_m_rectWorld.min, _aabb2D.min);
	                _m_rectWorld.max = Vector2.Max(_m_rectWorld.max, _aabb2D.max);
	            }
	        }
	    }
	}
}