using System.Collections.Generic;
using JetBrains.Annotations;

namespace GOE
{
    public class CommonHotRefPatchDealerMgr
    {
        private static CommonHotRefPatchDealerMgr _g_instance = new CommonHotRefPatchDealerMgr();
        [NotNull]public static CommonHotRefPatchDealerMgr instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new CommonHotRefPatchDealerMgr();
                return _g_instance;
            }
        }
        
        [NotNull]private Dictionary<string, _ICommonHotRefPatchDealer> _m_patchDealerDict = new Dictionary<string, _ICommonHotRefPatchDealer>();
        [NotNull]private List<string> _m_onlyServerTableNameList = new List<string>()
        {
            //只在服务器存在的表名
            "reward_sub",
            "rank_gift_pack"
        };

        public CommonHotRefPatchDealerMgr()
        {
            registerPatchDealer(new CommonRankRewardPatchDealer());
            registerPatchDealer(new CommonShopItemPatchDealer());
            registerPatchDealer(new CommonStepRewardPatchDealer());
            registerPatchDealer(new CrystalGiftPackPatchDealer());
            registerPatchDealer(new GiftPackPatchDealer());
            registerPatchDealer(new RewardRefPatchDealer());
            registerPatchDealer(new GiftPackGroupPatchDealer());
            registerPatchDealer(new ActivityStepRewardPatchDealer());
            registerPatchDealer(new RechargeRebateStepPatchDealer());
            registerPatchDealer(new ActivityFundStepPatchDealer());
            registerPatchDealer(new ActivityRankRewardPatchDealer());
        }
        
        public bool isOnlyServerTable(string _tableName)
        {
            return _m_onlyServerTableNameList.Contains(_tableName);
        }
        
        /// <summary>
        /// 注册处理器
        /// </summary>
        /// <param name="_dealer"></param>
        public void registerPatchDealer(_ICommonHotRefPatchDealer _dealer)
        {
            if (_dealer == null)
                return;

            _m_patchDealerDict.TryAdd(_dealer.getTableName, _dealer);
        }
        
        
        /// <summary>
        /// 获取处理器
        /// </summary>
        /// <param name="_tableName"></param>
        /// <returns></returns>
        public _ICommonHotRefPatchDealer getPatchDealer(string _tableName)
        {
            return _m_patchDealerDict.GetValueOrDefault(_tableName);
        }
    }
}