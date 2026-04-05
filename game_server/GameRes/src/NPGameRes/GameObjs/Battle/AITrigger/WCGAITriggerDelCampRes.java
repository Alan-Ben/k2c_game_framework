package NPGameRes.GameObjs.Battle.AITrigger;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.Battle.Variable.WCGVariableGroupObj;
import NPGameRes.GameObjs.Battle._AWCGBasicAITrigger;
import WCGCommon.Enum.NPEnum.EWCGAITriggerType;
import WCGCommon.Enum.NPEnum.EWCGCampResouceType;


//增加阵营资源
public class WCGAITriggerDelCampRes extends _AWCGBasicAITrigger
{
    //资源枚举
    private EWCGCampResouceType _m_eResType;
    //CampID
    private int _m_lCampID;
    //数量
    private long _m_lCount;
    //额外数量计算公式
    private WCGVariableGroupObj _m_sExVariable;

    public EWCGCampResouceType ResType()
    {
        return _m_eResType;
    }

    public int CampID()
    {
        return _m_lCampID;
    }

    public long Count()
    {
        return _m_lCount;
    }

    public WCGVariableGroupObj ExVariable()
    {
        return _m_sExVariable;
    }

    public EWCGAITriggerType triggerType()
    {
        return EWCGAITriggerType.DEL_CAMP_RES;
    }


    public static WCGAITriggerDelCampRes read(String _str)
    {
        WCGAITriggerDelCampRes effectObj = new WCGAITriggerDelCampRes();
        //解析字符串
        String[] strs = CommonFunc.charSplit(_str, ':');
        //逐个判断
        if (strs.length < 3)
        {
            CommLog.error("效果配置错误 - DEL_CAMP_RES example: enum:campID:resType:cout(:exVariable) Error Str: " + _str);
            return null;
        }

        try
        {
            effectObj._m_eResType = EWCGCampResouceType.valueOf(strs[0].toUpperCase().trim());
            effectObj._m_lCampID = Integer.parseInt(strs[1].trim());
            effectObj._m_lCount = Long.parseLong(strs[2].trim());
            if (strs.length > 3)
                effectObj._m_sExVariable = WCGVariableGroupObj.readVariableGroup(strs[3], "_m_sExVariable 附加值高级公式错误： ");

            return effectObj;
        } catch (Exception e)
        {
            CommLog.error("效果配置错误 - DEL_CAMP_RES example: enum:campID:resType:cout(:exVariable) Error Str: " + _str);
            return null;
        }
    }
}