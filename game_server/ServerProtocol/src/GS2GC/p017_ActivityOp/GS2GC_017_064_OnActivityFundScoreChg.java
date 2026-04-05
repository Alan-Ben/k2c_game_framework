package GS2GC.p017_ActivityOp;

import java.nio.ByteBuffer;
/*********
 * 活动基金-分数变化推送
 **/
public class GS2GC_017_064_OnActivityFundScoreChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 基金ID */
private long fundId;
/** 公式分数 */
private long formulaScore;
/** 任务分数 */
private long taskScore;


public GS2GC_017_064_OnActivityFundScoreChg() {
	fundId = (long)0;
	formulaScore = (long)0;
	taskScore = (long)0;
}

public GS2GC_017_064_OnActivityFundScoreChg(
	 long _fundId
	, long _formulaScore
	, long _taskScore
) {	fundId = _fundId;
	formulaScore = _formulaScore;
	taskScore = _taskScore;
}

public final byte getMainOrder() { return (byte)17; }

public final byte getSubOrder() { return (byte)64; }

/** 基金ID */
public long getFundId() { return fundId; }
/** 基金ID */
public void setFundId(long _fundId) { fundId = _fundId; }
/** 公式分数 */
public long getFormulaScore() { return formulaScore; }
/** 公式分数 */
public void setFormulaScore(long _formulaScore) { formulaScore = _formulaScore; }
/** 任务分数 */
public long getTaskScore() { return taskScore; }
/** 任务分数 */
public void setTaskScore(long _taskScore) { taskScore = _taskScore; }


public final int GetBufSize() {
	int _size = 24;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 26;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) fundId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) formulaScore = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) taskScore = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(fundId);
	_buf.putLong(formulaScore);
	_buf.putLong(taskScore);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)17);
	_buf.put((byte)64);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)17);
	_recBuf.put((byte)64);
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

