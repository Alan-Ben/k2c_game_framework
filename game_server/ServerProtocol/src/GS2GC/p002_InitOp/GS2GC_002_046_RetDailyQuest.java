package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
/*********
 * 日常任务
 **/
public class GS2GC_002_046_RetDailyQuest implements ALBasicProtocolPack._IALProtocolStructure {
/** 日常任务数据 */
private java.util.ArrayList<Common.QuestObj.DailyQuest_Group> dailyquestInfoList;


public GS2GC_002_046_RetDailyQuest() {
	dailyquestInfoList = new java.util.ArrayList<Common.QuestObj.DailyQuest_Group>();
}

public GS2GC_002_046_RetDailyQuest(
	 java.util.ArrayList<Common.QuestObj.DailyQuest_Group> _dailyquestInfoList
) {	dailyquestInfoList = _dailyquestInfoList;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)46; }

/** 日常任务数据 */
public java.util.ArrayList<Common.QuestObj.DailyQuest_Group> getDailyquestInfoList() { return dailyquestInfoList; }
/** 日常任务数据 */
public void addDailyquestInfoList(Common.QuestObj.DailyQuest_Group _dailyquestInfoList) { dailyquestInfoList.add(_dailyquestInfoList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2;
	for(int _i = 0; _i < dailyquestInfoList.size(); _i++) {
	_size += 4 + dailyquestInfoList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
	for(int _i = 0; _i < dailyquestInfoList.size(); _i++) {
	_size += 4 + dailyquestInfoList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _dailyquestInfoListCount = _buf.getShort();
	for(int _i = 0; _i < _dailyquestInfoListCount; _i++) { 
		Common.QuestObj.DailyQuest_Group _dailyquestInfoList = new Common.QuestObj.DailyQuest_Group();
		if(_buf.remaining() <= 0) return;
	int __dailyquestInfoListCustLen = _buf.getInt();
	int __dailyquestInfoListCurPos = _buf.position();
	_dailyquestInfoList.ReadUnzipBuf(_buf, __dailyquestInfoListCurPos + __dailyquestInfoListCustLen);
	_buf.position(__dailyquestInfoListCurPos + __dailyquestInfoListCustLen);

		dailyquestInfoList.add(_dailyquestInfoList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)dailyquestInfoList.size());
	for(int _i = 0; _i < dailyquestInfoList.size(); _i++) { 
		_buf.putInt(dailyquestInfoList.get(_i).GetBufSize());
	dailyquestInfoList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)46);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)46);
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

