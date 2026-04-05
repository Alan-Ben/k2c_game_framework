package GC2GS.p040_MarsPeopleOp;

import java.nio.ByteBuffer;
/*********
 * 火星居民-处理选择帮助
 **/
public class GC2GS_040_005_ReqDealChoiceHelp implements ALBasicProtocolPack._IALProtocolStructure {
/** 实例ID */
private long id;
/** 选择 */
private int choice;


public GC2GS_040_005_ReqDealChoiceHelp() {
	id = (long)0;
	choice = 0;
}

public GC2GS_040_005_ReqDealChoiceHelp(
	 long _id
	, int _choice
) {	id = _id;
	choice = _choice;
}

public final byte getMainOrder() { return (byte)40; }

public final byte getSubOrder() { return (byte)5; }

/** 实例ID */
public long getId() { return id; }
/** 实例ID */
public void setId(long _id) { id = _id; }
/** 选择 */
public int getChoice() { return choice; }
/** 选择 */
public void setChoice(int _choice) { choice = _choice; }


public final int GetBufSize() {
	int _size = 12;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) id = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) choice = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(id);
	_buf.putInt(choice);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)40);
	_buf.put((byte)5);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)40);
	_recBuf.put((byte)5);
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

