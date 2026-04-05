using System;
using System.Collections.Generic;

using UnityEngine;

namespace ALPackage
{
    /****************
     * 正在下载的资源信息对象
     **/
    public interface _IALLoadingAssetInterface
    {
        string assetPath
        {
            get;
        }

        float progress
        {
            get;
        }

        bool isSucDone
        {
            get;
        }

        bool isSuc
        {
            get;
        }
    }
}
