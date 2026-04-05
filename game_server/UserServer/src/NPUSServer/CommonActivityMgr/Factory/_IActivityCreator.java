package NPUSServer.CommonActivityMgr.Factory;

import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.NPUserServer;
import USDB.Bo.ActivityBaseBO;

/******
 * 活动对象的创建接口
 */
@FunctionalInterface
public interface _IActivityCreator
{
    _AActivityBase create(NPUserServer _server, ActivityBaseBO _bo);
}