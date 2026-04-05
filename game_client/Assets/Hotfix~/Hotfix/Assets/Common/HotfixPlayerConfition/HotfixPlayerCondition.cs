using GOE;

namespace Hotfix
{
    public class HotfixPlayerCondition
    {
        public static bool isEnable(NPVarInfo _varVariableInfo, string _type, string _info)
        {
            switch (_type)
            {
                //新增类型在这里处理
                case HotfixConditionType.TEST: //打开制作图腾--自由搭建界面
                    UnityEngine.Debug.LogError($"============测试热更confition成功：{_info}");
                    return false;
                default:
                    Debug.LogError($"热更条件类型：{_type} 未定义，请确认是否填写正确（ C_HOTFIX_CONDITION:热更条件类型:参数），或者让程序添加解析");
                    return false;
            }
        }
    }
}