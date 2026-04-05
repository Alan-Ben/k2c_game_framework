package NPGameRes.GmCmds;

import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPCommon.GMCommand.CmdClassBase;
import NPCommon.Log.CommLog;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.Util.RunResult;
import NPGameRes.NPRefDataMgr;
import NPGameRes.Refs.RefGeneral;
import NPGameRes.ServerVersionInfo;

import java.lang.reflect.Modifier;

@ACommander(comment = "配置表相关命令", name = "ref")
public class CmdRef extends CmdClassBase
{
    @ACommand(comment = "设置 配置表的配置(表名,id,字段名,value)")
    public RunResult set(final String _tableName, String _id, final String fieldName, String value)
    {
        RefContainerBase<?> refMgr = NPRefDataMgr.getInstance().lookupRefMgrByTableName(_tableName);
        if (null == refMgr)
            return RunResult.failed("can not find refmgr for table:" + _tableName);

        long keyV = Long.parseLong(_id);
        RefBase ref = refMgr.get(keyV);
        if (null == ref)
            return RunResult.failed("can not find ref for id:" + _id);

        boolean bOK = ref.gmSetValue(fieldName, value);
        if (!bOK)
        {
            return RunResult.failed("set failed");
        }
        ref.Assert();

        return RunResult.succ("ok");
    }

    @ACommand(comment = "获取 ref配置表的配置(类名,id,key)")
    public RunResult get(String _tableName, String _id, final String key)
    {
        RefContainerBase<?> refMgr = NPRefDataMgr.getInstance().lookupRefMgrByTableName(_tableName);

        if (null == refMgr)
            return RunResult.failed("can not find refmgr for table:" + _tableName);

        long keyV = Long.parseLong(_id);
        RefBase ref = refMgr.get(keyV);
        if (null == ref)
            return RunResult.failed("can not find ref for id:" + _id);

        return ref.gmGetValue(key);
    }

    @ACommand(comment = "获取 ref配置表的配置(类名,id)")
    public RunResult showLine(String _tableName, String _id)
    {
        RefContainerBase<?> refMgr = NPRefDataMgr.getInstance().lookupRefMgrByTableName(_tableName);

        if (null == refMgr)
            return RunResult.failed("can not find refmgr for table:" + _tableName);

        long keyV = Long.parseLong(_id);
        RefBase ref = refMgr.get(keyV);
        if (null == ref)
            return RunResult.failed("can not find ref for id:" + _id);
        Class<? extends RefBase> clazz = ref.getClass();
        StringBuilder sb = new StringBuilder();
        sb.append(String.format("line[%s] of table:%s\n", _id, _tableName));
        for (java.lang.reflect.Field field : clazz.getDeclaredFields())
        {
            int modifiers = field.getModifiers();
            if (Modifier.isStatic(modifiers) || !Modifier.isPublic(modifiers))
            {
                continue;
            }
            RunResult result = ref.gmGetValue(field.getName());
            if (result.isSucc())
            {
                sb.append(String.format("[%s] = %s\n", field.getName(), result.getMsg()));
            } else
            {
                sb.append(String.format("[%s] = got failed:%s\n", field.getName(), result.getMsg()));
            }
        }
        return RunResult.succ(sb.toString());
    }

    @ACommand(comment = "重新加载")
    public RunResult reload(String _reloadTag)
    {
        //标记为空，为了预防错误处理，返回错误
        if(_reloadTag.isEmpty())
        {
            return RunResult.failed("reload tag can not be empty!");
        }

        //读取预加载配表版本
        String readyRefVersion = ServerVersionInfo.getInstance().getReadyRefVersion();
        if (readyRefVersion == null)
        {
            CommLog.error("can not load ready res version!");
            return RunResult.failed("can not load game res version!");
        }

        //读取配表文件进入内存
        boolean readyResult = NPRefDataMgr.getInstance().getRefDataReloader().readyAllFiles(readyRefVersion);
        if (!readyResult)
        {
            return RunResult.failed("ref reload readyAllFiles fail");
        }

        //应用预加载配置
        boolean reloadResult = NPRefDataMgr.getInstance().getRefDataReloader().dealAllProcess(_reloadTag);
        if (!reloadResult)
        {
            return RunResult.failed("ref reload dealAllProcess fail");
        }

        //设置服务器配置版本为预加载版本
        ServerVersionInfo.getInstance().setSucLoadRefVersion(readyRefVersion);

        return RunResult.succ("OK");
    }

    @ACommand(comment = "准备热更新数据")
    public RunResult reloadReady()
    {
        //读取预加载配表版本
        String readyRefVersion = ServerVersionInfo.getInstance().getReadyRefVersion();
        if (readyRefVersion == null)
        {
            CommLog.fatal("can not load ready res version!");
            return RunResult.failed("can not load game res version!");
        }

        //读取配表文件进入内存
        NPRefDataMgr.getInstance().getRefDataReloader().syncRefReloadThreadDealReadyAllFiles(readyRefVersion, _isSucc ->
        {
            if (_isSucc)
            {
                CommLog.info("RefVersion: " + ServerVersionInfo.getInstance().getVersion() +
                        " ReadyVersion: " + NPRefDataMgr.getInstance().getRefDataReloader().getVersion());
            } else
            {
                CommLog.error("reloadReady deal fail");
            }
        });

        return RunResult.succ("reloadReady start! readyRefVersion:" + readyRefVersion);
    }

    @ACommand(comment = "处理热更新数据")
    public RunResult reloadDeal(String _reloadTag)
    {
        //标记为空，为了预防错误处理，返回错误
        if(_reloadTag.isEmpty())
        {
            return RunResult.failed("reload tag can not be empty!");
        }

        //预加载配表版本
        String readyRefVersion = NPRefDataMgr.getInstance().getRefDataReloader().getVersion();

        //应用预加载配置
        boolean reloadResult = NPRefDataMgr.getInstance().getRefDataReloader().dealAllProcess(_reloadTag);
        if (!reloadResult)
        {
            return RunResult.failed("ref reload dealAllProcess fail");
        }

        //设置服务器配置版本为预加载版本
        ServerVersionInfo.getInstance().setSucLoadRefVersion(readyRefVersion);

        return RunResult.succ("reloadDeal over ReadyVersion: " + readyRefVersion);
    }

    @ACommand(comment = "设置 general表配置(字段名,value)")
    public RunResult setGeneral(final String fieldName, String value)
    {
        boolean bOK = RefGeneral.Ref().gmSetValue(fieldName, value);
        if (!bOK)
        {
            RunResult.failed(" set general field: " + fieldName + " failed.");
        }
        RefGeneral.Ref().Assert();
        return RunResult.succ("ok");
    }

    @ACommand(comment = "获取 general表的配置(字段名)")
    public RunResult getGeneral(final String key)
    {
        RunResult result = RefGeneral.Ref().gmGetValue(key);
        if (result.isSucc())
        {
            return RunResult.succ(String.format("[%s] = %s", key, result.getMsg()));
        } else
            return RunResult.failed(String.format("[%s] = got failed:%s", key, result.getMsg()));

    }

}
