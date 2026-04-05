package NPCommon.Util.Delegate;

/***
 * 监听函数的基类
 */
public class HandlerBase
{
    public boolean isMatchName(String matchName)
    {
        String objName = toString();
        return objName.startsWith(matchName);
    }
}
