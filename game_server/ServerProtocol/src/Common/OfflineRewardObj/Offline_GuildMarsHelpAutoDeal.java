package Common.OfflineRewardObj;

import java.nio.ByteBuffer;
/*********
 * 公会-火星互助-帮助
 **/
public class Offline_GuildMarsHelpAutoDeal implements ALBasicProtocolPack._IALProtocolStructure {
/** 帮助次数 */
private int dealedCount;


public Offline_GuildMarsHelpAutoDeal() {
	dealedCount = 0;
}

public Offline_GuildMarsHelpAutoDeal(
	 int _dealedCount
) {	dealedCount = _dealedCount;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 帮助次数 */
public int getDealedCount() { return dealedCount; }
/** 帮助次数 */
public void setDealedCount(int _dealedCount) { dealedCount = _dealedCount; }


public final int GetBufSize() {
	int _size = 4;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 6;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dealedCount = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(dealedCount);
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

