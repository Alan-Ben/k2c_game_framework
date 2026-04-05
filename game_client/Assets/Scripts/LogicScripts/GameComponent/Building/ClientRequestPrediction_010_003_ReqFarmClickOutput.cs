using GC2GS.p010_BuildingOp;
using GS2GC.p010_BuildingOp;

namespace GOE
{
    public class ClientRequestPrediction_010_003_ReqFarmClickOutput : _AClientRequestPrediction<GC2GS_010_003_ReqFarmClickOutput, GS2GC_010_003_RetFarmClickOutput>
    {
        private readonly long _m_buildingId;
        private readonly long _m_clickNum;
        private readonly long _m_opTimeMs;
        private long _m_predictGold;
        
        
        public ClientRequestPrediction_010_003_ReqFarmClickOutput(long _buildingId, long _clickNum, long _opTimeMs)
        {
            _m_buildingId = _buildingId;
            _m_clickNum = _clickNum;
            _m_opTimeMs = _opTimeMs;
        }
        
        
        protected override GC2GS_010_003_ReqFarmClickOutput _createProtocolObj()
        {
            return new GC2GS_010_003_ReqFarmClickOutput(_m_buildingId, _m_clickNum, _m_opTimeMs);
        }

        protected override GS2GC_010_003_RetFarmClickOutput _predictResponse()
        {
            return new GS2GC_010_003_RetFarmClickOutput();
        }

        protected override void _enablePrediction(GS2GC_010_003_RetFarmClickOutput _predictResponse)
        {
            FarmingBuildingInfo farmingBuilding = NPPlayer.instance.buildingComp.getFarmingBuildingInfo(_m_buildingId);
            if (farmingBuilding == null)
                return;

            _m_predictGold = farmingBuilding.clickEarnings;
            NPPlayer.instance.specialItemComp.goldData._addValue(_m_predictGold);
        }

        protected override bool _isPredictionCorrect(GS2GC_010_003_RetFarmClickOutput _predictResponse, bool _isRequestSuc, GS2GC_010_003_RetFarmClickOutput _serverResponse)
        {
            // 只有发送失败时才算作预测失误
            return _isRequestSuc;
        }

        protected override void _fixWrongPrediction(GS2GC_010_003_RetFarmClickOutput _predictResponse, bool _isRequestSuc, GS2GC_010_003_RetFarmClickOutput _serverResponse)
        {
            // 只有请求失败时才进行数据修正，因为这时服务端不会推送最新的数据覆盖预测的数据
            if (!_isRequestSuc)
            {
                // 如果预测错误，就减掉自己加的金币
                NPPlayer.instance.specialItemComp.goldData._addValue(-_m_predictGold);
            }
        }
    }
}