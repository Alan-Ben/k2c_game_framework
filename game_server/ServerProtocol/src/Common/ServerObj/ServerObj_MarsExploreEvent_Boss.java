package Common.ServerObj;

import java.nio.ByteBuffer;
/*********
 * 火星探索事件-BOSS事件
 **/
public class ServerObj_MarsExploreEvent_Boss implements ALBasicProtocolPack._IALProtocolStructure {
/** 已领奖 */
private boolean hadReward;


public ServerObj_MarsExploreEvent_Boss() {
	hadReward = false;
}

public ServerObj_MarsExploreEvent_Boss(
	 boolean _hadReward
) {	hadReward = _hadReward;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 已领奖 */
public boolean getHadReward() { return hadReward; }
/** 已领奖 */
public void setHadReward(boolean _hadReward) { hadReward = _hadReward; }


public final int GetBufSize() {
	int _size = 1;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 3;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hadReward = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.put(hadReward?(byte)1:(byte)0);
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

