package MGClient.UI;

import ALBasicServer.ALBasicMutex.MutexObject;
import MGClient.ClientPlayer.ClientPlayer;
import NPCommon.Util.CallBack._ICallBackT;
import NPCommon.Util.Delegate.HandlerOne;
import com.google.gson.JsonArray;
import com.google.gson.JsonObject;

import javax.swing.*;
import javax.swing.border.EmptyBorder;
import java.awt.*;
import java.awt.datatransfer.Clipboard;
import java.awt.datatransfer.StringSelection;
import java.awt.event.*;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Base64;
import java.util.Comparator;

public class PlayerFrame extends JFrame
{
    /**
     *
     */
    private static final long serialVersionUID = 1L;
    private JTextField txtInput;
    private JTextArea txtOutPut;
    private JTextArea txtProtoLog;
    private ClientPlayer mPlayer;
    private ArrayList<String> _mProtoLogs = new ArrayList<String>();
    private ArrayList<String> _mOutPutLines = new ArrayList<String>();
    MutexObject _m_locker = new MutexObject();
    private JTextField textServerCmd;
    public ClientPlayer getPlayer(){ return mPlayer;}

    public PlayerFrame(ClientPlayer _player)
    {
        mPlayer = _player;
        this.setTitle(_player.getName() + "(" + _player.getCid() + ")");

        //菜单栏
        JMenuBar menuBar = new JMenuBar();
        this.setJMenuBar(menuBar);

        //增加按钮
        JMenu clashMenu = new JMenu("命令");
        menuBar.add(clashMenu);

        JMenuItem itemGenBase64 = new JMenuItem("生成Base64命令");
        itemGenBase64.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e)
            {
                getPlayer().getCmdList("help", new _ICallBackT<String[]>()
                {
                    @Override
                    public void onRunOver(String[] _commandArgs)
                    {
                        String[] sortedArgs = Arrays.stream(_commandArgs)
                                .sorted(Comparator.comparingInt(o -> o.charAt(0)))
                                .toArray(String[]::new);

                        String rawCommandType = PromptSelect.prompt("命令类型", sortedArgs);
                        if (rawCommandType == null || rawCommandType.isEmpty())
                            return;

                        String[] commandArgs = rawCommandType.split(":");
                        if (commandArgs.length < 1)
                            return;

                        String commandType = commandArgs[0];
                        getPlayer().getCmdList(commandType, new _ICallBackT<String[]>()
                        {
                            @Override
                            public void onRunOver(String[] _commandArgs)
                            {
                                String rawFuncType = PromptSelect.prompt("功能类型", 1200, 100, _commandArgs);
                                if (rawFuncType == null || rawFuncType.isEmpty())
                                    return;

                                String[] rawFuncArgs = rawFuncType.split("\\|");

                                String funcLine = rawFuncArgs[0].trim();
                                String[] funcArgs = funcLine.split("\\s+");
                                String funcType;
                                String command;

                                //处理没有参数的情况
                                if (funcArgs.length <= 1)
                                {
                                    funcType = funcArgs[0];
                                    command = "base64 deal " + genCommandBase64(commandType, funcType, new JsonArray());
                                }else
                                {
                                    funcType = funcArgs[0];
                                    funcArgs = Arrays.copyOfRange(funcArgs, 1, funcArgs.length);

                                    JsonArray strArray = PromptInputList.promptJsonArray(rawFuncType, funcArgs.length);
                                    command = "base64 deal " + genCommandBase64(commandType, funcType, strArray);
                                }

                                Clipboard clipboard = Toolkit.getDefaultToolkit().getSystemClipboard();  //得到系统剪贴板
                                StringSelection selection = new StringSelection(command);
                                clipboard.setContents(selection, null);
                            }
                        });
                    }
                });
            }
        });
        clashMenu.add(itemGenBase64);

        //
        GridBagLayout gridBagLayout = new GridBagLayout();
        gridBagLayout.columnWidths = new int[]{434, 0};
        gridBagLayout.rowHeights = new int[]{228, 23, 23, 0};
        gridBagLayout.columnWeights = new double[]{1.0, Double.MIN_VALUE};
        gridBagLayout.rowWeights = new double[]{1.0, 0.0, 0.0, Double.MIN_VALUE};
        getContentPane().setLayout(gridBagLayout);
        JPanel panel = new JPanel();
        GridBagConstraints gbc_panel = new GridBagConstraints();
        gbc_panel.fill = GridBagConstraints.BOTH;
        gbc_panel.insets = new Insets(0, 0, 5, 0);
        gbc_panel.gridx = 0;
        gbc_panel.gridy = 0;
        getContentPane().add(panel, gbc_panel);
        panel.setBorder(new EmptyBorder(10, 10, 10, 10));
        panel.setLayout(new BoxLayout(panel, BoxLayout.X_AXIS));
        JSplitPane splitPane_1 = new JSplitPane();
        panel.add(splitPane_1);
        JPanel panel_2 = new JPanel();
        splitPane_1.setLeftComponent(panel_2);
        GridBagLayout gbl_panel_2 = new GridBagLayout();
        gbl_panel_2.columnWidths = new int[]{6, 0};
        gbl_panel_2.rowHeights = new int[]{26, 0};
        gbl_panel_2.columnWeights = new double[]{1.0, Double.MIN_VALUE};
        gbl_panel_2.rowWeights = new double[]{1.0, Double.MIN_VALUE};
        panel_2.setLayout(gbl_panel_2);
        JScrollPane scrollPane_1 = new JScrollPane();
        GridBagConstraints gbc_scrollPane_1 = new GridBagConstraints();
        gbc_scrollPane_1.fill = GridBagConstraints.BOTH;
        gbc_scrollPane_1.gridx = 0;
        gbc_scrollPane_1.gridy = 0;
        panel_2.add(scrollPane_1, gbc_scrollPane_1);
        txtProtoLog = new JTextArea();
        txtProtoLog.addMouseListener(new MouseAdapter()
        {
            @Override
            public void mouseClicked(MouseEvent arg0)
            {
                if (arg0.getButton() == 3)
                {
                    clearProtoOutPutUI();
                }
            }
        });

        txtProtoLog.setFont(new Font("微软雅黑", Font.PLAIN, 13));
        txtProtoLog.setForeground(Color.BLACK);
        txtProtoLog.setColumns(30);
        txtProtoLog.setBackground(SystemColor.controlHighlight);
        txtProtoLog.setRows(10);
        scrollPane_1.setViewportView(txtProtoLog);
        JPanel panel_3 = new JPanel();
        splitPane_1.setRightComponent(panel_3);
        GridBagLayout gbl_panel_3 = new GridBagLayout();
        gbl_panel_3.columnWidths = new int[]{119, 0};
        gbl_panel_3.rowHeights = new int[]{24, 0};
        gbl_panel_3.columnWeights = new double[]{1.0, Double.MIN_VALUE};
        gbl_panel_3.rowWeights = new double[]{1.0, Double.MIN_VALUE};
        panel_3.setLayout(gbl_panel_3);
        JScrollPane scrollPane = new JScrollPane();
        GridBagConstraints gbc_scrollPane = new GridBagConstraints();
        gbc_scrollPane.fill = GridBagConstraints.BOTH;
        gbc_scrollPane.gridx = 0;
        gbc_scrollPane.gridy = 0;
        panel_3.add(scrollPane, gbc_scrollPane);
        txtOutPut = new JTextArea();
        txtOutPut.setFont(new Font("微软雅黑", Font.PLAIN, 13));
        txtOutPut.setEditable(false);
        txtOutPut.setBackground(Color.WHITE);
        txtOutPut.setForeground(Color.BLACK);
        scrollPane.setViewportView(txtOutPut);
        txtOutPut.setRows(10);

        JPanel panel_1 = new JPanel();
        GridBagConstraints gbc_panel_1 = new GridBagConstraints();
        gbc_panel_1.insets = new Insets(0, 0, 5, 0);
        gbc_panel_1.anchor = GridBagConstraints.NORTH;
        gbc_panel_1.fill = GridBagConstraints.HORIZONTAL;
        gbc_panel_1.gridx = 0;
        gbc_panel_1.gridy = 1;
        getContentPane().add(panel_1, gbc_panel_1);
        panel_1.setBorder(new EmptyBorder(5, 5, 5, 5));
        GridBagLayout gbl_panel_1 = new GridBagLayout();
        gbl_panel_1.columnWidths = new int[]{48};
        gbl_panel_1.rowHeights = new int[]{23, 0};
        gbl_panel_1.columnWeights = new double[]{1.0, 0.0};
        gbl_panel_1.rowWeights = new double[]{0.0, Double.MIN_VALUE};
        panel_1.setLayout(gbl_panel_1);
        txtInput = new JTextField();
        txtInput.setFont(new Font("微软雅黑", Font.PLAIN, 12));
        txtInput.addActionListener(new ActionListener()
        {
            public void actionPerformed(ActionEvent arg0)
            {
                runCommand();

            }
        });
        txtInput.setText("");
        txtInput.addInputMethodListener(new InputMethodListener()
        {
            public void caretPositionChanged(InputMethodEvent arg0)
            {
            }

            public void inputMethodTextChanged(InputMethodEvent arg0)
            {
            }
        });
        GridBagConstraints gbc_txtInput = new GridBagConstraints();
        gbc_txtInput.fill = GridBagConstraints.BOTH;
        gbc_txtInput.insets = new Insets(0, 0, 0, 5);
        gbc_txtInput.gridx = 0;
        gbc_txtInput.gridy = 0;
        panel_1.add(txtInput, gbc_txtInput);

        JButton txtOK = new JButton("客户端指令");
        txtOK.addMouseListener(new MouseAdapter()
        {
            @Override
            public void mouseClicked(MouseEvent arg0)
            {
                runCommand();
            }
        });

        GridBagConstraints gbc_txtOK = new GridBagConstraints();
        gbc_txtOK.anchor = GridBagConstraints.NORTHWEST;
        gbc_txtOK.gridx = 1;
        gbc_txtOK.gridy = 0;
        panel_1.add(txtOK, gbc_txtOK);

        JPanel panel_4 = new JPanel();
        panel_4.setBorder(new EmptyBorder(5, 5, 5, 5));
        GridBagConstraints gbc_panel_4 = new GridBagConstraints();
        gbc_panel_4.anchor = GridBagConstraints.NORTH;
        gbc_panel_4.fill = GridBagConstraints.HORIZONTAL;
        gbc_panel_4.gridx = 0;
        gbc_panel_4.gridy = 2;
        getContentPane().add(panel_4, gbc_panel_4);
        GridBagLayout gbl_panel_4 = new GridBagLayout();
        gbl_panel_4.columnWidths = new int[]{48, 0, 0};
        gbl_panel_4.rowHeights = new int[]{23, 0};
        gbl_panel_4.columnWeights = new double[]{1.0, 0.0, Double.MIN_VALUE};
        gbl_panel_4.rowWeights = new double[]{0.0, Double.MIN_VALUE};
        panel_4.setLayout(gbl_panel_4);

        textServerCmd = new JTextField();
        textServerCmd.setText("");
        textServerCmd.setFont(new Font("微软雅黑", Font.PLAIN, 12));
        textServerCmd.addActionListener(new ActionListener()
        {
            public void actionPerformed(ActionEvent arg0)
            {
                runServerCommand();
            }
        });
        GridBagConstraints gbc_textServerCmd = new GridBagConstraints();
        gbc_textServerCmd.fill = GridBagConstraints.BOTH;
        gbc_textServerCmd.insets = new Insets(0, 0, 0, 5);
        gbc_textServerCmd.gridx = 0;
        gbc_textServerCmd.gridy = 0;
        panel_4.add(textServerCmd, gbc_textServerCmd);

        JButton btServerCmd = new JButton("服务器指令");
        btServerCmd.addActionListener(new ActionListener()
        {
            public void actionPerformed(ActionEvent e)
            {
                runServerCommand();
            }
        });
        GridBagConstraints gbc_btServerCmd = new GridBagConstraints();
        gbc_btServerCmd.anchor = GridBagConstraints.NORTHWEST;
        gbc_btServerCmd.gridx = 1;
        gbc_btServerCmd.gridy = 0;
        panel_4.add(btServerCmd, gbc_btServerCmd);


        mPlayer.onProtoArrived.addHandler(null, new HandlerOne<String>()
        {

            @Override
            public void handle(String s)
            {

                _mProtoLogs.add(s);
                if (_mProtoLogs.size() > 100)
                {
                    _mProtoLogs.remove(0);
                }
                updateProtoOutPutUI();

            }

        });
    }

    protected void runServerCommand()
    {
        String sCommand = textServerCmd.getText().trim();
        if (sCommand.isEmpty())
            return;
        addOutPut(sCommand);
        mPlayer.runGm(sCommand);

    }

    private void clearProtoOutPutUI()
    {
        _mProtoLogs.clear();
        updateProtoOutPutUI();

    }

    private void updateProtoOutPutUI()
    {
        StringBuilder sb = new StringBuilder();
        for (int ii = 0; ii < _mProtoLogs.size(); ii++)
        {
            sb.append(_mProtoLogs.get(ii));
            sb.append("\n");
        }
        SwingUtilities.invokeLater(new Runnable()
        {
            @Override
            public void run()
            {
                txtProtoLog.setText(sb.toString());
            }
        });
    }

    private void updateOutPutUI()
    {
        StringBuilder sb = new StringBuilder();
        for (int ii = 0; ii < _mOutPutLines.size(); ii++)
        {
            sb.append(_mOutPutLines.get(ii));
            sb.append("\n");
        }
        SwingUtilities.invokeLater(new Runnable()
        {
            @Override
            public void run()
            {
                txtOutPut.setText(sb.toString());
            }
        });
    }

    private void runCommand()
    {
        String sCommand = txtInput.getText();
        if (sCommand.isEmpty())
            return;
        addOutPut(sCommand);
        String sResult = mPlayer.runCmd(sCommand);
        addOutPut(sResult);

        mPlayer.logProto("=====" + sCommand + "====");
    }

    private void addOutPut(String _line)
    {
        _mOutPutLines.add(_line);
        if (_mOutPutLines.size() > 100)
        {
            _mOutPutLines.remove(0);
        }
        updateOutPutUI();

    }

    /**
     * 使用json生成命令
     * {
     *     "cmdType": "test",
     *     "funcType": "addInt",
     *     "params": [
     *         1,
     *         2
     *     ]
     * }
     * @param _cmdType
     * @param _funcType
     * @param _args
     * @return
     */
    public String genCommandBase64(String _cmdType, String _funcType, JsonArray _args)
    {
        JsonObject jsonObject = new JsonObject();
        jsonObject.addProperty("cmdType", _cmdType);
        jsonObject.addProperty("funcType", _funcType);
        jsonObject.add("params", _args);
        // 使用Gson将JsonObject转换为JSON字符串，并进行Base64编码
        String jsonString = jsonObject.toString();
        return Base64.getEncoder().encodeToString(jsonString.getBytes());
    }
}
