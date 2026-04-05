package NPGameRes.GameObjs.Battle;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.Battle.Variable.WCGVariableGroupObj.EWCGVariableCalType;
import WCGCommon.Enum.NPEnum.EWCGTeamVariableType;

import java.util.ArrayList;
import java.util.List;

/*********************
 * 参数对象序列号结构体
 **/
public class WCGTeamVariableGroupObj
{

    /**
     * 本对象的条件类型
     */
    public WCGTeamVariableObj variableObj;
    public List<WCGTeamVariableGroupObj> childGroupList;
    public EWCGVariableCalType calType = EWCGVariableCalType.NONE;

    protected WCGTeamVariableGroupObj()
    {
        variableObj = null;
        childGroupList = new ArrayList<WCGTeamVariableGroupObj>();
    }

    public boolean hasVariable()
    {
        return (variableObj != null && variableObj.variableType != EWCGTeamVariableType.NONE)
                || (childGroupList != null && childGroupList.size() > 0);
    }

    /**************************************
     * 可能的高级条件读取方式函数，日后可能需要用到，有兴趣可参看
     **/
    private static char[] _g_arrTotalSplitArr = {'(', ')', '+', '-', '*', '|', '<', '>'};
    private static char[] _g_arrContainArr = {'(', ')'};


    //将字符串转化为本对象
    public static WCGTeamVariableGroupObj readVariableGroup(String _str, String _errHeader)
    {
        return readVariableGroup(_str, 0, 1, _errHeader);
    }

    public static WCGTeamVariableGroupObj readVariableGroup(String _str, int _startIdx, int _depth, String _err)
    {
        if (_depth > 5)
        {
            CommLog.error(_err);
            CommLog.error(" Notice!!! Single Variable depth more than 5!!! conditionStr: " + _str + " depth: " + _depth);
        }

        if (_str == null || _str.isEmpty())
            return null;

        WCGTeamVariableGroupObj obj = new WCGTeamVariableGroupObj();
        //下一个读取对象
        WCGTeamVariableGroupObj newGroupObj = new WCGTeamVariableGroupObj();

        int startIdx = _startIdx;
        int index;
        char tmpChar;

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
                if (tmpChar == '(')
                {
                    StringBuilder infoStr = new StringBuilder();
                    //读取下一个内容
                    index = _readStringToEnd(_str, index, infoStr, _err);

                    //读取内容对象
                    //UnityEngine.CommLog.error(_err + " variable: " + infoStr.toString());
                    WCGTeamVariableGroupObj newObj = readVariableGroup(infoStr.toString(), 0, _depth + 1, _err);
                    if (null != newObj)
                    {
                        newObj.calType = newGroupObj.calType;
                        //重置状态
                        newGroupObj.calType = EWCGVariableCalType.NONE;
                        //加入队列
                        obj.childGroupList.add(newObj);
                    }

                    if (index == -1)
                        break;
                } else if (tmpChar == ')')
                {
                    //报错，无)对应
                    CommLog.error(_err);
                    CommLog.error("Read Single Variable Err: " + _str + " there is a ex ')' !");
                } else if (tmpChar == '+')
                {
                    //UnityEngine.CommLog.error(_err + "set add");
                    //取出字符串进行处理
                    if (newGroupObj.calType != EWCGVariableCalType.NONE)
                    {
                        CommLog.error(_err);
                        CommLog.error("同一个对象前, 只能使用一个关系符号  \'+\' 或者 \'-\' 或者 \'*\' 或者 \'|\'");
                    }
                    newGroupObj.calType = EWCGVariableCalType.ADD;
                } else if (tmpChar == '-')
                {
                    //UnityEngine.CommLog.error(_err + "set sub");
                    //取出字符串进行处理
                    if (newGroupObj.calType != EWCGVariableCalType.NONE)
                    {
                        CommLog.error(_err);
                        CommLog.error("同一个对象前, 只能使用一个关系符号  \'+\' 或者 \'-\' 或者 \'*\' 或者 \'|\'");
                    }
                    newGroupObj.calType = EWCGVariableCalType.SUB;
                } else if (tmpChar == '*')
                {
                    //UnityEngine.CommLog.error(_err + "set mul");
                    //取出字符串进行处理
                    if (newGroupObj.calType != EWCGVariableCalType.NONE)
                    {
                        CommLog.error(_err);
                        CommLog.error("同一个对象前, 只能使用一个关系符号  \'+\' 或者 \'-\' 或者 \'*\' 或者 \'|\'");
                    }
                    newGroupObj.calType = EWCGVariableCalType.MUL;
                } else if (tmpChar == '|')
                {
                    //UnityEngine.CommLog.error(_err + "set div");
                    //取出字符串进行处理
                    if (newGroupObj.calType != EWCGVariableCalType.NONE)
                    {
                        CommLog.error(_err);
                        CommLog.error("同一个对象前, 只能使用一个关系符号  \'+\' 或者 \'-\' 或者 \'*\' 或者 \'|\'");
                    }
                    newGroupObj.calType = EWCGVariableCalType.DIV;
                } else if (tmpChar == '>')
                {
                    //UnityEngine.Debug.LogError(_err + "set div");
                    //取出字符串进行处理
                    if (newGroupObj.calType != EWCGVariableCalType.NONE)
                    {
                        CommLog.error(_err);
                        CommLog.error("同一个对象前, 只能使用一个关系符号  \'+\' 或者 \'-\' 或者 \'*\' 或者 \'|\'");
                    }
                    newGroupObj.calType = EWCGVariableCalType.MAX;
                } else if (tmpChar == '<')
                {
                    //UnityEngine.Debug.LogError(_err + "set div");
                    //取出字符串进行处理
                    if (newGroupObj.calType != EWCGVariableCalType.NONE)
                    {
                        CommLog.error(_err);
                        CommLog.error("同一个对象前, 只能使用一个关系符号  \'+\' 或者 \'-\' 或者 \'*\' 或者 \'|\'");
                    }
                    newGroupObj.calType = EWCGVariableCalType.MIN;
                }

                //累加
                startIdx = index + 1;
            } else if (index != -1)//此时取出字符串进行条件读取
            {
                StringBuilder infoStr = new StringBuilder(_str.substring(startIdx, index));
                //获取当前分隔符
                tmpChar = _str.charAt(index);

                while (index != -1 && tmpChar != '+' && tmpChar != '-' && tmpChar != '*' && tmpChar != '|' && tmpChar != '<' && tmpChar != '>')
                {
                    if (tmpChar == '(')
                    {
                        //添加括号
                        infoStr.append('(');
                        index = _readStringToEnd(_str, index, infoStr, _err);
                        //添加括号
                        infoStr.append(')');

                        //到结尾则返回
                        if (-1 == index)
                            break;

                        //增加读取下标
                        startIdx = index + 1;

                        //搜索下一个分割节点
                        index = CommonFunc.indexOfAny(_str, _g_arrTotalSplitArr, startIdx);
                    } else if (tmpChar == ')')
                    {
                        //报错，无)对应
                        CommLog.error(_err + "Read variable Err: " + _str + " there is a ex ')' !");
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

                //先读取对象，并创建新的下一个对象
                //UnityEngine.CommLog.error(_err + "single variable: " + infoStr.toString());
                //取完字符串，使用截取的字符串读取对应条件
                newGroupObj.variableObj = WCGTeamVariableObj.readVariable(infoStr.toString());
                //加入队列
                obj.childGroupList.add(newGroupObj);
                //创建新的对象
                newGroupObj = new WCGTeamVariableGroupObj();

                if (-1 != index)
                {
                    if (tmpChar == '+')
                    {
                        //UnityEngine.CommLog.error(_err + "set add");
                        //取出字符串进行处理
                        if (newGroupObj.calType != EWCGVariableCalType.NONE)
                        {
                            CommLog.error(_err);
                            CommLog.error("同一个对象前, 只能使用一个关系符号  \'+\' 或者 \'-\' 或者 \'*\' 或者 \'|\'");
                        }
                        newGroupObj.calType = EWCGVariableCalType.ADD;
                    } else if (tmpChar == '-')
                    {
                        //UnityEngine.CommLog.error(_err + "set sub");
                        //取出字符串进行处理
                        if (newGroupObj.calType != EWCGVariableCalType.NONE)
                        {
                            CommLog.error(_err);
                            CommLog.error("同一个对象前, 只能使用一个关系符号  \'+\' 或者 \'-\' 或者 \'*\' 或者 \'|\'");
                        }
                        newGroupObj.calType = EWCGVariableCalType.SUB;
                    } else if (tmpChar == '*')
                    {
                        //UnityEngine.CommLog.error(_err + "set mul");
                        //取出字符串进行处理
                        if (newGroupObj.calType != EWCGVariableCalType.NONE)
                        {
                            CommLog.error(_err);
                            CommLog.error("同一个对象前, 只能使用一个关系符号  \'+\' 或者 \'-\' 或者 \'*\' 或者 \'|\'");
                        }
                        newGroupObj.calType = EWCGVariableCalType.MUL;
                    } else if (tmpChar == '|')
                    {
                        //UnityEngine.CommLog.error(_err + "set div");
                        //取出字符串进行处理
                        if (newGroupObj.calType != EWCGVariableCalType.NONE)
                        {
                            CommLog.error(_err);
                            CommLog.error("同一个对象前, 只能使用一个关系符号  \'+\' 或者 \'-\' 或者 \'*\' 或者 \'|\'");
                        }
                        newGroupObj.calType = EWCGVariableCalType.DIV;
                    } else if (tmpChar == '>')
                    {
                        //UnityEngine.Debug.LogError(_err + "set div");
                        //取出字符串进行处理
                        if (newGroupObj.calType != EWCGVariableCalType.NONE)
                        {
                            CommLog.error(_err);
                            CommLog.error("同一个对象前, 只能使用一个关系符号  \'+\' 或者 \'-\' 或者 \'*\' 或者 \'|\'");
                        }
                        newGroupObj.calType = EWCGVariableCalType.MAX;
                    } else if (tmpChar == '<')
                    {
                        //UnityEngine.Debug.LogError(_err + "set div");
                        //取出字符串进行处理
                        if (newGroupObj.calType != EWCGVariableCalType.NONE)
                        {
                            CommLog.error(_err);
                            CommLog.error("同一个对象前, 只能使用一个关系符号  \'+\' 或者 \'-\' 或者 \'*\' 或者 \'|\'");
                        }
                        newGroupObj.calType = EWCGVariableCalType.MIN;
                    }
                }

                //累加
                startIdx = index + 1;
            } else //此时取出字符串进行条件读取
            {
                StringBuilder infoStr = new StringBuilder(_str.substring(startIdx));

                //UnityEngine.CommLog.error(_err + " single variable: " + infoStr.toString());
                //取完字符串，使用截取的字符串读取对应条件
                newGroupObj.variableObj = WCGTeamVariableObj.readVariable(infoStr.toString());
                //加入队列
                obj.childGroupList.add(newGroupObj);
                newGroupObj = new WCGTeamVariableGroupObj();
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
                CommLog.error("Read Single Variable Err: " + _str + " there is no ')' to be fixed!");
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
            if (tmpChar == ')')
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
