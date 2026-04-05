using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 抽卡累计奖励次数
    /// </summary>
    public class GGUICustomMonoGachaCumulativeRewardNeedNum : MonoBehaviour
    {
        [ALHeader("卡池id")]
        public long poolId;
        
        [ALHeader("抽卡计数文本")]
        public TextEx txtCumulativeCount;
        
        [ALHeader("可领取奖励的召唤计数key(两个参数 1.已召唤次数 2.可领奖召唤次数)")]
        public string canDrawRewardCumulativeCountKey;
        
        [ALHeader("不可领取奖励的召唤计数key(两个参数 1.已召唤次数 2.可领奖召唤次数)")]
        public string cannotDrawRewardCumulativeCountKey;

#if NP_GAME
        /// <summary>
        /// 卡池信息
        /// </summary>
        private GachaPoolInfo _m_wPoolInfo;
#endif
        
        private void Awake()
        {
#if NP_GAME
#endif
        }

        private void OnDestroy()
        {
#if NP_GAME

#endif
        }

        private void OnEnable()
        {
#if NP_GAME
                _refreshWnd();
                
                WinMsg.RegisterMsg(WinMsgType.ON_GACHA_POOL_INFO_CHG, _onGachaPoolInfoChg);
#endif
        }

        private void OnDisable()
        {
#if NP_GAME
                WinMsg.UnregisterMsg(WinMsgType.ON_GACHA_POOL_INFO_CHG, _onGachaPoolInfoChg);
#endif
        }

        private void _refreshWnd()
        {
#if NP_GAME
                if(_m_wPoolInfo == null)
                        _m_wPoolInfo = NPPlayer.instance.gachaComp.getGachaPoolInfo(poolId);
                
                GachaPoolRefObj poolRefObj = _m_wPoolInfo?.poolRefObj ?? GRefdataCoreMgr.instance.gachaPoolRefCore.getRef(poolId);
                if (poolRefObj == null)
                {
                        Debug.LogError($"[GGUICustomMonoGachaCumulativeRewardNeedNum _refreshWnd] poolRefObj is null, poolId: {poolId}");
                        return;
                }

                string key =
                        _m_wPoolInfo != null &&
                        _m_wPoolInfo.cumulativeRewardTimes >= poolRefObj.cumulative_reward_need_num
                                ? canDrawRewardCumulativeCountKey
                                : cannotDrawRewardCumulativeCountKey;

                if (string.IsNullOrEmpty(key))
                        key = TransKeyConst.common_useNum_num_num;
                
                ALUGUICommon.setLabelTxt(txtCumulativeCount,
                        TextTranslate.instance.getLanguage(key, _m_wPoolInfo?.cumulativeRewardTimes ?? 0, poolRefObj.cumulative_reward_need_num));
#endif
        }

        /// <summary>
        /// 卡池信息变更
        /// </summary>
        private void _onGachaPoolInfoChg(params object[] _objs)
        {
                if(_objs == null || _objs.Length < 1 || !(_objs[0] is long))
                        return;

                long gachaPoolId = (long)_objs[0];
                if(gachaPoolId == poolId)
                        _refreshWnd();
        }
    }
}