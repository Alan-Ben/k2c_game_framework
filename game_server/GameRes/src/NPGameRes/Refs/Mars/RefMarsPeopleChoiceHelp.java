package NPGameRes.Refs.Mars;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefField;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPCommon.Util.CommonFunc;

import java.util.ArrayList;
import java.util.List;

@RefTable(tableName = "mars_people_choice_help")
public class RefMarsPeopleChoiceHelp extends RefBase
{
    private static RefMarsPeopleHelpMgr _g_mgr = new RefMarsPeopleHelpMgr();

    public static RefMarsPeopleHelpMgr getMgr()
    {
        return _g_mgr;
    }
    
    @Override
    public RefMarsPeopleHelpMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefMarsPeopleHelpMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefMarsPeopleChoiceHelp newRef = (RefMarsPeopleChoiceHelp) _newRef;
        id = newRef.id;
        option_add_satisfaction_degree_list = newRef.option_add_satisfaction_degree_list;
        option_reward_id_list = newRef.option_reward_id_list;
    }

    @Override
    public long Id()
    {
        return id;
    }

    public static class RefMarsPeopleHelpMgr extends RefTableContainer<RefMarsPeopleChoiceHelp>
    {
        // 配表加载完成后的处理逻辑
        @Override
        protected void _onTableLoaded()
        {
        }
        
        public RefMarsPeopleChoiceHelp rndRefList()
        {
        	List<RefMarsPeopleChoiceHelp> list = getList();
        	if(list.isEmpty())
        		return null;
        	
			int idx = CommonFunc.randomInt(list.size() - 1);
			return list.get(idx);
        }
    }

    public long id;
    public ArrayList<Integer> option_add_satisfaction_degree_list = new ArrayList<>();//选项增加满意度列表
    public ArrayList<Long> option_reward_id_list = new ArrayList<>();//选项获取奖励列表

    @RefField(isIgnore = true)
    public RefMarsPeopleHelp helpRef;
    
    @RefField(isIgnore = true)
    public int optionCount;
    
    public long getOptionRewardId(int _idx)
    {
    	ArrayList<Long> list = this.option_reward_id_list;
    	
    	if(_idx < 0 || _idx >= list.size())
    		return 0;
    	
    	return list.get(_idx);
    }
    
    public int getOptionSatisfaction(int _idx)
    {
    	ArrayList<Integer> list = this.option_add_satisfaction_degree_list;
    	
    	if(_idx < 0 || _idx >= list.size())
    		return 0;
    	
    	return list.get(_idx);
    }
}