package MGClient.Cmd;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;

public class Commander
{
    private String name;
    private String comment;
    private Map<String, Command> commands;

    public Commander(MGClient.Cmd.Annotation.Commander annotation)
    {
        name = annotation.name();
        comment = annotation.comment();
        commands = new ConcurrentHashMap<>();
    }

    public String getName()
    {
        return name;
    }

    public String run(String cmdname, String[] args) throws Exception
    {
        Command command = commands.get(cmdname);
        if (command == null || cmdname.isEmpty())
        {
            StringBuilder result = new StringBuilder("command[" + cmdname + "] not found in commander[" + this.name + "]");
            result.append("\ncommand list:");
            for (String key : commands.keySet())
            {
                result.append("\n").append(key);
            }
            return result.toString();
        }
        return command.run(args);
    }

    public String getHelp()
    {
        StringBuilder sBuilder = new StringBuilder("");
        List<Command> cmdList = new ArrayList<>(commands.values());
        cmdList.sort((o1, o2) -> o1.getName().compareToIgnoreCase(o2.getName()));
        for (Command command : cmdList)
        {
            sBuilder.append(command.toString()).append("\n");
        }
        return sBuilder.toString();
    }

    public String toString()
    {
        return name + " : " + comment;
    }

    public void addCommand(Command command)
    {
        commands.put(command.getName(), command);
    }
}