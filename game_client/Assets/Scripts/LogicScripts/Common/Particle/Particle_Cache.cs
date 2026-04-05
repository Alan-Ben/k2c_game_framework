using ALPackage;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GOE
{
    /// <summary>
    ///Go类型资源的缓存池
    /// </summary>
    public class Particle_Cache : _AALCacheController<ParticleCacheItem, NPParticleMono>
    {
        private NPGParticleIndex _m_goIndex;
        private Transform _m_tParentTrans;
        private GameObject _m_gRootGo;

        //这里的父节点由外部传入，就不会造成要生成许多同名root的消耗问题了
        public Particle_Cache(NPGParticleIndex _index, GameObject _parent, int _minCacheCount = 1, int _maxCacheCount = 100)
            : base(_minCacheCount, _maxCacheCount)
        {
            _m_goIndex = _index;
            _m_gRootGo = new GameObject();
            _m_gRootGo.name = _m_goIndex.mainId + "_" + _m_goIndex.subId;
            _m_tParentTrans = _m_gRootGo.transform;
            if (_parent != null)
            {
                _m_gRootGo.transform.SetParent(_parent.transform);
                _m_gRootGo.transform.localPosition = Vector3.zero;
                _m_gRootGo.transform.localScale = Vector3.one;
            }
        }


        public Particle_Cache(NPGParticleIndex _index, Transform _parentTrans, int _minCacheCount = 1, int _maxCacheCount = 100)
            : base(_minCacheCount, _maxCacheCount)
        {
            _m_goIndex = _index;
            _m_tParentTrans = _parentTrans;
        }

        public NPGParticleIndex GoIndex { get { return _m_goIndex; } }


        //警告信息文字
        protected override string _warningTxt { get { return "Particle<" + _m_goIndex.mainId + "_" + _m_goIndex.subId + ">"; } }

        protected override ParticleCacheItem _createItem(NPParticleMono _template)
        {
            if (_template == null || null == _m_tParentTrans)
                return null;

            NPParticleMono mono = Object.Instantiate(_template) as NPParticleMono;

            if (null != mono.transform)
                mono.transform.SetParent(_m_tParentTrans.transform);

            return new ParticleCacheItem(mono);
        }

        protected override void _discardItem(ParticleCacheItem _item)
        {
            if (null == _item)
                return;

            NPParticleMono mono = _item.mono;
            //释放资源
            _item.discard();

            ALUnityCommon.releaseGameObj(mono);
        }

        protected override void _onInit(NPParticleMono _template)
        { }

        protected override void _resetItem(ParticleCacheItem _item)
        {
            if (null == _item)
                return;

            //重置状态
            _item.reset();

            //设置为子节点
            if(null != _item.mono && null != _item.mono.transform)
            {
                _item.mono.transform.SetParent(_m_tParentTrans.transform);
                _item.mono.transform.localPosition = Vector3.zero;
            }
        }

        protected override void _discard()
        {
            ALUnityCommon.releaseGameObj(_m_gRootGo);
            _m_gRootGo = null;
        }
    }
}
