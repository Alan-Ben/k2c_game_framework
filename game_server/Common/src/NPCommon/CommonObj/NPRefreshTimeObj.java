package NPCommon.CommonObj;

import ALBasicCommon.ALBasicCommonFun;
import NPCommon.Log.CommLog;
import NPCommon.RefData._IParseFromStringable;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Random;
import NPEnum.ENPTimeRefreshType;

import java.util.ArrayList;

/**
 * @description: 刷新类型
 * "刷新类型
 * 按照存在时间到期刷新
 * REF_BY_LIVE
 * 消失即刻马上刷新
 * REF_IF_DIS
 * 按照间隔刷新时间间隔刷新
 * REF_DURATION:MIN(S):MAX(S)
 * 按照定点时间刷新
 * REF_CLOCK:24进制小时数"
 * 按周定点时间刷新
 * REF_WEEK_CLOCK:一周的第x天:24进制小时数
 * @author: ricci
 * @date: 2022-08-31 16:48:04
 */
public class NPRefreshTimeObj implements _IParseFromStringable
{
    //刷新类型
    private ENPTimeRefreshType _m_refreshType = ENPTimeRefreshType.NONE;
    //刷新参数
    private ArrayList<Integer> _m_lParamList = new ArrayList<>();
    
    public NPRefreshTimeObj()
    {
    }
    public NPRefreshTimeObj(ENPTimeRefreshType _type, int _param)
    {
    	_m_refreshType = _type;
    	_m_lParamList.add(_param);
    }
    public NPRefreshTimeObj(ENPTimeRefreshType _type, int _param0, int _param1)
    {
    	_m_refreshType = _type;
    	_m_lParamList.add(_param0);
    	_m_lParamList.add(_param1);
    }

    public ENPTimeRefreshType getFreshType() {return _m_refreshType;}

    /***********
     * 根据下标获取参数
     * @param _idx
     * @return
     */
    public int getParam(int _idx)
    {
        if (_idx >= _m_lParamList.size())
            return -1;

        return _m_lParamList.get(_idx);
    }

    @Override
    public boolean parseFromString(String sValue)
    {
        NPStringReader sr = new NPStringReader(sValue);
        try
        {

            String type = sr.readItem();
            if (type == null || type.isEmpty())
            {
                return true;
            }
            _m_refreshType = ENPTimeRefreshType.valueOf(type);

            String tmpS = sr.readItem();
            while (tmpS != null)
            {
                //转化数字
                int param = Integer.parseInt(tmpS);
                //放入队列
                _m_lParamList.add(param);

                tmpS = sr.readItem();
            }
        } catch (Exception e)
        {
            e.printStackTrace();
        }

        return true;
    }

    /****************
     * 根据刷新类型判断是否只根据生命周期时间刷新
     * @return
     */
    public boolean isOnlyCheckLiveTime()
    {
        switch (_m_refreshType)
        {
            //只有按照存在时间到期刷新，才需要只判断存在时间，防止误刷
            case REF_BY_LIVE:
                return true;

            default:
                return false;
        }
    }

    /**************
     * 是否允许使用增量刷新
     * 部分刷新模式是不允许增量刷新的。
     * 如：
     * 按照存在时间到期刷新
     * REF_BY_LIVE
     * 消失即刻马上刷新
     * REF_IF_DIS
     * @return
     */
    public boolean canUseAddFresh()
    {
        switch (_m_refreshType)
        {
            //只有按照存在时间到期刷新，才需要只判断存在时间，防止误刷
            case REF_BY_LIVE:
            case REF_IF_DIS:
                return false;

            default:
                return true;
        }
    }

    /****************
     * 根据当前时间，获取下一个刷新时间戳
     * 根据刷新类型计算下一次刷新时间
     * <p>
     * 刷新类型
     * 按照存在时间到期刷新
     * REF_BY_LIVE
     * 消失即刻马上刷新
     * REF_IF_DIS
     * 按照间隔刷新时间间隔刷新
     * REF_DURATION:MIN(S):MAX(S)
     * 按照定点时间刷新
     * REF_CLOCK:24进制小时数
     * @return
     */
    public long getNextFreshTimeTagMS(long _lastRefreshTimeTagMS)
    {
        long nowTimeMS = ALBasicCommonFun.getNowTimeMS();

        try
        {
            switch (_m_refreshType)
            {
                //按照生命周期的都是需要马上刷新的，这种返回-1
                case NONE:
                case REF_IF_DIS:
                case REF_BY_LIVE:
                    return -1;

                //根据刷新周期计算
                case REF_DURATION:
                    //修正异常数据
                    if (-1 == _lastRefreshTimeTagMS)
                        _lastRefreshTimeTagMS = nowTimeMS;

                    //累加周期
                    int minS = getParam(0);
                    int maxS = getParam(1);

                    int addTimeS = 0;
                    if (minS < 0)
                        addTimeS = maxS;
                    else if (maxS < 0)
                        addTimeS = minS;
                    else
                    {
                        //随机范围处理
                        int rangeS = Math.abs(maxS - minS);
                        rangeS = Random.nextInt(rangeS);

                        //增加对应时间
                        addTimeS = rangeS + minS;
                    }

                    //当数据过小的时候使用新数据处理
                    if (addTimeS < 10)
                        addTimeS = 10;

                    //计算新时间
                    long freshTime = _lastRefreshTimeTagMS + (addTimeS * 1000L);

                    return freshTime;

                case REF_CLOCK:
                    //修正异常数据
                    if (-1 == _lastRefreshTimeTagMS)
                        _lastRefreshTimeTagMS = nowTimeMS;

                    long finalFreshTimeTagMs = -1;
                    //逐个获取时间比对最早的
                    for (int i = 0; i < _m_lParamList.size(); i++)
                    {
                        int clockHour = _m_lParamList.get(i);
                        //获取时间戳
                        long freshTagMS;
                        //使用上次刷新时间估计下次刷新时间
                        freshTagMS = CommonFunc.getNextAssignTimeMs(_lastRefreshTimeTagMS, clockHour, 0);
                        //判断是否有效,支持配置多个时间点，选择离当前时间最近的刷新时间
                        if (-1 == finalFreshTimeTagMs || freshTagMS < finalFreshTimeTagMs)
                            finalFreshTimeTagMs = freshTagMS;
                    }

                    return finalFreshTimeTagMs;
                case REF_WEEK_CLOCK:
                {
                    //修正异常数据
                    if (-1 == _lastRefreshTimeTagMS)
                        _lastRefreshTimeTagMS = nowTimeMS;

                    int dayOfWeek = _m_lParamList.get(0);
                    int clockHour = _m_lParamList.get(1);
                    //使用上次刷新时间估计下次刷新时间
                    return CommonFunc.getNextAssignTimeMs(_lastRefreshTimeTagMS, dayOfWeek, clockHour, 0);
                }
                case REF_MONTH_CLOCK:
                {
                    //修正异常数据
                    if (-1 == _lastRefreshTimeTagMS)
                        _lastRefreshTimeTagMS = nowTimeMS;

                    int dayOfMonth = _m_lParamList.get(0);
                    int clockHour = _m_lParamList.get(1);
                    //使用上次刷新时间估计下次刷新时间
                    return CommonFunc.getNextSpecificDayOfMonthTimeMs(_lastRefreshTimeTagMS, dayOfMonth, clockHour, 0);
                }
                case REF_MIN_CLOCK:
                {
                	//修正异常数据
                    if (-1 == _lastRefreshTimeTagMS)
                        _lastRefreshTimeTagMS = nowTimeMS;

                    int clockHour = _m_lParamList.get(0);
                    int clockMin = _m_lParamList.get(1);
                    
                    //估计下次刷新时间
                    long freshTagMS = CommonFunc.getNextAssignTimeMs(_lastRefreshTimeTagMS, clockHour, clockMin);

                    return freshTagMS;
                }
                default:
                    return Long.MAX_VALUE;
            }
        } catch (Exception e)
        {
            CommLog.error("getNextFreshTimeTagMS error", e);
            //读取发生异常
            return nowTimeMS + 10000;
        }
    }

    /****************
     * 根据当前时间，获取上一个刷新时间戳
     * 根据刷新类型计算上一次刷新时间
     * <p>
     * 刷新类型
     * 按照存在时间到期刷新
     * REF_BY_LIVE
     * 消失即刻马上刷新
     * REF_IF_DIS
     * 按照间隔刷新时间间隔刷新
     * REF_DURATION:MIN(S):MAX(S)
     * 按照定点时间刷新
     * REF_CLOCK:24进制小时数
     * @return
     */
    public long getBeforeFreshTimeTagMS(long _lastRefreshTimeTagMS)
    {
        long nowTimeMS = ALBasicCommonFun.getNowTimeMS();

        try
        {
            switch (_m_refreshType)
            {
                //按照生命周期的都是需要马上刷新的，这种返回-1
                case NONE:
                case REF_IF_DIS:
                case REF_BY_LIVE:
                case REF_DURATION:
                    return -1;
                case REF_CLOCK:
                    //修正异常数据
                    if (-1 == _lastRefreshTimeTagMS)
                        _lastRefreshTimeTagMS = nowTimeMS;

                    long finalFreshTimeTagMs = -1;
                    //逐个获取时间比对最早的
                    for (int i = 0; i < _m_lParamList.size(); i++)
                    {
                        int clockHour = _m_lParamList.get(i);
                        //获取时间戳
                        long freshTagMS;
                        //使用上次刷新时间估计下次刷新时间
                        freshTagMS = CommonFunc.getNextAssignTimeMs(_lastRefreshTimeTagMS - 86400000, clockHour, 0);
                        //判断是否有效,支持配置多个时间点，选择离当前时间最近的刷新时间
                        if (-1 == finalFreshTimeTagMs || freshTagMS > finalFreshTimeTagMs)
                            finalFreshTimeTagMs = freshTagMS;
                    }

                    return finalFreshTimeTagMs;
                case REF_WEEK_CLOCK:
                {
                    int dayOfWeek = _m_lParamList.get(0);
                    int clockHour = _m_lParamList.get(1);
                    //使用上次刷新时间估计下次刷新时间
                    return CommonFunc.getNextAssignTimeMs(nowTimeMS - 86400000 * 7, dayOfWeek, clockHour, 0);
                }
                case REF_MONTH_CLOCK:
                {
                    int dayOfMonth = _m_lParamList.get(0);
                    int clockHour = _m_lParamList.get(1);
                    //使用上次刷新时间估计下次刷新时间
                    return CommonFunc.getPreviousSpecificDayOfMonthTimeMs(nowTimeMS, dayOfMonth, clockHour, 0);
                }
                default:
                    return Long.MAX_VALUE;
            }
        } catch (Exception e)
        {
            CommLog.error("getBeforeFreshTimeTagMS error", e);
            //读取发生异常
            return nowTimeMS + 10000;
        }
    }
}
