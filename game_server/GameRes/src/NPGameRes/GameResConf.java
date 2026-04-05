package NPGameRes;

import ALBasicCommon.ALConfReader;

import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.util.Properties;

/***************
 * 配置文件
 *
 * @author Administrator
 *
 */
public class GameResConf
{
    private static GameResConf g_instance = new GameResConf();
    ;

    public static GameResConf getInstance()
    {
        return g_instance;
    }

    /**
     * 区域标记
     */
    private String _m_sRefPath;


    public String getRefPath()
    {
        return _m_sRefPath;
    }

    /*******************
     * 初始化函数
     *
     * @param _properties
     */
    public boolean init()
    {
        if (!_init("./conf/GameResConf.properties"))
            return false;

        _init("./customConf/GameResConf.properties");

        return true;
    }

    /*******************
     * 初始化单个配置文件的处理函数
     *
     * @param _properties
     */
    protected boolean _init(String _filePath)
    {
        Properties properties = new Properties();

        InputStream propertiesInputStream = null;

        try
        {
            propertiesInputStream = new FileInputStream(_filePath);
        } catch (Exception e)
        {
            e.printStackTrace();
        }

        if (null == propertiesInputStream)
        {
            return false;
        }

        // 载入配置文件
        try
        {
            properties.load(propertiesInputStream);
        } catch (IOException e)
        {
            e.printStackTrace();
        }

        try
        {
            System.out.println("[Conf init] Start load GameRes Properties ... ...");

            //配置文件地址
            _m_sRefPath = ALConfReader.readStr(properties, "GameRes.refPath", "");

            System.out.println("[Conf init] Finish load GameRes Properties ... ...");
        } catch (Exception e)
        {
            System.out.println("[Conf Init Error] Load GameRes Properties Error!!");
            e.printStackTrace();
        } finally
        {
            try
            {
                propertiesInputStream.close();
            } catch (IOException e)
            {
                e.printStackTrace();
            }
        }

        return true;
    }
}
