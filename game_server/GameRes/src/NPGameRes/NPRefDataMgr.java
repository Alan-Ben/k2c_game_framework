/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package NPGameRes;

import ALServerLog.ALServerLog;
import NPCommon.RefData.AbstractRefDataMgr;
import NPGameRes.Refs.RefGeneral;
import NPGameRes.UsHotRefDataMgr.ActivityHotRefDataMgr;

import java.io.File;

public class NPRefDataMgr extends AbstractRefDataMgr
{
    private static final String Defaule_RefData_Path = "../data" + File.separatorChar + "refData";

    private static NPRefDataMgr _instance = new NPRefDataMgr();

    public NPRefDataMgr()
    {
        super();
    }

    public static NPRefDataMgr getInstance()
    {
        return _instance;
    }


    private String _refPath = Defaule_RefData_Path;

    @Override
    public String getRefPath()
    {
        return _refPath;
    }

    /***********
     * 获取加载的数据类存放包路径
     *
     * @author alzq.z
     * @time 2019年4月3日 下午10:06:35
     */
    @Override
    public String getLoadRefdataClassPath()
    {
        return "NPGameRes.Refs";
    }

    @Override
    public void reload()
    {
        //重新执行NPCoreMgr的配表init过程
        NPGRefdataCoreMgr.getInstance().reload();

        //执行热更配表的加载
        ActivityHotRefDataMgr.getInstance().reActivateAllHotRefGroups();
    }

    /*************
     * 处理初始化配置路径相关处理
     *
     * @author alzq.z
     * @time 2019年4月3日 下午8:28:45
     * @param _refReloadThreadIdx 指定一个线程idx，用来专门执行配表重载操作
     */
    @Override
    protected void _init(int _refReloadThreadIdx)
    {
        //读取配置文件
        GameResConf.getInstance().init();
        //设置数据源路径
        String sRefPath = GameResConf.getInstance().getRefPath();
        //判断路径合法性
        if (sRefPath.isEmpty())
        {
            ALServerLog.Warning("RefPath not configured ,using the default:" + Defaule_RefData_Path);
            _refPath = Defaule_RefData_Path;
        } else
        {
            _refPath = sRefPath;
        }
        getRefDataReloader().setDealReloadThreadIdx(_refReloadThreadIdx);
    }

    public static RefGeneral getGeneral()
    {
        return RefGeneral.Ref();
    }

}
