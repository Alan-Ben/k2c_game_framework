package MGClient.Cmd;

import MGClient.ClientPlayer.ClientPlayer;
import NPCommon.Log.CommLog;
import NPCommon.Util.CommClass;

import java.lang.reflect.Method;
import java.util.Arrays;
import java.util.Map;
import java.util.Set;
import java.util.concurrent.ConcurrentHashMap;

/**
 * 不是你的模块，请咨询作者，弄清楚逻辑再动
 * @author colin
 * @date 2016年1月12日
 */
public class CommandMgr
{
    private Map<String, Commander> commanders = new ConcurrentHashMap<>();

    public void init(ClientPlayer player)
    {
        Set<Class<?>> clazzs = CommClass.getClasses("MGClient.Cmd.Cmds");
        for (Class<?> clazz : clazzs)
        {
            MGClient.Cmd.Annotation.Commander aCommander = clazz.getAnnotation(MGClient.Cmd.Annotation.Commander.class);
            if (aCommander == null)
            {
                continue;
            }

            CmdBase excuter = null;
            try
            {
                excuter = (CmdBase) clazz.newInstance();
            } catch (Exception e)
            {
                CommLog.error("", e);
                continue;
            }
            excuter.setOwner(player);
            Commander commander = new Commander(aCommander);
            for (Method m : clazz.getMethods())
            {
                MGClient.Cmd.Annotation.Command aCommand = m.getAnnotation(MGClient.Cmd.Annotation.Command.class);
                if (aCommand == null)
                {
                    continue;
                }
                Command command = new Command(aCommand, m, excuter);
                commander.addCommand(command);
            }
            commanders.put(commander.getName().toLowerCase(), commander);
        }
    }

    public String run(String cmdline)
    {
        if (cmdline == null)
        {
            return "null command";
        }
        cmdline = cmdline.trim();
        if (cmdline.isEmpty())
        {
            return "empty command";
        }
        //help
        String[] args = cmdline.split("\\s+");
        if (args[0].toLowerCase().equalsIgnoreCase("help"))
        {

            if (args.length == 1)
            {
                return getHelp();

            } else
            {
                String cmd = args[1].toLowerCase();
                Commander commander = commanders.get(cmd);
                if (commander == null)
                {
                    return "help commander[" + cmd + "] not found";
                } else
                {
                    return commander.getHelp();
                }
            }
        }
        //real command
        Commander commander = commanders.get(args[0].toLowerCase());
        if (commander == null)
        {
            return "commander[" + args[0] + "] not found";
        }
        String cmd = "";
        if (args.length >= 2)
        {
            cmd = args[1].toLowerCase();
        }
        try
        {
            if (args.length >= 2)
            {
                return commander.run(cmd, Arrays.copyOfRange(args, 2, args.length));
            } else
            {

                return commander.getHelp();
            }
        } catch (Exception e)
        {
            CommLog.error("error occurs while running command:{}", cmdline, e);
            return "error occurs while running command";
        }
    }

    private String getHelp()
    {
        StringBuilder sBuilder = new StringBuilder("");
        for (Commander commander : commanders.values())
        {
            sBuilder.append(commander.toString()).append("\n");
        }
        return sBuilder.toString();
    }
}
