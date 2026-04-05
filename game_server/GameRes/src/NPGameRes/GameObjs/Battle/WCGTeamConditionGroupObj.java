package NPGameRes.GameObjs.Battle;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.Battle.WCGSingleConditionGroupObj.EWCGJudgeChar;
import WCGCommon.Enum.NPEnum.EWCGTeamConditionType;

import java.util.ArrayList;
import java.util.List;

public class WCGTeamConditionGroupObj
{
    /**
     * 本对象的条件类型
     */
    public boolean trueEnable;  //是否正向正确判断
    public WCGTeamConditionObj conditionObj;

    public List<WCGTeamConditionGroupObj> childConditionList;
    public EWCGJudgeChar judgeType = EWCGJudgeChar.NONE;

    protected WCGTeamConditionGroupObj()
    {
        trueEnable = true;
        conditionObj = new WCGTeamConditionObj();
        childConditionList = new ArrayList<WCGTeamConditionGroupObj>();
    }

    public boolean hasCondition()
    {
        return (conditionObj != null && conditionObj.conditionType != EWCGTeamConditionType.NONE)
                || (childConditionList != null && childConditionList.size() > 0);
    }

    /**************************************
     * 可能的高级条件读取方式函数，日后可能需要用到，有兴趣可参看
     **/
    public static char _g_bracket_b = '(';
    public static char _g_bracket_e = ')';
    public static char _g_or = '|';
    public static char _g_and = '#';
    private static char _g_not = '!';
    public static char[] _g_arrTotalSplitArr = {_g_bracket_b, _g_bracket_e, _g_or, _g_and, _g_not};

    public static char[] _g_arrContainArr = {_g_bracket_b, _g_bracket_e};


    //将字符串转化为本对象
    public static WCGTeamConditionGroupObj readConditionGroupList(String _str, String _err)
    {
        return readConditionGroupList(_str, 0, 1, _err);
    }

    public static WCGTeamConditionGroupObj readConditionGroupList(String _str, int _startIdx, int _depth,
                                                                  String _err)
    {
        if (_depth > 5)
        {
            CommLog.error(_err);
            CommLog.error(" Notice!!! Single Condition depth more than 5!!! conditionStr: " + _str + " depth: " +
                    _depth);
        }

        if (_str == null || _str.isEmpty())
            return null;

        WCGTeamConditionGroupObj obj = new WCGTeamConditionGroupObj();

        int startIdx = _startIdx;
        int index;
        char tmpChar;
        boolean isTrueEnable = true;
        //读取第一个分隔位置信息
        do
        {
            index = CommonFunc.indexOfAny(_str, _g_arrTotalSplitArr, startIdx);
            //判断是否与开始位置一致，不一致表示有信息需要读取
            if (index == startIdx)
            {
                //获取当前分隔符
                tmpChar = _str.charAt(index);
                //判断此符号是否(，是则开始一个新集合的读取
                if (tmpChar == _g_bracket_b)
                {
                    StringBuilder infoStr = new StringBuilder();
                    //读取下一个内容
                    index = _readStringToEnd(_str, index, infoStr, _err);

                    //读取内容对象
                    //UnityEngine.CommLog.error(" sub conddi11: " + infoStr.ToString());
                    WCGTeamConditionGroupObj newObj =
                            readConditionGroupList(infoStr.toString(), 0, _depth + 1, _err);
                    //设置正向还是反向判断
                    newObj.trueEnable = isTrueEnable;
                    //重置正向
                    isTrueEnable = true;

                    if (null != newObj)
                        obj.childConditionList.add(newObj);

                    if (index == -1)
                        break;
                } else if (tmpChar == _g_bracket_e)
                {
                    //报错，无)对应
                    CommLog.error(_err);
                    CommLog.error("Read Single Condition Err: " + _str + " there is a ex ')' !");
                } else if (tmpChar == _g_or)
                {
                    //UnityEngine.CommLog.error("set or");
                    //取出字符串进行处理
                    if (obj.judgeType == EWCGJudgeChar.AND)
                    {
                        CommLog.error(_err);
                        CommLog.error("同一个括号内使用不同的关系符号, 只能使用一个关系符号  \'|\' 或者 \'~\'");
                    }
                    obj.judgeType = EWCGJudgeChar.OR;
                } else if (tmpChar == _g_and)
                {
                    //UnityEngine.CommLog.error("set and");
                    //取出字符串进行处理
                    if (obj.judgeType == EWCGJudgeChar.OR)
                    {
                        CommLog.error(_err);
                        CommLog.error("同一个括号内使用不同的关系符号, 只能使用一个关系符号  \'|\' 或者 \'~\'");
                    }
                    obj.judgeType = EWCGJudgeChar.AND;
                } else if (tmpChar == _g_not)
                {
                    //UnityEngine.Debug.LogError("set not");
                    //设置下一个判断条件反向
                    isTrueEnable = !isTrueEnable;
                }
                //累加
                startIdx = index + 1;
            } else if (index != -1) //此时取出字符串进行条件读取
            {
                StringBuilder infoStr = new StringBuilder(_str.substring(startIdx, index));
                //获取当前分隔符
                tmpChar = _str.charAt(index);

                while (index != -1 && tmpChar != _g_or && tmpChar != _g_and)
                {
                    if (tmpChar == _g_bracket_b)
                    {
                        //添加括号
                        infoStr.append(_g_bracket_b);
                        index = _readStringToEnd(_str, index, infoStr, _err);
                        //添加括号
                        infoStr.append(_g_bracket_e);

                        //到结尾则返回
                        if (-1 == index)
                            break;

                        //增加读取下标
                        startIdx = index + 1;

                        //搜索下一个分割节点
                        index = CommonFunc.indexOfAny(_str, _g_arrTotalSplitArr, startIdx);
                        //将信息加入
                        if (index == -1)
                            infoStr.append(_str.substring(startIdx));
                        else
                            infoStr.append(_str.substring(startIdx, index));
                    } else if (tmpChar == _g_bracket_e)
                    {
                        //报错，无)对应
                        CommLog.error(_err + "Read Single Condition Err: " + _str + " there is a ex ')' !");
                        startIdx = index + 1;

                        //搜索下一个分割节点
                        index = CommonFunc.indexOfAny(_str, _g_arrTotalSplitArr, startIdx);
                        //将信息加入
                        if (index == -1)
                            infoStr.append(_str.substring(startIdx));
                        else
                            infoStr.append(_str.substring(startIdx, index));
                    }

                    //获取当前分隔符
                    if (-1 != index)
                        tmpChar = _str.charAt(index);
                }

                if (-1 != index)
                {
                    if (tmpChar == _g_or)
                    {
                        //UnityEngine.CommLog.error("set or");
                        //取出字符串进行处理
                        if (obj.judgeType == EWCGJudgeChar.AND)
                        {
                            CommLog.error(_err);
                            CommLog.error("同一个括号内使用不同的关系符号, 只能使用一个关系符号  \'|\' 或者 \'~\'");
                        }
                        obj.judgeType = EWCGJudgeChar.OR;
                    } else if (tmpChar == _g_and)
                    {
                        //UnityEngine.CommLog.error("set and");
                        //取出字符串进行处理
                        if (obj.judgeType == EWCGJudgeChar.OR)
                        {
                            CommLog.error(_err);
                            CommLog.error("同一个括号内使用不同的关系符号, 只能使用一个关系符号  \'|\' 或者 \'~\'");
                        }
                        obj.judgeType = EWCGJudgeChar.AND;
                    }
                }

                //UnityEngine.CommLog.error("single conddi: " + infoStr.ToString());
                //取完字符串，使用截取的字符串读取对应条件
                WCGTeamConditionGroupObj newObj = new WCGTeamConditionGroupObj();
                newObj.conditionObj = WCGTeamConditionObj.readCondition(infoStr.toString());
                //设置正向还是反向判断
                newObj.trueEnable = isTrueEnable;
                //重置正向
                isTrueEnable = true;
                //加入队列
                obj.childConditionList.add(newObj);

                //累加
                startIdx = index + 1;
            } else //此时取出字符串进行条件读取
            {
                StringBuilder infoStr = new StringBuilder(_str.substring(startIdx));

                //UnityEngine.CommLog.error(index + " - " + startIdx + " single conddi: " + infoStr.ToString());
                //取完字符串，使用截取的字符串读取对应条件
                WCGTeamConditionGroupObj newObj = new WCGTeamConditionGroupObj();
                newObj.conditionObj = WCGTeamConditionObj.readCondition(infoStr.toString());
                //设置正向还是反向判断
                newObj.trueEnable = isTrueEnable;
                //重置正向
                isTrueEnable = true;
                //加入队列
                obj.childConditionList.add(newObj);
            }
        } while (index != -1 && startIdx < _str.length());

        return obj;
    }

    /********************
     * 读取到对应字符串括号的结尾
     **/
    protected static int _readStringToEnd(String _str, int _startIndex, StringBuilder _recBuilder, String _err)
    {
        int containCount = 1;
        int index = _startIndex;
        char tmpChar = (char) 0;
        //此时表示一个参数，将对应的反括号获取到
        do
        {
            //查询下一个容器分割符
            _startIndex = index + 1;
            index = CommonFunc.indexOfAny(_str, _g_arrContainArr, _startIndex);
            if (index == -1)
            {
                //未查询到对应索引，表示无对应
                //报错，无)对应
                CommLog.error(_err);
                CommLog.error("Read Single Condition Err: " + _str + " there is no ')' to be fixed!");
                _recBuilder.append(_str, _startIndex, _str.length() - _startIndex);
                return -1;
            }

            //累加字符串
            if (tmpChar != 0)
                _recBuilder.append(tmpChar);
            _recBuilder.append(_str.substring(_startIndex, index));

            //获取新字符
            tmpChar = _str.charAt(index);

            //判断是否到结尾
            if (tmpChar == _g_bracket_e)
            {
                //增加容器对应值
                containCount--;
            } else
            {
                //增加容器对应值
                containCount++;
            }
        } while (containCount > 0);

        return index;
    }
}