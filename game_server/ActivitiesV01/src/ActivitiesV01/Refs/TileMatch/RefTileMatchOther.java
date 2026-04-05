package ActivitiesV01.Refs.TileMatch;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefField;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefSingleContainer;
import NPCommon.Util.Pair.WCGPairInt;

import java.util.List;

@RefTable(ignore = true)
public class RefTileMatchOther extends RefBase
{
    private static RefTileMatchOtherMgr _g_mgr = new RefTileMatchOtherMgr();

    public static RefTileMatchOtherMgr getMgr()
    {
        return _g_mgr;
    }

    public static RefTileMatchOther Ref()
    {
        return getMgr().getRef();
    }

    public static class RefTileMatchOtherMgr extends RefSingleContainer<RefTileMatchOther>
    {
        protected RefTileMatchOtherMgr()
        {
            super();

            //设置新的值
            initPut(new RefTileMatchOther());
        }
    }

    //////////////////////////////


    @Override
    public RefTileMatchOtherMgr getStaticContainer()
    {
        return _g_mgr;
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTileMatchOtherMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefTileMatchOther newRef = (RefTileMatchOther) _newRef;
        tilematch_continuous_ext_score = newRef.tilematch_continuous_ext_score;
        tilematch_lazy_cd_id = newRef.tilematch_lazy_cd_id;
        tilematch_map_size = newRef.tilematch_map_size;
        tilematch_step_reward_mail_id = newRef.tilematch_step_reward_mail_id;
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
        return 0;
    }

    //general_process覆盖完成后调用
    public boolean NewAssert()
    {
        return true;
    }

    ////////////////////////////// NP留存配置 //////////////////////////////

    public List<Integer> tilematch_continuous_ext_score;//连消增加分数列表
    public long tilematch_lazy_cd_id;//三消体力道具id
    public WCGPairInt tilematch_map_size = new WCGPairInt(7, 6);//棋盘大小(行:列)
    public long tilematch_step_reward_mail_id;//三消阶段奖励补发邮件id
    @RefField(isIgnore = true)
    public int boom_arg = 1; //爆炸球爆炸半径
    @RefField(isIgnore = true)
    public int boom_boom_arg = 2; //爆炸球爆炸半径
}
