package Common;

import java.nio.ByteBuffer;
public class TutorialData implements ALBasicProtocolPack._IALProtocolStructure {
private long lastFinishForceTutorial;
private java.util.ArrayList<Long> finishTutorialId;


public TutorialData() {
	lastFinishForceTutorial = (long)0;
	finishTutorialId = new java.util.ArrayList<Long>();
}

public TutorialData(
	 long _lastFinishForceTutorial
	, java.util.ArrayList<Long> _finishTutorialId
) {	lastFinishForceTutorial = _lastFinishForceTutorial;
	finishTutorialId = _finishTutorialId;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getLastFinishForceTutorial() { return lastFinishForceTutorial; }
public void setLastFinishForceTutorial(long _lastFinishForceTutorial) { lastFinishForceTutorial = _lastFinishForceTutorial; }
public java.util.ArrayList<Long> getFinishTutorialId() { return finishTutorialId; }
public void addFinishTutorialId(long _finishTutorialId) { finishTutorialId.add(_finishTutorialId); }


public final int GetBufSize() {
	int _size = 8;
	_size += 2 + (finishTutorialId.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += 2 + (finishTutorialId.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) lastFinishForceTutorial = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _finishTutorialIdCount = _buf.getShort();
	for(int _i = 0; _i < _finishTutorialIdCount; _i++) { 
		long _finishTutorialId = (long)0;
		if(_buf.remaining() > 0) _finishTutorialId = _buf.getLong();
		finishTutorialId.add(_finishTutorialId);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(lastFinishForceTutorial);
	_buf.putShort((short)finishTutorialId.size());
	for(int _i = 0; _i < finishTutorialId.size(); _i++) { 
		_buf.putLong(finishTutorialId.get(_i));
	}
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

