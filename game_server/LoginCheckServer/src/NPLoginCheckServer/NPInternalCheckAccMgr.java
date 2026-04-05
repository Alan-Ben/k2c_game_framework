package NPLoginCheckServer;

import java.util.Hashtable;

/**********************
 * 公司内部测试的账号数据管理器
 * @author mj
 *
 */
public class NPInternalCheckAccMgr
{
    private static NPInternalCheckAccMgr _g_instance = new NPInternalCheckAccMgr();

    public static NPInternalCheckAccMgr getInstance()
    {
        if (null == _g_instance)
            _g_instance = new NPInternalCheckAccMgr();

        return _g_instance;
    }

    //是否需要检测，默认不检测，GM命令开启检测
    private boolean _m_bNeedCheck;

    //账号的信息存储数据表
    protected Hashtable<String, String> _m_htAccInfoTable;

    protected NPInternalCheckAccMgr()
    {
        //读取配置觉得要不要默认开启检测
        _m_bNeedCheck = LoginCheckServerConf.getInstance().isCheckAcc();

        _m_htAccInfoTable = new Hashtable<String, String>();

        _m_htAccInfoTable.put("devo".toLowerCase(), "123456789");
        _m_htAccInfoTable.put("freesia".toLowerCase(), "51522zzwlwlbb");
        _m_htAccInfoTable.put("alvin".toLowerCase(), "abc123456");
        _m_htAccInfoTable.put("uproject".toLowerCase(), "118899");
        _m_htAccInfoTable.put("dusunzibingfa".toLowerCase(), "denis666");
        _m_htAccInfoTable.put("kkz".toLowerCase(), "121");
        _m_htAccInfoTable.put("stara".toLowerCase(), "stara");
        _m_htAccInfoTable.put("rosie118".toLowerCase(), "rosie123456");
        _m_htAccInfoTable.put("ben".toLowerCase(), "ben123");
        _m_htAccInfoTable.put("jacky.zeng".toLowerCase(), "123456");
        _m_htAccInfoTable.put("bert12138".toLowerCase(), "iopjklbnm890");
        _m_htAccInfoTable.put("aki".toLowerCase(), "123456");
        _m_htAccInfoTable.put("ricci".toLowerCase(), "ricci");
        _m_htAccInfoTable.put("lin".toLowerCase(), "123456");
        _m_htAccInfoTable.put("ivy".toLowerCase(), "666");
        _m_htAccInfoTable.put("sura".toLowerCase(), "sura123654");
        _m_htAccInfoTable.put("cassen001".toLowerCase(), "2023cai");
        _m_htAccInfoTable.put("samuel".toLowerCase(), "123456");
        _m_htAccInfoTable.put("christine".toLowerCase(), "123456");
        _m_htAccInfoTable.put("mandy".toLowerCase(), "mandy123");
        _m_htAccInfoTable.put("rick106".toLowerCase(), "rick123456@");
        _m_htAccInfoTable.put("gavin".toLowerCase(), "2972336262");
        _m_htAccInfoTable.put("zeroth".toLowerCase(), "15980797893");
        _m_htAccInfoTable.put("rio".toLowerCase(), "110110cyc");
        _m_htAccInfoTable.put("mington".toLowerCase(), "123456");
        _m_htAccInfoTable.put("randy460".toLowerCase(), "randy460");
        _m_htAccInfoTable.put("egg".toLowerCase(), "egg147");
        _m_htAccInfoTable.put("henry".toLowerCase(), "henry123456");
        _m_htAccInfoTable.put("callum001".toLowerCase(), "123456789");
        _m_htAccInfoTable.put("tony123".toLowerCase(), "123321");
        _m_htAccInfoTable.put("hopen".toLowerCase(), "hopen123");
        _m_htAccInfoTable.put("wesley".toLowerCase(), "123456");
        _m_htAccInfoTable.put("mark".toLowerCase(), "mark487");
        _m_htAccInfoTable.put("coda".toLowerCase(), "coda123456");
        _m_htAccInfoTable.put("adam_panda".toLowerCase(), "adam123456");
        _m_htAccInfoTable.put("deeee_cheer".toLowerCase(), "matea753753");
        _m_htAccInfoTable.put("shanks".toLowerCase(), "123456");
        _m_htAccInfoTable.put("natalie".toLowerCase(), "xixi1225");
        _m_htAccInfoTable.put("jiumu".toLowerCase(), "rjywslqh");
        _m_htAccInfoTable.put("dana13960".toLowerCase(), "dana13960");
        _m_htAccInfoTable.put("hhermione".toLowerCase(), "24680");
        _m_htAccInfoTable.put("kenny01".toLowerCase(), "123456");
        _m_htAccInfoTable.put("maui_wang".toLowerCase(), "meng666");
        _m_htAccInfoTable.put("cinkin".toLowerCase(), "123456");
        _m_htAccInfoTable.put("eleven_chen".toLowerCase(), "19900429");
        _m_htAccInfoTable.put("jeff".toLowerCase(), "123456");
        _m_htAccInfoTable.put("zacks".toLowerCase(), "zacks0592");
        _m_htAccInfoTable.put("niki888".toLowerCase(), "qwertyuiop");
        _m_htAccInfoTable.put("leotest".toLowerCase(), "zlp123456");
        _m_htAccInfoTable.put("robin_y".toLowerCase(), "123456");
        _m_htAccInfoTable.put("cooper".toLowerCase(), "potato123");
        _m_htAccInfoTable.put("grant_li_0303".toLowerCase(), "grant_li_0303");
        _m_htAccInfoTable.put("penny".toLowerCase(), "linpinghua666");
        _m_htAccInfoTable.put("wong01".toLowerCase(), "123456");
        _m_htAccInfoTable.put("nick".toLowerCase(), "nickqwe123");
        _m_htAccInfoTable.put("mjdeven".toLowerCase(), "deven629");
        _m_htAccInfoTable.put("qa123789456".toLowerCase(), "test123789456");
        _m_htAccInfoTable.put("sofia".toLowerCase(), "123456");
        _m_htAccInfoTable.put("kira".toLowerCase(), "941870");
        _m_htAccInfoTable.put("nemo_vii".toLowerCase(), "mj123456");
        _m_htAccInfoTable.put("chris".toLowerCase(), "13572468");
        _m_htAccInfoTable.put("creeper".toLowerCase(), "1237043");
        _m_htAccInfoTable.put("121212".toLowerCase(), "111222");
        _m_htAccInfoTable.put("colin".toLowerCase(), "123456789");
        _m_htAccInfoTable.put("13527140056".toLowerCase(), "abc123cba123");
        _m_htAccInfoTable.put("sanity".toLowerCase(), "123456");
        _m_htAccInfoTable.put("clein0330".toLowerCase(), "123456");
        _m_htAccInfoTable.put("april".toLowerCase(), "123456");
        _m_htAccInfoTable.put("mc5309".toLowerCase(), "mc5309");
        _m_htAccInfoTable.put("darren".toLowerCase(), "123456");
        _m_htAccInfoTable.put("mjshirley".toLowerCase(), "shirley1");
        _m_htAccInfoTable.put("18950147761".toLowerCase(), "123456");
        _m_htAccInfoTable.put("nike".toLowerCase(), "xujiang");
        _m_htAccInfoTable.put("lucas".toLowerCase(), "123456");
        _m_htAccInfoTable.put("18759810577".toLowerCase(), "869225.jj");
        _m_htAccInfoTable.put("joey".toLowerCase(), "joey123");
        _m_htAccInfoTable.put("13646015339".toLowerCase(), "liang1987");
        _m_htAccInfoTable.put("cynthia".toLowerCase(), "zqw123456");
        _m_htAccInfoTable.put("fannie".toLowerCase(), "987456");
        _m_htAccInfoTable.put("alzq".toLowerCase(), "123456");
        _m_htAccInfoTable.put("lois".toLowerCase(), "123456");
        _m_htAccInfoTable.put("jarrintest".toLowerCase(), "123456789");
        _m_htAccInfoTable.put("connie".toLowerCase(), "connie123");
        _m_htAccInfoTable.put("jason".toLowerCase(), "jason123");
        _m_htAccInfoTable.put("albert".toLowerCase(), "123456");
        _m_htAccInfoTable.put("1924466".toLowerCase(), "flx1924466");
        _m_htAccInfoTable.put("owen".toLowerCase(), "owen123");
        _m_htAccInfoTable.put("adam0307a".toLowerCase(), "adam123456");
        _m_htAccInfoTable.put("jennifer0311".toLowerCase(), "123456");
        _m_htAccInfoTable.put("qwe123".toLowerCase(), "123");//alun
        _m_htAccInfoTable.put("scott".toLowerCase(), "123456");
    }

    /******************
     * 判断账号信息是否合法
     * @param _accName
     * @param _pass
     * @return
     */
    public boolean judgeAcc(String _accName, String _pass)
    {
        if (!_m_bNeedCheck)
            return true;

        String pass = _m_htAccInfoTable.get(_accName.toLowerCase());
        if (null == pass)
            return false;

        //匹配密码
        if (pass.equalsIgnoreCase(_pass))
            return true;

        return false;
    }

    /***********
     * 开关是否检测账号
     */
    public void setCheckAcc(boolean _needCheck)
    {
        _m_bNeedCheck = _needCheck;
    }

    /***************
     * 更新数据信息
     * @param _accName
     * @param _pass
     */
    public void updateAccInfo(String _accName, String _pass)
    {
        synchronized (this)
        {
            _m_htAccInfoTable.put(_accName.toLowerCase(), _pass);
        }
    }
}

