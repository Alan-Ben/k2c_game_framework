package NPCommon.RefData;

import ALServerLog.ALServerLog;

import java.util.List;

public class RefAssert
{

    /**
     * 校验配表长度是否一致
     * @param lista 第一个列表
     * @param listb 第二个列表
     * @param lists 剩余列表
     * @return
     */
    public static boolean listSize(List<?> lista, List<?> listb, List<?>... lists)
    {
        if (lista == null)
        {
            ALServerLog.Error("[RefAssert.listSize] list(1) 为空");
            return false;
        }
        if (listb == null)
        {
            ALServerLog.Error("[RefAssert.listSize] list(2) 为空");
            return false;
        }
        if (lista.size() != listb.size())
        {
            ALServerLog.Error("[RefAssert.listSize] list(1) 长度:{" + lista.size() + "}, list(2)长度:{" + listb.size() + "}");
            return false;
        }

        int size = lista.size();
        for (int i = 0; i < lists.length; ++i)
        {
            List<?> list = lists[i];
            if (list == null)
            {
                ALServerLog.Error("[RefAssert.listSize] list({" + (i) + "}) 为空");
                return false;
            }
            if (list.size() != size)
            {
                ALServerLog.Error("[RefAssert.listSize] list({" + i + "}) 长度:{" + list.size() + "}, 其他list长度:" + size);
                return false;
            }
        }
        return true;
    }

    /**
     * 校验配表字段的值是否在指定可选值范围内
     * @param value   要校验的值，可以是列表
     * @param options 范围值
     * @return
     */
    public static boolean inList(Object value, Object option1, Object... options)
    {
        if (value.getClass().isArray() || List.class.isAssignableFrom(value.getClass()))
        {
            for (Object v : (Iterable<?>) value)
            {
                if (!v.equals(option1) && !contained(options, v))
                {
                    return false;
                }
            }
        } else
        {
            return value.equals(option1) || contained(options, value);
        }
        return false;
    }

    /**
     * 校验配表字段的值是否在指定配表的指定字段的值
     * @param value     要校验的值，可以是列表
     * @param clazz     目标配表
     * @param fieldName 目标配表字段
     * @param option    可选值，若为可选值则跳过校验
     * @return
     */
//    public static boolean inRef(Object value, Class<? extends RefBase> clazz, String fieldName, Object... option) {
//        Field field = null;
//        try {
//            field = clazz.getField(fieldName);
//        } catch (Exception e) {
//            ALServerLog.Error("校验配表数值失败：[{"+clazz.getSimpleName()+"}]表没有字段[{"+fieldName+"}]");
//            return false;
//        }
//        RefField refField = field.getAnnotation(RefField.class);
//        boolean iskey = refField != null && refField.iskey();
//        if (value.getClass().isArray() || List.class.isAssignableFrom(value.getClass())) {
//            List<Object> list = iskey ? new ArrayList<>() : getRefValues(clazz, field);
//            for (Object v : (Iterable<?>) value) {
//                if (contained(option, v)) {
//                    continue;
//                }
//                if (iskey && AbstractRefDataMgr.get(clazz, v) == null) {
//                    ALServerLog.Error("校验配表数值失败, 配表数值[{"+v+"}]不存在于[{"+clazz.getSimpleName()+"}]表的[{"+fieldName+"}]字段中");
//                    return false;
//                } else if (!iskey && !list.contains(v)) {
//                    ALServerLog.Error("校验配表数值失败, 配表数值[{"+v+"}]不存在于[{"+clazz.getSimpleName()+"}]表的[{"+fieldName+"}]字段中");
//                    return false;
//                }
//            }
//        } else {
//            if (contained(option, value)) {
//                return true;
//            }
//            if (iskey && AbstractRefDataMgr.get(clazz, value) == null) {
//                ALServerLog.Error("校验配表数值失败, 配表数值[{"+value+"}]不存在于[{"+clazz.getSimpleName()+"}]表的[{"+fieldName+"}]字段中");
//                return false;
//            } else if (!iskey && !getRefValues(clazz, field).contains(value)) {
//                ALServerLog.Error("校验配表数值失败, 配表数值[{"+value+"}]不存在于[{"+clazz.getSimpleName()+"}]表的[{"+fieldName+"}]字段中");
//                return false;
//            }
//        }
//        return true;
//    }

//    private static List<Object> getRefValues(Class<? extends RefBase> clazz, Field field) {
//        RefContainer<?> container = AbstractRefDataMgr.getAll(clazz);
//        List<Object> list = new ArrayList<>();
//        for (Object ref : container.values()) {
//            try {
//                list.add(field.get(ref));
//            } catch (Exception e) {
//                ALServerLog.Error("校验配表数值失败：[{"+clazz.getSimpleName()+"}]表没有字段[{"+field.getName()+"}]");
//            }
//        }
//        return list;
//    }
    private static boolean contained(Object[] array, Object v)
    {
        for (Object a : array)
        {
            if (v.equals(a))
            {
                return true;
            }
        }
        return false;
    }

}
