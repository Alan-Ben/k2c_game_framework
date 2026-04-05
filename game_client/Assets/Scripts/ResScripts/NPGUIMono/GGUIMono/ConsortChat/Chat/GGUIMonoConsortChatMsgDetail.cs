using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;

public enum EConsortChatMsgDetailState
{
    Normal,
    Option,
    AIChat
}
/// <summary>
/// 
/// </summary>
public class GGUIMonoConsortChatMsgDetail : _AALBasicUIWndMono
{
    [ALHeader("妃子名")]
    public Text txtConsortName;
    [ALHeader("输入框")] 
    public InputFieldEmoji inputField;
    [ALHeader("发送消息按钮")] 
    public GameObject btnSend;
    [ALHeader("消息列表")]
    public GGUIMonoConsortChatMsgList msgList;
    [ALHeader("选项按钮容器")]
    public GGUIMonoConsortChatOptionItemContainer optionItemContainer;

    [ALHeader("AI聊天发送消息免费次数文本")]
    public Text txtAiChatLeftFreeTimes;
    [ALHeader("AI聊天发送消息消耗道具")]
    public NPGGUIMonoCommonItem monoAISendCostItem;
    [ALHeader("AI聊天，发送消息还有免费次数时显示的GO")] 
    public List<GameObject> aiFreeTimesShows;
    [ALHeader("ai聊天，发送消息需要消耗物品时显示的GO")]
    public List<GameObject> aiCostPriceShows;

    [ALHeader("AI聊天显示的GO")] 
    public List<GameObject> stateAIChatShows;
    [ALHeader("聊天普通状态显示的GO")]
    public List<GameObject> stateNormalChatShows;
    [ALHeader("聊天选项状态显示的GO")]
    public List<GameObject> stateOptionChatShows;
    [ALHeader("AI聊天未解锁提示文本")]
    public Text txtLockAITip;
    [ALHeader("AI聊天未解锁显示的GO")]
    public List<GameObject> lockAIChatShows;
    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(6203); } }
    public static string objName { get { return UIResPathAssistant.getObjName(6203);} }
}