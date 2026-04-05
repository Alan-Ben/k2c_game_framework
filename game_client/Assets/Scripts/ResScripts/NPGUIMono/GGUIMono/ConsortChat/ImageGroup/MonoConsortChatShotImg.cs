
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EConsortChatShotType
{
    [InspectorName("远景")]
    LongShot = 0,
    [InspectorName("中景")]
    MidShot = 1,
    [InspectorName("近景")]
    ShortShot = 2,
    [InspectorName("空景")]
    EmptyShot = 3,
}
public class MonoConsortChatShotImg : MonoBehaviour
{
    public List<ShotSetting> shotSettings; 
}
