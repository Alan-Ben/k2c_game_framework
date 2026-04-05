package MGClient.Cmd.Cmds;
${importList}
import MGClient.Cmd.Annotation.Command;
import MGClient.Cmd.CmdBase;
import java.util.List;

@MGClient.Cmd.Annotation.Commander(comment = "${comment}", name = "${name}")
public class ${class_name} extends CmdBase {
%for cmd in cmd_List:
	@Command(comment = "${cmd.comment}")
	public void ${cmd.name}(${cmd.param_list})
	{
		${cmd.body}
	}
% endfor
}
