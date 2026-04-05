package NPGameRes.GameObjs.Hero;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData._IParseFromStringable;
import NPCommon.Util.CommonFunc;

import java.util.ArrayList;

public class HeroUpgradeStepMailReweardItem implements _IParseFromStringable
{
	private int _m_iHeroStep;
    private ArrayList<NPCommonCostItem> _m_alItemList;
    
    public HeroUpgradeStepMailReweardItem(int _heroStep)
    {
    	_m_iHeroStep = _heroStep;
    	_m_alItemList = new ArrayList<>();
    }

    public int getHeroStep() {return _m_iHeroStep;}
    public ArrayList<NPCommonCostItem> getItemList() {return _m_alItemList;}

    @Override
    public boolean parseFromString(String sValue)
    {
        try
        {
        	String[] strs = CommonFunc.charSplit(sValue, ';');

        	for(int i = 0; i < strs.length; i++)
            {
            	if(null == strs[i] || strs[i].isEmpty())
            		continue;
            	
            	NPCommonCostItem item = new NPCommonCostItem();
            	item.parseFromString(strs[i]);
            	_m_alItemList.add(item);
            }

            return true;
        } catch (Exception e)
        {
            return false;
        }
    }
}
