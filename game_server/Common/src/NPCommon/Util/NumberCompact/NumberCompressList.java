package NPCommon.Util.NumberCompact;

import ALBasicServer.ALBasicMutex.MutexAtom;
import NPCommon.RefData._IParseFromStringable;
import NPCommon.Util.CommonFunc;

import java.util.ArrayList;
import java.util.Comparator;
import java.util.List;

/**
 * 数字压缩列表
 * -------------------------------------------
 * 实现了数字的压缩存储，可以将一组连续的数字压缩成一个范围
 * 例如：1,2,3,4,5,6,7,8,9,10,11,12,13,14
 * 压缩后为：1-14
 * ----------------------------------
 * 如果包含不连续的数字，会分割成多个范围
 * 例如：1,2,3,4,5,6,7,8,9,10,11,12,13,14,20,21,22,23,24,25
 * 压缩后为：1-14;20-25
 */
public class NumberCompressList implements _IParseFromStringable
{
    private List<NumberRange> _m_rangeList;
    private MutexAtom _m_mutex;

    public NumberCompressList()
    {
        _m_rangeList = new ArrayList<>();
        _m_mutex = new MutexAtom();

        _m_mutex.reducePriority(1000);
    }

    public void _lock()
    {
        _m_mutex.lock();
    }

    public void _unlock()
    {
        _m_mutex.unlock();
    }

    /**
     * 获取所包含的数字列表
     * @return 数字列表
     */
    public List<Integer> getNumberList()
    {
        _lock();
        try{
            List<Integer> list = new ArrayList<>();
            for (NumberRange range : _m_rangeList)
            {
                range.fillNumberList(list);
            }
            return list;
        }finally{
            _unlock();
        }
    }

    /**
     * 新增数字到列表
     * @param _number 待新增数字
     */
    public boolean addNumber(int _number)
    {
        _lock();
        try{
            //如果已经包含，不需要再添加
            if (containsNumber(_number))
                return false;

            NumberRange newRange = new NumberRange(_number, _number);
            NumberRange toMerge = null;

            for (NumberRange range : _m_rangeList)
            {
                if (range.canMerge(_number))
                {
                    toMerge = range;
                    break;
                }
            }

            if (toMerge != null)
            {
                toMerge.merge(_number);
                //尝试合并范围
                _mergeRangesIfNeeded();
            } else
            {
                _addNewRange(newRange);
            }

            return true;
        }finally{
            _unlock();
        }
    }

    /**
     * 新增范围到列表
     */
    private void _addNewRange(NumberRange _newRange)
    {
        _lock();
        try{
            _m_rangeList.add(_newRange);
            //按照起始数字排序
            _m_rangeList.sort(Comparator.comparingInt(o -> o.startNum));
        }finally{
            _unlock();
        }
    }

    /**
     * 移除数字
     * @param _number 待移除数字
     */
    public boolean removeNumber(int _number) {
        _lock();
        try{
            NumberRange rangeToRemoveFrom = null;
            //找到包含数字的范围
            for (NumberRange range : _m_rangeList) {
                if (range.contains(_number)) {
                    rangeToRemoveFrom = range;
                    break;
                }
            }

            if (rangeToRemoveFrom == null)
                return false;

            //先处理数字在范围两端的情况
            if (rangeToRemoveFrom.startNum == _number) {
                rangeToRemoveFrom.startNum++;
                if (rangeToRemoveFrom.startNum > rangeToRemoveFrom.endNum) {
                    _m_rangeList.remove(rangeToRemoveFrom);
                }
            } else if (rangeToRemoveFrom.endNum == _number) {
                rangeToRemoveFrom.endNum--;
            } else {
                //如果数字在范围中间，需要拆分范围
                NumberRange newRange = new NumberRange(_number + 1, rangeToRemoveFrom.endNum);
                rangeToRemoveFrom.endNum = _number - 1;

                //新增范围
                _addNewRange(newRange);
            }

            return true;
        }finally{
            _unlock();
        }
    }

    /**
     * 判断数字是否被包含
     * @return 是否被包含
     */
    public boolean containsNumber(int _number) {
        _lock();
        try{
            for (NumberRange range : _m_rangeList) {
                if (range.contains(_number)) {
                    return true;
                }
            }
            return false;
        }finally{
            _unlock();
        }
    }

    /**
     * 合并连续的范围
     */
    private void _mergeRangesIfNeeded()
    {
        _lock();
        try{
            for (int i = 0; i < _m_rangeList.size() - 1; i++)
            {
                NumberRange current = _m_rangeList.get(i);
                NumberRange next = _m_rangeList.get(i + 1);

                if (current.canMerge(next))
                {
                    current.merge(next);
                    _m_rangeList.remove(next);
                    //如果移除了一个元素，需要调整索引
                    i--;
                }
            }
        }finally{
            _unlock();
        }
    }

    /**
     * 获取最大值
     * @return
     */
    public int getMaxNum()
    {
        _lock();
        try{
            int maxNum = 0;
            for (NumberRange range : _m_rangeList)
            {
                if (range.endNum > maxNum)
                    maxNum = range.endNum;
            }
            return maxNum;
        }finally{
            _unlock();
        }
    }

    /**
     * 清空列表
     */
    public void clear()
    {
        _lock();
        try{
            _m_rangeList.clear();
        }finally{
            _unlock();
        }
    }

    @Override
    public String toString()
    {
        return CommonFunc.list2String(_m_rangeList);
    }

    @Override
    public boolean parseFromString(String _str)
    {
        if (_str == null || _str.isEmpty())
            return false;

        String[] strList = CommonFunc.charSplit(_str, ';');
        for (String str : strList)
        {
            NumberRange range = new NumberRange();
            if (!range.parseFromString(str))
            {
                return false;
            }
            _m_rangeList.add(range);
        }
        return true;
    }
}