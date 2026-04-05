package NPCommon.GMCommand.DefualtCmds;


import NPCommon.ErrMain.Result.Result;
import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPCommon.GMCommand.CmdClassBase;
import NPCommon.HotLoad.HotLoadHelper;
import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.RunResult;
import SevenZip.Zipper;

import java.util.Base64;

@ACommander(comment = "热加载相关命令", name = "hot")
public class CmdHot extends CmdClassBase
{
    @ACommand(comment = "加载类(类必须上传于../hot/目录下)")
    public RunResult load(String _classes)
    {
        String[] classes = CommonFunc.charSplit(_classes, ';');
        if (classes.length <= 0)
        {
            return RunResult.failed("need class name,example: WCGCommon.Util.WCGCommonFunc");
        }
        try
        {
            Result result = HotLoadHelper.hotLoadClass("../hot/", classes);

            //输出热更结果
            CommLog.info(result.getMsg());
            if(result.isSucc())
                return RunResult.succ(result.getMsg());
            else
                return RunResult.failed(result.getMsg());
        } catch (Exception e)
        {
            CommLog.error("run hot load:{} failed", e);

            return RunResult.failed("failed with exception:" + e);
        }
    }

    @ACommand(comment = "从二进制class bytes中加载类(类名AA.BB.CC,Base64编码后的class")
    public String loadClassFromBytes(String _className,String _encodeClass)
    {
        if (null == _className || _className.isEmpty())
        {
            return "invalid class name";
        }
        byte[] decode = Base64.getDecoder().decode(_encodeClass);
        if(decode ==null || decode.length<=0)
        {
            return "decode from base64 failed";
        }
        try
        {
            byte[] unzipBytes = Zipper.unzip(decode);
            Result result = HotLoadHelper.loadOrRedefineClass(unzipBytes, _className);
            if (result.isSucc())
            {
                return "ok!"+result.getMsg();
            }
            else
                return "failed!" + result.getMsg();
        }
        catch(Exception e)
        {
            CommLog.error("unzip error:",e);
            return e.getMessage();
        }

    }
}
