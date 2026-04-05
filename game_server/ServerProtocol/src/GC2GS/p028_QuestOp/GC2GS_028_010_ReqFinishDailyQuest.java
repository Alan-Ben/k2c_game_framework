package GC2GS.p028_QuestOp;

import java.nio.ByteBuffer;
/*********
 * 完成日常任务
 **/
public class GC2GS_028_010_ReqFinishDailyQuest implements ALBasicProtocolPack._IALProtocolStructure {
/** 任务刷新序列号 */
private long refreshSerial;
/** 任务id */
private long questId;


public GC2GS_028_010_ReqFinishDailyQuest() {
	refreshSerial = (long)0;
	questId = (long)0;
}

public GC2GS_028_010_ReqFinishDailyQuest(
	 long _refreshSerial
	, long _questId
) {	refreshSerial = _refreshSerial;
	questId = _questId;
}

public final byte getMainOrder() { return (byte)28; }

public final byte getSubOrder() { return (byte)10; }

/** 任务刷新序列号 */
public long getRefreshSerial() { return refreshSerial; }
/** 任务刷新序列号 */
public void setRefreshSerial(long _refreshSerial) { refreshSerial = _refreshSerial; }
/** 任务id */
public long getQuestId() { return questId; }
/** 任务id */
public void setQuestId(long _questId) { questId = _questId; }


public final int GetBufSize() {
	int _size = 16;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) refreshSerial = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) questId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(refreshSerial);
	_buf.putLong(questId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)28);
	_buf.put((byte)10);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)28);
	_recBuf.put((byte)10);
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

