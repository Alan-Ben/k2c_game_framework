package NPCommon.ErrMain.Result;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommClass;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;

/*******
 * 预定义错误管理器，在系统初始化的时候把预定一的错误结果注册到本对象，可以通过错误id来统一检索错误信息
 */
public class ResultMgr
{
    private static ResultMgr _instance = new ResultMgr();

    public static ResultMgr getInstance()
    {
        return _instance;
    }

    public ResultMgr()
    {
        regist(Result.SUCC);
    }

    private Map<Integer, Result> _m_map = new ConcurrentHashMap<>();//错误对象管理字典,errCode->Result

    /********
     * 注册一个错误结果
     * @param _result
     */
    public void regist(Result _result)
    {
        _m_map.put(_result.getCode(), _result);
    }

    /*****
     * 根据错误码，查找预定的错误
     * @param _code
     * @return
     */
    public Result lookupResult(int _code)
    {
        Result result = _m_map.get(_code);
        if (result != null)
            return result;

        return BasicServerErr.UNKNOW_ERR;
    }

    /****
     * 返回所有的预定义错误对象
     * @return
     */
    public List<Result> getResultList()
    {
        return new ArrayList<>(_m_map.values());
    }

    /**
     * 注册指定包下的错误对象
     * @param _packageName 包名
     */
    public void registByPackage(String _packageName)
    {
        List<Class<?>> classList = CommClass.getAllClassByInterface(_IErrHolder.class, _packageName);
        for (Class<?> clazz : classList)
        {
            try
            {
                _IErrHolder errHolder = (_IErrHolder) clazz.newInstance();
            } catch (Exception e)
            {
                CommLog.error("", e);
            }
        }
    }
}
