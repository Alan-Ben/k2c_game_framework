package NPCommon.Util;

import NPCommon.Enum.NPCommonEnum.ENPMissionDungeonType;
import NPCommon.Log.CommLog;
import NPCommon.RefData.AbstractRefDataMgr;
import NPEnum.EQuality;
import WCGCommon.Enum.NPEnum.*;

import java.util.ArrayList;
import java.util.List;

/************
 * 资源读取通用函数
 **/
public class WCGResCommon
{
    public static final int WCG_C_RELATION_ALL = (1 << (int) ENPRelationType.SELF.ordinal()) | (1 << (int) ENPRelationType.ALLY.ordinal()) | (1 << (int) ENPRelationType.FRIEND.ordinal()) | (1 << (int) ENPRelationType.ENEMY.ordinal());
    public static final int WCG_C_RELATION_SELF = 1 << (int) ENPRelationType.SELF.ordinal();
    public static final int WCG_C_RELATION_ALLY = 1 << (int) ENPRelationType.ALLY.ordinal();
    public static final int WCG_C_RELATION_FRIEND = 1 << (int) ENPRelationType.FRIEND.ordinal();
    public static final int WCG_C_RELATION_ENEMY = 1 << (int) ENPRelationType.ENEMY.ordinal();
    public static final int WCG_C_RELATION_SELF_ALLY = (1 << (int) ENPRelationType.SELF.ordinal()) | (1 << (int) ENPRelationType.ALLY.ordinal());
    public static final int WCG_C_RELATION_ALLY_FRIEND = (1 << (int) ENPRelationType.ALLY.ordinal()) | (1 << (int) ENPRelationType.FRIEND.ordinal());
    public static final int WCG_C_RELATION_SELF_ALLY_FRIEND = (1 << (int) ENPRelationType.SELF.ordinal()) | (1 << (int) ENPRelationType.ALLY.ordinal()) | (1 << (int) ENPRelationType.FRIEND.ordinal());

    public static final int WCG_C_MOVE_TYPE_ALL = (1 << (int) EWCGMoveType.GROUND.ordinal()) | (1 << (int) EWCGMoveType.FLY.ordinal());

    public static int WCG_C_LAYER_UNIT = EWCGLayer.GAME_UNIT.ordinal();
    public static int WCG_C_LAYER_UNIT_SHADOW = EWCGLayer.GAME_UNIT_SHADOW.ordinal();
    public static final int WCG_C_RELATION_ENEMY_FRIEND = (1 << (int) ENPRelationType.ENEMY.ordinal() | 1 << (int) ENPRelationType.FRIEND.ordinal());
    public static int WCG_C_LAYER_IGNORE = EWCGLayer.GAME_IGNORE_LAYER.ordinal();
    public static int WCG_C_LAYER_TRIGGER = EWCGLayer.GO_TRIGGER.ordinal();

    /****************
     * 读取二进制位的关系条件
     **/
    public static int readRelationBitValue(String _value)
    {
        if (_value == null || _value.isEmpty())
            return WCG_C_RELATION_ALL;

        int iRValue = 0;
        String[] relationStrs = CommonFunc.charSplit(_value, new char[]{'$', ';'}, true);
        for (int i = 0; i < relationStrs.length; i++)
        {
            ENPRelationType relationType = ENPRelationType.valueOf(relationStrs[i].toUpperCase().trim());
            iRValue |= 1 << (int) relationType.ordinal();
        }

        return iRValue;
    }

    /****************
     * 读取二进制位的品质信息
     **/
    public static int readDayOfWeekBitValue(String _value)
    {
        if (_value == null || _value.isEmpty())
            return 0;

        int value = 0;
        String[] dayOfWeekStrs = CommonFunc.charSplit(_value, new char[]{'$', ';'}, true);
        for (int i = 0; i < dayOfWeekStrs.length; i++)
        {
            DayOfWeek dayOfWeek = DayOfWeek.valueOf(dayOfWeekStrs[i].toUpperCase().trim());
            value |= 1 << (int) dayOfWeek.ordinal();
        }

        return value;
    }

    /****************
     * 读取二进制位的品质信息
     **/
    public static int readQualityBitValue(String _value)
    {
        if (_value == null || _value.isEmpty())
            return 0;

        int value = 0;
        String[] relationStrs = CommonFunc.charSplit(_value, new char[]{'$', ';'}, true);
        for (int i = 0; i < relationStrs.length; i++)
        {
            EQuality relationType = EQuality.valueOf(relationStrs[i].toUpperCase().trim());
            value |= 1 << (int) relationType.ordinal();
        }

        return value;
    }

    /****************
     * 读取二进制位的窗口类型条件
     **/
    public static long readWndTypeBitValue(String _value)
    {
        if (null == _value || _value.isEmpty())
            return 0;

        long value = 0;
//        string[] relationStrs = _value.Split(new char[] { '$', ';' }, StringSplitOptions.RemoveEmptyEntries);
//        for (int i = 0; i < relationStrs.Length; i++)
//        {
//            EWCGActorUnitType wndType = (EWCGActorUnitType)Enum.Parse(typeof(EWCGActorUnitType), relationStrs[i], true);
//            value |= 1L << (int)wndType;
//        }

        return value;
    }

    /****************
     * 读取二进制位的移动条件
     **/
    public static int readMoveTypeBitValue(String _value)
    {
        if (_value == null || _value.isEmpty())
            return WCG_C_MOVE_TYPE_ALL;

        int iRValue = 0;
        String[] moveTypeStrs = CommonFunc.charSplit(_value, '$');
        for (int i = 0; i < moveTypeStrs.length; i++)
        {
            EWCGMoveType moveTypeType = EWCGMoveType.valueOf(moveTypeStrs[i].toUpperCase().trim());

            iRValue |= 1 << (int) moveTypeType.ordinal();
        }

        return iRValue;
    }

    public static int readMoveTypeBitValue(String[] _valueArr, int _startIdx)
    {
        if (_valueArr.length <= _startIdx)
            return WCG_C_MOVE_TYPE_ALL;

        int iRValue = 0;
        for (int i = _startIdx; i < _valueArr.length; i++)
        {
            EWCGMoveType moveType = EWCGMoveType.valueOf(_valueArr[i].toUpperCase().trim());
            iRValue |= 1 << (int) moveType.ordinal();
        }
        return iRValue;
    }

    public static int readShowActorUnitTypeValue(List<EWCGActorUnitType> _valueList)
    {
        if (_valueList == null)
            return 0;

        int value = 0;

        for (int i = 0; i < _valueList.size(); i++)
            value |= 1 << (int) _valueList.get(i).ordinal();

        return value;
    }

    public static int readActorUnitTypeValue(String _value)
    {
        if (_value == null || _value.isEmpty())
            return 0;

        int value = 0;
        String[] relationStrs = CommonFunc.charSplit(_value, new char[]{'$', ';'}, true);

        for (int i = 0; i < relationStrs.length; i++)
        {
            EWCGActorUnitType wndType = EWCGActorUnitType.valueOf(relationStrs[i].toUpperCase().trim());
            value |= 1 << (int) wndType.ordinal();
        }

        return value;
    }

    public static int readActorTypeValue(String _value)
    {
        if (_value == null || _value.isEmpty())
            return 0;

        int value = 0;
        String[] relationStrs = CommonFunc.charSplit(_value, new char[]{'$', ';'}, true);

        for (int i = 0; i < relationStrs.length; i++)
        {
            EWCGActorType wndType = EWCGActorType.valueOf(relationStrs[i].trim().toUpperCase());
            value |= 1 << (int) wndType.ordinal();
        }

        return value;
    }

    /****************
     * 读取二进制位的移动条件
     **/
    public static long readControlType(String _value)
    {
        long value = 0;

        if (_value == null || _value.isEmpty())
            return value;

        String[] moveTypeStrs = CommonFunc.charSplit(_value, new char[]{'$'}, true);
        for (int i = 0; i < moveTypeStrs.length; i++)
        {
            EWCGControlState moveTypeType = EWCGControlState.valueOf(moveTypeStrs[i].trim().toUpperCase());
            value |= 1L << (int) moveTypeType.ordinal();
        }

        return value;
    }

    /****************
     * 读取二进制位的角色标记
     **/
    public static byte readMissionDungeonTypeValue(String _value)
    {
        byte value = 0;

        if (_value == null || _value.isEmpty())
            return value;

        String[] missionDungeonTypeStrs = CommonFunc.charSplit(_value, new char[]{'$'}, true);
        int tempValue = 0;
        for (int i = 0; i < missionDungeonTypeStrs.length; i++)
        {
            ENPMissionDungeonType missionDungeonType = ENPMissionDungeonType.valueOf(missionDungeonTypeStrs[i].trim().toUpperCase());
            tempValue |= (byte) 1 << (int) missionDungeonType.ordinal();
        }
        value = (byte) tempValue;
        return value;
    }

    @SuppressWarnings("unchecked")
    public static <T> ArrayList<T> parseList(Class<?> clazz, String _value)
    {
        ArrayList<T> list = new ArrayList<T>();
        if (null != _value && !_value.equals(""))
        {
            String[] valueList = _value.split(";");
            int length = valueList.length;
            for (int index = 0; index < length; index++)
            {
                try
                {
                    Object obj = AbstractRefDataMgr.parseCreateObj(clazz, valueList[index], "", clazz.getSimpleName());
                    list.add((T) obj);
                } catch (Exception e)
                {
                    CommLog.error("failed to parse object:", e);
                }

            }
        }
        return list;
    }
}
