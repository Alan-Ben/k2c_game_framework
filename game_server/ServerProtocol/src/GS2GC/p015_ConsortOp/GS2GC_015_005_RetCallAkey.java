package GS2GC.p015_ConsortOp;

import java.nio.ByteBuffer;
/*********
 * 家人-一键邀约
 **/
public class GS2GC_015_005_RetCallAkey implements ALBasicProtocolPack._IALProtocolStructure {
/** 邀约结果列表 */
private java.util.ArrayList<Common.ConsortObj.Consort_CallRes> resList;


public GS2GC_015_005_RetCallAkey() {
	resList = new java.util.ArrayList<Common.ConsortObj.Consort_CallRes>();
}

public GS2GC_015_005_RetCallAkey(
	 java.util.ArrayList<Common.ConsortObj.Consort_CallRes> _resList
) {	resList = _resList;
}

public final byte getMainOrder() { return (byte)15; }

public final byte getSubOrder() { return (byte)5; }

/** 邀约结果列表 */
public java.util.ArrayList<Common.ConsortObj.Consort_CallRes> getResList() { return resList; }
/** 邀约结果列表 */
public void addResList(Common.ConsortObj.Consort_CallRes _resList) { resList.add(_resList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (resList.size() * 37);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (resList.size() * 37);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _resListCount = _buf.getShort();
	for(int _i = 0; _i < _resListCount; _i++) { 
		Common.ConsortObj.Consort_CallRes _resList = new Common.ConsortObj.Consort_CallRes();
		if(_buf.remaining() <= 0) return;
	int __resListCustLen = _buf.getInt();
	int __resListCurPos = _buf.position();
	_resList.ReadUnzipBuf(_buf, __resListCurPos + __resListCustLen);
	_buf.position(__resListCurPos + __resListCustLen);

		resList.add(_resList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)resList.size());
	for(int _i = 0; _i < resList.size(); _i++) { 
		_buf.putInt(resList.get(_i).GetBufSize());
	resList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)15);
	_buf.put((byte)5);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)15);
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

