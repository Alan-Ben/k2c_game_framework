package NPCommon.CommonRank.RankObjComparer;

import NPCommon.CommonRank.RankObj;

/**
 * @description: 排行榜数据比较对象，逆向排序
 * @author: ricci
 * @date: 2022-06-17 14:15:17
 */
public class RankObjComparerNegative extends _ARankObjComparer
{
    /**
     * 排行的比较方法.
     * 要求
     * item1 < item2 返回 -1
     * item1 = item2 返回 0
     * item1 > item2 返回1
     * @param _item1 元素1
     * @param _item2 元素2
     * @return boolean
     */
    @Override
    public int compareItemFunc(RankObj _item1, RankObj _item2)
    {
        //数据异常则直接返回一个结果
        if (null == _item1)
            return -1;

        if (null == _item2)
            return 1;

        //先按照分数排序
        if (_item1.getScore() < _item2.getScore())
        {
            return 1;
        } else if (_item1.getScore() == _item2.getScore())
        {
            //分数相等时，按照时间排序
            return Long.compare(_item1.getUpdatedMs(), _item2.getUpdatedMs());
        } else
        {
            return -1;
        }
    }
}
