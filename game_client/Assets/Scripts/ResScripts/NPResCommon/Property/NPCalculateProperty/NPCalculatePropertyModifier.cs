// using System.Collections.Generic;
// using System.Text;
// using ALPackage;
// using NPEnum;
//
// namespace GOE
// {
//     /// <summary>
//     /// 辅助计算枚举Modifier
//     /// </summary>
//     [System.Serializable]
//     public class NPCalculatePropertyModifier : _ATNPBasicPropertyModifier<ENPPropertyCalculateType, NPCalculatePropertyModifier>
//     {
//         //创建一个编辑器对象
//         protected override NPCalculatePropertyModifier _createModifier()
//         {
//             return new NPCalculatePropertyModifier();
//         }
//        
//         /// <summary>
//         /// 全局的读取函数
//         /// </summary>
//         /// <param name="_str"></param>
//         /// <param name="_fieldName"></param>
//         /// <returns></returns>
//         public static NPCalculatePropertyModifier readPropertyModifier(string _str, string _fieldName)
//         {
//             NPCalculatePropertyModifier modifier = new NPCalculatePropertyModifier();
//         
//             //读取
//             modifier.readStr(_str, _fieldName);
//         
//             if(modifier.isEmpty())
//                 return null;
//         
//             return modifier;
//         }
//         
//         public override string ToString()
//         {
//             StringBuilder ret = new StringBuilder();
//
//             for (int i = 0; i < propertyObjList.Count; i++)
//             {
//                 ret.Append(_getValueString(propertyObjList[i]));
//                 ret.Append(";");
//             }
//
//             if (ret.Length > 0)
//                 ret.Remove(ret.Length - 1, 1);
//
//             return ret.ToString();
//         }
//         
//         //转成字符串，带变化值
//         public string ToStringWithDiff(NPCalculatePropertyModifier _diffModifier)
//         {
//             if (null == _diffModifier)
//                 return ToString();
//             StringBuilder ret = new StringBuilder();
//             _TNPBasicPropertyInfoObj<ENPPropertyCalculateType> diffObj = null;
//             for (int i = 0; i < propertyObjList.Count; i++)
//             {
//                 ret.Append(_getValueString(propertyObjList[i]));
//                 diffObj = _diffModifier.lookup(propertyObjList[i].type);
//                 if (null != diffObj)
//                 {
//                     // #1_common_propAddPer_num	(+ {0}%)
//                     // #1_common_propAdd_num	(+ {0})
//                     // #1_common_propReducedPer_num	(- {0}%)
//                     // #1_common_propReduced_num	(- {0})
//                     string typeStr = diffObj.type.ToString();
//                     if (GCommon.IsPer(typeStr)) 
//                     {
//                         ret.Append(TextTranslate.instance.getLanguage(TransKeyConst.common_propAddPer_num,_getValueRealStr(diffObj)));
//                     }
//                     else
//                     {
//                         ret.Append(TextTranslate.instance.getLanguage(TransKeyConst.common_propAdd_num,_getValueRealStr(diffObj)));
//                     }
//                 }
//             
//                 ret.Append(";");
//             }
//
//             if (ret.Length > 0)
//                 ret.Remove(ret.Length - 1, 1);
//
//             return ret.ToString();
//         }
//         
//         /// <summary>
//         /// 计算传参与自身属性加成的不同部分
//         /// </summary>
//         /// <param name="_iptmodifier"></param>
//         /// <returns></returns>
//         public NPCalculatePropertyModifier calNew(NPCalculatePropertyModifier _iptmodifier)
//         {
//             NPCalculatePropertyModifier ret = new NPCalculatePropertyModifier();
//
//             if (null == _iptmodifier)
//                 return ret;
//         
//             //获取枚举长度，通过遍历枚举取出自身和参数直接同类型reader
//             int enumCount = ALCommon.getEnumCount(typeof(ENPPropertyCalculateType));
//         
//             for (int i = 0; i < enumCount; i++)
//             {
//                 ENPPropertyCalculateType type = (ENPPropertyCalculateType) i;
//             
//                 _TNPBasicPropertyInfoObj<ENPPropertyCalculateType>  selfObj = lookup(type);
//                 //自身值
//                 long selfVal = selfObj == null ? 0 : selfObj.value;
//
//                 //获取传参中同类型数据
//                 _TNPBasicPropertyInfoObj<ENPPropertyCalculateType>  iptObj = _iptmodifier.lookup(type);
//                 long iptVal = iptObj == null ? 0 : iptObj.value;
//
//                 ret.addProperty(type, iptVal);
//             
//             }
//
//             return ret;
//         }
//         
//         /// <summary>
//         /// 计算传参与自身属性加成的不同部分
//         /// </summary>
//         /// <param name="_iptmodifier"></param>
//         /// <returns></returns>
//         public NPCalculatePropertyModifier calDiff(NPCalculatePropertyModifier _iptmodifier)
//         {
//             NPCalculatePropertyModifier ret = new NPCalculatePropertyModifier();
//
//             if (null == _iptmodifier)
//                 return ret;
//         
//             //获取枚举长度，通过遍历枚举取出自身和参数直接同类型reader
//             int enumCount = ALCommon.getEnumCount(typeof(ENPPropertyCalculateType));
//         
//             for (int i = 0; i < enumCount; i++)
//             {
//                 ENPPropertyCalculateType type = (ENPPropertyCalculateType) i;
//             
//                 _TNPBasicPropertyInfoObj<ENPPropertyCalculateType> selfObj = lookup(type);
//                 //自身值
//                 long selfVal = selfObj == null ? 0 : selfObj.value;
//             
//             
//                 //获取传参中同类型数据
//                 _TNPBasicPropertyInfoObj<ENPPropertyCalculateType> iptObj = _iptmodifier.lookup(type);
//                 long iptVal = iptObj == null ? 0 : iptObj.value;
//
//                 ret.addProperty(type, iptVal - selfVal);
//             
//             }
//
//             return ret;
//         }
//         
//         /// <summary>
//         /// 获取所有属性显示数据
//         /// </summary>
//         /// <param name="_newModifier">用于比较是否有变化的属性信息</param>
//         /// <returns></returns>
//         public List<NPPlayerPropertyShowInfo> getPropShowInfoList(NPCalculatePropertyModifier _newModifier = null)
//         {
//             List<NPPlayerPropertyShowInfo> ret = new List<NPPlayerPropertyShowInfo>();
//
//             _TNPBasicPropertyInfoObj<ENPPropertyCalculateType> propInfo = null;
//             _TNPBasicPropertyInfoObj<ENPPropertyCalculateType> propNextInfo = null;
//             for (int i = 0; i < propertyObjList.Count; i++)
//             {
//                 propInfo = propertyObjList[i];
//                 if(null == propInfo)
//                     continue;
//                 propNextInfo = (null == _newModifier) ? null : _newModifier.lookup(propertyObjList[i].type);
//                 if (null == propNextInfo || propNextInfo.value == propInfo.value)
//                 {
//                     ret.Add(new NPPlayerPropertyShowInfo(getTypeNameStr(propInfo.type),_getValueRealStr(propInfo)));   
//                 }
//                 else
//                 {
//                     ret.Add(new NPPlayerPropertyShowInfo(getTypeNameStr(propInfo.type),_getValueRealStr(propInfo),_getValueRealDiffStr(propInfo.type,propNextInfo.value - propInfo.value)));
//                 }
//             }
//
//             return ret;
//         }
//         
//         /// <summary>
//         /// 获取属性完整显示XXXX {0}
//         /// </summary>
//         /// <param name="_info"></param>
//         /// <returns></returns>
//         private string _getValueString(_TNPBasicPropertyInfoObj<ENPPropertyCalculateType> _info)
//         {
//             if (_info == null)
//                 return null;
//             string typeStr = _info.type.ToString();
//             string str = TextTranslate.instance.getLanguage("#1_card_calculate_" + typeStr);
//             if (GCommon.IsPer(typeStr)) 
//             {
//                 str += TextTranslate.instance.getLanguage(TransKeyConst.common_addPropPer_num, _getValueRealStr(_info));
//             }
//             else
//             {
//                 str += TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, _getValueRealStr(_info));
//             }
//             return str;
//         }
//         
//         /// <summary>
//         /// 获取显示值 {0}
//         /// </summary>
//         /// <param name="_info"></param>
//         /// <returns></returns>
//         private string _getValueRealStr(_TNPBasicPropertyInfoObj<ENPPropertyCalculateType> _info)
//         {
//             string str = null;
//             //不是百分比又要除100的特殊处理
//             string typeStr = _info.type.ToString();
//             if (GCommon.IsPer(typeStr))
//             {
//                 str = ((float)_info.value).TODecimalString(100);
//             }
//             else
//             {
//                 str = ((float)_info.value).TODecimalString();
//             }
//             
//             if (GCommon.IsPer(typeStr)) 
//             {
//                 str = TextTranslate.instance.getLanguage(TransKeyConst.common_addPropPer_num,str);
//             }
//             else
//             {
//                 str = TextTranslate.instance.getLanguage(TransKeyConst.common_add_num,str);
//             }
//             
//             return str;
//         }
//
//         /// <summary>
//         /// 获取属性名 XXX
//         /// </summary>
//         /// <param name="_type"></param>
//         /// <returns></returns>
//         private string getTypeNameStr(ENPPropertyCalculateType _type)
//         {
//             string str = TextTranslate.instance.getLanguage("#1_card_calculate_" + _type.ToString());
//             return str;
//         }
//         
//         /// <summary>
//         /// 获得增加值 {0}
//         /// </summary>
//         /// <param name="_type"></param>
//         /// <param name="_value"></param>
//         /// <returns></returns>
//         private string _getValueRealDiffStr(ENPPropertyCalculateType _type,long _value)
//         {
//             string str = null;
//             //不是百分比又要除100的特殊处理
//             string typeStr = _type.ToString();
//
//             if (GCommon.IsPer(typeStr))
//             {
//                 str = ((float)_value).TODecimalString(100);
//             }
//             else
//             {
//                 str = ((float)_value).TODecimalString();
//             }
//         
//             if (GCommon.IsPer(typeStr)) 
//             {
//                 str = TextTranslate.instance.getLanguage(TransKeyConst.common_propAddPer_num,str);
//             }
//             else
//             {
//                 str = TextTranslate.instance.getLanguage(TransKeyConst.common_propAdd_num,str);
//             }
//             
//             return str;
//         }
//     }
// }