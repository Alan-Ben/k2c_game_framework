package NPUSServer.Guild.GuildDungeon;

import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;

/**
 * 攻击怪物的结果数据
 * @author mj
 *
 */
public class GuildDungeonAttackResult 
{
	//结果
	public Result result = Result.failed(CommErr.UNKNOW_ERR.getCode());
	//伤害
	public long damge = 0;
	//是否击杀
	public boolean isKilled = false;
	//是否完成
	public boolean isDone = false;
}
