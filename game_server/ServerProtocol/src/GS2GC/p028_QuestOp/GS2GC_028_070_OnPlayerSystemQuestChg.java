package GS2GC.p028_QuestOp;

import java.nio.ByteBuffer;
/*********
 * 系统任务变更
 **/
public class GS2GC_028_070_OnPlayerSystemQuestChg implements ALBasicProtocolPack._IALProtocolStructure {
private Common.QuestObj.SystemQuest_Info info;


public GS2GC_028_070_OnPlayerSystemQuestChg() {
	info = new Common.QuestObj.SystemQuest_Info();
}

public GS2GC_028_070_OnPlayerSystemQuestChg(
	 Common.QuestObj.SystemQuest_Info _info
) {	info = _info;
}

public final byte getMainOrder() { return (byte)28; }

public final byte getSubOrder() { return (byte)70; }

public Common.QuestObj.SystemQuest_Info getInfo() { return info; }
public void setInfo(Common.QuestObj.SystemQuest_Info _info) { info = _info; }


public final int GetBufSize() {
	int _size = 24;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 26;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _infoCustLen = _buf.getInt();
	int _infoCurPos = _buf.position();
	info.ReadUnzipBuf(_buf, _infoCurPos + _infoCustLen);
	_buf.position(_infoCurPos + _infoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(info.GetBufSize());
	info.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)28);
	_buf.put((byte)70);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)28);
	_recBuf.put((byte)70);
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

