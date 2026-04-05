package NPUSServer.GMCommand.Cmds;

import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPCommon.GMCommand.GmCommandMgr;
import NPCommon.Log.CommLog;
import NPUSServer.GMCommand.UsCmdBase;
import com.google.gson.JsonArray;
import com.google.gson.JsonObject;
import com.google.gson.JsonParser;

@ACommander(comment = "玩家属性命令", name = "base64")
public class CmdBase64 extends UsCmdBase
{
    @ACommand(comment = "处理命令")
    public void deal(String _str)
    {
        //先对字符串进行base64解码
        String command = new String(java.util.Base64.getDecoder().decode(_str));

        //解析command
        JsonObject commandObject = new JsonParser().parse(command).getAsJsonObject();
        String commandType;
        String functionType;
        JsonArray params;

        try
        {
            //获取command的类型
            commandType = commandObject.get("cmdType").getAsString().trim();;
            //获取功能类型
            functionType = commandObject.get("funcType").getAsString().trim();;
            //获取参数列表
            params = commandObject.get("params").getAsJsonArray();
        } catch (Exception e)
        {
            CommLog.error("CmdBase64 json parse fail, commandStr:{}", _str, e);
            takeCallBack().onRunOver(false, "command json parse fail");
            return;
        }

        //转化为string数组
        String[] args = new String[params.size()];
        for (int i = 0; i < params.size(); i++)
        {
            args[i] = params.get(i).getAsString().trim();
        }

        //执行命令
        GmCommandMgr.getInstance().runSpecific(getExecutor(), commandType, functionType, args, getContext(),
                (_bSucc, _msg) -> takeCallBack().onRunOver(_bSucc, _msg));
    }

}
