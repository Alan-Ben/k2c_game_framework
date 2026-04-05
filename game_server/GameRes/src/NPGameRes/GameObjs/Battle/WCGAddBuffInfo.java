package NPGameRes.GameObjs.Battle;

import NPCommon.Util.CommonFunc;

import java.util.ArrayList;
import java.util.List;

public class WCGAddBuffInfo
{
    //bufid
    public long buf_id;
    //层数
    public int layer;
    //时间
    public int time_ms;

    public static List<WCGAddBuffInfo> readBuffInfoList(String _str)
    {
        List<WCGAddBuffInfo> list = new ArrayList<WCGAddBuffInfo>();
        if (_str.trim().length() <= 0)
        {
            return list;
        }

        //分割不同层级
        String[] bufStrS = CommonFunc.charSplit(_str, ';');
        //某个层级的特效ID信息
        WCGAddBuffInfo info = null;

        //遍历敌方的所有层级特效配置信息
        for (int i = 0; i < bufStrS.length; i++)
        {
            info = new WCGAddBuffInfo();

            String[] strs = CommonFunc.charSplit(bufStrS[i], ':');

            //查询
            info.buf_id = Long.parseLong(strs[0]);

            if (strs.length > 1)
                info.time_ms = Integer.parseInt(strs[1]);
            else
                info.time_ms = -1;

            if (strs.length > 2)
                info.layer = Integer.parseInt(strs[2]);
            else
                info.layer = 1;

            list.add(info);
        }

        return list;
    }
}