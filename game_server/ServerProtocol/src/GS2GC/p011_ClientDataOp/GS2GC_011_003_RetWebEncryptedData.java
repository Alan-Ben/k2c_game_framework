package GS2GC.p011_ClientDataOp;

import java.nio.ByteBuffer;
public class GS2GC_011_003_RetWebEncryptedData implements ALBasicProtocolPack._IALProtocolStructure {
private String data;


public GS2GC_011_003_RetWebEncryptedData() {
	data = "";
}

public GS2GC_011_003_RetWebEncryptedData(
	 String _data
) {	data = _data;
}

public final byte getMainOrder() { return (byte)11; }

public final byte getSubOrder() { return (byte)3; }

public String getData() { return data; }
public void setData(String _data) { data = _data; }


public final int GetBufSize() {
	int _size = 0;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(data);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(data);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) data = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, data);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)11);
	_buf.put((byte)3);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)11);
	_recBuf.put((byte)3);
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

