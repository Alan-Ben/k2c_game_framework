package NPGameRes.GameObjs.Battle;

import WCGCommon.Enum.NPEnum.ENPTeamPropertyType;


public class WCGSOTeamPropertyRefObj extends _IALBasicRefObj
{
    public long _refId()
    {
        return (long) team_property_type.ordinal();
    }

    public ENPTeamPropertyType team_property_type;//类型
    public String name;//名字
    public String icon;//图标;
    public String desc;//描述
}