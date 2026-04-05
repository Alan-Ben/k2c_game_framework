
using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;

namespace ALPackage
{
    public class MD5VersionObj
    {
        // 文件路径，也作为主键
        protected string _m_sFileKey;
        // 文件的md5值
        protected string _m_sFileMD5;
        // 文件的大小
        protected long _m_lFileSize;

        public virtual string fileKey { get { return _m_sFileKey; } set { _m_sFileKey = value; } }
        public virtual string fileMD5 { get { return _m_sFileMD5; } set { _m_sFileMD5 = value; } }
        public virtual long fileSize { get { return _m_lFileSize; } set { _m_lFileSize = value; } }
    }
}