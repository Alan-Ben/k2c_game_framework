using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p021_PlayerInfo
{

public class GS2GC_021_051_OnAddNewLazyCd : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// cd信息
/// </summary>
private NPCommon.NPCommon_PlayerLazyCD cdInfo;


public GS2GC_021_051_OnAddNewLazyCd() {
	cdInfo = new NPCommon.NPCommon_PlayerLazyCD();
}

public GS2GC_021_051_OnAddNewLazyCd(
	NPCommon.NPCommon_PlayerLazyCD _cdInfo
) {	cdInfo = _cdInfo;
}

public byte getMainOrder() { return (byte)21; }

public byte getSubOrder() { return (byte)51; }

/// <summary>
/// cd信息
/// </summary>
public NPCommon.NPCommon_PlayerLazyCD getCdInfo() { return cdInfo; }
/// <summary>
/// cd信息
/// </summary>
public void setCdInfo(NPCommon.NPCommon_PlayerLazyCD _cdInfo) { cdInfo = _cdInfo; }


public int GetBufSize() {
	int _size = 48;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 50;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _cdInfoCustLen = _buf.getInt();
	int _cdInfoCurPos = _buf.getCurPos();
	cdInfo.ReadUnzipBuf(_buf, _cdInfoCurPos + _cdInfoCustLen);
	_buf.setPosition(_cdInfoCurPos + _cdInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(cdInfo.GetBufSize());
	cdInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)21);
	_buf.put((byte)51);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)21);
	_recBuf.put((byte)51);
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
	builder.Append("cdInfo").Append(":").Append(cdInfo == null ? "null" : cdInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

