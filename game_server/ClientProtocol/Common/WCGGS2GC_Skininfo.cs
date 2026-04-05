using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class WCGGS2GC_Skininfo : ALBasicProtocolPack._IALProtocolStructure {
private long skinId;
private long remaniningTime;
private bool isUsing;


public WCGGS2GC_Skininfo() {
	skinId = (long)0;
	remaniningTime = (long)0;
	isUsing = false;
}

public WCGGS2GC_Skininfo(
	long _skinId
	, long _remaniningTime
	, bool _isUsing
) {	skinId = _skinId;
	remaniningTime = _remaniningTime;
	isUsing = _isUsing;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public long getSkinId() { return skinId; }
public void setSkinId(long _skinId) { skinId = _skinId; }
public long getRemaniningTime() { return remaniningTime; }
public void setRemaniningTime(long _remaniningTime) { remaniningTime = _remaniningTime; }
public bool getIsUsing() { return isUsing; }
public void setIsUsing(bool _isUsing) { isUsing = _isUsing; }


public int GetBufSize() {
	int _size = 17;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 19;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	skinId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	remaniningTime = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isUsing = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(skinId);
	_buf.putLong(remaniningTime);
	_buf.put(isUsing?(byte)1:(byte)0);
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
	builder.Append("skinId").Append(":").Append(skinId.ToString()).Append(", ");
	builder.Append("remaniningTime").Append(":").Append(remaniningTime.ToString()).Append(", ");
	builder.Append("isUsing").Append(":").Append(isUsing.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

