package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
public class GS2GC_002_255_OnQuestCountInit implements ALBasicProtocolPack._IALProtocolStructure {
/** 任务计数列表 */
private java.util.ArrayList<Common.QuestObj.Quest_Count> questCountList;


public GS2GC_002_255_OnQuestCountInit() {
	questCountList = new java.util.ArrayList<Common.QuestObj.Quest_Count>();
}

public GS2GC_002_255_OnQuestCountInit(
	 java.util.ArrayList<Common.QuestObj.Quest_Count> _questCountList
) {	questCountList = _questCountList;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)255; }

/** 任务计数列表 */
public java.util.ArrayList<Common.QuestObj.Quest_Count> getQuestCountList() { return questCountList; }
/** 任务计数列表 */
public void addQuestCountList(Common.QuestObj.Quest_Count _questCountList) { questCountList.add(_questCountList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (questCountList.size() * 16);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (questCountList.size() * 16);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _questCountListCount = _buf.getShort();
	for(int _i = 0; _i < _questCountListCount; _i++) { 
		Common.QuestObj.Quest_Count _questCountList = new Common.QuestObj.Quest_Count();
		if(_buf.remaining() <= 0) return;
	int __questCountListCustLen = _buf.getInt();
	int __questCountListCurPos = _buf.position();
	_questCountList.ReadUnzipBuf(_buf, __questCountListCurPos + __questCountListCustLen);
	_buf.position(__questCountListCurPos + __questCountListCustLen);

		questCountList.add(_questCountList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)questCountList.size());
	for(int _i = 0; _i < questCountList.size(); _i++) { 
		_buf.putInt(questCountList.get(_i).GetBufSize());
	questCountList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)255);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)255);
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

