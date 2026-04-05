using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;


/********************
 * 登录部分的通用结构体对象
 **/
public class PLoginCommonInfo : _ATALBasicSingleAssetObj<NPPSOLoginCommonInfo>
{
    private static PLoginCommonInfo _g_instance = new PLoginCommonInfo();
    public static PLoginCommonInfo instance
    {
        get
        {
            if (null == _g_instance)
                _g_instance = new PLoginCommonInfo();
            return _g_instance;
        }
    }

    /** 获取资源管理对象 */
    protected override _AALResourceCore _resCore { get { return PlatResCore.instance; } }
    /** 获取加载路径 */
    protected override string _resPath { get { return NPPSOLoginCommonInfo.assetPath; } }
    /** 获取加载对象名 */
    protected override string _objName { get { return NPPSOLoginCommonInfo.objName; } }
}
