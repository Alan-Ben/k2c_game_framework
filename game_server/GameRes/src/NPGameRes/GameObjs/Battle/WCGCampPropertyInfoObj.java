package NPGameRes.GameObjs.Battle;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import WCGCommon.Enum.NPEnum.EWCGCampPropertyType;

/****************
 * 记录属性信息的具体对象
 **/
public class WCGCampPropertyInfoObj
{
    /**
     * 属性类型
     */
    public EWCGCampPropertyType type;
    /**
     * 属性具体值
     */
    public long value;

    /******************
     * 从带入的字符串内读取属性加成信息
     *
     * @author alzq.z
     * @time Aug 27, 2013 10:57:11 PM
     */
    public static WCGCampPropertyInfoObj readPropertyInfoObj(String _str, String _fieldName)
    {
        if (null == _str || _str.isEmpty())
        {
            CommLog.error(String.format("read excel field(%s) info err!, str is null or empty!", _fieldName));
            return null;
        }

        WCGCampPropertyInfoObj obj = new WCGCampPropertyInfoObj();

        String[] strs = CommonFunc.charSplit(_str, ':');
        if (strs.length < 2)
        {
            CommLog.error("read property info err3! : " + _str);
            return null;
        }

        obj.type = EWCGCampPropertyType.valueOf(strs[0].toUpperCase().trim());
        obj.value = Long.parseLong(strs[1].trim());

        return obj;
    }
}
