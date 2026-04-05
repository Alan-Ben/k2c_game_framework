using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace NPGS2GC.p001_BasicOp
{

public class NPGS2GC_001_001_RetBasicInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 服务器版本
/// </summary>
private string version;
private long timeTag;
private int timezone;
/// <summary>
/// 夏令时时差
/// </summary>
private int dstOffset;
/// <summary>
/// 资源版本
/// </summary>
private string resVersion;


public NPGS2GC_001_001_RetBasicInfo() {
	version = "";
	timeTag = (long)0;
	timezone = 0;
	dstOffset = 0;
	resVersion = "";
}

public NPGS2GC_001_001_RetBasicInfo(
	string _version
	, long _timeTag
	, int _timezone
	, int _dstOffset
	, string _resVersion
) {	version = _version;
	timeTag = _timeTag;
	timezone = _timezone;
	dstOffset = _dstOffset;
	resVersion = _resVersion;
}

public byte getMainOrder() { return (byte)1; }

public byte getSubOrder() { return (byte)1; }

/// <summary>
/// 服务器版本
/// </summary>
public string getVersion() { return version; }
/// <summary>
/// 服务器版本
/// </summary>
public void setVersion(string _version) { version = _version; }
public long getTimeTag() { return timeTag; }
public void setTimeTag(long _timeTag) { timeTag = _timeTag; }
public int getTimezone() { return timezone; }
public void setTimezone(int _timezone) { timezone = _timezone; }
/// <summary>
/// 夏令时时差
/// </summary>
public int getDstOffset() { return dstOffset; }
/// <summary>
/// 夏令时时差
/// </summary>
public void setDstOffset(int _dstOffset) { dstOffset = _dstOffset; }
/// <summary>
/// 资源版本
/// </summary>
public string getResVersion() { return resVersion; }
/// <summary>
/// 资源版本
/// </summary>
public void setResVersion(string _resVersion) { resVersion = _resVersion; }


public int GetBufSize() {
	int _size = 16;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(version);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(resVersion);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 18;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(version);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(resVersion);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	version = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	timeTag = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	timezone = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	dstOffset = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	resVersion = _buf.getString();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putString(version);
	_buf.putLong(timeTag);
	_buf.putInt(timezone);
	_buf.putInt(dstOffset);
	_buf.putString(resVersion);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)1);
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
	builder.Append("version").Append(":").Append(version.ToString()).Append(", ");
	builder.Append("timeTag").Append(":").Append(timeTag.ToString()).Append(", ");
	builder.Append("timezone").Append(":").Append(timezone.ToString()).Append(", ");
	builder.Append("dstOffset").Append(":").Append(dstOffset.ToString()).Append(", ");
	builder.Append("resVersion").Append(":").Append(resVersion.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

