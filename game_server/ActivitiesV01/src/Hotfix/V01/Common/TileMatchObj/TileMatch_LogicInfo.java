package Hotfix.V01.Common.TileMatchObj;

import java.nio.ByteBuffer;
/*********
 * 三消-逻辑信息
 **/
public class TileMatch_LogicInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 逻辑序列号 */
private int serialId;
/** 逻辑类型 */
private Hotfix.V01.Enum.TileMatchEnum.ETileMatch_LogicType logicType;
/** 逻辑信息 */
private byte[] data;
/** 分数 */
private int score;


public TileMatch_LogicInfo() {
	serialId = 0;
	logicType = Hotfix.V01.Enum.TileMatchEnum.ETileMatch_LogicType.values()[0];
	data = null;
	score = 0;
}

public TileMatch_LogicInfo(
	 int _serialId
	, Hotfix.V01.Enum.TileMatchEnum.ETileMatch_LogicType _logicType
	, byte[] _data
	, int _score
) {	serialId = _serialId;
	logicType = _logicType;
	data = _data;
	score = _score;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 逻辑序列号 */
public int getSerialId() { return serialId; }
/** 逻辑序列号 */
public void setSerialId(int _serialId) { serialId = _serialId; }
/** 逻辑类型 */
public Hotfix.V01.Enum.TileMatchEnum.ETileMatch_LogicType getLogicType() { return logicType; }
/** 逻辑类型 */
public void setLogicType(Hotfix.V01.Enum.TileMatchEnum.ETileMatch_LogicType _logicType) { logicType = _logicType; }
/** 逻辑信息 */
public byte[] getData() { return data; }
public java.nio.ByteBuffer get_buffer_Data() { if(null == data)return null; else return ByteBuffer.wrap(data); }

/** 逻辑信息 */
public void setData(byte[] _data) { data = _data; }
public void setData(java.nio.ByteBuffer _data) 
{
	if(null == _data){return;}
	int _oldPos = _data.position();
	int _bufLength = _data.remaining();
	data = new byte[_bufLength];
	_data.get(data);
	_data.position(_oldPos);
}

/** 分数 */
public int getScore() { return score; }
/** 分数 */
public void setScore(int _score) { score = _score; }


public final int GetBufSize() {
	int _size = 12;
	_size += 4 + (data == null ? 0 : data.length);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;
	_size += 4 + (data == null ? 0 : data.length);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) serialId = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) logicType = Hotfix.V01.Enum.TileMatchEnum.ETileMatch_LogicType.ETileMatch_LogicType_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _dataCount = _buf.getInt();
	if(0 < _dataCount){
		data = new byte[_dataCount];
		_buf.get(data);
	}

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) score = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(serialId);
	_buf.putInt(logicType.ordinal());

	_buf.putInt((data == null ? 0 : data.length));
	if(null != data){_buf.put(data);}

	_buf.putInt(score);
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

