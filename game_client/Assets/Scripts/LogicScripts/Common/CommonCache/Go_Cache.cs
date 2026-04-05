using ALPackage;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GOE
{
    /// <summary>
    ///Go类型资源的缓存池
    /// </summary>
    public class Go_Cache : _AALCacheController<GameObject, GameObject>
    {
        private NPGGoIndex _m_goIndex;
        private GameObject _m_gRootGo;

        //这里的父节点由外部传入，就不会造成要生成许多同名root的消耗问题了
        public Go_Cache(NPGGoIndex _index, GameObject _parent, int _minCacheCount = 1, int _maxCacheCount = 5)
            : base(_minCacheCount, _maxCacheCount)
        {
            _m_goIndex = _index;
            _m_gRootGo = _parent;
        }
        public NPGGoIndex GoIndex { get { return _m_goIndex; } }


        //警告信息文字
        protected override string _warningTxt { get { return "Go<" + _m_goIndex.mainId + "_" + _m_goIndex.subId + ">"; } }

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
        { }

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
        { }
    }
}