using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;


/***************
 * 平台资源对象
 **/
public class GameResCore : _AALCompleteResourceCore
{
    private static GameResCore _g_instance = new GameResCore("game");
    public static GameResCore instance
    {
        get
        {
            if (null == _g_instance)
                _g_instance = new GameResCore("game");
            return _g_instance;
        }
    }

    /** 服务端版本号 */
    private static string _m_sServerVersion = "";
    //获取的服务器版本号信息
    private int _m_iServerMain;
    private int _m_iServerSub;
    private int _m_iServerPatch;
    private int _m_iServerBuild;

    //服务器同步的最新客户端版本号
    private static long _m_lNewClientVersion = 0;

    //创建仅有本地资源处理对象的数据处理对象
    public GameResCore(string _localFolerPath)
        : base(_localFolerPath)
    {
    }

    /// <summary>
    /// 是否远程资源是压缩资源，是则会调用解压处理，不是则直接拷贝到指定路径
    /// </summary>
    public override bool isRemoteResCompress { get { return false; } }

    public string serverVersion { get { return _m_sServerVersion; } }
    public int serverVersionNum
    {
        get
        {
            return (_m_iServerMain * 1000000) + (_m_iServerSub * 10000) + (_m_iServerPatch * 100) + _m_iServerBuild;
        }
    }

    public long NewClientVersion { get { return _m_lNewClientVersion; } }

    public void setServerVersion(string _version)
    {
        _m_sServerVersion = _version;

        //数据有效才处理
        if (!string.IsNullOrEmpty(_m_sServerVersion))
        {
            //拆分字符串，获取3个关键版本号
            try
            {
                string[] strs = _m_sServerVersion.Split('.');
                if (strs.Length > 0)
                    int.TryParse(strs[0], out _m_iServerMain);
                if (strs.Length > 1)
                    int.TryParse(strs[1], out _m_iServerSub);
                if (strs.Length > 2)
                    int.TryParse(strs[2], out _m_iServerPatch);
                if (strs.Length > 3)
                    int.TryParse(strs[3], out _m_iServerBuild);
            }
            catch (Exception)
            {

            }
        }
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

    }
}
