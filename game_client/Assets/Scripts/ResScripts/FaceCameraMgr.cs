using UnityEngine;
using System.Collections.Generic;
using ALPackage;


namespace GOE
{
	public class FaceCameraMgr  {

	    private static FaceCameraMgr _g_instance = new FaceCameraMgr();
	    public static FaceCameraMgr instance {
	        get {
	            if (null == _g_instance)
	                _g_instance = new FaceCameraMgr();
	            return _g_instance;
	        }
	    }

	    //所有需要朝向摄像机的对象管理队列
	    private HashSet<_IFaceCamera> faceCameraList;
	    //每个对象的旋转角度
	    private Quaternion _m_vRotation;
        private Vector3 _m_vRotateEuler;

	    protected FaceCameraMgr () {
	        faceCameraList = new HashSet<_IFaceCamera>();
	        _m_vRotation = Quaternion.identity;
            _m_vRotateEuler = _m_vRotation.eulerAngles;
        }

	    /*************
	     * 设置所有对象的旋转角度
	     **/
	    public void setRotation (Quaternion _rotation) {
	        _m_vRotation = _rotation;
            _m_vRotateEuler = _m_vRotation.eulerAngles;

            foreach (_IFaceCamera obj in faceCameraList)
	        {
	            if(obj == null)
	                continue;

	            obj.setRotation(_m_vRotateEuler);
	        }
	    }
    
	    /*************
	     * 注册单个需要朝向摄像机的对象
	     **/
	    public void RegFactCameraObj (_IFaceCamera _faceCameraMono) {
	        if(_faceCameraMono == null)
	            return;

	        faceCameraList.Add(_faceCameraMono);
	        _faceCameraMono.setRotation(_m_vRotateEuler);
	    }

	    //刷新角度
	    public void refreshRotate(_IFaceCamera _faceCameraMono)
	    {
	        if (_faceCameraMono == null)
	            return;

	        _faceCameraMono.setRotation(_m_vRotateEuler);
	    }
    
	    /*************
	     * 注销单个需要朝向摄像机的对象
	     **/
	    public void UnRegFactCameraObj (_IFaceCamera _faceCameraMono) {
	        if (_faceCameraMono == null)
	            return;

	        faceCameraList.Remove(_faceCameraMono);
	    }
    
	    /*************
	     * 清空所有注册对象
	     **/
	    public void reset () {
	        if (faceCameraList == null)
	            return;

	        faceCameraList.Clear();
	    }
	}
}