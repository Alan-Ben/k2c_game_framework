using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

/******************
 * 场景管理对象
 **/
namespace ALPackage
{
    public class ALSceneMgr
    {
        private static ALSceneMgr _g_instance = new ALSceneMgr();
        public static ALSceneMgr instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new ALSceneMgr();
                return _g_instance;
            }
        }

        /// <summary>
        /// 场景附加加载方式的处理类型，分为默认和非重复
        /// 非重复处理上会记录已经加载过的同索引场景，避免重复加载
        /// 不过非重复加载的场景在释放时也需要释放同样的次数才可真正释放
        /// </summary>
        public enum EALSceneAddLoadType
        {
            ALWARYS_NEW,
            KEEP_SINGLE,
        }

        /** 当前场景的加载序列号 */
        private int _m_iSerialize;
        /** 加载的 single Scene 对象，用此对象进行存储和进行互斥处理 */
        private ALSceneInfo _m_siSingleScene;
        /** 附加的场景信息队列 */
        private List<ALSceneInfo> _m_lSceneInfoList;
        /// <summary>
        /// 用add方式加载的非重复类型scene数据集
        /// </summary>
        private Dictionary<string, ALSceneInfo> _m_dAddLoadSingleTypeDic;
        //原生场景的hash值队列
        private List<int> _m_lSrcHashList;

        protected ALSceneMgr()
        {
            _m_iSerialize = 1;

            _m_siSingleScene = null;
            _m_lSceneInfoList = new List<ALSceneInfo>();
            _m_dAddLoadSingleTypeDic = new Dictionary<string, ALSceneInfo>();

            //初始化原生hash值队列
            _m_lSrcHashList = new List<int>();
            int sceneCount = SceneManager.sceneCount;
            Scene tmpScene;
            for(int i = 0; i < sceneCount; i++)
            {
                tmpScene = SceneManager.GetSceneAt(i);
                if(null == tmpScene)
                    continue;

                _m_lSrcHashList.Add(tmpScene.GetHashCode());
            }
        }

        protected int _newSerialize { get { return _m_iSerialize++; } }

        /******************
         * 加载对应名称的场景
         * single场景只与single场景互斥
         **/
        public void loadSingleSceneSyn(string _sceneName, Action<ALSceneInfo> _doneDelegate)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.LogWarning("loadSingleScene: " + _sceneName);
#endif

            //判断当前的独立场景是否一致，一致则直接返回
            if(null != _m_siSingleScene && _m_siSingleScene.isSceneLoaded && _m_siSingleScene.sceneName.Equals(_sceneName))
            {
                if (null != _doneDelegate)
                    _doneDelegate(_m_siSingleScene);

                return;
            }

            //此时直接释放原独立场景
            if(null != _m_siSingleScene)
            {
                _m_siSingleScene.discard();
                _m_siSingleScene = null;
            }

            //此时开启新的场景加载操作
            //SceneManager.LoadScene(_sceneName, LoadSceneMode.Additive);
            Scene newScene = SceneManager.LoadScene(_sceneName, new LoadSceneParameters(LoadSceneMode.Additive));
            if(null == newScene)
            {
                //新场景无效，直接当作完成
                if(null != _doneDelegate)
                    _doneDelegate(null);
            }
            else
            {
                //插入一个新的场景节点
                ALSceneInfo sceneInfo = new ALSceneInfo(_newSerialize, _sceneName, LoadSceneMode.Additive);
                //设置单例对象
                _m_siSingleScene = sceneInfo;

                //开启新场景加载监控
                _m_siSingleScene.loadScene(newScene, _doneDelegate);
            }
        }
        public void loadAddSceneSyn(string _sceneName, Action<ALSceneInfo> _doneDelegate, EALSceneAddLoadType _loadType = EALSceneAddLoadType.ALWARYS_NEW)
        {
            if(_AALMonoMain.instance.showDebugOutput && ALSOGlobalSetting.Instance.logLevel <= ALLogLevel.DEBUG)
            {
                UnityEngine.Debug.Log($"【{Time.frameCount}】[Asset]load Unity Scene Sync: {_sceneName}");
            }

            ALSceneInfo sceneInfo = null;
            if(_loadType == EALSceneAddLoadType.KEEP_SINGLE)
            {
                //尝试获取索引，如获取到则直接操作并返回
                if(_m_dAddLoadSingleTypeDic.TryGetValue(_sceneName, out sceneInfo) && null != sceneInfo)
                {
                    //处理加载操作后直接返回，加载操作中有累加行为
                    sceneInfo.loadScene(null, null, _doneDelegate);
                    return;
                }
            }

            //后面当做新场景进行加载处理
            //此时开启新的场景加载操作
            //SceneManager.LoadScene(_sceneName, LoadSceneMode.Additive);
            Scene newScene = SceneManager.LoadScene(_sceneName, new LoadSceneParameters(LoadSceneMode.Additive));
            if(null == newScene)
            {
                //新场景无效，直接当作完成
                if(null != _doneDelegate)
                    _doneDelegate(null);
            }
            else
            {
                //插入一个新的场景节点
                sceneInfo = new ALSceneInfo(_newSerialize, _sceneName, LoadSceneMode.Additive);

                //插入队列
                _m_lSceneInfoList.Add(sceneInfo);
                //如果是keep single的则需要加入到数据集中
                if(_loadType == EALSceneAddLoadType.KEEP_SINGLE)
                    _m_dAddLoadSingleTypeDic.Add(_sceneName, sceneInfo);

                //开启新场景加载监控
                if(null != sceneInfo)
                    sceneInfo.loadScene(newScene, _doneDelegate);
                else
                    UnityEngine.Debug.LogError("加载场景数据错误！");
            }
        }

        public void loadSingleScene(string _sceneName, Action<float> _processDelegate, Action<ALSceneInfo> _doneDelegate)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.LogWarning("loadSingleScene: " + _sceneName);
#endif

            //判断当前的独立场景是否一致，一致则直接返回
            if (null != _m_siSingleScene && _m_siSingleScene.isSceneLoaded && _m_siSingleScene.sceneName.Equals(_sceneName))
            {
                if (null != _processDelegate)
                    _processDelegate(1f);
                if (null != _doneDelegate)
                    _doneDelegate(_m_siSingleScene);

                return;
            }

            //此时直接释放原独立场景
            if (null != _m_siSingleScene)
            {
                _m_siSingleScene.discard();
                _m_siSingleScene = null;
            }

            //此时开启新的场景加载操作
            AsyncOperation asyncOp = SceneManager.LoadSceneAsync(_sceneName, LoadSceneMode.Additive);
            if (null == asyncOp)
            {
                //新场景无效，直接当作完成
                if (null != _processDelegate)
                    _processDelegate(1);
                if (null != _doneDelegate)
                    _doneDelegate(null);
            }
            else
            {
                //插入一个新的场景节点
                ALSceneInfo sceneInfo = new ALSceneInfo(_newSerialize, _sceneName, LoadSceneMode.Additive);
                //插入队列
                _m_siSingleScene = sceneInfo;

                //开启新场景加载监控
                _m_siSingleScene.loadScene(asyncOp, _processDelegate, _doneDelegate);
            }
        }
        public void loadAddScene(string _sceneName, Action<float> _processDelegate, Action<ALSceneInfo> _doneDelegate, EALSceneAddLoadType _loadType = EALSceneAddLoadType.ALWARYS_NEW)
        {
            if(_AALMonoMain.instance.showDebugOutput && ALSOGlobalSetting.Instance.logLevel <= ALLogLevel.DEBUG)
            {
                UnityEngine.Debug.Log($"【{Time.frameCount}】[Asset]load Unity Scene Async: {_sceneName}");
            }

            ALSceneInfo sceneInfo = null;
            if(_loadType == EALSceneAddLoadType.KEEP_SINGLE)
            {
                //尝试获取索引，如获取到则直接操作并返回
                if(_m_dAddLoadSingleTypeDic.TryGetValue(_sceneName, out sceneInfo) && null != sceneInfo)
                {
                    //处理加载操作后直接返回
                    sceneInfo.loadScene(null, _processDelegate, _doneDelegate);
                    return;
                }
            }

            //后面当做新场景进行加载处理
            //此时开启新的场景加载操作
            AsyncOperation asyncOp = SceneManager.LoadSceneAsync(_sceneName, LoadSceneMode.Additive);
            if (null == asyncOp)
            {
                //新场景无效，直接当作完成
                if (null != _processDelegate)
                    _processDelegate(1);
                if (null != _doneDelegate)
                    _doneDelegate(null);
            }
            else
            {
                //插入一个新的场景节点
                sceneInfo = new ALSceneInfo(_newSerialize, _sceneName, LoadSceneMode.Additive);

                //插入队列
                _m_lSceneInfoList.Add(sceneInfo);
                //如果是keep single的则需要加入到数据集中
                if(_loadType == EALSceneAddLoadType.KEEP_SINGLE)
                    _m_dAddLoadSingleTypeDic.Add(_sceneName, sceneInfo);

                //开启新场景加载监控
                if(null != sceneInfo)
                    sceneInfo.loadScene(asyncOp, _processDelegate, _doneDelegate);
                else
                    UnityEngine.Debug.LogError("加载场景数据错误！");
            }
        }

        /// <summary>
        /// 设置同步加载的Scene对象信息
        /// </summary>
        /// <param name="_sceneInfo"></param>
        public void checkAndSetSyncSceneInfoObj(ALSceneInfo _sceneInfo, Scene _sceneObj)
        {
            if(null == _sceneInfo)
                return;

            //如果有新场景则进行比对后进行处理
            if(_sceneObj.name.Equals(_sceneInfo.sceneName))
            {
                //设置数据对象并返回
                if(!_sceneInfo.setSceneObj(_sceneObj))
                {
                    //设置失败则需要删除此场景
                    SceneManager.UnloadSceneAsync(_sceneObj);
                    //SceneManager.UnloadScene(finalScene);
                }

                //返回
                return;
            }
            else
            {
#if UNITY_EDITOR
                UnityEngine.Debug.LogError("发现一个未识别的加载场景对象（也可能是有2个scene同时加载成功，请注意区分）:" + _sceneObj.name);
#endif
            }

            //执行到此时说明场景加载失败
            if(null != _sceneInfo)
            {
                _sceneInfo.onLoadFail();
            }
        }

        /********************
         * 设置场景信息中的相关对象，此函数在场景加载完成之后调用
         * 此时需要匹配所有场景，取出新的场景并放入对应信息对象中
         **/
        public void checkAndSetSceneInfoObj(ALSceneInfo _sceneInfo)
        {
            if (null == _sceneInfo)
                return;

            int sceneCount = SceneManager.sceneCount;
            //是否新场景
            bool isNew = true;
            Scene finalScene;
            for(int i = 0; i < sceneCount; i++)
            {
                isNew = true;
                finalScene = SceneManager.GetSceneAt(i);
                if(!finalScene.IsValid())
                    continue;

                int hashCode = finalScene.GetHashCode();
                if(_m_lSrcHashList.Contains(hashCode))
                {
                    //如果是原始场景则不做任何处理
                    continue;
                }

                //逐个与已加载scene比较
                for (int n = 0; n < _m_lSceneInfoList.Count; n++)
                {
                    if(_m_lSceneInfoList[n].sceneObj.GetHashCode() == hashCode)
                    {
                        isNew = false;
                        break;
                    }
                }

                //队列没找到则根据单例对象进行判断
                if(isNew && null != _m_siSingleScene && _m_siSingleScene.sceneObj.GetHashCode() == hashCode)
                {
                    isNew = false;
                }

                //如果不是新场景且与需要设置的对象一致
                if(isNew)
                {
                    //如果有新场景则进行比对后进行处理
                    if(finalScene.name.Equals(_sceneInfo.sceneName))
                    {
                        //设置数据对象并返回
                        if(!_sceneInfo.setSceneObj(finalScene))
                        {
                            //设置失败则需要删除此场景
                            SceneManager.UnloadSceneAsync(finalScene);
                            //SceneManager.UnloadScene(finalScene);
                        }

                        //返回
                        return;
                    }
                    else
                    {
#if UNITY_EDITOR
                        UnityEngine.Debug.LogError("发现一个未识别的加载场景对象（也可能是有2个scene同时加载成功，请注意区分）:" + finalScene.name);
#endif
                    }
                }
            }

            //执行到此时说明场景加载失败
            if(null != _sceneInfo)
            {
                _sceneInfo.onLoadFail();
            }
        }

        /*******************
         * 从场景信息队列中删除对应场景信息
         **/
        protected internal void _removeSceneInfo(ALSceneInfo _sceneInfo)
        {
            if(null == _sceneInfo)
                return;

            for(int i = 0; i < _m_lSceneInfoList.Count; i++)
            {
                if(_m_lSceneInfoList[i].sceneSerialize == _sceneInfo.sceneSerialize)
                {
                    _m_lSceneInfoList.RemoveAt(i);
                    break;
                }
            }

            //判断是否单例对象
            if (null != _m_siSingleScene && _m_siSingleScene.sceneSerialize == _sceneInfo.sceneSerialize)
            {
                _m_siSingleScene = null;
            }
            else
            {
                //这里要注意这个数据集删除需要与单例的处理区分开，避免同名场景带来逻辑错乱
                //尝试从数据集中删除对应的场景
                ALSceneInfo curScene = null;
                if (_m_dAddLoadSingleTypeDic.TryGetValue(_sceneInfo.sceneName, out curScene))
                {
                    //判断是否一致，是则删除
                    if (curScene == _sceneInfo)
                        _m_dAddLoadSingleTypeDic.Remove(_sceneInfo.sceneName);
                }
            }
        }
    }
}
