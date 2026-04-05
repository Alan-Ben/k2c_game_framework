using GOE;

namespace Hotfix
{
    public class HotfixPlayerEffect
    {
        /// <summary>
        /// 主工程调用，处理热更效果
        /// </summary>
        public static void dealPlayerEffect(NPVarInfo _varVariableInfo, string _type, string _info)
        {
            switch (_type)
            {
                case HotfixEffectType.OPEN_TILEMATCH_GAME:
                    QueueMgr.instance.addNode_InGame_MainUIMainWnd(GGUIWndTileMatchGame.instance, HotfixUINodeTagConst.TILEMATCH_GAME, 0);
                    break;
                default:
                    Debug.LogError($"热更效果类型：{_type} 未定义，请确认是否填写正确（C_HOTFIX_EFFECT:操作类型:参数（参数可不填）），或者让程序添加解析");
                    break;
            }
        }
    }
}