package GOM2CD_R.gom_p001_DataOp;

import java.nio.ByteBuffer;
public class TestCD_Data implements ALBasicProtocolPack._IALProtocolStructure {
private long id;
private String testStr;
private int value;


public TestCD_Data() {
	id = (long)0;
	testStr = "";
	value = 0;
}

public TestCD_Data(
	 long _id
	, String _testStr
	, int _value
) {	id = _id;
	testStr = _testStr;
	value = _value;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)1; }

public long getId() { return id; }
public void setId(long _id) { id = _id; }
public String getTestStr() { return testStr; }
public void setTestStr(String _testStr) { testStr = _testStr; }
public int getValue() { return value; }
public void setValue(int _value) { value = _value; }


public final int GetBufSize() {
	int _size = 12;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(testStr);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(testStr);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) id = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) testStr = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) value = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(id);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, testStr);
	_buf.putInt(value);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)1);
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

