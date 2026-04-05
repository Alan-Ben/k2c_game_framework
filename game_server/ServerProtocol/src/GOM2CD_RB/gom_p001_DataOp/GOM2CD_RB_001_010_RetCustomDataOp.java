package GOM2CD_RB.gom_p001_DataOp;

import java.nio.ByteBuffer;
public class GOM2CD_RB_001_010_RetCustomDataOp implements ALBasicProtocolPack._IALProtocolStructure {
/** 处理数据的返回内容 */
private byte[] opData;


public GOM2CD_RB_001_010_RetCustomDataOp() {
	opData = null;
}

public GOM2CD_RB_001_010_RetCustomDataOp(
	 byte[] _opData
) {	opData = _opData;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)10; }

/** 处理数据的返回内容 */
public byte[] getOpData() { return opData; }
public java.nio.ByteBuffer get_buffer_OpData() { if(null == opData)return null; else return ByteBuffer.wrap(opData); }

/** 处理数据的返回内容 */
public void setOpData(byte[] _opData) { opData = _opData; }
public void setOpData(java.nio.ByteBuffer _opData) 
{
	if(null == _opData){return;}
	int _oldPos = _opData.position();
	int _bufLength = _opData.remaining();
	opData = new byte[_bufLength];
	_opData.get(opData);
	_opData.position(_oldPos);
}



public final int GetBufSize() {
	int _size = 0;
	_size += 4 + (opData == null ? 0 : opData.length);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + (opData == null ? 0 : opData.length);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _opDataCount = _buf.getInt();
	if(0 < _opDataCount){
		opData = new byte[_opDataCount];
		_buf.get(opData);
	}

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt((opData == null ? 0 : opData.length));
	if(null != opData){_buf.put(opData);}

}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)10);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
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

