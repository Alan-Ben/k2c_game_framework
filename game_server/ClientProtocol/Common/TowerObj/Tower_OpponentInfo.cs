using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.TowerObj
{

/// <summary>
/// 爬塔_对手信息
/// </summary>
public class Tower_OpponentInfo : ALBasicProtocolPack._IALProtocolStructure {
private Common.TowerObj.Tower_PosInfo posInfo;
private long playerId;


public Tower_OpponentInfo() {
	posInfo = new Common.TowerObj.Tower_PosInfo();
	playerId = (long)0;
}

public Tower_OpponentInfo(
	Common.TowerObj.Tower_PosInfo _posInfo
	, long _playerId
) {	posInfo = _posInfo;
	playerId = _playerId;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public Common.TowerObj.Tower_PosInfo getPosInfo() { return posInfo; }
public void setPosInfo(Common.TowerObj.Tower_PosInfo _posInfo) { posInfo = _posInfo; }
public long getPlayerId() { return playerId; }
public void setPlayerId(long _playerId) { playerId = _playerId; }


public int GetBufSize() {
	int _size = 24;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 26;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _posInfoCustLen = _buf.getInt();
	int _posInfoCurPos = _buf.getCurPos();
	posInfo.ReadUnzipBuf(_buf, _posInfoCurPos + _posInfoCustLen);
	_buf.setPosition(_posInfoCurPos + _posInfoCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	playerId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(posInfo.GetBufSize());
	posInfo.PutUnzipBuf(_buf);
	_buf.putLong(playerId);
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
	builder.Append("posInfo").Append(":").Append(posInfo == null ? "null" : posInfo.ToString()).Append(", ");
	builder.Append("playerId").Append(":").Append(playerId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

