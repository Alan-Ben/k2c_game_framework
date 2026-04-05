package NPGameRes.GameObjs.Battle;

import NPCommon.Util.CommonFunc;
import WCGCommon.Enum.NPEnum.EWCGCampResouceType;

import java.util.ArrayList;
import java.util.List;

public class WCGCampRes
{
    public EWCGCampResouceType cost_resource_type;//资源类型
    public int count;//资源数量

    public static List<WCGCampRes> readCampResList(String _str)
    {
        List<WCGCampRes> costList = new ArrayList<WCGCampRes>();

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
                WCGCampRes cost = new WCGCampRes();
                cost.cost_resource_type = EWCGCampResouceType.valueOf(temp[0].toUpperCase().trim());
                cost.count = Integer.parseInt(temp[1].trim());
                costList.add(cost);
            } catch (Exception e)
            {

            }
        }
        return costList;
    }
}