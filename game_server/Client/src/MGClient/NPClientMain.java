package MGClient;

import ALBasicClient.ALBasicClient;
import ALBasicServer.ALBasicServer;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;
import MGClient.ClientPlayer.ClientPlayer;
import MGClient.ClientPlayer.ClientPlayer.EClientPlayerType;
import MGClient.ClientPlayer.GamePlayerMgr;
import MGClient.Common.LoginStat;
import MGClient.Logic.LogicObjs.LogicFactory;
import MGClient.Model.LoginEntry;
import MGClient.Model.LoginListModel;
import MGClient.Model.OnlinePlayerTableModel;
import MGClient.Model.OnlinePlayerTableModel.ETableColumns;
import MGClient.Model.OnlinePlayerTableModel.PlayerTableRowData;
import MGClient.UI.*;
import NPCommon.CommonObj.NPStringReader;
import NPCommon.Dispather._AWCGProtoLogger;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.ResultMgr;
import NPCommon.Log.CommLog;
import NPCommon.Log._ILogAppender;
import NPCommon.Util.CommFile;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.DelayExecutor.DelayExecutor;
import NPCommon.Util.Delegate.HandlerNone;
import NPCommon.Util.Delegate.HandlerOne;
import NPCommon.Util.RunResult;
import NPEnum.ENPPlayerParam;
import NPGameRes.NPGRefdataCoreMgr;
import NPGameRes.NPRefDataMgr;
import SevenZip.Zipper;

import javax.swing.*;
import javax.swing.Timer;
import javax.swing.border.EmptyBorder;
import java.awt.*;
import java.awt.datatransfer.Clipboard;
import java.awt.datatransfer.StringSelection;
import java.awt.event.*;
import java.io.*;
import java.nio.charset.Charset;
import java.nio.charset.StandardCharsets;
import java.util.*;
import java.util.List;
import java.util.concurrent.ConcurrentHashMap;


public class NPClientMain implements _ILogAppender, ActionListener, _IALSynTask
{

    private static NPClientMain _g_ClientMain = null;

    public static NPClientMain getInstance()
    {
        return _g_ClientMain;
    }

    private DelayExecutor _m_delayExecuter = new DelayExecutor(1000);
    JFrame _m_Frame;
    private JTable mTable;
    @SuppressWarnings("rawtypes")
    private JList mList;
    private Map<ClientPlayer, PlayerFrame> mPlayerFrames = new ConcurrentHashMap<ClientPlayer, PlayerFrame>();
    private static SettingPanel _g_SettingPanel;
    private LinkedList<String> _m_lLogList = new LinkedList<String>();
    private boolean _m_logChged = false;
    private JTextArea _m_textArea = new JTextArea();
    private Timer _m_timer;

    //压测相关
    private int _m_StressPendingNum = 200;
    private int _m_iTickAddNum = 2;
    private boolean _m_bStresssRuning = false;
    private EClientPlayerType _m_eTestType = EClientPlayerType.normal;


    private SettingPanel getSettingPlanel()
    {
        if (null == _g_SettingPanel)
        {
            _g_SettingPanel = new SettingPanel();

            _g_SettingPanel.setLocation(500, 500);
            _g_SettingPanel.setSize(500, 200);
        }
        return _g_SettingPanel;
    }

    public static void main(String[] args)
    {
        CommonFunc.setTimeZone("GMT+8:00");
        ALBasicServer.initBasicServer(0);
        //初始化配表数据
        if (!NPRefDataMgr.getInstance().loadData(0))
        {
            return;
        }

        //初始化客户端配置对象
        NPGRefdataCoreMgr.getInstance().init();

        ALBasicClient.init();

        _g_ClientMain = new NPClientMain();


//    	ALBasicServer.startListener(5869568, 6546, null);

    }

    @SuppressWarnings({"rawtypes", "unchecked"})
    NPClientMain()
    {
        ResultMgr.getInstance().registByPackage(CommErr.class.getPackage().getName());
        _m_Frame = new JFrame();
        _m_Frame.setDefaultCloseOperation(JFrame.DISPOSE_ON_CLOSE);

        _m_Frame.addWindowListener(new WindowAdapter()
        {
            public void windowClosing(WindowEvent we)
            {
                System.exit(0);
            }
        });
        GConfigure.getInstance().OnConfigChged.addHandler(null, new HandlerNone()
        {
            @Override
            public void handle()
            {
                updateTitle();
            }
        });

        //菜单=============================
        JMenuBar menuBar = new JMenuBar();
        _m_Frame.setJMenuBar(menuBar);

        JMenu mnFileMenu = new JMenu("File");
        menuBar.add(mnFileMenu);

        JMenuItem mntmSetStartIndex = new JMenuItem("设置玩家起始ID...");
        mntmSetStartIndex.addActionListener(new ActionListener()
        {
            public void actionPerformed(ActionEvent arg0)
            {
                String strIndex = PromptInput.prompt("输入起始ID", "100");
                try
                {
                    int index = Integer.parseInt(strIndex);
                    _g_num_index = index;
                    updateTitle();
                } catch (Exception e)
                {
                    JOptionPane.showMessageDialog(null, "输入错误，必须为整型数据: " + strIndex, "WARNING_MESSAGE", JOptionPane.WARNING_MESSAGE);
                    return;
                }
            }
        });
        mnFileMenu.add(mntmSetStartIndex);

        JMenuItem mntmOpen = new JMenuItem("加载测试账号列表...");
        mntmOpen.addActionListener(new ActionListener()
        {
            public void actionPerformed(ActionEvent arg0)
            {
                JFileChooser fc = new JFileChooser("./test_data/");
                fc.showOpenDialog(_m_Frame);
                File file = fc.getSelectedFile();

                Charset cs = StandardCharsets.UTF_8;
                BufferedReader br = null;
                try
                {
                    FileInputStream fr = new FileInputStream(file);
                    br = new BufferedReader(new InputStreamReader(fr, cs));
                } catch (FileNotFoundException e)
                {
                    CommLog.error("", e);
                }
                if (null == br)
                {
                    return;
                }

                try
                {
                    String accName = br.readLine();
                    while (accName != null)
                    {
                        if (!accName.trim().isEmpty())
                        {
                            addSpecifiedPlayer(accName);
                            accName = br.readLine();
                        }

                    }
                } catch (IOException e)
                {
                    return;
                } finally
                {
                    CommFile.close(br);
                }

            }
        });
        mnFileMenu.add(mntmOpen);

        JMenuItem mntmAdd10 = new JMenuItem("add 10 player");
        mntmAdd10.addActionListener(new ActionListener()
        {
            public void actionPerformed(ActionEvent arg0)
            {
                addNumPlayer(10);
            }

        });
        mnFileMenu.add(mntmAdd10);

        JMenuItem mntmAdd20 = new JMenuItem("add 20 player");
        mntmAdd20.addActionListener(new ActionListener()
        {
            public void actionPerformed(ActionEvent arg0)
            {
                addNumPlayer(20);
            }

        });
        mnFileMenu.add(mntmAdd20);

        JMenuItem mntmAdd50 = new JMenuItem("add 50 player");
        mntmAdd50.addActionListener(new ActionListener()
        {
            public void actionPerformed(ActionEvent arg0)
            {
                addNumPlayer(50);
            }

        });
        mnFileMenu.add(mntmAdd50);
        JMenuItem mntmAdd100 = new JMenuItem("add 100 player");
        mntmAdd100.addActionListener(new ActionListener()
        {
            public void actionPerformed(ActionEvent arg0)
            {
                addNumPlayer(100);
            }

        });
        mnFileMenu.add(mntmAdd100);

        JMenuItem mntmAdd200 = new JMenuItem("add 200 player");
        mntmAdd200.addActionListener(new ActionListener()
        {
            public void actionPerformed(ActionEvent arg0)
            {
                addNumPlayer(200);
            }

        });
        mnFileMenu.add(mntmAdd200);

        JMenuItem mntmAdd300 = new JMenuItem("add 300 player");
        mntmAdd300.addActionListener(new ActionListener()
        {
            public void actionPerformed(ActionEvent arg0)
            {
                addNumPlayer(300);
            }

        });
        mnFileMenu.add(mntmAdd300);

        for (int i = 1; i <= 5; i++)
        {
            final int index = i;
            JMenuItem menuItem = new JMenuItem("add player " + i);
            menuItem.addActionListener(new ActionListener()
            {
                @Override
                public void actionPerformed(ActionEvent e)
                {
                    addIndexedPlayer(index);
                }

            });
            mnFileMenu.add(menuItem);
        }
        //添加制定名字用户
        JMenuItem menuItemSpecify = new JMenuItem("add spcified Player ");
        menuItemSpecify.addActionListener(new ActionListener()
        {
            @Override
            public void actionPerformed(ActionEvent e)
            {
                String playerName = PromptInput.prompt("输入指定帐号名", "");
                if (playerName.isEmpty()) return;

                addSpecifiedPlayer(playerName);
            }
        });
        mnFileMenu.add(menuItemSpecify);


        //添加制定名字用户
        JMenuItem menuStressTest = new JMenuItem("压力测试--排队(人数)");
        menuStressTest.addActionListener(new ActionListener()
        {
            @Override
            public void actionPerformed(ActionEvent e)
            {
                String testType = PromptSelect.prompt("输入测试类型", new String[]{"normal", "login_test", "enter_game_test"});
                if (testType == null || testType.isEmpty())
                    return;
                EClientPlayerType eType = EClientPlayerType.valueOf(testType);
                String playerNum = PromptInput.prompt("输入保持的排队人数", "200");
                if (!CommonFunc.isNumeric(playerNum))
                {
                    return;
                }
                int num = Integer.parseInt(playerNum);

                String strTickAddNum = PromptInput.prompt("每50Ms增加的登录人数", "2");
                if (!CommonFunc.isNumeric(strTickAddNum))
                {
                    return;
                }
                int tickAddNum = Integer.parseInt(strTickAddNum);

                LoginStat.getInstance().clear();
                startKeepOnlineLogic(num, tickAddNum, eType);

            }


        });
        mnFileMenu.add(menuStressTest);

        //添加制定名字用户
        JMenuItem menuStopStressTest = new JMenuItem("停止压力测试");
        menuStopStressTest.addActionListener(new ActionListener()
        {
            @Override
            public void actionPerformed(ActionEvent e)
            {
                _m_bStresssRuning = false;
            }
        });
        mnFileMenu.add(menuStopStressTest);


        //全员执行GM命令
        JMenuItem menuAllRunGM = new JMenuItem("全部玩家执行GM命令");
        menuAllRunGM.addActionListener(e ->
        {
            String command = PromptInput.prompt("输入要执行的GM命令", "war _1sign");
            if (command.isEmpty())
                return;
            GamePlayerMgr.getInstance().runGM(command);
        });
        mnFileMenu.add(menuAllRunGM);

        //全员执行GM命令
        JMenuItem menuAllRunClientGM = new JMenuItem("全部玩家执行客户端命令");
        menuAllRunClientGM.addActionListener(e ->
        {
            String command = PromptInput.prompt("输入要执行的客户端命令", "war _1sign");
            if (command.isEmpty())
                return;
            GamePlayerMgr.getInstance().runCMD(command);
        });
        mnFileMenu.add(menuAllRunClientGM);
        mnFileMenu.addSeparator();

        //最近登录的玩家
        BufferedReader br = null;
        try
        {
            FileInputStream fr = new FileInputStream("./test_data/recent.txt");
            br = new BufferedReader(new InputStreamReader(fr, StandardCharsets.UTF_8));
            String accName = br.readLine();
            while (accName != null)
            {
                String theAccName = accName;
                JMenuItem menunRecent = new JMenuItem(accName);
                menunRecent.addActionListener(new ActionListener()
                {
                    @Override
                    public void actionPerformed(ActionEvent e)
                    {
                        addSpecifiedPlayer(theAccName);
                    }
                });
                mnFileMenu.add(menunRecent);
                accName = br.readLine();
            }
        } catch (Exception e)
        {
            CommLog.error("", e);
        } finally
        {
            CommFile.close(br);
        }

////-----========================================File Menu End ==========================================


        JMenu mnLogicMenu = new JMenu("AI逻辑");
        menuBar.add(mnLogicMenu);
        List<String> logicNames = LogicFactory.getInstance().enumName();
        for (final String name : logicNames)
        {
            JMenuItem menuItem = new JMenuItem(name);
            menuItem.addActionListener(new ActionListener()
            {
                @Override
                public void actionPerformed(ActionEvent e)
                {
                    changePlayerLogic(name);
                }
            });
            mnLogicMenu.add(menuItem);
        }

        JMenu mnSettingMenu = new JMenu("选择服务器");
        menuBar.add(mnSettingMenu);

        JMenuItem menuItem = new JMenuItem("自定义...");
        menuItem.addActionListener(new ActionListener()
        {
            @Override
            public void actionPerformed(ActionEvent e)
            {
                getSettingPlanel().setVisible(true);
            }
        });
        mnSettingMenu.add(menuItem);

        JMenuItem menuItemLocalHost = new JMenuItem("127.0.0.1:5101");
        menuItemLocalHost.addActionListener(new ActionListener()
        {
            @Override
            public void actionPerformed(ActionEvent e)
            {
                GConfigure.getInstance().setServerIP("127.0.0.1", 5101);
                updateTitle();
            }
        });
        mnSettingMenu.add(menuItemLocalHost);

        JMenuItem menuItemNPMaster = new JMenuItem("192.168.10.252 master");
        menuItemNPMaster.addActionListener(new ActionListener()
        {
            @Override
            public void actionPerformed(ActionEvent e)
            {
                GConfigure.getInstance().setServerIP("192.168.10.252", 35101);
                updateTitle();
            }
        });
        mnSettingMenu.add(menuItemNPMaster);

        JMenuItem menuItemNPInternal = new JMenuItem("192.168.10.252 internal");
        menuItemNPInternal.addActionListener(new ActionListener()
        {
            @Override
            public void actionPerformed(ActionEvent e)
            {
                GConfigure.getInstance().setServerIP("192.168.10.252", 39014);
                updateTitle();
            }
        });
        mnSettingMenu.add(menuItemNPInternal);

        List<String> lines = CommFile.getLinesFromFile("./ServerList.txt");
        for (String line : lines)
        {
            NPStringReader reader = new NPStringReader(line);
            String ip = reader.readItem(':');
            String port = reader.readItem('\t', ' ');
            String name = reader.readItem();
            if (ip != null && port != null)
            {
                JMenuItem menu = new JMenuItem(String.format("%s(%s:%s)", name, ip, port));
                menuItemNPMaster.addActionListener(new ActionListener()
                {
                    @Override
                    public void actionPerformed(ActionEvent e)
                    {
                        GConfigure.getInstance().setServerIP(ip, Integer.valueOf(port));
                        updateTitle();
                    }
                });
                mnSettingMenu.add(menu);

            }
        }

        //添加制定名字用户
        JMenuItem menuItemSetUsId = new JMenuItem("设置当前Us服务器Id");
        menuItemSetUsId.addActionListener(new ActionListener()
        {
            @Override
            public void actionPerformed(ActionEvent e)
            {
                String strId = PromptInput.prompt("输入Us服务器Id", "" + GlobalStorage.getInstance().getRecentServerId());
                if (strId.isEmpty() || strId == null)
                    return;
                if (!CommonFunc.isNumeric(strId))
                {
                    return;
                }
                int usId = Integer.parseInt(strId);
                GlobalStorage.getInstance().saveRecentServerId(usId);
                updateTitle();
            }
        });
        mnSettingMenu.add(menuItemSetUsId);

        //////////////////////////////////////////////////////////////////////////////////
        JMenu mnStressMenu = new JMenu("压力测试");
        menuBar.add(mnStressMenu);

        JMenuItem stressLoginItem = new JMenuItem("测试登录");
        stressLoginItem.addActionListener(new ActionListener()
        {
            @Override
            public void actionPerformed(ActionEvent e)
            {
                StressPanel stressPanel = new StressPanel();
                stressPanel.setSize(300, 200);
                stressPanel.setLocation(500, 500);
                stressPanel.setModal(true);
                stressPanel.setVisible(true);
                runStressProcess(stressPanel.getStartId(), stressPanel.getPlayerNum(), EClientPlayerType.login_test);
            }
        });
        mnStressMenu.add(stressLoginItem);

        JMenuItem stressEnterGameItem = new JMenuItem("测试EnterGame");
        stressEnterGameItem.addActionListener(new ActionListener()
        {
            @Override
            public void actionPerformed(ActionEvent e)
            {
                StressPanel stressPanel = new StressPanel();
                stressPanel.setSize(300, 200);
                stressPanel.setLocation(500, 500);
                stressPanel.setModal(true);
                stressPanel.setVisible(true);
                runStressProcess(stressPanel.getStartId(), stressPanel.getPlayerNum(), EClientPlayerType.enter_game_test);

            }
        });
        mnStressMenu.add(stressEnterGameItem);


        //////////////////////////////////////////////////////////////////////////////////
        JMenu mnHotMenu = new JMenu("代码热更");
        menuBar.add(mnHotMenu);

        JMenuItem genClassStrItem = new JMenuItem("生成命令");
        genClassStrItem.addActionListener(new ActionListener()
        {
            @Override
            public void actionPerformed(ActionEvent e)
            {
                String projectName = PromptSelect.prompt("选择项目", new String[]{"UserServer", "Common", "GameRes", "RoomServer", "HttpServer","ActivitiesV01"});
                if (projectName == null || projectName.isEmpty())
                    return;
                String strClassName = PromptInput.prompt("输入完整类名:", "");
                if (strClassName == null || strClassName.isEmpty())
                    return;
                RunResult runResult = genClassHotFixString(projectName, strClassName);
                if (runResult.isSucc())
                {
                    Clipboard clipboard = Toolkit.getDefaultToolkit().getSystemClipboard();  //得到系统剪贴板
                    StringSelection selection = new StringSelection(runResult.getMsg());
                    clipboard.setContents(selection, null);
                } else
                {
                    JOptionPane.showMessageDialog(null, runResult.getMsg(), "WARNING_MESSAGE", JOptionPane.WARNING_MESSAGE);
                }

            }
        });
        mnHotMenu.add(genClassStrItem);


        GridBagLayout gridBagLayout = new GridBagLayout();
        gridBagLayout.columnWidths = new int[]{600, 0};
        gridBagLayout.rowHeights = new int[]{698, 0};
        gridBagLayout.columnWeights = new double[]{1.0, Double.MIN_VALUE};
        gridBagLayout.rowWeights = new double[]{1.0, Double.MIN_VALUE};
        _m_Frame.getContentPane().setLayout(gridBagLayout);

        JPanel panel = new JPanel();
        panel.setBorder(new EmptyBorder(10, 10, 10, 10));
        GridBagConstraints gbc_panel = new GridBagConstraints();
        gbc_panel.fill = GridBagConstraints.BOTH;
        gbc_panel.gridx = 0;
        gbc_panel.gridy = 0;
        _m_Frame.getContentPane().add(panel, gbc_panel);
        GridBagLayout gbl_panel = new GridBagLayout();
        gbl_panel.columnWidths = new int[]{478, 0};
        gbl_panel.rowHeights = new int[]{482, 2, 204, 0};
        gbl_panel.columnWeights = new double[]{1.0, Double.MIN_VALUE};
        gbl_panel.rowWeights = new double[]{0.0, 0.0, 1.0, Double.MIN_VALUE};
        panel.setLayout(gbl_panel);

        JSplitPane splitPane = new JSplitPane();
        GridBagConstraints gbc_splitPane = new GridBagConstraints();
        gbc_splitPane.fill = GridBagConstraints.BOTH;
        gbc_splitPane.insets = new Insets(0, 0, 5, 0);
        gbc_splitPane.gridx = 0;
        gbc_splitPane.gridy = 0;
        panel.add(splitPane, gbc_splitPane);

        JPanel lPanel = new JPanel();
        JPanel rPanel = new JPanel();
        splitPane.setLeftComponent(lPanel);
        splitPane.setRightComponent(rPanel);
        GridBagLayout gbl_lPanel = new GridBagLayout();
        gbl_lPanel.columnWidths = new int[]{258, 0};
        gbl_lPanel.rowHeights = new int[]{322, 0};
        gbl_lPanel.columnWeights = new double[]{1.0, Double.MIN_VALUE};
        gbl_lPanel.rowWeights = new double[]{1.0, Double.MIN_VALUE};
        lPanel.setLayout(gbl_lPanel);

        JScrollPane scrollPane = new JScrollPane();
        scrollPane.setHorizontalScrollBarPolicy(ScrollPaneConstants.HORIZONTAL_SCROLLBAR_NEVER);
        GridBagConstraints gbc_scrollPane = new GridBagConstraints();
        gbc_scrollPane.fill = GridBagConstraints.BOTH;
        gbc_scrollPane.gridx = 0;
        gbc_scrollPane.gridy = 0;
        lPanel.add(scrollPane, gbc_scrollPane);

        mList = new JList();
        scrollPane.setViewportView(mList);
        mList.setVisibleRowCount(20);
        mList.setModel(new LoginListModel());
        GridBagLayout gbl_rPanel = new GridBagLayout();
        gbl_rPanel.columnWidths = new int[]{452, 0};
        gbl_rPanel.rowHeights = new int[]{428, 0};
        gbl_rPanel.columnWeights = new double[]{1.0, Double.MIN_VALUE};
        gbl_rPanel.rowWeights = new double[]{1.0, Double.MIN_VALUE};
        rPanel.setLayout(gbl_rPanel);

        JScrollPane scrollPane_1 = new JScrollPane();
        scrollPane_1.setHorizontalScrollBarPolicy(ScrollPaneConstants.HORIZONTAL_SCROLLBAR_NEVER);
        GridBagConstraints gbc_scrollPane_1 = new GridBagConstraints();
        gbc_scrollPane_1.fill = GridBagConstraints.BOTH;
        gbc_scrollPane_1.gridx = 0;
        gbc_scrollPane_1.gridy = 0;
        rPanel.add(scrollPane_1, gbc_scrollPane_1);

        mTable = new JTable();

        mTable.addMouseListener(new MouseAdapter()
        {
            @Override
            public void mouseClicked(MouseEvent e)
            {
                if (e.getClickCount() == 2)
                {
                    JTable table = (JTable) e.getSource();
                    int row = table.rowAtPoint(e.getPoint()); //获得行位置
                    OnlinePlayerTableModel model = (OnlinePlayerTableModel) table.getModel();
                    long uid = (long) model.getValueAt(row, ETableColumns.COL_CID.ordinal());

                    ClientPlayer player = GamePlayerMgr.getInstance().lookupPlayerById(uid);
                    if (null == player)
                    {
                        JOptionPane.showMessageDialog(null, "can not find player of uid:" + uid, "WARNING_MESSAGE", JOptionPane.WARNING_MESSAGE);
                        return;
                    }
                    PlayerFrame frame = getPlayerFrame(player);

                    frame.setVisible(true);
                }

            }

            public void mouseReleased(MouseEvent event)
            {
                if (event.getButton() == MouseEvent.BUTTON3)
                {
                    ////////////menu1 全部登出
                    JPopupMenu popupMenu = new JPopupMenu();
                    // 增加菜单项到菜单上
                    JMenuItem item = new JMenuItem("全部登出");
                    item.addActionListener(new ActionListener()
                    {
                        public void actionPerformed(ActionEvent arg0)
                        {
                            logoutAllPlayers();
                        }

                    });
                    popupMenu.add(item);
                    //////////// menu2 复制cid////////////////////////////
                    JTable table = (JTable) event.getSource();
                    int row = table.rowAtPoint(event.getPoint()); //获得行位置
                    OnlinePlayerTableModel model = (OnlinePlayerTableModel) table.getModel();
                    long cid = (long) model.getValueAt(row, ETableColumns.COL_CID.ordinal());
                    // 增加菜单项到菜单上
                    JMenuItem item2 = new JMenuItem("复制cid");
                    item2.addActionListener(new ActionListener()
                    {
                        public void actionPerformed(ActionEvent arg0)
                        {
                            Clipboard clipboard = Toolkit.getDefaultToolkit().getSystemClipboard();  //得到系统剪贴板
                            StringSelection selection = new StringSelection("" + cid);
                            clipboard.setContents(selection, null);
                        }
                    });
                    popupMenu.add(item2);


                    popupMenu.show(event.getComponent(), event.getX(),
                            event.getY());
                }
            }


        });
        scrollPane_1.setViewportView(mTable);
        mTable.setBorder(new EmptyBorder(5, 5, 5, 5));
        mTable.setModel(new OnlinePlayerTableModel());
        mTable.setSelectionMode(ListSelectionModel.SINGLE_SELECTION);
        JSeparator separator = new JSeparator();
        separator.setForeground(Color.DARK_GRAY);
        GridBagConstraints gbc_separator = new GridBagConstraints();
        gbc_separator.fill = GridBagConstraints.HORIZONTAL;
        gbc_separator.insets = new Insets(0, 0, 5, 0);
        gbc_separator.gridx = 0;
        gbc_separator.gridy = 1;
        panel.add(separator, gbc_separator);

        JScrollPane scrollPane_2 = new JScrollPane();
        GridBagConstraints gbc_scrollPane_2 = new GridBagConstraints();
        gbc_scrollPane_2.fill = GridBagConstraints.BOTH;
        gbc_scrollPane_2.gridx = 0;
        gbc_scrollPane_2.gridy = 2;
        panel.add(scrollPane_2, gbc_scrollPane_2);

        _m_textArea = new JTextArea();
        scrollPane_2.setViewportView(_m_textArea);
        _m_textArea.setText("12321321321\r\n123123\r\n123\r\n123\r\n123");
        _m_textArea.setRows(10);


        _m_Frame.pack();
        _m_Frame.setLocationRelativeTo(null);
        _m_Frame.setVisible(true);

        //reg hander
        GamePlayerMgr.getInstance().onPlayerLoginGame.addHandler(null, new HandlerOne<ClientPlayer>()
        {
            @Override
            public void handle(ClientPlayer _player)
            {
                _listenOnPlayerLoginGame(_player);
            }
        });
        GamePlayerMgr.getInstance().onPlayerLogouted.addHandler(null, new HandlerOne<ClientPlayer>()
        {
            @Override
            public void handle(ClientPlayer _player)
            {
                _listenOnPlayerLogout(_player);

            }
        });
        _AWCGProtoLogger.setLogger(new ClientProtoLogger());
        CommLog.regAppender(this);
        _m_timer = new javax.swing.Timer(500, this);
        _m_timer.start();

        updateTitle();
    }

    /******
     * 根据输入的类名，生成热更类。class
     * @param strClassName
     */
    private RunResult genClassHotFixString(String projectName, String strClassName)
    {
        String rootPath = "../" + projectName + "/bin";

        String filePath = rootPath + "/" + strClassName.replace('.', '/') + ".class";

        byte[] bytesFromFile;
        try
        {
            FileInputStream is = new FileInputStream(filePath);
            bytesFromFile = new byte[is.available()];
            is.read(bytesFromFile);
            is.close();
        } catch (Exception e)
        {
            String msg = String.format("[ERROR]read file:%s failed,exception:%s", filePath, e.getMessage());
            return RunResult.failed(msg);
        }
        try
        {
            byte[] ziped = Zipper.zip(bytesFromFile);
            String encodeStr = Base64.getEncoder().encodeToString(ziped);
            String command = "hot loadClassFromBytes " + strClassName + " " + encodeStr;
            return RunResult.succ(command);
        } catch (Exception e)
        {
            return RunResult.failed(e.getMessage());
        }


    }

    protected void _listenOnPlayerLogout(ClientPlayer _player)
    {
        synchronized (this)
        {
            OnlinePlayerTableModel tableModel = (OnlinePlayerTableModel) mTable.getModel();
            tableModel.removeRowByUid(_player.getCid());
            CommLog.info("table rowcount ={}", tableModel.getRowCount());

            mPlayerFrames.remove(_player);
        }
        SwingUtilities.invokeLater(new Runnable()
        {
            @Override
            public void run()
            {
                mTable.updateUI();
            }
        });

    }

    protected void startKeepOnlineLogic(int _num, int _tickAddNum, EClientPlayerType _eTestType)
    {
        _m_StressPendingNum = _num;
        _m_iTickAddNum = _tickAddNum;
        _m_eTestType = _eTestType;
        _m_bStresssRuning = true;
        ALSynTaskManager.getInstance().regTask(this, 50);

    }

    private void logoutAllPlayers()
    {
        ArrayList<ClientPlayer> allPlayers = new ArrayList<>();
        GamePlayerMgr.getInstance().getAllPlayers(allPlayers);
        for (ClientPlayer player : allPlayers)
        {
            player.logout();
        }


    }

    protected void addSpecifiedPlayer(String playerName)
    {
        String name = playerName;
        String password = name;

        addPlayerCore(name, password);

    }

    public void dealyUpdateLoginListUI()
    {
        ALSynTaskManager.getInstance().regTask(new _IALSynTask()
        {
            @Override
            public void run()
            {
                _m_delayExecuter.delayExecute(() -> SwingUtilities.invokeLater(new Runnable()
                {
                    @Override
                    public void run()
                    {
                        mList.updateUI();
                    }
                }));
            }
        });

    }

    public ClientPlayer addPlayerCore(String name, String password)
    {
        ClientPlayer player = GamePlayerMgr.getInstance().lookupPlayerByName(name);
        if (null != player)
        {
            return null;
        }

        LoginListModel model = (LoginListModel) this.mList.getModel();
        LoginEntry entry = model.addEntry(name, name);
        dealyUpdateLoginListUI();
        player = new ClientPlayer(name, password);
        player.login(GConfigure.getInstance().getServerIP(), GConfigure.getInstance().getPort());
        player.OnLsDisconnected.addHandler(null, new HandlerNone()
        {
            @Override
            public void handle()
            {
                model.removeEntry(entry);
                dealyUpdateLoginListUI();
            }
        });
        player.OnLsLoginSucc.addHandler(null, new HandlerNone()
        {
            @Override
            public void handle()
            {
                entry.isLogined = true;
                dealyUpdateLoginListUI();
            }
        });
        player.OnLsConnectFailed.addHandler(null, new HandlerNone()
        {
            @Override
            public void handle()
            {
                model.removeEntry(entry);
                dealyUpdateLoginListUI();
            }
        });
        player.OnReceiveGateInfo.addHandler(null, new HandlerOne<Boolean>()
        {
            @Override
            public void handle(Boolean _bRemove)
            {
                entry.isLogined = true;
                if (_bRemove)
                {
                    model.removeEntry(entry);
                }
                dealyUpdateLoginListUI();
            }
        });

        return player;
    }

    public void onImportMatchTestLoginList()
    {

    }

    public void changePlayerLogic(String logicName)
    {
        GamePlayerMgr.getInstance().changeLogic(logicName);
    }

    private static int _g_num_index = 100000;

    public void addNumPlayer(int num)
    {
        for (int i = 0; i < num; i++)
        {
            _g_num_index++;
            updateTitle();

            addIndexedPlayer(_g_num_index);
        }
    }

    private void updateTitle()
    {
        String sTitle = String.format("Login[%s:%d]  UsId[%d]  Index[%d] "
                , GConfigure.getInstance().getServerIP()
                , GConfigure.getInstance().getPort()
                , GlobalStorage.getInstance().getRecentServerId()
                , _g_num_index);
        _m_Frame.setTitle(sTitle);

    }

    public ClientPlayer addIndexedPlayer(long index)
    {
        String name = "player_" + index;
        String password = name;

        return addPlayerCore(name, password);
    }

    private PlayerFrame getPlayerFrame(ClientPlayer player)
    {
        PlayerFrame frame = this.mPlayerFrames.get(player);
        if (null == frame)
        {
            frame = new PlayerFrame(player);
            frame.setSize(700, 500);
            frame.setLocationRelativeTo(null);
            this.mPlayerFrames.put(player, frame);
        }
        return frame;

    }

    public void _listenOnPlayerLoginGame(final ClientPlayer _player)
    {
        synchronized (this)
        {
            LoginListModel model = (LoginListModel) mList.getModel();
            for (int i = 0; i < model.getSize(); i++)
            {
                LoginEntry entry = (LoginEntry) model.getElementAt(i);
                if (entry.name.compareToIgnoreCase(_player.getName()) == 0)
                {
                    model.removeAt(i);
                }
            }

            PlayerTableRowData row = new PlayerTableRowData();
            row.initRow(_player);

            OnlinePlayerTableModel tableModel = (OnlinePlayerTableModel) mTable.getModel();
            tableModel.addRow(row);
        }
        SwingUtilities.invokeLater(new Runnable()
        {
            @Override
            public void run()
            {
                mTable.updateUI();
            }
        });
        dealyUpdateLoginListUI();
        _player.OnParamsInited.addHandler(null, new HandlerNone()
        {
            @Override
            public void handle()
            {
                SwingUtilities.invokeLater(new Runnable()
                {

                    @Override
                    public void run()
                    {
                        OnlinePlayerTableModel tableModel = (OnlinePlayerTableModel) mTable.getModel();
                        PlayerTableRowData row = tableModel.lookupRowByUid(_player.getCid());
                        if (null != row)
                        {
                            row.initParams(_player);
                            mTable.updateUI();
                        }

                    }
                });
            }
        });
        _player.OnParamUpdateOne.addHandler(null, new HandlerOne<ENPPlayerParam>()
        {
            @Override
            public void handle(final ENPPlayerParam eParam)
            {
                SwingUtilities.invokeLater(new Runnable()
                {
                    @Override
                    public void run()
                    {
                        OnlinePlayerTableModel tableModel = (OnlinePlayerTableModel) mTable.getModel();
                        PlayerTableRowData row = tableModel.lookupRowByUid(_player.getCid());
                        if (null != row)
                        {
                            row.updateParam(eParam, _player.getParam(eParam));
                            mTable.updateUI();
                        }

                    }
                });

            }
        });

        getPlayerFrame(_player);

    }

    private void runStressProcess(long startId, int playerNum, EClientPlayerType _playerType)
    {
        for (int i = 0; i < playerNum; i++)
        {
            ClientPlayer player = addIndexedPlayer(startId + i);
            player.setPlayerType(_playerType);
        }
    }

    @Override
    public void append(String _log)
    {
        synchronized (this)
        {
            _m_lLogList.addLast(_log);
            if (_m_lLogList.size() > 500)
            {
                _m_lLogList.removeFirst();
            }
            _m_logChged = true;
        }


    }

    @Override
    public void actionPerformed(ActionEvent arg0) //定时500ms任务
    {
        StringBuilder sb = new StringBuilder();

        synchronized (this)
        {
            if (_m_logChged)
            {
                for (String str : _m_lLogList)
                {
                    sb.append(str);
                    sb.append("\n");
                }
                _m_logChged = false;
                _m_textArea.setText(sb.toString());
            }
        }
        //sb.append(LoginStat.getInstance().toString());

    }

    @Override
    public void run()
    {
        if (!_m_bStresssRuning)
            return;
        _runKeepOnlineLogic();
        if (_m_StressPendingNum > 0)
            ALSynTaskManager.getInstance().regTask(this, 50);
    }

    private void _runKeepOnlineLogic()
    {
        LoginListModel model = (LoginListModel) this.mList.getModel();
        int pendingCount = model.getSize();
        if (pendingCount < _m_StressPendingNum)
        {
            for (int i = 0; i < _m_iTickAddNum; i++)
            {
                ClientPlayer player = addIndexedPlayer(++_g_num_index);
                player.setPlayerType(_m_eTestType);
                if (null != player)
                {
                    player.addLoginCommands(new String[]{
                            "player sendInit",
                    });
                }
            }
            updateTitle();
        }


    }


}
