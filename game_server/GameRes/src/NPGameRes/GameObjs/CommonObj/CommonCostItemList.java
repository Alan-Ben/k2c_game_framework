package NPGameRes.GameObjs.CommonObj;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData._IParseFromStringable;
import NPCommon.Util.CommonFunc;

import java.util.ArrayList;

public class CommonCostItemList implements _IParseFromStringable
{
    private ArrayList<NPCommonCostItem> _m_alCostItemList;
    
    public CommonCostItemList()
    {
    	_m_alCostItemList = new ArrayList<>();
    }
    
    public ArrayList<NPCommonCostItem> getCostItemList() {return _m_alCostItemList;}

    @Override
    public boolean parseFromString(String sValue)
    {
        try
        {
            String[] strs = CommonFunc.charSplit(sValue, ';');
            
            for(int i = 0; i < strs.length; i++)
            {
            	String itemStr = strs[i];
            	if(null == itemStr)
            		continue;
            	
            	NPCommonCostItem item = new NPCommonCostItem();
        		item.parseFromString(itemStr);
        		
        		_m_alCostItemList.add(item);
            }

            return true;
        } 
        catch (Exception e)
        {
            return false;
        }
    }
}
