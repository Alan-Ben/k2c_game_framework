package NPCommon.Util;

/******
 * 实现该接口的类，可以把指定类型对象格式化成一个字符串
 * @param <T>
 */
@FunctionalInterface
public interface ClassFormatter<T>
{
    String getString(T _obj);
}

