package Common.TreasureHuntObj;

import java.nio.ByteBuffer;
/*********
 * 太空寻宝-矿石排行榜项
 **/
public class TreasureHunt_OreRankItem implements ALBasicProtocolPack._IALProtocolStructure {
/** 排名 */
private int rank;
/** cid */
private long cid;
/** 重量 */
private int weight;


public TreasureHunt_OreRankItem() {
	rank = 0;
	cid = (long)0;
	weight = 0;
}

public TreasureHunt_OreRankItem(
	 int _rank
	, long _cid
	, int _weight
) {	rank = _rank;
	cid = _cid;
	weight = _weight;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 排名 */
public int getRank() { return rank; }
/** 排名 */
public void setRank(int _rank) { rank = _rank; }
/** cid */
public long getCid() { return cid; }
/** cid */
public void setCid(long _cid) { cid = _cid; }
/** 重量 */
public int getWeight() { return weight; }
/** 重量 */
public void setWeight(int _weight) { weight = _weight; }


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
	if(_buf.remaining() > 0) rank = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) weight = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(rank);
	_buf.putLong(cid);
	_buf.putInt(weight);
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

