package NPGameRes.UsHotRefDataMgr.HotLoaders;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.Log.CommLog;

import java.util.ArrayList;

public class ParseUtil
{
    /**
     * 解析NPCommonCostItem列表
     *
     * 格式: "itemType-itemId:count;itemType-itemId:count"
     * 分隔符: 分号(;)分隔每个item
     * 使用 NPCommonCostItem.parseFromString() 解析每个元素
     */
    public static ArrayList<NPCommonCostItem> parseCommonCostItemList(String value)
    {
        ArrayList<NPCommonCostItem> result = new ArrayList<>();
        if (value == null || value.isEmpty())
            return result;

        try
        {
            String[] parts = value.split(";");
            for (String part : parts)
            {
                if (!part.isEmpty())
                {
                    NPCommonCostItem item = new NPCommonCostItem();
                    if (item.parseFromString(part))
                    {
                        result.add(item);
                    }
                }
            }
        }
        catch (Exception e)
        {
            CommLog.error("parse common cost item list failed: {}", value, e);
        }

        return result;
    }
}
