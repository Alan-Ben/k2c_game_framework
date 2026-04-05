package NPHsClient;

import NP2HS_R.p002_HsClientOp.NP2HS_R_002_001_ReqExecGmSuper;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Delegate.HandlerOne;
import NPCommon.Util.Pair.WCGPair;
import NPHsClient.HSListener.HSMsgDispather;
import WCGCommon.Enum.NPEnum.ENPSingleServerType;
import WCGCommon.Enum.NPEnum.EServerType;

import java.io.File;
import java.io.IOException;
import java.nio.ByteBuffer;
import java.util.ArrayList;
import java.util.List;

public class HsClientMain
{
    public static String outPutfileName = "";

    public static void main(String[] args)
    {
        CommonFunc.setTimeZone("GMT+8:00");

        if (args.length == 0)
        {
            showHelp();
            return;
        }
        //===============解析参数====
        String strServerList = "";
        String strCmd = "";
        boolean isAll = false;
        String strPort = "";
        String strServerType = "";
        //新增：全部需要执行 ref reload的服务器列表
        boolean isAllRef = false;

        for (String arg : args)
        {
            //1.此部分是用于解析此次操作要作用的服务器目标
            //1.1和1.2是互斥的，只能有一个
            //1.1区分是否对所有读取配表的服务器执行
            if (checkHasArg(arg, new String[]{"--G", "-G", "--g", "-g"}))
            {
                isAllRef = true;
            }

            //1.2解析目标服务器类型
            String serverType = parseArg(arg, new String[]{"--T", "-T", "--t", "-t",});
            if (null != serverType)
            {
                strServerType = serverType;
            }
            //区分是否对该类型下的所有服务器执行
            if (checkHasArg(arg, new String[]{"--A", "-A", "--a", "-a"}))
            {
                isAll = true;
            }
            //解析指定的服务器id列表
            String serverListArg = parseArg(arg, new String[]{"--S", "-S", "--s", "-s",});
            if (null != serverListArg)
            {
                if (serverListArg.compareToIgnoreCase("all") == 0)
                {
                    isAll = true;
                } else
                {
                    strServerList = serverListArg;
                }
            }

            //2.解析命令
            String cmd = parseArg(arg, new String[]{"--C", "-C", "--c", "-c",});
            if (null != cmd)
            {
                strCmd = cmd;
            }

            //3.解析输出文件名
            String fileNameArg = parseArg(arg, new String[]{"--F", "-F", "--f", "-f",});
            if (null != fileNameArg)
            {
                outPutfileName = fileNameArg;
            }

            //4.解析目标端口
            String sPort = parseArg(arg, new String[]{"--P", "-P", "--p", "-p",});
            if (null != sPort)
            {
                strPort = sPort;
            }
        }


        if (strServerType.isEmpty())
        {
            strServerType = "us";
        }
        WCGPair<EServerType, Integer> serverTypeInfo = parseServerType(strServerType);
        if (null == serverTypeInfo)
        {
            System.out.println("invalid server type:" + strServerType);
            return;
        }
        if (strCmd.isEmpty())
        {
            System.out.println("need gm command,please input  --C param");
            return;
        }

        //不是single服务器的情况下必须制定服务器列表或者all

        List<Integer> tempServerList = new ArrayList<Integer>();

        if (serverTypeInfo.first == EServerType.SINGLE)
        {
            tempServerList.add(serverTypeInfo.second);
        } else
        {
            if (!isAll && !isAllRef && strServerList.isEmpty())
            {
                System.out.println("Need Server List,please input  --S param");
                return;
            } else
            {
                try
                {
                    tempServerList = CommonFunc.listIntFromString(strServerList);
                } catch (Exception e)
                {
                    System.out.println("can not parse serverlist from :" + strServerList + " 服务器id列表格式不正确,解析失败. eg:-S1;2;3;4 or -SAll");
                    return;
                }
            }
        }

        if (outPutfileName.isEmpty())
        {
            System.out.println("Need out put file name, please input --F param");
            return;
        }
        if (!strPort.isEmpty() && !CommonFunc.isNumeric(strPort))
        {
            System.out.println("Hs port:" + strPort + " is not number");
            return;
        }

        //检测文件。
        File file = new File(outPutfileName);
        if (!file.exists())
        {
            try
            {
                file.createNewFile();
            } catch (IOException e)
            {
                System.out.println("can not create file:" + outPutfileName + " " + e.getMessage());
                return;
            }
        }
        if (!(file.isFile() && file.canWrite()))
        {
            System.out.println("Can not write to file:" + outPutfileName);
            return;
        }
        file.delete();


        final List<Integer> serverList = tempServerList;
        if (strCmd.contains("@"))
        {
            strCmd = strCmd.replace('@', ' ');
        } else
        {
            strCmd = strCmd.replace('/', ' ');
        }

        final String gmCommand = strCmd;
        final boolean isToAll = isAll;
        final boolean isToAllRef = isAllRef;

        int port = strPort.isEmpty() ? 9202 : Integer.parseInt(strPort);

        HsClient client = new HsClient();
        client.OnLoginSucc.addHandler(null, new HandlerOne<String>()
        {
            @Override
            public void handle(String t)
            {
                System.out.println("login to hs success!");
                NP2HS_R_002_001_ReqExecGmSuper proto = new NP2HS_R_002_001_ReqExecGmSuper();
                proto.setServerType(serverTypeInfo.first.ordinal());
                proto.setIsAll(isToAll);
                proto.getServerIdList().addAll(serverList);
                proto.setGmComamnd(gmCommand);
                proto.setIsAllRef(isToAllRef);

                client.sendProto(proto);
            }
        });

        client.OnError.addHandler(null, new HandlerOne<String>()
        {
            @Override
            public void handle(String _errMsg)
            {
                System.out.println("got err:" + _errMsg);
                client.exit();
            }
        });

        client.OnReceiveMsg.addHandler(null, new HandlerOne<ByteBuffer>()
        {
            @Override
            public void handle(ByteBuffer _msg)
            {
                System.out.println(String.format("received msg [%d-%d] ", _msg.get(0), _msg.get(1)));
                HSMsgDispather.getInstance().DealProtocol(client, _msg);

            }
        });

        client.login("127.0.0.1", port, "root", "123", "");
    }


    private static WCGPair<EServerType, Integer> parseServerType(String strServerType)
    {
        if (strServerType.compareToIgnoreCase("ps") == 0)
        {
            return new WCGPair<>(EServerType.SINGLE, 0);
        }
        if (strServerType.compareToIgnoreCase("lcs") == 0)
        {
            return new WCGPair<>(EServerType.SINGLE, ENPSingleServerType.LOGIN_CHECK.ordinal());
        }
        if (strServerType.compareToIgnoreCase("cs") == 0)
        {
            return new WCGPair<>(EServerType.SINGLE, ENPSingleServerType.COMMON.ordinal());
        }
        if (strServerType.compareToIgnoreCase("hs") == 0)
        {
            return new WCGPair<>(EServerType.SINGLE, ENPSingleServerType.HTTP.ordinal());
        }
        if (strServerType.compareToIgnoreCase("is") == 0)
        {
            return new WCGPair<>(EServerType.SINGLE, ENPSingleServerType.INTERFACE.ordinal());
        }
        if (strServerType.compareToIgnoreCase("rcs") == 0)
        {
            return new WCGPair<>(EServerType.SINGLE, ENPSingleServerType.RECORD.ordinal());
        }
        if (strServerType.compareToIgnoreCase("ss") == 0)
        {
            return new WCGPair<>(EServerType.SINGLE, ENPSingleServerType.SCHEDULE.ordinal());
        }
        if (strServerType.compareToIgnoreCase("mms") == 0)
        {
            return new WCGPair<>(EServerType.SINGLE, ENPSingleServerType.MARRY_MATCH.ordinal());
        }
        if (strServerType.compareToIgnoreCase("dns") == 0)
        {
            return new WCGPair<>(EServerType.SINGLE, ENPSingleServerType.DINNER.ordinal());
        }

        //////////////////////////////////////////////////////////////// 多服分界线 ////////////////////////////////////////////////////////////////
        if (strServerType.compareToIgnoreCase("ls") == 0)
        {
            return new WCGPair<>(EServerType.LOGIN, 0);
        }
        if (strServerType.compareToIgnoreCase("us") == 0)
        {
            return new WCGPair<>(EServerType.USER, 0);
        }
        if (strServerType.compareToIgnoreCase("gs") == 0)
        {
            return new WCGPair<>(EServerType.GATE, 0);
        }
        if (strServerType.compareToIgnoreCase("cgs") == 0)
        {
            return new WCGPair<>(EServerType.CROSS_GAME, 0);
        }
        if (strServerType.compareToIgnoreCase("crs") == 0)
        {
            return new WCGPair<>(EServerType.CROSS_RANK, 0);
        }
        return null;
    }


    private static void showHelp()
    {
        String help = "--T serverType must specified. eg: hs,ps,cs,us,hs,ls,gs,\n"
                + "[--S] [S]erverIdList of server typeId,if -a has not specified,must not be empty. eg: --S1;2;3;4  --SAll\n"
                + "[--A] is send to [A]ll us server. eg: --A \n"
                + "[--G] is send to All load Ref server. eg: --G \n"
                + "--C [C]ommand to execute,can not be empty. eg: --Cref/reload or --C'hot@reload@aa.bb$1;cc.dd$2'\n"
                + "--F [F]ile to out put the results,can not be empty. eg: --F./out.txt\n"
                + "[--P] [P]ort of HsServer default is 9202. eg: --P9202 \n";

        System.out.println(help);
    }

    private static String parseArg(String arg, String[] _argPrefix)
    {
        for (String prefix : _argPrefix)
        {
            if (arg.startsWith(prefix))
            {

                String strArg = arg.substring(prefix.length());
                if (strArg == null || strArg.isEmpty())
                    return null;
                strArg = strArg.trim();


                //去掉头尾的引号
                if (strArg.startsWith("'") || strArg.startsWith("\""))
                {
                    strArg = strArg.substring(1);
                }
                if (strArg.endsWith("'") || strArg.endsWith("\""))
                {
                    strArg = strArg.substring(0, strArg.length() - 1);
                }
                return strArg;
            }
        }
        return null;
    }

    private static boolean checkHasArg(String arg, String[] _argPrefix)
    {
        for (String prefix : _argPrefix)
        {
            if (arg.startsWith(prefix))
            {
                return true;
            }
        }
        return false;
    }


}
