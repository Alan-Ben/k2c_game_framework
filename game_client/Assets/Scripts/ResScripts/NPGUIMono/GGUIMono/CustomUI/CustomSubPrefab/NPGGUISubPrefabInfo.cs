using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using System;


namespace GOE
{
	/** 加载prefab信息对象 */
	[System.Serializable]
	public class NPGGUISubPrefabInfo
	{
	    //加载对象信息，对象需要继承WCGGGUIMonoSubPrefabMono脚本
	    public string assetPath;
	    public string wndObjName;
	    //加载的父节点对象
	    public Transform parentObj;
	}
}