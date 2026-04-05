package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
/*********
 * 建筑初始化
 **/
public class GS2GC_002_010_RetBuildingInit implements ALBasicProtocolPack._IALProtocolStructure {
/** 已建造的建筑ID列表 */
private java.util.ArrayList<Long> buildingIdList;
/** 已建造的农田建筑列表 */
private java.util.ArrayList<Common.BuildingObj.Building_Farm> farmList;
/** 已建造的经营建筑列表 */
private java.util.ArrayList<Common.BuildingObj.Building_Business> businessList;
/** 农田建筑暴击信息 */
private Common.BuildingObj.Building_FarmMultipleInfo farmMultipleInfo;


public GS2GC_002_010_RetBuildingInit() {
	buildingIdList = new java.util.ArrayList<Long>();
	farmList = new java.util.ArrayList<Common.BuildingObj.Building_Farm>();
	businessList = new java.util.ArrayList<Common.BuildingObj.Building_Business>();
	farmMultipleInfo = new Common.BuildingObj.Building_FarmMultipleInfo();
}

public GS2GC_002_010_RetBuildingInit(
	 java.util.ArrayList<Long> _buildingIdList
	, java.util.ArrayList<Common.BuildingObj.Building_Farm> _farmList
	, java.util.ArrayList<Common.BuildingObj.Building_Business> _businessList
	, Common.BuildingObj.Building_FarmMultipleInfo _farmMultipleInfo
) {	buildingIdList = _buildingIdList;
	farmList = _farmList;
	businessList = _businessList;
	farmMultipleInfo = _farmMultipleInfo;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)10; }

/** 已建造的建筑ID列表 */
public java.util.ArrayList<Long> getBuildingIdList() { return buildingIdList; }
/** 已建造的建筑ID列表 */
public void addBuildingIdList(long _buildingIdList) { buildingIdList.add(_buildingIdList); }
/** 已建造的农田建筑列表 */
public java.util.ArrayList<Common.BuildingObj.Building_Farm> getFarmList() { return farmList; }
/** 已建造的农田建筑列表 */
public void addFarmList(Common.BuildingObj.Building_Farm _farmList) { farmList.add(_farmList); }
/** 已建造的经营建筑列表 */
public java.util.ArrayList<Common.BuildingObj.Building_Business> getBusinessList() { return businessList; }
/** 已建造的经营建筑列表 */
public void addBusinessList(Common.BuildingObj.Building_Business _businessList) { businessList.add(_businessList); }
/** 农田建筑暴击信息 */
public Common.BuildingObj.Building_FarmMultipleInfo getFarmMultipleInfo() { return farmMultipleInfo; }
/** 农田建筑暴击信息 */
public void setFarmMultipleInfo(Common.BuildingObj.Building_FarmMultipleInfo _farmMultipleInfo) { farmMultipleInfo = _farmMultipleInfo; }


public final int GetBufSize() {
	int _size = 32;
	_size += 2 + (buildingIdList.size() * 8);
	_size += 2 + (farmList.size() * 24);
	_size += 2;
	for(int _i = 0; _i < businessList.size(); _i++) {
	_size += 4 + businessList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 34;
	_size += 2 + (buildingIdList.size() * 8);
	_size += 2 + (farmList.size() * 24);
	_size += 2;
	for(int _i = 0; _i < businessList.size(); _i++) {
	_size += 4 + businessList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _buildingIdListCount = _buf.getShort();
	for(int _i = 0; _i < _buildingIdListCount; _i++) { 
		long _buildingIdList = (long)0;
		if(_buf.remaining() > 0) _buildingIdList = _buf.getLong();
		buildingIdList.add(_buildingIdList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _farmListCount = _buf.getShort();
	for(int _i = 0; _i < _farmListCount; _i++) { 
		Common.BuildingObj.Building_Farm _farmList = new Common.BuildingObj.Building_Farm();
		if(_buf.remaining() <= 0) return;
	int __farmListCustLen = _buf.getInt();
	int __farmListCurPos = _buf.position();
	_farmList.ReadUnzipBuf(_buf, __farmListCurPos + __farmListCustLen);
	_buf.position(__farmListCurPos + __farmListCustLen);

		farmList.add(_farmList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _businessListCount = _buf.getShort();
	for(int _i = 0; _i < _businessListCount; _i++) { 
		Common.BuildingObj.Building_Business _businessList = new Common.BuildingObj.Building_Business();
		if(_buf.remaining() <= 0) return;
	int __businessListCustLen = _buf.getInt();
	int __businessListCurPos = _buf.position();
	_businessList.ReadUnzipBuf(_buf, __businessListCurPos + __businessListCustLen);
	_buf.position(__businessListCurPos + __businessListCustLen);

		businessList.add(_businessList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _farmMultipleInfoCustLen = _buf.getInt();
	int _farmMultipleInfoCurPos = _buf.position();
	farmMultipleInfo.ReadUnzipBuf(_buf, _farmMultipleInfoCurPos + _farmMultipleInfoCustLen);
	_buf.position(_farmMultipleInfoCurPos + _farmMultipleInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)buildingIdList.size());
	for(int _i = 0; _i < buildingIdList.size(); _i++) { 
		_buf.putLong(buildingIdList.get(_i));
	}
	_buf.putShort((short)farmList.size());
	for(int _i = 0; _i < farmList.size(); _i++) { 
		_buf.putInt(farmList.get(_i).GetBufSize());
	farmList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putShort((short)businessList.size());
	for(int _i = 0; _i < businessList.size(); _i++) { 
		_buf.putInt(businessList.get(_i).GetBufSize());
	businessList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putInt(farmMultipleInfo.GetBufSize());
	farmMultipleInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)10);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)10);
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

