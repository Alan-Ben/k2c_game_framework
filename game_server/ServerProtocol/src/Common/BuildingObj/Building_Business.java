package Common.BuildingObj;

import java.nio.ByteBuffer;
/*********
 * 经营建筑数据
 **/
public class Building_Business implements ALBasicProtocolPack._IALProtocolStructure {
/** 建筑ID */
private long buildingId;
/** 等级 */
private int lvl;
/** 雇员数量 */
private int employeeCount;
/** 产品列表 */
private java.util.ArrayList<Long> productList;


public Building_Business() {
	buildingId = (long)0;
	lvl = 0;
	employeeCount = 0;
	productList = new java.util.ArrayList<Long>();
}

public Building_Business(
	 long _buildingId
	, int _lvl
	, int _employeeCount
	, java.util.ArrayList<Long> _productList
) {	buildingId = _buildingId;
	lvl = _lvl;
	employeeCount = _employeeCount;
	productList = _productList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 建筑ID */
public long getBuildingId() { return buildingId; }
/** 建筑ID */
public void setBuildingId(long _buildingId) { buildingId = _buildingId; }
/** 等级 */
public int getLvl() { return lvl; }
/** 等级 */
public void setLvl(int _lvl) { lvl = _lvl; }
/** 雇员数量 */
public int getEmployeeCount() { return employeeCount; }
/** 雇员数量 */
public void setEmployeeCount(int _employeeCount) { employeeCount = _employeeCount; }
/** 产品列表 */
public java.util.ArrayList<Long> getProductList() { return productList; }
/** 产品列表 */
public void addProductList(long _productList) { productList.add(_productList); }


public final int GetBufSize() {
	int _size = 16;
	_size += 2 + (productList.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;
	_size += 2 + (productList.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) buildingId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) lvl = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) employeeCount = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _productListCount = _buf.getShort();
	for(int _i = 0; _i < _productListCount; _i++) { 
		long _productList = (long)0;
		if(_buf.remaining() > 0) _productList = _buf.getLong();
		productList.add(_productList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(buildingId);
	_buf.putInt(lvl);
	_buf.putInt(employeeCount);
	_buf.putShort((short)productList.size());
	for(int _i = 0; _i < productList.size(); _i++) { 
		_buf.putLong(productList.get(_i));
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

