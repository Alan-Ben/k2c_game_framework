package USDB.Update;

import ALMySqlCommon.ALMySqlDBConditionObj;
import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlDBExcutor;
import NPCommon.DB.ErrDealer.SelectExceptionDealer;
import NPCommon.DB.UpdateBase;
import NPCommon.DB.WCGDBObj;
import NPCommon.Log.CommLog;
import USDB.Bo.PlayerMarsGoRouteBO;

import java.util.ArrayList;
import java.util.HashMap;

public class Update_1_0_0_1_To_1_0_1_0 extends UpdateBase 
{
	@SuppressWarnings("unchecked")
	@Override
	public boolean run(WCGDBObj _dbObj) 
	{
		//GOB-6987 【优化-1】主线任务任务ID已修改 需要做新旧兼容
		//https://www.teambition.com/task/69200e3edfc8a701b7e8b652
		//备注：当前 player_quest 只有主线任务数据，无其他任务数据
        if (!ALMySqlDBExcutor.execute("UPDATE player_quest SET quest_id = 1,quest_step = FLOOR(quest_step / 100);", _dbObj))
        {
            CommLog.error("Update_1_0_0_1_To_1_0_1_0 run failed at update player_quest quest_id and quest_step");
            return false;
        }

        if (!ALMySqlDBExcutor.execute("UPDATE player_quest_target SET quest_id = 1,quest_step = FLOOR(quest_step / 100),quest_target = FLOOR(quest_target / 100);", _dbObj))
        {
            CommLog.error("Update_1_0_0_1_To_1_0_1_0 run failed at update player_quest_target quest_id, quest_step and quest_target");
            return false;
        }

    	//需要清空已完成任务数据
    	String sql1 ="TRUNCATE `player_quest_count`;";
    	if(!ALMySqlDBExcutor.execute(sql1, _dbObj))
    		return false;
    	
    	//前往火星数据迁移调整
    	//数据表：player_mars_go_route，保留stage=1的数据，更新stage字段到记录最大值
    	//1. 获取来源数据并进行整理，记录玩家最大的stage
    	HashMap<Long, Integer> playerMarsGoRouteMap = new HashMap<>();
    	ArrayList<PlayerMarsGoRouteBO> playerMarsGoRouteStage1List = new ArrayList<>();
    	SelectExceptionDealer errDealer = new SelectExceptionDealer();
    	PlayerMarsGoRouteBO playerMarsGoRouteBo = new PlayerMarsGoRouteBO();
    	ArrayList<PlayerMarsGoRouteBO> marGoRouteBoList = (ArrayList<PlayerMarsGoRouteBO>) ALMySqlDBExcutor.getListByCondition(_dbObj, new ALMySqlDBConditionObj(), playerMarsGoRouteBo, errDealer);
        if (errDealer.isHasException())
        {
            return false;
        }
        for(int i = 0; i < marGoRouteBoList.size(); i++)
        {
        	PlayerMarsGoRouteBO bo = marGoRouteBoList.get(i);
        	if(null == bo)
        		continue;
        	
        	if(bo.getStage() == 1)
        	{
        		playerMarsGoRouteStage1List.add(bo);
        	}
        	
        	int stage = playerMarsGoRouteMap.computeIfAbsent(bo.getCid(), k -> bo.getStage());
        	int maxStage = Math.max(stage, bo.getStage());
        	playerMarsGoRouteMap.put(bo.getCid(), maxStage);
        }
        //2. 移除stage>1的玩家数据
        String sql_2 ="DELETE FROM `player_mars_go_route` WHERE `stage` > 1;";
    	if(!ALMySqlDBExcutor.execute(sql_2, _dbObj))
    		return false;
        //3. 更新stage=1的玩家数据
        for(int i = 0; i < playerMarsGoRouteStage1List.size(); i++)
        {
        	PlayerMarsGoRouteBO bo = playerMarsGoRouteStage1List.get(i);
        	if(null == bo)
        		continue;
        	
    		int maxStage = playerMarsGoRouteMap.get(bo.getCid());
    		if(maxStage > 1)
    		{
    			String sql_3 = "UPDATE `player_mars_go_route` SET `stage` = " + maxStage + " WHERE `id`=" + bo.getId() + ";";
    			if(!ALMySqlDBExcutor.execute(sql_3, _dbObj))
    	    		return false;
    		}
        }
    	
		return true;
	}
}