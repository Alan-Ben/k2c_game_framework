using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIWndConsortCGDetail : _ATALBasicUIWnd<GGUIMonoConsortCGDetail>
    {
        private static GGUIWndConsortCGDetail _g_instance;
        public static GGUIWndConsortCGDetail instance { get { return _g_instance ??= new GGUIWndConsortCGDetail(); } }
        
        private ConsortCGRefObj _m_rConsortCGRefObj;//妃子CG表数据

        private NPGGuiWndTexture _m_wConsortCGImg;//妃子CG图片
        private GGUIWndSimpleVideo _m_wCgVideo;//妃子CG视频
        
        // 视频监控序列号，防止无效回调
        private long _m_lVideoMonitorSerialize;
        
        public GGUIWndConsortCGDetail() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoConsortCGDetail.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoConsortCGDetail.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        protected override void _onShowWnd()
        {
            _m_wCgVideo?.showWnd();
        }

        protected override void _onHideWnd()
        {
            _m_lVideoMonitorSerialize = ALSerializeOpMgr.next();
            
            _m_wConsortCGImg?.discardTexture();
            _m_wCgVideo?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wConsortCGImg?.discardTexture();
            _m_wCgVideo?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wConsortCGImg?.discard();
            _m_wConsortCGImg = null;

            _m_wCgVideo?.discard();
            _m_wCgVideo = null;
            
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onCloseBtnClick);
            }
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.cgImage != null)
                _m_wConsortCGImg = new NPGGuiWndTexture(wnd.cgImage);

            if (wnd.monoCgVideo != null)
                _m_wCgVideo = new GGUIWndSimpleVideo(wnd.monoCgVideo);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onCloseBtnClick);
        }

        public void setData(ConsortCGRefObj _consortCgRef)
        {
            _m_rConsortCGRefObj = _consortCgRef;
            
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if(wnd == null || _m_rConsortCGRefObj == null)
                return;

            // 显示图片
            if (_m_wConsortCGImg != null && _m_rConsortCGRefObj.cg_icon != null && _m_rConsortCGRefObj.cg_icon.enable())
            {
                _m_wConsortCGImg.showWnd();
                _m_wConsortCGImg.setTexture(_m_rConsortCGRefObj.cg_icon);
            }
            
            _showVideo();
        }


        /// <summary>
        /// 播放CG视频
        /// </summary>
        private void _showVideo()
        {
            if (wnd == null)
                return;

            // 更新序列号
            long curSerialize = _m_lVideoMonitorSerialize = ALSerializeOpMgr.next();;
            
            if (_m_wCgVideo == null || _m_rConsortCGRefObj == null || _m_rConsortCGRefObj.cg_video_index == null 
                || !_m_rConsortCGRefObj.cg_video_index.isValid())
            {
                _sampleAnimation(wnd.wndAnimation, wnd.onVideoEndAniName, 1f);
                return;
            }

            // 显示并播放视频
            _m_wCgVideo.showWnd();
            _m_wCgVideo.setVideoClip(_m_rConsortCGRefObj.cg_video_index);
            _m_wCgVideo.playVideo(() =>
            {
                // 视频开始播放时的回调
                if (curSerialize != _m_lVideoMonitorSerialize || wnd == null)
                    return;
                
                _playAnimation(wnd.wndAnimation, wnd.onVideoStartAniName);
            });

            // 监控视频播放结束
            _m_wCgVideo.monitorPlay(() =>
            {
                // 视频播放结束时的回调
                if (curSerialize != _m_lVideoMonitorSerialize || wnd == null)
                    return;
                
                _playAnimation(wnd.wndAnimation, wnd.onVideoEndAniName);
            });
        }

        /// <summary>
        /// 播放窗口动画
        /// </summary>
        /// <param name="_animationName">动画名称</param>
        private void _playAnimation(Animation _ani, string _animationName)
        {
            if (wnd == null || _ani == null || string.IsNullOrEmpty(_animationName))
                return;
            
            _ani.ForcePlay(_animationName);
        }

        private void _sampleAnimation(Animation _ani, string _animationName, float _normalizeTime)
        {
            if (wnd == null || _ani == null || string.IsNullOrEmpty(_animationName))
                return;
            
            _ani.Sample(_animationName, _normalizeTime);
        }
        
        /// <summary>
        /// 点击关闭按钮
        /// </summary>
        private void _onCloseBtnClick(GameObject _gameObject)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_CONSORT_CG_DETAIL);
        }
    }
}