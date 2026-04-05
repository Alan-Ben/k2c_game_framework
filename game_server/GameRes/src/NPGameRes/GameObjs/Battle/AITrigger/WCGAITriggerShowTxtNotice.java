package NPGameRes.GameObjs.Battle.AITrigger;

import NPGameRes.GameObjs.Battle._AWCGBasicAITrigger;
import WCGCommon.Enum.NPEnum.EWCGAITriggerType;

/// <summary>
/// AI 触发效果：设置指定的队伍输
/// </summary>
public class WCGAITriggerShowTxtNotice extends _AWCGBasicAITrigger
{

//  private string _m_sAllyTxt;
//  private string _m_sFriendTxt;
//  private string _m_sEnemyTxt;
//
//  private string _m_lArg1;
//  private string _m_lArg2;
//
//  public string allyTxt (){ return _m_sAllyTxt; }
//  public string firendTxt (){ return _m_sFriendTxt; }
//  public string enemyTxt (){ return _m_sEnemyTxt; }
//  public string arg1 (){ return _m_lArg1; }
//  public string arg2 (){ return _m_lArg2; }

    public EWCGAITriggerType triggerType()
    {
        return EWCGAITriggerType.SHOW_TXT_NOTICE;
    }

    public static WCGAITriggerShowTxtNotice read(String _infoStr)
    {
        WCGAITriggerShowTxtNotice obj = new WCGAITriggerShowTxtNotice();

//       String[] strs = WCGCommonFunc.charSplit(_infoStr, ':');
//
//      obj._m_sAllyTxt = strs[0];
//      if (strs.length > 1)
//          obj._m_sFriendTxt = strs[1];
//      if (strs.length > 2)
//          obj._m_sEnemyTxt = strs[2];
//
//      if (strs.length > 3)
//          obj._m_lArg1 = strs[3];
//      if (strs.length > 4)
//          obj._m_lArg2 = strs[4];

        return obj;
    }
}