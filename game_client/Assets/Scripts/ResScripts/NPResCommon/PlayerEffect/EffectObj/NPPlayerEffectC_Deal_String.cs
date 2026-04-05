using System;
using ALPackage;
using NPEnum;

namespace GOE
{
	/// <summary>
	/// 针对String的数据进行处理
	/// </summary>
	public class NPPlayerEffectC_Deal_String : _ANPPlayerEffectInfo
	{
	    private EClientStringDealType _m_eDealType;       //处理枚举
	    private string _m_stringValue;

	    public NPPlayerEffectC_Deal_String()
	    {
	        _m_eDealType = EClientStringDealType.NONE;
	        _m_stringValue = String.Empty;
	    }

	    /************
	     * 效果类型
	     **/
	    public override ENPPlayerEffectType effectType { get { return ENPPlayerEffectType.C_DEAL_STRING; } }

	    public override void DealPlayerEffect(NPVarInfo _varVariableInfo)
	    {
#if NP_GAME
	        switch(_m_eDealType)
	        {
				case EClientStringDealType.FORCE_CLOSE_NODE_BY_TAG:
                    //通过node标记强制关闭node
					QueueMgr.instance.forceCloseNodeByTag(_m_stringValue);
			        break;
				case EClientStringDealType.ENABLE_DIALOG_BOX:
                    //显示或者隐藏对话里面的对话框，程序是控制对话框的加载父节点是activity实现，参数true/false
					NPGGUIWndDialogue.instance.enableDialogBoxParent(ALCommon.GetBool(_m_stringValue));
			        break;
				case EClientStringDealType.SIMULATE_CLICK_CLOTHES_EDITOR_WND_BTN:
                    //模拟点击衣柜编辑界面按钮
					WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_CLOTHES_EDITOR_WND_BTN, _m_stringValue);
			        break;
				case EClientStringDealType.CITY_HIDE_UI:
					break;
				case EClientStringDealType.ROOM_HIDE_UI:
					//卧室界面隐藏UI，参数时间秒（-1表示一直隐藏）
					if (string.IsNullOrEmpty(_m_stringValue))
						return;
					float roomDurationSec = ALCommon.ParseFloat(_m_stringValue);
					MainAdditionRoomTDScene.instance.dealEffectHideUI(roomDurationSec);
					GMainGUIAddSceneRoom.instance.dealEffectHideUI(roomDurationSec);
					break;
				case EClientStringDealType.SWITCH_HERO_INFO_TAB:
                    //切换伙伴信息界面页签
                    WinMsg.SendMsg(WinMsgType.SWITCH_HERO_INFO_TAB, _m_stringValue);
                    break;
				case EClientStringDealType.SWITCH_CONSORT_MAIN_TAB:
                    //切换情人主界面页签
                    WinMsg.SendMsg(WinMsgType.SWITCH_CONSORT_MAIN_TAB, _m_stringValue);
                    break;
                case EClientStringDealType.SIMULATE_CLICK_CITY_MAIN_EXPAND_BAR:
                    //模拟点击展开主城bar按钮
                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_CITY_MAIN_EXPAND_BAR, _m_stringValue);
                    break;
                default:
	                break;
	        }
#endif
	    }

	    public static NPPlayerEffectC_Deal_String readEffect(string _str)
	    {
	        string[] strs = _str.Split(':');
	        if(strs.Length < 2)
	        {
	            UnityEngine.Debug.LogError("配置错误 - C_DEAL_STRING   example: enum:type:str Error Str: " + _str);
	            return null;
	        }

	        NPPlayerEffectC_Deal_String effectObj = new NPPlayerEffectC_Deal_String();

	        try
	        {
	            effectObj._m_eDealType = (EClientStringDealType)ALCommon.EnumParse(typeof(EClientStringDealType), strs[0], true);
	            effectObj._m_stringValue = strs[1];

	            return effectObj;
	        }
	        catch(Exception)
	        {
	            UnityEngine.Debug.LogError("配置错误 - C_DEAL_STRING   example: enum:type:str Error Str: " + _str);
	            return null;
	        }
	    }
	}
}