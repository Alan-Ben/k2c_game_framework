using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 通用视频播放子窗口
    /// </summary>
    public class GGUISubWndCommonVideo : _ATALBasicUISubWnd<GGUIMonoCommonVideo>
    {
        private NPGSubPrefab _m_spSubPrefab;//加载出的对象

        public GGUISubWndCommonVideo(GGUIMonoCommonVideo _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;
            
        }
        
        protected override void _onDiscard()
        {
            _m_spSubPrefab?.discard();
            _m_spSubPrefab = null;
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_spSubPrefab?.hide();
        }

        protected override void _onReset()
        {
            _m_spSubPrefab?.hide();
        }
        
        public void playVideo(NPCommonAssetPathInfo _assetPathInfo)
        {
            if(wnd == null || _assetPathInfo == null || !_assetPathInfo.enable)
                return;
            
            _m_spSubPrefab?.discard();

            _m_spSubPrefab = new NPGSubPrefab(_assetPathInfo, wnd.videoParent);
            _m_spSubPrefab.load(_m_spSubPrefab.show);
        }
    }
}