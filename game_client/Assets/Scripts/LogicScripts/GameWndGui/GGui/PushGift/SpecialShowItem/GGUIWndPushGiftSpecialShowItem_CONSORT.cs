using System.Collections.Generic;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public class GGUIWndPushGiftSpecialShowItem_CONSORT : _AGGUIWndPushGiftSpecialShowItem<GGUIMonoPushGiftSpecialShowItem_CONSORT>
    {
        private ConsortInfo_UnlockNotAutoRefresh _m_iConsortInfo;
        private List<GGottenConsortInfo> _m_lTmpGottenConsortInfoList;
     
        private GGUISubWndConsortDetailInfo _m_wConsortDetailInfo;
        
        public GGUIWndPushGiftSpecialShowItem_CONSORT(NPCommonAssetPathInfo _assetPathInfo, Transform _parent) : base(_assetPathInfo, _parent)
        {
        }

        protected override void _onWndInitDone()
        {
            base._onWndInitDone();
            
            if(wnd == null)
                return;
            
            if (wnd.monoConsortDetailInfo != null)
                _m_wConsortDetailInfo = new GGUISubWndConsortDetailInfo(wnd.monoConsortDetailInfo);
            
            ALUGUICommon.combineBtnClick(wnd.btnPreview, _onClickPreviewBtn);
        }

        protected override void _onDiscard()
        {
            base._onDiscard();

            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnPreview, _onClickPreviewBtn);
            }
            
            _m_wConsortDetailInfo?.discard();
            _m_wConsortDetailInfo = null;
            
            _m_lTmpGottenConsortInfoList?.Clear();
            _m_lTmpGottenConsortInfoList = null;

            _m_iConsortInfo = null;
        }

        protected override void _onShowWnd()
        {
            base._onShowWnd();
        }

        protected override void _onHideWnd()
        {
            base._onHideWnd();
            
            _m_wConsortDetailInfo?.hideWnd();
        }

        protected override void _onReset()
        {
            base._onReset();
            
            _m_wConsortDetailInfo?.resetWnd();
        }

        protected override void _onSetData()
        {
            if (_m_iCommonCostItem == null || _m_iCommonCostItem.getItemType() != ENPItemType.CONSORT)
            {
                Debug.LogError($"GGUIMonoPushGiftSpecialShowItem_CONSORT_SKIN是专门用于展示 ENPItemType.CONSORT 类型的窗口, 但是setData时传入的展示道具为:{_m_iCommonCostItem}");
                return;
            }

            _m_iConsortInfo = new ConsortInfo_UnlockNotAutoRefresh(_m_iCommonCostItem.subId);
        }

        protected override void _onRefreshWnd()
        {
            if(wnd == null || _m_iConsortInfo == null)
                return;

            // 每次刷新时更新一次已获取妃子数据
            _m_iConsortInfo.updateGGottenConsortInfo();
            
            if (_m_wConsortDetailInfo != null)
            {
                _m_wConsortDetailInfo.showWnd();
                _m_wConsortDetailInfo.setData(_m_iConsortInfo);
            }
        }
        
        /// <summary>
        /// 点击妃子预览按钮
        /// </summary>
        private void _onClickPreviewBtn(GameObject _go)
        {
            if (_m_iConsortInfo == null)
                return;
            
            // 更新已获取妃子数据
            _m_iConsortInfo.updateGGottenConsortInfo();
            
            if (_m_iConsortInfo.gottenConsortInfo != null)
            {
                if(_m_lTmpGottenConsortInfoList == null)
                    _m_lTmpGottenConsortInfoList = new List<GGottenConsortInfo>();
                _m_lTmpGottenConsortInfoList.Clear();
                
                _m_lTmpGottenConsortInfoList.Add(_m_iConsortInfo.gottenConsortInfo);
                GNodeUnLockConsortDetail.addConsortNode(_m_lTmpGottenConsortInfoList, 0);
            }
            else if(_m_iConsortInfo.consortRefShowInfo != null)
            {
                QueueMgr.instance.AddNode(new GNodeLockConsortDetail(_m_iConsortInfo.consortRefShowInfo));
            }
        }
    }
}