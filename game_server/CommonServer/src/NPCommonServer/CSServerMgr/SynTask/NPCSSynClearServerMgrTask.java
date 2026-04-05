package NPCommonServer.CSServerMgr.SynTask;

import ALBasicServer.ALTask._IALSynTask;
import NPCommonServer.CSServerMgr.CSServerMgr;

/*********
 * 清空区域信息的任务
 * @author Administrator
 *
 */
public class NPCSSynClearServerMgrTask implements _IALSynTask
{

    @Override
    public void run()
    {
        //从平台断开时清空区域信息
        CSServerMgr.getInstance().clear();
    }

}
