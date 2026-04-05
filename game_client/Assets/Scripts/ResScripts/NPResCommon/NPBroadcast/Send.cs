using System;

using ALPackage;


namespace GOE
{
	/// <summary>
	/// 窗口消息
	/// </summary>
	public class WinMsg
	{
	    /// <summary>
	    /// 广播
	    /// </summary>
	    public static void SendMsg(WinMsgType _type)
	    {
	        try
	        {
	            ALMsgSys.SendMsg((int)_type);
	        }
	        catch (Exception _ex)
	        {
#if UNITY_EDITOR
	            UnityEngine.Debug.LogError("Send Msg Err: " + _ex.Message + "\n" + _ex.StackTrace);
#endif
	        }
	    }
	    public static void SendMsg (WinMsgType _type, params object[] _objs) {
	        try
	        {
	            ALMsgSys.SendMsg((int)_type, _objs);
	        }
	        catch(Exception _ex)
	        {
#if UNITY_EDITOR
	            UnityEngine.Debug.LogError("Send Msg Err: " + _ex.Message + "\n" + _ex.StackTrace);
#endif
	        }
	    }

	    /// <summary>
	    /// 注册广播事件
	    /// </summary>
	    public static void RegisterMsg(WinMsgType _type, MsgRecAction _callback)
	    {
	        ALMsgSys.RegisterMsg((int)_type, _callback);
	    }

	    public static void RegisterMsgAct(WinMsgType _type, Action _callback)
	    {
	        ALMsgSys.RegisterMsgAct((int)_type, _callback);
	    }

	    /// <summary>
	    /// 反注册广播事件
	    /// </summary>
	    public static void UnregisterMsg(WinMsgType _type, MsgRecAction _callback)
	    {
	        ALMsgSys.UnregisterMsg((int)_type, _callback);
	    }
	    public static void UnregisterMsgAct(WinMsgType _type, Action _callback)
	    {
	        ALMsgSys.UnregisterMsgAct((int)_type, _callback);
	    }
	}
}