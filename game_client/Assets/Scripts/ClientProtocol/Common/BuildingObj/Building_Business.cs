using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.BuildingObj
{

/// <summary>
/// 经营建筑数据
/// </summary>
public class Building_Business : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 建筑ID
/// </summary>
private long buildingId;
/// <summary>
/// 等级
/// </summary>
private int lvl;
/// <summary>
/// 雇员数量
/// </summary>
private int employeeCount;
/// <summary>
/// 产品列表
/// </summary>
private List<long> productList;


public Building_Business() {
	buildingId = (long)0;
	lvl = 0;
	employeeCount = 0;
	productList = new List<long>();
}

public Building_Business(
	long _buildingId
	, int _lvl
	, int _employeeCount
	, List<long> _productList
) {	buildingId = _buildingId;
	lvl = _lvl;
	employeeCount = _employeeCount;
	productList = _productList;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 建筑ID
/// </summary>
public long getBuildingId() { return buildingId; }
/// <summary>
/// 建筑ID
/// </summary>
public void setBuildingId(long _buildingId) { buildingId = _buildingId; }
/// <summary>
/// 等级
/// </summary>
public int getLvl() { return lvl; }
/// <summary>
/// 等级
/// </summary>
public void setLvl(int _lvl) { lvl = _lvl; }
/// <summary>
/// 雇员数量
/// </summary>
public int getEmployeeCount() { return employeeCount; }
/// <summary>
/// 雇员数量
/// </summary>
public void setEmployeeCount(int _employeeCount) { employeeCount = _employeeCount; }
/// <summary>
/// 产品列表
/// </summary>
public List<long> getProductList() { return productList; }
/// <summary>
/// 产品列表
/// </summary>
public void addProductList(long _productList) { productList.Add(_productList); }


public int GetBufSize() {
	int _size = 16;
	_size += 2 + (productList.Count * 8);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 18;
	_size += 2 + (productList.Count * 8);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	buildingId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	lvl = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	employeeCount = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _productListCount = _buf.getShort();
	for(int _i = 0; _i < _productListCount; _i++) { 
		long _productList = (long)0;
		_productList = _buf.getLong();
		productList.Add(_productList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(buildingId);
	_buf.putInt(lvl);
	_buf.putInt(employeeCount);
	_buf.putShort((short)productList.Count);
	for(int _i = 0; _i < productList.Count; _i++) { 
		_buf.putLong(productList[_i]);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
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
	builder.Append("buildingId").Append(":").Append(buildingId.ToString()).Append(", ");
	builder.Append("lvl").Append(":").Append(lvl.ToString()).Append(", ");
	builder.Append("employeeCount").Append(":").Append(employeeCount.ToString()).Append(", ");
	builder.Append("productList").Append(":").Append(productList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

