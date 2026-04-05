using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;


/***************
 * 平台资源对象
 **/
public class AreaResCore : _AALCompleteResourceCore
{
    private static AreaResCore _g_instance = new AreaResCore("area");
    public static AreaResCore instance
    {
        get
        {
            if (null == _g_instance)
                _g_instance = new AreaResCore("area");
            return _g_instance;
        }
    }

    /** 服务端版本号 */
    private static string _m_sServerResVersion = "";

    //创建仅有本地资源处理对象的数据处理对象
    public AreaResCore(string _localFolerPath)
        : base(_localFolerPath)
    {
    }

    /// <summary>
    /// 是否远程资源是压缩资源，是则会调用解压处理，不是则直接拷贝到指定路径
    /// </summary>
    public override bool isRemoteResCompress { get { return false; } }

    public override bool canLoadLocalRes { get { return false; } }
    public string serverResVersion { get { return _m_sServerResVersion; } }

    public void setServerResVersion(string _version)
    {
        _m_sServerResVersion = _version;
    }

    /// <summary>
    /// 在有资源不是最新且需要更新远程资源的时候的事件函数
    /// </summary>
    /// <param name="_versionInfo"></param>
    protected override void _onLoadResWhichIsNotNewest(ALAssetBundleVersionInfo _versionInfo)
    {
#if UNITY_EDITOR
        if (null != _versionInfo)
            UnityEngine.Debug.LogError($"Area Res Core 加载未更新的资源: {_versionInfo.assetPath}");
#endif
    }
}
