using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

/// <summary>
/// 红点信息
/// </summary>
public class Common_RedDotInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 红点类型
/// </summary>
private CommonEnum.ERedDotType redDotType;
/// <summary>
/// 需要检查的时间点
/// </summary>
private long needCheckTimeMs;


public Common_RedDotInfo() {
	redDotType = 0;
	needCheckTimeMs = (long)0;
}

public Common_RedDotInfo(
	CommonEnum.ERedDotType _redDotType
	, long _needCheckTimeMs
) {	redDotType = _redDotType;
	needCheckTimeMs = _needCheckTimeMs;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 红点类型
/// </summary>
public CommonEnum.ERedDotType getRedDotType() { return redDotType; }
/// <summary>
/// 红点类型
/// </summary>
public void setRedDotType(CommonEnum.ERedDotType _redDotType) { redDotType = _redDotType; }
/// <summary>
/// 需要检查的时间点
/// </summary>
public long getNeedCheckTimeMs() { return needCheckTimeMs; }
/// <summary>
/// 需要检查的时间点
/// </summary>
public void setNeedCheckTimeMs(long _needCheckTimeMs) { needCheckTimeMs = _needCheckTimeMs; }


public int GetBufSize() {
	int _size = 12;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 14;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	redDotType = (CommonEnum.ERedDotType)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	needCheckTimeMs = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt((int)redDotType);

	_buf.putLong(needCheckTimeMs);
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
	builder.Append("redDotType").Append(":").Append(redDotType.ToString()).Append(", ");
	builder.Append("needCheckTimeMs").Append(":").Append(needCheckTimeMs.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

