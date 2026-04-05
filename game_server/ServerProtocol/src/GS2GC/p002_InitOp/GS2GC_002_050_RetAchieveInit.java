package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
public class GS2GC_002_050_RetAchieveInit implements ALBasicProtocolPack._IALProtocolStructure {
/** 成就点列表 */
private java.util.ArrayList<Common.AchieveObj.Achieve_AchievePointInfo> achievePointList;
/** 成就列表 */
private java.util.ArrayList<Common.AchieveObj.Achieve_Info> achieveList;


public GS2GC_002_050_RetAchieveInit() {
	achievePointList = new java.util.ArrayList<Common.AchieveObj.Achieve_AchievePointInfo>();
	achieveList = new java.util.ArrayList<Common.AchieveObj.Achieve_Info>();
}

public GS2GC_002_050_RetAchieveInit(
	 java.util.ArrayList<Common.AchieveObj.Achieve_AchievePointInfo> _achievePointList
	, java.util.ArrayList<Common.AchieveObj.Achieve_Info> _achieveList
) {	achievePointList = _achievePointList;
	achieveList = _achieveList;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)50; }

/** 成就点列表 */
public java.util.ArrayList<Common.AchieveObj.Achieve_AchievePointInfo> getAchievePointList() { return achievePointList; }
/** 成就点列表 */
public void addAchievePointList(Common.AchieveObj.Achieve_AchievePointInfo _achievePointList) { achievePointList.add(_achievePointList); }
/** 成就列表 */
public java.util.ArrayList<Common.AchieveObj.Achieve_Info> getAchieveList() { return achieveList; }
/** 成就列表 */
public void addAchieveList(Common.AchieveObj.Achieve_Info _achieveList) { achieveList.add(_achieveList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (achievePointList.size() * 20);
	_size += 2;
	for(int _i = 0; _i < achieveList.size(); _i++) {
	_size += 4 + achieveList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (achievePointList.size() * 20);
	_size += 2;
	for(int _i = 0; _i < achieveList.size(); _i++) {
	_size += 4 + achieveList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _achievePointListCount = _buf.getShort();
	for(int _i = 0; _i < _achievePointListCount; _i++) { 
		Common.AchieveObj.Achieve_AchievePointInfo _achievePointList = new Common.AchieveObj.Achieve_AchievePointInfo();
		if(_buf.remaining() <= 0) return;
	int __achievePointListCustLen = _buf.getInt();
	int __achievePointListCurPos = _buf.position();
	_achievePointList.ReadUnzipBuf(_buf, __achievePointListCurPos + __achievePointListCustLen);
	_buf.position(__achievePointListCurPos + __achievePointListCustLen);

		achievePointList.add(_achievePointList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _achieveListCount = _buf.getShort();
	for(int _i = 0; _i < _achieveListCount; _i++) { 
		Common.AchieveObj.Achieve_Info _achieveList = new Common.AchieveObj.Achieve_Info();
		if(_buf.remaining() <= 0) return;
	int __achieveListCustLen = _buf.getInt();
	int __achieveListCurPos = _buf.position();
	_achieveList.ReadUnzipBuf(_buf, __achieveListCurPos + __achieveListCustLen);
	_buf.position(__achieveListCurPos + __achieveListCustLen);

		achieveList.add(_achieveList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)achievePointList.size());
	for(int _i = 0; _i < achievePointList.size(); _i++) { 
		_buf.putInt(achievePointList.get(_i).GetBufSize());
	achievePointList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putShort((short)achieveList.size());
	for(int _i = 0; _i < achieveList.size(); _i++) { 
		_buf.putInt(achieveList.get(_i).GetBufSize());
	achieveList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)50);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)50);
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

