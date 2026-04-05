using ALPackage;
using NPEnum;
using SQLite4Unity3d;
using System.Collections.Generic;


/**************
 * 情人皮肤等级配表
 **/

[System.Serializable]
public class GConsortSkinLvlRefObj : _IALBasicRefObj
{
    public long _refId { get { return id; } }

    public long id;
    public long consort_skin_id;//皮肤ID
    public long lvl;//等级
}

public class GSOConsortSkinLvlRefSet : _TALSOBasicRefSet<GConsortSkinLvlRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/consort.unity3d"; } }
    public static string objName { get { return "consort_skin_lvl"; } }
}
