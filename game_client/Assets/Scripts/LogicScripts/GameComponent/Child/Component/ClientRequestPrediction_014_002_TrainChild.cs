using Common.ChildObj;
using CommonEnum;
using GC2GS.p014_ChildOp;
using GS2GC.p004_PlayerOp;
using GS2GC.p014_ChildOp;

namespace GOE
{
    /// <summary>
    /// 客户端对服务端训练子嗣的预测
    /// </summary>
    public class ClientRequestPrediction_014_002_TrainChild : _AClientRequestPrediction<GC2GS_014_002_ReqTrainChild, GS2GC_014_002_RetTrainChild>
    {
        private readonly long _m_id;

        // 预测前的数据
        private int _m_originLvl;
        private long _m_seatId;
        private int _m_originSeatEnergy;
        private int _m_originSeatMaxEnergy;
        private long _m_originSeatLastCalcTime;
        private long _m_originSeatFullNextRemainMs;
        
        
        public ClientRequestPrediction_014_002_TrainChild(long _id)
        {
            _m_id = _id;
        }
        
        
        protected override GC2GS_014_002_ReqTrainChild _createProtocolObj()
        {
            return new GC2GS_014_002_ReqTrainChild(_m_id);
        }
        protected override GS2GC_014_002_RetTrainChild _predictResponse()
        {
            ChildInfo childInfo = NPPlayer.instance.childComp.getChildInfo(_m_id);
            if (childInfo == null)
                return new GS2GC_014_002_RetTrainChild();
            
            return new GS2GC_014_002_RetTrainChild(
                childInfo.getEducationCostValue(), 
                childInfo.getEducationExpValue(), 
                childInfo.getEarningsCurrentAddValue());
        }
        protected override void _enablePrediction(GS2GC_014_002_RetTrainChild _predictResponse)
        {
            ChildInfo childInfo = NPPlayer.instance.childComp.getChildInfo(_m_id);
            if (childInfo == null)
                return;

            _m_originLvl = childInfo.level;
            
            _m_seatId = childInfo.seatInfo.id;
            _m_originSeatEnergy = childInfo.seatInfo.energy;
            _m_originSeatMaxEnergy = childInfo.seatInfo.maxEnergy;
            _m_originSeatLastCalcTime = childInfo.seatInfo.lastCalTime;
            _m_originSeatFullNextRemainMs = childInfo.seatInfo.fullNextRemainMs;
            
            // 本地预测子嗣等级 + 1
            NPPlayer.instance.childComp._onChildLvlChg(new GS2GC_014_052_OnChildLvlChg(_m_id, _m_originLvl + 1));

            // 本地预测脑力消耗情况，脑力 - 1，并更新相关时间戳
            long newLastCalcTime = _m_originSeatLastCalcTime;
            long newFullNextRemainMs = _m_originSeatFullNextRemainMs;
            if (_m_originSeatEnergy >= _m_originSeatMaxEnergy)
                newLastCalcTime = FpsAndPingMgr.instance.serverTimeTag; // 如果是这种情况根据网络延迟必定会跟服务端有差，无法避免
            int newEnergy = _m_originSeatEnergy - 1;
            if (newEnergy < 0)
                newEnergy = 0;
            if (newEnergy < _m_originSeatMaxEnergy && newFullNextRemainMs > 0)
            {
                newLastCalcTime -= newFullNextRemainMs;
                newFullNextRemainMs = 0;
            }
            Child_SeatInfo seatInfo = new Child_SeatInfo(_m_seatId, _m_originSeatMaxEnergy, newEnergy, newLastCalcTime, newFullNextRemainMs);
            NPPlayer.instance.childComp._onSeatChg(new GS2GC_014_053_OnSeatChg(seatInfo));
            NPPlayer.instance.specialItemComp.goldData._addValue(-_predictResponse.getCostValue());
            NPPlayer.instance.rescourceComp.addValue(ECurrency.HERO_EXP, _predictResponse.getGainValue());
            NPPlayer.instance.childComp._onChildBonusChg(new GS2GC_014_062_OnChildBonusChg(_m_id, childInfo.earnings + _predictResponse.getAddBonus()));;
        }

        protected override bool _isPredictionCorrect(GS2GC_014_002_RetTrainChild _predictResponse, bool _isRequestSuc, GS2GC_014_002_RetTrainChild _serverResponse)
        {
            // 如果通信成功比对客户端预测值和服务端实际值
            if (_isRequestSuc)
            {
                return _predictResponse.getAddBonus() == _serverResponse.getAddBonus() &&
                       _predictResponse.getGainValue() == _serverResponse.getGainValue() &&
                       _predictResponse.getCostValue() == _serverResponse.getCostValue();
            }
            
            return false;
        }

        protected override void _fixWrongPrediction(GS2GC_014_002_RetTrainChild _predictResponse, bool _isRequestSuc, GS2GC_014_002_RetTrainChild _serverResponse)
        {
            // 发送消息，提示客户端预测失败
            WinMsg.SendMsg(WinMsgType.CHILD_EDUCATION_PREDICT_FAIL);
            
            // // 如果通信失败，客户端本地预测的值要手动改为原值
            // if (!_isRequestSuc)
            // {
            //     NPPlayer.instance.childComp._onChildLvlChg(new GS2GC_014_052_OnChildLvlChg(_m_id, _m_originLvl));
            //     Child_SeatInfo seatInfo = new Child_SeatInfo(_m_seatId, _m_originSeatMaxEnergy, _m_originSeatEnergy, _m_originSeatLastCalcTime, _m_originSeatFullNextRemainMs);
            //     NPPlayer.instance.childComp._onSeatChg(new GS2GC_014_053_OnSeatChg(seatInfo));
            // }
            // 如果通信成功，就让服务端的值自行覆盖本地值即可
        }
    }
}