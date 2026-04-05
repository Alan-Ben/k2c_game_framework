using System;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// Particle资源的缓存池管理器
    /// </summary>
    public class Particle_CacheMgr : _ACacheControllerMgr<ParticleCacheItem, NPParticleMono, Particle_Cache>
    {
        //最小缓存数
        private readonly int _m_iMinCacheCount;
        //最大缓存数
        private readonly int _m_iMaxCacheCount;

        public Particle_CacheMgr(string _rootFolderName, int _minCacheCount = 1, int _maxCacheCount = 5) : base(_rootFolderName)
        {
            this._m_iMinCacheCount = _minCacheCount;
            this._m_iMaxCacheCount = _maxCacheCount;
        }

        public Particle_CacheMgr(GameObject _cacheParent, int _minCacheCount = 1, int _maxCacheCount = 5) : base(_cacheParent)
        {
            this._m_iMinCacheCount = _minCacheCount;
            this._m_iMaxCacheCount = _maxCacheCount;
        }

        protected override void LoadResource(BasicResIndexInfo _index, Action<NPParticleMono> _onLoaded)
        {
            NPGParticleIndex ParticleIndex = _index as NPGParticleIndex;
            if (ParticleIndex == null)
            {
                Debug.LogError(_index + " is not a NPGParticleIndex");
                if (_onLoaded != null)
                {
                    _onLoaded(null);
                }
                return;
            }

            GParticleResCore.instance.loadObj(_index, (_assetHandle) =>
            {
                if (_assetHandle == null || _assetHandle.loadedInfo == null)
                {
#if UNITY_EDITOR
                    UnityEngine.Debug.LogError("NPGParticleIndex加载错误: " + _assetHandle.mainId + " - " + _assetHandle.subId);
#endif
                    if (_onLoaded != null)
                    {
                        _onLoaded(null);
                    }
                    return;
                }
                //获取资源对象
                GameObject assetGo = _assetHandle.loadedInfo.obj;
                if (null == assetGo)
                {
#if UNITY_EDITOR
                    UnityEngine.Debug.LogError("NPGParticleIndex加载错误: " + _assetHandle.mainId + " - " + _assetHandle.subId);
#endif
                    if (_onLoaded != null)
                    {
                        _onLoaded(null);
                    }
                    return;
                }

                //加载完成回调
                NPParticleMono particleMono = assetGo.GetComponent<NPParticleMono>();
                if (null == particleMono)
                {
                    UnityEngine.Debug.LogError("Particle: " + _index.mainId + " - " + _index.subId + " GetComponent<NPParticleMono> Error!");
                    //释放资源
                    _assetHandle.discard();
                    return;
                }

                if (_onLoaded != null)
                {
                    _onLoaded(particleMono);
                }
            });
        }

        /// <summary>
        /// 创建缓存池
        /// </summary>
        /// <param name="_index"></param>
        /// <param name="_asset"></param>
        /// <returns></returns>
        protected override Particle_Cache CreateCache(BasicResIndexInfo _index)
        {
            NPGParticleIndex particleIndex = _index as NPGParticleIndex;
            if (particleIndex == null)
            {
                Debug.LogError(_index + " is not a NPGParticleIndex");
                return null;
            }
            return new Particle_Cache(particleIndex, _m_RootGo, _m_iMinCacheCount, _m_iMaxCacheCount);
        }
    }
}