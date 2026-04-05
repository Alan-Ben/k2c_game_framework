package GC2GS.p028_QuestOp;

import java.nio.ByteBuffer;
public class GC2GS_028_004_ReqAddClientTargetCount implements ALBasicProtocolPack._IALProtocolStructure {
private long questId;
private long questStepId;
private long questStepTargetId;
/** 修改的进度值 */
private int changeCount;


public GC2GS_028_004_ReqAddClientTargetCount() {
	questId = (long)0;
	questStepId = (long)0;
	questStepTargetId = (long)0;
	changeCount = 0;
}

public GC2GS_028_004_ReqAddClientTargetCount(
	 long _questId
	, long _questStepId
	, long _questStepTargetId
	, int _changeCount
) {	questId = _questId;
	questStepId = _questStepId;
	questStepTargetId = _questStepTargetId;
	changeCount = _changeCount;
}

public final byte getMainOrder() { return (byte)28; }

public final byte getSubOrder() { return (byte)4; }

public long getQuestId() { return questId; }
public void setQuestId(long _questId) { questId = _questId; }
public long getQuestStepId() { return questStepId; }
public void setQuestStepId(long _questStepId) { questStepId = _questStepId; }
public long getQuestStepTargetId() { return questStepTargetId; }
public void setQuestStepTargetId(long _questStepTargetId) { questStepTargetId = _questStepTargetId; }
/** 修改的进度值 */
public int getChangeCount() { return changeCount; }
/** 修改的进度值 */
public void setChangeCount(int _changeCount) { changeCount = _changeCount; }


public final int GetBufSize() {
	int _size = 28;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 30;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) questId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) questStepId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) questStepTargetId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) changeCount = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(questId);
	_buf.putLong(questStepId);
	_buf.putLong(questStepTargetId);
	_buf.putInt(changeCount);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)28);
	_buf.put((byte)4);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)28);
	_recBuf.put((byte)4);
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

