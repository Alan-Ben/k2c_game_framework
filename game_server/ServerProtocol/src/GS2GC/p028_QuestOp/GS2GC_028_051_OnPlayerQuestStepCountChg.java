package GS2GC.p028_QuestOp;

import java.nio.ByteBuffer;
public class GS2GC_028_051_OnPlayerQuestStepCountChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 任务ID */
private long questId;
/** 任务步骤 */
private long questStep;
/** 任务步骤下的目标ID */
private long questTargetId;
/** 任务步骤的计数 */
private long questTargetCount;


public GS2GC_028_051_OnPlayerQuestStepCountChg() {
	questId = (long)0;
	questStep = (long)0;
	questTargetId = (long)0;
	questTargetCount = (long)0;
}

public GS2GC_028_051_OnPlayerQuestStepCountChg(
	 long _questId
	, long _questStep
	, long _questTargetId
	, long _questTargetCount
) {	questId = _questId;
	questStep = _questStep;
	questTargetId = _questTargetId;
	questTargetCount = _questTargetCount;
}

public final byte getMainOrder() { return (byte)28; }

public final byte getSubOrder() { return (byte)51; }

/** 任务ID */
public long getQuestId() { return questId; }
/** 任务ID */
public void setQuestId(long _questId) { questId = _questId; }
/** 任务步骤 */
public long getQuestStep() { return questStep; }
/** 任务步骤 */
public void setQuestStep(long _questStep) { questStep = _questStep; }
/** 任务步骤下的目标ID */
public long getQuestTargetId() { return questTargetId; }
/** 任务步骤下的目标ID */
public void setQuestTargetId(long _questTargetId) { questTargetId = _questTargetId; }
/** 任务步骤的计数 */
public long getQuestTargetCount() { return questTargetCount; }
/** 任务步骤的计数 */
public void setQuestTargetCount(long _questTargetCount) { questTargetCount = _questTargetCount; }


public final int GetBufSize() {
	int _size = 32;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 34;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) questId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) questStep = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) questTargetId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) questTargetCount = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(questId);
	_buf.putLong(questStep);
	_buf.putLong(questTargetId);
	_buf.putLong(questTargetCount);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)28);
	_buf.put((byte)51);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)28);
	_recBuf.put((byte)51);
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

