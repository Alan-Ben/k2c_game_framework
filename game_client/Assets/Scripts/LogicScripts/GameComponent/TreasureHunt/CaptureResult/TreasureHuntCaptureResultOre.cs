using System;
using Common.TreasureHuntEnum;
using Common.TreasureHuntObj;
using ALBasicProtocolPack;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 太空寻宝捕捉结果 - 矿石
    /// </summary>
    public class TreasureHuntCaptureResultOre : TreasureHuntCaptureResultBase
    {
        /// <summary>
        /// 矿石详细数据
        /// </summary>
        private TreasureHunt_CaptureResult_Ore _serverOreData;
        private _ITreasureHuntOreInfo _m_iOreInfo;

        public TreasureHuntCaptureResultOre([NotNull] TreasureHunt_CaptureReward protocolData) : base(protocolData)
        {
            if (protocolData.getData() != null)
            {
                _serverOreData = new TreasureHunt_CaptureResult_Ore();
                _serverOreData.readPackage(protocolData.getData());
                
                Debug.Log_EditorOnly($"获得矿石:{GCommon.GetInfoPropertys(_serverOreData)}");
            }
        }
        
        /// <summary>
        /// 矿石ID
        /// </summary>
        public long oreId => _serverOreData?.getOreId() ?? 0;

        /// <summary>
        /// 重量
        /// </summary>
        public int weight => _serverOreData?.getWeight() ?? 0;

        /// <summary>
        /// 是否首次捕捉
        /// </summary>
        public bool isFirstCapture => _serverOreData?.getIsFirstCapture() ?? false;

        /// <summary>
        /// 最大矿石归属者cid
        /// </summary>
        public long serverMaxCid => _serverOreData?.getServerMaxCid() ?? 0;

        /// <summary>
        /// 最大矿石重量
        /// </summary>
        public int serverMaxWeight => _serverOreData?.getServerMaxWeight() ?? 0;

        /// <summary>
        /// 是否首次捕捉高级
        /// </summary>
        public bool isFirstDrawAdvanced => _serverOreData?.getIsFirstDrawAdvanced() ?? false;

        public _ITreasureHuntOreInfo oreInfo
        {
            get
            {
                if (_serverOreData == null)
                    return null;
                
                if (_m_iOreInfo == null || _m_iOreInfo.oreId != _serverOreData.getOreId())
                {
                    TreasureHuntOreRefObj oreRefObj = GRefdataCoreMgr.instance.treasureHuntOreRefCore.getRef(_serverOreData.getOreId());
                    if (oreRefObj == null)
                        return null;

                    ETreasureHuntOreState oreState = oreRefObj.isReachAdvanceOreMass(_serverOreData.getWeight()) ? ETreasureHuntOreState.ACTIVATED_ADVANCED : ETreasureHuntOreState.ACTIVATED_NORMAL;
                    _ITreasureHuntSkillInfo normalSkillInfo = new TreasureHuntCommonSkillInfo(oreRefObj.normal_skill_id, true, 0, null, 0);
                    _ITreasureHuntSkillInfo advancedSkillInfo = new TreasureHuntCommonSkillInfo(oreRefObj.advanced_skill_id, true, 0, null, 0);
                    
                    _m_iOreInfo = new TreasureHuntCommonOreInfo(oreRefObj, oreState, 1, _serverOreData.getWeight(), 0, normalSkillInfo, advancedSkillInfo);
                }
                return _m_iOreInfo;
            }
        }
        
        /// <summary>
        /// 处理矿石捕捉结果
        /// </summary>
        public override void showCaptureResult(Action _showDone)
        {
            NPUINoticeMgr.instance.addDealer(new NoticeDealer_CustomAction((_setDealDone) =>
            {
                GGUIWndTreasureHuntCaptureOreResult.instance.load();
                GGUIWndTreasureHuntCaptureOreResult.instance.regLoadDoneDelegate(() =>
                {
                    GGUIWndTreasureHuntCaptureOreResult.instance.setData(this);
                    GGUIWndTreasureHuntCaptureOreResult.instance.showWnd();
                });
            }, NPNoticeType.g_AllTypeArr, ()=>
            {
                GGUIWndTreasureHuntCaptureOreResult.instance.discard();
                _showDone?.Invoke();
            }, default, UINodeTagConst.C_TREASURE_HUNT_CAPTURE_ORE_RESULT, true, true, true, true));
        }
    }
}
