package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
public class GS2GC_002_036_RetQuestInit implements ALBasicProtocolPack._IALProtocolStructure {
/** 任务列表 */
private java.util.ArrayList<Common.QuestObj.Quest_info> questList;


public GS2GC_002_036_RetQuestInit() {
	questList = new java.util.ArrayList<Common.QuestObj.Quest_info>();
}

public GS2GC_002_036_RetQuestInit(
	 java.util.ArrayList<Common.QuestObj.Quest_info> _questList
) {	questList = _questList;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)36; }

/** 任务列表 */
public java.util.ArrayList<Common.QuestObj.Quest_info> getQuestList() { return questList; }
/** 任务列表 */
public void addQuestList(Common.QuestObj.Quest_info _questList) { questList.add(_questList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2;
	for(int _i = 0; _i < questList.size(); _i++) {
	_size += 4 + questList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
	for(int _i = 0; _i < questList.size(); _i++) {
	_size += 4 + questList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _questListCount = _buf.getShort();
	for(int _i = 0; _i < _questListCount; _i++) { 
		Common.QuestObj.Quest_info _questList = new Common.QuestObj.Quest_info();
		if(_buf.remaining() <= 0) return;
	int __questListCustLen = _buf.getInt();
	int __questListCurPos = _buf.position();
	_questList.ReadUnzipBuf(_buf, __questListCurPos + __questListCustLen);
	_buf.position(__questListCurPos + __questListCustLen);

		questList.add(_questList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)questList.size());
	for(int _i = 0; _i < questList.size(); _i++) { 
		_buf.putInt(questList.get(_i).GetBufSize());
	questList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)36);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)36);
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

