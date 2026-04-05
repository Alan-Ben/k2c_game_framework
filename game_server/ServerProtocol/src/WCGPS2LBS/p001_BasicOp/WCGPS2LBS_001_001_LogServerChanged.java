package WCGPS2LBS.p001_BasicOp;

import java.nio.ByteBuffer;
public class WCGPS2LBS_001_001_LogServerChanged implements ALBasicProtocolPack._IALProtocolStructure {
private int serverTypeId;
private boolean addOrRemoved;


public WCGPS2LBS_001_001_LogServerChanged() {
	serverTypeId = 0;
	addOrRemoved = false;
}

public WCGPS2LBS_001_001_LogServerChanged(
	 int _serverTypeId
	, boolean _addOrRemoved
) {	serverTypeId = _serverTypeId;
	addOrRemoved = _addOrRemoved;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)1; }

public int getServerTypeId() { return serverTypeId; }
public void setServerTypeId(int _serverTypeId) { serverTypeId = _serverTypeId; }
public boolean getAddOrRemoved() { return addOrRemoved; }
public void setAddOrRemoved(boolean _addOrRemoved) { addOrRemoved = _addOrRemoved; }


public final int GetBufSize() {
	int _size = 5;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 7;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) serverTypeId = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) addOrRemoved = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(serverTypeId);
	_buf.put(addOrRemoved?(byte)1:(byte)0);
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

