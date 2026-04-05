package ActivitiesV02.Activities.RegularActivity;

import ActivitiesV02.Events.Event_P_NUM_MERGE_NEW_BLOCK;
import NPEnum.ENPPlayerVariableVarType;
import NPUSServer.Common.Event.EventParamVarTypeMap;
import NPUSServer.HotActivity._AActivityInitializer;

public class NumMergeActivityInitializer extends _AActivityInitializer
{
    @Override
    protected int getActivityType()
    {
        return NumMergeActivity.s_typeId;
    }

    @Override
    public boolean init()
    {
        //2048新方块生成  参数映射
        EventParamVarTypeMap.getInstance().reg(Event_P_NUM_MERGE_NEW_BLOCK.ID,"ID", ENPPlayerVariableVarType.ID);
        EventParamVarTypeMap.getInstance().reg(Event_P_NUM_MERGE_NEW_BLOCK.ID,"COUNT", ENPPlayerVariableVarType.COUNT);

        return true;
    }
}
