using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

/******************
 * 地图中不同区域的提示信息脚本对象
 **/
public class WCGAreaMeshMono : MonoBehaviour
{
    /** 对应区域的Id */
    public int areaId;
    //对应的mesh对象
    public List<MeshFilter> areaMesh;

    // Update is called once per frame
    void Awake()
    {
        //注册本对象
        WCGAreaMeshMgr.instance.regAreaMesh(this);
    }
    // Update is called once per frame
    void Start()
    {
    }
}
