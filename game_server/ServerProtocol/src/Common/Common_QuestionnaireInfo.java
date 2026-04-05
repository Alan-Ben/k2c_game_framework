package Common;

import java.nio.ByteBuffer;
public class Common_QuestionnaireInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 问卷实例id */
private long questionnaireId;
/** 链接 */
private String urlLink;
/** 问卷编码 */
private String questionnaireCode;
/** 开始时间 */
private long startTimeMs;
/** 结束时间 */
private long endTimeMs;
/** 奖励列表 */
private java.util.ArrayList<NPCommon.NPCommon_ItemInfo> rewardList;


public Common_QuestionnaireInfo() {
	questionnaireId = (long)0;
	urlLink = "";
	questionnaireCode = "";
	startTimeMs = (long)0;
	endTimeMs = (long)0;
	rewardList = new java.util.ArrayList<NPCommon.NPCommon_ItemInfo>();
}

public Common_QuestionnaireInfo(
	 long _questionnaireId
	, String _urlLink
	, String _questionnaireCode
	, long _startTimeMs
	, long _endTimeMs
	, java.util.ArrayList<NPCommon.NPCommon_ItemInfo> _rewardList
) {	questionnaireId = _questionnaireId;
	urlLink = _urlLink;
	questionnaireCode = _questionnaireCode;
	startTimeMs = _startTimeMs;
	endTimeMs = _endTimeMs;
	rewardList = _rewardList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 问卷实例id */
public long getQuestionnaireId() { return questionnaireId; }
/** 问卷实例id */
public void setQuestionnaireId(long _questionnaireId) { questionnaireId = _questionnaireId; }
/** 链接 */
public String getUrlLink() { return urlLink; }
/** 链接 */
public void setUrlLink(String _urlLink) { urlLink = _urlLink; }
/** 问卷编码 */
public String getQuestionnaireCode() { return questionnaireCode; }
/** 问卷编码 */
public void setQuestionnaireCode(String _questionnaireCode) { questionnaireCode = _questionnaireCode; }
/** 开始时间 */
public long getStartTimeMs() { return startTimeMs; }
/** 开始时间 */
public void setStartTimeMs(long _startTimeMs) { startTimeMs = _startTimeMs; }
/** 结束时间 */
public long getEndTimeMs() { return endTimeMs; }
/** 结束时间 */
public void setEndTimeMs(long _endTimeMs) { endTimeMs = _endTimeMs; }
/** 奖励列表 */
public java.util.ArrayList<NPCommon.NPCommon_ItemInfo> getRewardList() { return rewardList; }
/** 奖励列表 */
public void addRewardList(NPCommon.NPCommon_ItemInfo _rewardList) { rewardList.add(_rewardList); }


public final int GetBufSize() {
	int _size = 24;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(urlLink);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(questionnaireCode);
	_size += 2;
	for(int _i = 0; _i < rewardList.size(); _i++) {
	_size += 4 + rewardList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 26;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(urlLink);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(questionnaireCode);
	_size += 2;
	for(int _i = 0; _i < rewardList.size(); _i++) {
	_size += 4 + rewardList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) questionnaireId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) urlLink = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) questionnaireCode = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) startTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) endTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _rewardListCount = _buf.getShort();
	for(int _i = 0; _i < _rewardListCount; _i++) { 
		NPCommon.NPCommon_ItemInfo _rewardList = new NPCommon.NPCommon_ItemInfo();
		if(_buf.remaining() <= 0) return;
	int __rewardListCustLen = _buf.getInt();
	int __rewardListCurPos = _buf.position();
	_rewardList.ReadUnzipBuf(_buf, __rewardListCurPos + __rewardListCustLen);
	_buf.position(__rewardListCurPos + __rewardListCustLen);

		rewardList.add(_rewardList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(questionnaireId);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, urlLink);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, questionnaireCode);
	_buf.putLong(startTimeMs);
	_buf.putLong(endTimeMs);
	_buf.putShort((short)rewardList.size());
	for(int _i = 0; _i < rewardList.size(); _i++) { 
		_buf.putInt(rewardList.get(_i).GetBufSize());
	rewardList.get(_i).PutUnzipBuf(_buf);
	}
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

