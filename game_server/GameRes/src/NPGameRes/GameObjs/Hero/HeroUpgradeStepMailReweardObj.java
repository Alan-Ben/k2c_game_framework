package NPGameRes.GameObjs.Hero;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.Log.CommLog;
import NPCommon.RefData._IParseFromStringable;
import NPCommon.Util.CommonFunc;

import java.util.ArrayList;

/*******************************
 * 
 * 字段来源：hero.upgrade_step_mail_reward_item_list
 * 骑士升阶邮件奖励（从第2个阶段开始, 共7个阶段，阶段之间用“|"划分）
 * 举例： bag_item-1:1;bag_item-2:1|bag_item-1:1|bag_item-1:1||||
 * 
 * @author mj
 *
 */
public class HeroUpgradeStepMailReweardObj implements _IParseFromStringable
{
	private ArrayList<HeroUpgradeStepMailReweardItem> _m_alStepRewardList;
	
	public HeroUpgradeStepMailReweardObj()
	{
		_m_alStepRewardList = new ArrayList<>();
	}
	
    @Override
    public boolean parseFromString(String sValue)
    {
    	try
    	{
        	if (null == sValue || sValue.trim().isEmpty())
                return true;

        	String[] strs = CommonFunc.charSplit(sValue, '|');
            
            for(int i = 0; i < strs.length; i++)
            {
            	//骑士升阶邮件奖励（从第2个阶段开始, 共7个阶段，阶段之间用“|"划分）
            	int step = 2 + i;
            	
            	HeroUpgradeStepMailReweardItem reward = new HeroUpgradeStepMailReweardItem(step);
            	if(null != strs[i] && !strs[i].isEmpty())
            	{
            		reward.parseFromString(strs[i]);
            	}
            	_m_alStepRewardList.add(reward);
            }
            
        	return true;
    	}
    	catch(Exception e)
    	{
    		CommLog.error("", e);
    		return false;
    	}
    }
    
    /**
     * 获取指定大臣阶段的邮件奖励物品
     * @param _heroStep
     * @return
     */
    public ArrayList<NPCommonCostItem> getRewardByStep(int _heroStep)
    {
    	for(int i = 0; i < _m_alStepRewardList.size(); i++)
    	{
    		HeroUpgradeStepMailReweardItem reward = _m_alStepRewardList.get(i);
    		if(null == reward)
    			continue;
    		
    		if(reward.getHeroStep() == _heroStep)
    			return reward.getItemList();
    	}
    	
    	return null;
    }
}
