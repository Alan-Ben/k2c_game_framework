package NPGameRes.GameObjs.Battle;

import NPCommon.Util.CommonFunc;

import java.util.ArrayList;
import java.util.List;

/************************
 * 属性加成信息对象
 **/
public class NPTeamPropertyModifier
{
    public List<WCGTeamPropertyInfoObj> propertyObjList = new ArrayList<WCGTeamPropertyInfoObj>();

    /******************
     * 从带入的字符串内读取属性加成信息
     *
     * @author alzq.z
     * @time Aug 27, 2013 10:57:11 PM
     */
    public static NPTeamPropertyModifier readPropertyModifier(String _str, String _fieldName)
    {
        if (null == _str || _str.isEmpty())
            return null;
        NPTeamPropertyModifier modifier = new NPTeamPropertyModifier();

        String[] strs = CommonFunc.charSplit(_str, ';');
        for (int i = 0; i < strs.length; i++)
        {
            String itemStr = strs[i];
            //解析属性对象信息
            WCGTeamPropertyInfoObj infoObj = WCGTeamPropertyInfoObj.readPropertyInfoObj(itemStr, _fieldName);
            if (null == infoObj)
                continue;

            //加入数据集
            modifier.propertyObjList.add(infoObj);
        }

        return modifier;
    }
}