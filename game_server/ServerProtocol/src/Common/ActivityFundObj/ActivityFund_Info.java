package Common.ActivityFundObj;

import java.nio.ByteBuffer;
/*********
 * 活动基金-基金信息
 **/
public class ActivityFund_Info implements ALBasicProtocolPack._IALProtocolStructure {
/** 基金ID */
private long fundId;
/** 活动实例ID（常驻基金为0） */
private long activityInstanceId;
/** 公式分数 */
private long formulaScore;
/** 任务分数 */
private long taskScore;
/** 已领取免费档阶段 */
private int hadDrawFreeStep;
/** 已领取付费档阶段 */
private int hadDrawPayStep;


public ActivityFund_Info() {
	fundId = (long)0;
	activityInstanceId = (long)0;
	formulaScore = (long)0;
	taskScore = (long)0;
	hadDrawFreeStep = 0;
	hadDrawPayStep = 0;
}

public ActivityFund_Info(
	 long _fundId
	, long _activityInstanceId
	, long _formulaScore
	, long _taskScore
	, int _hadDrawFreeStep
	, int _hadDrawPayStep
) {	fundId = _fundId;
	activityInstanceId = _activityInstanceId;
	formulaScore = _formulaScore;
	taskScore = _taskScore;
	hadDrawFreeStep = _hadDrawFreeStep;
	hadDrawPayStep = _hadDrawPayStep;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 基金ID */
public long getFundId() { return fundId; }
/** 基金ID */
public void setFundId(long _fundId) { fundId = _fundId; }
/** 活动实例ID（常驻基金为0） */
public long getActivityInstanceId() { return activityInstanceId; }
/** 活动实例ID（常驻基金为0） */
public void setActivityInstanceId(long _activityInstanceId) { activityInstanceId = _activityInstanceId; }
/** 公式分数 */
public long getFormulaScore() { return formulaScore; }
/** 公式分数 */
public void setFormulaScore(long _formulaScore) { formulaScore = _formulaScore; }
/** 任务分数 */
public long getTaskScore() { return taskScore; }
/** 任务分数 */
public void setTaskScore(long _taskScore) { taskScore = _taskScore; }
/** 已领取免费档阶段 */
public int getHadDrawFreeStep() { return hadDrawFreeStep; }
/** 已领取免费档阶段 */
public void setHadDrawFreeStep(int _hadDrawFreeStep) { hadDrawFreeStep = _hadDrawFreeStep; }
/** 已领取付费档阶段 */
public int getHadDrawPayStep() { return hadDrawPayStep; }
/** 已领取付费档阶段 */
public void setHadDrawPayStep(int _hadDrawPayStep) { hadDrawPayStep = _hadDrawPayStep; }


public final int GetBufSize() {
	int _size = 40;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 42;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) fundId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) activityInstanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) formulaScore = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) taskScore = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hadDrawFreeStep = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hadDrawPayStep = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(fundId);
	_buf.putLong(activityInstanceId);
	_buf.putLong(formulaScore);
	_buf.putLong(taskScore);
	_buf.putInt(hadDrawFreeStep);
	_buf.putInt(hadDrawPayStep);
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

