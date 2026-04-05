package WCGCommon.Game;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.RefWrap;

import java.util.ArrayList;
import java.util.List;

public class WCGLogicFuncs
{

    public static boolean checkMatchOpened(int _startTimeSec, int _now, int _openSec, int _closeSec, RefWrap<Integer> pTime2Next)
    {

        int span = _now - _startTimeSec;
        int roundTime = _openSec + _closeSec;
        int left = span % roundTime;

        boolean isOpen = false;
        int time2Next = 100;
        if (left < _openSec)
        {
            isOpen = true;
            time2Next = (int) (_openSec - left);

        } else
        {
            isOpen = false;
            time2Next = roundTime - left;
        }
        pTime2Next.v = time2Next;
        return isOpen;
    }

    //"prizeid":"13118:2;13117:2;13112:4" 解析为数组
    public static class ItemCount
    {
        public long uniformId;
        public long count;
    }

    public static List<ItemCount> parseItemList(String prizeStr)
    {
        List<ItemCount> ret = new ArrayList<>();
        if (prizeStr == null || prizeStr.isEmpty())
        {
            return ret;
        }
        String[] strs = CommonFunc.charSplit(prizeStr, ';');
        for (int i = 0; i < strs.length; i++)
        {
            ItemCount itemCount = new ItemCount();

            String[] pair = CommonFunc.charSplit(strs[i], ':');
            if (pair.length < 2)
            {
                CommLog.error("奖励配置不正确item[{}]:{} ,src={}", i, strs[i], prizeStr, new Exception());
                continue;
            }
            itemCount.uniformId = Long.parseLong(pair[0]);
            itemCount.count = Long.parseLong(pair[1]);
            ret.add(itemCount);
        }
        return ret;
    }

}
