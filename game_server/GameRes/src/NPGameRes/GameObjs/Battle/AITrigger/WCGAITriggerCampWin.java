package NPGameRes.GameObjs.Battle.AITrigger;

import NPCommon.Log.CommLog;
import NPGameRes.GameObjs.Battle._AWCGBasicAITrigger;
import WCGCommon.Enum.NPEnum.EWCGAITriggerType;

/// <summary>
/// AI 触发效果： 设置ai中对应索引的值(idx索引暂定0-99)
/// </summary>
public class WCGAITriggerCampWin extends _AWCGBasicAITrigger
{

    private int _m_iCampId;

    public int CampID()
    {
        return _m_iCampId;
    }

    public EWCGAITriggerType triggerType()
    {
        return EWCGAITriggerType.CAMP_WIN;
    }

    public static WCGAITriggerCampWin read(String _infoStr)
    {
        WCGAITriggerCampWin obj = new WCGAITriggerCampWin();
        try
        {
            obj._m_iCampId = Integer.parseInt(_infoStr.trim());
            return obj;
        } catch (Exception e)
        {
            CommLog.error("ai camp_win配置错误 正确配置:阵营id");
            return null;
        }
    }

}
