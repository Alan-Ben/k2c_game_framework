package GS2GC.p028_QuestOp;

import java.nio.ByteBuffer;
public class GS2GC_028_052_OnPlayerQuestRemove implements ALBasicProtocolPack._IALProtocolStructure {
private long questId;


public GS2GC_028_052_OnPlayerQuestRemove() {
	questId = (long)0;
}

public GS2GC_028_052_OnPlayerQuestRemove(
	 long _questId
) {	questId = _questId;
}

public final byte getMainOrder() { return (byte)28; }

public final byte getSubOrder() { return (byte)52; }

public long getQuestId() { return questId; }
public void setQuestId(long _questId) { questId = _questId; }


public final int GetBufSize() {
	int _size = 8;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) questId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(questId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)28);
	_buf.put((byte)52);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)28);
	_recBuf.put((byte)52);
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

