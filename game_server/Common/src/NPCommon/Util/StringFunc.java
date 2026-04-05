package NPCommon.Util;

import ALBasicProtocolPack._IALProtocolStructure;
import NPCommon.Log.CommLog;
import NPCommon.RefData._IParseFromStringable;
import NPCommon.Util.Pair.WCGPairLong;

import java.nio.charset.StandardCharsets;
import java.util.*;
import java.util.concurrent.ConcurrentHashMap;

public class StringFunc
{
    /******
     * 从字符串中解析对象列表，需要提供工厂类
     * @param _sData
     * @param _facory
     * @param <T>
     * @return
     */
    public static <T extends _IParseFromStringable> List<T> listFromString(String _sData, _IClassFactory<T> _facory)
    {
        return listFromString(_sData, ';', _facory);
    }

    public static <T extends _IParseFromStringable> List<T> listFromString(String _sData, char _token, _IClassFactory<T> _facory)
    {
        List<T> ret = new ArrayList<>();
        if (_sData == null || _sData.isEmpty())
        {
            return ret;
        }
        String[] strs = CommonFunc.charSplit(_sData, _token);
        for (String s : strs)
        {
            T obj = _facory.create();
            try
            {
                if (!s.trim().isEmpty())
                {
                    if (!obj.parseFromString(s))
                    {
                        continue;
                    }
                } else
                    continue;
            } catch (Exception e)
            {
                CommLog.error("Parse list failed ,src:{}", _sData, e);
                continue;
            }
            ret.add(obj);
        }
        return ret;

    }

    /******
     * 对象列表转成字符串
     * @param intList
     * @param _token
     * @param <T>
     * @return
     */
    public static <T> String list2String(Collection<T> intList, char _token)
    {
        if (intList.isEmpty())
            return "";
        try
        {
            StringBuilder sBuilder = new StringBuilder();
            for (T num : intList)
            {
                sBuilder.append(num.toString());
                sBuilder.append(_token);
            }

            sBuilder.deleteCharAt(sBuilder.length() - 1);
            return sBuilder.toString();
        } catch (Exception e)
        {
            return "";
        }
    }

    public static <T> String list2String(Collection<T> objList, char _token, ClassFormatter<T> _formater)
    {
        if (objList == null || objList.isEmpty())
            return "";
        StringBuilder sBuilder = new StringBuilder();
        for (T obj : objList)
        {
            sBuilder.append(_formater.getString(obj));
            sBuilder.append(_token);
        }
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }

    /***
     * 对象列表转成字符串，指定格式化类
     * @param objList
     * @param _formater
     * @param <T>
     * @return
     */
    public static <T> String list2String(Collection<T> objList, ClassFormatter<T> _formater)
    {
        return list2String(objList, ';', _formater);

    }

    public static <T> String list2String(Collection<T> intList)
    {
        return list2String(intList, ';');

    }

    /******
     * 数组转成字符串
     * @param arr
     * @param <T>
     * @return
     */
    public static <T> String listArray2String(T[] arr)
    {
        return listArray2String(arr, ';');
    }

    public static <T> String listArray2String(T[] arrs, char _token)
    {
        if (arrs == null || arrs.length == 0)
            return "";
        StringBuilder sBuilder = new StringBuilder();
        for (T obj : arrs)
        {
            sBuilder.append(obj.toString());
            sBuilder.append(_token);
        }
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }

    /*******
     * Map<Long,Long>转成字符串
     * @param _map
     * @return
     */
    public static String mapLongToString(Map<Long, Long> _map)
    {
        if (null == _map || _map.isEmpty())
        {
            return "";
        }
        StringBuilder sb = new StringBuilder();
        for (Map.Entry<Long, Long> entry : _map.entrySet())
        {
            sb.append(entry.getKey());
            sb.append(':');
            sb.append(entry.getValue());
            sb.append(';');
        }
        sb.deleteCharAt(sb.length() - 1);
        return sb.toString();
    }

    /****
     * Map<Integer,Integer>转成字符串
     * @param _map
     * @return
     */
    public static String mapIntToString(Map<Integer, Integer> _map)
    {
        if (null == _map || _map.isEmpty())
        {
            return "";
        }
        StringBuilder sb = new StringBuilder();
        for (Map.Entry<Integer, Integer> entry : _map.entrySet())
        {
            sb.append(entry.getKey());
            sb.append(':');
            sb.append(entry.getValue());
            sb.append(';');
        }
        sb.deleteCharAt(sb.length() - 1);
        return sb.toString();
    }

    /*****
     * 从字符串中解析Map<Long,Long>
     * @param _sData
     * @return
     */
    public static Map<Long, Long> mapLongFromString(String _sData)
    {
        Map<Long, Long> map = new ConcurrentHashMap<>();
        if (null == _sData || _sData.isEmpty())
            return map;
        try
        {
            String[] strs = CommonFunc.charSplit(_sData, new char[]{';'}, true);
            for (String str : strs)
            {
                WCGPairLong pair = WCGPairLong.fromString(str, ':');
                if (null != pair)
                {
                    map.put(pair.first(), pair.second());
                }
            }
        } catch (Exception e)
        {
            CommLog.error("mapLongFromString  parse failed,str =" + _sData, e);
        }
        return map;
    }

    /******
     * 从字符串中解析Map<Integer,Integer>
     * @param _sData
     * @return
     */
    public static Map<Integer, Integer> mapIntFromString(String _sData)
    {
        Map<Integer, Integer> map = new ConcurrentHashMap<>();
        if (null == _sData || _sData.isEmpty())
            return map;
        try
        {
            String[] strs = CommonFunc.charSplit(_sData, new char[]{';'}, true);
            for (String str : strs)
            {
                WCGPairLong pair = WCGPairLong.fromString(str, ':');
                if (null != pair)
                {
                    map.put((int) pair.first(), (int) pair.second());
                }
            }
        } catch (Exception e)
        {
            CommLog.error("mapLongFromString  parse failed,str =" + _sData, e);
        }
        return map;
    }

    /****
     * 从字符串中解析Short列表
     * @param _sData
     * @return
     */
    public static List<Short> listShortFromString(String _sData)
    {
        return listShortFromString(_sData, ';');
    }

    public static List<Short> listShortFromString(String _sData, char token)
    {
        List<Short> ret = new ArrayList<>();
        if (_sData == null || _sData.isEmpty())
        {
            return ret;
        }
        String[] strs = CommonFunc.charSplit(_sData, token);
        for (String s : strs)
        {
            short value = 0;
            try
            {
                if (!s.trim().isEmpty())
                {
                    value = Short.parseShort(s.trim());
                } else
                    continue;
            } catch (Exception e)
            {
                CommLog.error("Parse Long list failed ,src:{}", _sData, e);
                continue;
            }
            ret.add(value);
        }
        return ret;
    }

    /****
     * 从字符串中解析Integer列表
     * @param _sData
     * @return
     */
    public static List<Integer> listIntFromString(String _sData)
    {
        return listIntFromString(_sData, ';');
    }

    public static List<Integer> listIntFromString(String _sData, char token)
    {
        List<Integer> ret = new ArrayList<>();
        if (_sData == null || _sData.isEmpty())
        {
            return ret;
        }
        String[] strs = CommonFunc.charSplit(_sData, token);
        for (String s : strs)
        {
            int value = 0;
            try
            {
                if (!s.trim().isEmpty())
                {
                    value = Integer.parseInt(s.trim());
                } else
                    continue;
            } catch (Exception e)
            {
                CommLog.error("Parse Long list failed ,src:{}", _sData, e);
                continue;
            }
            ret.add(value);
        }
        return ret;
    }

    /****
     * 从字符串中解析Long列表
     * @param _sData
     * @return
     */
    public static List<Long> listLongFromString(String _sData)
    {
        return listLongFromString(_sData, ';');
    }

    public static List<Long> listLongFromString(String _sData, char token)
    {
        List<Long> ret = new ArrayList<>();
        if (_sData == null || _sData.isEmpty())
        {
            return ret;
        }
        String[] strs = CommonFunc.charSplit(_sData, token);
        for (String s : strs)
        {
            long value = 0L;
            try
            {
                value = Long.parseLong(s.trim());
            } catch (Exception e)
            {
                CommLog.error("Parse Long list failed ,src:{}", _sData, e);
            }
            ret.add(value);
        }
        return ret;
    }

    /****
     * 从字符串中解析String列表
     * @param _sData
     * @return
     */
    public static List<String> listStringFromString(String _sData)
    {
        return listStringFromString(_sData, ';');
    }

    public static List<String> listStringFromString(String _sData, char token)
    {
        List<String> ret = new ArrayList<>();
        if (_sData == null || _sData.isEmpty())
        {
            return ret;
        }
        String[] strs = CommonFunc.charSplit(_sData, token);
        ret.addAll(Arrays.asList(strs));
        return ret;
    }


    /****
     * 字符串拼接
     * @return
     */
    public static String joinString(List<String> _strList)
    {
        StringBuffer sb = new StringBuffer();
        for (String s : _strList)
        {
            sb.append(s);
        }
        return sb.toString();
    }

    public static String joinString(String join, String[] strAry)
    {
        StringBuffer sb = new StringBuffer();
        for (int i = 0; i < strAry.length; i++)
        {
            if (i == (strAry.length - 1))
            {
                sb.append(strAry[i]);
            } else
            {
                sb.append(strAry[i]).append(join);
            }
        }
        return sb.toString();
    }

    public static String joinString(char join, String... strAry)
    {
        StringBuffer sb = new StringBuffer();
        for (int i = 0; i < strAry.length; i++)
        {
            if (i == (strAry.length - 1))
            {
                sb.append(strAry[i]);
            } else
            {
                sb.append(strAry[i]).append(join);
            }
        }

        return sb.toString();
    }


    public static String joinString(String join, List<String> strAry)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < strAry.size(); i++)
        {
            if (i == (strAry.size() - 1))
            {
                sb.append(strAry.get(i));
            } else
            {
                sb.append(strAry.get(i)).append(join);
            }
        }

        return sb.toString();
    }

    /******
     * base64 解码字符串
     * @param str
     * @return
     */
    public static String decodeString(String str)
    {
        byte[] decode = Base64.getDecoder().decode(str);
        return new String(decode, StandardCharsets.UTF_8);
    }

    /******
     * base64 编码字符串
     * @param str
     * @return
     */
    public static String encodeString(String str)
    {
        byte[] bytes = str.getBytes(StandardCharsets.UTF_8);
        return Base64.getEncoder().encodeToString(bytes);
    }

    /********
     * 协议对象格式化为字符串
     * @param _proto
     * @return
     */
    public static String getString(_IALProtocolStructure _proto)
    {
        StringBuilder sb = new StringBuilder();
        CommonFunc.GetInfoPropertys(_proto, sb);
        return sb.toString();
    }

    /******
     * 指定大小，分割字符串为数组
     * @param _str
     * @param _size
     * @return
     */
    public static List<String> splitString(String _str, int _size)
    {
        List<String> retList = new ArrayList<>();
        int index = 0;
        while (index <= _str.length() - 1)
        {
            if (index + _size <= _str.length())
                retList.add(_str.substring(index, index + _size));
            else
            {
                retList.add(_str.substring(index));
                break;
            }
            index += _size;
        }
        return retList;

    }
}
