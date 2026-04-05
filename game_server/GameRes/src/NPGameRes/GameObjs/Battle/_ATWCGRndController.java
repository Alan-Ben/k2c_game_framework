package NPGameRes.GameObjs.Battle;

import java.util.ArrayList;
import java.util.List;


/******************
 * 随机控制对象类
 **/
public abstract class _ATWCGRndController<T extends _IWCGRndItem>
{
    /**
     * 有概率队列
     */
    public List<T> chanceList;
    /**
     * 无概率队列
     */
    public List<T> noChanceList;

    public _ATWCGRndController()
    {
        chanceList = new ArrayList<T>();
        noChanceList = new ArrayList<T>();
    }


    /****************
     * 添加一个概率对象
     **/
    public void addItem(T _item)
    {
        if (null == _item)
            return;

        if (_item.rndChance() <= 0)
            noChanceList.add(_item);
        else
            chanceList.add(_item);
    }

    /******************
     * 获取随机对象
     **/
    public T getRndItem(_IWCGRndMaker _rndMaker, long _timeline)
    {
        if (null == _rndMaker)
            return defaultValue();

        if (chanceList.size() > 0)
        {
            //进行有概率随机
            int rndNum = _rndMaker.Random(10000, _timeline);
            T tmp = null;
            for (int i = 0; i < chanceList.size(); i++)
            {
                tmp = chanceList.get(i);
                if (null == tmp)
                    continue;

                rndNum -= tmp.rndChance();
                if (rndNum < 0)
                    return tmp;
            }
        }

        if (noChanceList.size() <= 0)
            return defaultValue();

        //进行无概率的随机
        return noChanceList.get(_rndMaker.Random(noChanceList.size(), _timeline));
    }

    /***************
     * 默认值
     **/
    public abstract T defaultValue();
}
