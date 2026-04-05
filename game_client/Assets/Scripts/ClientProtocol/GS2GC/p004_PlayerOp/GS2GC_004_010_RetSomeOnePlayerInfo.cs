using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p004_PlayerOp
{

public class GS2GC_004_010_RetSomeOnePlayerInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 玩家展示信息
/// </summary>
private Common.NpPlayerInfoObj.PlayerInfo_CommonShow someOneShowInfo;


public GS2GC_004_010_RetSomeOnePlayerInfo() {
	someOneShowInfo = new Common.NpPlayerInfoObj.PlayerInfo_CommonShow();
}

public GS2GC_004_010_RetSomeOnePlayerInfo(
	Common.NpPlayerInfoObj.PlayerInfo_CommonShow _someOneShowInfo
) {	someOneShowInfo = _someOneShowInfo;
}

public byte getMainOrder() { return (byte)4; }

public byte getSubOrder() { return (byte)10; }

/// <summary>
/// 玩家展示信息
/// </summary>
public Common.NpPlayerInfoObj.PlayerInfo_CommonShow getSomeOneShowInfo() { return someOneShowInfo; }
/// <summary>
/// 玩家展示信息
/// </summary>
public void setSomeOneShowInfo(Common.NpPlayerInfoObj.PlayerInfo_CommonShow _someOneShowInfo) { someOneShowInfo = _someOneShowInfo; }


public int GetBufSize() {
	int _size = 0;
	_size += 4 + someOneShowInfo.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + someOneShowInfo.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _someOneShowInfoCustLen = _buf.getInt();
	int _someOneShowInfoCurPos = _buf.getCurPos();
	someOneShowInfo.ReadUnzipBuf(_buf, _someOneShowInfoCurPos + _someOneShowInfoCustLen);
	_buf.setPosition(_someOneShowInfoCurPos + _someOneShowInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(someOneShowInfo.GetBufSize());
	someOneShowInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)10);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)10);
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
	builder.Append("someOneShowInfo").Append(":").Append(someOneShowInfo == null ? "null" : someOneShowInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

