package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
/*********
 * 政务初始化
 **/
public class GS2GC_002_009_RetAnecdoteInit implements ALBasicProtocolPack._IALProtocolStructure {
/** 事件数据列表 */
private java.util.ArrayList<Common.AnecdoteObj.Anecdote_EventInfo> eventList;


public GS2GC_002_009_RetAnecdoteInit() {
	eventList = new java.util.ArrayList<Common.AnecdoteObj.Anecdote_EventInfo>();
}

public GS2GC_002_009_RetAnecdoteInit(
	 java.util.ArrayList<Common.AnecdoteObj.Anecdote_EventInfo> _eventList
) {	eventList = _eventList;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)9; }

/** 事件数据列表 */
public java.util.ArrayList<Common.AnecdoteObj.Anecdote_EventInfo> getEventList() { return eventList; }
/** 事件数据列表 */
public void addEventList(Common.AnecdoteObj.Anecdote_EventInfo _eventList) { eventList.add(_eventList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2;
	for(int _i = 0; _i < eventList.size(); _i++) {
	_size += 4 + eventList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
	for(int _i = 0; _i < eventList.size(); _i++) {
	_size += 4 + eventList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _eventListCount = _buf.getShort();
	for(int _i = 0; _i < _eventListCount; _i++) { 
		Common.AnecdoteObj.Anecdote_EventInfo _eventList = new Common.AnecdoteObj.Anecdote_EventInfo();
		if(_buf.remaining() <= 0) return;
	int __eventListCustLen = _buf.getInt();
	int __eventListCurPos = _buf.position();
	_eventList.ReadUnzipBuf(_buf, __eventListCurPos + __eventListCustLen);
	_buf.position(__eventListCurPos + __eventListCustLen);

		eventList.add(_eventList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)eventList.size());
	for(int _i = 0; _i < eventList.size(); _i++) { 
		_buf.putInt(eventList.get(_i).GetBufSize());
	eventList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)9);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)9);
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

