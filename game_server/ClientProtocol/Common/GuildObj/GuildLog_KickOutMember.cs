using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.GuildObj
{

/// <summary>
/// 联盟踢出成员
/// </summary>
public class GuildLog_KickOutMember : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 操作者信息
/// </summary>
private Common.GuildObj.GuildLog_OperatorInfo operatorInfo;
/// <summary>
/// 目标成员名
/// </summary>
private string targetPlayerName;


public GuildLog_KickOutMember() {
	operatorInfo = new Common.GuildObj.GuildLog_OperatorInfo();
	targetPlayerName = "";
}

public GuildLog_KickOutMember(
	Common.GuildObj.GuildLog_OperatorInfo _operatorInfo
	, string _targetPlayerName
) {	operatorInfo = _operatorInfo;
	targetPlayerName = _targetPlayerName;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 操作者信息
/// </summary>
public Common.GuildObj.GuildLog_OperatorInfo getOperatorInfo() { return operatorInfo; }
/// <summary>
/// 操作者信息
/// </summary>
public void setOperatorInfo(Common.GuildObj.GuildLog_OperatorInfo _operatorInfo) { operatorInfo = _operatorInfo; }
/// <summary>
/// 目标成员名
/// </summary>
public string getTargetPlayerName() { return targetPlayerName; }
/// <summary>
/// 目标成员名
/// </summary>
public void setTargetPlayerName(string _targetPlayerName) { targetPlayerName = _targetPlayerName; }


public int GetBufSize() {
	int _size = 0;
	_size += 4 + operatorInfo.GetBufSize();
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(targetPlayerName);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + operatorInfo.GetBufSize();
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(targetPlayerName);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _operatorInfoCustLen = _buf.getInt();
	int _operatorInfoCurPos = _buf.getCurPos();
	operatorInfo.ReadUnzipBuf(_buf, _operatorInfoCurPos + _operatorInfoCustLen);
	_buf.setPosition(_operatorInfoCurPos + _operatorInfoCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	targetPlayerName = _buf.getString();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(operatorInfo.GetBufSize());
	operatorInfo.PutUnzipBuf(_buf);
	_buf.putString(targetPlayerName);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
	PutUnzipBuf(_recBuf);
}
public byte[] makePackage() {
	int _bufSize = GetBufSize();
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void readPackage(byte[] _buf) {
	ALProtocolBuf _bufObj = new ALProtocolBuf(_buf);
	ReadUnzipBuf(_bufObj, -1);
}
public void readPackage(ALProtocolBuf _buf) {
	ReadUnzipBuf(_buf, -1);
}
public override string ToString() {
	System.Text.StringBuilder builder = new System.Text.StringBuilder();

	builder.Append("{");
	builder.Append("operatorInfo").Append(":").Append(operatorInfo == null ? "null" : operatorInfo.ToString()).Append(", ");
	builder.Append("targetPlayerName").Append(":").Append(targetPlayerName.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

