using System;
using System.Threading;
using System.Collections.Generic;

using System.IO;

using UnityEngine;

namespace ALPackage
{
    /// <summary>
    /// 内存进行补丁操作的处理对象，一般用于静默处理或者后台更新。
    /// 此更新一般同时只做一个下载，为了避免资源占用过多
    /// 且此补丁只会做文件读写，不会进行AB操作
    /// </summary>
    public abstract class _AALResourceMemPatch
    {
        //远程资源根目录
        private string _m_sRemoteURL;

        /** 缓存存储位置的管理patch信息结构体，与基本补丁管理其实是同一个对象 */
        protected ALLocalPatchInfo _m_piPatchInfo;

        //创建单纯从远程下载资源的处理对象
        public _AALResourceMemPatch(string _remoteURL, string _localPatchFolderPath, _AALResourceCore _basicResourceCore)
        {
        }
    }
}
