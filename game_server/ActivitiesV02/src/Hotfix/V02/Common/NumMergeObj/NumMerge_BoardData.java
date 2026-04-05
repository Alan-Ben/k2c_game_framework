package Hotfix.V02.Common.NumMergeObj;

import java.nio.ByteBuffer;
/*********
 * 数字合并-棋盘数据
 **/
public class NumMerge_BoardData implements ALBasicProtocolPack._IALProtocolStructure {
/** 当前是第几步 */
private int currentStep;
/** 16个格子数据 */
private java.util.ArrayList<Hotfix.V02.Common.NumMergeObj.NumMerge_BlockBase> blocks;
/** 当前得分 */
private long currentScore;
/** 单局积分最大值 */
private long maxScore;
/** 累计得分 */
private long totalScore;


public NumMerge_BoardData() {
	currentStep = 0;
	blocks = new java.util.ArrayList<Hotfix.V02.Common.NumMergeObj.NumMerge_BlockBase>();
	currentScore = (long)0;
	maxScore = (long)0;
	totalScore = (long)0;
}

public NumMerge_BoardData(
	 int _currentStep
	, java.util.ArrayList<Hotfix.V02.Common.NumMergeObj.NumMerge_BlockBase> _blocks
	, long _currentScore
	, long _maxScore
	, long _totalScore
) {	currentStep = _currentStep;
	blocks = _blocks;
	currentScore = _currentScore;
	maxScore = _maxScore;
	totalScore = _totalScore;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 当前是第几步 */
public int getCurrentStep() { return currentStep; }
/** 当前是第几步 */
public void setCurrentStep(int _currentStep) { currentStep = _currentStep; }
/** 16个格子数据 */
public java.util.ArrayList<Hotfix.V02.Common.NumMergeObj.NumMerge_BlockBase> getBlocks() { return blocks; }
/** 16个格子数据 */
public void addBlocks(Hotfix.V02.Common.NumMergeObj.NumMerge_BlockBase _blocks) { blocks.add(_blocks); }
/** 当前得分 */
public long getCurrentScore() { return currentScore; }
/** 当前得分 */
public void setCurrentScore(long _currentScore) { currentScore = _currentScore; }
/** 单局积分最大值 */
public long getMaxScore() { return maxScore; }
/** 单局积分最大值 */
public void setMaxScore(long _maxScore) { maxScore = _maxScore; }
/** 累计得分 */
public long getTotalScore() { return totalScore; }
/** 累计得分 */
public void setTotalScore(long _totalScore) { totalScore = _totalScore; }


public final int GetBufSize() {
	int _size = 28;
	_size += 2 + (blocks.size() * 12);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 30;
	_size += 2 + (blocks.size() * 12);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) currentStep = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _blocksCount = _buf.getShort();
	for(int _i = 0; _i < _blocksCount; _i++) { 
		Hotfix.V02.Common.NumMergeObj.NumMerge_BlockBase _blocks = new Hotfix.V02.Common.NumMergeObj.NumMerge_BlockBase();
		if(_buf.remaining() <= 0) return;
	int __blocksCustLen = _buf.getInt();
	int __blocksCurPos = _buf.position();
	_blocks.ReadUnzipBuf(_buf, __blocksCurPos + __blocksCustLen);
	_buf.position(__blocksCurPos + __blocksCustLen);

		blocks.add(_blocks);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) currentScore = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) maxScore = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) totalScore = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(currentStep);
	_buf.putShort((short)blocks.size());
	for(int _i = 0; _i < blocks.size(); _i++) { 
		_buf.putInt(blocks.get(_i).GetBufSize());
	blocks.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putLong(currentScore);
	_buf.putLong(maxScore);
	_buf.putLong(totalScore);
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

