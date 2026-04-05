package GS2GC.p007_CommOp;

import java.nio.ByteBuffer;
public class GS2GC_007_004_RetQuestionnaireInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 是否有奖励 */
private boolean hasReward;
/** 问卷奖励信息 */
private Common.Common_QuestionnaireRewardInfo rewardInfo;


public GS2GC_007_004_RetQuestionnaireInfo() {
	hasReward = false;
	rewardInfo = new Common.Common_QuestionnaireRewardInfo();
}

public GS2GC_007_004_RetQuestionnaireInfo(
	 boolean _hasReward
	, Common.Common_QuestionnaireRewardInfo _rewardInfo
) {	hasReward = _hasReward;
	rewardInfo = _rewardInfo;
}

public final byte getMainOrder() { return (byte)7; }

public final byte getSubOrder() { return (byte)4; }

/** 是否有奖励 */
public boolean getHasReward() { return hasReward; }
/** 是否有奖励 */
public void setHasReward(boolean _hasReward) { hasReward = _hasReward; }
/** 问卷奖励信息 */
public Common.Common_QuestionnaireRewardInfo getRewardInfo() { return rewardInfo; }
/** 问卷奖励信息 */
public void setRewardInfo(Common.Common_QuestionnaireRewardInfo _rewardInfo) { rewardInfo = _rewardInfo; }


public final int GetBufSize() {
	int _size = 14;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 16;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hasReward = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _rewardInfoCustLen = _buf.getInt();
	int _rewardInfoCurPos = _buf.position();
	rewardInfo.ReadUnzipBuf(_buf, _rewardInfoCurPos + _rewardInfoCustLen);
	_buf.position(_rewardInfoCurPos + _rewardInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.put(hasReward?(byte)1:(byte)0);
	_buf.putInt(rewardInfo.GetBufSize());
	rewardInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)7);
	_buf.put((byte)4);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)7);
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

