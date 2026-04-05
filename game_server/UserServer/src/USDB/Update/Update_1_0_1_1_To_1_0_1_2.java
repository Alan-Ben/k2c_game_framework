package USDB.Update;

import ALMySqlCommon.ALMySqlDBConditionObj;
import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlDBExcutor;
import ALServerLog.ALServerLog;
import NPCommon.DB.ErrDealer.SelectExceptionDealer;
import NPCommon.DB.UpdateBase;
import NPCommon.DB.WCGDBObj;
import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPUSServer.GeneralV.UsID;
import NPUSServer.UserServerConf;
import USDB.Bo.GuildBO;
import USDB.Bo.PlayerBO;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.Map;

/**
 * GOB-8196 联盟ID生成规则修正
 * https://www.teambition.com/task/69524214ad5b54ea3ce942db
 * 
 * 特别说明：
 		1. 只有欧洲区数据，美洲区尚未开服，无联盟数据，无需处理（大区ID固定为1）
 		2. 只有本服数据，无需考虑跨服数据处理
 * 
 * 处理方案：
 		1. 取出最后一条player玩家数据，用于解析usId
 		1. 获取所有的联盟数据，整理需要转换的联盟ID
 		2. 遍历转换联盟ID列表，使用update修改对应的数据
 * 
 * @author mj
 *
 */
public class Update_1_0_1_1_To_1_0_1_2 extends UpdateBase
{
	@SuppressWarnings({ "unchecked", "rawtypes" })
	@Override
	public boolean run(WCGDBObj _dbObj)
	{
		//取出最后一条player数据，用于获取当前的USID
		PlayerBO playerBo = new PlayerBO();
		SelectExceptionDealer playerBoErrDealer = new SelectExceptionDealer();
		ArrayList<PlayerBO> playerBoList = (ArrayList)ALMySqlDBExcutor.executeQuery("SELECT * FROM `player` ORDER BY `id` DESC LIMIT 1;", _dbObj, playerBo, playerBoErrDealer);
		if (playerBoErrDealer.isHasException())
        {
            return false;
        }
		//没有玩家记录，必然没有联盟记录，直接返回成功
		if(playerBoList.isEmpty())
		{
			ALServerLog.Info("Update_1_0_1_1_To_1_0_1_2 not player bo.");
			return true;
		}
		//通过解析cid获取对应的usId
		long cid = ((PlayerBO)playerBoList.get(0)).getCid();
		if(cid <= 0)
		{
			ALServerLog.Error("Update_1_0_1_1_To_1_0_1_2 not player cid error.");
			return false;
		}
		
		int usId = CommonFunc.parseServerTypeIdFromCid(cid);
		if(usId <= 0)
		{
			CommLog.error("Update_1_0_1_1_To_1_0_1_2 parse cid:{} error, usId fail.", cid);
			return false;
		}
		
		CommLog.info("Update_1_0_1_1_To_1_0_1_2 parse cid:{} get usId:{}", cid, usId);
		
		//新旧联盟ID映射关系
		HashMap<Long, Long> guildIdTransMap = new HashMap<>();
		//遍历所有联盟数据列表
		SelectExceptionDealer guildBoErrDealer = new SelectExceptionDealer();
		GuildBO guildBo = new GuildBO();
		ArrayList<GuildBO> guildBoList = (ArrayList<GuildBO>) ALMySqlDBExcutor.getListByCondition(_dbObj, new ALMySqlDBConditionObj(), guildBo, guildBoErrDealer);
		if (guildBoErrDealer.isHasException())
        {
            return false;
        }
		for(int i = 0; i < guildBoList.size(); i++)
		{
			GuildBO bo = guildBoList.get(i);
			if(null == bo)
				continue;
			
			long preGuildId = bo.getGuildId();
			long newGuildId = transNewId(usId, preGuildId);
			if(newGuildId <= 0)
			{
				CommLog.error("Update_1_0_1_1_To_1_0_1_2 trans guild error, preGuildId:{}.", preGuildId);
				return false;
			}
			
			guildIdTransMap.put(preGuildId, newGuildId);
			CommLog.info("Update_1_0_1_1_To_1_0_1_2 trans guild suc, preGuildId:{} newGuildId:{}.", preGuildId, newGuildId);
		}
		CommLog.info("Update_1_0_1_1_To_1_0_1_2 trans guild size:{}.", guildIdTransMap.size());
		//更新数据
		for(Map.Entry<Long, Long> entry : guildIdTransMap.entrySet()) 
		{
			if(null == entry)
				continue;
			
			//联盟基础数据
			String sql1 ="UPDATE `guild` SET `guild_id` = '" + entry.getValue() + "' WHERE `guild_id` = '" + entry.getKey() + "';";
	    	if(!ALMySqlDBExcutor.execute(sql1, _dbObj))
	    		return false;
	    	
	    	//联盟活跃宝箱
			String sql2 ="UPDATE `guild_active_box` SET `guild_id` = '" + entry.getValue() + "' WHERE `guild_id` = '" + entry.getKey() + "';";
	    	if(!ALMySqlDBExcutor.execute(sql2, _dbObj))
	    		return false;
	    	
	    	//联盟活跃宝箱
			String sql3 ="UPDATE `guild_box` SET `guild_id` = '" + entry.getValue() + "' WHERE `guild_id` = '" + entry.getKey() + "';";
	    	if(!ALMySqlDBExcutor.execute(sql3, _dbObj))
	    		return false;
	    	
	    	//联盟协作攻击日志表
			String sql4 ="UPDATE `guild_cooperate_attack_log` SET `guild_id` = '" + entry.getValue() + "' WHERE `guild_id` = '" + entry.getKey() + "';";
	    	if(!ALMySqlDBExcutor.execute(sql4, _dbObj))
	    		return false;
	    	
	    	//联盟副本数据
			String sql6 ="UPDATE `guild_dungeon_monster` SET `guildId` = '" + entry.getValue() + "' WHERE `guildId` = '" + entry.getKey() + "';";
	    	if(!ALMySqlDBExcutor.execute(sql6, _dbObj))
	    		return false;
	    	
	    	//联盟副本配置数据
			String sql7 ="UPDATE `guild_dungeon_set` SET `guildId` = '" + entry.getValue() + "' WHERE `guildId` = '" + entry.getKey() + "';";
	    	if(!ALMySqlDBExcutor.execute(sql7, _dbObj))
	    		return false;
	    	
	    	//联盟副本数据
			String sql8 ="UPDATE `guild_dungeon_log` SET `guildId` = '" + entry.getValue() + "' WHERE `guildId` = '" + entry.getKey() + "';";
	    	if(!ALMySqlDBExcutor.execute(sql8, _dbObj))
	    		return false;
	    	
	    	//联盟副本实例数据
			String sql9 ="UPDATE `guild_dungeon_instance` SET `guildId` = '" + entry.getValue() + "' WHERE `guildId` = '" + entry.getKey() + "';";
	    	if(!ALMySqlDBExcutor.execute(sql9, _dbObj))
	    		return false;
	    	
	    	//联盟副本全局配置数据
			String sql10 ="UPDATE `guild_dungeon_global_set` SET `guildId` = '" + entry.getValue() + "' WHERE `guildId` = '" + entry.getKey() + "';";
	    	if(!ALMySqlDBExcutor.execute(sql10, _dbObj))
	    		return false;
	    	
	    	//联盟副本伤害排行数据
			String sql11 ="UPDATE `guild_dungeon_damage_rank` SET `guildId` = '" + entry.getValue() + "' WHERE `guildId` = '" + entry.getKey() + "';";
	    	if(!ALMySqlDBExcutor.execute(sql11, _dbObj))
	    		return false;
	    	
	    	//联盟解散数据
			String sql12 ="UPDATE `guild_dissolve` SET `guild_id` = '" + entry.getValue() + "' WHERE `guild_id` = '" + entry.getKey() + "';";
	    	if(!ALMySqlDBExcutor.execute(sql12, _dbObj))
	    		return false;
	    	
	    	//公会协作据点信息表
			String sql13 ="UPDATE `guild_cooperate_point_info` SET `guild_id` = '" + entry.getValue() + "' WHERE `guild_id` = '" + entry.getKey() + "';";
	    	if(!ALMySqlDBExcutor.execute(sql13, _dbObj))
	    		return false;
	    	
	    	//公会协作主表
			String sql14 ="UPDATE `guild_cooperate_main` SET `guild_id` = '" + entry.getValue() + "' WHERE `guild_id` = '" + entry.getKey() + "';";
	    	if(!ALMySqlDBExcutor.execute(sql14, _dbObj))
	    		return false;
	    	
	    	//联盟协作伤害排行榜
			String sql15 ="UPDATE `guild_cooperate_damage_rank` SET `guild_id` = '" + entry.getValue() + "' WHERE `guild_id` = '" + entry.getKey() + "';";
	    	if(!ALMySqlDBExcutor.execute(sql15, _dbObj))
	    		return false;
	    	
	    	//联盟事件数据
			String sql16 ="UPDATE `guild_event` SET `guild_id` = '" + entry.getValue() + "' WHERE `guild_id` = '" + entry.getKey() + "';";
	    	if(!ALMySqlDBExcutor.execute(sql16, _dbObj))
	    		return false;
	    	
	    	//联盟大礼
			String sql17 ="UPDATE `guild_great_reward` SET `guild_id` = '" + entry.getValue() + "' WHERE `guild_id` = '" + entry.getKey() + "';";
	    	if(!ALMySqlDBExcutor.execute(sql17, _dbObj))
	    		return false;
	    	
	    	//联盟大臣派遣
			String sql18 ="UPDATE `guild_hero_dispatch` SET `guild_id` = '" + entry.getValue() + "' WHERE `guild_id` = '" + entry.getKey() + "';";
	    	if(!ALMySqlDBExcutor.execute(sql18, _dbObj))
	    		return false;
	    	
	    	//联盟入盟请求数据
			String sql19 ="UPDATE `guild_join_request` SET `guild_id` = '" + entry.getValue() + "' WHERE `guild_id` = '" + entry.getKey() + "';";
	    	if(!ALMySqlDBExcutor.execute(sql19, _dbObj))
	    		return false;
	    	
	    	//联盟日志数据
			String sql20 ="UPDATE `guild_log` SET `guild_id` = '" + entry.getValue() + "' WHERE `guild_id` = '" + entry.getKey() + "';";
	    	if(!ALMySqlDBExcutor.execute(sql20, _dbObj))
	    		return false;
	    	
	    	//联盟成员在火星系统的互助
			String sql21 ="UPDATE `guild_mars_help` SET `guild_id` = '" + entry.getValue() + "' WHERE `guild_id` = '" + entry.getKey() + "';";
	    	if(!ALMySqlDBExcutor.execute(sql21, _dbObj))
	    		return false;
	    	
	    	//联盟分享矿数据
			String sql22 ="UPDATE `guild_mars_mine_share` SET `guild_id` = '" + entry.getValue() + "' WHERE `guild_id` = '" + entry.getKey() + "';";
	    	if(!ALMySqlDBExcutor.execute(sql22, _dbObj))
	    		return false;
	    	
	    	//联盟成员数据
			String sql23 ="UPDATE `guild_member` SET `guild_id` = '" + entry.getValue() + "' WHERE `guild_id` = '" + entry.getKey() + "';";
	    	if(!ALMySqlDBExcutor.execute(sql23, _dbObj))
	    		return false;
	    	
	    	//本服火星矿产数据
			String sql24 ="UPDATE `us_mars_mine` SET `guild_id` = '" + entry.getValue() + "' WHERE `guild_id` = '" + entry.getKey() + "';";
	    	if(!ALMySqlDBExcutor.execute(sql24, _dbObj))
	    		return false;
	    	
	    	//排行榜子数据
			String sql25 ="UPDATE `us_rank_sub_obj` SET `objId` = '" + entry.getValue() + "' WHERE `objId` = '" + entry.getKey() + "';";
	    	if(!ALMySqlDBExcutor.execute(sql25, _dbObj))
	    		return false;
		}

        //设置已完成标记，如果已存在则更新
        String sql25 ="DELETE FROM `us_param` WHERE `tag` = 16;";
        if(!ALMySqlDBExcutor.execute(sql25, _dbObj))
            return false;

        String sql26 ="INSERT INTO `us_param` (`tag`, `param`) VALUES (16, 1);";
        if(!ALMySqlDBExcutor.execute(sql26, _dbObj))
            return false;

		return true;
	}
	
	/**
	 * 转换新ID
	 * @param _usId
	 * @param _preId
	 */
	public static long transNewId(int _usId, long _preId)
	{
		//自增ID，原来组装方案 1 * 10000 + 10001 = 20001，2 * 10000 + 1 = 20001
		long autoId = (_preId - _usId) / 10000;
		if(autoId <= 0)
			return 0;
		
		//用新规则生成
        return UsID.makeUsId(UserServerConf.getInstance().getPlatAreaId(), _usId, autoId);
	}
}
