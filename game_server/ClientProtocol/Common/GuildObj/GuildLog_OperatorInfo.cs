using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.GuildObj
{

/// <summary>
/// 联盟操作者信息
/// </summary>
public class GuildLog_OperatorInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 操作者名
/// </summary>
private string operatorName;
/// <summary>
/// 操作者职位
/// </summary>
private Common.GuildEnum.EGuildPositionType operatorPosition;


public GuildLog_OperatorInfo() {
	operatorName = "";
	operatorPosition = 0;
}

public GuildLog_OperatorInfo(
	string _operatorName
	, Common.GuildEnum.EGuildPositionType _operatorPosition
) {	operatorName = _operatorName;
	operatorPosition = _operatorPosition;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 操作者名
/// </summary>
public string getOperatorName() { return operatorName; }
/// <summary>
/// 操作者名
/// </summary>
public void setOperatorName(string _operatorName) { operatorName = _operatorName; }
/// <summary>
/// 操作者职位
/// </summary>
public Common.GuildEnum.EGuildPositionType getOperatorPosition() { return operatorPosition; }
/// <summary>
/// 操作者职位
/// </summary>
public void setOperatorPosition(Common.GuildEnum.EGuildPositionType _operatorPosition) { operatorPosition = _operatorPosition; }


public int GetBufSize() {
	int _size = 4;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(operatorName);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 6;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(operatorName);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	operatorName = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	operatorPosition = (Common.GuildEnum.EGuildPositionType)_buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putString(operatorName);
	_buf.putInt((int)operatorPosition);

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
	builder.Append("operatorName").Append(":").Append(operatorName.ToString()).Append(", ");
	builder.Append("operatorPosition").Append(":").Append(operatorPosition.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

