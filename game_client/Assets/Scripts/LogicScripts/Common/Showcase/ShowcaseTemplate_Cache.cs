using ALPackage;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GOE
{
    /// <summary>
    /// ShowcaseTemplate类型资源的缓存池
    /// </summary>
    public class ShowcaseTemplate_Cache : _AALCacheController<NPShowcaseTemplateMono, NPShowcaseTemplateMono>
    {
        private NPGShowcaseIndex _m_showcaseIndex;
        private GameObject _m_gRootGo;

        //这里的父节点由外部传入，就不会造成要生成许多同名root的消耗问题了
        public ShowcaseTemplate_Cache(NPGShowcaseIndex _index, GameObject _parent, int _minCacheCount = 1, int _maxCacheCount = 3)
            : base(_minCacheCount, _maxCacheCount)
        {
            _m_showcaseIndex = _index;
            _m_gRootGo = _parent;
        }
        public NPGShowcaseIndex GoIndex { get { return _m_showcaseIndex; } }


        //警告信息文字
        protected override string _warningTxt { get { return "ShowcaseTemplate<" + _m_showcaseIndex.mainId + "_" + _m_showcaseIndex.subId + ">"; } }

        protected override NPShowcaseTemplateMono _createItem(NPShowcaseTemplateMono _template)
        {
            if(_template == null || null == _m_gRootGo)
                return null;

            NPShowcaseTemplateMono go = Object.Instantiate(_template) as NPShowcaseTemplateMono;

            if(null != go.transform)
                go.transform.SetParent(_m_gRootGo.transform);

            return go;
        }

        protected override void _discardItem(NPShowcaseTemplateMono _item)
        {
            if(null == _item || null == _item.transform)
                return;

            ALUnityCommon.releaseGameObj(_item);
        }

        protected override void _onInit(NPShowcaseTemplateMono _template)
        {
            
        }

        protected override void _resetItem(NPShowcaseTemplateMono _item)
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