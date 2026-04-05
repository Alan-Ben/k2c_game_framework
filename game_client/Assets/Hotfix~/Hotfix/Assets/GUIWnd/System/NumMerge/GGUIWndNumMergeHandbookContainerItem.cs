using ALPackage;
using GOE;
using UnityEngine;

namespace Hotfix
{
    /// <summary>
    /// 2048 图鉴item
    /// </summary>
    public class GGUIWndNumMergeHandbookContainerItem : _AHotfixBaseSubWnd<GGUIMonoNumMergeHandbookContainerItem>
    {
        //棋子配置信息
        private NumMergeBlockRefObj _m_blockRefObj;
        //棋子图标
        private NPGGuiWndTexture _m_wBlockIcon;
        

        public GGUIWndNumMergeHandbookContainerItem(GGUIHotfixCommonMono _wnd) : base(_wnd)
        {
            initWnd();
        }
        

        protected override void _onShowWnd()
        {
            _m_wBlockIcon?.showWnd();
            
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_wBlockIcon?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_wBlockIcon?.discardTexture();
        }
        protected override void _onDiscard()
        {
            if (hotfixWnd == null)
                return;

            _m_wBlockIcon?.discard();
            _m_wBlockIcon = null;

            ALUGUICommon.uncombineBtnClick(hotfixWnd.btnDetail, _onClickDetail);
        }
        protected override void _onWndInitDoneHotfix()
        {
            if (hotfixWnd == null)
                return;

            if (hotfixWnd.icon != null)
                _m_wBlockIcon = new NPGGuiWndTexture(hotfixWnd.icon);

            ALUGUICommon.combineBtnClick(hotfixWnd.btnDetail, _onClickDetail);
        }

       
        public void refreshWnd(NumMergeBlockRefObj _blockRefObj)
        {
            _m_blockRefObj = _blockRefObj;
            refreshWnd();
        }
        //刷新界面
        public void refreshWnd()
        {
            if (hotfixWnd == null || !_m_bIsShow || _m_blockRefObj == null)
                return;

            _m_wBlockIcon?.setTexture(_m_blockRefObj.icon);
            //设置棋子名称
            ALUGUICommon.setLabelTxt(hotfixWnd.name, TextTranslate.instance.getLanguage(_m_blockRefObj.name));
        }

        //点击详情按钮
        private void _onClickDetail(GameObject _go)
        {
            if (_m_blockRefObj == null)
                return;

            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndNumMergeBlockDetail.instance, () =>
            {
                GGUIWndNumMergeBlockDetail.instance.showWnd();
                GGUIWndNumMergeBlockDetail.instance.refreshWnd(_m_blockRefObj);
            }, EUIQueueStageType.MAIN, HotfixUINodeTagConst.NUMMERGE_BLOCK_DETAIL, true, false);
        }
    }
}
