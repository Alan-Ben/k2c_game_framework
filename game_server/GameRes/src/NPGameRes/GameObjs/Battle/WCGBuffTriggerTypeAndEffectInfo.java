package NPGameRes.GameObjs.Battle;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.StringFunc;
import WCGCommon.Enum.NPEnum.EWCGSkillTriggerType;

import java.util.ArrayList;
import java.util.List;

public class WCGBuffTriggerTypeAndEffectInfo
{

    public EWCGSkillTriggerType trigger_type; //buff 被动触发类型

    public List<Long> trigger_effect_list;// buff 被动触发效果列表

    public float trigger_effect_list_chance;//trigger_effect_list触发的概率

    public static List<WCGBuffTriggerTypeAndEffectInfo> _readBuffTriggerList(String _info)
    {
        List<WCGBuffTriggerTypeAndEffectInfo> list = new ArrayList<WCGBuffTriggerTypeAndEffectInfo>();
        if (null == _info || _info.trim().isEmpty())
        {
            return list;
        }

        try
        {
            //逐个解析
            String[] strs = CommonFunc.charSplit(_info, ';');
            for (int i = 0; i < strs.length; i++)
            {
                String str = strs[i];

                //单个进行解析
                String[] items = CommonFunc.charSplit(str, ':', 3);

                //读取数据
                WCGBuffTriggerTypeAndEffectInfo effect = new WCGBuffTriggerTypeAndEffectInfo();

                effect.trigger_type = EWCGSkillTriggerType.valueOf(items[0].toUpperCase().trim());

                if (items[1].length() > 0)
                    effect.trigger_effect_list_chance = Integer.parseInt(items[1].trim());

                effect.trigger_effect_list = StringFunc.listLongFromString(items[2], ':');

                //加入队列
                list.add(effect);
            }
        } catch (Exception e)
        {
            CommLog.error("第  WCGBuffTriggerTypeAndEffectInfo 数据错误！ 数据: " + _info);
        }

        return list;
    }
}
