package USDB.Update;

import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlDBExcutor;
import NPCommon.DB.UpdateBase;
import NPCommon.DB.WCGDBObj;

public class Update_1_0_1_0_To_1_0_1_1 extends UpdateBase
{
	@Override
	public boolean run(WCGDBObj _dbObj)
	{
		//https://www.teambition.com/task/694e855164fce6b1a19191f8
		//https://www.teambition.com/task/693b6dceb010d21352280587

    	String sql1 ="TRUNCATE `us_mars_mine_occupy`;";
    	if(!ALMySqlDBExcutor.execute(sql1, _dbObj))
    		return false;

    	String sql2 ="TRUNCATE `us_mars_mine`;";
    	if(!ALMySqlDBExcutor.execute(sql2, _dbObj))
    		return false;

    	String sql3 ="TRUNCATE `player_mars_team_collect_result`;";
    	if(!ALMySqlDBExcutor.execute(sql3, _dbObj))
    		return false;

    	String sql4 ="UPDATE `player_mars_explore_team` SET `curState` = 2, `lossValue` = 0, `curStateStartMs` = 0, `curStateKeepTimeMS` = 0, `targetPos` = 0, `extData` = NULL;";
    	if(!ALMySqlDBExcutor.execute(sql4, _dbObj))
    		return false;

    	String sql5 ="TRUNCATE `player_mars_explore_pvp_log`;";
    	if(!ALMySqlDBExcutor.execute(sql5, _dbObj))
    		return false;

    	String sql6 ="TRUNCATE `player_mars_explore_event`;";
    	if(!ALMySqlDBExcutor.execute(sql6, _dbObj))
    		return false;

    	String sql7 ="DELETE FROM `player_lazy_cd` WHERE `cd_id` = 111;";
    	if(!ALMySqlDBExcutor.execute(sql7, _dbObj))
    		return false;

	    return true;
	}
}
