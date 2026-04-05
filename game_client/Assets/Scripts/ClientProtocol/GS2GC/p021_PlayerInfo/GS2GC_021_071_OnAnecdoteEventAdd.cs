using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p021_PlayerInfo
{

/// <summary>
/// 新增政务事件
/// </summary>
public class GS2GC_021_071_OnAnecdoteEventAdd : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 事件数据列表
/// </summary>
private List<Common.AnecdoteObj.Anecdote_EventInfo> eventList;


public GS2GC_021_071_OnAnecdoteEventAdd() {
	eventList = new List<Common.AnecdoteObj.Anecdote_EventInfo>();
}

public GS2GC_021_071_OnAnecdoteEventAdd(
	List<Common.AnecdoteObj.Anecdote_EventInfo> _eventList
) {	eventList = _eventList;
}

public byte getMainOrder() { return (byte)21; }

public byte getSubOrder() { return (byte)71; }

/// <summary>
/// 事件数据列表
/// </summary>
public List<Common.AnecdoteObj.Anecdote_EventInfo> getEventList() { return eventList; }
/// <summary>
/// 事件数据列表
/// </summary>
public void addEventList(Common.AnecdoteObj.Anecdote_EventInfo _eventList) { eventList.Add(_eventList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2;
for(int _i = 0; _i < eventList.Count; _i++) {
	_size += 4 + eventList[_i].GetBufSize();
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
for(int _i = 0; _i < eventList.Count; _i++) {
	_size += 4 + eventList[_i].GetBufSize();
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _eventListCount = _buf.getShort();
	for(int _i = 0; _i < _eventListCount; _i++) { 
		Common.AnecdoteObj.Anecdote_EventInfo _eventList = new Common.AnecdoteObj.Anecdote_EventInfo();
		int __eventListCustLen = _buf.getInt();
	int __eventListCurPos = _buf.getCurPos();
	_eventList.ReadUnzipBuf(_buf, __eventListCurPos + __eventListCustLen);
	_buf.setPosition(__eventListCurPos + __eventListCustLen);

		eventList.Add(_eventList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)eventList.Count);
	for(int _i = 0; _i < eventList.Count; _i++) { 
		_buf.putInt(eventList[_i].GetBufSize());
	eventList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)21);
	_buf.put((byte)71);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)21);
	_recBuf.put((byte)71);
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
	builder.Append("eventList").Append(":").Append(eventList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

