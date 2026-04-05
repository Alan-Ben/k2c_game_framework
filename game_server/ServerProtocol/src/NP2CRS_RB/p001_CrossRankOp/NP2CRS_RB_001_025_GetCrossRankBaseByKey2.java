package NP2CRS_RB.p001_CrossRankOp;

import java.nio.ByteBuffer;
public class NP2CRS_RB_001_025_GetCrossRankBaseByKey2 implements ALBasicProtocolPack._IALProtocolStructure {
private Common.RankObj.Rank_BaseItem rankItem;


public NP2CRS_RB_001_025_GetCrossRankBaseByKey2() {
	rankItem = new Common.RankObj.Rank_BaseItem();
}

public NP2CRS_RB_001_025_GetCrossRankBaseByKey2(
	 Common.RankObj.Rank_BaseItem _rankItem
) {	rankItem = _rankItem;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)25; }

public Common.RankObj.Rank_BaseItem getRankItem() { return rankItem; }
public void setRankItem(Common.RankObj.Rank_BaseItem _rankItem) { rankItem = _rankItem; }


public final int GetBufSize() {
	int _size = 32;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 34;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _rankItemCustLen = _buf.getInt();
	int _rankItemCurPos = _buf.position();
	rankItem.ReadUnzipBuf(_buf, _rankItemCurPos + _rankItemCustLen);
	_buf.position(_rankItemCurPos + _rankItemCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(rankItem.GetBufSize());
	rankItem.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)25);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)25);
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

