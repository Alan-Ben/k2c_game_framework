package NPGameRes.GameObjs.Battle;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;

import java.util.ArrayList;
import java.util.List;

public class WCGHeroReliveInfo
{
    public int actor_v_idx; //影响复活时长的buffID
    public int v;//影响复活时长的buff层数
    public int hero_relive_time;//英雄复活时间
    public List<WCGActorCost> hero_relive_cost;//英雄复活消耗

    public static List<WCGHeroReliveInfo> readHeroReliveInfoList(String _str)
    {
        if (_str.isEmpty())
            return null;
        List<WCGHeroReliveInfo> infoList = new ArrayList<WCGHeroReliveInfo>();

        String[] strs = CommonFunc.charSplit(_str, '|');

        for (int i = 0; i < strs.length; i++)
        {
            String[] temp = CommonFunc.charSplit(strs[i], ':', 4);

            if (temp.length < 4)
            {
                CommLog.error("英雄复活时间需要配置  buff_id:buff层数:复活时长");
                continue;
            }

            try
            {
                WCGHeroReliveInfo itemInfo = new WCGHeroReliveInfo();
                itemInfo.actor_v_idx = Integer.parseInt(temp[0].trim());
                itemInfo.v = Integer.parseInt(temp[1].trim());
                itemInfo.hero_relive_time = Integer.parseInt(temp[2].trim());
                itemInfo.hero_relive_cost = WCGActorCost.readCostList(temp[3].trim());
                infoList.add(itemInfo);
            } catch (Exception e)
            {

            }
        }
        return infoList;
    }
}
