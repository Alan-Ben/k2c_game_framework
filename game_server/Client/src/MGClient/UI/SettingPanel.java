package MGClient.UI;


import MGClient.GConfigure;
import NPCommon.Util.CommonFunc;

import javax.swing.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;

@SuppressWarnings("serial")
public class SettingPanel extends JFrame
{
    private JTextField textField;

    public SettingPanel()
    {
        setAlwaysOnTop(true);
        setType(Type.POPUP);
        setTitle("设置,可以指定端口号");
        getContentPane().setLayout(null);

        textField = new JTextField();
        textField.setText(GConfigure.getInstance().getServerIP() + ":" + GConfigure.getInstance().getPort());
        textField.setBounds(220, 53, 150, 21);
        getContentPane().add(textField);
        textField.setColumns(10);

        JButton btnNewButton = new JButton("确定");
        btnNewButton.addActionListener(new ActionListener()
        {
            public void actionPerformed(ActionEvent arg0)
            {
                String str = textField.getText().trim();
                if (str.isEmpty())
                    return;
                String[] strs = CommonFunc.charSplit(str, ':');
                int port = strs.length > 1 ? Integer.parseInt(strs[1]) : 5101;
                GConfigure.getInstance().setServerIP(strs[0], port);
                setVisible(false);
            }
        });
        btnNewButton.setBounds(181, 99, 93, 23);
        getContentPane().add(btnNewButton);

        JLabel lblNewLabel = new JLabel("服务器地址:");
        lblNewLabel.setBounds(139, 56, 71, 15);
        getContentPane().add(lblNewLabel);

    }
}
