
package NPUSServer.GMCommand.Cmds;


import Common.MarsEnum.EMarsExploreEventType;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.ErrMain.Result.Result;
import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPEnum.ENPPlayerRecordParam;
import NPEnum.EQuality;
import NPGameRes.Refs.Mars.*;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.GMCommand.UsCmdBase;
import NPUSServer.GeneralV.UsID;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.NPUSUserMgr.GameSystem.MarsMineSystem.MarsMineSystem;
import NPUSServer.NPUSUserMgr.UserComp.MarsComp.MarsCalculate;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep2_BuildingComp.MarsBuildingInfo;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep2_BuildingComp.MarsPeopleBuildingFunc;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep2_BuildingComp._AMarsBuildingFunc;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep3_PeopleComp.MarsPeopleIntelligentInfo;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreTeam.MarsExploreTeam;
import NPUSServer.USLog;

import java.util.ArrayList;

@ACommander(comment = "火星相关命令", name = "mars")
public class CmdMars extends UsCmdBase
{
    @ACommand(comment = "设置当前阶段[阶段]")
    public String cmdSetStage(int _stage)
    {
    	RefMarsGoRoute ref = RefGeneral.Ref().getMarsGoRouteStageMapMgr().getLevelData(_stage);
    	if(null == ref)
    		return "fail, not find ref.";
    	
    	getOwner().getMarsGoRouteComponent().cmdSetStage(_stage, getContext());
    	return "ok";
    }
    
    @ACommand(comment = "设置前往火星的开启时间[以之前开始时间为基础的偏移秒数]")
    public String cmdSetStartTime(int _offsecs)
    {
    	getOwner().getMarsGoRouteComponent().cmdSetStartMs(_offsecs, getContext());
    	return "ok";
    }

    @ACommand(comment = "设置完成前往火星")
    public String cmdSetDone()
    {
    	getOwner().getMarsGoRouteComponent().setAllDone(getContext());
    	return "ok";
    }

    @ACommand(comment = "设置满意度[数值]")
    public String setSatisfaction(int _num)
    {
    	getOwner().getMarsPeopleComponent().setSatisfaction(_num, getContext());
    	
        return "ok";
    }

    @ACommand(comment = "设置健康值[数值]")
    public String setHealth(int _num)
    {
    	getOwner().getMarsBuildingComponent().setHealthIndex(_num);
    	
        return "ok";
    }

    @ACommand(comment = "设置幸福值[数值]")
    public String setHappy(int _num)
    {
    	getOwner().getMarsBuildingComponent().setHappyIndex(_num);
    	
        return "ok";
    }

    @ACommand(comment = "计算事件随机值[事件ID，健康值，幸福值]")
    public String getEventPer(long _eventId, long _health, long _happy)
    {
    	RefMarsEvent ref = RefMarsEvent.getMgr().get(_eventId);
    	if(null == ref)
    		return "fail";
    	
    	int per = ref.getPerByValue(_health, _happy);
    	
        return "per:" + per;
    }
    
    @ACommand(comment = "补充信件数据[补充数量]")
    public String buildLetterList(int _count)
    {
    	getOwner().getMarsPeopleComponent().getLetterMgr().buildLetterList(_count, getContext());
    	
        return "ok";
    }
    
    @ACommand(comment = "补充求助数据")
    public String buildHelp()
    {
    	getOwner().getMarsPeopleComponent().getHelpMgr().buildHelp(getContext());
    	
        return "ok";
    }
    
    @ACommand(comment = "创建求助数据[求助ID]")
    public String createHelp(long _helpId)
    {
    	RefMarsPeopleHelp ref = RefMarsPeopleHelp.getMgr().get(_helpId);
    	if(null == ref)
    		return "fail, not find ref.";
    	
    	getOwner().getMarsPeopleComponent().getHelpMgr().createHelp(ref, getContext());
    	
        return "ok";
    }
    
    @ACommand(comment = "处理事件")
    public String dealEvent()
    {
    	getOwner().getMarsPeopleComponent().getEventMgr().dealEvent(getContext());
    	
        return "ok";
    }

    @ACommand(comment = "处理指定事件[事件配置ID]")
    public String dealAEvent(long _eventId)
    {
    	getOwner().getMarsPeopleComponent().getEventMgr().dealEvent(_eventId, getContext());
    	
        return "ok";
    }
    
    @ACommand(comment = "清空事件计数")
    public String clearDailyEventCount()
    {
    	getOwner().getMarsPeopleComponent().getEventMgr().clearDailyEvent();
    	
        return "ok";
    }
    
    @ACommand(comment = "设置指定AI决策截至时间[AI决策配置ID，偏移秒数]")
    public String SetIntelligentEndMs(long _refId, int _secs)
    {
    	MarsPeopleIntelligentInfo info = getOwner().getMarsPeopleComponent().getIntelligentMgr().lookup(_refId);
    	if(null == info)
    		return "fail, not find.";
    	
    	info.cmdSetEndMs(_secs, getContext());
    	
        return "ok";
    }
    
    @ACommand(comment = "展示所有建筑数据")
    public String showAllBuilding()
    {
    	return getOwner().getMarsBuildingComponent().toString();
    }

    @ACommand(comment = "展示火星实力")
    public String showMarsPower()
    {
    	StringBuilder sb = new StringBuilder();
    	
    	sb.append("\n------------------ mars power max power ------------------");
    	sb.append("\nmaxPower:").append(getOwner().getRecordComponent().getRecordCount(ENPPlayerRecordParam.MARS_MAX_POWER));
    	sb.append("\nmaxTeamPower:").append(getOwner().getRecordComponent().getRecordCount(ENPPlayerRecordParam.MARS_TEAM_MAX_POWER));
    	
    	MarsCalculate.calMarsPower(getOwner(), sb);
    	
    	return sb.toString();
    }

    @ACommand(comment = "展示火星探索队伍")
    public String showMarsTeam()
    {
    	return getOwner().getMarsExploreComponent().getTeamMgr().toString();
    }
    
    @ACommand(comment = "展示派遣建筑计算[建筑ID]")
    public String showPeopleBuilding(long _buildingId)
    {
    	MarsBuildingInfo info = getOwner().getMarsBuildingComponent().lookupBuilding(_buildingId);
    	if(null == info)
    		return "fail, not find.";
    	
    	ArrayList<_AMarsBuildingFunc> funcList = info.getFuncList();
    	for(int i = 0; i < funcList.size(); i++)
    	{
    		if(funcList.get(0) instanceof MarsPeopleBuildingFunc)
    		{
    			MarsPeopleBuildingFunc peopleFunc = (MarsPeopleBuildingFunc) funcList.get(0);
    			return peopleFunc.showEnergyOutputValuePerMin(getContext());
    		}
    	}
    	
    	return "fail, building not people func.";
    }

    @ACommand(comment = "设置空闲居民数量[数值]")
    public String setIdlePeople(int _num)
    {
    	getOwner().getMarsPeopleComponent().getNumInfo().setIdleNum(_num, getContext());
    	
        return "ok";
    }

    @ACommand(comment = "设置生病居民数量[数值]")
    public String setSickPeople(int _num)
    {
    	getOwner().getMarsPeopleComponent().getNumInfo().setSickNum(_num, getContext());
    	
        return "ok";
    }

    @ACommand(comment = "设置移民数据[结束偏移时间（秒），移民偏移数量]")
    public String setImmigrant(int _offsecs, int _offNum)
    {
    	getOwner().getMarsPeopleComponent().getImmigrantInfo().cmdSetImmigrant(_offsecs, _offNum, getContext());
    	
        return "ok";
    }

    @ACommand(comment = "设置移民次数[当天使用次数]")
    public String setImmigrantCount(int _count)
    {
    	getOwner().getMarsPeopleComponent().getImmigrantInfo().cmdSetImmigrantCount(_count, getContext());
    	
        return "ok";
    }

    @ACommand(comment = "刷新探索事件[数量]")
    public String refreshEvents(int _num)
    {
    	getOwner().getMarsExploreComponent().getEventMgr().addRefreshEvent(_num, getContext());
    	
        return "ok";
    }
    
    @ACommand(comment = "设置探索次数[次数]")
    public String setExploreSum(int _value)
    {
    	getOwner().getMarsExploreComponent().getExploreInfo().setExploreNum(_value, getContext());
    	
        return "ok";
    }

    @ACommand(comment = "创建探索事件[事件类型，事件ID，位置ID，品质]")
    public String buildEvent(EMarsExploreEventType _eventType, long _eventId, long _pos, EQuality _quanlity)
    {
    	RefMarsExploreLvl exploreLvlRef = getOwner().getMarsExploreComponent().getExploreInfo().getLvlRef();
		if(null == exploreLvlRef)
			return "fail, not find explore lvl.";
		
    	if(_pos <= 0)
    	{
			RefMarsExplorePos posRef = exploreLvlRef.randPosRef(getOwner().getMarsExploreComponent().getEventMgr().getUsedPosSet());
			if(null == posRef)
				return "fail, not find can use pos.";
			
			_pos = posRef.id;
    	}
    	
    	if(EQuality.NONE == _quanlity)
    	{
    		_quanlity = exploreLvlRef.refresh_event_quality_list.random();
    		if(null == _quanlity)
    			return "fail, not find quality.";
    	}
    	
    	getOwner().getMarsExploreComponent().getEventMgr().buildEvent(_eventId, _eventType, exploreLvlRef.explore_level, _quanlity, _pos, getContext());
    	
        return "ok";
    }

    @ACommand(comment = "设置Food建筑开启[建筑ID，开关]")
    public String setFoodPowerOn(long _buildingId, boolean _isPowerOn)
    {
    	MarsBuildingInfo info = getOwner().getMarsBuildingComponent().lookupBuilding(_buildingId);
    	if(null == info)
    		return "fail, not find building.";
    	
    	info.setFoodPower(_isPowerOn, getContext());
    	
    	return "ok";
    }
    
    @ACommand(comment = "刷新队伍数据[队伍ID]")
    public String refreshTeam(long _teamId)
    {
    	MarsExploreTeam team = getOwner().getMarsExploreComponent().getTeamMgr().lookup(_teamId);
    	if(null == team)
    		return "fail, not find team.";
    	
    	team.cmdRefreshState();
    	
        return "ok";
    }
    
    @ACommand(comment = "设置队伍空闲[队伍ID]")
    public String setTeamIdle(long _teamId)
    {
    	MarsExploreTeam team = getOwner().getMarsExploreComponent().getTeamMgr().lookup(_teamId);
    	if(null == team)
    		return "fail, not find team.";
    	
    	team.setIdleStatus(getContext());
    	
        return "ok";
    }
    
    @ACommand(comment = "设置队伍损耗数量[队伍ID，损耗数量]")
    public String setTeamLossValue(long _teamId, long _lossValue)
    {
    	MarsExploreTeam team = getOwner().getMarsExploreComponent().getTeamMgr().lookup(_teamId);
    	if(null == team)
    		return "fail, not find team.";
    	
    	team.cmdSetLossValue(_lossValue, getContext());
    	
        return "ok";
    }
    
    @ACommand(comment = "随机刷出火星矿")
    public String buildRandMine()
    {
    	getOwner().getMarsMineComponent().buildRandMine(getContext());
    	
        return "ok";
    }
    
    @ACommand(comment = "执行检查火星建筑解锁部件[建筑ID]")
    public String checkUnlockEquip(long _buildingId)
    {
    	MarsBuildingInfo info = getOwner().getMarsBuildingComponent().lookupBuilding(_buildingId);
    	if(null == info)
    		return "fail, not find.";
    	
    	info.checkUnlockEquipment(false, getContext());
    	
        return "ok";
    }
    
    @ACommand(comment = "随机刷出火星矿（从已有的火星池中刷取）")
    public void buildRandMineFromPool()
    {
    	RefMarsExploreLvl exploreLvlRef = getOwner().getMarsExploreComponent().getExploreInfo().getLvlRef();
		if(null == exploreLvlRef)
		{
			takeCallBack().onRunOver(false, "can not get explore lvl.");
			return;
		}

    	RefMarsExploreLvl.MarsExploreMineRndInfo mineLvl = exploreLvlRef.mineIdList.random();
		if(mineLvl.mineLvl <= 0)
		{
			takeCallBack().onRunOver(false, "can not refresh mine by explore lvl.");
			return;
		}

        RefMarsExploreMine mineRef = RefGeneral.Ref().randMarsLvlMineMap(mineLvl.mineLvl);
        if(null == mineRef)
        {
            USLog.error(getOwner().getUSServer(), "player:{} explore lvl:{} mineLvl:{} mars explore not find mine ref."
                    , getOwner().getCid(), exploreLvlRef.explore_level, mineLvl);
            return;
        }

         //生成一个已存在的矿给玩家，如果数量低于阈值会直接生成新矿
    	MarsMineSystem.RandOtherPlayerMine(getOwner().getUSServer(), mineRef, (_err, _mineObj) ->
		{
            //调用事件处理
            getOwner().getMarsMineComponent().onMineCreated(_mineObj, getContext());

			if(null != _mineObj) //使用池子里的数据
			{
				takeCallBack().onRunOver(true, "ok");
			}
			else //池子没有数据
			{
				takeCallBack().onRunOver(false, "mar mine pool create fail.");
			}
		});
    }

    @ACommand(comment = "刷出固定Id火星矿")
    public void buildMineFromDbId(long _mineId, int _areaId, int _usId) {
        buildMineFromId(UsID.makeUsId(_areaId, _usId, _mineId));
    }
    @ACommand(comment = "刷出固定Id火星矿")
    public void buildMineFromId(long _mineId)
    {
        //生成一个已存在的矿给玩家，如果数量低于阈值会直接生成新矿
        MarsMineSystem.GetMarsMine(getOwner().getUSServer(), _mineId, (_err, _mineObj) ->
        {
            if(null != _mineObj) //使用池子里的数据
            {
                //调用事件处理
                getOwner().getMarsMineComponent().onMineCreated(_mineObj, getContext());

                takeCallBack().onRunOver(true, "ok");
            }
            else //池子没有数据
            {
                takeCallBack().onRunOver(false, "mar mine pool create fail.");
            }
        });
    }

    @ACommand(comment = "展示探险品质奖励")
    public String showExploreQualityReward(int _quality)
    {
    	RefMarsExploreLvl exploreLvlRef = getOwner().getMarsExploreComponent().getExploreInfo().getLvlRef();
		if(null == exploreLvlRef)
		{
			return "can not get explore lvl.";
		}

    	StringBuilder sb = new StringBuilder();
		sb.append("\nexplorerLvl:").append(exploreLvlRef.explore_level);
		sb.append("\tquality:").append(_quality);
		
    	ArrayList<NPCommonCostItem> itemList = exploreLvlRef.getQualityRewardItemList(_quality);
    	if(null == itemList)
    	{
    		sb.append("\tnot item");
    		return sb.toString();
    	}
    	
    	for(int i = 0; i < itemList.size(); i++)
    	{
    		NPCommonCostItem item = itemList	.get(i);
    		if(null == item)
    			continue;
    		
    		sb.append("\t").append(item.toString());
    	}
    	
    	return sb.toString();
    }

    @ACommand(comment = "查看玩家火星属性")
    public String listMarsProperty()
    {
        return getOwner().getMarsComponent().getMarsPropertyMgr().toString();
    }
    
    @ACommand(comment = "设置火星建筑立即完成[建筑ID]")
    public String setBuildingDone(long _buildingId)
    {
    	MarsBuildingInfo info = getOwner().getMarsBuildingComponent().lookupBuilding(_buildingId);
    	if(null == info)
    		return "fail, not find building.";
    	
    	Result result = info.setBuildingDone(getContext());
    	return result.getCode() + ": " + result.getMsg();
    }
    
    @ACommand(comment = "完成互助[是否自动]")
    public String cmdDealHelp(boolean _isAuto)
    {
        GuildInfo guild = getUserServer().getGuildMgr().lookupGuild(getOwner().getGuildComponent().getGuildId());
        if (guild == null)
            return "not in Local guild";
    	
    	guild.getMarsHelpMgr().cmdDealHelp(getOwner().getCid(), _isAuto);
    	
    	return "ok";
    }

    @ACommand(comment = "测试移民数量[预计移民人数，测试次数]")
    public String cmdTestImmigrant(int _value, int _count)
    {
    	StringBuilder sb = new StringBuilder();
    	for(int i = 0; i < _count; i++)
    	{
    		RefGeneral.Ref().randMarsImmigrationValue(_value, sb);
    	}

    	return sb.toString();
    }

    @ACommand(comment = "设置矿的剩余资源数量[矿ID，剩余数量]")
    public void setMineRemainNum(long _mineId, long _remainNum)
    {
        //通过MarsMineSystem统一处理，支持本服和跨服矿
        MarsMineSystem.SetMineRemainNum(getOwner().getUSServer(), _mineId, _remainNum, _errCode ->
        {
            if (_errCode == 0)
            {
                takeCallBack().onRunOver(true, "ok, mineId=" + _mineId + ", remainNum=" + _remainNum);
            } else
            {
                takeCallBack().onRunOver(false, "fail, errCode=" + _errCode);
            }
        });
    }

    @ACommand(comment = "一键建造所有火星建筑")
    public String cmdBuildAllBuildings()
    {
        StringBuilder sb = new StringBuilder();
        sb.append("开始一键建造所有火星建筑...\n");

        int successCount = 0;
        int skipCount = 0;

        java.util.List<RefMarsBuilding> refList = NPGameRes.Refs.Mars.RefMarsBuilding.getMgr().getList();
        for(int i = 0; i < refList.size(); i++)
        {
            RefMarsBuilding ref = refList.get(i);
            if(null == ref)
                continue;

            MarsBuildingInfo building = getOwner().getMarsBuildingComponent().lookupBuilding(ref.id);
            if(null == building)
            {
                sb.append("建筑ID:").append(ref.id).append(" - 未找到建筑对象\n");
                skipCount++;
                continue;
            }

            //已经建造过的跳过
            if(building.isBuilt())
            {
                sb.append("建筑ID:").append(ref.id).append(" - 已建造，跳过\n");
                skipCount++;
                continue;
            }

            Result result = building.cmdGmBuild(getContext());
            if(result.isSucc())
            {
                sb.append("建筑ID:").append(ref.id).append(" - 建造成功\n");
                successCount++;
            }
            else
            {
                sb.append("建筑ID:").append(ref.id).append(" - 建造失败，错误码:").append(result.getCode()).append("\n");
                skipCount++;
            }
        }

        sb.append("\n执行完成！成功:").append(successCount).append("个，跳过:").append(skipCount).append("个");

        return sb.toString();
    }
}
