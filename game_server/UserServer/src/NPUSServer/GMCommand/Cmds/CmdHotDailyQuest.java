package NPUSServer.GMCommand.Cmds;

import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPCommon.Log.CommLog;
import NPUSServer.GMCommand.UsCmdBase;
import NPUSServer.NPUSMain;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.DailyQuestComp.DailyQuestComponent;
import NPUSServer.NPUSUserMgr.UserComp.DailyQuestComp.DailyQuestInfo;
import NPUSServer.NPUserServer;

import java.lang.reflect.Field;
import java.lang.reflect.Method;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.Map.Entry;

@ACommander(comment = "每日任务热更1", name = "dailyQuestHot1")
public class CmdHotDailyQuest extends UsCmdBase
{
    @SuppressWarnings("unchecked")
    @ACommand(comment = "重新注册监听")
    public String reload(int _usIdx)
    {
        //反射获取dailyQuestComponent下的_m_mapTotalQuestInfoMap
        Field mapField;

        Field infoListField;
        Method infoReg;
        Method infoUnreg;
        try
        {
            Class<DailyQuestComponent> compClazz = DailyQuestComponent.class;
            mapField = compClazz.getDeclaredField("_m_mapTotalQuestInfoMap");
            mapField.setAccessible(true);

            Class<DailyQuestInfo> infoClazz = DailyQuestInfo.class;
            infoListField = infoClazz.getDeclaredField("_m_evtEntryList");
            infoListField.setAccessible(true);

            infoReg = infoClazz.getDeclaredMethod("_regEvtEntry");
            infoReg.setAccessible(true);
            infoUnreg = infoClazz.getDeclaredMethod("_unRegEvtEntry");
            infoUnreg.setAccessible(true);

        } catch (Exception e)
        {
            CommLog.error("", e);
            return "fail";
        }

        NPUserServer userServer = NPUSMain.GetServer(_usIdx);
        for (NPUSUserData userdata : userServer.getUsUserMgr().getAllCacheUserData())
        {
            userdata.lockUser();
            try
            {
                DailyQuestComponent dailyQuestComponent = userdata.getDailyQuestComponent();

                //field转换为map然后遍历
                Object _map = mapField.get(dailyQuestComponent);
                if (_map != null)
                {
                    //遍历map
                    HashMap<Long, DailyQuestInfo> map = (HashMap<Long, DailyQuestInfo>) _map;
                    for (Entry<Long, DailyQuestInfo> entry : map.entrySet())
                    {
                        DailyQuestInfo info = entry.getValue();
                        //先反注册
                        infoUnreg.invoke(info);
                        //清空列表
                        infoListField.set(info, new ArrayList<>());
                        //重新调用注册
                        infoReg.invoke(info);
                    }
                }
            } catch (Exception e)
            {
                CommLog.error("", e);
            } finally
            {
                userdata.unlockUser();
            }
        }
        return "done";
    }
}
