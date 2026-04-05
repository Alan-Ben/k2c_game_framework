using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ALPackage
{
#if AL_PUERTS || AL_XLUA
    /// <summary>
    /// 单个Lua执行实例对象，包含相关信息的缓存处理
    /// </summary>
    public class ALSTUGUICommon
    {
        /// <summary>
        /// 根据子对象路径，查询对应的Text对象
        /// </summary>
        /// <param name="_mono"></param>
        /// <param name="_childPath"></param>
        /// <returns></returns>
        public static Text getMonoChildText(MonoBehaviour _mono, string _childPath)
        {
            if (null == _mono)
                return null;

            return getMonoChildText(_mono.transform, _childPath);
        }

        /// <summary>
        /// 根据子对象路径，查询对应的Text对象
        /// </summary>
        /// <param name="_trans"></param>
        /// <param name="_childPath"></param>
        /// <returns></returns>
        public static Text getMonoChildText(Transform _trans, string _childPath)
        {
            if (null == _trans)
                return null;

            Transform childTrans = _trans.Find(_childPath);
            if (null == childTrans)
                return null;

            return childTrans.GetComponent<Text>();
        }

        /// <summary>
        /// 根据子对象路径，查询对应的GameObject对象
        /// </summary>
        /// <param name="_mono"></param>
        /// <param name="_childPath"></param>
        /// <returns></returns>
        public static GameObject getMonoChildGO(MonoBehaviour _mono, string _childPath)
        {
            if (null == _mono)
                return null;

            return getMonoChildGO(_mono.transform, _childPath);
        }

        /// <summary>
        /// 根据子对象路径，查询对应的Text对象
        /// </summary>
        /// <param name="_trans"></param>
        /// <param name="_childPath"></param>
        /// <returns></returns>
        public static GameObject getMonoChildGO(Transform _trans, string _childPath)
        {
            if (null == _trans)
                return null;

            Transform childTrans = _trans.Find(_childPath);
            if (null == childTrans)
                return null;

            return childTrans.gameObject;
        }
    }
#endif
}
