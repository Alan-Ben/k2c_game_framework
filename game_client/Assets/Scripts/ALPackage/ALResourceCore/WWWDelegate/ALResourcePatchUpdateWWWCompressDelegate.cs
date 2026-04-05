using System;
using System.Threading;
using System.Collections.Generic;
using System.IO;

using UnityEngine;

#if AL_SEVEN_ZIP
/*****************
 * 下载完补丁的资源文件并写入对应位置的处理对象
 **/
namespace ALPackage
{
    public class ALResourcePatchUpdateWWWCompressDelegate : _AALResourcePatchUpdateDelegate, SevenZip.ICodeProgress
    {
        /** 已经处理完成的解压字节数 */
        private long _m_lUnzipSize;

        public ALResourcePatchUpdateWWWCompressDelegate(_AALResourceCore _resourceCore, ALAssetBundleVersionInfo _versionInfo)
            : base(_resourceCore, _versionInfo)
        {
            _m_lUnzipSize = 0;
        }

        public long unzipSize { get { return _m_lUnzipSize; } }

        /*****************
         * 设置已解压的进度
         **/
        public void SetProgress(Int64 _inSize, Int64 _outSize)
        {
            //直接设置处理字节数，不进行加锁，因为处理的频率太高
            _m_lUnzipSize = _outSize;
        }

        /// <summary>
        /// 写入文件的处理函数
        /// </summary>
        /// <param name="_resourceCore"></param>
        /// <param name="_versionInfo"></param>
        protected override void _writeFileFunc(ALAssetBundleVersionInfo _versionInfo, string _localPath)
        {
            //增加补丁对象的信息，带入设置进度的回调对象
            _m_rcResourceCore._addLZMAPatchFile(_versionInfo, _localPath, this);

            //设置进度为完成
            _m_lUnzipSize = _m_viVersionInfo.fileSize;
        }
    }
}
#endif

