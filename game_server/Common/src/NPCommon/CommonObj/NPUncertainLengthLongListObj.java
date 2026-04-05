package NPCommon.CommonObj;

import NPCommon.RefData._IParseFromStringable;

import java.util.ArrayList;

/**
 * @description: 用':'分割 的不定长 Long数据列表内容
 */
public class NPUncertainLengthLongListObj implements _IParseFromStringable
{
    //刷新类型
    private ArrayList<Long> _m_list = new ArrayList<>();

    @Override
    public boolean parseFromString(String _sValue)
    {
        try
        {
            String[] longStrList = _sValue.split(":");
            for (String longStr : longStrList)
            {
                Long val = Long.valueOf(longStr);
                _m_list.add(val);
            }
        } catch (Exception e)
        {
            e.printStackTrace();
        }

        return true;
    }
}
