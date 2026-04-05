package NPUSServer.GMCommand.Cmds;

import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPEnum.ENPItemType;
import NPUSServer.GMCommand.UsCmdBase;
import NPUSServer.NPUSUserMgr.UserComp.PlayerLazyCDComp.PlayerLazyCD;

/**
 * @description: 任务相关命令
 * @author: mark
 * @date: 2022-04-27 14:17:59
 */
@ACommander(comment = "cd", name = "cd")
public class CmdCD extends UsCmdBase
{
    @ACommand(comment = "获得lazyCd[cdId][数量]")
    public String gainLazyCD(long _id, int _count)
    {
        getOwner().gainItem(ENPItemType.LAZY_CD, _id, _count, getContext());
        if (!getContext().getCollector().isEmpty())
        {
            getOwner().sendMsgToGC(getContext().getCollector().toProto());
        }
        return "ok";
    }

    @ACommand(comment = "设置lazyCd的数量[cdId][数量]")
    public String setLazyCD(int _cdId, int _chg)
    {
        return getOwner().getLazyCDComponent().gmChg(_cdId, _chg, getContext());
    }

    @ACommand(comment = "设置lazyCd获取下一点的时间[cdId][秒]")
    public String setLazyCdNextTime(int _cdId, int _sec)
    {
        return getOwner().getLazyCDComponent().ensureLazyCD(_cdId).cmdSetTimeToRecoverNextCd(_sec) ? "ok" : "fail";
    }

    @ACommand(comment = "获得fixedCd[cdId][数量]")
    public String gainFixedCd(long _id, int _count)
    {
        getOwner().gainItem(ENPItemType.FIXED_CD, _id, _count, getContext());
        if (!getContext().getCollector().isEmpty())
        {
            getOwner().sendMsgToGC(getContext().getCollector().toProto());
        }
        return "ok";
    }

    @ACommand(comment = "设置fixedCd数量[id][数量]")
    public String setFixedCd(long _id, int _count)
    {
        getOwner().getFixedCdComponent().setItemCount(_id, _count, getContext());
        return "ok";
    }

    @ACommand(comment = "重新刷新fixedCd")
    public String setFixedCdFreshTime(long _id)
    {
        getOwner().getFixedCdComponent().cmdSetFreshTime(_id);
        return "ok";
    }

    @ACommand(comment = "fixed cd info")
    public String fixedCdInfo()
    {
        return getOwner().getFixedCdComponent().toString();
    }

    @ACommand(comment = "lazy cd info")
    public String lazyCdInfo()
    {
        return getOwner().getLazyCDComponent().toString();
    }

    @ACommand(comment = "增加指定CD额外计数[cdId][计数]")
    public String addCountExt(long _id, int _count)
    {
    	PlayerLazyCD cd = getOwner().getLazyCDComponent().ensureLazyCD((int) _id);
    	cd.addCountExt(_count);
    	
    	return "ok";
    }

    @ACommand(comment = "减少指定CD额外计数[cdId][计数]")
    public String descCount(long _id, int _count)
    {
    	PlayerLazyCD cd = getOwner().getLazyCDComponent().ensureLazyCD((int) _id);
    	cd.descCount(_count, getContext());
    	
    	return "ok";
    }
}
