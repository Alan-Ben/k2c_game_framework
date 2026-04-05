using System;
using NPEnum;

namespace GOE
{
    public class DinnerJoinWayDetail
    {
        private GDinnerJoinCostRefObj _m_refObj;
        //基础人气
        public string basic => _m_refObj == null ? "" : _m_refObj.join_gain_coin.ToString();
        //评级加成 
        public string rankBonus => (Math.Max(NPPlayer.instance.playerInfo.curLevelRef.lvl -5, 0)).ToString();
        //伙伴觉醒 
        public string fellowTalent => (NPPlayer.instance.heroComponent.playerPropertyContainer.getValue(ENPPlayerPropertyType.DINNER_JOINER_SCORE_PER)/100f).ToString();
        //家人关系 
        public string familyRelationShip => (NPPlayer.instance.consortComp.getPlayerPropertyContainer().getValue(ENPPlayerPropertyType.DINNER_JOINER_SCORE_PER)/100f).ToString();

        public DinnerJoinWayDetail(GDinnerJoinCostRefObj _refObj)
        {
            _m_refObj = _refObj;
        }
    }
}