using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
#if AL_XLUA
    [XLua.LuaCallCSharp]
#endif
    /// <summary>
    /// 全局注册动画控制对象的管理器
    /// </summary>
    public class PrefabControllerMgr
    {
        private static PrefabControllerMgr _g_instance;
        [NotNull]public static PrefabControllerMgr instance
        {
            get
            {
                if(_g_instance == null) {
                    _g_instance = new PrefabControllerMgr();
                }
                return _g_instance;
            }
        }
        
        
        [NotNull]private Dictionary<string, List<PrefabControlRegInfo>> _m_animationMonoDict = new Dictionary<string, List<PrefabControlRegInfo>>();

        /// <summary>
        /// 注册
        /// </summary>
        public void regMono(PrefabControlMono _mono)
        {
            if(null == _mono || null == _mono.dataList)
                return;
            
            _mono.dataList.ForEach(_data =>
            {
                regMono(_data);
            });
        }

        /// <summary>
        /// 注册
        /// </summary>
        public void regMono(PrefabControlRegInfo _regInfo)
        {
            if(null == _regInfo || string.IsNullOrEmpty(_regInfo.controlName))
                return;
            
            List<PrefabControlRegInfo> list = __safeGetItem(_regInfo.controlName);

            if(null != list)
                list.Add(_regInfo);
        }
        
        /// <summary>
        /// 删除
        /// </summary>
        public void unregMono(PrefabControlMono _mono)
        {
            if(null == _mono || null == _mono.dataList)
                return;
            
            _mono.dataList.ForEach(_data =>
            {
                unregMono(_data);
            });
        }

        /// <summary>
        /// 删除
        /// </summary>
        public void unregMono(PrefabControlRegInfo _regInfo)
        {
            if(null == _regInfo || string.IsNullOrEmpty(_regInfo.controlName))
                return;
            
            List<PrefabControlRegInfo> list = __getItem(_regInfo.controlName);

            if (null != list)
            {
                list.Remove(_regInfo);
                //判断是否需要删除队列
                if(list.Count <= 0)
                    _m_animationMonoDict.Remove(_regInfo.controlName);

                //移除加载对象
                _regInfo.discard();
            }
        }

        /// <summary>
        /// 为指定标记的对象加载子对象
        /// </summary>
        /// <param name="_tag"></param>
        /// <param name="_uiResPathId"></param>
        public void loadPrefab(string _tag, long _uiResPathId, float _duration = 0)
        {
            List<PrefabControlRegInfo> list = __getItem(_tag);
            if (null == list)
                return;

            PrefabControlRegInfo tmpInfo = null;
            for (int i = 0; i < list.Count; i++)
            {
                tmpInfo = list[i];
                if (null == tmpInfo)
                    continue;

                tmpInfo.loadPrefab(_uiResPathId, _duration);
            }
        }

        /// <summary>
        /// 为指定标记的对象销毁子对象
        /// </summary>
        /// <param name="_tag"></param>
        public void discardPrefab(string _tag)
        {
            List<PrefabControlRegInfo> list = __getItem(_tag);
            if (null == list)
                return;

            PrefabControlRegInfo tmpInfo = null;
            for (int i = 0; i < list.Count; i++)
            {
                tmpInfo = list[i];
                if (null == tmpInfo)
                    continue;

                tmpInfo.discard();
            }
        }

        /// <summary>
        /// 安全的获取函数，必定返回一个列表
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        private List<PrefabControlRegInfo> __safeGetItem(string _name)
        {
            if(null == _name)
                return null;

            List<PrefabControlRegInfo> list = null;
            if(!_m_animationMonoDict.TryGetValue(_name, out list))
            {
                list = new List<PrefabControlRegInfo>();
                _m_animationMonoDict.Add(_name, list);
            }

            return list;
        }
        private List<PrefabControlRegInfo> __getItem(string _name)
        {
            if(null == _name)
                return null;
            
            List<PrefabControlRegInfo> list = null;
            if(!_m_animationMonoDict.TryGetValue(_name, out list))
            {
                return null;
            }

            return list;
        }
    }
}