using UnityEngine;
using System.Diagnostics;

public class Debug
{
    #region ******** Extended ******** 

    public static void Log(object message)
    {
        UnityEngine.Debug.Log(message);
    }

    public static void Log(object message, Object context)
    {
        UnityEngine.Debug.Log(message, context);
    }

    public static void LogError(object message)
    {
        UnityEngine.Debug.LogError(message);
    }

    public static void LogError(object message, Object context)
    {
        UnityEngine.Debug.LogError(message, context);
    }

    public static void LogWarning(object message)
    {
        UnityEngine.Debug.LogWarning(message);
    }

    public static void LogWarning(object message, Object context)
    {
        UnityEngine.Debug.LogWarning(message, context);
    }

    public static void LogErrorFormat(string format, params object[] args)
    {
        UnityEngine.Debug.LogErrorFormat(format, args);
    }

    public static void LogErrorFormat(Object context, string format, params object[] args)
    {
        UnityEngine.Debug.LogErrorFormat(context, format, args);
    }

    public static void LogFormat(string format, params object[] args)
    {
        UnityEngine.Debug.LogFormat(format, args);
    }

    public static void LogFormat(Object context, string format, params object[] args)
    {
        UnityEngine.Debug.LogFormat(context, format, args);
    }

    public static void LogWarningFormat(string format, params object[] args)
    {
        UnityEngine.Debug.LogWarningFormat(format, args);
    }

    public static void LogWarningFormat(Object context, string format, params object[] args)
    {
        UnityEngine.Debug.LogWarningFormat(context, format, args);
    }

    #endregion

    #region ******** Mirrored ******** 

    public static void LogException(System.Exception exception, Object context = null)
    {
        UnityEngine.Debug.LogException(exception, context);
    }

    public static bool developerConsoleVisible
    {
        get { return UnityEngine.Debug.developerConsoleVisible; }
        set { UnityEngine.Debug.developerConsoleVisible = value; }
    }

    public static bool isDebugBuild
    {
        get { return UnityEngine.Debug.isDebugBuild; }
    }

    public static void DrawLine(Vector3 start, Vector3 end, Color? color = null, float duration = 0.0f, bool depthTest = true)
    {
        Color col = color.HasValue ? color.Value : Color.white; // workaround for problem with color constant as default value 
        UnityEngine.Debug.DrawLine(start, end, col, duration, depthTest);
    }

    public static void DrawRay(Vector3 start, Vector3 dir, Color? color = null, float duration = 0.0f, bool depthTest = true)
    {
        Color col = color.HasValue ? color.Value : Color.white; // workaround for problem with color constant as default value 
        UnityEngine.Debug.DrawRay(start, dir, col, duration, depthTest);
    }

    public static void Break()
    {
        UnityEngine.Debug.Break();
    }

    public static void DebugBreak()
    {
        UnityEngine.Debug.DebugBreak();
    }

    public static void ClearDeveloperConsole()
    {
        UnityEngine.Debug.ClearDeveloperConsole();
    }

    #endregion
    #region ******** 输出超长字符 *******

    private static int oneLogMaxCharCount = 1000;

	private static int longMsgMaxLogCount = 20;
	
	/// <summary>
	/// 用于输出长度超过1000字符的log，拆分成多条输出
	/// </summary>
	public static void Log_Long(object message, Object context = null)
	{
		string sMessage = $"{message}";
		int messageLength = sMessage.Length;
		if (messageLength > oneLogMaxCharCount)
		{
			int curLogCount = 0;
			for (int i = 0; i < messageLength; i += oneLogMaxCharCount)
			{
				if(curLogCount> longMsgMaxLogCount)
					break;
				if (i + oneLogMaxCharCount < messageLength)
					UnityEngine.Debug.Log($"[{curLogCount}]{sMessage.Substring(i, oneLogMaxCharCount)}", context);
				else
					UnityEngine.Debug.Log($"[{curLogCount}]{sMessage.Substring(i, messageLength - i)}", context);
				curLogCount++;
			}
		}
		else
			UnityEngine.Debug.Log( $"{sMessage}", context );
	}
	
	/// <summary>
	/// 用于输出长度超过1000字符的log，拆分成多条输出
	/// </summary>
	public static void LogError_Long(object message, Object context = null)
	{
		string sMessage = $"{message}";
		int messageLength = sMessage.Length;
		if (messageLength > oneLogMaxCharCount)
		{
			int curLogCount = 0;
			for (int i = 0; i < messageLength; i += oneLogMaxCharCount)
			{
				if(curLogCount> longMsgMaxLogCount)
					break;
				if (i + oneLogMaxCharCount < messageLength)
					UnityEngine.Debug.LogError($"[{curLogCount}]{sMessage.Substring(i, oneLogMaxCharCount)}", context);
				else
					UnityEngine.Debug.LogError($"[{curLogCount}]{sMessage.Substring(i,messageLength - i)}", context);
				curLogCount++;
			}
		}
		else
			UnityEngine.Debug.LogError( $"{sMessage}", context );
	}
	
	/// <summary>
	/// 用于输出长度超过1000字符的log，拆分成多条输出
	/// </summary>
	public static void LogWarning_Long(object message, Object context = null)
	{
		string sMessage = $"{message}";
		int messageLength = sMessage.Length;
		if (messageLength > oneLogMaxCharCount)
		{
			int curLogCount = 0;
			for (int i = 0; i < messageLength; i += oneLogMaxCharCount)
			{
				if(curLogCount> longMsgMaxLogCount)
					break;
				if (i + oneLogMaxCharCount < messageLength)
					UnityEngine.Debug.LogWarning($"[{curLogCount}]{sMessage.Substring(i, oneLogMaxCharCount)}", context);
				else
					UnityEngine.Debug.LogWarning($"[{curLogCount}]{sMessage.Substring(i, messageLength - i)}", context);
				curLogCount++;
			}
		}
		else
			UnityEngine.Debug.LogWarning( $"{sMessage}", context );
	}
	
	/// <summary>
	/// 用于输出长度超过1000字符的log，拆分成多条输出
	/// </summary>
	public static void LogErrorFormat_Long(string format, params object[] args)
	{
		LogErrorFormat_Long(null, format,args);
	}
	
	/// <summary>
	/// 用于输出长度超过1000字符的log，拆分成多条输出
	/// </summary>
	public static void LogErrorFormat_Long(Object context, string format, params object[] args)
	{
		string sMessage = string.Format(format, args);
		int messageLength = sMessage.Length;
		if (messageLength > oneLogMaxCharCount)
		{
			int curLogCount = 0;
			for (int i = 0; i < messageLength; i += oneLogMaxCharCount)
			{
				if(curLogCount> longMsgMaxLogCount)
					break;
				if (i + oneLogMaxCharCount < messageLength)
					UnityEngine.Debug.LogErrorFormat(context, $"[{curLogCount}]{sMessage.Substring(i, oneLogMaxCharCount)}");
				else
					UnityEngine.Debug.LogErrorFormat(context, $"[{curLogCount}]{sMessage.Substring(i, messageLength - i)}");
				curLogCount++;
			}
		}
		else
			UnityEngine.Debug.LogErrorFormat(context, sMessage);
	}
	
	/// <summary>
	/// 用于输出长度超过1000字符的log，拆分成多条输出
	/// </summary>
	public static void LogFormat_Long(string format, params object[] args)
	{
		LogFormat_Long(null, format,args);
	}
	
	/// <summary>
	/// 用于输出长度超过1000字符的log，拆分成多条输出
	/// </summary>
	public static void LogFormat_Long(Object context, string format, params object[] args)
	{
		string sMessage = string.Format(format, args);
		int messageLength = sMessage.Length;
		if (messageLength > oneLogMaxCharCount)
		{
			int curLogCount = 0;
			for (int i = 0; i < messageLength; i += oneLogMaxCharCount)
			{
				if(curLogCount> longMsgMaxLogCount)
					break;
				if (i + oneLogMaxCharCount < messageLength)
					UnityEngine.Debug.LogFormat(context, $"[{curLogCount}]{sMessage.Substring(i, oneLogMaxCharCount)}");
				else
					UnityEngine.Debug.LogFormat(context, $"[{curLogCount}]{sMessage.Substring(i, messageLength - i)}");
				curLogCount++;
			}
		}
		else
			UnityEngine.Debug.LogFormat(context, sMessage);
	}
	
	/// <summary>
	/// 用于输出长度超过1000字符的log，拆分成多条输出
	/// </summary>
	public static void LogWarningFormat_Long(string format, params object[] args)
	{
		LogWarningFormat_Long(null, format,args);
	}

	/// <summary>
	/// 用于输出长度超过1000字符的log，拆分成多条输出
	/// </summary>
	public static void LogWarningFormat_Long(Object context, string format, params object[] args)
	{
		string sMessage = string.Format(format, args);
		int messageLength = sMessage.Length;
		if (messageLength > oneLogMaxCharCount)
		{
			int curLogCount = 0;
			for (int i = 0; i < messageLength; i += oneLogMaxCharCount)
			{
				if(curLogCount> longMsgMaxLogCount)
					break;
				if (i + oneLogMaxCharCount < messageLength)
					UnityEngine.Debug.LogWarningFormat(context, $"[{curLogCount}]{sMessage.Substring(i, oneLogMaxCharCount)}");
				else
					UnityEngine.Debug.LogWarningFormat(context, $"[{curLogCount}]{sMessage.Substring(i, messageLength - i)}");
				curLogCount++;
			}
		}
		else
			UnityEngine.Debug.LogWarningFormat(context, sMessage);
	}
	
	#endregion

	//	1.手机端需要显示的log使用 Debug.Log
	//	平常不开启，showDebugOutput开启后需要显示的log需要增加判断
	//	if(_AALMonoMain.instance.showDebugOutput)
	//	Debug.Log($"【主城】此处有报错 Value:{Value}");
	
	//	2. #if UNITY_EDITOR 可以直接 替换为 Debug.LogXXX_EditorOnly（二者等价，均不会有多余的消耗）
	//	#if UNITY_EDITOR
	//		Debug.LogError($"【主城】此处有报错Value:{Value}");
	//	#endif
	//	改为：
	//	Debug.Log_EditorOnly($"【主城】此处有报错Value:{Value}");
	//	(Tips:带有_EditorOnly后缀的增加了[Conditional("DEBUG_EDITOR")],
	//	实现只有在对应宏DEBUG_EDITOR开启的情况下才会编译该行代码，解决不希望有多余的format消耗的问题)
	//
	//	3.需要输出超过1000字符的log使用带  _Long 的方法：例：Debug.Log_Long("我超长.....超过1000字符了.....");
	//
	// 	NPLogs.Log 设置为 DEBUG_EDITOR（安卓如果需要输出的时候用） 或者 UNITY_EDITOR 宏开启的时候才输出log
	// 	需要在安卓包里开启编辑器下Log,增加 DEBUG_EDITOR 宏再打包，
	// 	编辑器下不需要显示编辑器Log,修改代码 Debug，删除里面的UNITY_EDITOR
	// 	项目工程默认设置：均不开启 DEBUG_EDITOR

	/// <summary>
	/// 通过宏 DEBUG_NORMAL控制Log是否输出，一般为运行时不需要展示的log
	/// </summary>

	#region Editor Only

	#region ******** Extended ******** 

	[Conditional("DEBUG_EDITOR"), Conditional("UNITY_EDITOR")]
	public static void Log_EditorOnly(object message)
	{
		Log(message);
	}

	[Conditional("DEBUG_EDITOR"), Conditional("UNITY_EDITOR")]
	public static void Log_EditorOnly(object message, Object context)
	{
		Log(message, context);
	}

	[Conditional("DEBUG_EDITOR"), Conditional("UNITY_EDITOR")]
	public static void LogError_EditorOnly(object message)
	{
		LogError(message);
	}

	[Conditional("DEBUG_EDITOR"), Conditional("UNITY_EDITOR")]
	public static void LogError_EditorOnly(object message, Object context)
	{
		LogError(message, context);
	}

	[Conditional("DEBUG_EDITOR"), Conditional("UNITY_EDITOR")]
	public static void LogWarning_EditorOnly(object message)
	{
		LogWarning(message);
	}

	[Conditional("DEBUG_EDITOR"), Conditional("UNITY_EDITOR")]
	public static void LogWarning_EditorOnly(object message, Object context)
	{
		LogWarning(message, context);
	}

	[Conditional("DEBUG_EDITOR"), Conditional("UNITY_EDITOR")]
	public static void LogErrorFormat_EditorOnly(string format, params object[] args)
	{
		LogErrorFormat(format, args);
	}

	[Conditional("DEBUG_EDITOR"), Conditional("UNITY_EDITOR")]
	public static void LogErrorFormat_EditorOnly(Object context, string format, params object[] args)
	{
		LogErrorFormat(context, format, args);
	}

	[Conditional("DEBUG_EDITOR"), Conditional("UNITY_EDITOR")]
	public static void LogFormat_EditorOnly(string format, params object[] args)
	{
		LogFormat(format, args);
	}

	[Conditional("DEBUG_EDITOR"), Conditional("UNITY_EDITOR")]
	public static void LogFormat_EditorOnly(Object context, string format, params object[] args)
	{
		LogFormat(context, format, args);
	}

	[Conditional("DEBUG_EDITOR"), Conditional("UNITY_EDITOR")]
	public static void LogWarningFormat_EditorOnly(string format, params object[] args)
	{
		LogWarningFormat(format, args);
	}

	[Conditional("DEBUG_EDITOR"), Conditional("UNITY_EDITOR")]
	public static void LogWarningFormat_EditorOnly(Object context, string format, params object[] args)
	{
		LogWarningFormat(context, format, args);
	}
	
	/// <summary>
	/// 单独打印提示信息，策划不需要关心那种
	/// </summary>
	/// <param name="_str"></param>
	[Conditional("DEBUG_EDITOR"), Conditional("UNITY_EDITOR")]
	public static void LogError_EditorOnly_SysTip(string _str)
	{
		LogError($"<color=green>【提示信息】</color>{_str}");
	}

	#endregion

    #endregion
	
}