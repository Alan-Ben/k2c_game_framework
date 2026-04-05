using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p034_InnOp
{

/// <summary>
/// 旅店客人变更
/// </summary>
public class GS2GC_034_060_OnInnGuestChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 客人信息
/// </summary>
private Common.InnObj.Inn_GuestInfo guestInfo;


public GS2GC_034_060_OnInnGuestChg() {
	guestInfo = new Common.InnObj.Inn_GuestInfo();
}

public GS2GC_034_060_OnInnGuestChg(
	Common.InnObj.Inn_GuestInfo _guestInfo
) {	guestInfo = _guestInfo;
}

public byte getMainOrder() { return (byte)34; }

public byte getSubOrder() { return (byte)60; }

/// <summary>
/// 客人信息
/// </summary>
public Common.InnObj.Inn_GuestInfo getGuestInfo() { return guestInfo; }
/// <summary>
/// 客人信息
/// </summary>
public void setGuestInfo(Common.InnObj.Inn_GuestInfo _guestInfo) { guestInfo = _guestInfo; }


public int GetBufSize() {
	int _size = 21;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 23;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _guestInfoCustLen = _buf.getInt();
	int _guestInfoCurPos = _buf.getCurPos();
	guestInfo.ReadUnzipBuf(_buf, _guestInfoCurPos + _guestInfoCustLen);
	_buf.setPosition(_guestInfoCurPos + _guestInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(guestInfo.GetBufSize());
	guestInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)34);
	_buf.put((byte)60);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)34);
	_recBuf.put((byte)60);
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
	builder.Append("guestInfo").Append(":").Append(guestInfo == null ? "null" : guestInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

