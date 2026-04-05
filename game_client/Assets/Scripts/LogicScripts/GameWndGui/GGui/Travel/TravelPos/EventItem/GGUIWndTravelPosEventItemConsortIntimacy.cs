using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 游历地点事件item - 妃子亲密度类型Wnd
    /// </summary>
    public class GGUIWndTravelPosEventItemConsortIntimacy : _AGGUIWndTravelPosEventItem<GGUIMonoTravelPosEventItemConsortIntimacy>
    {
        private GGUIWndConsortCardItem _m_wConsortCardItem; // 妃子卡牌信息展示item
        
        private TravelEventConsortIntimacyRefObj _m_rConsortIntimacyRefObj; // 亲密度事件配表
        private _IConsortShowInfo _m_iConsortShowInfo; // 妃子展示信息接口

        public GGUIWndTravelPosEventItemConsortIntimacy(GGUIMonoTravelPosEventItemConsortIntimacy _mono) : base(_mono)
        {
        }

        protected override void _onWndInitDoneSub()
        {
            if (wnd == null)
                return;

            // 绑定妃子详情按钮
            if (wnd.btnConsortInfo != null)
                ALUGUICommon.combineBtnClick(wnd.btnConsortInfo, _onBtnConsortInfoClick);
            
            if(wnd.monoConsortCard != null)
                _m_wConsortCardItem = new GGUIWndConsortCardItem(wnd.monoConsortCard, null);
        }

        protected override void _onDiscardSub()
        {
            // 解绑妃子详情按钮
            if (wnd != null && wnd.btnConsortInfo != null)
                ALUGUICommon.uncombineBtnClick(wnd.btnConsortInfo, _onBtnConsortInfoClick);

            if(_m_wConsortCardItem != null)
                _m_wConsortCardItem.discard();
            
            _m_rConsortIntimacyRefObj = null;
            _m_iConsortShowInfo = null;
        }

        protected override void _onShowWndSub()
        {
        }

        protected override void _onHideWndSub()
        {
            _m_wConsortCardItem?.hideWnd();
        }

        protected override void _onResetSub()
        {
            _m_wConsortCardItem?.resetWnd();
        }

        protected override void _onSetData(TravelEventRefObj _eventRefObj)
        {
            // 获取亲密度事件配表
            _m_rConsortIntimacyRefObj = _eventRefObj == null
                ? null
                : GRefdataCoreMgr.instance.travelEventConsortIntimacyRefCore.getRef(_eventRefObj.event_id);

            long consortId = _m_rConsortIntimacyRefObj?.consort_id ?? 0;
            _m_iConsortShowInfo = NPPlayer.instance.consortComp.getConsortInfo(consortId);
            if (_m_iConsortShowInfo == null)
                _m_iConsortShowInfo = new ConsortRefShowInfo(consortId);
        }

        protected override void _onRefreshWnd()
        {
            if (wnd == null || _m_rConsortIntimacyRefObj == null)
                return;

            _refreshIntimacyValue();

            if (_m_wConsortCardItem != null)
            {
                if (_m_iConsortShowInfo != null)
                {
                    _m_wConsortCardItem.showWnd();
                    _m_wConsortCardItem.setInfo(_m_iConsortShowInfo);    
                }
                else
                {
                    _m_wConsortCardItem.hideWnd();
                }
            }
        }


        /// <summary>
        /// 刷新亲密度增加值文本
        /// </summary>
        private void _refreshIntimacyValue()
        {
            if (wnd == null || wnd.txtAddIntimacyValue == null || _m_rConsortIntimacyRefObj == null)
                return;

            int addIntimacy = _m_rConsortIntimacyRefObj.add_intimacy;

            if (!string.IsNullOrEmpty(wnd.txtAddIntimacyValueKey))
                ALUGUICommon.setLabelTxt(wnd.txtAddIntimacyValue, TextTranslate.instance.getLanguage(wnd.txtAddIntimacyValueKey, addIntimacy));
            else
                ALUGUICommon.setLabelTxt(wnd.txtAddIntimacyValue, TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, addIntimacy));
        }

        /// <summary>
        /// 妃子详情按钮点击
        /// </summary>
        private void _onBtnConsortInfoClick(GameObject _)
        {
            if (_m_rConsortIntimacyRefObj == null)
                return;

            GGottenConsortInfo gottenConsortInfo = NPPlayer.instance.consortComp.getConsortInfo(_m_rConsortIntimacyRefObj.consort_id);
            if (gottenConsortInfo != null)
            {
                GNodeUnLockConsortDetail.addConsortNodeInteraction(gottenConsortInfo);
            }
            else
            {
                QueueMgr.instance.AddNode(new GNodeLockConsortDetail(new ConsortRefShowInfo(_m_rConsortIntimacyRefObj.consort_id)));
            }
        }
    }
}
