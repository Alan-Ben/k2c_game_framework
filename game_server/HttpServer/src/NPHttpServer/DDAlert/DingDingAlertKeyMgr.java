package NPHttpServer.DDAlert;


import Common.NpServerObj.NpServerObj_DDRobot;
import NPEnum.ENPDDAlertType;

import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;

public class DingDingAlertKeyMgr
{
    private static DingDingAlertKeyMgr _instance = new DingDingAlertKeyMgr();

    public static DingDingAlertKeyMgr getInstance()
    {
        return _instance;
    }

    private Map<ENPDDAlertType, NpServerObj_DDRobot> _m_robotMap = new ConcurrentHashMap<>();

    public DingDingAlertKeyMgr()
    {
        addRobot(ENPDDAlertType.COMMON
                , "9c2e5efc1879d9b85a4492396449de13b5b820f211ec4048ba587b583aa1fe01"
                , "SECd38ab58acb1112e4e8c9f2380922ae59f21eec08d6f0f3535e6cb38cdec42279");

        addRobot(ENPDDAlertType.MEMORY
                , "9c2e5efc1879d9b85a4492396449de13b5b820f211ec4048ba587b583aa1fe01"
                , "SECd38ab58acb1112e4e8c9f2380922ae59f21eec08d6f0f3535e6cb38cdec42279");
    }

    public void addRobot(ENPDDAlertType _type, String _token, String _secret)
    {
        NpServerObj_DDRobot robot = new NpServerObj_DDRobot(_type, _token, _secret);
        _m_robotMap.put(robot.getAlertType(), robot);
    }

    public NpServerObj_DDRobot lookupRobot(ENPDDAlertType _robotType)
    {
        return _m_robotMap.get(_robotType);
    }

    public NpServerObj_DDRobot remove(ENPDDAlertType _type)
    {
        return _m_robotMap.remove(_type);
    }
}
