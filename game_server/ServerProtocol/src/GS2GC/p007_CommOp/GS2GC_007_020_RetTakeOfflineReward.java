package GS2GC.p007_CommOp;

import java.nio.ByteBuffer;
public class GS2GC_007_020_RetTakeOfflineReward implements ALBasicProtocolPack._IALProtocolStructure {
private byte[] extData;


public GS2GC_007_020_RetTakeOfflineReward() {
	extData = null;
}

public GS2GC_007_020_RetTakeOfflineReward(
	 byte[] _extData
) {	extData = _extData;
}

public final byte getMainOrder() { return (byte)7; }

public final byte getSubOrder() { return (byte)20; }

public byte[] getExtData() { return extData; }
public java.nio.ByteBuffer get_buffer_ExtData() { if(null == extData)return null; else return ByteBuffer.wrap(extData); }

public void setExtData(byte[] _extData) { extData = _extData; }
public void setExtData(java.nio.ByteBuffer _extData) 
{
	if(null == _extData){return;}
	int _oldPos = _extData.position();
	int _bufLength = _extData.remaining();
	extData = new byte[_bufLength];
	_extData.get(extData);
	_extData.position(_oldPos);
}



public final int GetBufSize() {
	int _size = 0;
	_size += 4 + (extData == null ? 0 : extData.length);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + (extData == null ? 0 : extData.length);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _extDataCount = _buf.getInt();
	if(0 < _extDataCount){
		extData = new byte[_extDataCount];
		_buf.get(extData);
	}

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt((extData == null ? 0 : extData.length));
	if(null != extData){_buf.put(extData);}

}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)7);
	_buf.put((byte)20);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)7);
	_recBuf.put((byte)20);
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

