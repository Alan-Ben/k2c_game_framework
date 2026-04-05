using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;

/**************
 * SDK登录Item
 **/
public class NPGGUIMonoSDKLoginGridItem : _TALUGUIMonoGridItem
{

    /** 登录Icon */
    public Image imgLoginIcon;
    /** 登录平台名字 */
    public Text txtName;
    /** 登录按钮 */
    public GameObject btnLogin;

    //曾经登陆过的展示
    public List<GameObject> everLoginShowList;
    //上一次登陆过的展示  比曾经登陆的展示优先级来得高
    public List<GameObject> lastLoginShowList;

}
