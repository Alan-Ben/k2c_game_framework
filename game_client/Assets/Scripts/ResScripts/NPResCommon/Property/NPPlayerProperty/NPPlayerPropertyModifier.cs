using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using GOE;
using NPEnum;

/************************
 * 属性加成信息对象
 **/
[System.Serializable]
public class NPPlayerPropertyModifier
{
    public List<NPPlayerPropertyInfoObj> propertyObjList = new List<NPPlayerPropertyInfoObj>();

    public long getPropertyValue(ENPPlayerPropertyType _propertyType)
    {
        for (int i = 0; i < propertyObjList.Count; i++)
        {
            if (propertyObjList[i].type == _propertyType)
                return propertyObjList[i].value;
        }

        return 0;
    }

    /// <summary>
    /// 单个数据的读取函数
    /// </summary>
    /// <param name="_str"></param>
    public void ParseFromString(string _str)
    {
        readStr(_str, string.Empty);
    }

    /******************
     * 从带入的字符串内读取属性加成信息
     * 
     * @author alzq.z
     * @time   Aug 27, 2013 10:57:11 PM
     */
    public void readStr(string _str, string _fieldName = "")
    {
        if (string.IsNullOrEmpty(_str))
            return;

        string[] strs = _str.Split(';');
        for (int i = 0; i < strs.Length; i++)
        {
            string itemStr = strs[i];
            //解析属性对象信息
            NPPlayerPropertyInfoObj infoObj = NPPlayerPropertyInfoObj.readPropertyInfoObj(itemStr, _fieldName);
            if (null == infoObj)
                continue;

            //加入数据集
            propertyObjList.Add(infoObj);
        }
    }

    /******************
     * 从带入的字符串内读取属性加成信息
     * 
     * @author alzq.z
     * @time   Aug 27, 2013 10:57:11 PM
     */
    public static NPPlayerPropertyModifier readPropertyModifier(string _str, string _fieldName)
    {
        if (string.IsNullOrEmpty(_str))
            return null;

        NPPlayerPropertyModifier modifier = new NPPlayerPropertyModifier();

        modifier.readStr(_str, _fieldName);

        return modifier;
    }


    /****
     * 深度拷贝复制一个对象
     * @return
     */
    public NPPlayerPropertyModifier duplicate()
    {
        NPPlayerPropertyModifier ret = new NPPlayerPropertyModifier();
        foreach (NPPlayerPropertyInfoObj npPropertyInfoObj in propertyObjList)
        {
            ret.propertyObjList.Add(npPropertyInfoObj.duplicate());
        }

        return ret;
    }

    public NPPlayerPropertyModifier duplicate(int _stack)
    {
        NPPlayerPropertyModifier ret = new NPPlayerPropertyModifier();
        foreach (NPPlayerPropertyInfoObj npPropertyInfoObj in propertyObjList)
        {
            ret.propertyObjList.Add(npPropertyInfoObj.duplicate(_stack));
        }

        return ret;
    }

    /****
     * 查找某个属性
     * @param _type
     * @return
     */
    private NPPlayerPropertyInfoObj lookup(ENPPlayerPropertyType _type)
    {
        foreach (NPPlayerPropertyInfoObj npPropertyInfoObj in propertyObjList)
        {
            if (npPropertyInfoObj.type == _type)
            {
                return npPropertyInfoObj;
            }
        }

        return null;
    }


    /// <summary>
    /// 将给定modify合并到自己身上
    /// </summary>
    /// <param name="_modifier"></param>
    /// <returns></returns>
    public NPPlayerPropertyModifier mergeM(NPPlayerPropertyModifier _modifier)
    {
        foreach (NPPlayerPropertyInfoObj obj2 in _modifier.propertyObjList)
        {
            NPPlayerPropertyInfoObj obj1 = lookup(obj2.type);
            if (null != obj1)
            {
                obj1.value += obj2.value;
            }
            else
            {
                propertyObjList.Add(obj2.duplicate());
            }
        }

        return this;
    }

    /// <summary>
    /// 将给定万分比加成到自己身上
    /// </summary>
    /// <param name="_multiple"></param>
    /// <returns></returns>
    public NPPlayerPropertyModifier multipleM(long _multiple)
    {
        // if (_multiple == 0) {
        //     return this;
        // }
        foreach (NPPlayerPropertyInfoObj obj in propertyObjList)
        {
            obj.value = obj.value * _multiple / 10000;
        }

        return this;
    }
    
    
#if NP_GAME
    /// <summary>
    /// 获取所有属性显示数据
    /// </summary>
    /// <param name="_newModifier">用于比较是否有变化的属性信息</param>
    /// <returns></returns>
    public List<NPPlayerPropertyShowInfo> getPropShowInfoList(NPPlayerPropertyModifier _newModifier = null)
    {
        List<NPPlayerPropertyShowInfo> ret = new List<NPPlayerPropertyShowInfo>();

        NPPlayerPropertyInfoObj propInfo = null;
        NPPlayerPropertyInfoObj propNextInfo = null;
        for (int i = 0; i < propertyObjList.Count; i++)
        {
            propInfo = propertyObjList[i];
            if (null == propInfo)
                continue;
            propNextInfo = (null == _newModifier) ? null : _newModifier.lookup(propertyObjList[i].type);
            if (null == propNextInfo || propNextInfo.value == propInfo.value)
            {
                ret.Add(new NPPlayerPropertyShowInfo(GCommon.getPlayerPropertyName(propInfo.type), _getRealValueString(propInfo.type, propInfo.value)));
            }
            else
            {
                ret.Add(new NPPlayerPropertyShowInfo(GCommon.getPlayerPropertyName(propInfo.type), _getRealValueString(propInfo.type, propInfo.value), _getValueRealDiffStr(propInfo.type, propNextInfo.value - propInfo.value)));
            }
        }

        return ret;
    }
#endif

    /// <summary>
    /// 转换成属性显示。
    /// </summary>
    /// <returns></returns>
    public List<string> toPropStringListWithDiff(NPPlayerPropertyModifier _newModifier)
    {
        if (null == _newModifier)
        {
            return toPropStringList();
        }

        List<string> ret = new List<string>();

        for (int i = 0; i < propertyObjList.Count; i++)
        {
            if (null == propertyObjList[i])
                continue;
            ret.Add(_getValueString(propertyObjList[i], _newModifier.lookup(propertyObjList[i].type)));
        }

        return ret;
    }

    /// <summary>
    /// 转换成属性显示。
    /// </summary>
    /// <returns></returns>
    public string toPropStringWithDiff(NPPlayerPropertyModifier _newModifier)
    {
        StringBuilder ret = new StringBuilder();

        for (int i = 0; i < propertyObjList.Count; i++)
        {
            if (null == propertyObjList[i])
                continue;
            ret.Append(_getValueString(propertyObjList[i], _newModifier.lookup(propertyObjList[i].type)));
            ret.Append(";");
        }

        if (ret.Length > 0)
            ret.Remove(ret.Length - 1, 1);

        return ret.ToString();
    }

    /// <summary>
    /// 转换成属性显示。
    /// </summary>
    /// <returns></returns>
    public List<string> toPropStringList()
    {
        List<string> ret = new List<string>();

        for (int i = 0; i < propertyObjList.Count; i++)
        {
            if (null == propertyObjList[i])
                continue;
            ret.Add(_getValueString(propertyObjList[i]));
        }

        return ret;
    }

    /// <summary>
    /// 转换成属性显示。
    /// </summary>
    /// <returns></returns>
    public string toPropString()
    {
        StringBuilder ret = new StringBuilder();

        for (int i = 0; i < propertyObjList.Count; i++)
        {
            if (null == propertyObjList[i])
                continue;
            ret.Append(_getValueString(propertyObjList[i]));
            ret.Append(";");
        }

        if (ret.Length > 0)
            ret.Remove(ret.Length - 1, 1);

        return ret.ToString();
    }

    private string _getRealValueString(ENPPlayerPropertyType _type, long _value)
    {
        string typeStr = _type.ToString();
        float showValue = _value;
        string str = null;
        //部分万分比特殊处理
        if (GCommon.IsPer(typeStr)) //	+{0}%
        {
            str = TextTranslate.instance.getLanguage(TransKeyConst.common_addPropPer_num, showValue.TODecimalString(100,2));
        }
        else //+{0}
        {
            str = TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, showValue.TODecimalString());
        }

        return str;
    }

    private string _getValueRealDiffStr(ENPPlayerPropertyType _type, long _value)
    {
        string typeStr = _type.ToString();
        float showValue = _value;
        string str = null;

        // #1_common_propAddPer_num	(+ {0}%)
        // #1_common_propAdd_num	(+ {0})
        // #1_common_propReducedPer_num	(- {0}%)
        // #1_common_propReduced_num	(- {0})
        //部分万分比特殊处理
        if (GCommon.IsPer(typeStr))  //	+{0}%
        {
            str = TextTranslate.instance.getLanguage(TransKeyConst.common_propAddPer_num, showValue.TODecimalString(100));
        }
        else //+{0}
        {
            str = TextTranslate.instance.getLanguage(TransKeyConst.common_propAdd_num, showValue.TODecimalString());
        }

        return str;
    }

    //获得加成属性文本
    private string _getValueString(NPPlayerPropertyInfoObj _info)
    {
        if (_info == null)
            return null;

        string str = TextTranslate.instance.getLanguage("#1_player_prop_" + _info.type.ToString());
        // str += " + ";

        float showValue = _info.value;

        // #1_common_propAddPer_num	(+ {0}%)
        // #1_common_propAdd_num	(+ {0})
        // #1_common_propReducedPer_num	(- {0}%)
        // #1_common_propReduced_num	(- {0})
        // #1_common_addPropPer_num	+{0}%
        // #1_common_addProp_num	+{0}
        // #1_common_reducedPropPer_num	- {0}%
        // #1_common_reducedProp_num	- {0}

        string typeStr = _info.type.ToString();
        //部分万分比特殊处理
        if (GCommon.IsPer(typeStr)) //	+{0}%
        {
            str += TextTranslate.instance.getLanguage(TransKeyConst.common_addPropPer_num, showValue.TODecimalString(100));
        }
        else //+{0}
        {
            str += TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, showValue.TODecimalString());
        }


        return str;
    }


    //获得加成属性文本,带差异值的   如：攻击+100（+10）
    private string _getValueString(NPPlayerPropertyInfoObj _info, NPPlayerPropertyInfoObj _newInfo)
    {
        if (_info == null)
            return null;

        string str = TextTranslate.instance.getLanguage("#1_player_prop_" + _info.type.ToString());
        // str += " + ";

        float showValue = _info.value;
        float diffValue = null == _newInfo ? 0 : _newInfo.value - _info.value;

        // #1_common_propAddPer_num	(+ {0}%)
        // #1_common_propAdd_num	(+ {0})
        // #1_common_propReducedPer_num	(- {0}%)
        // #1_common_propReduced_num	(- {0})
        // #1_common_addPropPer_num	+{0}%
        // #1_common_addProp_num	+{0}
        // #1_common_reducedPropPer_num	- {0}%
        // #1_common_reducedProp_num	- {0}

        string typeStr = _info.type.ToString();
        //部分万分比特殊处理
        if (GCommon.IsPer(typeStr)) // +{0}%
        {
            str += TextTranslate.instance.getLanguage(TransKeyConst.common_addPropPer_num, showValue.TODecimalString(100));
            // str += showValue.ToString("f0");
            if (diffValue > 0)
                str += TextTranslate.instance.getLanguage(TransKeyConst.common_propAddPer_num, diffValue.TODecimalString(100));
        }
        else
        {
            // +{0}
            str += showValue.ToString("f0");
            if (diffValue > 0)
                str += TextTranslate.instance.getLanguage(TransKeyConst.common_propAdd_num, diffValue.TODecimalString());
        }


        return str;
    }
}