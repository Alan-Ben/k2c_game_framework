package GS2GC.p028_QuestOp;

import java.nio.ByteBuffer;
public class GS2GC_028_053_OnPlayerQuestCountChg implements ALBasicProtocolPack._IALProtocolStructure {
private Common.QuestObj.Quest_Count questCount;


public GS2GC_028_053_OnPlayerQuestCountChg() {
	questCount = new Common.QuestObj.Quest_Count();
}

public GS2GC_028_053_OnPlayerQuestCountChg(
	 Common.QuestObj.Quest_Count _questCount
) {	questCount = _questCount;
}

public final byte getMainOrder() { return (byte)28; }

public final byte getSubOrder() { return (byte)53; }

public Common.QuestObj.Quest_Count getQuestCount() { return questCount; }
public void setQuestCount(Common.QuestObj.Quest_Count _questCount) { questCount = _questCount; }


public final int GetBufSize() {
	int _size = 16;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _questCountCustLen = _buf.getInt();
	int _questCountCurPos = _buf.position();
	questCount.ReadUnzipBuf(_buf, _questCountCurPos + _questCountCustLen);
	_buf.position(_questCountCurPos + _questCountCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(questCount.GetBufSize());
	questCount.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)28);
	_buf.put((byte)53);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)28);
	_recBuf.put((byte)53);
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

