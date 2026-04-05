using System;
using System.Collections.Generic;

using UnityEngine;

namespace ALPackage
{
    /*********************
     * 全局设置对象
     * 固定读取配置路径为 Assets/Resources/ALSetting.asset
     **/
    [System.Serializable]
    public class ALSOGlobalSetting : ScriptableObject
    {
        private static ALSOGlobalSetting g_instance = null;

        public static ALSOGlobalSetting Instance
        {
            get
            {
                if (null == g_instance)
                    g_instance = Resources.Load("ALGlobalSetting", typeof(ALSOGlobalSetting)) as ALSOGlobalSetting;

                if(null == g_instance)
                {
                    UnityEngine.Debug.LogError("There is not global setting file at 'Assets/Resources/ALGlobalSetting.asset'");
                    g_instance = new ALSOGlobalSetting();
                    g_instance.logLevel = ALLogLevel.ERROR;
                }

#if UNITY_EDITOR
                //editor如果高于sys默认设置为sys
                if(g_instance.logLevel > ALLogLevel.SYS_OUT)
                    g_instance.logLevel = ALLogLevel.SYS_OUT;
#endif

                return g_instance;
            }
        }

        public ALLogLevel logLevel = ALLogLevel.WARNING;

#if UNITY_EDITOR
        /** combin是否使用阴影 */
        public UnityEngine.Rendering.ShadowCastingMode shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
#endif

#if AL_CREATURE_SYS
        /** 对象停止下落的高度 */
        public float stopDropHeight = 0.2f;
        /** 对象判断是否下落的高度 */
        public float judgeDropHeight = 0.5f;
#endif

        public ALSOGlobalSetting()
        {
        }
    }
}
