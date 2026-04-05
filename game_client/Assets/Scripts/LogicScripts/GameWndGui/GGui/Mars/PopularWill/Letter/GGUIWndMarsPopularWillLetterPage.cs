using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星-民意-信件 列表子窗口（PrefabSubWnd）
    /// 负责加载 GGUIMonoMarsPopularWillLetter 并驱动内部 Grid 刷新
    /// </summary>
    public class GGUIWndMarsPopularWillLetterPage : _ANPGGUIBasicLoadPrefabSubWnd<GGUIMonoMarsPopularWillLetterPage>
    {
        private NPCommonAssetPathInfo _m_pagePathInfo;
        
        private List<_IMarsPeopleWillLetter> _m_TmpletterInfoList = new List<_IMarsPeopleWillLetter>();
        
        private GGUIWndMarsPopularWillLetterItemGrid _m_letterGrid;

        public GGUIWndMarsPopularWillLetterPage(NPCommonAssetPathInfo _pagePathInfo, Transform _parent) : base(_parent)
        {
            _m_pagePathInfo = _pagePathInfo;
        }

        protected override string _monoAssetPath { get { return _m_pagePathInfo?.asset_path; } }
        protected override string _monoObjName  { get { return _m_pagePathInfo?.obj_name; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.letterItemGrid != null)
                _m_letterGrid = new GGUIWndMarsPopularWillLetterItemGrid(wnd.letterItemGrid);
        }

        protected override void _onShowWnd()
        {
            refreshWnd();
            
            WinMsg.RegisterMsg(WinMsgType.ON_MARS_LETTER_ADD, _onAddLetter);
            WinMsg.RegisterMsg(WinMsgType.ON_MARS_LETTER_DEL, _onDelLetter);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_MARS_LETTER_ADD, _onAddLetter);
            WinMsg.UnregisterMsg(WinMsgType.ON_MARS_LETTER_DEL, _onDelLetter);
            
            _m_letterGrid?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_letterGrid?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_letterGrid?.discard();
            _m_letterGrid = null;
            
            _m_TmpletterInfoList?.Clear();
            _m_TmpletterInfoList = null;
        }

        /// <summary>
        /// 
        /// </summary>
        public void refreshWnd()
        {
            // 在刷新窗口前，先更新信件的已处理状态
            NPPlayer.instance.marsComp.peopleSubComponent.updateLetterDealedState();
            
            if (wnd == null || !_m_bIsShow)
                return;

            _refreshLetterGrid(true);
        }
        
        private void _refreshLetterGrid(bool _needUpdateInfo)
        {
            if (wnd == null || !_m_bIsShow || _m_letterGrid == null)
                return;

            if (_needUpdateInfo)
            {
                if (_m_TmpletterInfoList == null)
                    _m_TmpletterInfoList = new List<_IMarsPeopleWillLetter>();
                _m_TmpletterInfoList.Clear();
                _m_TmpletterInfoList.AddRange(NPPlayer.instance.marsComp.peopleSubComponent.letterList);
                int letterCount = _m_TmpletterInfoList.Count;
                int letterShowMaxCount = GRefdataCoreMgr.instance.npGeneral.mars_letter_limit;
                if (letterCount > letterShowMaxCount)
                    _m_TmpletterInfoList.RemoveRange(letterShowMaxCount, letterCount - letterShowMaxCount);
            }
         
            _m_letterGrid.showWnd();
            _m_letterGrid.setData(_m_TmpletterInfoList);
        }

        #region 窗口消息处理

        /// <summary>
        /// 新增信件
        /// </summary>
        /// <param name="_objs"></param>
        private void _onAddLetter(params object[] _objs)
        {
            if(_objs == null || _objs.Length < 1 || !(_objs[0] is _IMarsPeopleWillLetter letter))
                return;
            
            if(_m_TmpletterInfoList == null)
                _m_TmpletterInfoList = new List<_IMarsPeopleWillLetter>();
            _m_TmpletterInfoList.Insert(0, letter);
            
            _refreshLetterGrid(false);
        }
        
        /// <summary>
        /// 移除信件
        /// </summary>
        /// <param name="_objs"></param>
        private void _onDelLetter(params object[] _objs)
        {
            if(_objs == null || _objs.Length < 1 || !(_objs[0] is long _letterInstanceId) || _m_TmpletterInfoList == null)
                return;
            
            _m_TmpletterInfoList.RemoveAll(letterInfo => letterInfo != null && letterInfo.instanceId == _letterInstanceId);
            _refreshLetterGrid(false);
        }

        #endregion
    }
}
