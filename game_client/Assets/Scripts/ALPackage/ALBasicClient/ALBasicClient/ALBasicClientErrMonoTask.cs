using System;
using System.Collections.Generic;

namespace ALPackage
{
   /**********************
    * 用于在接收线程调用的错误函数任务
    **/
   public class ALBasicClientErrMonoTask : _IALBaseMonoTask
   {
       private Action _m_aDelegate;

       public ALBasicClientErrMonoTask(Action _delegate)
       {
           _m_aDelegate = _delegate;
       }

       /*******************
        * 任务具体的执行函数
        **/
       public void deal()
       {
           if (null == _m_aDelegate)
               return;

           _m_aDelegate();
       }
   }
}
