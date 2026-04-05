package Common;

import java.nio.ByteBuffer;
public class WCGGS2GC_CasualStatInfo implements ALBasicProtocolPack._IALProtocolStructure {
private long casualId;
private int win;
private int lose;
private int draw;
private int serialWin;
private int serialLose;


public WCGGS2GC_CasualStatInfo() {
	casualId = (long)0;
	win = 0;
	lose = 0;
	draw = 0;
	serialWin = 0;
	serialLose = 0;
}

public WCGGS2GC_CasualStatInfo(
	 long _casualId
	, int _win
	, int _lose
	, int _draw
	, int _serialWin
	, int _serialLose
) {	casualId = _casualId;
	win = _win;
	lose = _lose;
	draw = _draw;
	serialWin = _serialWin;
	serialLose = _serialLose;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getCasualId() { return casualId; }
public void setCasualId(long _casualId) { casualId = _casualId; }
public int getWin() { return win; }
public void setWin(int _win) { win = _win; }
public int getLose() { return lose; }
public void setLose(int _lose) { lose = _lose; }
public int getDraw() { return draw; }
public void setDraw(int _draw) { draw = _draw; }
public int getSerialWin() { return serialWin; }
public void setSerialWin(int _serialWin) { serialWin = _serialWin; }
public int getSerialLose() { return serialLose; }
public void setSerialLose(int _serialLose) { serialLose = _serialLose; }


public final int GetBufSize() {
	int _size = 28;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 30;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) casualId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) win = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) lose = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) draw = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) serialWin = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) serialLose = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(casualId);
	_buf.putInt(win);
	_buf.putInt(lose);
	_buf.putInt(draw);
	_buf.putInt(serialWin);
	_buf.putInt(serialLose);
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

