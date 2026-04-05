using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 妃子解锁详情页面互动page显示使用参数
    /// </summary>
    public interface _IGGUIWndUnlockConsortDetailInteractionPageParam
    {
        /// <summary>
        /// 显示页签
        /// </summary>
        EUnlockConsortDetailWndInteractionPageTabType interactionPageTabType { get; set; }
        
        /// <summary>
        /// 互动赠送礼物页面选中的item下标
        /// </summary>
        int interactionSendGiftPageSelectIndex { get; set; }
    }
    
    /// <summary>
    /// 妃子解锁详情页面互动page
    /// </summary>
    public class GGUIWndUnlockConsortDetailInteractionPage : _AGGUIWndUnLockConsortDetailTabPage<GGUIMonoUnlockConsortDetailInteractionPage>
    {
        private _IGGUIWndUnlockConsortDetailInteractionPageParam _m_param;
        
        private GGUIWndUnlockConsortDetailInteractionPageTabWnd _m_wTabWnd;// 页签子窗口
        
        private GGUIWndConsortSkinBtnItem _m_wConsortSkinItemBtn;// 伙伴皮肤按钮item
        
        public GGUIWndUnlockConsortDetailInteractionPage(_IGGUIWndUnlockConsortDetailInteractionPageParam _param, NPCommonAssetPathInfo _commonAssetPathInfo, Transform _parent) : base(_commonAssetPathInfo, _parent)
        {
            _m_param = _param;
        }

        /// <summary>
        /// 本窗口对应的页签类型
        /// </summary>
        public override EUnLockConsortDetailWndTabType tabPageType { get { return EUnLockConsortDetailWndTabType.INTERACTION; } }

        protected override void _onWndInitDoneSub()
        {
            if(wnd == null)
                return;

            if (wnd.monoTab != null)
                _m_wTabWnd = new GGUIWndUnlockConsortDetailInteractionPageTabWnd(_m_param, wnd.monoTab);

            if (wnd.monoConsortSkinItemBtn != null)
            {
                _m_wConsortSkinItemBtn = new GGUIWndConsortSkinBtnItem(wnd.monoConsortSkinItemBtn);
                _m_wConsortSkinItemBtn.onClick += _onSKinItemBtnClick;
            }
         
            ALUGUICommon.combineBtnClick(wnd.btnStory, _onStoryBtnClick);
        }

        protected override void _onDiscardSub()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnStory, _onStoryBtnClick);
            }
            
            _m_wTabWnd?.discard();
            _m_wTabWnd = null;

            if (_m_wConsortSkinItemBtn != null)
            {
                _m_wConsortSkinItemBtn.onClick -= _onSKinItemBtnClick;
                _m_wConsortSkinItemBtn.discard();
                _m_wConsortSkinItemBtn = null;
            }
        }

        protected override void _onShowWndSub()
        {
        }

        protected override void _onHideWndSub()
        {
            _m_wTabWnd?.hideWnd();
            _m_wConsortSkinItemBtn?.hideWnd();
        }

        protected override void _onResetSub()
        {
            _m_wTabWnd?.resetWnd();
            _m_wConsortSkinItemBtn?.resetWnd();
        }

        protected override void _setDataSub()
        {
        }
        
        protected override void _refreshWndSub()
        {
            if (_m_wTabWnd != null)
            {
                _m_wTabWnd.showWnd();
                _m_wTabWnd.setData(_m_iConsortShowInfo);       
            }

            if (_m_wConsortSkinItemBtn != null)
            {
                if (_m_iConsortShowInfo != null && _m_iConsortShowInfo.consortRefObj != null &&
                    _m_iConsortShowInfo.consortRefObj.skinList != null &&
                    _m_iConsortShowInfo.consortRefObj.skinList.Count > 1)
                {
                    // 有多皮肤时才显示
                    _m_wConsortSkinItemBtn.showWnd();
                    _m_wConsortSkinItemBtn.setInfo(_m_iConsortShowInfo);    
                }
                else
                {
                    _m_wConsortSkinItemBtn.hideWnd();
                }
            }
        }

        public override void onlyShowActor(bool _show)
        {
            base.onlyShowActor(_show);
            
            _m_wTabWnd?.onlyShowActor(_show);
        }

        private void _onSKinItemBtnClick(GGUIWndConsortSkinBtnItem _item)
        {
            if(_m_iConsortShowInfo == null)
                return;
            
            QueueMgr.instance.AddNode(new GNodeConsortSkinMain(_m_iConsortShowInfo.consortId));
        }

        /// <summary>
        /// 设置选中页签
        /// </summary>
        /// <param name="_interactionPageTab"></param>
        public void setSelectTab(EUnlockConsortDetailWndInteractionPageTabType _interactionPageTab)
        {
            _m_wTabWnd?.selectTab(_interactionPageTab, true);
        }
        
        private void _onStoryBtnClick(GameObject _go)
        {
            if (_m_iConsortShowInfo == null)
                return;
         
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndUnlockConsortDetailInteractionStory.instance, () =>
            {
                GGUIWndUnlockConsortDetailInteractionStory.instance.showWnd();
                GGUIWndUnlockConsortDetailInteractionStory.instance.setData(_m_iConsortShowInfo);
            }, UINodeTagConst.C_UNLOCK_CONSORT_DETAIL_STORY_WND);
        }
    }
}