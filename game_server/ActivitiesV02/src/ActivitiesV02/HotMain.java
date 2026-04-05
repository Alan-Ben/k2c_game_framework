package ActivitiesV02;

import ActivitiesV02.Activities.RegularActivity.NumMergeActivity;
import ActivitiesV02.Activities.RegularActivity.NumMergeActivityInitializer;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CallBack._ICallBackResult;
import NPUSServer.CommonActivityMgr.Factory.CommonActivityFactory;
import NPUSServer.HotActivity.Annotation.HotActivityJar;
import NPUSServer.HotActivity._AHotActivityMain;

@HotActivityJar(comment = "热更Jar包说明")
public class HotMain extends _AHotActivityMain
{
    public static HotMain getInstance() { return _g_instance;}
    private static HotMain _g_instance = new HotMain();

    public HotMain()
    {
        registActivity(new NumMergeActivityInitializer());
    }

    public static Result runSync()
    {
        return _g_instance.initSync();
    }

    public static void runAsync(_ICallBackResult _handler)
    {
    	_g_instance.initAsync(_handler);
    }

    @Override
    protected void registActivityFactory()
    {
        CommonActivityFactory.getInstance().regCreator(NumMergeActivity.s_typeId, NumMergeActivity::new);
    }

    @Override
    protected String getRefPackPath() { return this.getClass().getPackage().getName() + ".Refs";}

    @Override
    protected String getGMCommandPackPath() {return this.getClass().getPackage().getName() + ".Cmds";}

    @Override
    protected String getEventPackPath() {return this.getClass().getPackage().getName() + ".Events";}

    @Override
    protected String getBoPackPath() { return this.getClass().getPackage().getName() + ".Bo";}

    @Override
    protected String getLogBoPackPath() { return this.getClass().getPackage().getName() + ".LogBo";}

    @Override
    protected String getOptLogBoPackPath() { return this.getClass().getPackage().getName() + ".OptLogBo";}

    @Override
    protected String getMsgDealerPackPath() { return this.getClass().getPackage().getName() + ".MsgDealers";}

}
