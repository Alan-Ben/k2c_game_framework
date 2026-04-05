using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using ALPackage;

namespace GOE
{
    /********************
     * 登录部分的通用结构体对象
     **/
    public class GGameCustomInfo
    {
        private static GGameCustomInfo _g_instance = new GGameCustomInfo();
        public static GGameCustomInfo instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new GGameCustomInfo();
                return _g_instance;
            }
        }

        private NPGSOGameCustomInfo _m_obj;

        public GGameCustomInfo()
        {
            _m_obj = Resources.Load(NPGSOGameCustomInfo.objName) as NPGSOGameCustomInfo;
        }
        
        /// <summary>
        /// 加载出来的数据集
        /// </summary>
        public NPGSOGameCustomInfo obj { get { return _m_obj; }}

        /// <summary>
        /// 自定义配置是否生效
        /// </summary>
        public bool isEnable
        {
            get { return (null != obj && obj.isEnable); }
        }
    }
}
