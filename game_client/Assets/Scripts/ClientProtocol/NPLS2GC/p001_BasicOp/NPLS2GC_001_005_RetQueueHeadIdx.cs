using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace NPLS2GC.p001_BasicOp
{

public class NPLS2GC_001_005_RetQueueHeadIdx : ALBasicProtocolPack._IALProtocolStructure {
private long headQueueIdx;


public NPLS2GC_001_005_RetQueueHeadIdx() {
	headQueueIdx = (long)0;
}

public NPLS2GC_001_005_RetQueueHeadIdx(
	long _headQueueIdx
) {	headQueueIdx = _headQueueIdx;
}

public byte getMainOrder() { return (byte)1; }

public byte getSubOrder() { return (byte)5; }

public long getHeadQueueIdx() { return headQueueIdx; }
public void setHeadQueueIdx(long _headQueueIdx) { headQueueIdx = _headQueueIdx; }


public int GetBufSize() {
	int _size = 8;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	headQueueIdx = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(headQueueIdx);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)5);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)5);
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
	builder.Append("headQueueIdx").Append(":").Append(headQueueIdx.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

