package Common.QuestObj;

import java.nio.ByteBuffer;
/*********
 * 系统任务数据
 **/
public class SystemQuest_Info implements ALBasicProtocolPack._IALProtocolStructure {
/** 组ID */
private long groupId;
/** 步骤 */
private int step;
/** 计数 */
private long count;


public SystemQuest_Info() {
	groupId = (long)0;
	step = 0;
	count = (long)0;
}

public SystemQuest_Info(
	 long _groupId
	, int _step
	, long _count
) {	groupId = _groupId;
	step = _step;
	count = _count;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 组ID */
public long getGroupId() { return groupId; }
/** 组ID */
public void setGroupId(long _groupId) { groupId = _groupId; }
/** 步骤 */
public int getStep() { return step; }
/** 步骤 */
public void setStep(int _step) { step = _step; }
/** 计数 */
public long getCount() { return count; }
/** 计数 */
public void setCount(long _count) { count = _count; }


public final int GetBufSize() {
	int _size = 20;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 22;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) groupId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) step = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) count = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(groupId);
	_buf.putInt(step);
	_buf.putLong(count);
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

