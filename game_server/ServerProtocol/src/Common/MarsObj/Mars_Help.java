package Common.MarsObj;

import java.nio.ByteBuffer;
/*********
 * 火星居民-求助数据
 **/
public class Mars_Help implements ALBasicProtocolPack._IALProtocolStructure {
/** 实例ID */
private long id;
/** 信件ID */
private long helpId;
private long npcId;
private int chooseIdx;
private boolean isFinish;


public Mars_Help() {
	id = (long)0;
	helpId = (long)0;
	npcId = (long)0;
	chooseIdx = 0;
	isFinish = false;
}

public Mars_Help(
	 long _id
	, long _helpId
	, long _npcId
	, int _chooseIdx
	, boolean _isFinish
) {	id = _id;
	helpId = _helpId;
	npcId = _npcId;
	chooseIdx = _chooseIdx;
	isFinish = _isFinish;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 实例ID */
public long getId() { return id; }
/** 实例ID */
public void setId(long _id) { id = _id; }
/** 信件ID */
public long getHelpId() { return helpId; }
/** 信件ID */
public void setHelpId(long _helpId) { helpId = _helpId; }
public long getNpcId() { return npcId; }
public void setNpcId(long _npcId) { npcId = _npcId; }
public int getChooseIdx() { return chooseIdx; }
public void setChooseIdx(int _chooseIdx) { chooseIdx = _chooseIdx; }
public boolean getIsFinish() { return isFinish; }
public void setIsFinish(boolean _isFinish) { isFinish = _isFinish; }


public final int GetBufSize() {
	int _size = 29;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 31;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) id = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) helpId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) npcId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) chooseIdx = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isFinish = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(id);
	_buf.putLong(helpId);
	_buf.putLong(npcId);
	_buf.putInt(chooseIdx);
	_buf.put(isFinish?(byte)1:(byte)0);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
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

