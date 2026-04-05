package GS2GC.p015_ConsortOp;

import java.nio.ByteBuffer;
/*********
 * 家人-获取所有经营技能数据
 **/
public class GS2GC_015_009_RetGetAllBusinessSkill implements ALBasicProtocolPack._IALProtocolStructure {
/** 结果列表 */
private java.util.ArrayList<Common.ConsortObj.Consort_BusinessSkill> dataList;


public GS2GC_015_009_RetGetAllBusinessSkill() {
	dataList = new java.util.ArrayList<Common.ConsortObj.Consort_BusinessSkill>();
}

public GS2GC_015_009_RetGetAllBusinessSkill(
	 java.util.ArrayList<Common.ConsortObj.Consort_BusinessSkill> _dataList
) {	dataList = _dataList;
}

public final byte getMainOrder() { return (byte)15; }

public final byte getSubOrder() { return (byte)9; }

/** 结果列表 */
public java.util.ArrayList<Common.ConsortObj.Consort_BusinessSkill> getDataList() { return dataList; }
/** 结果列表 */
public void addDataList(Common.ConsortObj.Consort_BusinessSkill _dataList) { dataList.add(_dataList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (dataList.size() * 28);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (dataList.size() * 28);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _dataListCount = _buf.getShort();
	for(int _i = 0; _i < _dataListCount; _i++) { 
		Common.ConsortObj.Consort_BusinessSkill _dataList = new Common.ConsortObj.Consort_BusinessSkill();
		if(_buf.remaining() <= 0) return;
	int __dataListCustLen = _buf.getInt();
	int __dataListCurPos = _buf.position();
	_dataList.ReadUnzipBuf(_buf, __dataListCurPos + __dataListCustLen);
	_buf.position(__dataListCurPos + __dataListCustLen);

		dataList.add(_dataList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)dataList.size());
	for(int _i = 0; _i < dataList.size(); _i++) { 
		_buf.putInt(dataList.get(_i).GetBufSize());
	dataList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)15);
	_buf.put((byte)9);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)15);
	_recBuf.put((byte)9);
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

