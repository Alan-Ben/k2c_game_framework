package NPCommon.CommonProcess;

import ALBasicServer.ALProcess.ALProcess;

/**
 * @description:
 * @author: ricci
 * @date: 2022-07-19 15:55:17
 */
public class ProcessExample
{


    public static void process()
    {
        ALProcess mainProcess = ALProcess.CreateProcess("main_process");
        ALProcess process = ALProcess.CreateProcess("listProcess_2");
        ALProcess[] list = new ALProcess[2];
        list[0] = process;
        list[1] = ALProcess.CreateProcess("listProcess_1");
        list[1].addFuncProcess(() ->
        {
            long id = Thread.currentThread().getId();
            System.out.printf("threadId:{%s} processName:{%s}\n", id, "process_list[0]");
            return true;
        });

        process.addResDelegateProcess(action ->
        {
            long id = Thread.currentThread().getId();
            System.out.printf("threadId:{%s} processName:{%s}\n", id, "process_1");
            action.dealAction(false);
        }, "process_1").addActionProcess(action ->
        {
            long id = Thread.currentThread().getId();
            System.out.printf("threadId:{%s} processName:{%s}\n", id, "process_2");
            action.dealAction();
        }, "process_2").addFuncProcess(() ->
        {
            long id = Thread.currentThread().getId();
            System.out.printf("threadId:{%s} processName:{%s}\n", id, "process_3");
            return false;
        }, null, false).addFuncProcess(() ->
        {
            long id = Thread.currentThread().getId();
            System.out.printf("threadId:{%s} processName:{%s}\n", id, "process_4");
            return false;
        }, () ->
        {
            long id = Thread.currentThread().getId();
            System.out.printf("threadId:{%s} processName:{%s}\n dealFail", id, "process_4");
        }, false);

        mainProcess.addMultiProcess(list);

        mainProcess.dealProcess(new _IEZProcessMonitor()
        {
            @Override
            public void onRootProecssStop()
            {
                System.out.println("onRootProecssStop");
            }

            @Override
            public void onRootProecssSuc()
            {
                System.out.println("onRootProecssSuc");
            }
        });
    }
}
