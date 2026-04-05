using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 通用含特效的品质GO
    /// </summary>
    public class GGUISubWndQualityShowGo : _ANPGGUIBasicSubWnd<GGUISubMonoQualityShowGo>
    {
        //配置数据
        private NPQualityExtRefObj _m_rQualityExtRef;
        
        //加载出来的品质GO序号
        private NPGGoIndex _m_qualityGoIndex;
        //加载出来的品质GO
        private GameObject _m_qualityGo;
        
        public GGUISubWndQualityShowGo(GGUISubMonoQualityShowGo _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
        }
        
        protected override void _onDiscard()
        {
            _pushBackQualityGo();
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _pushBackQualityGo();
        }

        protected override void _onReset()
        {
            _pushBackQualityGo();
        }
        
        public void setData(ENPItemType _itemType, long _subId)
        {
            _m_rQualityExtRef = GCommon.getQualityExtRefObj(_itemType, _subId);

            _pushBackQualityGo();
            _popQualityGo();
        }

        public void setData(EQuality _quality)
        {
            _m_rQualityExtRef = GRefdataCoreMgr.instance.qualityExtRefCore.getRef((int)_quality);

            _pushBackQualityGo();
            _popQualityGo();
        }
        
        public void setData(NPQualityExtRefObj _qualityExtRef)
        {
            _m_rQualityExtRef = _qualityExtRef;

            _pushBackQualityGo();
            _popQualityGo();
        }
        
        //加载品质GO
        private void _popQualityGo()
        {
            if (wnd == null || wnd.goQualityParent == null || _m_rQualityExtRef == null)
                return;

            if (_m_rQualityExtRef != null && _m_rQualityExtRef.quality_go_index != null)
            {
                _m_qualityGoIndex = _m_rQualityExtRef.quality_go_index;
                GGoIndexCacheMgr.instance.popItem(_m_qualityGoIndex, _go =>
                {
                    if (_go == null || wnd == null || wnd.goQualityParent == null)
                        return;

                    _go.transform.SetParent(wnd.goQualityParent);
                    _go.transform.localPosition = Vector3.zero;
                    _go.transform.localScale = Vector3.one;
                    _m_qualityGo = _go;
                });
            }
        }

        //回收品质GO
        private void _pushBackQualityGo()
        {
            if (_m_qualityGoIndex != null && _m_qualityGo != null)
                GGoIndexCacheMgr.instance.pushbackItem(_m_qualityGoIndex, _m_qualityGo);
            _m_qualityGoIndex = null;
            _m_qualityGo = null;
        }
    }
}