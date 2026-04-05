using System;
using System.Collections.Generic;
using Common.TreasureHuntEnum;
using Common.TreasureHuntObj;
using ALBasicProtocolPack;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 太空寻宝捕捉结果 - 奖励
    /// </summary>
    public class TreasureHuntCaptureResultReward : TreasureHuntCaptureResultBase
    {
        /// <summary>
        /// 奖励详细数据
        /// </summary>
        private TreasureHunt_CaptureResult_Reward _rewardData;

        /// <summary>
        /// 物品列表
        /// </summary>
        public List<NPCommon.NPCommon_ItemInfo> rewardItemList => _rewardData?.getItemList();

        public TreasureHuntCaptureResultReward([NotNull] TreasureHunt_CaptureReward protocolData) : base(protocolData)
        {
            if (protocolData.getData() != null)
            {
                _rewardData = new TreasureHunt_CaptureResult_Reward();
                _rewardData.readPackage(protocolData.getData());
                
                Debug.Log_EditorOnly($"获得奖励:{GCommon.GetInfoPropertys(_rewardData)}");
            }
        }

        /// <summary>
        /// 处理奖励捕捉结果
        /// </summary>
        public override void showCaptureResult(Action _showDone)
        {
            GCommon.dealGainItem(rewardItemList, TransKeyConst.common_getreward_tip, _showDone);
        }
    }
}
