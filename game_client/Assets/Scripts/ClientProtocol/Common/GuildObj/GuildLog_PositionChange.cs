using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.GuildObj
{

/// <summary>
/// 联盟职位变更
/// </summary>
public class GuildLog_PositionChange : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 操作者信息
/// </summary>
private Common.GuildObj.GuildLog_OperatorInfo operatorInfo;
/// <summary>
/// 目标成员名
/// </summary>
private string targetPlayerName;
/// <summary>
/// 目标职位
/// </summary>
private Common.GuildEnum.EGuildPositionType targetPosition;


public GuildLog_PositionChange() {
	operatorInfo = new Common.GuildObj.GuildLog_OperatorInfo();
	targetPlayerName = "";
	targetPosition = 0;
}

public GuildLog_PositionChange(
	Common.GuildObj.GuildLog_OperatorInfo _operatorInfo
	, string _targetPlayerName
	, Common.GuildEnum.EGuildPositionType _targetPosition
) {	operatorInfo = _operatorInfo;
	targetPlayerName = _targetPlayerName;
	targetPosition = _targetPosition;
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
/// <summary>
/// 目标职位
/// </summary>
public Common.GuildEnum.EGuildPositionType getTargetPosition() { return targetPosition; }
/// <summary>
/// 目标职位
/// </summary>
public void setTargetPosition(Common.GuildEnum.EGuildPositionType _targetPosition) { targetPosition = _targetPosition; }


public int GetBufSize() {
	int _size = 4;
	_size += 4 + operatorInfo.GetBufSize();
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(targetPlayerName);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 6;
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
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	targetPosition = (Common.GuildEnum.EGuildPositionType)_buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(operatorInfo.GetBufSize());
	operatorInfo.PutUnzipBuf(_buf);
	_buf.putString(targetPlayerName);
	_buf.putInt((int)targetPosition);

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
	builder.Append("targetPosition").Append(":").Append(targetPosition.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

