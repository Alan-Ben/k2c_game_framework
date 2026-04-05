using System;
using UnityEngine;
using System.Collections.Generic;


namespace GOE
{
	public interface _IFaceCamera {
	    void setRotation (Vector3 _euler);
	}

	public class FaceCamera : MonoBehaviour, _IFaceCamera
	{
		[ALHeader("是否需要每帧刷新")]
		public bool needUpdateRefresh = false;
		
	    void Awake () {
	        //注册对象
	        FaceCameraMgr.instance.RegFactCameraObj(this);
	    }

	    void OnEnable()
	    {
	        FaceCameraMgr.instance.refreshRotate(this);
	    }

	    void OnDestroy()
	    {
	        FaceCameraMgr.instance.UnRegFactCameraObj(this);
	    }
	    
	    public void Update()
	    {
		    if(!needUpdateRefresh)
			    return;
		    
		    FaceCameraMgr.instance.refreshRotate(this);
	    }

	    /*************
	    * 设置对象的朝向，面对摄像头
	    **/
	    public void setRotation (Vector3 _euler)
	    {
	        Transform trans = transform;
	        if (null == trans)
	            return;

	        Vector3 preEluler = trans.eulerAngles;
	        preEluler.y = _euler.y - 180;
	        trans.eulerAngles = preEluler;
	    }
	}

}