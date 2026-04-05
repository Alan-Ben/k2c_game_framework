using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p007_CommOp
{

public class GS2GC_007_059_PushRedDotChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 红点信息
/// </summary>
private Common.Common_RedDotInfo redDotInfo;


public GS2GC_007_059_PushRedDotChg() {
	redDotInfo = new Common.Common_RedDotInfo();
}

public GS2GC_007_059_PushRedDotChg(
	Common.Common_RedDotInfo _redDotInfo
) {	redDotInfo = _redDotInfo;
}

public byte getMainOrder() { return (byte)7; }

public byte getSubOrder() { return (byte)59; }

/// <summary>
/// 红点信息
/// </summary>
public Common.Common_RedDotInfo getRedDotInfo() { return redDotInfo; }
/// <summary>
/// 红点信息
/// </summary>
public void setRedDotInfo(Common.Common_RedDotInfo _redDotInfo) { redDotInfo = _redDotInfo; }


public int GetBufSize() {
	int _size = 16;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _redDotInfoCustLen = _buf.getInt();
	int _redDotInfoCurPos = _buf.getCurPos();
	redDotInfo.ReadUnzipBuf(_buf, _redDotInfoCurPos + _redDotInfoCustLen);
	_buf.setPosition(_redDotInfoCurPos + _redDotInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(redDotInfo.GetBufSize());
	redDotInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)7);
	_buf.put((byte)59);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)7);
	_recBuf.put((byte)59);
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
	builder.Append("redDotInfo").Append(":").Append(redDotInfo == null ? "null" : redDotInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

