package NPUSServer.GMCommand.Cmds;

import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPUSServer.GMCommand.UsCmdBase;

/**
 * 关卡测试命令，看懂再改
 * @author Valen
 * @date 2016年7月8日
 */
@ACommander(comment = "Buff相关命令", name = "buff")
public class CmdBuff extends UsCmdBase
{
	@ACommand(comment = "展示所有buff数据")
    public String listBuff()
    {
		return getOwner().getBuffComponent().toString();
    }
	
	@ACommand(comment = "调整buff [buffId，buff层数，持续时间(秒：-1永久)]")
    public String chgBuff(long _buffId, int _layers, int _secs)
    {
		getOwner().getBuffComponent().ensureBuff(_buffId, _layers, _secs, false, getContext());
		
		return "ok";
    }
	
	@ACommand(comment = "设置buff [buffId，buff层数，持续时间(秒：-1永久)]")
    public String setBuff(long _buffId, int _layers, int _secs)
    {
		getOwner().getBuffComponent().ensureBuff(_buffId, _layers, _secs, true, getContext());
		
		return "ok";
    }

	@ACommand(comment = "移除buff [buffId]")
    public String removeBuff(long _buffId)
    {
		getOwner().getBuffComponent().removeBuff(_buffId, getContext());

		return "ok";
    }

	@ACommand(comment = "查询当前层数大于1的buff")
    public String queryBuffs()
    {
		return getOwner().getBuffComponent().getMultiLayerBuffs();
    }

}
