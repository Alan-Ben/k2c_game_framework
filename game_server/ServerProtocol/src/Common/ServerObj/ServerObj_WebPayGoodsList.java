package Common.ServerObj;

import java.nio.ByteBuffer;
/*********
 * 网页支付商品列表
 **/
public class ServerObj_WebPayGoodsList implements ALBasicProtocolPack._IALProtocolStructure {
/** 商品列表 */
private java.util.ArrayList<Common.ServerObj.ServerObj_WebPayGoodsInfo> goodsList;


public ServerObj_WebPayGoodsList() {
	goodsList = new java.util.ArrayList<Common.ServerObj.ServerObj_WebPayGoodsInfo>();
}

public ServerObj_WebPayGoodsList(
	 java.util.ArrayList<Common.ServerObj.ServerObj_WebPayGoodsInfo> _goodsList
) {	goodsList = _goodsList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 商品列表 */
public java.util.ArrayList<Common.ServerObj.ServerObj_WebPayGoodsInfo> getGoodsList() { return goodsList; }
/** 商品列表 */
public void addGoodsList(Common.ServerObj.ServerObj_WebPayGoodsInfo _goodsList) { goodsList.add(_goodsList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2;
	for(int _i = 0; _i < goodsList.size(); _i++) {
	_size += 4 + goodsList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
	for(int _i = 0; _i < goodsList.size(); _i++) {
	_size += 4 + goodsList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _goodsListCount = _buf.getShort();
	for(int _i = 0; _i < _goodsListCount; _i++) { 
		Common.ServerObj.ServerObj_WebPayGoodsInfo _goodsList = new Common.ServerObj.ServerObj_WebPayGoodsInfo();
		if(_buf.remaining() <= 0) return;
	int __goodsListCustLen = _buf.getInt();
	int __goodsListCurPos = _buf.position();
	_goodsList.ReadUnzipBuf(_buf, __goodsListCurPos + __goodsListCustLen);
	_buf.position(__goodsListCurPos + __goodsListCustLen);

		goodsList.add(_goodsList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)goodsList.size());
	for(int _i = 0; _i < goodsList.size(); _i++) { 
		_buf.putInt(goodsList.get(_i).GetBufSize());
	goodsList.get(_i).PutUnzipBuf(_buf);
	}
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

