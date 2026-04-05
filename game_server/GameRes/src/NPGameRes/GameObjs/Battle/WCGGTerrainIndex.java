package NPGameRes.GameObjs.Battle;

import NPCommon.Log.CommLog;
import NPCommon.RefData._IParseFromStringable;

public class WCGGTerrainIndex extends WCGBasicResIndexInfo implements _IParseFromStringable
{
    /************
     * 资源加载路径
     **/
    @Override
    public String assetPath()
    {
        return "terrain/terrain_" + mainId;
    }

    @Override
    public String objName()
    {
        return "terrain_" + mainId;
    }


    @Override
    public boolean parseFromString(String _str)
    {
        //拆分字符串后进行读取
        String[] strs = _str.split("\\_");

        if (strs.length < 1)
        {
            //Debug.LogWarning("没有配置 main id! columnName: " + _columnName);
            return false;
        }
        mainId = Integer.parseInt(strs[0].trim());

        if (strs.length < 2)
        {
            CommLog.error("没有配置 sub id!");
            return false;
        }

        subId = Integer.parseInt(strs[1].trim());
        return true;
    }
}
