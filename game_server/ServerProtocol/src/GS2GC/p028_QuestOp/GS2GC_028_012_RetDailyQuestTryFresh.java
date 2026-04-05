package GS2GC.p028_QuestOp;

import java.nio.ByteBuffer;
/*********
 * 尝试刷新任务数据
 **/
public class GS2GC_028_012_RetDailyQuestTryFresh implements ALBasicProtocolPack._IALProtocolStructure {
/** 日常任务数据 */
private Common.QuestObj.DailyQuest_Group dailyquestInfo;


public GS2GC_028_012_RetDailyQuestTryFresh() {
	dailyquestInfo = new Common.QuestObj.DailyQuest_Group();
}

public GS2GC_028_012_RetDailyQuestTryFresh(
	 Common.QuestObj.DailyQuest_Group _dailyquestInfo
) {	dailyquestInfo = _dailyquestInfo;
}

public final byte getMainOrder() { return (byte)28; }

public final byte getSubOrder() { return (byte)12; }

/** 日常任务数据 */
public Common.QuestObj.DailyQuest_Group getDailyquestInfo() { return dailyquestInfo; }
/** 日常任务数据 */
public void setDailyquestInfo(Common.QuestObj.DailyQuest_Group _dailyquestInfo) { dailyquestInfo = _dailyquestInfo; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + dailyquestInfo.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + dailyquestInfo.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _dailyquestInfoCustLen = _buf.getInt();
	int _dailyquestInfoCurPos = _buf.position();
	dailyquestInfo.ReadUnzipBuf(_buf, _dailyquestInfoCurPos + _dailyquestInfoCustLen);
	_buf.position(_dailyquestInfoCurPos + _dailyquestInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(dailyquestInfo.GetBufSize());
	dailyquestInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)28);
	_buf.put((byte)12);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)28);
	_recBuf.put((byte)12);
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

