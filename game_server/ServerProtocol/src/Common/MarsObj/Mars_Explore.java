package Common.MarsObj;

import java.nio.ByteBuffer;
/*********
 * 火星探索-基础数据
 **/
public class Mars_Explore implements ALBasicProtocolPack._IALProtocolStructure {
private int lvl;
/** 探索次数 */
private int exploreSum;


public Mars_Explore() {
	lvl = 0;
	exploreSum = 0;
}

public Mars_Explore(
	 int _lvl
	, int _exploreSum
) {	lvl = _lvl;
	exploreSum = _exploreSum;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public int getLvl() { return lvl; }
public void setLvl(int _lvl) { lvl = _lvl; }
/** 探索次数 */
public int getExploreSum() { return exploreSum; }
/** 探索次数 */
public void setExploreSum(int _exploreSum) { exploreSum = _exploreSum; }


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
	if(_buf.remaining() > 0) lvl = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) exploreSum = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(lvl);
	_buf.putInt(exploreSum);
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

