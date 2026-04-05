using System.Collections.Generic;
using ALPackage;
using ChatPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public partial class GPlayerTravelComponent
    {
        private class RedTipDealer
        {
            private GPlayerTravelComponent _m_component;

            public RedTipDealer(GPlayerTravelComponent _component)
            {
                _m_component = _component;
            }

            /// <summary>
            /// 初始化聊天红点
            /// </summary>
            public void init()
            {
                _refreshAllRed();
                WinMsg.RegisterMsgAct(WinMsgType.ON_LAZY_CD_CHG, _refreshCostRed);
            }

            private void _refreshAllRed()
            {
                _refreshCostRed();
            }

            public void clear()
            {
                WinMsg.UnregisterMsgAct(WinMsgType.ON_LAZY_CD_CHG, _refreshCostRed);
            }

            /// <summary>
            /// 刷新游历消耗红点数据
            /// </summary>
            private void _refreshCostRed()
            {
                int travelRedCount = 0;

                PlayerLazyCDInfo lazyCdInfo = NPPlayer.instance.lazyCdComp.getLazyCDInfo(GRefdataCoreMgr.instance.npGeneral.travel_cost_lazycd_id);
                if(null != lazyCdInfo && lazyCdInfo.getCount() >= GRefdataCoreMgr.instance.npGeneral.travel_show_lazycd_red_count)
                    travelRedCount++;
                
                RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_TRAVEL_COST, travelRedCount);
            }

            
        }
    }
}