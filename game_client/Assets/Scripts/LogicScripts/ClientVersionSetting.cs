using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 客户端版本信息存储对象
    /// </summary>
    [System.Serializable]
    public class ClientVersionSetting : ScriptableObject
    {
         private static ClientVersionSetting g_instance = null;

        public static ClientVersionSetting instance
        {
            get {
                if (g_instance == null && _AALMonoMain.instance != null) {
                    g_instance = (_AALMonoMain.instance as MainCameraMono).npClientVersionSetting;
                }

                if (null == g_instance) {
                    Debug.Log("load ClientVersionSetting from resource");
                    g_instance = Resources.Load(objName, typeof(ClientVersionSetting)) as ClientVersionSetting;
                }

                if (null == g_instance) {
                    UnityEngine.Debug.LogError("There is no clientVersionSetting file at " + assetPath);
                }

                return g_instance;
            }
        }

        //当前客户端版本号
        [NotNull] public WCGClientInfo ClientVersionInfo;

        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "Assets/App_Resources/Resources/client_version_setting.asset"; } }
        public static string objName { get { return "client_version_setting"; } }
    }
}


