package Common;

import java.nio.ByteBuffer;
public class WCGGS2GC_SeasonRankInfo implements ALBasicProtocolPack._IALProtocolStructure {
private int seaonId;
private int rank;
private int grade;
private int star;
private int legendScore;


public WCGGS2GC_SeasonRankInfo() {
	seaonId = 0;
	rank = 0;
	grade = 0;
	star = 0;
	legendScore = 0;
}

public WCGGS2GC_SeasonRankInfo(
	 int _seaonId
	, int _rank
	, int _grade
	, int _star
	, int _legendScore
) {	seaonId = _seaonId;
	rank = _rank;
	grade = _grade;
	star = _star;
	legendScore = _legendScore;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public int getSeaonId() { return seaonId; }
public void setSeaonId(int _seaonId) { seaonId = _seaonId; }
public int getRank() { return rank; }
public void setRank(int _rank) { rank = _rank; }
public int getGrade() { return grade; }
public void setGrade(int _grade) { grade = _grade; }
public int getStar() { return star; }
public void setStar(int _star) { star = _star; }
public int getLegendScore() { return legendScore; }
public void setLegendScore(int _legendScore) { legendScore = _legendScore; }


public final int GetBufSize() {
	int _size = 20;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 22;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) seaonId = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) rank = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) grade = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) star = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) legendScore = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(seaonId);
	_buf.putInt(rank);
	_buf.putInt(grade);
	_buf.putInt(star);
	_buf.putInt(legendScore);
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

