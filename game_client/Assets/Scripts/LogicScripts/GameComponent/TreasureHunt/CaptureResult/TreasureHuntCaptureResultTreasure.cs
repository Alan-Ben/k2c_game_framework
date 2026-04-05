using System;
using Common.TreasureHuntEnum;
using Common.TreasureHuntObj;
using ALBasicProtocolPack;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 太空寻宝捕捉结果 - 奇物
    /// </summary>
    public class TreasureHuntCaptureResultTreasure : TreasureHuntCaptureResultBase
    {
        /// <summary>
        /// 奇物详细数据
        /// </summary>
        private TreasureHunt_CaptureResult_Treasure _treasureData;

        /// <summary>
        /// 奇物ID
        /// </summary>
        public long treasureId => _treasureData?.getTreasureId() ?? 0;

        public TreasureHuntCaptureResultTreasure([NotNull] TreasureHunt_CaptureReward protocolData) : base(protocolData)
        {
            if (protocolData.getData() != null)
            {
                _treasureData = new TreasureHunt_CaptureResult_Treasure();
                _treasureData.readPackage(protocolData.getData());
                
                Debug.Log_EditorOnly($"获得奇物:{GCommon.GetInfoPropertys(_treasureData)}");
            }
        }

        /// <summary>
        /// 处理奇物捕捉结果
        /// </summary>
        public override void showCaptureResult(Action _showDone)
        {
            NPUINoticeMgr.instance.addDealer(new NoticeDealer_CustomAction((_setDealDone) =>
            {
                GGUIWndTreasureHuntCaptureTreasureResult.instance.load();
                GGUIWndTreasureHuntCaptureTreasureResult.instance.regLoadDoneDelegate(() =>
                {
                    GGUIWndTreasureHuntCaptureTreasureResult.instance.setData(this);
                    GGUIWndTreasureHuntCaptureTreasureResult.instance.showWnd();
                });
            }, NPNoticeType.g_AllTypeArr, ()=>
            {
                GGUIWndTreasureHuntCaptureTreasureResult.instance.discard();
                _showDone?.Invoke();
            }, default, UINodeTagConst.C_TREASURE_HUNT_CAPTURE_TREASURE_RESULT, true, true, true, true));
        }
    }
}
