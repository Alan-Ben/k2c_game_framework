package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
public class GS2GC_002_042_RetQuestionnaireInit implements ALBasicProtocolPack._IALProtocolStructure {
/** 问卷列表 */
private java.util.ArrayList<Common.Common_QuestionnaireInfo> infoList;
/** 问卷奖励列表 */
private java.util.ArrayList<Common.Common_QuestionnaireRewardInfo> rewardList;


public GS2GC_002_042_RetQuestionnaireInit() {
	infoList = new java.util.ArrayList<Common.Common_QuestionnaireInfo>();
	rewardList = new java.util.ArrayList<Common.Common_QuestionnaireRewardInfo>();
}

public GS2GC_002_042_RetQuestionnaireInit(
	 java.util.ArrayList<Common.Common_QuestionnaireInfo> _infoList
	, java.util.ArrayList<Common.Common_QuestionnaireRewardInfo> _rewardList
) {	infoList = _infoList;
	rewardList = _rewardList;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)42; }

/** 问卷列表 */
public java.util.ArrayList<Common.Common_QuestionnaireInfo> getInfoList() { return infoList; }
/** 问卷列表 */
public void addInfoList(Common.Common_QuestionnaireInfo _infoList) { infoList.add(_infoList); }
/** 问卷奖励列表 */
public java.util.ArrayList<Common.Common_QuestionnaireRewardInfo> getRewardList() { return rewardList; }
/** 问卷奖励列表 */
public void addRewardList(Common.Common_QuestionnaireRewardInfo _rewardList) { rewardList.add(_rewardList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2;
	for(int _i = 0; _i < infoList.size(); _i++) {
	_size += 4 + infoList.get(_i).GetBufSize();
	}

	_size += 2 + (rewardList.size() * 13);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
	for(int _i = 0; _i < infoList.size(); _i++) {
	_size += 4 + infoList.get(_i).GetBufSize();
	}

	_size += 2 + (rewardList.size() * 13);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _infoListCount = _buf.getShort();
	for(int _i = 0; _i < _infoListCount; _i++) { 
		Common.Common_QuestionnaireInfo _infoList = new Common.Common_QuestionnaireInfo();
		if(_buf.remaining() <= 0) return;
	int __infoListCustLen = _buf.getInt();
	int __infoListCurPos = _buf.position();
	_infoList.ReadUnzipBuf(_buf, __infoListCurPos + __infoListCustLen);
	_buf.position(__infoListCurPos + __infoListCustLen);

		infoList.add(_infoList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _rewardListCount = _buf.getShort();
	for(int _i = 0; _i < _rewardListCount; _i++) { 
		Common.Common_QuestionnaireRewardInfo _rewardList = new Common.Common_QuestionnaireRewardInfo();
		if(_buf.remaining() <= 0) return;
	int __rewardListCustLen = _buf.getInt();
	int __rewardListCurPos = _buf.position();
	_rewardList.ReadUnzipBuf(_buf, __rewardListCurPos + __rewardListCustLen);
	_buf.position(__rewardListCurPos + __rewardListCustLen);

		rewardList.add(_rewardList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)infoList.size());
	for(int _i = 0; _i < infoList.size(); _i++) { 
		_buf.putInt(infoList.get(_i).GetBufSize());
	infoList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putShort((short)rewardList.size());
	for(int _i = 0; _i < rewardList.size(); _i++) { 
		_buf.putInt(rewardList.get(_i).GetBufSize());
	rewardList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)42);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)42);
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

