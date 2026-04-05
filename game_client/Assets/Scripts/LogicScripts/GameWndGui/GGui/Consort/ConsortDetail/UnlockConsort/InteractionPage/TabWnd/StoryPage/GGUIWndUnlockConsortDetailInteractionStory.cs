using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIWndUnlockConsortDetailInteractionStory : _ANPGGUIBasicWnd<GGUIMonoUnlockConsortDetailInteractionStory>
    {
        private GGottenConsortInfo _m_consortInfo;
        
        private GGUIWndConsortStoryContainer _m_wndConsortStoryContainer;
        
        private static GGUIWndUnlockConsortDetailInteractionStory _g_instance = null;
        public static GGUIWndUnlockConsortDetailInteractionStory instance { get { return _g_instance ??= new GGUIWndUnlockConsortDetailInteractionStory(); } }
        
        public GGUIWndUnlockConsortDetailInteractionStory() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoUnlockConsortDetailInteractionStory.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoUnlockConsortDetailInteractionStory.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override bool isShowAniPlayOnlyOne { get { return true; } }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.storyContainer != null)
                _m_wndConsortStoryContainer = new GGUIWndConsortStoryContainer(wnd.storyContainer);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onCloseBtnClick);
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onCloseBtnClick);
            }
            
            if(_m_wndConsortStoryContainer != null)
                _m_wndConsortStoryContainer.discard();
            _m_wndConsortStoryContainer = null;
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wndConsortStoryContainer?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wndConsortStoryContainer?.resetWnd();
        }

        public void setData(GGottenConsortInfo _consortInfo)
        {
            _m_consortInfo = _consortInfo;

            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if (_m_wndConsortStoryContainer != null && _m_consortInfo != null && _m_consortInfo.consortRefObj != null)
            {
                _m_wndConsortStoryContainer.showWnd();
                _m_wndConsortStoryContainer.setData(_m_consortInfo.consortRefObj.getNeedShowConsortStoryRefObjDic(), _m_consortInfo,
                    () =>
                    {
                        _onCloseBtnClick(null);
                    });
            }
        }
        
        /// <summary>
        /// 点击关闭按钮
        /// </summary>
        private void _onCloseBtnClick(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_UNLOCK_CONSORT_DETAIL_STORY_WND);
        }
    }
}