package NPGameRes.GameObjs.Battle.AITrigger;

import NPGameRes.GameObjs.Battle._AWCGBasicAITrigger;
import WCGCommon.Enum.NPEnum.EWCGAITriggerType;

import java.util.List;

//ai生产完成触发音效效果
public class WCGAITriggerSoundPlay extends _AWCGBasicAITrigger
{

    /**
     * Audio  列表
     */
    private List<Long> _m_lAudioIDList;

    public List<Long> AudioIDList()
    {
        return _m_lAudioIDList;
    }

    public EWCGAITriggerType triggerType()
    {
        return EWCGAITriggerType.SOUND_PLAY;
    }

    public static WCGAITriggerSoundPlay read(String _infoStr)
    {
        WCGAITriggerSoundPlay obj = new WCGAITriggerSoundPlay();

//      String[] strs =  WCGCommonFunc.charSplit(_infoStr, ':');
//
//      for (int i = 0; i < strs.length; ++i)
//      {
//          try
//          {
//              obj._m_lAudioIDList.Add(Long.parseLong(strs[i]).trim());
//          }
//          catch (Exception e)
//          {
//              CommLog.error("Error Format for WCGEffectSoundPlayer -  example: enum:groupId:groupId.... Error Str: " + strs);
//              continue;
//          }
//      }

        return obj;
    }
}