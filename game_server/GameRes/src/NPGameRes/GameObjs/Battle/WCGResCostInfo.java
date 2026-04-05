package NPGameRes.GameObjs.Battle;

import NPCommon.Util.CommonFunc;

import java.util.ArrayList;
import java.util.List;

public class WCGResCostInfo
{
    public long uniform_item_id; //统一物品ID
    public int num;//生产消耗数量

    public static List<WCGResCostInfo> readCostList(String _str)
    {
        List<WCGResCostInfo> costList = new ArrayList<WCGResCostInfo>();
        if (_str == null || _str.isEmpty())
            return costList;

        String[] strs = CommonFunc.charSplit(_str, ';');

        for (int i = 0; i < strs.length; i++)
        {
            String[] temp = CommonFunc.charSplit(strs[i], ':');
            if (temp.length < 2)
            {
                //Debug.Log("资源需要配置  枚举:消耗");
                continue;
            }

            try
            {
                WCGResCostInfo cost = new WCGResCostInfo();
                cost.uniform_item_id = Long.parseLong(temp[0].trim());
                cost.num = Integer.parseInt(temp[1].trim());
                costList.add(cost);
            } catch (Exception e)
            {

            }
        }
        return costList;
    }
}