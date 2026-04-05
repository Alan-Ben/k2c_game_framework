package NPUSServer.GMCommand.Cmds;


import Common.GraveObj.GraveObj_NewInfo;
import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPCommon.Util.CommonFunc;
import NPUSServer.GMCommand.UsCmdBase;

/**
 * @author mark
 * @date 2022年4月14日
 */
@ACommander(comment = "杰出者系统", name = "grave")
public class CmdGrave extends UsCmdBase
{
    @ACommand(comment = "增加新晋杰出者[称号ID，玩家CID，结束时间（秒）]")
    public String addNewPlayer(long _titleId, long _cid, int _endTs)
    {
    	long endMs = CommonFunc.getNowTimeMS() + _endTs * 1000;
    	
    	GraveObj_NewInfo newInfo = new GraveObj_NewInfo();
    	newInfo.setTitleId(_titleId);
    	newInfo.setCid(_cid);
    	
    	getUserServer().getGraveMgr().addGraveNewInfo(newInfo, endMs);
    	
        return "ok";
    }
    
    @ACommand(comment = "增加杰出者[称号ID，玩家CID]")
    public String addPlayer(long _titleId, long _cid)
    {
    	GraveObj_NewInfo newInfo = new GraveObj_NewInfo();
    	newInfo.setTitleId(_titleId);
    	newInfo.setCid(_cid);
    	
    	getUserServer().getGraveMgr().getGraveRecordMgr().addRecord(newInfo);
    	
        return "ok";
    }
}
