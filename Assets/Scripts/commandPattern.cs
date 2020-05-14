public abstract class Command {
    public abstract void execute(); // abstractは継承先で必ず実装 C++のvirtual

    public virtual void undo() {} // virtualは継承先で実装してもよいし、しなくてもよい
}