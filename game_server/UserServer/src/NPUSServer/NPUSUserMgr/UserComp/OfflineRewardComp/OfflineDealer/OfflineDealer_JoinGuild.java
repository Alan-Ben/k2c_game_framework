package NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineDealer;

import Common.OfflineRewardEnum.EOfflineRewardEnum;
import Common.OfflineRewardObj.Offline_PlayerJoinGuild;
import NPCommon.Enum.EUsParam;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineRewardInfo;
import NPUSServer.USLog;
import USDB.Update.Update_1_0_1_1_To_1_0_1_2;

public class OfflineDealer_JoinGuild extends _AOfflineDataDealer
{
    @Override
    public EOfflineRewardEnum getEnum()
    {
        return EOfflineRewardEnum.JOIN_GUILD;
    }

    @Override
    public boolean isValid()
    {
        return true;
    }

    @Override
    public boolean syncToClient()
    {
        return false;
    }

    @Override
    protected void _preDeal(OfflineRewardInfo _info, NPPlayerContext _context)
    {
        if(null == _info.getOfflineData())
        {
            return;
        }

        try
        {
            Offline_PlayerJoinGuild obj = new Offline_PlayerJoinGuild();
            obj.readPackage(_info.getOfflineData());

            long guildId = obj.getNewGuildId();

            long hadRepairGuildId = _info.getUserData().getUSServer().getUSParams().getParam(EUsParam.HAD_REPAIR_GUILD_ID);
            if (hadRepairGuildId == 1 && (guildId % 100000 != _info.getUserData().getUSServer().getServerTypeId()))
            {
                guildId = Update_1_0_1_1_To_1_0_1_2.transNewId(_info.getUserData().getUSServer().getServerTypeId(), guildId);
            }

            _info.getUserData().getGuildComponent().joinGuild(guildId, obj.getGuildName(), obj.getSimpleName(), obj.getBuildingAddPerArr());
        }
        catch(Exception ex)
        {
            USLog.error(_info.getUserData().getUSServer(), "", ex);
        }
    }
}
