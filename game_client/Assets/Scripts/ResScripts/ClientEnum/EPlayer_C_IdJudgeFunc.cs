namespace GOE
{
    /// <summary>
    /// 客户端针对Id判断的类型
    /// </summary>
    public enum EPlayer_C_IdJudgeFunc 
    {
	    NONE, //0 ==== 
	    IS_SYS_INFO_NEW, //1 ==== SysInfo是否新的
	    IS_TUTORIAL_DONE, //2 ==== 是否完成了对应的引导
	    IS_FUNC_UNLOCK_TIP_DONE, //3 ==== 功能解锁弹窗表现完成 id是ENPFunctionType枚举的索引
	    CUR_IS_IN_MISSION_WIN_WND, //4 ==== 当前是否在某个关卡胜利节点，mission表的id
	    IS_PLAYER_ARENA, //5 ==== 判断是否玩家所在的擂台 space_item_id
	    IS_GENDER_TYPE_JUDGE, //6 ==== 判断玩家性别：ENPGenderType枚举的索引
    }
}
