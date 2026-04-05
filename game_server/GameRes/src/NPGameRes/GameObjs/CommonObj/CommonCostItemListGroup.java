package NPGameRes.GameObjs.CommonObj;

import NPCommon.RefData._IParseFromStringable;
import NPCommon.Util.CommonFunc;

import java.util.ArrayList;

public class CommonCostItemListGroup implements _IParseFromStringable
{
    private ArrayList<CommonCostItemList> _m_alCostItemListGroup;
    
    public CommonCostItemListGroup()
    {
    	_m_alCostItemListGroup = new ArrayList<>();
    }
    
    public ArrayList<CommonCostItemList> getCostItemListGroup() {return _m_alCostItemListGroup;}

    @Override
    public boolean parseFromString(String sValue)
    {
        try
        {
            String[] strs = CommonFunc.charSplit(sValue, '|');
            
            for(int i = 0; i < strs.length; i++)
            {
            	String itemListGroupStrs = strs[i];
            	if(null == itemListGroupStrs)
            		continue;
            	
            	CommonCostItemList listGroup = new CommonCostItemList();
            	listGroup.parseFromString(itemListGroupStrs);
            	
            	_m_alCostItemListGroup.add(listGroup);
            }

            return true;
        } 
        catch (Exception e)
        {
            return false;
        }
    }
}
