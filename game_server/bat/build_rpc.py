# -*- coding: UTF-8 -*-
import os
import time

ProjectRootPath = {
    "CommonServer": "../CommonServer/src",
    "USServer": "../UserServer/src",
    "RoomServer": "../RoomServer/src",
    "LoginServer": "../LoginServer/src",
    "PlatServer": "../PlatServer/src",
    "GateServer": "../GatewayServer/src",
    "MarryMatchServer": "../MarryMatchServer/src",
}
rpcTempalte = open("./tempalte_rpc_class.java", "r", encoding="utf-8").read()
dealerTemplate = open("./tempalte_rpc_handler.java", "r", encoding="utf-8").read()


def createRpcClass(classPackage, className, protoPackage, classCommnet):
    path = "../Common/src/" + classPackage.replace(".", "/")

    if not os.path.exists(path):
        os.makedirs(path)  # 循环创建目录

    filePathName = path + "/" + className + ".java"
    classFile = open(filePathName, "wt", encoding="utf-8")
    txt = rpcTempalte.replace("<%ClassPackage%>", classPackage)
    txt = txt.replace("<%ClassName%>", className)
    txt = txt.replace("<%ProtoPackage%>", protoPackage)
    txt = txt.replace("<%ClassComment%>", classCommnet)
    classFile.write(txt)
    classFile.close()
    print("created rpc:%s" % filePathName)


def createRpcDelaer(dealerPackage, rpcPackage, rpcName, _comment):
    startPos = dealerPackage.find('.')  # 第一个.位置
    projectName = dealerPackage[0:startPos]  # 项目名
    delalerClassName = rpcName + "_Handler";  # 处理类的类名
    rootPath = ProjectRootPath.get(projectName)  # 文件根路径
    if rootPath is None:
        rootPath = "../%s/src" % projectName
    pathDir = rootPath + "/" + dealerPackage.replace(".", "/")  # 处理类所在文件夹
    dealerPathName = pathDir + "/" + delalerClassName + ".java"  # 处理类完整路径名

    # print "projectName="+projectName
    # print "dealerPackage="+dealerPackage
    # print "delalerClassName="+delalerClassName
    # print "rootPath="+rootPath
    # print "pathDir="+pathDir
    # print "dealerPathName="+dealerPathName;

    if not os.path.exists(pathDir):
        os.makedirs(pathDir)  # 循环创建目录

    if os.path.isfile(dealerPathName):
        print("ignore dealer:" + dealerPathName)
        return

    handlerFile = open(dealerPathName, "wt", encoding="utf-8")
    txt = dealerTemplate.replace("<%PackageName%>", dealerPackage)
    txt = txt.replace("<%RpcFullClassName%>", rpcPackage + "." + rpcName)
    txt = txt.replace("<%RpcName%>", rpcName)
    txt = txt.replace("<%RpcCommnet%>", _comment)
    handlerFile.write(txt)
    handlerFile.close()
    print("created dealer:%s" % dealerPathName)


def tryGenRpcCode(filePath, fileName):
    fileHandler = open(filePath, "r", encoding="utf-8")
    isRpc = False
    classPackage = ""
    dealerPackage = ""
    rpcName = ""
    protoPackage = ""
    classCommnet = ""
    for line in fileHandler.readlines():
        line = line.strip()
        if "JavaPackage" in line:  # 读取生成协议的Java路径
            protoPackage = line.split(' ')[1].strip()[:-1]
        if not isRpc:
            if "[RPC]" in line:
                isRpc = True
                classCommnet = line.strip()[7:]
            else:
                continue
        if isRpc:
            rpcName = fileName[0:-6]
            if "[CLASS_PACKAGE]" in line:
                classPackage = line.split(':')[1].strip()
            if "[DEALER_PACKAGE]" in line:
                dealerPackage = line.split(':')[1].strip()

    if not isRpc:
        return
    # print rpcName
    # print classPackage
    # print dealerPackage
    # print protoPackage
    print("----------------")
    createRpcClass(classPackage, rpcName, protoPackage, classCommnet)
    createRpcDelaer(dealerPackage, classPackage, rpcName, classCommnet)


def walkFile(dir):
    curDir = os.path.dirname(dir)
    for root, dirs, files in os.walk(curDir):
        for f in files:
            if f.endswith(".alpro"):
                tryGenRpcCode(os.path.join(root, f), f)
        for subDir in dirs:
            walkFile(subDir)


if __name__ == '__main__':
    walkFile("../ServerProtocol/ProtocolScripts/ALLRPC")
