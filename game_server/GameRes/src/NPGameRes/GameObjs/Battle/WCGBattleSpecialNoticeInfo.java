package NPGameRes.GameObjs.Battle;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;

import java.util.ArrayList;
import java.util.List;

public class WCGBattleSpecialNoticeInfo
{
    public String assetPath;
    public String wndObjName;

    /*******************
     * 从字符串初始化数据
     **/
    public void readIndex(String _str, String _columnName)
    {
        //拆分字符串后进行读取
        String[] strs = CommonFunc.charSplit(_str, ':');

        if (strs.length < 2)
        {
            CommLog.error("没有配置 特殊提示窗口对象名! columnName: " + _columnName);
            return;
        }
        assetPath = strs[0];
        wndObjName = strs[1];
    }

    /************
     * 读取队列
     **/
    public static List<WCGBattleSpecialNoticeInfo> readSpecialNoticeInfoList(String _str)
    {
        List<WCGBattleSpecialNoticeInfo> list = new ArrayList<WCGBattleSpecialNoticeInfo>();
        if (null == _str || _str.length() <= 0)
            return list;

        String[] strs = CommonFunc.charSplit(_str, '|');
        for (int i = 0; i < strs.length; i++)
        {
//            WCGBattleSpecialNoticeInfo info = new WCGBattleSpecialNoticeInfo();
//            info.readIndex(strs[i],"");
//
//            list.add(info);
        }
        return list;
    }

    /***********
     * 构造索引名称
     **/
    public String makeIndexName()
    {
        return assetPath + wndObjName;
    }

    @Override
    public String toString()
    {
        return String.format("%s:%s", assetPath, wndObjName);
    }
}