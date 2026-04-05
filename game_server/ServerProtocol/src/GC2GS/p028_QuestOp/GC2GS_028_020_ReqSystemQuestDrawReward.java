package GC2GS.p028_QuestOp;

import java.nio.ByteBuffer;
/*********
 * 系统任务领取奖励
 **/
public class GC2GS_028_020_ReqSystemQuestDrawReward implements ALBasicProtocolPack._IALProtocolStructure {
/** 组id */
private long groupId;
/** 步骤 */
private int step;


public GC2GS_028_020_ReqSystemQuestDrawReward() {
	groupId = (long)0;
	step = 0;
}

public GC2GS_028_020_ReqSystemQuestDrawReward(
	 long _groupId
	, int _step
) {	groupId = _groupId;
	step = _step;
}

public final byte getMainOrder() { return (byte)28; }

public final byte getSubOrder() { return (byte)20; }

/** 组id */
public long getGroupId() { return groupId; }
/** 组id */
public void setGroupId(long _groupId) { groupId = _groupId; }
/** 步骤 */
public int getStep() { return step; }
/** 步骤 */
public void setStep(int _step) { step = _step; }


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
	if(_buf.remaining() > 0) groupId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) step = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(groupId);
	_buf.putInt(step);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)28);
	_buf.put((byte)20);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)28);
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

