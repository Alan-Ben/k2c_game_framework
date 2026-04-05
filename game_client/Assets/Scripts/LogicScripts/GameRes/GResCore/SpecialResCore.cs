using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;


/***************
 * 特殊资源对象，一般用于公告，广告等特殊信息加载处理
 **/
public class SpecialResCore : _AALCompleteResourceCore
{
    private static SpecialResCore _g_instance = new SpecialResCore("special");
    public static SpecialResCore instance
    {
        get
        {
            if (null == _g_instance)
                _g_instance = new SpecialResCore("special");
            return _g_instance;
        }
    }

    /** 服务端版本号 */
    private static string _m_sServerResVersion = "";

    //服务器同步的最新客户端版本号
    private static long _m_lNewClientVersion = 0;

    //创建仅有本地资源处理对象的数据处理对象
    public SpecialResCore(string _localFolerPath)
        : base(_localFolerPath)
    {
    }

    /// <summary>
    /// 是否远程资源是压缩资源，是则会调用解压处理，不是则直接拷贝到指定路径
    /// </summary>
    public override bool isRemoteResCompress { get { return false; } }

    public string serverResVersion { get { return _m_sServerResVersion; } }
    public long NewClientVersion { get { return _m_lNewClientVersion; } }

    public void setServerResVersion(string _version)
    {
        _m_sServerResVersion = _version;
    }

    public void setNewClientVersion(long _version)
    {
        _m_lNewClientVersion = _version;
    }

    /// <summary>
    /// 在有资源不是最新且需要更新远程资源的时候的事件函数
    /// </summary>
    /// <param name="_versionInfo"></param>
    protected override void _onLoadResWhichIsNotNewest(ALAssetBundleVersionInfo _versionInfo)
    {
#if UNITY_EDITOR
        if (null != _versionInfo)
            UnityEngine.Debug.LogError($"Special Res Core 加载未更新的资源: {_versionInfo.assetPath}");
#endif
    }
}
