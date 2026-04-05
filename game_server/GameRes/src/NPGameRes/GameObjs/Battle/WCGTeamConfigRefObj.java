package NPGameRes.GameObjs.Battle;

import WCGCommon.Enum.NPEnum.EWCGBattleTeamSaveType;

public class WCGTeamConfigRefObj extends _IALBasicRefObj
{
    public long _refId()
    {
        return id;
    }

    public long id;

    public int max_army_count;//上阵兵种最大数量
    public int max_hero_count;//上阵英雄最大数量
    public String name;//模式名稱
    public String desc;//关闭的卡槽位展示信息

    public EWCGBattleTeamSaveType team_save_type;//阵容保存方式

//	public void adapt(RefTeamConfig refTeamConfig)
//	{
//		 this.id = refTeamConfig.id;
//		 this.desc = refTeamConfig.desc;
//		 this.max_army_count = refTeamConfig.max_army_count;
//		 this.max_hero_count = refTeamConfig.max_hero_count;
//		 this.team_save_type = refTeamConfig.team_save_type;
//		 
//	}
}