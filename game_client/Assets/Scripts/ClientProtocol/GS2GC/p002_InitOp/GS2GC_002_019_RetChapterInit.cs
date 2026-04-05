using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

public class GS2GC_002_019_RetChapterInit : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 位置信息
/// </summary>
private Common.ChapterObj.Chapter_PosInfo posInfo;
/// <summary>
/// 鼓舞信息
/// </summary>
private Common.ChapterObj.Chapter_InspireInfo inspireInfo;
/// <summary>
/// 事件信息
/// </summary>
private Common.ChapterObj.Chapter_EventInfo eventInfo;
/// <summary>
/// 已领取剧情奖励列表
/// </summary>
private List<long> hadDrawPlotRewardList;


public GS2GC_002_019_RetChapterInit() {
	posInfo = new Common.ChapterObj.Chapter_PosInfo();
	inspireInfo = new Common.ChapterObj.Chapter_InspireInfo();
	eventInfo = new Common.ChapterObj.Chapter_EventInfo();
	hadDrawPlotRewardList = new List<long>();
}

public GS2GC_002_019_RetChapterInit(
	Common.ChapterObj.Chapter_PosInfo _posInfo
	, Common.ChapterObj.Chapter_InspireInfo _inspireInfo
	, Common.ChapterObj.Chapter_EventInfo _eventInfo
	, List<long> _hadDrawPlotRewardList
) {	posInfo = _posInfo;
	inspireInfo = _inspireInfo;
	eventInfo = _eventInfo;
	hadDrawPlotRewardList = _hadDrawPlotRewardList;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)19; }

/// <summary>
/// 位置信息
/// </summary>
public Common.ChapterObj.Chapter_PosInfo getPosInfo() { return posInfo; }
/// <summary>
/// 位置信息
/// </summary>
public void setPosInfo(Common.ChapterObj.Chapter_PosInfo _posInfo) { posInfo = _posInfo; }
/// <summary>
/// 鼓舞信息
/// </summary>
public Common.ChapterObj.Chapter_InspireInfo getInspireInfo() { return inspireInfo; }
/// <summary>
/// 鼓舞信息
/// </summary>
public void setInspireInfo(Common.ChapterObj.Chapter_InspireInfo _inspireInfo) { inspireInfo = _inspireInfo; }
/// <summary>
/// 事件信息
/// </summary>
public Common.ChapterObj.Chapter_EventInfo getEventInfo() { return eventInfo; }
/// <summary>
/// 事件信息
/// </summary>
public void setEventInfo(Common.ChapterObj.Chapter_EventInfo _eventInfo) { eventInfo = _eventInfo; }
/// <summary>
/// 已领取剧情奖励列表
/// </summary>
public List<long> getHadDrawPlotRewardList() { return hadDrawPlotRewardList; }
/// <summary>
/// 已领取剧情奖励列表
/// </summary>
public void addHadDrawPlotRewardList(long _hadDrawPlotRewardList) { hadDrawPlotRewardList.Add(_hadDrawPlotRewardList); }


public int GetBufSize() {
	int _size = 16;
	_size += 4 + inspireInfo.GetBufSize();
	_size += 4 + eventInfo.GetBufSize();
	_size += 2 + (hadDrawPlotRewardList.Count * 8);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 18;
	_size += 4 + inspireInfo.GetBufSize();
	_size += 4 + eventInfo.GetBufSize();
	_size += 2 + (hadDrawPlotRewardList.Count * 8);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _posInfoCustLen = _buf.getInt();
	int _posInfoCurPos = _buf.getCurPos();
	posInfo.ReadUnzipBuf(_buf, _posInfoCurPos + _posInfoCustLen);
	_buf.setPosition(_posInfoCurPos + _posInfoCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _inspireInfoCustLen = _buf.getInt();
	int _inspireInfoCurPos = _buf.getCurPos();
	inspireInfo.ReadUnzipBuf(_buf, _inspireInfoCurPos + _inspireInfoCustLen);
	_buf.setPosition(_inspireInfoCurPos + _inspireInfoCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _eventInfoCustLen = _buf.getInt();
	int _eventInfoCurPos = _buf.getCurPos();
	eventInfo.ReadUnzipBuf(_buf, _eventInfoCurPos + _eventInfoCustLen);
	_buf.setPosition(_eventInfoCurPos + _eventInfoCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _hadDrawPlotRewardListCount = _buf.getShort();
	for(int _i = 0; _i < _hadDrawPlotRewardListCount; _i++) { 
		long _hadDrawPlotRewardList = (long)0;
		_hadDrawPlotRewardList = _buf.getLong();
		hadDrawPlotRewardList.Add(_hadDrawPlotRewardList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(posInfo.GetBufSize());
	posInfo.PutUnzipBuf(_buf);
	_buf.putInt(inspireInfo.GetBufSize());
	inspireInfo.PutUnzipBuf(_buf);
	_buf.putInt(eventInfo.GetBufSize());
	eventInfo.PutUnzipBuf(_buf);
	_buf.putShort((short)hadDrawPlotRewardList.Count);
	for(int _i = 0; _i < hadDrawPlotRewardList.Count; _i++) { 
		_buf.putLong(hadDrawPlotRewardList[_i]);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)19);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)19);
	PutUnzipBuf(_recBuf);
}
public byte[] makePackage() {
	int _bufSize = GetBufSize();
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void readPackage(byte[] _buf) {
	ALProtocolBuf _bufObj = new ALProtocolBuf(_buf);
	ReadUnzipBuf(_bufObj, -1);
}
public void readPackage(ALProtocolBuf _buf) {
	ReadUnzipBuf(_buf, -1);
}
public override string ToString() {
	System.Text.StringBuilder builder = new System.Text.StringBuilder();

	builder.Append("{");
	builder.Append("posInfo").Append(":").Append(posInfo == null ? "null" : posInfo.ToString()).Append(", ");
	builder.Append("inspireInfo").Append(":").Append(inspireInfo == null ? "null" : inspireInfo.ToString()).Append(", ");
	builder.Append("eventInfo").Append(":").Append(eventInfo == null ? "null" : eventInfo.ToString()).Append(", ");
	builder.Append("hadDrawPlotRewardList").Append(":").Append(hadDrawPlotRewardList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

