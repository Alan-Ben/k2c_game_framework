namespace GOE
{
	/// <summary>
	/// 客户端string处理效果枚举
	/// </summary>
	public enum EClientStringDealType
	{
		NONE, //0 ==== 
        FORCE_CLOSE_NODE_BY_TAG, //1 ==== 通过node标记强制关闭node
        SPACE_FORCE_PLAY_PLAYER_ANI, //2 ==== 设置大地图玩家行为动画 动画名称
        ENABLE_DIALOG_BOX, //3 ==== 显示或者隐藏对话里面的对话框，程序是控制对话框的加载父节点是activity实现，参数true/false
        SIMULATE_CLICK_CLOTHES_EDITOR_WND_BTN, //4 ==== 模拟点击衣柜编辑界面按钮
        CITY_HIDE_UI, //5 ==== 主城界面隐藏UI，参数时间秒（-1表示一直隐藏）
        ROOM_HIDE_UI, //6 ==== 卧室界面隐藏UI，参数时间秒（-1表示一直隐藏）
        SWITCH_HERO_INFO_TAB, //7 ==== 切换伙伴信息界面页签，参数：INFO、BUSINESS、TALENT、STAR、HALO、BLESS
        SWITCH_CONSORT_MAIN_TAB, //8 ==== 切换情人主界面页签，参数：CHAT、MOMENTS、CONSORT_LIST
        SIMULATE_CLICK_CITY_MAIN_EXPAND_BAR, //9 ==== 模拟点击主城主界面扩展栏按钮，参数：TRUE（展开）/FALSE（收起）
    }
}

