package NPUSServer.USRank;

import NPCommon.CommonRank.RankObj;
import NPCommon.CommonRank.RankObjComparer._ARankObjComparer;

public class CommRankComparer_ScoreGreater extends _ARankObjComparer
{
	@Override
    public int compareItemFunc(RankObj _item1, RankObj _item2)
    {
        // 添加null检查
        if (null == _item1)
            return -1;

        if (null == _item2)
            return 1;

        //分数相等处理:同等分数，先到达的对象排前面
        if (_item1.getScore() == _item2.getScore())
        {
            //时间戳:低->高排序
            return Long.compare(_item1.getUpdatedMs(), _item2.getUpdatedMs());
        }

        //分数:高->低排序
        return Long.compare(_item2.getScore(), _item1.getScore());
    }
}
