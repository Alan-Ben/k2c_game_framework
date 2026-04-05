using ALPackage;
using GOE;
using JetBrains.Annotations;
using UnityEngine;

namespace Hotfix
{
    /// <summary>
    /// 2048 图鉴界面
    /// </summary>
    public class GGUIWndNumMergeHandbook : _AHotfixBaseWnd<GGUIMonoNumMergeHandbook>
    {
        [NotNull] public static GGUIWndNumMergeHandbook instance { get { return _g_instance ??= new GGUIWndNumMergeHandbook(); } }
        private static GGUIWndNumMergeHandbook _g_instance;
        

        //图鉴容器
        private GGUIWndNumMergeHandbookContainer _m_wHandbookContainer;
        

        private GGUIWndNumMergeHandbook() 
            : base(EALUIWndLayer.ADDITION)
        {
        }
        

        protected override string _monoAssetPath { get { return UIResPathAssistant.getAssetPath(8602); } }
        protected override string _monoObjName { get { return UIResPathAssistant.getObjName(8602); } }
        


        protected override void _onShowWnd()
        {
            _m_wHandbookContainer?.showWnd();
            
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_wHandbookContainer?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_wHandbookContainer?.resetWnd();
        }
        protected override void _onDiscard()
        {
            if (hotfixWnd == null)
                return;

            _m_wHandbookContainer?.discard();
            _m_wHandbookContainer = null;

            ALUGUICommon.uncombineBtnClick(hotfixWnd.btnClose, _onClickClose);
        }
        protected override void _onWndInitDoneHotfix()
        {
            if (hotfixWnd == null)
                return;

            //初始化图鉴容器
            if (hotfixWnd.monoItemContainer != null)
                _m_wHandbookContainer = new GGUIWndNumMergeHandbookContainer(hotfixWnd.monoItemContainer);

            ALUGUICommon.combineBtnClick(hotfixWnd.btnClose, _onClickClose);
        }
        

        //刷新窗口
        public void refreshWnd()
        {
            if (hotfixWnd == null || !_m_bIsShow)
                return;

            //显示图鉴容器
            _m_wHandbookContainer?.showItemList();
        }


        //点击关闭按钮
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(HotfixUINodeTagConst.NUMMERGE_HANDBOOK);
        }
    }
}
