package NPGameRes.GameObjs.Battle;

import NPCommon.Util.CommonFunc;
import WCGCommon.Enum.NPEnum.EWCGTeamTriggerType;

import java.util.ArrayList;
import java.util.List;

public class WCGTeamTriggerEffectID extends _AWCGBasicTeamTrigger
{
    private List<Long> _m_lEffectList = new ArrayList<Long>();

    public List<Long> EffectList()
    {
        return _m_lEffectList;
    }

    @Override
    public EWCGTeamTriggerType triggerType()
    {
        return EWCGTeamTriggerType.EFFECT_ID;
    }

    public static WCGTeamTriggerEffectID read(String _infoStr)
    {
        WCGTeamTriggerEffectID obj = new WCGTeamTriggerEffectID();

        String[] strs = CommonFunc.charSplit(_infoStr, ':');
        obj._m_lEffectList = new ArrayList<Long>();
        for (int i = 0; i < strs.length; i++)
        {
            obj._m_lEffectList.add(Long.parseLong(strs[i].trim()));
        }
        return obj;
    }
}
