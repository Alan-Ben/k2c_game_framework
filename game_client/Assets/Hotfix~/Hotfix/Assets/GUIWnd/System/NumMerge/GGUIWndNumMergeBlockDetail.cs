using ALPackage;
using GOE;
using JetBrains.Annotations;
using UnityEngine;

namespace Hotfix
{
    /// <summary>
    /// 2048 棋子详情界面
    /// </summary>
    public class GGUIWndNumMergeBlockDetail : _AHotfixBaseWnd<GGUIMonoNumMergeBlockDetail>
    {
        [NotNull] public static GGUIWndNumMergeBlockDetail instance { get { return _g_instance ??= new GGUIWndNumMergeBlockDetail(); } }
        private static GGUIWndNumMergeBlockDetail _g_instance;


        //棋子配置信息
        private NumMergeBlockRefObj _m_blockRefObj;
        //棋子图标
        private NPGGuiWndTexture _m_wBlockIcon;
        //奖券道具
        private GGUIWndCommonSimpleItem _m_wTicketReward;


        private GGUIWndNumMergeBlockDetail()
            : base(EALUIWndLayer.ADDITION)
        {
        }


        protected override string _monoAssetPath { get { return UIResPathAssistant.getAssetPath(8603); } }
        protected override string _monoObjName { get { return UIResPathAssistant.getObjName(8603); } }



        protected override void _onShowWnd()
        {
            _m_wBlockIcon?.showWnd();
            _m_wTicketReward?.showWnd();

            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_wBlockIcon?.hideWnd();
            _m_wTicketReward?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_wBlockIcon?.discardTexture();
            _m_wTicketReward?.resetWnd();
        }
        protected override void _onDiscard()
        {
            if (hotfixWnd == null)
                return;

            _m_wBlockIcon?.discard();
            _m_wBlockIcon = null;

            _m_wTicketReward?.discard();
            _m_wTicketReward = null;

            ALUGUICommon.uncombineBtnClick(hotfixWnd.btnClose, _onClickClose);
        }
        protected override void _onWndInitDoneHotfix()
        {
            if (hotfixWnd == null)
                return;

            //初始化棋子图标
            if (hotfixWnd.imgIcon != null)
                _m_wBlockIcon = new NPGGuiWndTexture(hotfixWnd.imgIcon);

            //初始化奖券道具
            if (hotfixWnd.monoTicketReward != null)
                _m_wTicketReward = new GGUIWndCommonSimpleItem(hotfixWnd.monoTicketReward);

            ALUGUICommon.combineBtnClick(hotfixWnd.btnClose, _onClickClose);
        }


        //刷新窗口
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

            //设置棋子图标
            _m_wBlockIcon?.setTexture(_m_blockRefObj.icon);
            //设置棋子名称
            ALUGUICommon.setLabelTxt(hotfixWnd.txtName, TextTranslate.instance.getLanguage(_m_blockRefObj.name));
            //设置棋子描述
            ALUGUICommon.setLabelTxt(hotfixWnd.txtDesc, TextTranslate.instance.getLanguage(_m_blockRefObj.desc));
            //设置合成得分
            ALUGUICommon.setLabelTxt(hotfixWnd.txtScore, _m_blockRefObj.merge_gain_score);
            //设置合成获得奖券
            _m_wTicketReward?.setItem(new NPCommonCostItem(HotfixRefdataCoreMgr.instance.numMergeOtherRefObj.num_merge_ticket_item, _m_blockRefObj.merge_gain_ticket));
        }


        //点击关闭按钮
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(HotfixUINodeTagConst.NUMMERGE_BLOCK_DETAIL);
        }
    }
}
