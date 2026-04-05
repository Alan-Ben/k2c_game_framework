using System;
using Common.GuildEnum;
using Common.MarsEnum;
using Common.MarsObj;
using GC2GS.p041_MarsExploreOp;
using GS2GC.p041_MarsExploreOp;

namespace GOE
{
    /// <summary>
    /// 维修状态的额外数据
    /// </summary>
    public class MarsTeamEx_Repaire : _ATMarsTeamBasicExData<MarsTeamState_Repair>, _IGuildMarsHelp
    {
        private long _m_repairNum;
        private long _m_guildHelpId;
        private long _m_guildHelpSecs;
        private long _m_lItemHelpSecs;//物品帮助减少的时间
        
        public EGuildMarsHelpObjType helpObjType => EGuildMarsHelpObjType.TEAM_REPAIR;

        public long guildHelpObjId => teamInfo?.teamId ?? 0;
        public override long guildHelpId => _m_guildHelpId;
        public long repairNum => _m_repairNum;

        public MarsTeamEx_Repaire(MarsExploreTeamInfo _tiTeamInfo, byte[] _exData)
           : base(_tiTeamInfo, _exData)
        {
        }

        /// <summary>
        /// 读取数据，并返回结构体
        /// </summary>
        /// <param name="_exData"></param>
        /// <returns></returns>
        protected override MarsTeamState_Repair _readExData(byte[] _exData)
        {
            MarsTeamState_Repair data = new MarsTeamState_Repair();
            data.readPackage(_exData);
            _m_repairNum = data.getRepairNum();
            _m_guildHelpId = data.getGuildHelpId();
            _m_guildHelpSecs = data.getGuildHelpSecs();
            _m_lItemHelpSecs = data.getItemHelpSecs();
            return data;
        }

        /// <summary>
        /// 额外可以减少的存在时间，根据不同状态额外数据做处理
        /// 一般为0
        /// </summary>
        public override long exReduceTimeMS { get { return _m_guildHelpSecs * 1000 + _m_lItemHelpSecs * 1000; } }

        /// <summary>
        /// 更新扩展数据，如果状态没变化，则调用此接口
        /// </summary>
        /// <param name="_exData"></param>
        public override void onUpdateExData(_IMarsTeamExDataInterface _exData)
        {

        }

        /// <summary>
        /// 进入本状态类的处理
        /// </summary>
        public override void onEnterExData()
        {
            NPPlayer.instance.guildMarsHelpComp.regMyMarsHelp(this);
        }

        /// <summary>
        /// 退出本状态类的处理
        /// </summary>
        public override void onExitExData()
        {
            NPPlayer.instance.guildMarsHelpComp.unregMyMarsHelp(this);
        }
        
        
        
        public void onGuildHelpChg(long _guildHelpId, long _guildHelpSecs)
        {
            _m_guildHelpId = _guildHelpId;
            _m_guildHelpSecs = _guildHelpSecs;
        }

        public void onGuildHelpDel()
        {
            
        }
        
        public void onItemHelpSecsChg(long _itemHelpSecs)
        {
            _m_lItemHelpSecs = _itemHelpSecs;
        }
        
        public override EMarsBagItemUseTimeType timeType { get { return EMarsBagItemUseTimeType.MARS_TEAM_REPAIR; } }
        public override void reqCompleteNow(Action<bool> _complete)
        {
            if (teamInfo == null)
            {
                _complete?.Invoke(false);
                return;
            }
            
            int maskSerialize = MainCameraMono.selfInstance.openAllInputMask();
            NPGSClientListener.sendRequestByLog(new GC2GS_041_016_ReqSetTempRepairDone(teamInfo.teamId), 
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_041_016_RetSetTempRepairDone>((_isSuc, _msg) =>
                {
                    MainCameraMono.selfInstance.closeAllInputMask(maskSerialize);
                    _complete?.Invoke(_isSuc);
                }));
        }
    }
}