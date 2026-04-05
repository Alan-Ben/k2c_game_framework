package GS2GC.p004_PlayerOp;

import java.nio.ByteBuffer;
public class GS2GC_004_065_OnEventRecordChg implements ALBasicProtocolPack._IALProtocolStructure {
private Common.PlayerObj.Player_EventRecordInfo record;


public GS2GC_004_065_OnEventRecordChg() {
	record = new Common.PlayerObj.Player_EventRecordInfo();
}

public GS2GC_004_065_OnEventRecordChg(
	 Common.PlayerObj.Player_EventRecordInfo _record
) {	record = _record;
}

public final byte getMainOrder() { return (byte)4; }

public final byte getSubOrder() { return (byte)65; }

public Common.PlayerObj.Player_EventRecordInfo getRecord() { return record; }
public void setRecord(Common.PlayerObj.Player_EventRecordInfo _record) { record = _record; }


public final int GetBufSize() {
	int _size = 24;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 26;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _recordCustLen = _buf.getInt();
	int _recordCurPos = _buf.position();
	record.ReadUnzipBuf(_buf, _recordCurPos + _recordCustLen);
	_buf.position(_recordCurPos + _recordCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(record.GetBufSize());
	record.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)65);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)65);
	PutUnzipBuf(_recBuf);
}
public final ByteBuffer makePackage() {
	int _bufSize = GetBufSize();
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void readPackage(ByteBuffer _buf) {
	ReadUnzipBuf(_buf, -1);
}
}

