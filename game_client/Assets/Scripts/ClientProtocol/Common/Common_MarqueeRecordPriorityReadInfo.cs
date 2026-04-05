using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class Common_MarqueeRecordPriorityReadInfo : ALBasicProtocolPack._IALProtocolStructure {
private long priorityId;
private long hadReadDbId;


public Common_MarqueeRecordPriorityReadInfo() {
	priorityId = (long)0;
	hadReadDbId = (long)0;
}

public Common_MarqueeRecordPriorityReadInfo(
	long _priorityId
	, long _hadReadDbId
) {	priorityId = _priorityId;
	hadReadDbId = _hadReadDbId;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public long getPriorityId() { return priorityId; }
public void setPriorityId(long _priorityId) { priorityId = _priorityId; }
public long getHadReadDbId() { return hadReadDbId; }
public void setHadReadDbId(long _hadReadDbId) { hadReadDbId = _hadReadDbId; }


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
	priorityId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	hadReadDbId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(priorityId);
	_buf.putLong(hadReadDbId);
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
	builder.Append("priorityId").Append(":").Append(priorityId.ToString()).Append(", ");
	builder.Append("hadReadDbId").Append(":").Append(hadReadDbId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

