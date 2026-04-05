
using UnityEngine;


namespace GOE
{
	public partial class NPWorldMapExportWnd
	{
	    private class UnitData
	    {
	        private long _m_lRefId;
	        private Transform _m_transUnit;
	        private Rect _m_rectAABB;
        
	        public UnitData(long _refId, Transform _unitTransform, Rect _aabb2D)
	        {
	            _m_lRefId = _refId;
	            _m_transUnit = _unitTransform;
	            _m_rectAABB = _aabb2D;
	        }
        
	        public long refId { get { return _m_lRefId; } }
	        public Transform unitTransform { get { return _m_transUnit; } }
	        public Rect rectAABB { get { return _m_rectAABB; } }
	    }
	}
}