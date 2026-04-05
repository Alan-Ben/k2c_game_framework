using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 妃子详细信息prefab窗口
    /// </summary>
    public class GGUIPrefabWndRecruitItemDetail_Consort : _AGGUIPrefabWndRecruitItemDetail<GGUIPrefabMonoRecruitItemDetail_Consort, RecruitConsortItemInfo>
    {
        private GGUISubWndConsortDetailInfo _m_wndConsortDetailInfo;
        private List<GGottenConsortInfo> _m_lGottenConsortInfoList;
        // 关联大臣图标容器
        private GGUIWndHeroConsortSimpleIconContainer _m_wRelationHeroIconContainer;
        
        public GGUIPrefabWndRecruitItemDetail_Consort(NPCommonAssetPathInfo _assetPath, Transform _parent) : base(_assetPath, _parent)
        {
        }

        protected override void _onWndInitDoneSub()
        {
            if(wnd == null)
                return;

            if (wnd.monoConsortDetailInfo != null)
                _m_wndConsortDetailInfo = new GGUISubWndConsortDetailInfo(wnd.monoConsortDetailInfo);

            if (wnd.relationHeroIconContainer != null)
                _m_wRelationHeroIconContainer = new GGUIWndHeroConsortSimpleIconContainer(wnd.relationHeroIconContainer);

            ALUGUICommon.combineBtnClick(wnd.btnMoreInfo, _onMoreInfoBtnClick);
        }

        protected override void _onDiscardSub()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnMoreInfo, _onMoreInfoBtnClick);
            }
            
            _m_wndConsortDetailInfo?.discard();
            _m_wndConsortDetailInfo = null;
            
            _m_wRelationHeroIconContainer?.discard();
            _m_wRelationHeroIconContainer = null;
        }

        protected override void _onShowWndSub()
        {
            WinMsg.RegisterMsgAct(WinMsgType.ON_RECRUIT_EXCHANGE_SUCC, _onRecruitExchangeSucc);
        }

        protected override void _onHideWndSub()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_RECRUIT_EXCHANGE_SUCC, _onRecruitExchangeSucc);

            _m_wndConsortDetailInfo?.hideWnd();
            _m_wRelationHeroIconContainer?.hideWnd();
            
            _m_lGottenConsortInfoList?.Clear();
        }

        protected override void _onResetSub()
        {
            _m_wndConsortDetailInfo?.resetWnd();
            _m_wRelationHeroIconContainer?.resetWnd();
        }

        protected override void _onSetData()
        {
        }

        protected override void _onRefreshWnd()
        {
            if(wnd == null || _m_iRecruitItemInfo == null)
                return;

            if (_m_wndConsortDetailInfo != null)
            {
                _m_wndConsortDetailInfo.showWnd();
                _m_wndConsortDetailInfo.setData(_m_iRecruitItemInfo.consortInfo);
            }
            
            // 刷新关联大臣列表
            GConsortRefObj consortRefObj = _m_iRecruitItemInfo.consortInfo?.consortRefObj;
            if (_m_wRelationHeroIconContainer != null && consortRefObj != null 
                && consortRefObj.relation_hero_id_list != null && consortRefObj.relation_hero_id_list.Count > 0)
            {
                _m_wRelationHeroIconContainer.showWnd();
                _m_wRelationHeroIconContainer.showItemList(consortRefObj.relation_hero_id_list, EHeroConsortSimpleIconShowType.HERO);
            }
            else
            {
                _m_wRelationHeroIconContainer?.hideWnd();
            }
        }
        
        /// <summary>
        /// 兑换成功消息
        /// </summary>
        private void _onRecruitExchangeSucc()
        {
            // 刷新窗口
            _refreshWnd();
        }
        
        /// <summary>
        /// 更多信息按钮点击
        /// </summary>
        private void _onMoreInfoBtnClick(GameObject _go)
        {
            if(_m_iRecruitItemInfo == null || _m_iRecruitItemInfo.consortInfo == null)
                return;
            
            if (_m_lGottenConsortInfoList == null)
                _m_lGottenConsortInfoList = new List<GGottenConsortInfo>();
            _m_lGottenConsortInfoList.Clear();

            if (_m_iRecruitItemInfo.consortInfo.gottenConsortInfo != null)
            {
                _m_lGottenConsortInfoList.Add(_m_iRecruitItemInfo.consortInfo.gottenConsortInfo);
                GNodeUnLockConsortDetail.addConsortNode(_m_lGottenConsortInfoList, 0);
            }
            else if(_m_iRecruitItemInfo.consortInfo.consortRefShowInfo != null)
            {
                QueueMgr.instance.AddNode(new GNodeLockConsortDetail(_m_iRecruitItemInfo.consortInfo.consortRefShowInfo));
            }
        }
    }
}