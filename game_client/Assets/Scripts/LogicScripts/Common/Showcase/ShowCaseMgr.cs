using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class ShowCaseMgr
    {
        private static ShowCaseMgr _g_instance = new ShowCaseMgr();
        public static ShowCaseMgr instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new ShowCaseMgr();
                return _g_instance;
            }
        }

        //是否初始化完成
        private bool _m_isInitDone;

        //showcase对象列表
        /** 总的缓存队列 */
        [NotNull]private List<ShowcaseInfo> _m_lTotalCacheList = new List<ShowcaseInfo>();
        /** 还可使用的缓存队列 */
        [NotNull]private List<ShowcaseInfo> _m_lEnableCacheList = new List<ShowcaseInfo>();
        /** 已经被使用的缓存队列 */
        [NotNull]private List<ShowcaseInfo> _m_lUsedItemList = new List<ShowcaseInfo>();
        
        //开始初始化
        public void init(Action _doneDelegate)
        {
            if (_m_isInitDone)
            {
                if (null != _doneDelegate)
                    _doneDelegate();
                return;
            }

            _m_isInitDone = true;
            _onInit(_doneDelegate);
        }
        
        private void _onInit(Action _doneDelegate)
        {
            //加载showCase场景，场景常驻
            MainAdditionShowCaseTDScene.instance.enterAndShowScene(() =>
            {
                MainAdditionShowCaseTDScene scene = MainAdditionShowCaseTDScene.instance;
                if(null == scene.showCaseMono || null == scene.showCaseMono.showcaseRootList)
                {
                    Debug.LogError("Showcase场景里，所有根节点都没有挂ShowcaseSceneMono，请检查！！");
                    if (null != _doneDelegate)
                        _doneDelegate();
                    return;
                }
                
                //根据场景配置创建对应showcase实例
                for (int i = 0; i < scene.showCaseMono.showcaseRootList.Count; i++)
                {
                    ShowcaseInfo showcaseInfo = new ShowcaseInfo(scene.showCaseMono.showcaseRootList[i]);
                    _m_lTotalCacheList.Add(showcaseInfo);
                    _m_lEnableCacheList.Add(showcaseInfo);
                }
                
                if (null != _doneDelegate)
                    _doneDelegate();
            });
        }

        /// <summary>
        /// 获取一个有效的对象
        /// </summary>
        /// <returns></returns>
        public ShowcaseInfo popAvailableInfo()
        {
            if (!_m_isInitDone)
            {
                Debug.LogError($"showcase管理器未初始化就使用");
                return null;
            }
            
            if (_m_lEnableCacheList.Count <= 0)
            {
                Debug.LogError($"showcase使用超过配置上限，上限：{_m_lTotalCacheList.Count}");
                return null;
            }

            //取出最后一个对象
            ShowcaseInfo firstItem = _m_lEnableCacheList[_m_lEnableCacheList.Count - 1];
            _m_lEnableCacheList.RemoveAt(_m_lEnableCacheList.Count - 1);
            //放入使用队列
            _m_lUsedItemList.Add(firstItem);
#if UNITY_EDITOR
            if (firstItem == null)
            {
                Debug.LogError($"showcase中拿到的对象为空，外面应该有哪里持有的这个引用并且销毁了对象，请认真检查!");
            }
#endif
            return firstItem;
        }

        /// <summary>
        /// 回收对象
        /// </summary>
        /// <param name="_showcaseInfo"></param>
        public void pushBackInfo(ShowcaseInfo _showcaseInfo)
        {
            if (!_m_isInitDone)
            {
                Debug.LogError($"showcase管理器未初始化就使用");
                return;
            }
            
            if (null == _showcaseInfo)
            {
#if UNITY_EDITOR
                UnityEngine.Debug.LogError("showcase中pushBack了一个空对象，请检查逻辑代码是否有问题");
#endif
                return;
            }
            
            //从使用队列删除 判断是否成功删除，未成功删除则不处理
            if(!_m_lUsedItemList.Remove(_showcaseInfo))
                return;
            
            //设置对象无效
            _showcaseInfo.disableShowCase();
            
            //放入缓存队列
            _m_lEnableCacheList.Add(_showcaseInfo);
        }

        //根据标记获取showcase对象，目前标记就用资源prefab名字就行
        public ShowcaseInfo getShowCaseInfoByName(string _actorName)
        {
            if (_m_lUsedItemList.Count == 0)
                return null;

            foreach (ShowcaseInfo showcaseInfo in _m_lUsedItemList)
            {
                if(null == showcaseInfo)
                    continue;

                if (null != showcaseInfo.TemplateIndex && showcaseInfo.TemplateIndex.objName == _actorName)
                {
                    return showcaseInfo;
                }
            }

            return null;
        }
        
        //销毁
        public void discardAll()
        {
            if (!_m_isInitDone)
            {
                Debug.LogError($"showcase管理器未初始化就使用");
            }
            
            foreach (ShowcaseInfo showcaseInfo in _m_lTotalCacheList)
            {
                if(null == showcaseInfo)
                    continue;
                showcaseInfo.disableShowCase();
            }
            _m_lTotalCacheList.Clear();
            
            _m_lEnableCacheList.Clear();
            _m_lUsedItemList.Clear();

            _m_isInitDone = false;
        }
    }
}