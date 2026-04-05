using UnityEngine;
using System.Collections.Generic;
using System;
using ALPackage;
using ALBasicProtocolPack;

namespace GOE
{
    /// <summary>
    /// 登录服务器管理对象，管理所有用于登录的服务器信息
    /// </summary>
    public class LSMgr : _LSBasicMgr
    {
        private static LSMgr _g_instance = new LSMgr();
        public static LSMgr instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new LSMgr();
                return _g_instance;
            }
        }

        protected LSMgr()
            : base()
        {
        }
    }
}