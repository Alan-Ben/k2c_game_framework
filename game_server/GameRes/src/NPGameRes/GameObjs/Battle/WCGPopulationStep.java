package NPGameRes.GameObjs.Battle;

import NPCommon.Util.CommonFunc;

import java.util.ArrayList;

public class WCGPopulationStep
{
    /**
     * 本阶段对应人口万分比
     */
    public int populationPer;
    /**
     * 本阶段获取资源的万分比
     */
    public int resRewardPer;

    public static ArrayList<WCGPopulationStep> readPopulationStepList(String _str)
    {
        ArrayList<WCGPopulationStep> list = new ArrayList<WCGPopulationStep>();
        if (_str.isEmpty())
            return list;

        String[] strs = CommonFunc.charSplit(_str, ';');
        for (int i = 0; i < strs.length; ++i)
        {
            String infoStr = strs[i];
            if (infoStr.isEmpty())
                continue;
            String[] subStrs = CommonFunc.charSplit(infoStr, ':');
            if (subStrs.length < 2)
                continue;
            WCGPopulationStep stepInfo = new WCGPopulationStep();
            stepInfo.populationPer = Integer.parseInt(subStrs[0].trim());
            stepInfo.resRewardPer = Integer.parseInt(subStrs[1].trim());
            list.add(stepInfo);
        }
        return list;
    }
}