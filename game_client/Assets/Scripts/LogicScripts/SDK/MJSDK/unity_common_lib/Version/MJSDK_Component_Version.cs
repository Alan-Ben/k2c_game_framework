using UnityEngine;
using System.Collections;
using System;

namespace MJSDK_Package
{
    /// <summary>
    /// UnitySDK部分的版本号存储对象
    /// </summary>
    public class MJSDK_Component_Version : _AMJSDK_Unity_BasicVersion
    {
        //组件的标记
        private string _m_sComponentTag;

        public MJSDK_Component_Version(string _componentTag, int _majorVersion
                , int _minorVersion, int _buildVersion, int _fixVersion)
            : base(_majorVersion, _minorVersion, _buildVersion, _fixVersion)
        {
            _m_sComponentTag = _componentTag;
        }

        /**************
         * 返回输出版本号的头部信息
         * @return
         */
        protected override string printHeader { get { return _m_sComponentTag; } }

        /// <summary>
        /// 获取组件名称
        /// </summary>
        /// <returns></returns>
        public string getComponentTag()
        {
            return _m_sComponentTag;
        }



        #region Unity组件本身版本判断
        /****************
         * 提供方便判断版本号的判断函数。本函数判断当前版本是否达到目标版本
         * @param _majorV
         * @param _minorV
         * @param _buildV
         * @param _fixV
         * @param _fail_PrintMask 匹配失败，日志输出标识
         * @return
         */
        public bool judgeVersionIsEnough(int _majorV, int _minorV, int _buildV, int _fixV, string _fail_PrintMask)
        {
            //最低要求版本
            MJSDK_Component_Version target_min_version = new MJSDK_Component_Version(printHeader, _majorV, _minorV, _buildV, _fixV);
            //当前集成版本
            MJSDK_Component_Version curVersion = new MJSDK_Component_Version(_fail_PrintMask, m_componet_version.majorV, m_componet_version.minorV, m_componet_version.buildV, m_componet_version.fixV);
            return judgeVersionDependence(curVersion, target_min_version, "【Unity_Dependence_Unity】");
        }
        #endregion


        #region Unity组件与平台组件版本依赖判定
        /// <summary>
        /// 判断组件库依赖对应平台组件版本情况(实时获取平台组件对应版本信息)
        /// </summary>
        /// <param name="_majorV">平台库最低依赖版本号</param>
        /// <param name="_minorV">平台库最低依赖版本号</param>
        /// <param name="_buildV">平台库最低依赖版本号</param>
        /// <param name="_fixV">平台库最低依赖版本号</param>
        /// <param name="_isSucDelegate">结果回执</param>
        public void judgePlatformDependenceVersionIsEnoughBysys_version(int _majorV, int _minorV, int _buildV, int _fixV)
        {
            MJSDK_BasicLib.sys_version(_m_sComponentTag, (MJSDK_Version_Model _version) =>
            {
                //最低要求版本
                MJSDK_Component_Version target_min_version = new MJSDK_Component_Version(_m_sComponentTag, _majorV, _minorV, _buildV, _fixV);
                //当前集成版本
                MJSDK_Component_Version curVersion = new MJSDK_Component_Version(_m_sComponentTag, _version.majorV, _version.minorV, _version.buildV, _version.fixV);
                //版本比较
                judgeVersionDependence(curVersion, target_min_version, "【Unity_Dependence_Platform】");
            }, (int errCode, string errMsg) =>
            {

                string dependence_fail_msg =
                "【Unity_Dependence_Platform】组件【" + _m_sComponentTag + "】"
                + "依赖平台组件【" + _m_sComponentTag + "】失败。原因：errCode:" + errCode + "errMsg:" + errMsg;
                //处理组件版本依赖判定-失败信息
                dealJudgeDependenceFailMsg(_m_sComponentTag, dependence_fail_msg);
            });
        }


        /// <summary>
        /// 判断组件库依赖对应平台组件版本情况
        /// </summary>
        /// <param name="_platformVerison">当前集成的平台版本号</param>
        /// <param name="_majorV">平台库最低依赖版本号</param>
        /// <param name="_minorV">平台库最低依赖版本号</param>
        /// <param name="_buildV">平台库最低依赖版本号</param>
        /// <param name="_fixV">平台库最低依赖版本号</param>
        public bool judgePlatformDependenceVersionIsEnough(string _platformVerison, int _majorV, int _minorV, int _buildV, int _fixV)
        {
            try
            {
                //最低要求版本
                MJSDK_Component_Version target_min_version = new MJSDK_Component_Version(_m_sComponentTag, _majorV, _minorV, _buildV, _fixV);
                //当前集成版本
                string[] platformVerison_arr = _platformVerison.Split('.');
                MJSDK_Component_Version curVersion = new MJSDK_Component_Version(_m_sComponentTag, int.Parse(platformVerison_arr[0]), int.Parse(platformVerison_arr[1]), int.Parse(platformVerison_arr[2]), int.Parse(platformVerison_arr[3]));
                //版本比较
                return judgeVersionDependence(curVersion, target_min_version, "【Unity_Dependence_Platform】");
            }
            catch (Exception ex)
            {

                string dependence_fail_msg =
                    "【Unity_Dependence_Platform】组件【" + _m_sComponentTag + "】"
                    + "依赖平台组件【" + _m_sComponentTag + "】失败。原因：" + ex.Message;
                //处理组件版本依赖判定-失败信息
                dealJudgeDependenceFailMsg(_m_sComponentTag, dependence_fail_msg);
                return false;
            }
        }
        #endregion




        #region 版本号判定公共方法
        /// <summary>
        /// 通用版本号依赖关系判断方法
        /// </summary>
        /// <param name="_curVersion">当前集成组件版本信息</param>
        /// <param name="_target_min_version">最低依赖组件版本信息</param>
        /// <param name="_logHeader">日志头部</param>
        /// <returns></returns>
        private static bool judgeVersionDependence(MJSDK_Component_Version _curVersion, MJSDK_Component_Version _target_min_version, string _logHeader = "")
        {

            //标识符
            string curComponet_tag = _curVersion.getComponentTag();
            string targetMinComponet_tag = _target_min_version.getComponentTag();

#if MJSDK_UNITY_DEBUG
            MJSDK_Log.mjsdkLog("judgeVersionDependence");
            _curVersion.printVersion();
            _target_min_version.printVersion();
#endif

            try
            {
                //判断当前版本号是否大于最低要求版本号
                int compareResult = compareVersion(_curVersion.getVersionString(), _target_min_version.getVersionString());

                //平台集成版本号小于最低依赖版本
                if (compareResult == -1)
                {
                    string dependence_fail_msg =
                     _logHeader
                    + "Unity库组件【" + curComponet_tag + "】"
                    + "依赖平台组件【" + targetMinComponet_tag + "】失败。"
                    + "当前版本：" + _curVersion.getVersionString()
                    + ",最低要求版本：" + _target_min_version.getVersionString();
                    //处理组件版本依赖判定-失败信息
                    dealJudgeDependenceFailMsg(curComponet_tag, dependence_fail_msg);
                    return false;
                }

                return true;
            }
            catch (System.Exception ex)
            {
                string dependence_fail_msg =
                     _logHeader
                    + "组件【" + curComponet_tag + "】"
                    + "依赖平台组件【" + targetMinComponet_tag + "】失败。原因：" + ex.Message;
                //处理组件版本依赖判定-失败信息
                dealJudgeDependenceFailMsg(curComponet_tag, dependence_fail_msg);
                return false;
            }
        }

        //比较两个版本大小
        private static int compareVersion(string version1, string version2)
        {
            string[] parts1 = version1.Split('.');
            string[] parts2 = version2.Split('.');

            // Ensure we compare up to the maximum number of segments
            int maxLength = Math.Max(parts1.Length, parts2.Length);

            for (int i = 0; i < maxLength; i++)
            {
                int num1 = i < parts1.Length ? int.Parse(parts1[i]) : 0;
                int num2 = i < parts2.Length ? int.Parse(parts2[i]) : 0;

                if (num1 < num2)
                {
                    return -1;
                }
                else if (num1 > num2)
                {
                    return 1;
                }
            }
            // If all segments are equal, the versions are the same
            return 0;
        }

        /// <summary>
        /// 处理组件版本依赖判定-失败信息
        /// </summary>
        /// <param name="_componentTag">组件标识符</param>
        /// <param name="_errMsg">错误信息</param>
        public static void dealJudgeDependenceFailMsg(string _componentTag, string _errMsg)
        {
            //日志打印
            MJSDK_Log.mjsdkLog(_errMsg, E_MJSDK_BusType.Error_SDKInitErr);
            //错误回执-自动方式
            MJSDK.logicInterface.onMJSDKError(MJSDK_Error.C_Unity_Dependence_Platform_ComponentVersionErr, _errMsg);
            //存储组件初始化失败原因
            MJSDK_Tool_Lib.setComponentErrorMsg(_componentTag, _errMsg);
            //埋点上报
            MJSDK_BasicLib.mj_sdkTrace(MJSDK_Event.C_Unity_Event_Unity_Dependence_Platform_Fail, _errMsg);
        }
        #endregion
    }
}
