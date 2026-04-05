package GS2GC.p028_QuestOp;

import java.nio.ByteBuffer;
public class GS2GC_028_050_OnPlayerQuestChg implements ALBasicProtocolPack._IALProtocolStructure {
private Common.QuestObj.Quest_info quest;


public GS2GC_028_050_OnPlayerQuestChg() {
	quest = new Common.QuestObj.Quest_info();
}

public GS2GC_028_050_OnPlayerQuestChg(
	 Common.QuestObj.Quest_info _quest
) {	quest = _quest;
}

public final byte getMainOrder() { return (byte)28; }

public final byte getSubOrder() { return (byte)50; }

public Common.QuestObj.Quest_info getQuest() { return quest; }
public void setQuest(Common.QuestObj.Quest_info _quest) { quest = _quest; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + quest.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + quest.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _questCustLen = _buf.getInt();
	int _questCurPos = _buf.position();
	quest.ReadUnzipBuf(_buf, _questCurPos + _questCustLen);
	_buf.position(_questCurPos + _questCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(quest.GetBufSize());
	quest.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)28);
	_buf.put((byte)50);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)28);
	_recBuf.put((byte)50);
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

