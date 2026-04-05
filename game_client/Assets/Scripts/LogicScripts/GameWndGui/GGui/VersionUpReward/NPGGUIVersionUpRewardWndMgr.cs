using ALPackage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace GOE
{
    public class NPGGUIVersionUpRewardWndMgr
    {
        private static NPGGUIVersionUpRewardWndMgr _g_instance = new NPGGUIVersionUpRewardWndMgr();
        public static NPGGUIVersionUpRewardWndMgr instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new NPGGUIVersionUpRewardWndMgr();
                return _g_instance;
            }
        }

        private NPGGUIWndVersionUpRreward _m_wVersionUpReward;

        public NPGGUIVersionUpRewardWndMgr()
        {
        }

        // 处理按钮显示逻辑
        public void DealWndActivated(NPGGUIMonoCustomVersionUpReward _wnd)
        {
            if(null == _wnd)
                return;

            if(_m_wVersionUpReward != null)
                _m_wVersionUpReward.discard();
            _m_wVersionUpReward = new NPGGUIWndVersionUpRreward(_wnd);

            cacluVersionUpRewrad();
        }

        //取出版本更新奖励
        private void cacluVersionUpRewrad()
        {
            NPGVersionUpRewardRefObj reward = NPPlayer.instance.calNewVersionReward(GameResCore.instance.NewClientVersion);
            //无奖励则直接返回
            if(null == reward)
                return;

            if(_m_wVersionUpReward != null && reward != null)
                _m_wVersionUpReward.setReward(getReward(reward.reward_list));
        }


        //根据RewardID列表取出所有需要展示的奖励数据
        private CommonItemData[] getReward(List<NPCommonCostItem> _reward)
        {
            if(_reward == null)
                return null;

            List<CommonItemData> rewradShowList = new List<CommonItemData>();
            List<CommonItemData> tempRewradShowList = new List<CommonItemData>();

            for(int i = 0; i < _reward.Count; i++)
            {
                tempRewradShowList.Add(new CommonItemData(_reward[i].item, _reward[i].count, _reward[i].count.ToString()));

                rewradShowList.AddRange(tempRewradShowList);
            }

            return rewradShowList.ToArray();
        }

        // 处理按钮隐藏逻辑
        public void DealWndInactivated(NPGGUIMonoCustomVersionUpReward _wnd)
        {
            if(_m_wVersionUpReward != null)
                _m_wVersionUpReward.discard();
            _m_wVersionUpReward = null;
        }

        public void onGetRewardDown()
        {
            if(_m_wVersionUpReward != null)
                _m_wVersionUpReward.onGetRewardDown();
        }
    }
}
