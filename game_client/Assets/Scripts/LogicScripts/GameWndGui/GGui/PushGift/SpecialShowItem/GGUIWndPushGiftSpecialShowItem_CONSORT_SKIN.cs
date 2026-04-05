using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 推送礼包特殊展示物品-妃子皮肤
    /// </summary>
    public class GGUIWndPushGiftSpecialShowItem_CONSORT_SKIN : _AGGUIWndPushGiftSpecialShowItem<GGUIMonoPushGiftSpecialShowItem_CONSORT_SKIN>
    {
        private _IConsortSkinShowInfo _m_consortSkinShowInfo; // 妃子皮肤展示信息
        
        private GGUISubWndConsortSkinShow _m_wConsortSkinShow; // 妃子皮肤展示子窗口
        
        public GGUIWndPushGiftSpecialShowItem_CONSORT_SKIN(NPCommonAssetPathInfo _assetPathInfo, Transform _parent) : base(_assetPathInfo, _parent)
        {
        }

        protected override void _onWndInitDone()
        {
            base._onWndInitDone();
            
            if (wnd == null)
                return;
            
            // 构建妃子皮肤展示子窗口
            if (wnd.monoConsortSkinShow != null)
                _m_wConsortSkinShow = new GGUISubWndConsortSkinShow(wnd.monoConsortSkinShow);
            
            // 绑定预览按钮
            ALUGUICommon.combineBtnClick(wnd.btnPreview, _onClickPreviewBtn);
        }

        protected override void _onDiscard()
        {
            base._onDiscard();
            
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnPreview, _onClickPreviewBtn);
            }
            
            _m_wConsortSkinShow?.discard();
            _m_wConsortSkinShow = null;
            
            _m_consortSkinShowInfo = null;
        }

        protected override void _onShowWnd()
        {
            base._onShowWnd();
        }

        protected override void _onHideWnd()
        {
            base._onHideWnd();
            
            _m_wConsortSkinShow?.hideWnd();
        }

        protected override void _onReset()
        {
            base._onReset();
            
            _m_wConsortSkinShow?.resetWnd();
        }

        protected override void _onSetData()
        {
            if (_m_iCommonCostItem == null || _m_iCommonCostItem.getItemType() != ENPItemType.CONSORT_SKIN)
            {
                Debug.LogError($"GGUIMonoPushGiftSpecialShowItem_CONSORT_SKIN是专门用于展示 ENPItemType.CONSORT_SKIN 类型的窗口, 但是setData时传入的展示道具为:{_m_iCommonCostItem}");
                return;
            }
            
            _m_consortSkinShowInfo = ConsortUtil.getConsortSkinShowInfo(_m_iCommonCostItem.subId);
        }

        protected override void _onRefreshWnd()
        {
            if (wnd == null || _m_consortSkinShowInfo == null)
                return;
            
            // 刷新妃子皮肤展示子窗口
            if (_m_wConsortSkinShow != null)
            {
                _m_wConsortSkinShow.showWnd();
                _m_wConsortSkinShow.setData(_m_consortSkinShowInfo);
            }
        }
        
        /// <summary>
        /// 点击预览按钮
        /// </summary>
        private void _onClickPreviewBtn(GameObject _go)
        {
            if (_m_consortSkinShowInfo == null || _m_consortSkinShowInfo.skinRefObj == null)
                return;
            
            // 打开妃子皮肤窗口
            QueueMgr.instance.AddNode(new GNodeConsortSkinMain(_m_consortSkinShowInfo.skinRefObj.consort_id, _m_consortSkinShowInfo.skinId));
        }
    }
}