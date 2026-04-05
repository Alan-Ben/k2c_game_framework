package NPUSServer.GMCommand.Cmds;

import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPGameRes.GameObjs.PlayerCondition.NPPlayerConditionGroupObj;
import NPGameRes.GameObjs.PlayerVariable.NPPlayerVariableGroupObj;
import NPGameRes.Refs.Parse.NPPlayerEffectListParse;
import NPGameRes.Refs.RefRemoteEffect;
import NPUSServer.GMCommand.UsCmdBase;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.EffectDealer.NPPlayerEffectDealer;
import NPUSServer.NPUSUserMgr.VariableDealer.NPPlayerVariableDeal;

/**
 * 关卡测试命令，看懂再改
 * @author Valen
 * @date 2016年7月8日
 */
@ACommander(comment = "执行效果", name = "exec")
public class CmdExec extends UsCmdBase
{
    @ACommand(comment = "高级公式")
    public String var(String _infoStr)
    {
        if (null == _infoStr)
            return "fail, str is null";

        NPPlayerVariableGroupObj p = new NPPlayerVariableGroupObj();
        p.parseFromString(_infoStr);
        long value = NPPlayerVariableDeal.getInstance().CalculateVariableResult(getOwner()
                , p, null);
        return "ok, value: " + value;
    }

    @ACommand(comment = "执行Conditon")
    public String cond(String _infoStr)
    {
        if (null == _infoStr)
            return "fail, str is null";

        NPPlayerConditionGroupObj p = new NPPlayerConditionGroupObj();
        p.parseFromString(_infoStr);
        if (NPPlayerConditionDealerMgr.IsEnable(p, getOwner(), null))
        {
            return "ok & pass, " + _infoStr;
        } else
        {
            return "ok & refuse, " + _infoStr;
        }
    }

    @ACommand(comment = "执行远程效果[RemoteEffect配表ID]")
    public String remote(long _refId)
    {
        RefRemoteEffect ref = RefRemoteEffect.getMgr().get(_refId);
        if (null == ref)
        {
            return "Fail, not find remote effect ref";
        }

        getOwner().getPlayerComponent().execRemoteEffect(ref, getContext());
        return "ok, " + _refId;
    }

    @ACommand(comment = "执行效果[效果表达式]")
    public String effect(String _infoStr)
    {
        if (null == _infoStr)
            return "fail, str is null";

        NPPlayerEffectListParse p = new NPPlayerEffectListParse();
        p.parseFromString(_infoStr);
        NPPlayerEffectDealer.dealEffect(p.getPlayerEffectList(), getOwner(), null, getContext());
        
        //推送数据
        if(!getContext().getCollector().isEmpty())
        {
        	getOwner().sendMsgToGC(getContext().getCollector().toProto());
        }
        
        return "ok, " + _infoStr;
    }
}
