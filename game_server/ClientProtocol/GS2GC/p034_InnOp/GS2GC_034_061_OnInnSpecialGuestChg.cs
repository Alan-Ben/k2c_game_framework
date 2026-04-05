using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p034_InnOp
{

/// <summary>
/// 旅店特殊客人变更
/// </summary>
public class GS2GC_034_061_OnInnSpecialGuestChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 特殊客人信息
/// </summary>
private Common.InnObj.Inn_SpecialGuestInfo specialGuestInfo;


public GS2GC_034_061_OnInnSpecialGuestChg() {
	specialGuestInfo = new Common.InnObj.Inn_SpecialGuestInfo();
}

public GS2GC_034_061_OnInnSpecialGuestChg(
	Common.InnObj.Inn_SpecialGuestInfo _specialGuestInfo
) {	specialGuestInfo = _specialGuestInfo;
}

public byte getMainOrder() { return (byte)34; }

public byte getSubOrder() { return (byte)61; }

/// <summary>
/// 特殊客人信息
/// </summary>
public Common.InnObj.Inn_SpecialGuestInfo getSpecialGuestInfo() { return specialGuestInfo; }
/// <summary>
/// 特殊客人信息
/// </summary>
public void setSpecialGuestInfo(Common.InnObj.Inn_SpecialGuestInfo _specialGuestInfo) { specialGuestInfo = _specialGuestInfo; }


public int GetBufSize() {
	int _size = 14;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 16;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _specialGuestInfoCustLen = _buf.getInt();
	int _specialGuestInfoCurPos = _buf.getCurPos();
	specialGuestInfo.ReadUnzipBuf(_buf, _specialGuestInfoCurPos + _specialGuestInfoCustLen);
	_buf.setPosition(_specialGuestInfoCurPos + _specialGuestInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(specialGuestInfo.GetBufSize());
	specialGuestInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)34);
	_buf.put((byte)61);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)34);
	_recBuf.put((byte)61);
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
	builder.Append("specialGuestInfo").Append(":").Append(specialGuestInfo == null ? "null" : specialGuestInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

