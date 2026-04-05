using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p004_PlayerOp
{

public class GS2GC_004_011_RetSomeOnePlayerBriefInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 玩家简要信息
/// </summary>
private NPCommon.PlayerInfo_IconShow playerBrief;


public GS2GC_004_011_RetSomeOnePlayerBriefInfo() {
	playerBrief = new NPCommon.PlayerInfo_IconShow();
}

public GS2GC_004_011_RetSomeOnePlayerBriefInfo(
	NPCommon.PlayerInfo_IconShow _playerBrief
) {	playerBrief = _playerBrief;
}

public byte getMainOrder() { return (byte)4; }

public byte getSubOrder() { return (byte)11; }

/// <summary>
/// 玩家简要信息
/// </summary>
public NPCommon.PlayerInfo_IconShow getPlayerBrief() { return playerBrief; }
/// <summary>
/// 玩家简要信息
/// </summary>
public void setPlayerBrief(NPCommon.PlayerInfo_IconShow _playerBrief) { playerBrief = _playerBrief; }


public int GetBufSize() {
	int _size = 0;
	_size += 4 + playerBrief.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + playerBrief.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _playerBriefCustLen = _buf.getInt();
	int _playerBriefCurPos = _buf.getCurPos();
	playerBrief.ReadUnzipBuf(_buf, _playerBriefCurPos + _playerBriefCustLen);
	_buf.setPosition(_playerBriefCurPos + _playerBriefCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(playerBrief.GetBufSize());
	playerBrief.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)11);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)11);
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
	builder.Append("playerBrief").Append(":").Append(playerBrief == null ? "null" : playerBrief.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

