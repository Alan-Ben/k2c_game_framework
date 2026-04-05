package NPCommon.Util;

import java.lang.reflect.Field;

//反射辅助类型，提供热更相关辅助方法
public class CommFix
{

    /*****
     * 通过成员名字，取得对象的成员Field类型,不能获得父类的或重载的成员
     * @param _info
     * @param _filedName
     * @return
     */
    public static Object getFieldObj(Object _info, String _filedName)
    {

        try
        {
            Field field = _info.getClass().getDeclaredField(_filedName);
            field.setAccessible(true);
            Object retObject = field.get(_info);
            field.setAccessible(false);
            return retObject;
        } catch (Throwable e)
        {
            return null;
        }
    }

    /******
     * A对象是B对象的成员，返回该成员的Field类型。
     * @param _info
     * @param _fieldObj
     * @return
     */
    public static Field getFieldByObj(Object _info, Object _fieldObj)
    {

        try
        {
            for (Field field : _info.getClass().getDeclaredFields())
            {
                field.setAccessible(true);
                Object o = field.get(_info);
                if (o == _fieldObj)
                    return field;
            }
            return null;
        } catch (Throwable e)
        {
            return null;
        }
    }

}
