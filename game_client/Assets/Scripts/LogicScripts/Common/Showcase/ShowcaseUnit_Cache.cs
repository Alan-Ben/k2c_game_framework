using ALPackage;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GOE
{
    /// <summary>
    /// Showcase加载的全部单位资源的缓存池
    /// </summary>
    public class ShowcaseUnit_Cache : _AALCacheController<GameObject, GameObject>
    {
        private BasicResIndexInfo _m_showcaseIndex;
        private GameObject _m_gRootGo;

        //这里的父节点由外部传入，就不会造成要生成许多同名root的消耗问题了
        public ShowcaseUnit_Cache(BasicResIndexInfo _index, GameObject _parent, int _minCacheCount = 1, int _maxCacheCount = 3)
            : base(_minCacheCount, _maxCacheCount)
        {
            _m_showcaseIndex = _index;
            _m_gRootGo = _parent;
        }
        public BasicResIndexInfo GoIndex { get { return _m_showcaseIndex; } }


        //警告信息文字
        protected override string _warningTxt { get { return "ShowcaseUnit<" + _m_showcaseIndex.mainId + "_" + _m_showcaseIndex.subId + ">"; } }

        protected override GameObject _createItem(GameObject _template)
        {
            if(_template == null || null == _m_gRootGo)
                return null;

            GameObject go = Object.Instantiate(_template) as GameObject;

            if(null != go.transform)
                go.transform.SetParent(_m_gRootGo.transform);

            return go;
        }

        protected override void _discardItem(GameObject _item)
        {
            if(null == _item || null == _item.transform)
                return;

            ALUnityCommon.releaseGameObj(_item);
        }

        protected override void _onInit(GameObject _template)
        {
            
        }

        protected override void _resetItem(GameObject _item)
        {
            if(null == _item || null == _item.transform || null == _m_gRootGo)
                return;

            //先Disable，再修改parent
            ALUGUICommon.setGameObjDisable(_item.gameObject);
            
            //设置为子节点
            _item.transform.SetParent(_m_gRootGo.transform);
            _item.transform.localPosition = Vector3.zero;
        }

        protected override void _discard()
        {
            
        }
    }
}