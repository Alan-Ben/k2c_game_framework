namespace GOE
{
    /// <summary>
    /// 客户端特殊条件类型枚举
    /// </summary>
    public enum EClientSpecialConditionType
    {
	    NONE, //0 ==== 
        GUILD_COOPERATE_REWARD_POS_CAN_GET_REWARD, //1 ==== 联盟协作奖励据点是否可以领取奖励
        GUILD_COOPERATE_ATTR_POS_CAN_CONSTRUCT, //2 ==== 联盟协作属性据点是否可以建造
        GUILD_COOPERATE_CUR_SHOW_AREA_CAN_CONSTRUCT, //3 ==== 联盟协作当前显示区域是否可以建造
    }
}
