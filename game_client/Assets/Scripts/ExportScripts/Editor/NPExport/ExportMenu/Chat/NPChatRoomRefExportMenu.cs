
using GOE;
using System;


namespace GOE
{
	//第一个类型是模板类,  第二个类型是实际游戏中运用的类,  第三个类型是表的集合类
	public class NPChatRoomRefExportMenu : NPBasicAutoExportMenuItem<NPChatRoomRefObj, NPGSOChatRoomRefSet>
	{
	    public NPChatRoomRefExportMenu(Func<string, string, bool> _judgeCanShowFunc)
	        : base(ENPExportSettingEnum.CHAT_ROOM, NPGSOChatRoomRefSet.assetPath, NPGSOChatRoomRefSet.objName, "chat_room", _judgeCanShowFunc)
	    {
	    }

	    /****************
	     * 显示的菜单文字
	     **/
	    protected override string _menuText { get { return "聊天室配置 (chat_room)"; } }
	}

}