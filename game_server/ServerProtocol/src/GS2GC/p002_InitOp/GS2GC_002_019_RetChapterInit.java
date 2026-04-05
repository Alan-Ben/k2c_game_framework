package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
public class GS2GC_002_019_RetChapterInit implements ALBasicProtocolPack._IALProtocolStructure {
/** 位置信息 */
private Common.ChapterObj.Chapter_PosInfo posInfo;
/** 鼓舞信息 */
private Common.ChapterObj.Chapter_InspireInfo inspireInfo;
/** 事件信息 */
private Common.ChapterObj.Chapter_EventInfo eventInfo;
/** 已领取剧情奖励列表 */
private java.util.ArrayList<Long> hadDrawPlotRewardList;


public GS2GC_002_019_RetChapterInit() {
	posInfo = new Common.ChapterObj.Chapter_PosInfo();
	inspireInfo = new Common.ChapterObj.Chapter_InspireInfo();
	eventInfo = new Common.ChapterObj.Chapter_EventInfo();
	hadDrawPlotRewardList = new java.util.ArrayList<Long>();
}

public GS2GC_002_019_RetChapterInit(
	 Common.ChapterObj.Chapter_PosInfo _posInfo
	, Common.ChapterObj.Chapter_InspireInfo _inspireInfo
	, Common.ChapterObj.Chapter_EventInfo _eventInfo
	, java.util.ArrayList<Long> _hadDrawPlotRewardList
) {	posInfo = _posInfo;
	inspireInfo = _inspireInfo;
	eventInfo = _eventInfo;
	hadDrawPlotRewardList = _hadDrawPlotRewardList;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)19; }

/** 位置信息 */
public Common.ChapterObj.Chapter_PosInfo getPosInfo() { return posInfo; }
/** 位置信息 */
public void setPosInfo(Common.ChapterObj.Chapter_PosInfo _posInfo) { posInfo = _posInfo; }
/** 鼓舞信息 */
public Common.ChapterObj.Chapter_InspireInfo getInspireInfo() { return inspireInfo; }
/** 鼓舞信息 */
public void setInspireInfo(Common.ChapterObj.Chapter_InspireInfo _inspireInfo) { inspireInfo = _inspireInfo; }
/** 事件信息 */
public Common.ChapterObj.Chapter_EventInfo getEventInfo() { return eventInfo; }
/** 事件信息 */
public void setEventInfo(Common.ChapterObj.Chapter_EventInfo _eventInfo) { eventInfo = _eventInfo; }
/** 已领取剧情奖励列表 */
public java.util.ArrayList<Long> getHadDrawPlotRewardList() { return hadDrawPlotRewardList; }
/** 已领取剧情奖励列表 */
public void addHadDrawPlotRewardList(long _hadDrawPlotRewardList) { hadDrawPlotRewardList.add(_hadDrawPlotRewardList); }


public final int GetBufSize() {
	int _size = 16;
	_size += 4 + inspireInfo.GetBufSize();
	_size += 4 + eventInfo.GetBufSize();
	_size += 2 + (hadDrawPlotRewardList.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;
	_size += 4 + inspireInfo.GetBufSize();
	_size += 4 + eventInfo.GetBufSize();
	_size += 2 + (hadDrawPlotRewardList.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _posInfoCustLen = _buf.getInt();
	int _posInfoCurPos = _buf.position();
	posInfo.ReadUnzipBuf(_buf, _posInfoCurPos + _posInfoCustLen);
	_buf.position(_posInfoCurPos + _posInfoCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _inspireInfoCustLen = _buf.getInt();
	int _inspireInfoCurPos = _buf.position();
	inspireInfo.ReadUnzipBuf(_buf, _inspireInfoCurPos + _inspireInfoCustLen);
	_buf.position(_inspireInfoCurPos + _inspireInfoCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _eventInfoCustLen = _buf.getInt();
	int _eventInfoCurPos = _buf.position();
	eventInfo.ReadUnzipBuf(_buf, _eventInfoCurPos + _eventInfoCustLen);
	_buf.position(_eventInfoCurPos + _eventInfoCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _hadDrawPlotRewardListCount = _buf.getShort();
	for(int _i = 0; _i < _hadDrawPlotRewardListCount; _i++) { 
		long _hadDrawPlotRewardList = (long)0;
		if(_buf.remaining() > 0) _hadDrawPlotRewardList = _buf.getLong();
		hadDrawPlotRewardList.add(_hadDrawPlotRewardList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(posInfo.GetBufSize());
	posInfo.PutUnzipBuf(_buf);
	_buf.putInt(inspireInfo.GetBufSize());
	inspireInfo.PutUnzipBuf(_buf);
	_buf.putInt(eventInfo.GetBufSize());
	eventInfo.PutUnzipBuf(_buf);
	_buf.putShort((short)hadDrawPlotRewardList.size());
	for(int _i = 0; _i < hadDrawPlotRewardList.size(); _i++) { 
		_buf.putLong(hadDrawPlotRewardList.get(_i));
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)19);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)19);
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

