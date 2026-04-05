using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace NPCommon
{

public class NPCommon_TimerProductInfo : ALBasicProtocolPack._IALProtocolStructure {
private long refId;
private long unitMultiple;
private long unitDuration;
private long lastEndTimeMs;


public NPCommon_TimerProductInfo() {
	refId = (long)0;
	unitMultiple = (long)0;
	unitDuration = (long)0;
	lastEndTimeMs = (long)0;
}

public NPCommon_TimerProductInfo(
	long _refId
	, long _unitMultiple
	, long _unitDuration
	, long _lastEndTimeMs
) {	refId = _refId;
	unitMultiple = _unitMultiple;
	unitDuration = _unitDuration;
	lastEndTimeMs = _lastEndTimeMs;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public long getRefId() { return refId; }
public void setRefId(long _refId) { refId = _refId; }
public long getUnitMultiple() { return unitMultiple; }
public void setUnitMultiple(long _unitMultiple) { unitMultiple = _unitMultiple; }
public long getUnitDuration() { return unitDuration; }
public void setUnitDuration(long _unitDuration) { unitDuration = _unitDuration; }
public long getLastEndTimeMs() { return lastEndTimeMs; }
public void setLastEndTimeMs(long _lastEndTimeMs) { lastEndTimeMs = _lastEndTimeMs; }


public int GetBufSize() {
	int _size = 32;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 34;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	refId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	unitMultiple = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	unitDuration = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	lastEndTimeMs = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(refId);
	_buf.putLong(unitMultiple);
	_buf.putLong(unitDuration);
	_buf.putLong(lastEndTimeMs);
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
	builder.Append("refId").Append(":").Append(refId.ToString()).Append(", ");
	builder.Append("unitMultiple").Append(":").Append(unitMultiple.ToString()).Append(", ");
	builder.Append("unitDuration").Append(":").Append(unitDuration.ToString()).Append(", ");
	builder.Append("lastEndTimeMs").Append(":").Append(lastEndTimeMs.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

