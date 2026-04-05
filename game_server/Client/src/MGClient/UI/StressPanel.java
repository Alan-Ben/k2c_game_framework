package MGClient.UI;

import javax.swing.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;

@SuppressWarnings("serial")
public class StressPanel extends JDialog
{
    private JTextField txtStartID;
    private JTextField textNum;

    public StressPanel()
    {
        setAlwaysOnTop(true);
        setType(Type.POPUP);
        setTitle("设置");
        getContentPane().setLayout(null);

        txtStartID = new JTextField();
        txtStartID.setText("0");
        txtStartID.setBounds(89, 22, 86, 21);
        getContentPane().add(txtStartID);
        txtStartID.setColumns(10);

        JButton btnNewButton = new JButton("确定");
        btnNewButton.setBounds(159, 139, 93, 23);
        getContentPane().add(btnNewButton);
        btnNewButton.addActionListener(new ActionListener()
        {
            public void actionPerformed(ActionEvent arg0)
            {
                setVisible(false);
            }
        });

        JLabel lblNewLabel = new JLabel("开始ID:");
        lblNewLabel.setBounds(31, 25, 71, 15);
        getContentPane().add(lblNewLabel);

        JLabel label = new JLabel("数量:");
        label.setBounds(41, 53, 71, 15);
        getContentPane().add(label);

        textNum = new JTextField();
        textNum.setText("500");
        textNum.setColumns(10);
        textNum.setBounds(89, 53, 86, 21);
        getContentPane().add(textNum);


    }

    public long getStartId()
    {
        return Long.parseLong(txtStartID.getText().trim());
    }

    public int getPlayerNum()
    {
        return Integer.parseInt(textNum.getText().trim());
    }

}
