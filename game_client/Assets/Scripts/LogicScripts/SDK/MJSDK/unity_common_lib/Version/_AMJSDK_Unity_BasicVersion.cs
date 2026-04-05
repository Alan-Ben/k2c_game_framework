using UnityEngine;
using System.Collections;

namespace MJSDK_Package
{
    /// <summary>
    /// UnitySDK部分的基本版本号存储对象
    /// </summary>
    public abstract class _AMJSDK_Unity_BasicVersion
    {
        private MJSDK_Version_Model _m_component_version;
        //版本号对象
        public MJSDK_Version_Model m_componet_version { get { return _m_component_version; } }
        //构造方法
        public _AMJSDK_Unity_BasicVersion(int _majorVersion
                , int _minorVersion, int _buildVersion, int _fixVersion)
        {
            _m_component_version = new MJSDK_Version_Model();

            _m_component_version.majorV = _majorVersion;
            _m_component_version.minorV = _minorVersion;
            _m_component_version.buildV = _buildVersion;
            _m_component_version.fixV = _fixVersion;
        }

        /**************
         * 返回输出版本号的头部信息
         * @return
         */
        protected abstract string printHeader { get; }

        //输出版本号
        public void printVersion()
        {
            MJSDK_Log.mjsdkLog(printHeader +"V:"+getVersionString());
        }

        /// <summary>
        /// 获取版本号字符串
        /// </summary>
        /// <returns></returns>
        public string getVersionString()
        {
            string version = _m_component_version.majorV + "."+ _m_component_version.minorV + "."+ _m_component_version.buildV + "."+ _m_component_version.fixV;
            return version;
        }
    }
}
