
using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// showcase的场景
    /// </summary>
    public class MainAdditionShowCaseTDScene : _ABasicAdditionTDScene
    {
        [NotNull]
        public static MainAdditionShowCaseTDScene instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new MainAdditionShowCaseTDScene();
                return _g_instance;
            }
        }
        private static MainAdditionShowCaseTDScene _g_instance;
        
        // 需要加载的场景配置
        private NPGSceneIndex _m_td_scene_idx;

        public MainAdditionShowCaseTDScene()
        {
            //先写死,后面读配置
            _m_td_scene_idx = GRefdataCoreMgr.instance.npGeneral.show_case_scene_index;
            if (_m_td_scene_idx == null || !_m_td_scene_idx.isValid())
            {
                Debug.LogError("showcase场景资源索引没有配置");
                _m_td_scene_idx = new NPGSceneIndex();
                _m_td_scene_idx.mainId = 1;
                _m_td_scene_idx.subId = 1;
            }
        }
        
        // 场景的mono
        private NPShowcaseSceneMono _m_showCaseMono;
        
        public NPShowcaseSceneMono showCaseMono { get { return _m_showCaseMono; } }


        /// <summary>
        /// 是否在切换的时候会被释放
        /// </summary>
        public override bool needDiscardOnSwitch { get { return false; } }
        /// <summary>
        /// 资源加载对象
        /// </summary>
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        /// <summary>
        /// 获得默认的场景
        /// </summary>
        protected override BasicResIndexInfo _getDefaultSceneIndex()
        {
            return _m_td_scene_idx;
        }
        
        /// <summary>
        /// scene初始化完成后做的操作
        /// </summary>
        protected override void _onSceneInited()
        {
        }
        
        /// <summary>
        /// 显示scene做的操作
        /// </summary>
        /// <param name="_delegate"></param>
        public override void _dealShowScene(Action _delegate)
        {
            _checkSceneAndEnter(() =>
            {
                if (null != _delegate)
                    _delegate();
            });
        }

        /// <summary>
        /// 隐藏本scene对象的操作
        /// </summary>
        public override void _dealHideScene(Action _delegate)
        {
            if (_delegate != null) 
                _delegate.Invoke();
        }

        /// <summary>
        /// 离开本附加Scene时的处理
        /// </summary>
        public override void onSwitchHideScene()
        {
            
        }
        
        protected override void _onQuitTDScene()
        {
            _m_showCaseMono = null;
        }

        //检测并确认本场景是否需要重新加载
        private bool _checkNeedReload()
        {
            //判断是否变动了场景，如变动则需要重新加载，暂时不做判断
            //一般使用sceneIndex进行一下判断
            if(null == sceneIndex)
                return true;

            NPGSceneIndex curIndex = _m_td_scene_idx;

            if(sceneIndex == null || curIndex == null)
                return true;

            return sceneIndex.mainId != curIndex.mainId || sceneIndex.subId != curIndex.subId;
        }
        
        private void _checkSceneAndEnter(Action _doneDelegate)
        {
            //判断是否需要重新加载
            if(!_checkNeedReload())
            {
                //注册初始化回调，如已经初始化则会直接调用
                regInitDelegate(_doneDelegate);
                return;
            }

            //需要先退出再进入
            quitScene();

            enterScene(_m_td_scene_idx, _doneDelegate);
        }

        protected override void _onRootGOLoaded(GameObject _go)
        {
            if (_m_showCaseMono == null)
                _m_showCaseMono = _go.GetComponent<NPShowcaseSceneMono>();
        }
    }
}
