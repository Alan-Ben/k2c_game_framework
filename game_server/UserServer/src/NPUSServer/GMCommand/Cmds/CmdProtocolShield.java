package NPUSServer.GMCommand.Cmds;

import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPUSServer.GMCommand.UsCmdBase;

import java.util.List;

/**
 * 协议屏蔽相关GM命令
 *
 * 提供全服级别的协议屏蔽管理功能，包括：
 * - 添加屏蔽协议
 * - 移除屏蔽协议
 * - 查询屏蔽列表
 * - 清空所有屏蔽
 */
@ACommander(comment = "协议屏蔽相关命令", name = "protocolShield")
public class CmdProtocolShield extends UsCmdBase
{
    @ACommand(comment = "屏蔽协议 [主协议号] [副协议号]")
    public String shield(int mainProtocol, int subProtocol)
    {
        if (getUserServer().getProtocolShieldMgr().addShieldProtocol(mainProtocol, subProtocol))
        {
            return String.format("ok, 协议 %d-%d 已屏蔽", mainProtocol, subProtocol);
        }
        else
        {
            return String.format("fail, 协议 %d-%d 已存在屏蔽列表中", mainProtocol, subProtocol);
        }
    }

    @ACommand(comment = "解除屏蔽协议 [主协议号] [副协议号]")
    public String unshield(int mainProtocol, int subProtocol)
    {
        if (getUserServer().getProtocolShieldMgr().removeShieldProtocol(mainProtocol, subProtocol))
        {
            return String.format("ok, 协议 %d-%d 已解除屏蔽", mainProtocol, subProtocol);
        }
        else
        {
            return String.format("fail, 协议 %d-%d 不在屏蔽列表中", mainProtocol, subProtocol);
        }
    }

    @ACommand(comment = "查看所有屏蔽协议")
    public String list()
    {
        List<String> protocols = getUserServer().getProtocolShieldMgr().getAllShieldProtocols();
        if (protocols.isEmpty())
        {
            return "当前没有屏蔽的协议";
        }

        StringBuilder sb = new StringBuilder();
        sb.append("当前屏蔽的协议列表：\n");
        for (String protocol : protocols)
        {
            sb.append("  ").append(protocol).append("\n");
        }
        return sb.toString();
    }

    @ACommand(comment = "清空所有屏蔽协议")
    public String clearAll()
    {
        getUserServer().getProtocolShieldMgr().clearAll();
        return "ok, 已清空所有屏蔽协议";
    }
}
