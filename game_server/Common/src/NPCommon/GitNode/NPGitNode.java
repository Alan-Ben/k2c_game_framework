package NPCommon.GitNode;

import NPCommon.Log.CommLog;

import java.io.BufferedReader;
import java.io.InputStream;
import java.io.InputStreamReader;

public class NPGitNode
{
    private static String _m_sGitNodeInfo = null;

    public static void PrintGitNodeInfo()
    {
        if (null == _m_sGitNodeInfo)
        {
            loadGitNodeInfo();
        }
        CommLog.info(_m_sGitNodeInfo);
    }

    public static String getGitNodeInfo()
    {
        return _m_sGitNodeInfo;
    }

    private static void loadGitNodeInfo()
    {
        InputStream res = NPGitNode.class.getResourceAsStream("version.txt");
        if (res == null)
        {
            CommLog.error("ERROR:  version.txt not found!!!!!!!!");
            _m_sGitNodeInfo = "";
            return;
        }

        BufferedReader br = new BufferedReader(new InputStreamReader(res));
        StringBuilder sb = new StringBuilder();
        String line = null;
        try
        {
            while ((line = br.readLine()) != null)
            {
                sb.append(line);
                sb.append("\n");
            }
            _m_sGitNodeInfo = sb.toString();
        } catch (Exception e)
        {
            CommLog.error("load version.txt failed! ", e);
            _m_sGitNodeInfo = "";
        }
    }
}
