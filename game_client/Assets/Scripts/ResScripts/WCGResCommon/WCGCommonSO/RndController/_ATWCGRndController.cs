using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/******************
 * 随机控制对象类
 **/
 [System.Serializable]
public abstract class _ATWCGRndController<T> where T : _IWCGRndItem
{
    /** 有概率队列 */
    public List<T> chanceList;
    /** 无概率队列 */
    public List<T> noChanceList;

    public _ATWCGRndController()
    {
        chanceList = new List<T>();
        noChanceList = new List<T>();
    }


    /****************
     * 添加一个概率对象
     **/
    public void addItem(T _item)
    {
        if (null == _item)
            return;

        if (_item.rndChance <= 0)
            noChanceList.Add(_item);
        else
            chanceList.Add(_item);
    }

    /******************
     * 获取随机对象
     **/
    public T getRndItem(_IWCGRndMaker _rndMaker, long _timeline)
    {
        if (null == _rndMaker)
            return defaultValue;

        if(chanceList.Count > 0)
        {
            //进行有概率随机
            int rndNum = _rndMaker.Random(10000, _timeline);
            T tmp = default(T);
            for(int i = 0; i < chanceList.Count; i++)
            {
                tmp = chanceList[i];
                if (null == tmp)
                    continue;

                rndNum -= tmp.rndChance;
                if (rndNum < 0)
                    return tmp;
            }
        }

        if (noChanceList.Count <= 0)
            return defaultValue;

        //进行无概率的随机
        return noChanceList[_rndMaker.Random(noChanceList.Count, _timeline)];
    }

    /***************
     * 默认值
     **/
    public abstract T defaultValue { get; }
}
