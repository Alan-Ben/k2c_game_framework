using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

/// <summary>
/// 建筑初始化
/// </summary>
public class GS2GC_002_010_RetBuildingInit : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 已建造的建筑ID列表
/// </summary>
private List<long> buildingIdList;
/// <summary>
/// 已建造的农田建筑列表
/// </summary>
private List<Common.BuildingObj.Building_Farm> farmList;
/// <summary>
/// 已建造的经营建筑列表
/// </summary>
private List<Common.BuildingObj.Building_Business> businessList;
/// <summary>
/// 农田建筑暴击信息
/// </summary>
private Common.BuildingObj.Building_FarmMultipleInfo farmMultipleInfo;


public GS2GC_002_010_RetBuildingInit() {
	buildingIdList = new List<long>();
	farmList = new List<Common.BuildingObj.Building_Farm>();
	businessList = new List<Common.BuildingObj.Building_Business>();
	farmMultipleInfo = new Common.BuildingObj.Building_FarmMultipleInfo();
}

public GS2GC_002_010_RetBuildingInit(
	List<long> _buildingIdList
	, List<Common.BuildingObj.Building_Farm> _farmList
	, List<Common.BuildingObj.Building_Business> _businessList
	, Common.BuildingObj.Building_FarmMultipleInfo _farmMultipleInfo
) {	buildingIdList = _buildingIdList;
	farmList = _farmList;
	businessList = _businessList;
	farmMultipleInfo = _farmMultipleInfo;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)10; }

/// <summary>
/// 已建造的建筑ID列表
/// </summary>
public List<long> getBuildingIdList() { return buildingIdList; }
/// <summary>
/// 已建造的建筑ID列表
/// </summary>
public void addBuildingIdList(long _buildingIdList) { buildingIdList.Add(_buildingIdList); }
/// <summary>
/// 已建造的农田建筑列表
/// </summary>
public List<Common.BuildingObj.Building_Farm> getFarmList() { return farmList; }
/// <summary>
/// 已建造的农田建筑列表
/// </summary>
public void addFarmList(Common.BuildingObj.Building_Farm _farmList) { farmList.Add(_farmList); }
/// <summary>
/// 已建造的经营建筑列表
/// </summary>
public List<Common.BuildingObj.Building_Business> getBusinessList() { return businessList; }
/// <summary>
/// 已建造的经营建筑列表
/// </summary>
public void addBusinessList(Common.BuildingObj.Building_Business _businessList) { businessList.Add(_businessList); }
/// <summary>
/// 农田建筑暴击信息
/// </summary>
public Common.BuildingObj.Building_FarmMultipleInfo getFarmMultipleInfo() { return farmMultipleInfo; }
/// <summary>
/// 农田建筑暴击信息
/// </summary>
public void setFarmMultipleInfo(Common.BuildingObj.Building_FarmMultipleInfo _farmMultipleInfo) { farmMultipleInfo = _farmMultipleInfo; }


public int GetBufSize() {
	int _size = 32;
	_size += 2 + (buildingIdList.Count * 8);
	_size += 2 + (farmList.Count * 24);
	_size += 2;
for(int _i = 0; _i < businessList.Count; _i++) {
	_size += 4 + businessList[_i].GetBufSize();
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 34;
	_size += 2 + (buildingIdList.Count * 8);
	_size += 2 + (farmList.Count * 24);
	_size += 2;
for(int _i = 0; _i < businessList.Count; _i++) {
	_size += 4 + businessList[_i].GetBufSize();
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _buildingIdListCount = _buf.getShort();
	for(int _i = 0; _i < _buildingIdListCount; _i++) { 
		long _buildingIdList = (long)0;
		_buildingIdList = _buf.getLong();
		buildingIdList.Add(_buildingIdList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _farmListCount = _buf.getShort();
	for(int _i = 0; _i < _farmListCount; _i++) { 
		Common.BuildingObj.Building_Farm _farmList = new Common.BuildingObj.Building_Farm();
		int __farmListCustLen = _buf.getInt();
	int __farmListCurPos = _buf.getCurPos();
	_farmList.ReadUnzipBuf(_buf, __farmListCurPos + __farmListCustLen);
	_buf.setPosition(__farmListCurPos + __farmListCustLen);

		farmList.Add(_farmList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _businessListCount = _buf.getShort();
	for(int _i = 0; _i < _businessListCount; _i++) { 
		Common.BuildingObj.Building_Business _businessList = new Common.BuildingObj.Building_Business();
		int __businessListCustLen = _buf.getInt();
	int __businessListCurPos = _buf.getCurPos();
	_businessList.ReadUnzipBuf(_buf, __businessListCurPos + __businessListCustLen);
	_buf.setPosition(__businessListCurPos + __businessListCustLen);

		businessList.Add(_businessList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _farmMultipleInfoCustLen = _buf.getInt();
	int _farmMultipleInfoCurPos = _buf.getCurPos();
	farmMultipleInfo.ReadUnzipBuf(_buf, _farmMultipleInfoCurPos + _farmMultipleInfoCustLen);
	_buf.setPosition(_farmMultipleInfoCurPos + _farmMultipleInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)buildingIdList.Count);
	for(int _i = 0; _i < buildingIdList.Count; _i++) { 
		_buf.putLong(buildingIdList[_i]);
	}
	_buf.putShort((short)farmList.Count);
	for(int _i = 0; _i < farmList.Count; _i++) { 
		_buf.putInt(farmList[_i].GetBufSize());
	farmList[_i].PutUnzipBuf(_buf);
	}
	_buf.putShort((short)businessList.Count);
	for(int _i = 0; _i < businessList.Count; _i++) { 
		_buf.putInt(businessList[_i].GetBufSize());
	businessList[_i].PutUnzipBuf(_buf);
	}
	_buf.putInt(farmMultipleInfo.GetBufSize());
	farmMultipleInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)10);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)10);
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
	builder.Append("buildingIdList").Append(":").Append(buildingIdList.ToString()).Append(", ");
	builder.Append("farmList").Append(":").Append(farmList.ToString()).Append(", ");
	builder.Append("businessList").Append(":").Append(businessList.ToString()).Append(", ");
	builder.Append("farmMultipleInfo").Append(":").Append(farmMultipleInfo == null ? "null" : farmMultipleInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

