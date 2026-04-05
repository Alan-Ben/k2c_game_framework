using System;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 动画对象注册的信息
    /// </summary>
    [Serializable]
    public class PrefabControlRegInfo
    {
        [ALHeader("加载预制体的注册名称")]
        public string controlName;
        [ALHeader("加载预制体挂载的父节点对象")]
        public Transform parentGo;

#if NP_GAME
        /// <summary>
        /// 实际加载保存的对象，需要保证加载的唯一性
        /// 一个对象下同一时间只支持加载一个对象
        /// </summary>
        [System.NonSerialized]
        private NPGSubPrefab _m_spSubPrefab;
#endif

        [System.NonSerialized]
        private long _m_serialize = 0;
        
        #region 控制接口

        public void loadPrefab(long _uiPathId, float _duration = 0)
        {
            _m_serialize = ALSerializeOpMgr.next();

            //释放当前资源
            _discardSubPrefab();

            if (0 == _uiPathId)
                return;

#if NP_GAME
            //加载新资源
            NPUIResPathRefObj pathRef = GRefdataCoreMgr.instance.uiResPathRefCore.getRef(_uiPathId);
            if(null == pathRef)
            {
                ALLog.Error($"can not find ui res path[{_uiPathId}]");

                return;
            }

            //加载新对象
            _m_spSubPrefab = new NPGSubPrefab(pathRef.asset_path, pathRef.obj_name, parentGo);
            //调用加载
            _m_spSubPrefab.load(_m_spSubPrefab.show);

            long serialize = _m_serialize;
            //n秒后卸载
            if (_duration > 0)
            {
                //停留n秒
                ALCommonTaskController.CommonActionAddMonoTask(() =>
                {
                    if(serialize != _m_serialize)
                        return;
                    _discardSubPrefab();
                }, _duration);
            }
#endif
        }

        public void discard()
        {
            _m_serialize = ALSerializeOpMgr.next();
            
            _discardSubPrefab();
        }

        /// <summary>
        /// 释放当前的加载对象
        /// </summary>
        protected void _discardSubPrefab()
        {
#if NP_GAME
            if (null != _m_spSubPrefab)
                _m_spSubPrefab.discard();

            _m_spSubPrefab = null;
#endif
        }

#endregion
    }
    /// <summary>
    /// 注册动画控制对象的mono对象
    /// </summary>
    public class PrefabControlMono : MonoBehaviour
    {
        /// <summary>
        /// 需要注册的数据列表
        /// </summary>
        public List<PrefabControlRegInfo> dataList;

        //是否需要检测
        private bool _m_bNeedCheck = false;

        //有效和无效的时候分别注册和注销显示对象
        private void OnEnable()
        {
            _m_bNeedCheck = true;

            //到管理对象中进行处理
            ALCommonActionMonoTask.addNextFrameTask(_check);
        }

        private void OnDisable()
        {
            _m_bNeedCheck = true;

            //到管理对象中进行处理
            ALCommonActionMonoTask.addNextFrameTask(_check);
        }

        private void OnDestroy()
        {
            _m_bNeedCheck = true;
            //直接检测
            _check();
        }

        protected void _check()
        {
            if (!_m_bNeedCheck || null == this || null == gameObject)
            {
#if UNITY_EDITOR
                if (_m_bNeedCheck)
                    UnityEngine.Debug.LogError("Check mono Enable error!");
#endif
                return;
            }

            _m_bNeedCheck = false;

#if NP_GAME
            if (gameObject.activeInHierarchy)
                //到管理对象中进行处理
                PrefabControllerMgr.instance.regMono(this);
            else
                //到管理对象中进行处理
                PrefabControllerMgr.instance.unregMono(this);
#endif
        }
    }
}
