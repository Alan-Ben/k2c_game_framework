package NPCommon.Util;

import ALBasicCommon.ALBasicCommonFun;
import ALBasicProtocolPack._IALProtocolStructure;
import Common.Common_Context;
import Common.Common_Lineup;
import Common.NpServerObj.NpServerObj_PlatFormMailText;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.CommonObj.NPCommonItem;
import NPCommon.Log.CommLog;
import NPCommon.NPCommon_BattleCardInfo;
import NPCommon.NPCommon_ItemInfo;
import NPCommon.NPCommon_PlayerFightInfo;
import NPCommon.NPVersion;
import NPCommon.RefData._IParseFromStringable;
import NPCommon.Util.Pair.WCGPairInt;
import NPCommon.Util.Pair.WCGPairIntList;
import NPEnum.ENPGameEvent;
import NPEnum.ENPItemType;
import NPEnum.EQuality;
import SevenZip.Zipper;
import WCGCommon.Enum.NPEnum.EWCGActorType;
import WCGCommon.Enum.NPEnum.EWCGLocalPushContentType;
import WCGCommon.Security.Base64;
import com.google.gson.Gson;
import com.google.gson.JsonElement;
import com.google.gson.JsonObject;
import com.google.gson.JsonParser;

import java.io.UnsupportedEncodingException;
import java.lang.management.ManagementFactory;
import java.lang.reflect.Method;
import java.lang.reflect.Type;
import java.net.InetAddress;
import java.nio.ByteBuffer;
import java.nio.charset.StandardCharsets;
import java.text.SimpleDateFormat;
import java.util.*;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

public class CommonFunc
{
    final private static Gson _m_gson = new Gson();

    public static Gson googleJson() {return _m_gson;}

    public static <T> T fromJson(String _sValue, Class<T> clazz)
    {
        T ret = _m_gson.fromJson(_sValue, clazz);
        return ret;
    }

    public static <T> T fromJson(String _sValue, Type type)
    {
        //new TypeToken<>()
        //java.lang.reflect.Type type = new TypeToken<String>(){}.getType();
        T ret = _m_gson.fromJson(_sValue, type);
        return ret;
    }

    /*由于Java是基于Unicode编码的，因此，一个汉字的长度为1，而不是2。
     * 但有时需要以字节单位获得字符串的长度。例如，“123abc长城”按字节长度计算是10，而按Unicode计算长度是8。
     * 为了获得10，需要从头扫描根据字符的Ascii来获得具体的长度。如果是标准的字符，Ascii的范围是0至255，如果是汉字或其他全角字符，Ascii会大于255。
     * 因此，可以编写如下的方法来获得以字节为单位的字符串长度。*/
    public static int getStringCount(String s)
    {
        int length = 0;
        for (int i = 0; i < s.length(); i++)
        {
            int ascii = Character.codePointAt(s, i);
            if (ascii >= 0 && ascii <= 255)
                length++;
            else
                length += 2;

        }
        return length;

    }


    /**
     * 生成随即密码
     * @param _len 生成的密码的总长度
     * @return 密码的字符串
     */
    public static String genRandomStr(int _len)
    {
        byte[] byteArr = new byte[_len / 2];
        for (int i = 0; i < byteArr.length; i++)
        {
            byteArr[i] = (byte) ALBasicCommonFun.getRandomInt(256);
        }
        //返回转化而成的字符串
        return ALBasicCommonFun.getHexString(byteArr);
    }

    /**
     * 产生一个处于[0,Delta]之间的随机整数
     * @param iDelta
     * @return
     */
    public static int randomInt(int iDelta)
    {
        return Math.abs(Random.nextInt(iDelta + 1));
    }

    /**
     * 产生一个处于[0,Delta]之间的随机整数(自定义随机数生成器)
     * @param iDelta  最大值
     * @param _random 随机数生成器
     * @return
     */
    public static int randomInt(int iDelta, java.util.Random _random)
    {
        return Math.abs(Random.nextInt(iDelta + 1, _random));
    }

    public static final int INT_MAX_LESS = Integer.MAX_VALUE - 1;

    /**
     * 产生一个处于[0,Delta]之间的随机整数
     * @param iDelta
     * @return
     */
    public static long randomLong(long iDelta)
    {
        if (iDelta <= INT_MAX_LESS)
        {
            return randomInt((int) iDelta);
        } else
        {
            int upper = (int) (iDelta / INT_MAX_LESS);
            upper = randomInt(upper);
            int lower = (int) (iDelta % INT_MAX_LESS);
            lower = randomInt(lower);

            return upper * INT_MAX_LESS + lower;
        }
    }

    /**
     * 产生一个处于[0,Delta]之间的随机整数
     * @param iDelta  最大值
     * @param _random 随机数生成器
     * @return
     */
    public static long randomLong(long iDelta, java.util.Random _random)
    {
        if (iDelta <= INT_MAX_LESS)
        {
            return randomInt((int) iDelta, _random);
        } else
        {
            int upper = (int) (iDelta / INT_MAX_LESS);
            upper = randomInt(upper, _random);
            int lower = (int) (iDelta % INT_MAX_LESS);
            lower = randomInt(lower, _random);

            return upper * INT_MAX_LESS + lower;
        }
    }

    /***
     *  产生一个处于[_min,_max]之间的随机整数
     * @param _min
     * @param _max
     * @return
     */
    public static int randomInt(int _min, int _max)
    {
        int min = Math.min(_min, _max);
        int max = Math.max(_min, _max);
        return min + randomInt(max - min);
    }

    public static long randomLong(long _min, long _max)
    {
        long min = Math.min(_min, _max);
        long max = Math.max(_min, _max);
        return min + randomLong(max - min);
    }

    /**
     * 产生一个处于[0,Delta)之间的随机整数
     * @param iDelta
     * @return
     */
    public static int randomInt_noInclude(int iDelta)
    {
        return Math.abs(Random.nextInt(iDelta));
    }

    /**
     * rateList 含有各项的权重， totalRate是总权重
     * @param _rateList
     * @param _totalRate
     * @return 获取列表_rateList的下标编号
     */
    public static int getRandomRateByList(List<Integer> _rateList, int _totalRate)
    {
        if (0 == _totalRate && _rateList.size() > 0)
        {
            return 0;
        }

        if (0 == _totalRate)
        {
            return -1;
        }

        int rand = randomInt(_totalRate - 1) + 1;

        for (int index = 0; index < _rateList.size(); index++)
        {
            int curRate = _rateList.get(index);
            if (rand <= curRate)
            {
                return index;
            }
            rand -= curRate;
        }
        return -1;
    }

    /**
     * rateList 含有各项的权重
     * @param _rateList
     * @return
     */
    public static int getRandomIndexByRate(List<Integer> _rateList)
    {
        int rand = 0;

        for (Integer rate : _rateList)
        {
            rand += rate;
        }

        if (_rateList.size() > 0 && rand == 0)
        {
            return randomInt(_rateList.size() - 1);
        }
        return getRandomRateByList(_rateList, rand);
    }

    /**
     * 根据权重列表，随机获取count个结果索引
     * @param _rateList
     * @param count
     * @return
     */
    public static List<Integer> getRandomIndexByRate(List<Integer> _rateList, int count)
    {
        ArrayList<Integer> copy = new ArrayList<>(_rateList);
        ArrayList<Integer> ret = new ArrayList<>();
        int idx = 0;
        while (count > 0)
        {
            idx = getRandomIndexByRate(copy);
            ret.add(idx);
            copy.set(idx, 0);
            count -= 1;
        }
        return ret;
    }

    /**
     * 获取一定范围内的随机值
     * @param valueRange
     * @return
     */
    public static int getRandomValueInRange(ArrayList<Integer> valueRange)
    {
        if (valueRange == null || valueRange.isEmpty())
        {
            return 0;
        }

        int min = valueRange.get(0);
        int max = valueRange.size() > 1 ? valueRange.get(1) : Integer.MAX_VALUE;

        int v1 = Math.min(min, max);
        int v2 = Math.max(min, max);

        int res = Random.nextInt(v2 - v1 + 1, v1);
        return res;
    }

    private static TimeZone nowTimeZone = TimeZone.getDefault();

    public static TimeZone getTimeZone()
    {
        return nowTimeZone;
    }

    private static Locale _loc = Locale.getDefault(Locale.Category.FORMAT);


    public static final int MIN_SEC = 60;
    public static final int HOUR_SEC = 60 * 60;
    public static final int DAY_SEC = 24 * 60 * 60;
    public static final int WEEK_SEC = 7 * 24 * 60 * 60;

    public static void setTimeZone(String _timeZone)
    {
        TimeZone.setDefault(TimeZone.getTimeZone(_timeZone));
        nowTimeZone = TimeZone.getDefault();
    }

    public static Calendar newCalendar()
    {
        return Calendar.getInstance(nowTimeZone, _loc);
    }

    public static long getNowTimeMS()
    {
        return System.currentTimeMillis();
    }

    public static int getNowTimeSec()
    {
        return (int) (getNowTimeMS() / 1000);
    }

    public static long getFirstDayOfNextMonthZeroClockMS()
    {
        Calendar objCalendar = newCalendar();
        int year = objCalendar.get(Calendar.YEAR);
        int month = objCalendar.get(Calendar.MONTH);
        Calendar targetCalendar = newCalendar();
        targetCalendar.set(year, month + 1, 1, 0, 0, 0);

        return targetCalendar.getTimeInMillis();
    }

    public static int getFirstDayOfNextMonthZeroClockS()
    {
        return (int) (getFirstDayOfNextMonthZeroClockMS() / 1000);
    }

    public static long getFirstDayOfWeekZeroClockMS()
    {
        Calendar objCalendar = newCalendar();
        int year = objCalendar.get(Calendar.YEAR);
        int month = objCalendar.get(Calendar.MONTH);
        int day = objCalendar.get(Calendar.DAY_OF_MONTH);
        int weekday = objCalendar.get(Calendar.DAY_OF_WEEK);
        Calendar targetCalendar = newCalendar();
        targetCalendar.set(year, month, day - weekday + 1, 0, 0, 0); // 周日那天0点的时间

        return targetCalendar.getTimeInMillis();
    }

    public static int getFirstDayOfWeekZeroClockS()
    {
        return (int) (getFirstDayOfWeekZeroClockMS() / 1000);
    }

    public static int getFirstDayOfWeekZeroClockS(int timeS)
    {
        Calendar objCalendar = newCalendar();
        objCalendar.setTimeInMillis(timeS * 1000l);
        int year = objCalendar.get(Calendar.YEAR);
        int month = objCalendar.get(Calendar.MONTH);
        int day = objCalendar.get(Calendar.DAY_OF_MONTH);
        int weekday = objCalendar.get(Calendar.DAY_OF_WEEK);

        Calendar targetCalendar = newCalendar();
        targetCalendar.set(year, month, day - weekday + 1, 0, 0, 0); // 周日那天0点的时间
        return (int) (targetCalendar.getTimeInMillis() / 1000);
    }

    /*********
     * 从2020-01-01开始数过了几周，从0开始。
     * @return
     */
    public static int getNowWeekTag()
    {
        //目标时间毫秒数
        //2020-1-6 周一，根据这个时间计算周数
        Calendar objCalendar = newCalendar();
        objCalendar.set(2020, Calendar.JANUARY, 6, 0, 0, 0);
        objCalendar.set(Calendar.MILLISECOND, 0);
        long newWeekBase = objCalendar.getTimeInMillis();

        int dayDiff = CommonFunc.getDayDiff(newWeekBase, CommonFunc.getNowTimeMS());
        return dayDiff / 7;
    }

    //返回唯一的周编号
    public static int getWeekId()
    {
        Calendar objCalendar = newCalendar();
        return objCalendar.get(Calendar.YEAR) * 100 + objCalendar.get(Calendar.WEEK_OF_YEAR);
    }

    public static int getWeekElapsedSec()
    {
        return getNowTimeSec() - getFirstDayOfWeekZeroClockS();
    }

    public static long getTodayZeroClockMS(int offsetSec)
    {
        Calendar objCalendar = newCalendar();
        // 先设置为当天零点
        objCalendar.set(Calendar.HOUR_OF_DAY, 0);
        objCalendar.set(Calendar.MINUTE, 0);
        objCalendar.set(Calendar.SECOND, 0);
        objCalendar.set(Calendar.MILLISECOND, 0);

        // 然后根据偏移量调整
        if (offsetSec != 0)
            objCalendar.add(Calendar.SECOND, -offsetSec);

        return objCalendar.getTimeInMillis();
    }

    public static long getTodayZeroClockS()
    {
        return getTodayZeroClockMS(0) / 1000;
    }

    public static long getNextZeroClockTimeMS()
    {
        Calendar objCalendar = newCalendar();
        int year = objCalendar.get(Calendar.YEAR);
        int month = objCalendar.get(Calendar.MONTH);
        int day = objCalendar.get(Calendar.DAY_OF_MONTH);
        objCalendar.set(year, month, day + 1, 0, 0, 0);
        objCalendar.set(Calendar.MILLISECOND, 0);

        return objCalendar.getTimeInMillis();
    }

    public static int getZeroClockS(int timeSec)
    {
        Calendar objCalendar = newCalendar();
        objCalendar.setTimeInMillis(timeSec * 1000L);
        int year = objCalendar.get(Calendar.YEAR);
        int month = objCalendar.get(Calendar.MONTH);
        int day = objCalendar.get(Calendar.DAY_OF_MONTH);
        objCalendar.set(year, month, day, 0, 0, 0);

        return (int) (objCalendar.getTimeInMillis() / 1000);
    }

    public static long getZeroClockMS(long _timeMs)
    {
        Calendar objCalendar = newCalendar();
        objCalendar.setTimeInMillis(_timeMs);
        int year = objCalendar.get(Calendar.YEAR);
        int month = objCalendar.get(Calendar.MONTH);
        int day = objCalendar.get(Calendar.DAY_OF_MONTH);
        objCalendar.set(year, month, day, 0, 0, 0);
        objCalendar.set(Calendar.MILLISECOND, 0);

        return objCalendar.getTimeInMillis();
    }

    public static int getTodayHour()
    {
        Calendar objCalendar = newCalendar();
        return objCalendar.get(Calendar.HOUR_OF_DAY);
    }

    public static int getTodayMin()
    {
        Calendar objCalendar = newCalendar();
        return objCalendar.get(Calendar.MINUTE);
    }

    public static int getOffsetTimeZone()
    {
        long timeMS = getNowTimeMS();
        return getTimeZone().getOffset(timeMS) / (HOUR_SEC * 1000);
    }

    public static int getNowTagYYYYMMDD() //20180214
    {
        Calendar objCalendar = newCalendar();
        int year = objCalendar.get(Calendar.YEAR);
        int month = objCalendar.get(Calendar.MONTH) + 1;
        int day = objCalendar.get(Calendar.DAY_OF_MONTH);
        return year * 10000 + month * 100 + day;
    }

    public static long getZeroFromTimeTag(int _timeTag)//yyyymmdd
    {
        if (_timeTag <= 0)
        {
            return 0;
        }
        Calendar objCalendar = newCalendar();
        int year = _timeTag / 10000;
        int month = (_timeTag % 10000) / 100;
        int day = _timeTag % 100;
        //月份从0开始
        objCalendar.set(year, month - 1, day, 0, 0, 0);
        return objCalendar.getTimeInMillis();
    }

    public static int getTimeTagYYYYMMDD(long timeSec)
    {
        if (timeSec <= 0)
            return 0;
        Calendar objCalendar = newCalendar();
        objCalendar.setTimeInMillis(timeSec * 1000L);
        int year = objCalendar.get(Calendar.YEAR);
        int month = objCalendar.get(Calendar.MONTH) + 1;
        int day = objCalendar.get(Calendar.DAY_OF_MONTH);
        return year * 10000 + month * 100 + day;
    }

    public static int getTimeByYYYYMM(int timeSec)
    {
        if (timeSec <= 0)
            return 0;
        Calendar objCalendar = newCalendar();
        objCalendar.setTimeInMillis(timeSec * 1000L);
        int year = objCalendar.get(Calendar.YEAR);
        int month = objCalendar.get(Calendar.MONTH) + 1;
        return year * 10000 + month * 100;
    }

    public static String getTimeString(int timeSec)
    {
        Calendar objCalendar = newCalendar();
        objCalendar.setTimeInMillis(timeSec * 1000l);
        SimpleDateFormat df = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss", _loc);
        return df.format(objCalendar.getTime());
    }


    public static String[] charSplit(String s, char token)
    {
        return charSplit(s, token, Integer.MAX_VALUE);
    }

    public static String[] charSplit(String s, char token, int _limit)
    {
        if (null == s)
        {
            return new String[0];
        }
        s = s.trim();
        int iStart = 0;
        ArrayList<String> arr = new ArrayList<>();
        for (int i = 0; i < s.length(); i++)
        {
            if (s.charAt(i) == token)
            {
                String sub = s.substring(iStart, i);
                arr.add(sub);
                iStart = i + 1;
                if (arr.size() == _limit - 1)
                {
                    break;
                }
            }
        }
        arr.add(s.substring(iStart, s.length()));

        String[] ret = new String[arr.size()];
        return arr.toArray(ret);
    }


    /******************
     * 用带入的tokens中的任意符号对s进行拆分
     * @param s
     * @param tokens
     * @return
     */
    public static String[] charSplit(String s, char[] tokens)
    {
        return charSplit(s, tokens, Integer.MAX_VALUE, false);
    }

    public static String[] charSplit(String s, char[] tokens, int _limit)
    {
        return charSplit(s, tokens, _limit, false);
    }

    public static String[] charSplit(String s, char[] tokens, boolean bRemoveEmptyEntries)
    {
        return charSplit(s, tokens, Integer.MAX_VALUE, bRemoveEmptyEntries);
    }

    public static String[] charSplit(String s, char token, int _limit, boolean _bTrim)
    {
        if (null == s)
        {
            CommLog.error("charSplit null ,may be config error.", new Exception());
            return new String[0];
        }
        if (_bTrim)
            s = s.trim();

        int iStart=0;
        ArrayList<String> arr= new ArrayList<>();
        int len = s.length();
        for(int i=0;i<len;i++)
        {
            if(s.charAt(i)==token)
            {
                String sub=s.substring(iStart, i);
                arr.add(sub);
                iStart=i+1;
                if(arr.size()==_limit-1)
                {
                    break;
                }
            }
        }
        arr.add(s.substring(iStart));

        String[] ret = new String[arr.size()];
        return arr.toArray(ret);
    }

    public static String[] charSplit(String s, char[] tokens, int _limit, boolean bRemoveEmptyEntries)
    {
        int iStart = 0;
        s = s.trim();
        ArrayList<String> arr = new ArrayList<>();
        for (int i = 0; i < s.length(); i++)
        {
            boolean bFound = false;
            for (int j = 0; j < tokens.length; j++)
            {
                if (s.charAt(i) == tokens[j])
                {
                    bFound = true;
                    break;
                }
            }
            if (bFound)
            {
                String sub = s.substring(iStart, i);
                iStart = i + 1;
                if (!(sub.trim().isEmpty() && bRemoveEmptyEntries))
                {
                    arr.add(sub);

                    if (arr.size() == _limit - 1)
                    {
                        break;
                    }
                }
            }
        }
        String leftStr = s.substring(iStart, s.length());
        if (!(leftStr.trim().isEmpty() && bRemoveEmptyEntries))
        {
            arr.add(leftStr);
        }

        String[] ret = new String[arr.size()];
        return arr.toArray(ret);
    }

    /******
     * 拼接Url地址
     * @param _urlBase
     * @param s
     * @return
     */
    public static String urlConcat(String _urlBase, String s)
    {
        if (s == null || s.isEmpty())
            return _urlBase;
        if (_urlBase.endsWith("/"))
        {
            if (s.startsWith("/"))
            {
                return _urlBase + s.substring(1);
            } else
                return _urlBase + s;
        } else
        {
            if (s.startsWith("/"))
            {
                return _urlBase + s;
            } else
                return _urlBase + "/" + s;
        }

    }

    public static String trimFloatNum(String s)
    {
        if (s.length() > 2)
        {
            if (s.charAt(s.length() - 2) == '.' && s.charAt(s.length() - 1) == '0')
                return s.substring(0, s.length() - 2);
        }
        return s;
    }

    public static String trimFloatNum(float f)
    {
        return trimFloatNum("" + f);
    }

    public static String trimFloatNum(double f)
    {
        return trimFloatNum("" + f);
    }

    public static int indexOfAny(String src, char[] tokens, int start)
    {
        if (null == src)
        {
            return -1;
        }
        if (start < 0 || start > src.length() - 1)
        {
            return -1;
        }
        if (tokens.length == 0)
        {
            return -1;
        }
        for (int i = start; i < src.length(); i++)
        {
            char c = src.charAt(i);
            for (int j = 0; j < tokens.length; j++)
            {
                if (tokens[j] == c)
                {
                    return i;
                }
            }
        }
        return -1;
    }

    public static <T extends Object> List<String> toStringList(List<T> objs)
    {
        List<String> ret = new ArrayList<>();
        for (T o : objs)
        {
            ret.add(o.toString());
        }
        return ret;
    }

    /*****
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

    /******
     * 列表转成字符串
     * @param intList
     * @param <T>
     * @return
     */
    public static <T> String list2String(Collection<T> intList)
    {
        return list2String(intList, ';');
    }

    public static <T> String list2String(Collection<T> intList, char _token)
    {
        if (intList == null || intList.isEmpty())
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

    /**
     * 从字符串中解析出列表对象
     * @param _str
     * @param clazz
     * @return
     * @param <T>
     */
    public static <T extends _IParseFromStringable> List<T> listStringableFromString(String _str, Class<T> clazz)
    {
        List<T> resultList = new ArrayList<>();
        String[] strs = CommonFunc.charSplit(_str, ';');

        for(int i = 0; i < strs.length; i++)
        {
            String itemStr = strs[i];
            if(null == itemStr)
                continue;

            try
            {
                T item = clazz.newInstance();
                item.parseFromString(itemStr);
                resultList.add(item);
            }
            catch (Exception e)
            {
                CommLog.error("ListFromString - create instance failed: clazz={}, error={}", clazz.getName(), e.getMessage());
            }
        }

        return resultList;
    }

    /********************
     * 使用带入的比较函数，对队列进行排序
     * -1 表示小于，0表示等于， 1表示大于
     * 按照从小到大进行排序
     **/
    public static <T> void sortAscList(List<T> _list, Comparator<T> _judgeFunc)
    {
        if (null == _list || null == _judgeFunc)
            return;

        T minObj = null;
        int tempJudgeIdx = 0;
        for (int i = 0; i < _list.size(); i++)
        {
            //假定最小值为第一个
            minObj = _list.get(i);
            tempJudgeIdx = i;

            //逐个比较
            for (int j = i + 1; j < _list.size(); j++)
            {
                if (_judgeFunc.compare(minObj, _list.get(j)) > 0)
                {
                    minObj = _list.get(j);
                    tempJudgeIdx = j;
                }
            }

            //设置当前位置的值
            if (i != tempJudgeIdx)
            {
                minObj = _list.get(i);
                _list.set(i, _list.get(tempJudgeIdx));
                _list.set(tempJudgeIdx, minObj);
            }
        }
    }

    //只取出指定次数的对象
    public static <T> void sortAscList(List<T> _list, Comparator<T> _judgeFunc, int _limitTimes)
    {
        if (null == _list || null == _judgeFunc)
            return;

        T minObj = null;
        int tempJudgeIdx = 0;
        for (int i = 0; i < _list.size(); i++)
        {
            //判断是否达到次数
            if (i >= _limitTimes)
                return;

            //假定最小值为第一个
            minObj = _list.get(i);
            tempJudgeIdx = i;

            //逐个比较
            for (int j = i + 1; j < _list.size(); j++)
            {
                if (_judgeFunc.compare(minObj, _list.get(j)) > 0)
                {
                    minObj = _list.get(j);
                    tempJudgeIdx = j;
                }
            }

            //设置当前位置的值
            if (i != tempJudgeIdx)
            {
                minObj = _list.get(i);
                _list.set(i, _list.get(tempJudgeIdx));
                _list.set(tempJudgeIdx, minObj);
            }
        }
    }

    public static byte[] ByteBfferToBytes(ByteBuffer byteBuff)
    {
        int oldPositon = byteBuff.position();
        byteBuff.position(0);
        byte[] ret = new byte[byteBuff.remaining()];
        byteBuff.get(ret);
        byteBuff.position(oldPositon);
        return ret;
    }

    public static String ByteBufferReadString(ByteBuffer bf)
    {
        int len = bf.getInt();
        if (len <= 0)
        {
            return "";
        }
        byte[] bytes = new byte[len];
        bf.get(bytes);
        try
        {
            return new String(bytes, "utf-8");
        } catch (UnsupportedEncodingException e)
        {
            CommLog.error("", e);
            return "";
        }
    }

    /******
     * 用反射的方式，把一个对象显示成一个可读字符串
     * @param _objInfo
     * @param _sb
     * @return
     */
    public static String GetInfoPropertys(Object _objInfo, StringBuilder _sb)
    {
        try
        {
            if (null == _sb)
            {
                _sb = new StringBuilder();
            }
            if (_objInfo == null)
            {
                return _sb.toString();
            }
            Class<?> clazz = _objInfo.getClass();
            if (List.class.isAssignableFrom(clazz))
            {
                @SuppressWarnings("unchecked")
                List<Object> objs = (List<Object>) _objInfo;

                _sb.append(String.format("size:%d", objs.size()));
                for (Object obj : objs)
                {
                    GetInfoPropertys(obj, _sb);
                }
                return _sb.toString();
            }
            if (clazz.isArray())
            {
                if (byte[].class == clazz)
                {
                    _sb.append("[");
                    _sb.append(ALBasicCommonFun.getHexString((byte[]) _objInfo));
                    _sb.append("]");
                } else
                {
                    Object[] objs = (Object[]) _objInfo;

                    _sb.append(String.format("size:%d", objs.length));
                    for (Object obj : objs)
                    {
                        GetInfoPropertys(obj, _sb);
                    }
                    return _sb.toString();
                }

            }

            _sb.append("[");

            if (clazz.isPrimitive() || clazz == String.class || clazz.isEnum()
                    || clazz == Long.class
                    || clazz == Integer.class
                    || clazz == Short.class
                    || clazz == Double.class
                    || clazz == Float.class
                    || clazz == Boolean.class
            )
            {
                _sb.append(_objInfo.toString()).append("]");
                return _sb.toString();
            }
            Method[] pInfos = clazz.getMethods();
            for (Method pTemp : pInfos)
            {
                String Pname = pTemp.getName();
                if (!Pname.startsWith("get") || Pname.endsWith("MainOrder") || Pname.endsWith("SubOrder"))
                {
                    continue;
                }
                if (Pname.compareTo("getModifiers") == 0
                        || Pname.compareTo("getSuperclass") == 0
                        || Pname.compareTo("getComponentType") == 0
                        || Pname.compareTo("getName") == 0
                        || Pname.compareTo("getAnnotation") == 0
                        || Pname.compareTo("getClass") == 0
                )
                {
                    continue;
                }

                String pTypeName = pTemp.getReturnType().getSimpleName();

                Object[] params = new Object[0];
                Object Pvalue = null;
                try
                {
                    Pvalue = pTemp.invoke(_objInfo, params);
                } catch (Exception e)
                {
                    return _objInfo.getClass().getSimpleName() + ":" + _objInfo.toString();
                }
                if (pTemp.getReturnType().isPrimitive() || pTemp.getReturnType().getSimpleName().startsWith("String"))
                {
                    String value = (Pvalue == null ? "NULL" : Pvalue.toString());
                    _sb.append(String.format("%s: %s, ", Pname.substring(3, Pname.length()), value));
                } else
                {
                    _sb.append(String.format("(%s)%s:[", pTypeName, Pname.substring(3, Pname.length())));
                    GetInfoPropertys(Pvalue, _sb);
                    _sb.append("]");
                }
            }
            _sb.append("],\n");
            return _sb.toString();
        } catch (Exception e)
        {
            CommLog.error("", e);
            return "";
        }
    }


    public static String lineupToString(Common_Lineup _lineup)
    {
        ByteBuffer buffer = _lineup.makePackage();
        byte[] bytes = new byte[buffer.remaining()];
        buffer.get(bytes);
        String str = Base64.encode(bytes);
        return str;
    }

    public static Common_Lineup lineupFromString(String _str)
    {
        if (null == _str || _str.isEmpty())
        {
            return null;
        }
        Common_Lineup lineup = new Common_Lineup();
        byte[] bytes = Base64.decode(_str);
        lineup.readPackage(ByteBuffer.wrap(bytes));
        return lineup;
    }

    public static JsonObject StringMapToJson(Map<String, String> params)
    {
        JsonObject json = new JsonObject();
        for (String key : params.keySet())
        {
            json.addProperty(key, params.get(key));
        }
        return json;
    }

    public static JsonObject string2JsonObject(String str)
    {
        try
        {
            return new JsonParser().parse(str).getAsJsonObject();
        } catch (Exception e)
        {
            return null;
        }
    }

    public static String getTextByLanguage(String jsonString, int language)
    {
        if (language == 0)
        {
            language = 1;
        }
        try
        {
            JsonElement jsonElement = new JsonParser().parse(jsonString);
            if (jsonElement == null)
            {
                return jsonString;
            }
            return getTextByLanguage(jsonElement.getAsJsonObject(), language);
        } catch (Exception e)
        {
            return jsonString;
        }
    }

    private static String getTextByLanguage(JsonObject jsonObject, int language)
    {
        try
        {
            JsonElement value = jsonObject.get("" + language);
            if (value == null)
            {
                value = jsonObject.get("1");
                if (value == null)
                {
                    return jsonObject.toString();
                }
                return value.getAsString();
            }
            return value.getAsString();
        } catch (Exception e)
        {
            return jsonObject.toString();
        }
    }

    public static boolean checkEamil(String email)
    {
        boolean tag = true;
        String pattern1 = "^\\s*([A-Za-z0-9_-]+(\\.\\w+)*@(\\w+\\.)+\\w{2,5})\\s*$";
        Pattern pattern = Pattern.compile(pattern1);
        Matcher mat = pattern.matcher(email);
        if (!mat.find())
        {
            tag = false;
        }
        return tag;
    }


    public static boolean isEmptyJsonArray(String _rewardJson)
    {
        if (null == _rewardJson) return true;
        if (_rewardJson.isEmpty()) return true;
        if (_rewardJson.replace(" ", "").compareTo("[]") == 0)
        {
            return true;
        }
        return false;
    }

    /**
     * 添加时间点判断日期（例如：23:00前算当日，23:00后算明日）
     * @param _hour
     * @param _min
     * @return
     */
    public static int getTodayTimeTagByTimepot(int _hour, int _min) //20180214
    {
        int curHour = getTodayHour();
        if (curHour < _hour)
            return getNowTagYYYYMMDD();

        int curMin = getTodayMin();
        if (curHour == _hour && curMin < _min)
            return getNowTagYYYYMMDD();

        Calendar objCalendar = newCalendar();
        objCalendar.add(Calendar.DAY_OF_MONTH, 1);
        int year = objCalendar.get(Calendar.YEAR);
        int month = objCalendar.get(Calendar.MONTH) + 1;
        int day = objCalendar.get(Calendar.DAY_OF_MONTH);
        return year * 10000 + month * 100 + day;
    }

    /**
     * 添加到达目标时间（如果超过当前时刻，则取下一天）的间隔时间
     * @param _hour
     * @param _min
     * @return
     */
    public static long getGapTimeMs(int _hour, int _min)
    {
        //当前时间毫秒数
        long curTimeMS = getNowTimeMS();

        //目标时间毫秒数
        Calendar objCalendar = newCalendar();
        objCalendar.set(Calendar.HOUR_OF_DAY, _hour);
        objCalendar.set(Calendar.MINUTE, _min);
        objCalendar.set(Calendar.SECOND, 0);
        long targetTimeMS = objCalendar.getTimeInMillis();

        if (targetTimeMS > curTimeMS)
            return (targetTimeMS - curTimeMS);

        objCalendar.add(Calendar.DAY_OF_MONTH, 1);
        return (objCalendar.getTimeInMillis() - curTimeMS);
    }

    /**************************
     * 获取指定时间的时间戳
     * @param _hour
     * @param _min
     * @return
     */
    public static long getAssignOffsetTimeMs(int _hour, int _min)
    {
        //当前时间毫秒数
        long curTimeMS = getNowTimeMS();

        //目标时间毫秒数
        Calendar objCalendar = newCalendar();
        objCalendar.set(Calendar.HOUR_OF_DAY, _hour);
        objCalendar.set(Calendar.MINUTE, _min);
        objCalendar.set(Calendar.SECOND, 0);
        long targetTimeMS = objCalendar.getTimeInMillis();

        if (targetTimeMS > curTimeMS)
            return targetTimeMS;

        objCalendar.add(Calendar.DAY_OF_MONTH, 1);
        return objCalendar.getTimeInMillis();
    }

    /**
     * 获取当前时间截止到下一天0点0分的剩余时间（秒）
     * @return
     */
    public static long getNextDayOffsetTimeMS()
    {
        //当前时间毫秒数
        long curTimeMS = getNowTimeMS();

        //目标时间毫秒数
        Calendar objCalendar = newCalendar();
        objCalendar.add(Calendar.DAY_OF_MONTH, 1);
        objCalendar.set(Calendar.HOUR_OF_DAY, 0);
        objCalendar.set(Calendar.MINUTE, 0);
        objCalendar.set(Calendar.SECOND, 0);
        long targetTimeMS = objCalendar.getTimeInMillis();

        return (targetTimeMS - curTimeMS);
    }

    /**
     * 获取指定时间戳到指定时刻的日期判断，方法类似于 getTodayTimeTagByTimepot
     * @param _timeMS
     * @param _hour
     * @param _min
     * @return
     */
    public static int getAssignTimeTagByTimepot(long _timeMS, int _hour, int _min)
    {
        //获取对应的小时，分钟数
        Calendar objCalendar = newCalendar();
        objCalendar.setTimeInMillis(_timeMS);

        //进行对比
        int assignHour = objCalendar.get(Calendar.HOUR_OF_DAY);
        if (assignHour < _hour)
            return getNowTagYYYYMMDD();

        int assignMin = objCalendar.get(Calendar.MINUTE);
        if (assignHour >= _hour && assignMin < _min)
            return getNowTagYYYYMMDD();

        Calendar nextObjCalendar = newCalendar();
        nextObjCalendar.add(Calendar.DAY_OF_MONTH, 1);
        int year = nextObjCalendar.get(Calendar.YEAR);
        int month = nextObjCalendar.get(Calendar.MONTH) + 1;
        int day = nextObjCalendar.get(Calendar.DAY_OF_MONTH);
        return year * 10000 + month * 100 + day;
    }

    //是否对应品质
    public static boolean IsQuality(EQuality _quality, int _qualityValue)
    {
        return (_qualityValue & (1 << (int) _quality.ordinal())) != 0;
    }

    //是否对应对象类型
    public static boolean IsActor(EWCGActorType _actorType, int _actorTypeValue)
    {
        return (_actorTypeValue & (1 << (int) _actorType.ordinal())) != 0;
    }

    @SuppressWarnings("null")
    public static void fatal(String s)
    {
        CommLog.error(s);

        Long l = null;
        @SuppressWarnings("unused")
        long ll = l;
    }

    /*************************
     * 获取N天的起始时间戳
     * @param _offsetDays
     * @return
     */
    public static long getZeroClockMSByOffsetDays(int _offsetDays)
    {
        Calendar objCalendar = newCalendar();
        objCalendar.add(Calendar.DAY_OF_MONTH, -_offsetDays);

        int year = objCalendar.get(Calendar.YEAR);
        int month = objCalendar.get(Calendar.MONTH);
        int day = objCalendar.get(Calendar.DAY_OF_MONTH);
        objCalendar.set(year, month, day, 0, 0, 0);

        return objCalendar.getTimeInMillis();
    }

    public static int getZeroClockSecByOffsetDays(int _offsetDays)
    {
        return (int) (getZeroClockMSByOffsetDays(_offsetDays) / 1000);
    }


    public static int getDayOfWeek()
    {
        Calendar objCalendar = newCalendar();
        return objCalendar.get(Calendar.DAY_OF_WEEK);
    }


    private static Long _g_defaultPushSetting = null;

    public static long makeDefaultPushSetting()
    {
        if (null == _g_defaultPushSetting)
        {
            _g_defaultPushSetting = new Long(0L);
            for (EWCGLocalPushContentType ePush : EWCGLocalPushContentType.values())
            {
                _g_defaultPushSetting |= 1 << ePush.ordinal();
            }
        }
        return _g_defaultPushSetting;

    }


    public static long parseClientVersion(String _strVersion)
    {
        String[] strs = CommonFunc.charSplit(_strVersion.trim(), '.');
        if (strs.length < 2 || strs.length > 4)
        {
            CommLog.error("Client version parse faild:str=" + _strVersion);
            return 0;
        }
        long ver = 0;
        for (int i = 0; i < strs.length; i++)
        {
            String subVer = strs[strs.length - i - 1];
            try
            {
                int verNum = Integer.parseInt(subVer);
                if (verNum >= 100 || verNum < 0)
                {
                    CommLog.error("Client version parse faild:str=" + _strVersion);
                    return 0;
                }
                ver = ver + (long) (Math.pow(100, i) * verNum);
            } catch (Exception e)
            {
                CommLog.error("Client version parse faild:str=" + _strVersion);
                return 0;
            }

        }
        return ver;
    }

    /******
     * 指定时间戳秒，格式化成日期yyyy-MM-dd
     * @param _timeSec
     * @return
     */
    public static String getDateString(int _timeSec)
    {
        Calendar objCalendar = newCalendar();
        objCalendar.setTimeInMillis(_timeSec * 1000L);
        SimpleDateFormat df = new SimpleDateFormat("yyyy-MM-dd", _loc);
        return df.format(objCalendar.getTime());
    }

    /******
     * 指定时间戳秒，格式化成时间格式 yyyy-MM-dd HH:mm:ss
     * @param _timeSec
     * @return
     */
    public static String getTimeStringSec(int _timeSec)
    {
        Calendar objCalendar = newCalendar();
        objCalendar.setTimeInMillis(_timeSec * 1000L);
        SimpleDateFormat df = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss", _loc);
        return df.format(objCalendar.getTime());
    }

    /******
     * 指定时间戳毫秒，格式化成时间格式 yyyy-MM-dd HH:mm:ss
     * @param _timeMs
     * @return
     */
    public static String getTimeStringMs(long _timeMs)
    {
        int sec = (int) (_timeMs / 1000);
        return getTimeStringSec(sec);
    }

    /******
     * 指定时间戳和格式，格式化成时间格式
     * @param _timeMs
     * @return
     */
    public static String getTimeString(long _timeMs, String _format)
    {
        Calendar objCalendar = newCalendar();
        objCalendar.setTimeInMillis(_timeMs);
        SimpleDateFormat df = new SimpleDateFormat(_format, _loc);
        return df.format(objCalendar.getTime());
    }

    /**
     * @return返回纳秒
     */
    public static long getNowTimeMicro()
    {
        return System.nanoTime(); // 纳秒

    }

    /**
     * 获取间隔天数
     * @param startTimeMs
     * @param endTimeMS
     * @return
     */
    public static int getDayDiff(long startTimeMs, long endTimeMS)
    {
        long start = getZeroClockMS(startTimeMs);
        long end = getZeroClockMS(endTimeMS);

        long absDiffMs = Math.abs(end - start);

        //即使有闰秒的情况也保证不会计算错误
        if (absDiffMs < 10000)
            return 0;

        int absDay = (int) ((absDiffMs + 10000) / (DAY_SEC * 1000L));

        return end > start ? absDay : -absDay;
    }

    /**
     * List拷贝，避免addAll生成内存碎片
     */
    public static <T> void appendList(List<T> _list, List<T> _listSrc)
    {
        int size = _listSrc.size();
        if (size > 1024)
        {
            _list.addAll(_listSrc);
            return;
        }
        for (int i = 0; i < size; i++)
        {
            _list.add(_listSrc.get(i));
        }
    }

    public static void printVersion()
    {
        System.out.println(NPVersion.getString());
    }

    /***
     * 返回协议结构体的字符串表示
     * @param _proto
     * @return
     */
    public static String getProtoString(_IALProtocolStructure _proto)
    {
        StringBuilder sb = new StringBuilder();
        CommonFunc.GetInfoPropertys(_proto, sb);
        return sb.toString();
    }


    /******
     * 检测字符串是否是ip
     * @param _serverIp
     * @return
     */
    public static boolean checkIp(String _serverIp)
    {
        String pattern1 = "\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}";

        Pattern pattern = Pattern.compile(pattern1);
        Matcher matcher = pattern.matcher(_serverIp);
        return matcher.matches();
    }

    //判断是否整数
    public static boolean isNumeric(String _str)
    {
        if (_str == null || _str.isEmpty())
            return false;
        Pattern pattern = Pattern.compile("[0-9]*");
        return pattern.matcher(_str).matches();
    }


    /*****
     * 判断数值是否在闭合区间内
     * @param _value
     * @param _minValue
     * @param _maxValue
     * @return
     */
    public static boolean inRange(long _value, long _minValue, long _maxValue)
    {
        if (-1 == _value)
            return false;

        if (-1 != _minValue && _value < _minValue)
            return false;

        if (-1 != _maxValue && _value > _maxValue)
            return false;

        return true;
    }

    /*****
     * 洗牌算法，随机打乱一个列表，取得_rndCount个随机数
     * 取N个数据
     * @param _list
     * @param <T>
     */
    public static <T> List<T> getRndCount(List<T> _list)
    {
        if (null == _list)
            return new ArrayList<T>();

        return getRndCount(_list, _list.size());
    }

    public static <T> List<T> getRndCount(List<T> _list, int _rndCount)
    {
        if (null == _list)
            return new ArrayList<T>();

        List<T> resList = new ArrayList<T>(_list);
        //逐个选取随机对象
        int dealCount = 0;
        while (dealCount < _rndCount && dealCount < resList.size())
        {
            int randIndex = CommonFunc.randomInt_noInclude(resList.size() - dealCount);
            if (randIndex < resList.size())
            {
                //交换到前面的位置
                T temp = resList.get(randIndex);
                resList.set(randIndex, resList.get(dealCount));
                resList.set(dealCount, temp);
            }

            dealCount++;
        }

        //移除自dealCount之后的数据
        if (dealCount == resList.size())
            return resList;

        return resList.subList(0, dealCount);
    }

    /***********
     * 从列表中随机选择1个对象返回
     * @param _List
     * @return
     */
    public static <T> T randSelect(List<T> _List)
    {
        if (null == _List || _List.isEmpty())
        {
            return null;
        }

        int index = CommonFunc.randomInt_noInclude(_List.size());
        return _List.get(index);
    }

    /*******
     * 压缩字符串
     * @param result
     * @return
     */
    public static byte[] zipString(String result)
    {
        try
        {
            byte[] bytes = result.getBytes(StandardCharsets.UTF_8);
            return Zipper.zip(bytes);
        } catch (Exception e)
        {
            CommLog.error("zip error:", e);
            return null;
        }
    }

    /**
     * 解压字符串
     * @param _bytes
     * @return
     */
    public static String unZipString(byte[] _bytes)
    {
        if (null == _bytes || _bytes.length == 0)
            return "";
        try
        {
            byte[] unzipBytes = Zipper.unzip(_bytes);
            return new String(unzipBytes, StandardCharsets.UTF_8);
        } catch (Exception e)
        {
            CommLog.error("unzip error:", e);
            return "";
        }
    }

    /**
     * 协议压缩并Base64编码
     * @param _proto
     * @return
     */
    public static String protoToString(_IALProtocolStructure _proto)
    {
        if (null == _proto)
        {
            return "";
        }
        byte[] bytes = CommonFunc.ByteBfferToBytes(_proto.makePackage());
        try
        {
            byte[] zippedBytes = Zipper.zip(bytes);
            return Base64.encode(zippedBytes);
        } catch (Exception e)
        {
            CommLog.error("", e);
            return "";
        }
    }

    /**
     * Base64编码协议解码
     * @param _proto
     * @return
     */
    public static boolean protoFromString(String _str, _IALProtocolStructure _proto)
    {
        if (null == _proto || null == _str || _str.isEmpty())
        {
            return false;
        }
        byte[] zippedBytes = Base64.decode(_str);
        if (zippedBytes.length <= 0)
        {
            return false;
        }
        try
        {
            byte[] bytes = Zipper.unzip(zippedBytes);
            _proto.readPackage(ByteBuffer.wrap(bytes));
            return true;
        } catch (Exception e)
        {
            CommLog.error("", e);
            return false;
        }
    }

    /**
     * 用于获取当前进程的进程ID
     */
    private static int _s_pid = 0;

    public static int getPid()
    {
        // 如果当前进程的进程ID为0，则尝试解析
        if (_s_pid == 0)
        {
            try
            {
                // 获取当前进程的名称
                String name = ManagementFactory.getRuntimeMXBean().getName();
                // 从名称中分离进程ID
                String pid = name.split("@")[0];
                // 将进程ID转换为整数
                _s_pid = Integer.valueOf(pid);
            } catch (Exception e)
            {
                // 记录错误信息，并在出现异常时返回0
                CommLog.error("resolve pid failed", e);
            }
        }
        // 返回当前进程的进程ID
        return _s_pid;
    }


    /**
     * 拷贝一个新的物品对象
     * @param _item NPCommon_ItemInfo
     * @return NPCommon_ItemInfo
     */
    public static NPCommon_ItemInfo itemDeepClone(NPCommonCostItem _item)
    {
        return new NPCommon_ItemInfo(_item.getItemType().ordinal(), _item.getItemId(), _item.getCount(), null);
    }

    /**
     * 对列表中的道具进行翻倍
     * @param _costItemList 道具列表
     * @param _percent      万分比
     */
    public static List<NPCommonCostItem> itemMultipleByPercent(ArrayList<NPCommonCostItem> _costItemList, long _percent)
    {
        List<NPCommonCostItem> res = new ArrayList<>();
        for (NPCommonCostItem item : _costItemList)
        {
            if (item == null)
            {
                continue;
            }
            NPCommonCostItem newCostItem = itemMultipleByPercent(item, _percent);
            res.add(newCostItem);
        }
        return res;
    }

    /**
     * 对列表中的道具进行翻倍
     * @param _costItemList 道具列表
     * @param _count        数量
     */
    public static List<NPCommonCostItem> itemMultiple(List<NPCommonCostItem> _costItemList, long _count)
    {
        if (_costItemList == null || _costItemList.isEmpty() || _count <= 0)
            return new ArrayList<>();

        if (_count == 1)
            return _costItemList;

        ArrayList<NPCommonCostItem> res = new ArrayList<>();
        for (NPCommonCostItem item : _costItemList)
        {
            if (item == null)
            {
                continue;
            }
            NPCommonCostItem newCostItem = itemMultiple(item, _count);
            res.add(newCostItem);
        }
        return res;
    }

    /**
     * 对列表中的道具进行翻倍
     * @param _costItem 道具列表
     * @param _percent  万分比
     */
    public static NPCommonCostItem itemMultipleByPercent(NPCommonCostItem _costItem, long _percent)
    {
        return new NPCommonCostItem(_costItem.getItemType(),
                _costItem.getItemId(), _costItem.getCount() * _percent / 10000L);
    }

    /**
     * 对列表中的道具进行翻倍
     * @param _costItem 道具列表
     * @param _count    数量
     */
    public static NPCommonCostItem itemMultiple(NPCommonCostItem _costItem, long _count)
    {
        return new NPCommonCostItem(_costItem.getItemType(),
                _costItem.getItemId(), _costItem.getCount() * _count);
    }

    /**
     * 获取给定时间戳的下一个还未经过的准点时间戳
     * @param _curTimeMs 当前时间
     * @param _hour
     * @param _min
     * @return
     */
    public static long getNextAssignTimeMs(long _curTimeMs, int _hour, int _min)
    {

        //目标时间毫秒数
        Calendar objCalendar = newCalendar();
        objCalendar.setTimeInMillis(_curTimeMs);

        objCalendar.set(Calendar.HOUR_OF_DAY, _hour);
        objCalendar.set(Calendar.MINUTE, _min);
        objCalendar.set(Calendar.SECOND, 0);
        objCalendar.set(Calendar.MILLISECOND, 0);

        int dayAmount = 0;
        //如果指定时间所在的指定时刻已经过了，就计算下一天的指定时刻
        if (objCalendar.getTimeInMillis() <= _curTimeMs)
        {
            dayAmount = 1;
        }
        objCalendar.add(Calendar.DAY_OF_MONTH, dayAmount);
        return objCalendar.getTimeInMillis();
    }

    /**
     * 获取给定时间戳的下一个还未经过的准点时间戳
     * 与客户端、策划同步dayOfWeek值， 0-6 代表 周天-周六
     * @return
     */
    public static long getNextAssignTimeMs(long _curTimeMs, int _dayOfWeek, int _hour, int _min)
    {

        //目标时间毫秒数
        Calendar objCalendar = newCalendar();
        objCalendar.setTimeInMillis(_curTimeMs);

        objCalendar.set(Calendar.HOUR_OF_DAY, _hour);
        //与客户端、策划同步dayOfWeek值， 0-6 代表 周天-周六
        objCalendar.set(Calendar.DAY_OF_WEEK, _dayOfWeek + 1);
        objCalendar.set(Calendar.MINUTE, _min);
        objCalendar.set(Calendar.SECOND, 0);
        objCalendar.set(Calendar.MILLISECOND, 0);

        int dayAmount = 0;
        //如果指定时间所在的指定时刻已经过了，就计算下一周的指定时刻
        if (objCalendar.getTimeInMillis() <= _curTimeMs)
        {
            dayAmount = 7;
        }
        objCalendar.add(Calendar.DAY_OF_MONTH, dayAmount);
        return objCalendar.getTimeInMillis();
    }

    /**
     * 获取当前月份的指定日期的Calendar对象
     * @param _curTimeMs
     * @param _dayOfMonth
     * @param _hour
     * @param _min
     * @return
     */
    private static Calendar _getThisMonthCalendar(long _curTimeMs, int _dayOfMonth, int _hour, int _min)
    {
        // 获取当前日期信息
        Calendar objCalendar = newCalendar();
        objCalendar.setTimeInMillis(_curTimeMs);

        int currentMonth = objCalendar.get(Calendar.MONTH);
        int currentYear = objCalendar.get(Calendar.YEAR);

        // 设置为当前月份的指定日期
        Calendar targetCal = newCalendar();
        targetCal.set(currentYear, currentMonth, 1); // 先设置为当月1号
        targetCal.set(Calendar.HOUR_OF_DAY, _hour);
        targetCal.set(Calendar.MINUTE, _min);
        targetCal.set(Calendar.SECOND, 0);
        targetCal.set(Calendar.MILLISECOND, 0);

        // 检查目标日期是否合法
        int maxDayOfMonth = targetCal.getActualMaximum(Calendar.DAY_OF_MONTH);
        int adjustedDay = Math.min(_dayOfMonth, maxDayOfMonth);
        targetCal.set(Calendar.DAY_OF_MONTH, adjustedDay);

        return targetCal;
    }

    /**
     * 获取下一个指定几号的时间戳
     * @param _curTimeMs 当前时间戳（毫秒）
     * @param _dayOfMonth 月中的日期（1-31）
     * @param _hour 小时（0-23）
     * @param _min 分钟（0-59）
     * @return 下一个指定几号的时间戳（毫秒）
     */
    public static long getNextSpecificDayOfMonthTimeMs(long _curTimeMs, int _dayOfMonth, int _hour, int _min)
    {
        //目标时间毫秒数
        Calendar targetCal = _getThisMonthCalendar(_curTimeMs, _dayOfMonth, _hour, _min);

        int maxDayOfMonth;

        // 如果目标时间已经过了，就计算下个月的指定日期
        if (targetCal.getTimeInMillis() <= _curTimeMs) {
            targetCal.add(Calendar.MONTH, 1);

            // 再次检查目标日期在下个月是否合法
            maxDayOfMonth = targetCal.getActualMaximum(Calendar.DAY_OF_MONTH);
            targetCal.set(Calendar.DAY_OF_MONTH, Math.min(_dayOfMonth, maxDayOfMonth));
        }

        return targetCal.getTimeInMillis();
    }

    /**
     * 获取当前时间的上一个指定几号的时间戳（上一次刷新时间）
     * @param _curTimeMs 当前时间戳（毫秒）
     * @param _dayOfMonth 月中的日期（1-31）
     * @param _hour 小时（0-23）
     * @param _min 分钟（0-59）
     * @return 上一个指定几号的时间戳（毫秒）
     */
    public static long getPreviousSpecificDayOfMonthTimeMs(long _curTimeMs, int _dayOfMonth, int _hour, int _min) {
        // 获取当前日期信息
        Calendar targetCal = _getThisMonthCalendar(_curTimeMs, _dayOfMonth, _hour, _min);

        int maxDayOfMonth;

        // 如果目标时间在当前时间之后或等于当前时间，就计算上个月的指定日期
        if (targetCal.getTimeInMillis() >= _curTimeMs) {
            targetCal.add(Calendar.MONTH, -1);

            // 再次检查目标日期在上个月是否合法
            maxDayOfMonth = targetCal.getActualMaximum(Calendar.DAY_OF_MONTH);
            targetCal.set(Calendar.DAY_OF_MONTH, Math.min(_dayOfMonth, maxDayOfMonth));
        }

        return targetCal.getTimeInMillis();
    }

    /**
     * 获取明日指定时间戳
     * @param _hour
     * @param _min
     * @return
     */
    public static long getTomorrowAssignTimeMs(int _hour, int _min)
    {
        //目标时间毫秒数
        Calendar objCalendar = newCalendar();
        objCalendar.set(Calendar.HOUR_OF_DAY, _hour);
        objCalendar.set(Calendar.MINUTE, _min);
        objCalendar.set(Calendar.SECOND, 0);
        objCalendar.set(Calendar.MILLISECOND, 0);
        objCalendar.add(Calendar.DAY_OF_MONTH, 1);
        return objCalendar.getTimeInMillis();
    }

    //  (_autoId*100 + NPUserServer.getInstance().getServerAreaId())*100000 + NPUserServer.getInstance().getServerTypeId();
    public static int parseServerTypeIdFromCid(long _cid)
    {
        //校验传入的CID长度需要大于等于8位
        if (_cid < 10000000L)
            return -1;

        return (int) (_cid % 100000);
    }
    /**
     * 通用实例ID解析服务器ID
     * @param _id
     * @return
     */
    public static int parseServerTypeIdFromInstanced(long _id)
    {
        return (int) (_id % 100000);
    }

    /**
     * 跨服实例ID解析CrossRankID
     * @param _id
     * @return
     */
    public static int parseCrossRankServerIdFromCrossInstanced(long _id)
    {
        return (int) (_id % 100);
    }

    /**
     * 跨服组队：增加 groupId，队伍用 teamId（队伍实例ID，groupId * 10000000  + 队伍自增ID）
     * @param _groupId
     * @param _idx
     * @return
     */
    public static long makeActivityTeamId(long _groupId, int _idx)
    {
        return _groupId * 10000000 + _idx;
    }

    /**
     * 跨服组队：解析队伍实例ID中的队伍ID
     * @param _id
     * @return
     */
    public static int parseActivityTeamIdx(long _id)
    {
        return (int) (_id % 10000000);
    }

    /**
     * 跨服组队：解析队伍实例ID中的groupId
     * @param _id
     * @return
     */
    public static long parseActivityTeamGroupId(long _id)
    {
        return _id / 10000000;
    }

    /**
     * 解析跨服实例ID中的groupId
     * @param _instanceId
     * @return
     */
    public static long parseGameLogicGroupId(long _instanceId)
    {
        return _instanceId / 100;
    }

    /**
     * 解析跨服实例ID中的服务器ID
     * @param _instanceId
     * @return
     */
    public static int parseGameLogicServerId(long _instanceId)
    {
        return (int) _instanceId % 100;
    }

    public static ArrayList<NPCommonCostItem> itemListDeepClone(ArrayList<NPCommonCostItem> _costItemList)
    {
        ArrayList<NPCommonCostItem> list = new ArrayList<>();
        for (NPCommonCostItem npCommonCostItem : _costItemList)
        {
            list.add(npCommonCostItem.duplicate());
        }
        return list;
    }

    /**
     * costItemList 转itemInfoList
     * @param _costItemList 物品列表
     * @return ArrayList<NPCommon_ItemInfo>
     */
    public static ArrayList<NPCommon_ItemInfo> costItemListToProto(List<NPCommonCostItem> _costItemList)
    {
        ArrayList<NPCommon_ItemInfo> list = new ArrayList<>();
        for (NPCommonCostItem item : _costItemList)
        {
            list.add(item.toProto());
        }
        return list;
    }

    /**
     * itemInfoList 转costItemList
     * @param _itemInfoList 物品列表
     * @return
     */
    public static ArrayList<NPCommonCostItem> protoItemListToCostItemList(List<NPCommon_ItemInfo> _itemInfoList)
    {
        ArrayList<NPCommonCostItem> list = new ArrayList<>();
        for (NPCommon_ItemInfo item : _itemInfoList)
        {
            list.add(new NPCommonCostItem(ENPItemType.ENPItemType_FromInt(item.getItemType()), item.getSubId(), item.getCount()));
        }
        return list;
    }

    public static void dropProtoTwoBytesAndReadPackage(_IALProtocolStructure _protoRet, ByteBuffer _buffer)
    {
        _protoRet.readPackage(_buffer);
    }

    /**
     * 整合物品，合并重复物品，并合计数量
     * @param _costItemList
     * @param _itemType
     * @param _itemId
     * @param _count
     */
    public static void mergeCostItem(List<NPCommonCostItem> _costItemList, ENPItemType _itemType, long _itemId, long _count)
    {
        boolean isMerged = false;
        for (NPCommonCostItem costItem : _costItemList)
        {
            if (null == costItem)
                continue;

            if (costItem.getItemType() == _itemType && costItem.getItemId() == _itemId)
            {
                isMerged = true;
                costItem.setCount(costItem.getCount() + _count);
                break;
            }
        }

        //新物品进入列表，用新对象避免外部对象被修改
        if (!isMerged)
        {
            _costItemList.add(new NPCommonCostItem(_itemType, _itemId, _count));
        }
    }

    /**
     * 整合物品，合并重复物品，并合计数量
     * @param _costItemList
     * @param _mergeCostItem
     */
    public static void mergeCostItem(List<NPCommonCostItem> _costItemList, NPCommonCostItem _mergeCostItem)
    {
        mergeCostItem(_costItemList, _mergeCostItem.getItemType(), _mergeCostItem.getItemId(), _mergeCostItem.getCount());
    }

    /**
     * 整合物品，合并重复物品，并合计数量
     * @param _costItemList
     * @param _mergeItem
     * @param _mergeCount
     */
    public static void mergeCostItem(List<NPCommonCostItem> _costItemList, NPCommonItem _mergeItem, long _mergeCount)
    {
        mergeCostItem(_costItemList, _mergeItem.getItemType(), _mergeItem.getItemId(), _mergeCount);
    }

    /**
     * 整合物品列表，合并重复物品，并合计数量
     * @param _costItemList
     * @param _mergeCostItemList
     */
    public static void mergeCostItemList(List<NPCommonCostItem> _costItemList, List<NPCommonCostItem> _mergeCostItemList)
    {
        for (NPCommonCostItem mergeItem : _mergeCostItemList)
        {
            if (null == mergeItem)
                continue;

            mergeCostItem(_costItemList, mergeItem);
        }
    }

    public static int getDstOffset()
    {
        Calendar objCalendar = newCalendar();
        return objCalendar.get(Calendar.DST_OFFSET);
    }

    /**
     * 商品打折
     * @param _costItemList 数量列表
     * @param _discount     折扣比例
     */
    public static List<NPCommonCostItem> costItemDiscount(List<NPCommonCostItem> _costItemList, long _discount)
    {
        // 没有折扣
        if (_discount == 10000)
            return _costItemList;

        List<NPCommonCostItem> list = new ArrayList<>();
        for (NPCommonCostItem item : _costItemList)
        {
            if (item == null)
                continue;

            long newCount = item.getCount() * _discount / 10000L;
            list.add(new NPCommonCostItem(item.getItemType(), item.getItemId(), newCount));
        }

        return list;
    }

    /**
     * 比较两个集合的元素是否一致
     * 1.集合大小是否相同
     * 2.比较元素的个数
     * 3.比较元素出现的频次
     */
    public static boolean isEqualCollection(final Collection<?> a, final Collection<?> b)
    {
        // 判断集合大小
        if (a.size() != b.size())
        {
            return false;
        }
        final CardinalityHelper helper = new CardinalityHelper(a, b);
        // 判断两个集合包含的不同元素的个数是否相同
        if (helper._m_cardinalityA.size() != helper._m_cardinalityB.size())
        {
            return false;
        }
        // 判断每个元素在两个集合中出现的频次是否相同
        for (final Object obj : helper._m_cardinalityA.keySet())
        {
            if (helper.freqA(obj) != helper.freqB(obj))
            {
                return false;
            }
        }
        return true;
    }

    /**
     * 获取一堆数字中的最大值
     * @param numbers
     * @return
     */
    public static int getMax(int... numbers)
    {
        int max = numbers[0];
        for (int i = 1; i < numbers.length; i++)
        {
            if (numbers[i] > max)
            {
                max = numbers[i];
            }
        }
        return max;
    }

    /********
     * 检测日期,必须是比现在更新的时间
     * @param strDate
     * @return
     */
    public static boolean checkValidNewDate(String strDate)
    {

        String pattern1 = "^20[2,3][0-9]-[0,1][0-9]-[0-3][0-9]$";
        Pattern pattern = Pattern.compile(pattern1);
        Matcher mat = pattern.matcher(strDate);
        return mat.find();
    }

    /**********
     * 红包分配算法
     * 逻辑处理：
     * 1. COUNT=1，获取全部数值TOTAL，不需要进行剩下步骤
     * 2. 计算平均值AVG（向上取整）： TOTAL / COUNT
     * 3. 当COUNT-1>0，即非最后一个红包的时候：
     *    a. 计算第一个区间（根据平均值上下浮动计算）：
     * 	    - 区间最小值 AVG - [(MAX - AVG) / (COUNT - 1)] && (>MIN)
     *      - 区间最大值 AVG + [(MAX - AVG) / (COUNT - 1)] && (<MAX)
     *    b. 计算第二个区间（根据配置占比计算）：
     *      - [剩余TOTAL * MIN-PER, 剩余TOTAL * MAX-PER]
     *    c. 比较2个区间，取共同部分，如果没有，返回空数组
     * 4. 当计算到COUNT最后一个时，直接取剩余TOTAL
     *
     * @param _sum     - 红包的总金额
     * @param _count   - 红包数量
     * @param _per     - 每个区间的百分比
     * @param _min     - 每个分配区间的最小值（所有分配区间相同）
     * @param _max     - 每个分配区间的最大值（所有分配区间相同）
     * @param _resList - 分配的结果数据
     */
    public static void redPacketAllocat(long _sum, int _count, WCGPairIntList _per, long _min, long _max
            , ArrayList<Long> _resList, int _depth)
    {
        if (_count <= 0 || _count > 1000)
        {
            CommLog.error("CommonFunc.redPacketAllocat count error, count:{}", _count);
            _resList.clear();
        } else if (_count == 1)
        {
            _resList.add(_sum);
        } else
        {
            long avgCount = (long) Math.ceil(1.0f * _sum / _count);
            long offSet = _max - avgCount;
            if (offSet < 0)
            {
                CommLog.error("CommonFunc.redPacketAllocat avg-offSet, max:{} avg:{}", _max, avgCount);
                _resList.clear();
                return;
            }
            //区间1
            long newMin = avgCount - offSet;
            long newMax = avgCount + offSet;
            //区间2
            if (null != _per && _depth < _per.getList().size())
            {
                WCGPairInt pair = _per.getList().get(_depth);
                if (null != pair)
                {
                    long newMin2 = (long) Math.ceil(1f * _sum * pair.first() / 10000f);
                    long newMax2 = (long) Math.ceil(1f * _sum * pair.second() / 10000f);

                    //跟第一次计算的区间进行比较，取重复部分区间
                    if (newMin < newMin2)
                    {
                        newMin = newMin2;
                    }
                    if (newMax > newMax2)
                    {
                        newMax = newMax2;
                    }
                }
            }
            //检查最值
            if (newMin < _min)
            {
                newMin = _min;
            }
            if (newMax > _max)
            {
                newMax = _max;
            }
            //注意：最大值小于最小值的场景处理
            long res = 0;
            if (newMax < newMin)
            {
                _resList.add(res);
                CommLog.warn("CommonFunc.redPacketAllocat Max < Min, [{},{}]", newMin, newMax);
            } else
            {
                res = randomLong(newMin, newMax);
                _resList.add(res);
            }

            //递归下一级别
            _depth++;
            int newCount = _count - 1;
            if (newCount > 0)
            {
                long newSum = _sum - res;
                //调用本方法
                redPacketAllocat(newSum, newCount, _per, _min, _max, _resList, _depth);
            }
        }
    }

    /**
     * 在指定列表中查找邮件text
     * @param _platformMailText 平台邮件内容
     * @param _language         指定语言
     * @return NpServerObj_PlatFormMailText
     */
    public static NpServerObj_PlatFormMailText lookupPlatFormMailTextById(ArrayList<NpServerObj_PlatFormMailText> _platformMailText
            , String _language
            , String _defaultLanguage)
    {
        //先检索指定语言
        if (!_language.isEmpty())
        {
            for (NpServerObj_PlatFormMailText text : _platformMailText)
            {
                if (text.getLang().equals(_language))
                {
                    return text;
                }
            }
        }

        //找不到指定语言时，检索默认语言
        for (NpServerObj_PlatFormMailText text : _platformMailText)
        {
            if (text.getLang().equals(_defaultLanguage))
            {
                return text;
            }
        }

        return null;
    }

    public static long simpleDateFormatTimeMs(String timeStr, String format)
    {
        SimpleDateFormat simpleDateFormat = new SimpleDateFormat(format);
        try
        {
            Date date = simpleDateFormat.parse(timeStr);
            return date.getTime();
        } catch (Exception e)
        {
            CommLog.error("", e);
        }
        return 0;
    }

    public static int simpleDateFormatTimeSec(String timeStr, String format)
    {
        return (int) (simpleDateFormatTimeMs(timeStr, format) / 1000);
    }

    public static int simpleDateFormatTimeSec(String timeStr)
    {
        return (int) (simpleDateFormatTimeMs(timeStr, "yyyy-MM-dd HH:mm:ss") / 1000);
    }
    
    public static long simpleDateFormatTimeMs(String timeStr)
    {
        return simpleDateFormatTimeMs(timeStr, "yyyy-MM-dd HH:mm:ss");
    }

    /**
     * 构造宠物战斗截面数据
     * @param _fightData 截面数据
     * @return
     */
    public static String fightInfo2SectionData(NPCommon_PlayerFightInfo _fightData)
    {
        StringBuilder sb = new StringBuilder();
        for (NPCommon_BattleCardInfo cardInfo : _fightData.getPetList())
        {
            sb.append("pet:").append(cardInfo.getPetRefId())
                    .append(",level:").append(cardInfo.getLevel())
                    .append(",star:").append(cardInfo.getStarRefId())
                    .append(",quality:").append(cardInfo.getQuality()).append(";");
        }
        return sb.toString();
    }

    public static String protoItemList2String(List<NPCommon_ItemInfo> _originItemList)
    {
        StringBuilder sb = new StringBuilder();
        for (NPCommon_ItemInfo itemInfo : _originItemList)
        {
            sb.append(String.format("%s-%d:%d", ENPItemType.ENPItemType_FromInt(itemInfo.getItemType()), itemInfo.getSubId(), itemInfo.getCount())).append(";");
        }
        return sb.toString();
    }

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
        String[] strs = charSplit(_sData, token);
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
        String[] strs = charSplit(_sData, token);
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

    public static byte[] decry(byte[] _key, byte[] _msg)
    {
        if (null == _msg)
            return null;

        if (null == _key)
        {
            System.out.println("key is null for decry!");
            return null;
        }

        byte[] deencryMsg = new byte[_msg.length];
        int curPos = 0;
        int keyPos = 0;
        int keyLen = _key.length;

        //逐个字节加密
        while (curPos < _msg.length)
        {
            deencryMsg[curPos] = (byte) (_msg[curPos] ^ _key[keyPos]);

            curPos++;
            //累加key索引
            keyPos++;
            if (keyPos == keyLen)
                keyPos = 0;
        }

        return deencryMsg;
    }

    public static String getHostName()
    {
        try
        {
            InetAddress addr = InetAddress.getLocalHost();//新建一个InetAddress类
            return addr.getHostName();// 获得本机名称
        } catch (Exception e)
        {
            CommLog.error("", e);
            return "unknown_host_name";
        }
    }

    public static String getHostAddress()
    {
        try
        {
            InetAddress addr = InetAddress.getLocalHost();//新建一个InetAddress类
            return addr.getHostAddress();
        } catch (Exception e)
        {
            CommLog.error("", e);
            return "unknown_host_address";
        }
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
                sb.append(strAry.get(i));
                if (null != join && !join.isEmpty())
                {
                    sb.append(join);
                }
            }
        }

        return sb.toString();
    }

    /********
     * 根据长度切割字符串，返回字符串数组
     * @param _str
     * @param _size
     * @return
     */
    public static List<String> splitStringByLength(String _str, int _size)
    {
        List<String> retList = new ArrayList<>();
        if (_size <= 0)
        {
            return retList;
        }
        if (_str == null || _str.isEmpty())
        {
            return retList;
        }
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

    /**
     * 解析获取CRS的服务器ID
     * @param _instanceId
     * @return
     */
    public static int parseCrossRankServerTypeId(long _instanceId)
    {
        return (int) (_instanceId % 100);
    }


    //创建一个事件上下文
    public static Common_Context newContext(ENPGameEvent _eGameEvent)
    {
        Common_Context context = new Common_Context();
        context.setEventId(_eGameEvent.ordinal());
        context.setGuid(WCGGuid.newGuid());
        return context;
    }

    //计算列表中某个物品的数量
    public static long calItemCount(List<NPCommonCostItem> _itmeList,int _itemType,long _itemId)
    {
        //使用stream 优化以下代码
        return _itmeList.stream().filter(item -> item.getItemType().ordinal() == _itemType && item.getItemId() == _itemId).mapToLong(NPCommonCostItem::getCount).sum();
    }

    //计算列表中某个物品的数量
    public static long calItemCountP(List<NPCommon_ItemInfo> _itmeList,int _itemType,long _itemId)
    {
        return _itmeList.stream().filter(item -> item.getItemType() == _itemType && item.getSubId() == _itemId).mapToLong(NPCommon_ItemInfo::getCount).sum();
    }
}
