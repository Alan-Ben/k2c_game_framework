using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

using ALPackage;

//玩家视野管理器
public class WCGAreaMeshMgr
{
    private static WCGAreaMeshMgr _g_instance = new WCGAreaMeshMgr();
    public static WCGAreaMeshMgr instance
    {
        get
        {
            if (null == _g_instance)
                _g_instance = new WCGAreaMeshMgr();
            return _g_instance;
        }
    }

    //区域信息
    protected class WCGAreaMeshInfo
    {
        public int areaId;
        public List<MeshFilter> meshList;
    }

    //数据对象列表
    private List<WCGAreaMeshInfo> _m_lAreaInfoList;

    //对应的放置对象
    private GameObject _m_tRootCannotTrans;
    private MeshFilter _m_mfRootCannotMeshFilter;
    private MeshRenderer _m_mrRootCannotMeshRender;
    //不可放置的mesh对象
    private Mesh _m_mCannotMesh;

    //对应的放置对象
    private GameObject _m_tRootCanTrans;
    private MeshFilter _m_mfRootCanMeshFilter;
    private MeshRenderer _m_mrRootCanMeshRender;
    //不可放置的mesh对象
    private Mesh _m_mCanMesh;

    private Material _m_mCanMat;
    private Material _m_mCannotMat;

    //是否初始化
    private bool _m_bIsInit;

    protected WCGAreaMeshMgr()
    {
        _m_lAreaInfoList = new List<WCGAreaMeshInfo>();

        _m_tRootCannotTrans = null;
        _m_mfRootCannotMeshFilter = null;
        _m_mrRootCannotMeshRender = null;
        _m_mCannotMesh = null;

        _m_tRootCanTrans = null;
        _m_mfRootCanMeshFilter = null;
        _m_mrRootCanMeshRender = null;
        _m_mCanMesh = null;

        _m_mCanMat = null;
        _m_mCannotMat = null;

        _m_bIsInit = false;
    }

    /*************
     * 初始化材质
     **/
    public void initMat(Material _canMat, Material _cannotMat)
    {
        _m_mCanMat = _canMat;
        _m_mCannotMat = _cannotMat;
    }

    /******************
     * 初始化相关数据
     **/
    public void init()
    {
        if (_m_bIsInit)
            return;

        _m_bIsInit = true;

        _m_tRootCannotTrans = new GameObject();
        //构建相关脚本
        _m_mfRootCannotMeshFilter = _m_tRootCannotTrans.AddComponent<MeshFilter>();
        _m_mrRootCannotMeshRender = _m_tRootCannotTrans.AddComponent<MeshRenderer>();

        //设置材质
        if (null != _m_mrRootCannotMeshRender)
            _m_mrRootCannotMeshRender.material = _m_mCannotMat;

        //设置对象信息
        _m_tRootCannotTrans.name = "area_mesh";
        GameObject.DontDestroyOnLoad(_m_tRootCannotTrans);

        //默认隐藏
        hideCannotMesh();

        _m_tRootCanTrans = new GameObject();
        //构建相关脚本
        _m_mfRootCanMeshFilter = _m_tRootCanTrans.AddComponent<MeshFilter>();
        _m_mrRootCanMeshRender = _m_tRootCanTrans.AddComponent<MeshRenderer>();

        //设置材质
        if (null != _m_mrRootCanMeshRender)
            _m_mrRootCanMeshRender.material = _m_mCanMat;

        //设置对象信息
        _m_tRootCanTrans.name = "area_mesh";
        GameObject.DontDestroyOnLoad(_m_tRootCanTrans);

        //默认隐藏
        hideCanMesh();
    }

    /***************
     * 注册对应区域的Mesh
     **/
    public void regAreaMesh(WCGAreaMeshMono _areaMesh)
    {
        if (null == _areaMesh)
            return;

        WCGAreaMeshInfo meshInfo = _getMeshInfo(_areaMesh.areaId);
        //注册mesh数据
        if (null == meshInfo)
        {
            meshInfo = new WCGAreaMeshInfo();
            meshInfo.areaId = _areaMesh.areaId;
            meshInfo.meshList = new List<MeshFilter>();
            _m_lAreaInfoList.Add(meshInfo);
        }

        for (int i = 0; i < _areaMesh.areaMesh.Count; i++)
        {
            if (null == _areaMesh.areaMesh[i])
                continue;

            meshInfo.meshList.Add(_areaMesh.areaMesh[i]);
        }
    }

    /************
     * 根据判断区域是否有效，重构Mesh信息
     **/
    public void rebuildMesh(Func<int, bool> _judgeAreaEnable)
    {
        if (null == _judgeAreaEnable)
            return;

        //逐个判断
        List<MeshFilter> allCanMeshList = new List<MeshFilter>();
        List<MeshFilter> allCannotMeshList = new List<MeshFilter>();
        for (int i = 0; i < _m_lAreaInfoList.Count; i++)
        {
            if (_judgeAreaEnable(_m_lAreaInfoList[i].areaId))
                allCanMeshList.AddRange(_m_lAreaInfoList[i].meshList);
            else
                allCannotMeshList.AddRange(_m_lAreaInfoList[i].meshList);
        }

        //设置mesh信息
        if (null != _m_mfRootCanMeshFilter)
            _m_mfRootCanMeshFilter.mesh = null;
        if (null != _m_mfRootCannotMeshFilter)
            _m_mfRootCannotMeshFilter.mesh = null;

        //构造mesh
        _releaseMesh();

        //构造新mesh
        _m_mCanMesh = ALUnityCommon.combineMesh(allCanMeshList);
        //设置mesh信息
        if (null != _m_mfRootCanMeshFilter)
            _m_mfRootCanMeshFilter.mesh = _m_mCanMesh;

        //构造新mesh
        _m_mCannotMesh = ALUnityCommon.combineMesh(allCannotMeshList);
        //设置mesh信息
        if (null != _m_mfRootCannotMeshFilter)
            _m_mfRootCannotMeshFilter.mesh = _m_mCannotMesh;
    }

    /************
     * 显示不可放置的mesh
     **/
    public void showCannotMesh()
    {
        if (null != _m_tRootCannotTrans)
            ALUGUICommon.setGameObjEnable(_m_tRootCannotTrans, true);
    }
    public void hideCannotMesh()
    {
        if (null != _m_tRootCannotTrans)
            ALUGUICommon.setGameObjEnable(_m_tRootCannotTrans, false);
    }
    public void showCanMesh()
    {
        if (null != _m_tRootCanTrans)
            ALUGUICommon.setGameObjEnable(_m_tRootCanTrans, true);
    }
    public void hideCanMesh()
    {
        if (null != _m_tRootCanTrans)
            ALUGUICommon.setGameObjEnable(_m_tRootCanTrans, false);
    }

    /*******
     * 析构函数
     **/
    public void discard()
    {
        //释放对象
        if(null != _m_tRootCannotTrans)
        {
            if (null != _m_mfRootCannotMeshFilter)
                _m_mfRootCannotMeshFilter.mesh = null;
            if (null != _m_mrRootCannotMeshRender)
                _m_mrRootCannotMeshRender.material = null;

            ALUnityCommon.releaseGameObj(_m_tRootCannotTrans);
        }
        _m_tRootCannotTrans = null;
        _m_mfRootCannotMeshFilter = null;
        _m_mrRootCannotMeshRender = null;

        if (null != _m_tRootCanTrans)
        {
            if (null != _m_mfRootCanMeshFilter)
                _m_mfRootCanMeshFilter.mesh = null;
            if (null != _m_mrRootCanMeshRender)
                _m_mrRootCanMeshRender.material = null;

            ALUnityCommon.releaseGameObj(_m_tRootCanTrans);
        }
        _m_tRootCanTrans = null;
        _m_mfRootCanMeshFilter = null;
        _m_mrRootCanMeshRender = null;

        //释放mesh信息
        _releaseMesh();

        _m_bIsInit = false;
    }

    /***********
     * 获取对应区域的mesh信息
     **/
    protected WCGAreaMeshInfo _getMeshInfo(int _areaId)
    {
        if (null == _m_lAreaInfoList)
            return null;

        for(int i = 0; i < _m_lAreaInfoList.Count; i++)
        {
            if (_m_lAreaInfoList[i].areaId == _areaId)
                return _m_lAreaInfoList[i];
        }

        return null;
    }

    /************
     * 释放mesh
     **/
    protected void _releaseMesh()
    {
        if (null != _m_mCanMesh)
            ALUnityCommon.releaseGameObj(_m_mCanMesh);
        _m_mCanMesh = null;

        if (null != _m_mCannotMesh)
            ALUnityCommon.releaseGameObj(_m_mCannotMesh);
        _m_mCannotMesh = null;
    }
}
