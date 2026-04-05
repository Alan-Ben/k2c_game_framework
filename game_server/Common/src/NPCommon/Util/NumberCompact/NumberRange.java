package NPCommon.Util.NumberCompact;

import NPCommon.RefData._IParseFromStringable;

import java.util.List;

/**
 * 数字范围判断
 */
public class NumberRange implements _IParseFromStringable
{
    public int startNum;
    public int endNum;

    public NumberRange()
    {
    }

    public NumberRange(int _startNum, int _endNum)
    {
        startNum = _startNum;
        endNum = _endNum;
    }

    /**
     * 检查数字是否在范围内
     * @param number 待检查数字
     * @return 是否在范围内
     */
    public boolean contains(int number) {
        return number >= startNum && number <= endNum;
    }

    /**
     * 填充数字列表
     * @param _list 待填充列表
     */
    public void fillNumberList(List<Integer> _list)
    {
        for (int i = startNum; i <= endNum; i++)
        {
            _list.add(i);
        }
    }

    /**
     * 检查数字是否可以合并到此范围
     * @param _number 待检查数字
     * @return 是否可以合并
     */
    public boolean canMerge(int _number)
    {
        return _number == endNum + 1 || _number == startNum - 1;
    }

    /**
     * 合并数字到此范围
     * @param _number 待合并数字
     */
    public void merge(int _number)
    {
        if (_number == endNum + 1)
        {
            endNum = _number;
        } else if (_number == startNum - 1)
        {
            startNum = _number;
        }
    }

    /**
     * 检查两个范围是否可以合并
     * @param _other 待检查范围
     * @return 是否可以合并
     */
    public boolean canMerge(NumberRange _other)
    {
        return endNum + 1 == _other.startNum || _other.endNum + 1 == startNum;
    }

    /**
     * 合并另一个范围到此范围
     * @param _other 待合并范围
     */
    public void merge(NumberRange _other)
    {
        if (this.canMerge(_other))
        {
            startNum = Math.min(startNum, _other.startNum);
            endNum = Math.max(endNum, _other.endNum);
        }
    }

    @Override
    public String toString()
    {
        return startNum == endNum ? String.valueOf(startNum) : startNum + "-" + endNum;
    }

    @Override
    public boolean parseFromString(String _str)
    {
        if (_str == null || _str.isEmpty())
            return false;

        String[] nums = _str.split("-");
        if (nums.length == 1)
        {
            startNum = Integer.parseInt(nums[0]);
            endNum = Integer.parseInt(nums[0]);
        } else if (nums.length == 2)
        {
            startNum = Integer.parseInt(nums[0]);
            endNum = Integer.parseInt(nums[1]);
        } else
        {
            return false;
        }
        return true;
    }
}