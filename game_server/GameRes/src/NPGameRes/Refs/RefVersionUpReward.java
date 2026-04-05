package NPGameRes.Refs;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefListContainer;

import java.util.ArrayList;
import java.util.Collections;
import java.util.Comparator;

@RefTable(tableName = "version_up_reward")
public class RefVersionUpReward extends RefBase
{
    private static RefVersionUpRewardMgr _g_mgr = new RefVersionUpRewardMgr();

    public static RefVersionUpRewardMgr getMgr()
    {
        return _g_mgr;
    }

    public static class RefVersionUpRewardMgr extends RefListContainer<RefVersionUpReward>
    {
        @Override
        public void onLoaded()
        {
            Collections.sort(getList(), new Comparator<RefVersionUpReward>()
            {
                @Override
                public int compare(RefVersionUpReward o1, RefVersionUpReward o2)
                {
                    return (int) (o2.div_num - o1.div_num);
                }
            });
        }
    }

    @Override
    public RefVersionUpRewardMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefVersionUpRewardMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefVersionUpReward newRef = (RefVersionUpReward) _newRef;
        div_num = newRef.div_num;
        reward_list = newRef.reward_list;
    }

    /**********
     * 获取对象数据Id，尽量唯一
     *
     * @author alzq.z
     * @time 2019年4月3日 下午11:35:24
     */
    @Override
    public long Id()
    {
        return div_num;
    }

    //div_num	对比版本号时的除数（版本比对将用当前版本号和记录版本号同时除以这个除数来进行对比）例如比对是否最大版本不同则使用10000，比对次版本则用100
    public long div_num;//一般只配置为100,10000,1000000三种数据

    public ArrayList<Long> reward_list = new ArrayList<Long>(); //rewardid 列表

}
