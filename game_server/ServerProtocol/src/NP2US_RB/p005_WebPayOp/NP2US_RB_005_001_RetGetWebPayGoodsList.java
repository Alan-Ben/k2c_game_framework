package NP2US_RB.p005_WebPayOp;

import java.nio.ByteBuffer;
/*********
 * 返回网页支付商品列表
 **/
public class NP2US_RB_005_001_RetGetWebPayGoodsList implements ALBasicProtocolPack._IALProtocolStructure {
/** 商品列表 */
private Common.ServerObj.ServerObj_WebPayGoodsList goodsList;


public NP2US_RB_005_001_RetGetWebPayGoodsList() {
	goodsList = new Common.ServerObj.ServerObj_WebPayGoodsList();
}

public NP2US_RB_005_001_RetGetWebPayGoodsList(
	 Common.ServerObj.ServerObj_WebPayGoodsList _goodsList
) {	goodsList = _goodsList;
}

public final byte getMainOrder() { return (byte)5; }

public final byte getSubOrder() { return (byte)1; }

/** 商品列表 */
public Common.ServerObj.ServerObj_WebPayGoodsList getGoodsList() { return goodsList; }
/** 商品列表 */
public void setGoodsList(Common.ServerObj.ServerObj_WebPayGoodsList _goodsList) { goodsList = _goodsList; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + goodsList.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + goodsList.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _goodsListCustLen = _buf.getInt();
	int _goodsListCurPos = _buf.position();
	goodsList.ReadUnzipBuf(_buf, _goodsListCurPos + _goodsListCustLen);
	_buf.position(_goodsListCurPos + _goodsListCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(goodsList.GetBufSize());
	goodsList.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)5);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)5);
	_recBuf.put((byte)1);
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

