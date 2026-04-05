using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class MCC_Common_User : ALBasicProtocolPack._IALProtocolStructure {
private long uid;
private long gameUid;
private Common.MCC_Common_User_Extra extraInfo;


public MCC_Common_User() {
	uid = (long)0;
	gameUid = (long)0;
	extraInfo = new Common.MCC_Common_User_Extra();
}

public MCC_Common_User(
	long _uid
	, long _gameUid
	, Common.MCC_Common_User_Extra _extraInfo
) {	uid = _uid;
	gameUid = _gameUid;
	extraInfo = _extraInfo;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public long getUid() { return uid; }
public void setUid(long _uid) { uid = _uid; }
public long getGameUid() { return gameUid; }
public void setGameUid(long _gameUid) { gameUid = _gameUid; }
public Common.MCC_Common_User_Extra getExtraInfo() { return extraInfo; }
public void setExtraInfo(Common.MCC_Common_User_Extra _extraInfo) { extraInfo = _extraInfo; }


public int GetBufSize() {
	int _size = 16;
	_size += 4 + extraInfo.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 18;
	_size += 4 + extraInfo.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	uid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	gameUid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _extraInfoCustLen = _buf.getInt();
	int _extraInfoCurPos = _buf.getCurPos();
	extraInfo.ReadUnzipBuf(_buf, _extraInfoCurPos + _extraInfoCustLen);
	_buf.setPosition(_extraInfoCurPos + _extraInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(uid);
	_buf.putLong(gameUid);
	_buf.putInt(extraInfo.GetBufSize());
	extraInfo.PutUnzipBuf(_buf);
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
	builder.Append("uid").Append(":").Append(uid.ToString()).Append(", ");
	builder.Append("gameUid").Append(":").Append(gameUid.ToString()).Append(", ");
	builder.Append("extraInfo").Append(":").Append(extraInfo == null ? "null" : extraInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

