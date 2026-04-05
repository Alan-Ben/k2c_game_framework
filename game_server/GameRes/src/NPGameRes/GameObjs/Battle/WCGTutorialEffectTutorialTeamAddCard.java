package NPGameRes.GameObjs.Battle;

import WCGCommon.Enum.NPEnum.EWCGTutorialEffectType;

public class WCGTutorialEffectTutorialTeamAddCard
{
//    private long _m_lCardId;
//    private int _m_lCardLevel;
//
//    public long cardId (){ return _m_lCardId; }
//
//    public int cardLevel (){ return _m_lCardLevel; }

    protected WCGTutorialEffectTutorialTeamAddCard()
    {
    }

    public EWCGTutorialEffectType effectType()
    {
        return EWCGTutorialEffectType.BATTLE_TEAM_ADD_CARD;
    }

    public static WCGTutorialEffectTutorialTeamAddCard readVariable(String _str)
    {
        return null;
//        if (string.IsNullOrEmpty(_str)) {
//            UnityEngine.Debug.LogError("效果配置错误 - WCGTutorialEffectTutorialTeamAddCard example: enum:cardId(:cardLevel)， Error Str: " + _str);
//            return null;
//        }
//
//        //解析字符串
//        string[] strs = _str.Split(':');
//        if (strs.length == 0) {
//            UnityEngine.Debug.LogError("效果配置错误 - WCGTutorialEffectTutorialTeamAddCard example: enum:card_id(:cardLevel) Error Str: " + _str);
//            return null;
//        }
//
//        WCGTutorialEffectTutorialTeamAddCard effectObj = new WCGTutorialEffectTutorialTeamAddCard();
//
//        try {
//            effectObj._m_lCardId = long.Parse(strs[0]);
//            if (strs.length > 1)
//                effectObj._m_lCardLevel = int.Parse(strs[1]);
//            return effectObj;
//        }
//        catch (Exception) {
//            UnityEngine.Debug.LogError("效果配置错误 - WCGTutorialEffectTutorialTeamAddCard example: enum:card_id(:cardLevel) Error Str: " + _str);
//            return null;
//        }
    }
}