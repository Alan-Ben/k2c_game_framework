using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

/// <summary>
/// 火星-火星建筑数据初始化
/// </summary>
public class GS2GC_002_073_RetMarsBuildingInit : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 建筑列表
/// </summary>
private List<Common.MarsObj.Mars_Building> buildingList;
/// <summary>
/// 主基地数据
/// </summary>
private Common.MarsObj.Mars_HomeBuilding homeBuilding;
/// <summary>
/// 居民建筑列表
/// </summary>
private List<Common.MarsObj.Mars_PeopleBuilding> peopleBuildingList;
/// <summary>
/// 部件列表
/// </summary>
private List<Common.MarsObj.Mars_BuildingEquipment> equipmentList;
/// <summary>
/// 能量产出数据列表
/// </summary>
private List<Common.MarsObj.Mars_BuildingEnergyOutput> energyOutputList;
/// <summary>
/// food部件列表
/// </summary>
private List<Common.MarsObj.Mars_Mars_BuildingEquipment_Food> foodEquipmentList;
/// <summary>
/// 建筑建造/升级列表
/// </summary>
private List<Common.MarsObj.Mars_BuildingUpQueue> buildingUpQueueList;
/// <summary>
/// 额外心情指数
/// </summary>
private long extMoodIndex;


public GS2GC_002_073_RetMarsBuildingInit() {
	buildingList = new List<Common.MarsObj.Mars_Building>();
	homeBuilding = new Common.MarsObj.Mars_HomeBuilding();
	peopleBuildingList = new List<Common.MarsObj.Mars_PeopleBuilding>();
	equipmentList = new List<Common.MarsObj.Mars_BuildingEquipment>();
	energyOutputList = new List<Common.MarsObj.Mars_BuildingEnergyOutput>();
	foodEquipmentList = new List<Common.MarsObj.Mars_Mars_BuildingEquipment_Food>();
	buildingUpQueueList = new List<Common.MarsObj.Mars_BuildingUpQueue>();
	extMoodIndex = (long)0;
}

public GS2GC_002_073_RetMarsBuildingInit(
	List<Common.MarsObj.Mars_Building> _buildingList
	, Common.MarsObj.Mars_HomeBuilding _homeBuilding
	, List<Common.MarsObj.Mars_PeopleBuilding> _peopleBuildingList
	, List<Common.MarsObj.Mars_BuildingEquipment> _equipmentList
	, List<Common.MarsObj.Mars_BuildingEnergyOutput> _energyOutputList
	, List<Common.MarsObj.Mars_Mars_BuildingEquipment_Food> _foodEquipmentList
	, List<Common.MarsObj.Mars_BuildingUpQueue> _buildingUpQueueList
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

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)73; }

/// <summary>
/// 建筑列表
/// </summary>
public List<Common.MarsObj.Mars_Building> getBuildingList() { return buildingList; }
/// <summary>
/// 建筑列表
/// </summary>
public void addBuildingList(Common.MarsObj.Mars_Building _buildingList) { buildingList.Add(_buildingList); }
/// <summary>
/// 主基地数据
/// </summary>
public Common.MarsObj.Mars_HomeBuilding getHomeBuilding() { return homeBuilding; }
/// <summary>
/// 主基地数据
/// </summary>
public void setHomeBuilding(Common.MarsObj.Mars_HomeBuilding _homeBuilding) { homeBuilding = _homeBuilding; }
/// <summary>
/// 居民建筑列表
/// </summary>
public List<Common.MarsObj.Mars_PeopleBuilding> getPeopleBuildingList() { return peopleBuildingList; }
/// <summary>
/// 居民建筑列表
/// </summary>
public void addPeopleBuildingList(Common.MarsObj.Mars_PeopleBuilding _peopleBuildingList) { peopleBuildingList.Add(_peopleBuildingList); }
/// <summary>
/// 部件列表
/// </summary>
public List<Common.MarsObj.Mars_BuildingEquipment> getEquipmentList() { return equipmentList; }
/// <summary>
/// 部件列表
/// </summary>
public void addEquipmentList(Common.MarsObj.Mars_BuildingEquipment _equipmentList) { equipmentList.Add(_equipmentList); }
/// <summary>
/// 能量产出数据列表
/// </summary>
public List<Common.MarsObj.Mars_BuildingEnergyOutput> getEnergyOutputList() { return energyOutputList; }
/// <summary>
/// 能量产出数据列表
/// </summary>
public void addEnergyOutputList(Common.MarsObj.Mars_BuildingEnergyOutput _energyOutputList) { energyOutputList.Add(_energyOutputList); }
/// <summary>
/// food部件列表
/// </summary>
public List<Common.MarsObj.Mars_Mars_BuildingEquipment_Food> getFoodEquipmentList() { return foodEquipmentList; }
/// <summary>
/// food部件列表
/// </summary>
public void addFoodEquipmentList(Common.MarsObj.Mars_Mars_BuildingEquipment_Food _foodEquipmentList) { foodEquipmentList.Add(_foodEquipmentList); }
/// <summary>
/// 建筑建造/升级列表
/// </summary>
public List<Common.MarsObj.Mars_BuildingUpQueue> getBuildingUpQueueList() { return buildingUpQueueList; }
/// <summary>
/// 建筑建造/升级列表
/// </summary>
public void addBuildingUpQueueList(Common.MarsObj.Mars_BuildingUpQueue _buildingUpQueueList) { buildingUpQueueList.Add(_buildingUpQueueList); }
/// <summary>
/// 额外心情指数
/// </summary>
public long getExtMoodIndex() { return extMoodIndex; }
/// <summary>
/// 额外心情指数
/// </summary>
public void setExtMoodIndex(long _extMoodIndex) { extMoodIndex = _extMoodIndex; }


public int GetBufSize() {
	int _size = 26;
	_size += 2 + (buildingList.Count * 16);
	_size += 2 + (peopleBuildingList.Count * 16);
	_size += 2 + (equipmentList.Count * 24);
	_size += 2 + (energyOutputList.Count * 28);
	_size += 2 + (foodEquipmentList.Count * 13);
	_size += 2 + (buildingUpQueueList.Count * 52);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 28;
	_size += 2 + (buildingList.Count * 16);
	_size += 2 + (peopleBuildingList.Count * 16);
	_size += 2 + (equipmentList.Count * 24);
	_size += 2 + (energyOutputList.Count * 28);
	_size += 2 + (foodEquipmentList.Count * 13);
	_size += 2 + (buildingUpQueueList.Count * 52);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _buildingListCount = _buf.getShort();
	for(int _i = 0; _i < _buildingListCount; _i++) { 
		Common.MarsObj.Mars_Building _buildingList = new Common.MarsObj.Mars_Building();
		int __buildingListCustLen = _buf.getInt();
	int __buildingListCurPos = _buf.getCurPos();
	_buildingList.ReadUnzipBuf(_buf, __buildingListCurPos + __buildingListCustLen);
	_buf.setPosition(__buildingListCurPos + __buildingListCustLen);

		buildingList.Add(_buildingList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _homeBuildingCustLen = _buf.getInt();
	int _homeBuildingCurPos = _buf.getCurPos();
	homeBuilding.ReadUnzipBuf(_buf, _homeBuildingCurPos + _homeBuildingCustLen);
	_buf.setPosition(_homeBuildingCurPos + _homeBuildingCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _peopleBuildingListCount = _buf.getShort();
	for(int _i = 0; _i < _peopleBuildingListCount; _i++) { 
		Common.MarsObj.Mars_PeopleBuilding _peopleBuildingList = new Common.MarsObj.Mars_PeopleBuilding();
		int __peopleBuildingListCustLen = _buf.getInt();
	int __peopleBuildingListCurPos = _buf.getCurPos();
	_peopleBuildingList.ReadUnzipBuf(_buf, __peopleBuildingListCurPos + __peopleBuildingListCustLen);
	_buf.setPosition(__peopleBuildingListCurPos + __peopleBuildingListCustLen);

		peopleBuildingList.Add(_peopleBuildingList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _equipmentListCount = _buf.getShort();
	for(int _i = 0; _i < _equipmentListCount; _i++) { 
		Common.MarsObj.Mars_BuildingEquipment _equipmentList = new Common.MarsObj.Mars_BuildingEquipment();
		int __equipmentListCustLen = _buf.getInt();
	int __equipmentListCurPos = _buf.getCurPos();
	_equipmentList.ReadUnzipBuf(_buf, __equipmentListCurPos + __equipmentListCustLen);
	_buf.setPosition(__equipmentListCurPos + __equipmentListCustLen);

		equipmentList.Add(_equipmentList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _energyOutputListCount = _buf.getShort();
	for(int _i = 0; _i < _energyOutputListCount; _i++) { 
		Common.MarsObj.Mars_BuildingEnergyOutput _energyOutputList = new Common.MarsObj.Mars_BuildingEnergyOutput();
		int __energyOutputListCustLen = _buf.getInt();
	int __energyOutputListCurPos = _buf.getCurPos();
	_energyOutputList.ReadUnzipBuf(_buf, __energyOutputListCurPos + __energyOutputListCustLen);
	_buf.setPosition(__energyOutputListCurPos + __energyOutputListCustLen);

		energyOutputList.Add(_energyOutputList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _foodEquipmentListCount = _buf.getShort();
	for(int _i = 0; _i < _foodEquipmentListCount; _i++) { 
		Common.MarsObj.Mars_Mars_BuildingEquipment_Food _foodEquipmentList = new Common.MarsObj.Mars_Mars_BuildingEquipment_Food();
		int __foodEquipmentListCustLen = _buf.getInt();
	int __foodEquipmentListCurPos = _buf.getCurPos();
	_foodEquipmentList.ReadUnzipBuf(_buf, __foodEquipmentListCurPos + __foodEquipmentListCustLen);
	_buf.setPosition(__foodEquipmentListCurPos + __foodEquipmentListCustLen);

		foodEquipmentList.Add(_foodEquipmentList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _buildingUpQueueListCount = _buf.getShort();
	for(int _i = 0; _i < _buildingUpQueueListCount; _i++) { 
		Common.MarsObj.Mars_BuildingUpQueue _buildingUpQueueList = new Common.MarsObj.Mars_BuildingUpQueue();
		int __buildingUpQueueListCustLen = _buf.getInt();
	int __buildingUpQueueListCurPos = _buf.getCurPos();
	_buildingUpQueueList.ReadUnzipBuf(_buf, __buildingUpQueueListCurPos + __buildingUpQueueListCustLen);
	_buf.setPosition(__buildingUpQueueListCurPos + __buildingUpQueueListCustLen);

		buildingUpQueueList.Add(_buildingUpQueueList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	extMoodIndex = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)buildingList.Count);
	for(int _i = 0; _i < buildingList.Count; _i++) { 
		_buf.putInt(buildingList[_i].GetBufSize());
	buildingList[_i].PutUnzipBuf(_buf);
	}
	_buf.putInt(homeBuilding.GetBufSize());
	homeBuilding.PutUnzipBuf(_buf);
	_buf.putShort((short)peopleBuildingList.Count);
	for(int _i = 0; _i < peopleBuildingList.Count; _i++) { 
		_buf.putInt(peopleBuildingList[_i].GetBufSize());
	peopleBuildingList[_i].PutUnzipBuf(_buf);
	}
	_buf.putShort((short)equipmentList.Count);
	for(int _i = 0; _i < equipmentList.Count; _i++) { 
		_buf.putInt(equipmentList[_i].GetBufSize());
	equipmentList[_i].PutUnzipBuf(_buf);
	}
	_buf.putShort((short)energyOutputList.Count);
	for(int _i = 0; _i < energyOutputList.Count; _i++) { 
		_buf.putInt(energyOutputList[_i].GetBufSize());
	energyOutputList[_i].PutUnzipBuf(_buf);
	}
	_buf.putShort((short)foodEquipmentList.Count);
	for(int _i = 0; _i < foodEquipmentList.Count; _i++) { 
		_buf.putInt(foodEquipmentList[_i].GetBufSize());
	foodEquipmentList[_i].PutUnzipBuf(_buf);
	}
	_buf.putShort((short)buildingUpQueueList.Count);
	for(int _i = 0; _i < buildingUpQueueList.Count; _i++) { 
		_buf.putInt(buildingUpQueueList[_i].GetBufSize());
	buildingUpQueueList[_i].PutUnzipBuf(_buf);
	}
	_buf.putLong(extMoodIndex);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)73);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)73);
	PutUnzipBuf(_recBuf);
}
public byte[] makePackage() {
	int _bufSize = GetBufSize();
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void readPackage(byte[] _buf) {
	ALProtocolBuf _bufObj = new ALProtocolBuf(_buf);
	ReadUnzipBuf(_bufObj, -1);
}
public void readPackage(ALProtocolBuf _buf) {
	ReadUnzipBuf(_buf, -1);
}
public override string ToString() {
	System.Text.StringBuilder builder = new System.Text.StringBuilder();

	builder.Append("{");
	builder.Append("buildingList").Append(":").Append(buildingList.ToString()).Append(", ");
	builder.Append("homeBuilding").Append(":").Append(homeBuilding == null ? "null" : homeBuilding.ToString()).Append(", ");
	builder.Append("peopleBuildingList").Append(":").Append(peopleBuildingList.ToString()).Append(", ");
	builder.Append("equipmentList").Append(":").Append(equipmentList.ToString()).Append(", ");
	builder.Append("energyOutputList").Append(":").Append(energyOutputList.ToString()).Append(", ");
	builder.Append("foodEquipmentList").Append(":").Append(foodEquipmentList.ToString()).Append(", ");
	builder.Append("buildingUpQueueList").Append(":").Append(buildingUpQueueList.ToString()).Append(", ");
	builder.Append("extMoodIndex").Append(":").Append(extMoodIndex.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

