using System;
using System.Collections.Generic;

namespace ALPackage
{
    /***************
     * 资源版本更新函数处理对象
     **/
    public interface _IALAssetBundleVersionInitFunc
    {
        void initVersionSetList(long _versionNum, List<ALAssetBundleVersionInfo> _versionInfoList);

        //初始化失败
        void initVersionFail();
    }
}
