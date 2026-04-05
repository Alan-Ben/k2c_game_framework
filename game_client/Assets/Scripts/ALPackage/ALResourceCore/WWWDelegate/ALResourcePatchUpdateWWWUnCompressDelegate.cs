using System;
using System.Threading;
using System.Collections.Generic;
using System.IO;

using UnityEngine;

/*****************
 * 下载完补丁的资源文件并写入对应位置的处理对象
 **/
namespace ALPackage
{
    public class ALResourcePatchUpdateWWWUnCompressDelegate : _AALResourcePatchUpdateDelegate
    {
        public ALResourcePatchUpdateWWWUnCompressDelegate(_AALResourceCore _resourceCore, ALAssetBundleVersionInfo _versionInfo)
            : base(_resourceCore, _versionInfo)
        {
        }

        /// <summary>
        /// 写入文件的处理函数
        /// </summary>
        /// <param name="_resourceCore"></param>
        /// <param name="_versionInfo"></param>
        protected override void _writeFileFunc(ALAssetBundleVersionInfo _versionInfo, string _localPath)
        {
            //增加补丁对象的信息，带入设置进度的回调对象
            _m_rcResourceCore._addUnCompressPatchFile(_m_viVersionInfo, _localPath);
        }
    }
}
