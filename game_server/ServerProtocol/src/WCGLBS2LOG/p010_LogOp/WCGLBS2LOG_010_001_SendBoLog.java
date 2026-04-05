package WCGLBS2LOG.p010_LogOp;

import java.nio.ByteBuffer;
public class WCGLBS2LOG_010_001_SendBoLog implements ALBasicProtocolPack._IALProtocolStructure {
private byte[] boBuffer;
private long timeStamp;


public WCGLBS2LOG_010_001_SendBoLog() {
	boBuffer = null;
	timeStamp = (long)0;
}

public WCGLBS2LOG_010_001_SendBoLog(
	 byte[] _boBuffer
	, long _timeStamp
) {	boBuffer = _boBuffer;
	timeStamp = _timeStamp;
}

public final byte getMainOrder() { return (byte)10; }

public final byte getSubOrder() { return (byte)1; }

public byte[] getBoBuffer() { return boBuffer; }
public java.nio.ByteBuffer get_buffer_BoBuffer() { if(null == boBuffer)return null; else return ByteBuffer.wrap(boBuffer); }

public void setBoBuffer(byte[] _boBuffer) { boBuffer = _boBuffer; }
public void setBoBuffer(java.nio.ByteBuffer _boBuffer) 
{
	if(null == _boBuffer){return;}
	int _oldPos = _boBuffer.position();
	int _bufLength = _boBuffer.remaining();
	boBuffer = new byte[_bufLength];
	_boBuffer.get(boBuffer);
	_boBuffer.position(_oldPos);
}

public long getTimeStamp() { return timeStamp; }
public void setTimeStamp(long _timeStamp) { timeStamp = _timeStamp; }


public final int GetBufSize() {
	int _size = 8;
	_size += 4 + (boBuffer == null ? 0 : boBuffer.length);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += 4 + (boBuffer == null ? 0 : boBuffer.length);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _boBufferCount = _buf.getInt();
	if(0 < _boBufferCount){
		boBuffer = new byte[_boBufferCount];
		_buf.get(boBuffer);
	}

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) timeStamp = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt((boBuffer == null ? 0 : boBuffer.length));
	if(null != boBuffer){_buf.put(boBuffer);}

	_buf.putLong(timeStamp);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)10);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)10);
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

