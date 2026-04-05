package NPGameRes.GameObjs.Battle;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import WCGCommon.Enum.NPEnum.ENPResouceType;

import java.util.ArrayList;
import java.util.List;

public class WCGActorCost
{
    public ENPResouceType cost_resource_type;//消耗类型
    public int produce_cost;//生产消耗数量

    public WCGActorCost()
    {
        cost_resource_type = ENPResouceType.GOLD;
        produce_cost = 0;
    }

    public WCGActorCost(WCGActorCost _cost)
    {
        cost_resource_type = _cost.cost_resource_type;
        produce_cost = _cost.produce_cost;
    }

    public static List<WCGActorCost> readCostList(String _str)
    {
        List<WCGActorCost> costList = new ArrayList<WCGActorCost>();

        String[] strs = CommonFunc.charSplit(_str, ';');

        for (int i = 0; i < strs.length; i++)
        {
            String[] temp = CommonFunc.charSplit(strs[i], ':');
            if (temp.length < 2)
                continue;

            try
            {
                WCGActorCost cost = new WCGActorCost();
                cost.cost_resource_type = ENPResouceType.valueOf(temp[0].toUpperCase().trim());
                cost.produce_cost = Integer.parseInt(temp[1].trim());
                costList.add(cost);
            } catch (Exception e)
            {
                CommLog.error("WCGActorCost parseFailed ,str=" + _str, e);
            }
        }
        return costList;
    }
}