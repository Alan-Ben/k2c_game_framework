package NPUSServer.NPUSUserMgr.UserComp.MuseumComp;

import ALBasicServer.ALProcess.ALProcess;
import NPCommon.CommonProcess._IEZProcessMonitor;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.MuseumComp.Gift.MuseumItemMgr;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.USLog;

public class MuseumComponent extends _ANPUserComponent
{
    private MuseumItemMgr _m_itemMgr;

    public MuseumComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.MUSEUM);

        _m_itemMgr = new MuseumItemMgr(this);
    }

    public MuseumItemMgr getItemMgr()
    {
        return _m_itemMgr;
    }

    @Override
    protected void _init()
    {
        // 使用ALProcess进行多步骤初始化
        ALProcess process = ALProcess.CreateProcess("inn_component_init");

        // 步骤1: 加载物品数据
        process.addResDelegateProcess(_action -> _m_itemMgr.init(_action::dealAction),
                "init_museum_item", null, false);

        // 执行初始化流程
        process.dealProcess(new _IEZProcessMonitor()
        {
            @Override
            public void onRootProecssSuc()
            {
                setInited();
            }

            @Override
            public void onRootProecssStop()
            {
                USLog.error(getUSServer(), "Player {} inn component init failed", getUserData().getCid());
                getUserData().setDataLoadFail();
            }
        });
    }

    @Override
    public ENPPlayerCompType[] getDependCompList()
    {
        return null;
    }

    @Override
    public void onInited()
    {

    }

    @Override
    public void dispose()
    {

    }
}
