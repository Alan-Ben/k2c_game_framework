package GS2GC.p010_BuildingOp;

import java.nio.ByteBuffer;
/*********
 * 通过效果获得额外员工推送
 **/
public class GS2GC_010_056_OnEffectGainBusinessWorkes implements ALBasicProtocolPack._IALProtocolStructure {
/** 效果获得员工数据列表 */
private java.util.ArrayList<Common.BuildingObj.Building_EffectGainWorkers> list;


public GS2GC_010_056_OnEffectGainBusinessWorkes() {
	list = new java.util.ArrayList<Common.BuildingObj.Building_EffectGainWorkers>();
}

public GS2GC_010_056_OnEffectGainBusinessWorkes(
	 java.util.ArrayList<Common.BuildingObj.Building_EffectGainWorkers> _list
) {	list = _list;
}

public final byte getMainOrder() { return (byte)10; }

public final byte getSubOrder() { return (byte)56; }

/** 效果获得员工数据列表 */
public java.util.ArrayList<Common.BuildingObj.Building_EffectGainWorkers> getList() { return list; }
/** 效果获得员工数据列表 */
public void addList(Common.BuildingObj.Building_EffectGainWorkers _list) { list.add(_list); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (list.size() * 16);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (list.size() * 16);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _listCount = _buf.getShort();
	for(int _i = 0; _i < _listCount; _i++) { 
		Common.BuildingObj.Building_EffectGainWorkers _list = new Common.BuildingObj.Building_EffectGainWorkers();
		if(_buf.remaining() <= 0) return;
	int __listCustLen = _buf.getInt();
	int __listCurPos = _buf.position();
	_list.ReadUnzipBuf(_buf, __listCurPos + __listCustLen);
	_buf.position(__listCurPos + __listCustLen);

		list.add(_list);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)list.size());
	for(int _i = 0; _i < list.size(); _i++) { 
		_buf.putInt(list.get(_i).GetBufSize());
	list.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)10);
	_buf.put((byte)56);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)10);
	_recBuf.put((byte)56);
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

