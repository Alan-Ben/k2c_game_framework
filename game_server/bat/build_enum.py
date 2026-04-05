# -*- coding: UTF-8 -*-
import os

#重新生成目标文件
if(os.path.exists('../ServerProtocol/ProtocolScripts/Enum/enum.txt')):
    os.remove('../ServerProtocol/ProtocolScripts/Enum/enum.txt')
#重新写入文件
fp = open('../ServerProtocol/ProtocolScripts/Enum/enum.txt', 'wb');

def gci(filepath):
    files = os.listdir(filepath)  
    for fi in files:    
        fi_d = os.path.join(filepath,fi)    
        if os.path.isdir(fi_d):
            gci(fi_d)    
        else:
            if '.txt' in fi:
                continue
            if 'Error' in fi:
                continue
            print(os.path.join(filepath,fi_d))
            #读入当前文件内容并写入
            tmpF = open(fi_d, 'rb')
            s = tmpF.read()
            fp.write(s)
            tmpF.close()
 
#遍历文件，并把内容写入指定文件中 
gci('../ServerProtocol/ProtocolScripts/Enum')
#gci('../NPCommon/src/NPCommon/Enum')
#关闭文件
fp.close()