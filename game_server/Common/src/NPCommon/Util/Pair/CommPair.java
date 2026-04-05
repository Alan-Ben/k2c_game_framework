package NPCommon.Util.Pair;

import java.util.Objects;

public class CommPair<A, B> {
    public A first;
    public B second;

    public CommPair(A paramA, B paramB) {
        this.first = paramA;
        this.second = paramB;
    }

    public CommPair() {
    }

    public static <A, B> CommPair<A, B> of(A paramA, B paramB) {
        return new CommPair<>(paramA, paramB);
    }

    public String toString() {
        return this.first + ":" + this.second;
    }

    public boolean equals(Object paramObject) {
        if (paramObject == this) {
            return true;
        }

        if (!(paramObject instanceof CommPair)) {
            return false;
        }

        CommPair<?, ?> other = (CommPair<?, ?>) paramObject;
        return Objects.equals(this.first, other.first) && Objects.equals(this.second, other.second);
    }

    public int hashCode() {
        if (this.first == null) {
            return this.second == null ? 0 : this.second.hashCode() + 1;
        } else {
            return this.second == null ? this.first.hashCode() + 2 : this.first.hashCode() * 17 + this.second.hashCode();
        }
    }
}