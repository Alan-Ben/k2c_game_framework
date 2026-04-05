using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 晚间副本掉血tip配置
    /// </summary>
    [Serializable]
    public class EveningDungeonBossLossBloodTip
    {
        [ALHeader("掉血量, 会找到掉血量超过这个值的最小值的那条配置显示")]
        public long lossBlood;
        
        [ALHeader("center_tips表中id")]
        public long centerTipId;
    }
    
    /// <summary>
    /// 掉血tip
    /// </summary>
    public class GGUIMonoEveningDungeonLossBloodTip : _AALBasicUIWndMono
    {
        [ALHeader("弹出tip父节点")]
        public Transform tipParent;
        [ALHeader("默认boss掉血tip在center_tips表中id")]
        public long defaultLossBloodCenterTipId;
        [ALHeader("掉血tip配置(会找到掉血量超过传入值的最小值的那条配置显示)")]
        public List<EveningDungeonBossLossBloodTip> lossBloodTipConfigList;
        
        private void Awake()
        {
            // 对lossBloodTipConfigList按照lossBlood从大到小排序
            lossBloodTipConfigList?.Sort((_a, _b) =>
            {
                if (_b == null) return -1;
                if (_a == null) return 1;
                if(ReferenceEquals(_a, _b)) return 0;
                
                return _b.lossBlood.CompareTo(_a.lossBlood);
            });
        }


#if NP_GAME
        /// <summary>
        /// 获取掉血tip配表数据
        /// </summary>
        /// <param name="lossBlood"></param>
        /// <returns></returns>
        public NPCenterTipsRefObj getLossBloodTipRefObj(long lossBlood)
        {
            if (lossBloodTipConfigList == null)
                return null;

            NPCenterTipsRefObj tipsRef = null;
            foreach (var item in lossBloodTipConfigList)
            {
                // 因为在awake时已经按照lossBlood从大到小排序了, 所以找到第一个配置的lossBlood小于等于传入值的配置即可
                if (item != null && lossBlood >= item.lossBlood)
                {
                    // 因为是map查找快所以不用预先存储
                    tipsRef = GRefdataCoreMgr.instance.tipMap.getRef(item.centerTipId);
                    if (tipsRef != null)
                        return tipsRef;
                }
            }

            return null;
        }        
#endif

    }
}