package Common;

import java.nio.ByteBuffer;
public class WCGGS2GC_BattleStatInfo implements ALBasicProtocolPack._IALProtocolStructure {
private int statType;
private int win;
private int lose;
private int draw;


public WCGGS2GC_BattleStatInfo() {
	statType = 0;
	win = 0;
	lose = 0;
	draw = 0;
}

public WCGGS2GC_BattleStatInfo(
	 int _statType
	, int _win
	, int _lose
	, int _draw
) {	statType = _statType;
	win = _win;
	lose = _lose;
	draw = _draw;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public int getStatType() { return statType; }
public void setStatType(int _statType) { statType = _statType; }
public int getWin() { return win; }
public void setWin(int _win) { win = _win; }
public int getLose() { return lose; }
public void setLose(int _lose) { lose = _lose; }
public int getDraw() { return draw; }
public void setDraw(int _draw) { draw = _draw; }


public final int GetBufSize() {
	int _size = 16;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) statType = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) win = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) lose = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) draw = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(statType);
	_buf.putInt(win);
	_buf.putInt(lose);
	_buf.putInt(draw);
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

