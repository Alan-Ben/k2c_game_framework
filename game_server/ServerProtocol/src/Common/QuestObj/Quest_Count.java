package Common.QuestObj;

import java.nio.ByteBuffer;
/*********
 * 任务计数
 **/
public class Quest_Count implements ALBasicProtocolPack._IALProtocolStructure {
/** 任务配置ID */
private long questId;
/** 完成次数 */
private int doneCount;


public Quest_Count() {
	questId = (long)0;
	doneCount = 0;
}

public Quest_Count(
	 long _questId
	, int _doneCount
) {	questId = _questId;
	doneCount = _doneCount;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 任务配置ID */
public long getQuestId() { return questId; }
/** 任务配置ID */
public void setQuestId(long _questId) { questId = _questId; }
/** 完成次数 */
public int getDoneCount() { return doneCount; }
/** 完成次数 */
public void setDoneCount(int _doneCount) { doneCount = _doneCount; }


public final int GetBufSize() {
	int _size = 12;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) questId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) doneCount = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(questId);
	_buf.putInt(doneCount);
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

