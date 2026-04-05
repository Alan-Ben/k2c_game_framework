
using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering;

namespace GOE
{
	/// <summary>
	/// 灯光配置的内部管理对象，这个对象只实现最简单的单个灯光控制问题，不做外围逻辑管理
	/// 所以这个对象一般外部不使用，而是通过LightMgr管理器进行调用
	/// </summary>
	public class LightsInnerController
    {
	    private static LightsInnerController _g_instance = new LightsInnerController();
	    public static LightsInnerController instance
	    {
	        get
	        {
	            if (null == _g_instance)
	                _g_instance = new LightsInnerController();
	            return _g_instance;
	        }
	    }

		//当前使用的灯光配置索引
	    private long _m_lCurLightKey;
	    private GameObject _m_gCurLightGo;
	    [NotNull]private readonly GameObject _m_gRoot;

		/// <summary>
		/// 存储不同灯光配置的对象池
		/// </summary>
		private Dictionary<long, GameObject> _m_dicLightPrefabDic;
		// 灯光开启序列号
		private int _m_iOpenSerialize;

	    private LightsInnerController()
	    {
			_m_lCurLightKey = 0;
			_m_gCurLightGo = null;
            _m_gRoot = new GameObject("LightsRoot");
		    GameObject.DontDestroyOnLoad(_m_gRoot);

			_m_dicLightPrefabDic = new Dictionary<long, GameObject>();
        }

	    /// <summary>
	    /// 开启环境灯光配置
	    /// </summary>
	    public void openLight(GLightGoIndex _index, Action _doneDelegate = null)
	    {
		    //无效数据不处理
		    if (!_index.isValid())
		    {
			    _doneDelegate?.Invoke();
			    return;
		    }
		    
			long lightKey = _getIndexKey(_index);
			//如果key为0，说明配表没配，默认关闭灯光
			if (lightKey == 0)
			{
				closeLight();
				_doneDelegate?.Invoke();
				return;
			}
            //判断当前索引是否一致，一致则不需要开启
            if (_m_lCurLightKey == lightKey)
            {
                _doneDelegate?.Invoke();
                return;
            }

			//不一致则尝试加载新配置
			//先判断是否已经有缓存
			if(_openCacheLight(lightKey))
            {
                _doneDelegate?.Invoke();
                return;
            }

			int serialize = _m_iOpenSerialize = ALSerializeOpMgr.next();
			//不在缓存中则加载
			_loadLight(_index
				, () =>
				{
					if (serialize != _m_iOpenSerialize)
					{
						_doneDelegate?.Invoke();
						return;
					}
					
					//加载完成后再次判断是否已经有缓存
					_openCacheLight(lightKey);

                    _doneDelegate?.Invoke();
                });
	    }
	    

	    /// <summary>
	    /// 关闭环境灯光配置
	    /// </summary>
	    public void closeLight()
	    {
			_m_lCurLightKey = 0;
            //关闭旧灯光
            ALUGUICommon.setGameObjEnable(_m_gCurLightGo, false);
            QualityMgr.instance.closePostProcessing();
            _m_gCurLightGo = null;
			_m_iOpenSerialize = ALSerializeOpMgr.next();
	    }

        /// <summary>
        /// 从缓存中获取灯光配置，并开启灯光
        /// </summary>
        /// <param name="_lightKey"></param>
        protected bool _openCacheLight(long _lightKey)
        {
			//判断是否已经有缓存
			if (!_m_dicLightPrefabDic.ContainsKey(_lightKey))
				return false;

            //关闭旧灯光
            closeLight();

            //这里直接开启灯光，并返回
            _m_lCurLightKey = _lightKey;
            _m_gCurLightGo = _m_dicLightPrefabDic[_lightKey];
            //开启新灯光
            ALUGUICommon.setGameObjEnable(_m_gCurLightGo, true);

            if (_m_gCurLightGo != null)
            {
	            Volume volume = _m_gCurLightGo.GetComponentInChildren<Volume>();
	            if (volume != null && volume.enabled)
	            {
		            QualityMgr.instance.openPostProcessing();
	            }
            }
          

			return true;
        }

		/// <summary>
		/// 加载灯光配置
		/// 灯光加载应该全部都是同步的，不然会出现灯光加载不出来的情况
		/// </summary>
		/// <param name="_index"></param>
		protected void _loadLight(GLightGoIndex _index, Action _onLoadDone)
        {
			if (null == _index)
				return;

#if UNITY_EDITOR
            ALLocalResLoaderMgr.instance?.loadSynUIAsset(_index.assetPath, _index.objName
				, (bool _isSuc, ALAssetBundleObj _assetObj) =>
                {
                    if (!_isSuc || _assetObj == null)
                    {
                        Debug.LogError($"Load Light Asset: {_index.assetPath}, {_index.objName}, res file load fail!");
						_onLoadDone?.Invoke();
                        return;
                    }

                    // 加载objName对应的资源
                    GameObject temp = _assetObj.load<GameObject>(_index.objName);
                    if (temp == null)
                    {
                        Debug.LogError($"Load Light Asset: {_index.assetPath}, {_index.objName}, res file load from AB fail!");
                        _onLoadDone?.Invoke();
                        return;
                    }

                    // 调用完成回调
                    _onLightGoLoaded(_index, temp);

                    _onLoadDone?.Invoke();
                }
                , null
                , (GameObject _go) =>
                {
                    if (null == _go)
                    {
                        Debug.LogError($"Load GUI Asset: {_index.assetPath}, {_index.objName}, res file load fail!");
                        _onLoadDone?.Invoke();
                        return;
                    }

                    // 调用完成回调
                    _onLightGoLoaded(_index, _go);

                    _onLoadDone?.Invoke();
                }
                , GameResCore.instance);
#else
            if (GameResCore.instance != null)
            {
                GameResCore.instance.loadSynAsset(_index.assetPath
					, (bool _isSuc, ALAssetBundleObj _assetObj) =>
						{
							if (!_isSuc || _assetObj == null)
							{
								Debug.LogError($"Load Light Asset: {_index.assetPath}, {_index.objName}, res file load fail!");
								_onLoadDone?.Invoke();
								return;
							}

							// 加载objName对应的资源
							GameObject temp = _assetObj.load<GameObject>(_index.objName);
							if (temp == null)
							{
								Debug.LogError($"Load Light Asset: {_index.assetPath}, {_index.objName}, res file load from AB fail!");
								_onLoadDone?.Invoke();
								return;
							}

							// 调用完成回调
							_onLightGoLoaded(_index, temp);

							_onLoadDone?.Invoke();
						}
                    , null);
            }
#endif

        }


        /// <summary>
        /// 在灯光配置对象加载完成之后调用的回调处理，这里会创建一份场景对象
        /// 并将对象放置到统一管理容器中，作为模板使用
        /// </summary>
        /// <param name="_lightIndex"></param>
        /// <param name="_go"></param>
        protected void _onLightGoLoaded(GLightGoIndex _lightIndex, GameObject _go)
        {
			if(_go == null)
                return;

			//获取索引key
			long key = _getIndexKey(_lightIndex);

			//判断当前key是否已经存在
			if (_m_dicLightPrefabDic.ContainsKey(key))
				return;

            // 使用模板实例化一个新的
            GameObject go = UnityEngine.Object.Instantiate(_go);

            // 先放到缓存池的父对象下面
            if (go != null)
            {
                go.transform.SetParent(_m_gRoot.transform);
				//修改名字
				go.name = _lightIndex.objName;
				//设置为不可见
				ALUGUICommon.setGameObjEnable(go, false);

				//放入数据集
				_m_dicLightPrefabDic.Add(key, go);
            }
        }

		/// <summary>
		/// 返回使用index获得的数据索引key
		/// </summary>
		/// <param name="_index"></param>
		/// <returns></returns>
		protected long _getIndexKey(GLightGoIndex _index)
		{
			return ALCommon.mergeInt(_index.mainId, _index.subId);
		}
    }
}