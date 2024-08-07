public interface IState<T>
{
    void OperateEnter(T sender, float speed, bool isPrince);
    void OperateUpdate(T sender, bool isRun);
    void OperateExit(T sender);
}