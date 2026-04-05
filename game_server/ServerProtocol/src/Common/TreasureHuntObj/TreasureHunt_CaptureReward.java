package Common.TreasureHuntObj;

import java.nio.ByteBuffer;
/*********
 * 太空寻宝-捕捉奖励
 **/
public class TreasureHunt_CaptureReward implements ALBasicProtocolPack._IALProtocolStructure {
/** 获得类型 */
private Common.TreasureHuntEnum.ETreasureHuntGainType gainType;
/** 数据 */
private byte[] data;


public TreasureHunt_CaptureReward() {
	gainType = Common.TreasureHuntEnum.ETreasureHuntGainType.values()[0];
	data = null;
}

public TreasureHunt_CaptureReward(
	 Common.TreasureHuntEnum.ETreasureHuntGainType _gainType
	, byte[] _data
) {	gainType = _gainType;
	data = _data;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 获得类型 */
public Common.TreasureHuntEnum.ETreasureHuntGainType getGainType() { return gainType; }
/** 获得类型 */
public void setGainType(Common.TreasureHuntEnum.ETreasureHuntGainType _gainType) { gainType = _gainType; }
/** 数据 */
public byte[] getData() { return data; }
public java.nio.ByteBuffer get_buffer_Data() { if(null == data)return null; else return ByteBuffer.wrap(data); }

/** 数据 */
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



public final int GetBufSize() {
	int _size = 4;
	_size += 4 + (data == null ? 0 : data.length);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 6;
	_size += 4 + (data == null ? 0 : data.length);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) gainType = Common.TreasureHuntEnum.ETreasureHuntGainType.ETreasureHuntGainType_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _dataCount = _buf.getInt();
	if(0 < _dataCount){
		data = new byte[_dataCount];
		_buf.get(data);
	}

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(gainType.ordinal());

	_buf.putInt((data == null ? 0 : data.length));
	if(null != data){_buf.put(data);}

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

