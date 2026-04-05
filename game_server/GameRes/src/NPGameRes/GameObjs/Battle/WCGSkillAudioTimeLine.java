package NPGameRes.GameObjs.Battle;

import java.util.ArrayList;
import java.util.List;

public class WCGSkillAudioTimeLine
{
    public long timeLine;//时间节点
    public List<Long> audio_id_list = new ArrayList<>(); //该时间节点对应要播放的音效id集合

    public static List<WCGSkillAudioTimeLine> readSkillAudioTimeLine(String _str)
    {
        List<WCGSkillAudioTimeLine> skillAudioTimeLineInfoList = new ArrayList<WCGSkillAudioTimeLine>();
//        if (_str==null || _str.isEmpty())
//        {
//            //CommLog.error("read excel WCGActorSkill info err!, str is null or empty!" );
//            return skillAudioTimeLineInfoList;
//        }
//        String[] strs =  WCGCommonFunc.charSplit(_str, ';');
//        for (int i = 0; i < strs.length; i++)
//        {
//            String[] subStrs = strs[i].Split(':');
//            if (subStrs.Length < 2)
//            {
//                CommLog.error("read excel WCGSkillAudioTimeLine info err! : " + _str);
//                continue;
//            }
//            WCGSkillAudioTimeLine actorSkillInfo = new WCGSkillAudioTimeLine();
//
//            actorSkillInfo.timeLine = Long.parseLong(subStrs[0].trim());
//
//            for (int j = 1; j < subStrs.Length; j++)
//                actorSkillInfo.audio_id_list.Add(Long.parseLong(subStrs[j]).trim());
//
//            skillAudioTimeLineInfoList.Add(actorSkillInfo);
//        }
        return skillAudioTimeLineInfoList;
    }
}