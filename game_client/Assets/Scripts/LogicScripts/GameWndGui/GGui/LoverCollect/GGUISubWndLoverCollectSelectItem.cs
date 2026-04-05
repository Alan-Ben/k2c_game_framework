using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public class GGUISubWndLoverCollectSelectItem : _ATALBasicUISubWnd<GGUIMonoLoverCollectSelectItem>
    {
        private long _m_loverId;

        public GGUISubWndLoverCollectSelectItem(GGUIMonoLoverCollectSelectItem _wnd)
            : base(_wnd)
        {
            initWnd();
        }


        protected override void _onShowWnd()
        {
        }
        protected override void _onHideWnd()
        {
        }
        protected override void _onReset()
        {
        }
        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnDetail, _onBtnDetailClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnSelect, _onBtnSelectClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            ALUGUICommon.combineBtnClick(wnd.btnDetail, _onBtnDetailClick);
            ALUGUICommon.combineBtnClick(wnd.btnSelect, _onBtnSelectClick);
        }


        public void setInfo(long _loverId)
        {
            _m_loverId = _loverId;
            refreshWnd();
        }

        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtLoverName, UniformItemSqliteAssistant.getTransName(ENPItemType.CONSORT, _m_loverId));
        }


        private void _onBtnDetailClick(GameObject _obj)
        {
            GGottenConsortInfo consortInfo = NPPlayer.instance.consortComp.getConsortInfo(_m_loverId);
            if (consortInfo != null)
                GNodeUnLockConsortDetail.addConsortNode(NPPlayer.instance.consortComp.getConsortList(), consortInfo.consortId);
            else
                QueueMgr.instance.AddNode(new GNodeLockConsortDetail(new ConsortRefShowInfo(_m_loverId)));
        }

        private void _onBtnSelectClick(GameObject _obj)
        {
            NPPlayer.instance.loverCollectComp.reqSetLoverTarget(_m_loverId, (_isSuc) =>
            {
                if (!_isSuc)
                    return;

                QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_LOVER_COLLECT_SELECT);
                GCommon.playPerformGroup(GGUIWndLoverCollectSelect.instance.performGroupId, () =>
                {
                    QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndLoverCollectRescue.instance, GGUIWndLoverCollectRescue.instance.showWnd, UINodeTagConst.C_LOVER_COLLECT_RESCUE);
                });
            });
        }
    }
}
