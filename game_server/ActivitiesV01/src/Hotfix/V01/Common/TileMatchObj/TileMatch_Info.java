package Hotfix.V01.Common.TileMatchObj;

import java.nio.ByteBuffer;
/*********
 * 三消-信息
 **/
public class TileMatch_Info implements ALBasicProtocolPack._IALProtocolStructure {
/** 阶段奖励信息 */
private Hotfix.V01.Common.TileMatchObj.TileMatch_StepRewardInfo stepRewardInfo;
/** 可领取阶段奖励列表 */
private java.util.ArrayList<Hotfix.V01.Common.TileMatchObj.TileMatch_CanDrawStepReward> canDrawStepRewardList;
/** 总分数 */
private long totalScore;


public TileMatch_Info() {
	stepRewardInfo = new Hotfix.V01.Common.TileMatchObj.TileMatch_StepRewardInfo();
	canDrawStepRewardList = new java.util.ArrayList<Hotfix.V01.Common.TileMatchObj.TileMatch_CanDrawStepReward>();
	totalScore = (long)0;
}

public TileMatch_Info(
	 Hotfix.V01.Common.TileMatchObj.TileMatch_StepRewardInfo _stepRewardInfo
	, java.util.ArrayList<Hotfix.V01.Common.TileMatchObj.TileMatch_CanDrawStepReward> _canDrawStepRewardList
	, long _totalScore
) {	stepRewardInfo = _stepRewardInfo;
	canDrawStepRewardList = _canDrawStepRewardList;
	totalScore = _totalScore;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 阶段奖励信息 */
public Hotfix.V01.Common.TileMatchObj.TileMatch_StepRewardInfo getStepRewardInfo() { return stepRewardInfo; }
/** 阶段奖励信息 */
public void setStepRewardInfo(Hotfix.V01.Common.TileMatchObj.TileMatch_StepRewardInfo _stepRewardInfo) { stepRewardInfo = _stepRewardInfo; }
/** 可领取阶段奖励列表 */
public java.util.ArrayList<Hotfix.V01.Common.TileMatchObj.TileMatch_CanDrawStepReward> getCanDrawStepRewardList() { return canDrawStepRewardList; }
/** 可领取阶段奖励列表 */
public void addCanDrawStepRewardList(Hotfix.V01.Common.TileMatchObj.TileMatch_CanDrawStepReward _canDrawStepRewardList) { canDrawStepRewardList.add(_canDrawStepRewardList); }
/** 总分数 */
public long getTotalScore() { return totalScore; }
/** 总分数 */
public void setTotalScore(long _totalScore) { totalScore = _totalScore; }


public final int GetBufSize() {
	int _size = 28;
	_size += 2 + (canDrawStepRewardList.size() * 12);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 30;
	_size += 2 + (canDrawStepRewardList.size() * 12);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _stepRewardInfoCustLen = _buf.getInt();
	int _stepRewardInfoCurPos = _buf.position();
	stepRewardInfo.ReadUnzipBuf(_buf, _stepRewardInfoCurPos + _stepRewardInfoCustLen);
	_buf.position(_stepRewardInfoCurPos + _stepRewardInfoCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _canDrawStepRewardListCount = _buf.getShort();
	for(int _i = 0; _i < _canDrawStepRewardListCount; _i++) { 
		Hotfix.V01.Common.TileMatchObj.TileMatch_CanDrawStepReward _canDrawStepRewardList = new Hotfix.V01.Common.TileMatchObj.TileMatch_CanDrawStepReward();
		if(_buf.remaining() <= 0) return;
	int __canDrawStepRewardListCustLen = _buf.getInt();
	int __canDrawStepRewardListCurPos = _buf.position();
	_canDrawStepRewardList.ReadUnzipBuf(_buf, __canDrawStepRewardListCurPos + __canDrawStepRewardListCustLen);
	_buf.position(__canDrawStepRewardListCurPos + __canDrawStepRewardListCustLen);

		canDrawStepRewardList.add(_canDrawStepRewardList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) totalScore = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(stepRewardInfo.GetBufSize());
	stepRewardInfo.PutUnzipBuf(_buf);
	_buf.putShort((short)canDrawStepRewardList.size());
	for(int _i = 0; _i < canDrawStepRewardList.size(); _i++) { 
		_buf.putInt(canDrawStepRewardList.get(_i).GetBufSize());
	canDrawStepRewardList.get(_i).PutUnzipBuf(_buf);
	}
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

