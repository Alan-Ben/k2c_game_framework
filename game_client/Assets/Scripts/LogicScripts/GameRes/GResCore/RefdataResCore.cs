using System;
using System.Collections.Generic;
using System.IO;
using ALPackage;
using GOE;


/***************
 * 平台资源对象
 **/
public class RefdataResCore : _AALCompleteResourceCore
{
    private static RefdataResCore _g_instance = new RefdataResCore("refdata");
    public static RefdataResCore instance
    {
        get
        {
            if (null == _g_instance)
                _g_instance = new RefdataResCore("refdata");
            return _g_instance;
        }
    }

    //创建仅有本地资源处理对象的数据处理对象
    public RefdataResCore(string _localFolerPath)
        : base(_localFolerPath)
    {
    }

    /// <summary>
    /// 是否远程资源是压缩资源，是则会调用解压处理，不是则直接拷贝到指定路径
    /// </summary>
    public override bool isRemoteResCompress { get { return false; } }

    /// <summary>
    /// 在有资源不是最新且需要更新远程资源的时候的事件函数
    /// </summary>
    /// <param name="_versionInfo"></param>
    protected override void _onLoadResWhichIsNotNewest(ALAssetBundleVersionInfo _versionInfo)
    {
#if UNITY_EDITOR
        if (null != _versionInfo && !_versionInfo.assetPath.StartsWith("language/"))
            UnityEngine.Debug.LogError($"Refdata Res Core 加载未更新的资源: {_versionInfo.assetPath}");
#endif
    }
}
