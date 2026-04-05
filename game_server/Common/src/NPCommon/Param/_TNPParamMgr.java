package NPCommon.Param;

import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;

/***************
 * 参数管理对象
 * @author mj
 *
 * @param <ENUM>
 */
public class _TNPParamMgr<D, P extends _ATNPParamBase<D>>
{
    private Map<Integer, P> params = new ConcurrentHashMap<Integer, P>();

    public P getParam(int _eParam)
    {
        return params.get(_eParam);
    }

    public void registParam(P param)
    {
        if (null == param)
            return;

        params.put(param.getIndex(), param);
    }

    /**************
     * 循环迭代每个对象处理
     * @param _iterator
     */
    public void dealItorator(_ATParamItorator<D, P> _iterator)
    {
        if (null == _iterator)
            return;

        //每个对象进行处理
        for (P param : params.values())
        {
            _iterator.dealParam(param);
        }
    }
}
