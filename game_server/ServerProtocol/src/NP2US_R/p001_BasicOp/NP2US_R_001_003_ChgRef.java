package NP2US_R.p001_BasicOp;

import java.nio.ByteBuffer;
public class NP2US_R_001_003_ChgRef implements ALBasicProtocolPack._IALProtocolStructure {
private String tableName;
private String id;
private String key;
private String value;


public NP2US_R_001_003_ChgRef() {
	tableName = "";
	id = "";
	key = "";
	value = "";
}

public NP2US_R_001_003_ChgRef(
	 String _tableName
	, String _id
	, String _key
	, String _value
) {	tableName = _tableName;
	id = _id;
	key = _key;
	value = _value;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)3; }

public String getTableName() { return tableName; }
public void setTableName(String _tableName) { tableName = _tableName; }
public String getId() { return id; }
public void setId(String _id) { id = _id; }
public String getKey() { return key; }
public void setKey(String _key) { key = _key; }
public String getValue() { return value; }
public void setValue(String _value) { value = _value; }


public final int GetBufSize() {
	int _size = 0;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(tableName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(id);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(key);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(value);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(tableName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(id);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(key);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(value);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) tableName = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) id = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) key = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) value = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, tableName);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, id);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, key);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, value);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)3);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)3);
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

