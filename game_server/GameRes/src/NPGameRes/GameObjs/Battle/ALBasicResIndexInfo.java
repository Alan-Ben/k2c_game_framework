package NPGameRes.GameObjs.Battle;

import NPCommon.Log.CommLog;

/********************
 * 基本的索引信息对象
 **/
public class ALBasicResIndexInfo
{
    public int mainId;
    public int subId;

    /*******************
     * 从字符串初始化数据
     **/
    public void readIndex(String _str, String _columnName)
    {
        //拆分字符串后进行读取
        String[] strs = _str.split("\\|\\|");

        if (strs.length < 1)
        {
            //Debug.LogWarning("没有配置 main id! columnName: " + _columnName);
            return;
        }
        mainId = Integer.parseInt(strs[0].trim());

        if (strs.length < 2)
        {
            CommLog.error("没有配置 sub id! columnName: " + _columnName);
            return;
        }
        subId = Integer.parseInt(strs[1].trim());
    }

    @Override
    public String toString()
    {
        return String.format("%d:%d", mainId, subId);
    }
}
