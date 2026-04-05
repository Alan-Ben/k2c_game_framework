package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
/*********
 * 火星-火星建筑数据初始化
 **/
public class GS2GC_002_073_RetMarsBuildingInit implements ALBasicProtocolPack._IALProtocolStructure {
/** 建筑列表 */
private java.util.ArrayList<Common.MarsObj.Mars_Building> buildingList;
/** 主基地数据 */
private Common.MarsObj.Mars_HomeBuilding homeBuilding;
/** 居民建筑列表 */
private java.util.ArrayList<Common.MarsObj.Mars_PeopleBuilding> peopleBuildingList;
/** 部件列表 */
private java.util.ArrayList<Common.MarsObj.Mars_BuildingEquipment> equipmentList;
/** 能量产出数据列表 */
private java.util.ArrayList<Common.MarsObj.Mars_BuildingEnergyOutput> energyOutputList;
/** food部件列表 */
private java.util.ArrayList<Common.MarsObj.Mars_Mars_BuildingEquipment_Food> foodEquipmentList;
/** 建筑建造/升级列表 */
private java.util.ArrayList<Common.MarsObj.Mars_BuildingUpQueue> buildingUpQueueList;
/** 额外心情指数 */
private long extMoodIndex;


public GS2GC_002_073_RetMarsBuildingInit() {
	buildingList = new java.util.ArrayList<Common.MarsObj.Mars_Building>();
	homeBuilding = new Common.MarsObj.Mars_HomeBuilding();
	peopleBuildingList = new java.util.ArrayList<Common.MarsObj.Mars_PeopleBuilding>();
	equipmentList = new java.util.ArrayList<Common.MarsObj.Mars_BuildingEquipment>();
	energyOutputList = new java.util.ArrayList<Common.MarsObj.Mars_BuildingEnergyOutput>();
	foodEquipmentList = new java.util.ArrayList<Common.MarsObj.Mars_Mars_BuildingEquipment_Food>();
	buildingUpQueueList = new java.util.ArrayList<Common.MarsObj.Mars_BuildingUpQueue>();
	extMoodIndex = (long)0;
}

public GS2GC_002_073_RetMarsBuildingInit(
	 java.util.ArrayList<Common.MarsObj.Mars_Building> _buildingList
	, Common.MarsObj.Mars_HomeBuilding _homeBuilding
	, java.util.ArrayList<Common.MarsObj.Mars_PeopleBuilding> _peopleBuildingList
	, java.util.ArrayList<Common.MarsObj.Mars_BuildingEquipment> _equipmentList
	, java.util.ArrayList<Common.MarsObj.Mars_BuildingEnergyOutput> _energyOutputList
	, java.util.ArrayList<Common.MarsObj.Mars_Mars_BuildingEquipment_Food> _foodEquipmentList
	, java.util.ArrayList<Common.MarsObj.Mars_BuildingUpQueue> _buildingUpQueueList
	, long _extMoodIndex
) {	buildingList = _buildingList;
	homeBuilding = _homeBuilding;
	peopleBuildingList = _peopleBuildingList;
	equipmentList = _equipmentList;
	energyOutputList = _energyOutputList;
	foodEquipmentList = _foodEquipmentList;
	buildingUpQueueList = _buildingUpQueueList;
	extMoodIndex = _extMoodIndex;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)73; }

/** 建筑列表 */
public java.util.ArrayList<Common.MarsObj.Mars_Building> getBuildingList() { return buildingList; }
/** 建筑列表 */
public void addBuildingList(Common.MarsObj.Mars_Building _buildingList) { buildingList.add(_buildingList); }
/** 主基地数据 */
public Common.MarsObj.Mars_HomeBuilding getHomeBuilding() { return homeBuilding; }
/** 主基地数据 */
public void setHomeBuilding(Common.MarsObj.Mars_HomeBuilding _homeBuilding) { homeBuilding = _homeBuilding; }
/** 居民建筑列表 */
public java.util.ArrayList<Common.MarsObj.Mars_PeopleBuilding> getPeopleBuildingList() { return peopleBuildingList; }
/** 居民建筑列表 */
public void addPeopleBuildingList(Common.MarsObj.Mars_PeopleBuilding _peopleBuildingList) { peopleBuildingList.add(_peopleBuildingList); }
/** 部件列表 */
public java.util.ArrayList<Common.MarsObj.Mars_BuildingEquipment> getEquipmentList() { return equipmentList; }
/** 部件列表 */
public void addEquipmentList(Common.MarsObj.Mars_BuildingEquipment _equipmentList) { equipmentList.add(_equipmentList); }
/** 能量产出数据列表 */
public java.util.ArrayList<Common.MarsObj.Mars_BuildingEnergyOutput> getEnergyOutputList() { return energyOutputList; }
/** 能量产出数据列表 */
public void addEnergyOutputList(Common.MarsObj.Mars_BuildingEnergyOutput _energyOutputList) { energyOutputList.add(_energyOutputList); }
/** food部件列表 */
public java.util.ArrayList<Common.MarsObj.Mars_Mars_BuildingEquipment_Food> getFoodEquipmentList() { return foodEquipmentList; }
/** food部件列表 */
public void addFoodEquipmentList(Common.MarsObj.Mars_Mars_BuildingEquipment_Food _foodEquipmentList) { foodEquipmentList.add(_foodEquipmentList); }
/** 建筑建造/升级列表 */
public java.util.ArrayList<Common.MarsObj.Mars_BuildingUpQueue> getBuildingUpQueueList() { return buildingUpQueueList; }
/** 建筑建造/升级列表 */
public void addBuildingUpQueueList(Common.MarsObj.Mars_BuildingUpQueue _buildingUpQueueList) { buildingUpQueueList.add(_buildingUpQueueList); }
/** 额外心情指数 */
public long getExtMoodIndex() { return extMoodIndex; }
/** 额外心情指数 */
public void setExtMoodIndex(long _extMoodIndex) { extMoodIndex = _extMoodIndex; }


public final int GetBufSize() {
	int _size = 26;
	_size += 2 + (buildingList.size() * 16);
	_size += 2 + (peopleBuildingList.size() * 16);
	_size += 2 + (equipmentList.size() * 24);
	_size += 2 + (energyOutputList.size() * 28);
	_size += 2 + (foodEquipmentList.size() * 13);
	_size += 2 + (buildingUpQueueList.size() * 52);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 28;
	_size += 2 + (buildingList.size() * 16);
	_size += 2 + (peopleBuildingList.size() * 16);
	_size += 2 + (equipmentList.size() * 24);
	_size += 2 + (energyOutputList.size() * 28);
	_size += 2 + (foodEquipmentList.size() * 13);
	_size += 2 + (buildingUpQueueList.size() * 52);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _buildingListCount = _buf.getShort();
	for(int _i = 0; _i < _buildingListCount; _i++) { 
		Common.MarsObj.Mars_Building _buildingList = new Common.MarsObj.Mars_Building();
		if(_buf.remaining() <= 0) return;
	int __buildingListCustLen = _buf.getInt();
	int __buildingListCurPos = _buf.position();
	_buildingList.ReadUnzipBuf(_buf, __buildingListCurPos + __buildingListCustLen);
	_buf.position(__buildingListCurPos + __buildingListCustLen);

		buildingList.add(_buildingList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _homeBuildingCustLen = _buf.getInt();
	int _homeBuildingCurPos = _buf.position();
	homeBuilding.ReadUnzipBuf(_buf, _homeBuildingCurPos + _homeBuildingCustLen);
	_buf.position(_homeBuildingCurPos + _homeBuildingCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _peopleBuildingListCount = _buf.getShort();
	for(int _i = 0; _i < _peopleBuildingListCount; _i++) { 
		Common.MarsObj.Mars_PeopleBuilding _peopleBuildingList = new Common.MarsObj.Mars_PeopleBuilding();
		if(_buf.remaining() <= 0) return;
	int __peopleBuildingListCustLen = _buf.getInt();
	int __peopleBuildingListCurPos = _buf.position();
	_peopleBuildingList.ReadUnzipBuf(_buf, __peopleBuildingListCurPos + __peopleBuildingListCustLen);
	_buf.position(__peopleBuildingListCurPos + __peopleBuildingListCustLen);

		peopleBuildingList.add(_peopleBuildingList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _equipmentListCount = _buf.getShort();
	for(int _i = 0; _i < _equipmentListCount; _i++) { 
		Common.MarsObj.Mars_BuildingEquipment _equipmentList = new Common.MarsObj.Mars_BuildingEquipment();
		if(_buf.remaining() <= 0) return;
	int __equipmentListCustLen = _buf.getInt();
	int __equipmentListCurPos = _buf.position();
	_equipmentList.ReadUnzipBuf(_buf, __equipmentListCurPos + __equipmentListCustLen);
	_buf.position(__equipmentListCurPos + __equipmentListCustLen);

		equipmentList.add(_equipmentList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _energyOutputListCount = _buf.getShort();
	for(int _i = 0; _i < _energyOutputListCount; _i++) { 
		Common.MarsObj.Mars_BuildingEnergyOutput _energyOutputList = new Common.MarsObj.Mars_BuildingEnergyOutput();
		if(_buf.remaining() <= 0) return;
	int __energyOutputListCustLen = _buf.getInt();
	int __energyOutputListCurPos = _buf.position();
	_energyOutputList.ReadUnzipBuf(_buf, __energyOutputListCurPos + __energyOutputListCustLen);
	_buf.position(__energyOutputListCurPos + __energyOutputListCustLen);

		energyOutputList.add(_energyOutputList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _foodEquipmentListCount = _buf.getShort();
	for(int _i = 0; _i < _foodEquipmentListCount; _i++) { 
		Common.MarsObj.Mars_Mars_BuildingEquipment_Food _foodEquipmentList = new Common.MarsObj.Mars_Mars_BuildingEquipment_Food();
		if(_buf.remaining() <= 0) return;
	int __foodEquipmentListCustLen = _buf.getInt();
	int __foodEquipmentListCurPos = _buf.position();
	_foodEquipmentList.ReadUnzipBuf(_buf, __foodEquipmentListCurPos + __foodEquipmentListCustLen);
	_buf.position(__foodEquipmentListCurPos + __foodEquipmentListCustLen);

		foodEquipmentList.add(_foodEquipmentList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _buildingUpQueueListCount = _buf.getShort();
	for(int _i = 0; _i < _buildingUpQueueListCount; _i++) { 
		Common.MarsObj.Mars_BuildingUpQueue _buildingUpQueueList = new Common.MarsObj.Mars_BuildingUpQueue();
		if(_buf.remaining() <= 0) return;
	int __buildingUpQueueListCustLen = _buf.getInt();
	int __buildingUpQueueListCurPos = _buf.position();
	_buildingUpQueueList.ReadUnzipBuf(_buf, __buildingUpQueueListCurPos + __buildingUpQueueListCustLen);
	_buf.position(__buildingUpQueueListCurPos + __buildingUpQueueListCustLen);

		buildingUpQueueList.add(_buildingUpQueueList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) extMoodIndex = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)buildingList.size());
	for(int _i = 0; _i < buildingList.size(); _i++) { 
		_buf.putInt(buildingList.get(_i).GetBufSize());
	buildingList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putInt(homeBuilding.GetBufSize());
	homeBuilding.PutUnzipBuf(_buf);
	_buf.putShort((short)peopleBuildingList.size());
	for(int _i = 0; _i < peopleBuildingList.size(); _i++) { 
		_buf.putInt(peopleBuildingList.get(_i).GetBufSize());
	peopleBuildingList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putShort((short)equipmentList.size());
	for(int _i = 0; _i < equipmentList.size(); _i++) { 
		_buf.putInt(equipmentList.get(_i).GetBufSize());
	equipmentList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putShort((short)energyOutputList.size());
	for(int _i = 0; _i < energyOutputList.size(); _i++) { 
		_buf.putInt(energyOutputList.get(_i).GetBufSize());
	energyOutputList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putShort((short)foodEquipmentList.size());
	for(int _i = 0; _i < foodEquipmentList.size(); _i++) { 
		_buf.putInt(foodEquipmentList.get(_i).GetBufSize());
	foodEquipmentList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putShort((short)buildingUpQueueList.size());
	for(int _i = 0; _i < buildingUpQueueList.size(); _i++) { 
		_buf.putInt(buildingUpQueueList.get(_i).GetBufSize());
	buildingUpQueueList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putLong(extMoodIndex);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)73);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)73);
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

