using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 跟随窗口
    /// </summary>
    public class GGUIWndCommonToolTip_DinnerJoinWay : _ATNPGGUIWndCommonItemToolTip<GGUIMonoCommonToolTip_DinnerJoinWay>
    {
        private DinnerJoinWayDetail _m_joinWayDetail;
        public GGUIWndCommonToolTip_DinnerJoinWay(DinnerJoinWayDetail _joinWayDetail, string _assetPath, string _assetName) : base(_assetPath, _assetName)
        {
            _m_joinWayDetail = _joinWayDetail;
        }

        protected override void _onDiscard()
        {
            base._onDiscard();
        }

        protected override void _onWndInitDone()
        {
            base._onWndInitDone();
            if (null == wnd)
                return;
        }

        protected override void _onClose()
        {
            QueueMgr.instance.forceCloseNodeByType(typeof(GNodeCommonToolTip_DinnerJoinWay));
        }
        
        public void setInfo(RectTransform _targetTransRoot, float _intervalX,float _intervalY)
        {
            _refreshWnd();
            setPos(_targetTransRoot,_intervalX, _intervalY);
        }
        
        private void _refreshWnd()
        {
            if(null == wnd || null == _m_joinWayDetail)
                return;
            
            ALUGUICommon.setLabelTxt(wnd.txtBasic,TextTranslate.instance.getLanguage(TransKeyConst.dinner_join_cost_tooltip_basic, _m_joinWayDetail.basic));
            ALUGUICommon.setLabelTxt(wnd.txtRankBonus,TextTranslate.instance.getLanguage(TransKeyConst.dinner_join_cost_tooltip_rank_bonus, _m_joinWayDetail.rankBonus));
            ALUGUICommon.setLabelTxt(wnd.txtFellowTalent,TextTranslate.instance.getLanguage(TransKeyConst.dinner_join_cost_tooltip_fellow_talent, _m_joinWayDetail.fellowTalent));
            ALUGUICommon.setLabelTxt(wnd.txtFamilyRelationShip,TextTranslate.instance.getLanguage(TransKeyConst.dinner_join_cost_tooltip_family_relation_ship, _m_joinWayDetail.familyRelationShip));
            
        }
    }
}