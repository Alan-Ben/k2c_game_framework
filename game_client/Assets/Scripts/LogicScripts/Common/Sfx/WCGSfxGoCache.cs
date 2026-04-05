using ALPackage;
using UnityEngine;

namespace GOE
{
    //3D单位模型  缓存池
    public class WCGSfxGoCache : _AALCacheController<NPSfxMono, NPSfxMono>
    {
        /** 对应资源主Id和子Id合并的索引  */
        private long _m_lSfxIndex;

        /** 缓存父节点对象 */
        private GameObject _m_goRootSfxGo;

        public WCGSfxGoCache(long _sfxIndex, Transform _parentTrans, int _max, int _min)
            : base(_min < 1 ? 1 : _min, _max < 30 ? 30 : _max)
        {
            _m_lSfxIndex = _sfxIndex;

            //构建本缓存的父节点
            _m_goRootSfxGo = new GameObject();
            _m_goRootSfxGo.name = "sfx_" + _sfxIndex;
            _m_goRootSfxGo.transform.SetParent(_parentTrans);
            _m_goRootSfxGo.transform.localScale = Vector3.one;
            _m_goRootSfxGo.transform.position = Vector3.zero;
        }

        //警告信息文字
        protected override string _warningTxt { get { return "sfx_" + (_m_lSfxIndex >> 32) + "_" + (_m_lSfxIndex & 0x00000000ffffffff); } }

        public long sfxIndex { get { return _m_lSfxIndex; } }

        protected override NPSfxMono _createItem(NPSfxMono _template)
        {
            if(_template == null)
                return null;

            NPSfxMono go = Object.Instantiate(_template) as NPSfxMono;

            return go;
        }

        protected override void _discardItem(NPSfxMono _go)
        {
            ALUnityCommon.releaseGameObj(_go);
        }

        protected override void _onInit(NPSfxMono _template)
        {
        }

        protected override void _resetItem(NPSfxMono _go)
        {
            if(null == _go)
                return;

            ALUGUICommon.setGameObjEnable(_go, false);
            if (_m_goRootSfxGo != null)
                _go.transform.SetParent(_m_goRootSfxGo.transform);

            //重置处理
            _go.resetSfx();
        }

        /**************
         * 释放所有资源
         **/
        public new void discard()
        {
            //调用基类处理
            base.discard();

            //删除根节点对象
            if(null != _m_goRootSfxGo)
                ALUnityCommon.releaseGameObj(_m_goRootSfxGo);
            _m_goRootSfxGo = null;
        }
        protected override void _discard()
        {
        }
    }
}
