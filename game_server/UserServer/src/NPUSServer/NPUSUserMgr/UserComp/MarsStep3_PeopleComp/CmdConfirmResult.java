package NPUSServer.NPUSUserMgr.UserComp.MarsStep3_PeopleComp;

import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;

/**
 * 确认准备中居民进入休闲中
 * @author mj
 *
 */
public class CmdConfirmResult 
{
	public Result result = Result.failed(CommErr.UNKNOW_ERR.getCode());
	public long num;
}
