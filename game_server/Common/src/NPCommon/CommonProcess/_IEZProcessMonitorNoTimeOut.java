package NPCommon.CommonProcess;

import ALBasicServer.ALProcess._AALProcess;
import ALBasicServer.ALProcess._IALProcessMonitor;
import NPCommon.Log.CommLog;

/**
 * 简易 ALProcess 接口 隐藏一些不常用的接口
 */
public interface _IEZProcessMonitorNoTimeOut extends _IALProcessMonitor
{
    /**
     * 返回对应过程标记的监控时长,指定tag超时时间，超过此时长会执行onTimeOut
     * 默认采用60秒超时
     * @param _processTag 过程名
     * @return long 时长
     */
    @Override
    default long monitorTimeMS(String _processTag)
    {
        return 100;
    }


    /**
     * 在超过监控时长的时候触发的回调,超时并不会阻碍运行，只是会
     * 额外执行 此函数
     * @param _processTime 执行时间
     * @param _processTag  过程名
     * @param _extInfo     额外信息
     */
    @Override
    default void onTimeout(long _processTime, String _processTag, String _extInfo)
    {

    }

    /**
     * 在完成步骤的时候，如果超过监控时长时调用的完成超时回调，超时并不会阻碍运行，只是会
     * 额外执行 此函数
     * @param _processTime 执行时间
     * @param _processTag  过程名
     * @param _extInfo     额外信息
     */
    @Override
    default void onTimeoutDone(long _processTime, String _processTag, String _extInfo)
    {

    }

    /**
     * 在出现错误（异常）的时候的步骤错误回调
     * @param _processTimeMS 执行时间
     * @param _processTag    过程名
     * @param _exInfo        额外信息
     * @param _ex            异常原因
     */
    @Override
    default void onErr(long _processTimeMS, String _processTag, String _exInfo, Exception _ex)
    {
        CommLog.error("Process:{} run onException cost:{} extInfo:{} err:{}", _processTag, _processTimeMS, _exInfo, _ex.getMessage());
    }

    /**
     * 在某个过程因为失败卡住的时候调用的事件函数
     * @param _process 错误进程
     */
    @Override
    default void onProcessFailStop(_AALProcess _process)
    {
        CommLog.info("Process:{} onProcessFailStop", _process.getFullProcessTag());
    }

    /**
     * 在根节点的过程被终止时触发的函数
     * 显式的调用 stopProcess() 或者在ResDelegateProcess的回调中返回false时调用
     */
    @Override
    void onRootProecssStop();

    /**
     * 在根节点的过程z正常结束时触发
     */
    @Override
    void onRootProecssSuc();

    /**
     * 在根节点的过程完成时执行的处理，此函数必定触发
     */
    @Override
    default void onRootProecssDone()
    {

    }
}
