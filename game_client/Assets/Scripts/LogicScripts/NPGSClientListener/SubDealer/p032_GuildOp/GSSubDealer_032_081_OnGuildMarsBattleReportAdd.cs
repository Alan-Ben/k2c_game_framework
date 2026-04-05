using ALBasicProtocolPack;
using GS2GC.p032_GuildOp;

namespace GOE
{
    /// <summary>
    /// 火星矿-新增联盟战报推送
    /// </summary>
    public class GSSubDealer_032_081_OnGuildMarsBattleReportAdd : NPSubDealer<GS2GC_032_081_OnGuildMarsBattleReportAdd>
    {
        protected override GS2GC_032_081_OnGuildMarsBattleReportAdd _createProtocolObj()
        {
            return new GS2GC_032_081_OnGuildMarsBattleReportAdd();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_032_081_OnGuildMarsBattleReportAdd _msg)
        {
            if (_msg == null)
                return;

            NPPlayer.instance.guildComp.onGuildMarsBattleReportAdd(_msg);
        }
    }
}
