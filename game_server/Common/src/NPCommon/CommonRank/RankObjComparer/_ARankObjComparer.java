package NPCommon.CommonRank.RankObjComparer;

import NPCommon.CommonRank.RankObj;

import java.util.List;

/**
 * @description: 排行榜数据比较对象
 * @author: ricci
 * @date: 2022-06-17 14:15:17
 */
public abstract class _ARankObjComparer
{
    /**
     * 通过排序方法重新排序整个排行
     */
    public void reRank(List<RankObj> _list)
    {
        if (null == _list)
            return;

        //使用子类提供的排序方法排序
        _list.sort(this::compareItemFunc);
    }

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
    public abstract int compareItemFunc(RankObj _item1, RankObj _item2);
}
