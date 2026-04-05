package NPCommon.CommonObj;

import NPCommon.Enum.NPCommonEnum.ENPVariableCalType;
import NPCommon.Log.CommLog;
import NPCommon.RefData._IParseFromStringable;
import NPCommon.Util.CommonFunc;

/**
 * 计算倍率（参考：高级公式的计算逻辑（_ATNPBasicVariableGroupObj））
 		只支持1层计算
 		只支持 加，减，乘，除
 * 
 * @author mj
 *
 */
public class NPCountRate implements _IParseFromStringable
{
    private static char[] _g_arrTotalSplitArr = {'+', '-', '*', '|'};

    private int _m_iRate;
    protected ENPVariableCalType _m_eCalType;
    private String _m_sParam;

    public NPCountRate()
    {
        _m_iRate = 0;
        _m_eCalType = ENPVariableCalType.NONE;
        _m_sParam = null;
    }

    public NPCountRate(int _rate, String _param)
    {
        _m_iRate = _rate;
        _m_sParam = _param;
    }

    public int rate()
    {
        return _m_iRate;
    }
    
    public ENPVariableCalType type()
    {
        return _m_eCalType;
    }

    public String param()
    {
        return _m_sParam;
    }

    /**
     * 只支持如下三种情况，后续有需要拓展再处理
     		补充说明：已经上线运行，改造高级公式成本过高，所以作为特殊处理存在
     		支持类型：完整类型[字符串格式，操作符号，数字]，有部分选填
     			1.  [数字]，则 _m_iRate 赋值即可，使用时直接取 _m_iRate
     			2. [字符串格式]，必须无操作符号，则 _m_sParam 赋值即可，使用时取 事件参数 _m_sParam 的数值
     			3. [字符串格式，操作符号，数字]，则 _m_iRate 操作(+,-,*,/) 事件参数 _m_sParam 的数值
     */
    @Override
    public boolean parseFromString(String sValue)
    {
        //构造读取对象
        NPStringReader stringReader = new NPStringReader(sValue);
        if (stringReader.isEmpty())
            return true;
        
        //1.  [数字]，则 _m_iRate 赋值即可，使用时直接取 _m_iRate
        if (CommonFunc.isNumeric(sValue))
        {
            this._m_iRate = Integer.parseInt(sValue);
            return true;
        }

        char tmpChar;
        
        try
        {
            //读取字符串
            String splitStr = stringReader.readItem(_g_arrTotalSplitArr);
            //获取最后一个分隔符
            tmpChar = stringReader.getLastSpliteChar();
            
            //2. [字符串格式]，必须无操作符号，则 _m_sParam 赋值即可，使用时取 事件参数 _m_sParam 的数值
            if (null != splitStr && splitStr.length() == sValue.length())
            {
                this._m_sParam = splitStr.toUpperCase();
                return true;
            }
            
            //3. [字符串格式，操作符号，数字]，则 _m_iRate 操作(+,-,*,/) 事件参数 _m_sParam 的数值
            this._m_sParam = splitStr.toUpperCase();
            //检查对应的操作符号
            if (tmpChar == '+')
            {
            	this._m_eCalType = ENPVariableCalType.ADD;
            } else if (tmpChar == '-')
            {
            	this._m_eCalType = ENPVariableCalType.SUB;
            } else if (tmpChar == '*')
            {
            	this._m_eCalType = ENPVariableCalType.MUL;
            } else if (tmpChar == '|')
            {
            	this._m_eCalType = ENPVariableCalType.DIV;
            }
            //数值部分
            String rateS = stringReader.readItem();
            this._m_iRate = Integer.parseInt(rateS);
            
            //除法时需要额外检查
            if(this._m_eCalType == ENPVariableCalType.DIV && 
                    this._m_iRate == 0)
            {
            	CommLog.error("NPCountRate fail - div is 0：str ={}", sValue);
                return false;
            }
            
            return true;
        }
        catch (Exception e)
        {
            CommLog.error("NPCountRate fail：str ={}", sValue, e);
            return false;
        }
    }
}
