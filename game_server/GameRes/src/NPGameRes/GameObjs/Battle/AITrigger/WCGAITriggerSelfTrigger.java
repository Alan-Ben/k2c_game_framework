package NPGameRes.GameObjs.Battle.AITrigger;

import ALServerLog.ALServerLog;
import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.Battle._AWCGBasicAITrigger;
import WCGCommon.Enum.NPEnum.EWCGAITriggerType;

import java.util.ArrayList;

/// <summary>
/// AI 触发效果： 对自己触发对应的效果队列
/// </summary>
public class WCGAITriggerSelfTrigger extends _AWCGBasicAITrigger
{

    //触发效果列表
    private ArrayList<Long> _m_lTriggerEffectList;

    public ArrayList<Long> EffectList()
    {
        return _m_lTriggerEffectList;
    }

    @Override
    public EWCGAITriggerType triggerType()
    {
        return EWCGAITriggerType.SELF_TRIGGER;
    }

    public static WCGAITriggerSelfTrigger read(String _infoStr)
    {
        WCGAITriggerSelfTrigger obj = new WCGAITriggerSelfTrigger();
        obj._m_lTriggerEffectList = new ArrayList<Long>();
        String[] idList = CommonFunc.charSplit(_infoStr, ':');
        for (int i = 0; i < idList.length; i++)
        {
            try
            {
                obj._m_lTriggerEffectList.add(Long.parseLong(idList[i].trim()));
            } catch (Exception e)
            {
                ALServerLog.Error("ai selftrigger 配置错误, 必须配置效果id" + _infoStr);
            }
        }
        return obj;
    }
}