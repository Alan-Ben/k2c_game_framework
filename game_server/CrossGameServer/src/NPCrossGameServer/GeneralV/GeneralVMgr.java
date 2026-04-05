package NPCrossGameServer.GeneralV;


import CGSDB.Bo.GeneralVBO;
import NPCommon.Log.CommLog;
import NPCrossGameServer.NPCrossGameServer;

import java.util.List;

public class GeneralVMgr
{
    private static GeneralVMgr _g_instance = new GeneralVMgr();

    public static GeneralVMgr getInstance()
    {
        if (null == _g_instance)
            _g_instance = new GeneralVMgr();
        return _g_instance;
    }

    //值映射存储集合
    private GeneralVObj[] _m_arrObjs;

    private GeneralVMgr()
    {
        EGeneralVType[] arrTypes = EGeneralVType.values();
        _m_arrObjs = new GeneralVObj[arrTypes.length];
        for (EGeneralVType eType : arrTypes)
        {
            _m_arrObjs[eType.ordinal()] = new GeneralVObj(eType);
        }
    }

    public GeneralVObj getVObj(EGeneralVType _eType)
    {
        return _m_arrObjs[_eType.ordinal()];
    }


    /****************
     * 初始化相关数据
     */
    public boolean s_init()
    {
        //初始化全局变量
        List<GeneralVBO> boList = NPCrossGameServer.getInstance().getBM().getBM(GeneralVBO.class).s_findAll();
        if (null == boList)
        {
            return false;
        }
        for (GeneralVBO generalVBO : boList)
        {
            int type = generalVBO.getType();
            if (type < 0 || type >= EGeneralVType.values().length)
            {
                CommLog.error("GeneralVBO invalid type:{}", type);
                continue;
            }
            EGeneralVType eType = EGeneralVType.values()[type];
            getVObj(eType).s_initBo(generalVBO);
        }
        for (GeneralVObj vObj : _m_arrObjs)
        {
            vObj.s_ensureBo();
        }
        return true;
    }

    public long makeNewId(EGeneralVType _eType)
    {
        GeneralVObj vObj = getVObj(_eType);
        return vObj.makeNewId();
    }
}
