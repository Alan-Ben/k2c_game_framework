package PayCenter.PayServer.GMCommand.Cmds;

import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import PayCenter.PayCenter;
import PayCenter.PCParams;
import PayCenter.PayServer.GMCommand.PCSCmdBase;

/**
 * CmdServer - PayCenter服务器GM命令处理器
 * 
 * 主要功能：
 * 1. 提供PayCenter系统的GM命令接口
 * 2. 支持服务器状态查询和管理操作
 * 3. 参数配置查看和调试功能
 * 4. 系统维护和故障排查命令
 * 
 * 设计特点：
 * - 基于注解驱动的GM命令框架
 * - 支持多种管理命令类型
 * - 简洁的命令实现方式
 * - 统一的返回格式
 */
@ACommander(comment = "PayCenter服务器相关命令", name = "server")
public class CmdServer extends PCSCmdBase
{
    @ACommand(comment = "显示服务器基本信息")
    public String info()
    {
        return "PayCenter Server";
    }

    @ACommand(comment = "显示服务器状态信息")
    public String status()
    {
        StringBuilder sb = new StringBuilder();
        sb.append("=== PayCenter Server Status ===\n");
        
        // 服务器基础信息
        sb.append(String.format("Server Count: %d\n", PayCenter.getInstance().getAllServerList().size()));
        
        // 内存信息
        Runtime runtime = Runtime.getRuntime();
        long totalMemory = runtime.totalMemory();
        long freeMemory = runtime.freeMemory();
        long usedMemory = totalMemory - freeMemory;
        long maxMemory = runtime.maxMemory();
        
        sb.append(String.format("Memory - Total: %d MB, Used: %d MB, Free: %d MB, Max: %d MB\n", 
            totalMemory / 1024 / 1024, 
            usedMemory / 1024 / 1024,
            freeMemory / 1024 / 1024,
            maxMemory / 1024 / 1024));
        
        return sb.toString();
    }

    @ACommand(comment = "执行垃圾回收")
    public String gc()
    {
        Runtime runtime = Runtime.getRuntime();
        long beforeGC = runtime.totalMemory() - runtime.freeMemory();
        
        System.gc();
        
        try { Thread.sleep(100); } catch (Exception ignored) {}
        
        long afterGC = runtime.totalMemory() - runtime.freeMemory();
        long freedMemory = beforeGC - afterGC;
        
        return String.format("GC completed. Freed memory: %d MB", freedMemory / 1024 / 1024);
    }
}