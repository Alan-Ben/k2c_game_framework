using ALPackage;
using NPEnum;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 客户端特殊判断条件
    /// </summary>
    public class NPPlayerCondition_C_SPECIAL : _ANPBasicPlayerCondition
	{
	    public override ENPPlayerConditionType conditionType { get { return ENPPlayerConditionType.C_SPECIAL; } }

	    private EClientSpecialConditionType _m_ePlayerCSSpecialType;

	    public EClientSpecialConditionType specialType { get { return _m_ePlayerCSSpecialType; } }

	    /// <summary>
	    /// 读取条件信息
	    /// </summary>
	    /// <param name="_reader"></param>
	    /// <returns></returns>
	    public static NPPlayerCondition_C_SPECIAL readStr(ALStringReader _reader)
	    {
	        string specialS = _reader.readItem(':');
	        if (null == specialS)
	        {
	            UnityEngine.Debug.LogError("Can not read str for NPPlayerCondition_C_SPECIAL[" + _reader.srcString + "]");
	            return null;
	        }

	        NPPlayerCondition_C_SPECIAL cond = new NPPlayerCondition_C_SPECIAL();
        
	        cond._m_ePlayerCSSpecialType = (EClientSpecialConditionType)ALPackage.ALCommon.EnumParse(typeof(EClientSpecialConditionType), specialS, true);

	        return cond;
	    }
	    public override bool isEnable(NPVarInfo _varVariableInfo)
	    {
#if NP_GAME
	        //根据不同类型进行处理
	        switch(_m_ePlayerCSSpecialType)
	        {
                //联盟协作奖励据点是否可以领取奖励
                case EClientSpecialConditionType.GUILD_COOPERATE_REWARD_POS_CAN_GET_REWARD:
					bool guildCooperateRewardPosCanGetReward = false;
                    GRefdataCoreMgr.instance.guildCooperateAreaRefCore.dealAllRef(_areaRef =>
                    {
						if(!guildCooperateRewardPosCanGetReward && _areaRef != null)
							guildCooperateRewardPosCanGetReward = NPPlayer.instance.guildCooperateComp.getAreaState(_areaRef.area_id) == EGuildCooperateMapAreaState.UNLOCK_HAVE_REWARD;
                    });
                    return guildCooperateRewardPosCanGetReward;
                //联盟协作属性据点是否可以建造
                case EClientSpecialConditionType.GUILD_COOPERATE_ATTR_POS_CAN_CONSTRUCT:
					bool guildCooperateAttrPosCanConstruct = false;
                    GRefdataCoreMgr.instance.guildCooperateAreaRefCore.dealAllRef(_areaRef =>
                    {
                        if (_areaRef == null)
                            return;

                        //判断区域是否解锁了
                        if (NPPlayer.instance.guildCooperateComp.getAreaState(_areaRef.area_id) == EGuildCooperateMapAreaState.LOCK)
                            return;

                        //判断是否有可以建造的属性据点
                        List<GuildCooperateRewardPointInfo> rewardPointList = NPPlayer.instance.guildCooperateComp.getRewardPointList(_areaRef.area_id);
                        if (rewardPointList == null)
                            return;

                        for (int i = 0; i < rewardPointList.Count; i++)
                        {
                            GuildCooperateRewardPointInfo rewardPointInfo = rewardPointList[i];
							if(rewardPointInfo == null) 
                                continue;

							//如果奖励据点还不能领取奖励，说明还可以建造
                            if (!guildCooperateAttrPosCanConstruct && rewardPointInfo.getRewardType() == ECommonRewardType.NOT_GET_REWARD)
                                guildCooperateAttrPosCanConstruct = true;
                        }
                    });
                    return guildCooperateAttrPosCanConstruct;
                //联盟协作当前显示区域是否可以建造
                case EClientSpecialConditionType.GUILD_COOPERATE_CUR_SHOW_AREA_CAN_CONSTRUCT:
                    if (!GGUIWndGuildCooperateMain.instance.isLoaded || !GGUIWndGuildCooperateMain.instance.isShow)
                        return false;
                    return GGUIWndGuildCooperateMain.instance.curShowAreaCanConstruct();
                default:
	                return false;
	        }
#else
	        return false;
#endif
	    }
	}
}