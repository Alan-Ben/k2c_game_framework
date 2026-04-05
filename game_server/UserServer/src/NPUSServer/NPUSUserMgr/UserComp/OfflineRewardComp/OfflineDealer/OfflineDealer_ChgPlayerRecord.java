package NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineDealer;

import Common.OfflineRewardEnum.EOfflineRewardEnum;
import Common.OfflineRewardObj.Offline_ChgPlayerRecord;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineRewardInfo;
import NPUSServer.USLog;

public class OfflineDealer_ChgPlayerRecord extends _AOfflineDataDealer
{
    @Override
    public EOfflineRewardEnum getEnum()
    {
        return EOfflineRewardEnum.CHG_PLAYER_RECORD;
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
            Offline_ChgPlayerRecord obj = new Offline_ChgPlayerRecord();
            obj.readPackage(_info.getOfflineData());

            _info.getUserData().getRecordComponent().ensureRecord(obj.getType(), obj.getNum(), obj.getDealType(), _context);
        }
        catch(Exception ex)
        {
            USLog.error(_info.getUserData().getUSServer(), "", ex);
        }
    }
}
