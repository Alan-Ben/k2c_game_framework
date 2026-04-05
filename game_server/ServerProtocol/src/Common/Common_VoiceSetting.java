package Common;

import java.nio.ByteBuffer;
public class Common_VoiceSetting implements ALBasicProtocolPack._IALProtocolStructure {
private boolean isSpeakerOn;
private boolean isMicroOn;


public Common_VoiceSetting() {
	isSpeakerOn = false;
	isMicroOn = false;
}

public Common_VoiceSetting(
	 boolean _isSpeakerOn
	, boolean _isMicroOn
) {	isSpeakerOn = _isSpeakerOn;
	isMicroOn = _isMicroOn;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public boolean getIsSpeakerOn() { return isSpeakerOn; }
public void setIsSpeakerOn(boolean _isSpeakerOn) { isSpeakerOn = _isSpeakerOn; }
public boolean getIsMicroOn() { return isMicroOn; }
public void setIsMicroOn(boolean _isMicroOn) { isMicroOn = _isMicroOn; }


public final int GetBufSize() {
	int _size = 2;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 4;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isSpeakerOn = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isMicroOn = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.put(isSpeakerOn?(byte)1:(byte)0);
	_buf.put(isMicroOn?(byte)1:(byte)0);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
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

