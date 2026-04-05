using UnityEngine;


namespace GOE
{
	public partial class NPWorldMapExportWnd
	{
	    private class ErrorObj
	    {
	        private GameObject _m_GOError;
	        private ErrorType _m_eErrorType;
        
	        public ErrorObj(GameObject _errorGo, ErrorType _errorType)
	        {
	            _m_GOError = _errorGo;
	            _m_eErrorType = _errorType;
	        }
        
	        public GameObject errorGO { get { return _m_GOError; } }
	        public ErrorType errorType { get { return _m_eErrorType; } set { _m_eErrorType = value; }
	        }
	    }
	}
}