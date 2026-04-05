package NPGameRes.GameObjs.Battle.AITrigger;

import NPGameRes.GameObjs.Battle._AWCGBasicAITrigger;
import WCGCommon.Enum.NPEnum.EWCGAITriggerType;

public class WCGAITriggerSetTutorialDone extends _AWCGBasicAITrigger
{

    private long _m_lTutorialId;//引导id

    public long tutorialId()
    {
        return _m_lTutorialId;
    }

    public EWCGAITriggerType triggerType()
    {
        return EWCGAITriggerType.SET_TUTORIAL_DONE;
    }

    public static WCGAITriggerSetTutorialDone read(String _str)
    {
        WCGAITriggerSetTutorialDone effectObj = new WCGAITriggerSetTutorialDone();
        return effectObj;
//        if (string.IsNullOrEmpty(_str)) {
//            UnityEngine.Debug.LogError("效果配置错误 - WCGAITriggerSetTutorialDone example: enum:EWCGTutorialType， Error Str: " + _str);
//            return null;
//        }
//
//        WCGAITriggerSetTutorialDone effectObj = new WCGAITriggerSetTutorialDone();
//
//        try {
//            effectObj._m_lTutorialId = long.Parse(_str);
//            return effectObj;
//        }
//        catch (Exception) {
//            UnityEngine.Debug.LogError("效果配置错误 - WCGAITriggerSetTutorialDone example: enum:id， Error Str: " + _str);
//            return null;
//        }
    }
}
