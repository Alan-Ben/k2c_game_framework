package MGClient.UI;

import javax.swing.*;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;

@SuppressWarnings("serial")
public class PromptInput extends javax.swing.JDialog
{
    private boolean _m_bIsOK = false;
    private JTextField _m_txtInput;

    public PromptInput()
    {
        getContentPane().setLayout(new FlowLayout(FlowLayout.LEFT, 10, 20));

        JLabel label = new JLabel("    输入：");
        getContentPane().add(label);

        _m_txtInput = new JTextField();
        getContentPane().add(_m_txtInput);
        _m_txtInput.setColumns(20);

        JLabel label_2 = new JLabel("");
        getContentPane().add(label_2);

        JButton button = new JButton("确定");
        button.addActionListener(new ActionListener()
        {
            public void actionPerformed(ActionEvent arg0)
            {
                _m_bIsOK = true;
                setVisible(false);
            }
        });
        getContentPane().add(button);

        JLabel label_5 = new JLabel("");
        getContentPane().add(label_5);
    }

    public String getInput()
    {

        return _m_txtInput.getText();
    }

    public static String prompt(String _title, String _default)
    {
        PromptInput dlg = new PromptInput();
        dlg.setModal(true);
        dlg.setSize(450, 100);
        dlg.setLocation(500, 500);
        dlg.init(_title, _default);
        dlg.setVisible(true);


        if (dlg._m_bIsOK)
        {
            return dlg.getInput();
        } else
        {
            return "";
        }
    }

    private void init(String _title, String _default)
    {
        this.setTitle(_title);
        _m_txtInput.setText(_default);
    }
}
