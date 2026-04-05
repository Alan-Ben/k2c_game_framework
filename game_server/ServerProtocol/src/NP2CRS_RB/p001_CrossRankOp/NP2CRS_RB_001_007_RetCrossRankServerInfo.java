package NP2CRS_RB.p001_CrossRankOp;

import java.nio.ByteBuffer;
public class NP2CRS_RB_001_007_RetCrossRankServerInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 已经承载实例数量 */
private int hadHandleInstanceNum;
/** 承载实例数量上限 */
private int handleInstanceLimit;


public NP2CRS_RB_001_007_RetCrossRankServerInfo() {
	hadHandleInstanceNum = 0;
	handleInstanceLimit = 0;
}

public NP2CRS_RB_001_007_RetCrossRankServerInfo(
	 int _hadHandleInstanceNum
	, int _handleInstanceLimit
) {	hadHandleInstanceNum = _hadHandleInstanceNum;
	handleInstanceLimit = _handleInstanceLimit;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)7; }

/** 已经承载实例数量 */
public int getHadHandleInstanceNum() { return hadHandleInstanceNum; }
/** 已经承载实例数量 */
public void setHadHandleInstanceNum(int _hadHandleInstanceNum) { hadHandleInstanceNum = _hadHandleInstanceNum; }
/** 承载实例数量上限 */
public int getHandleInstanceLimit() { return handleInstanceLimit; }
/** 承载实例数量上限 */
public void setHandleInstanceLimit(int _handleInstanceLimit) { handleInstanceLimit = _handleInstanceLimit; }


public final int GetBufSize() {
	int _size = 8;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hadHandleInstanceNum = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) handleInstanceLimit = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(hadHandleInstanceNum);
	_buf.putInt(handleInstanceLimit);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)7);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)7);
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

