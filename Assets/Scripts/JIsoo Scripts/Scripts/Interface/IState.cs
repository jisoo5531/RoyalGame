public interface IState<T>
{
    void OperateEnter(T sender, float speed);
    void OperateUpdate(T sender);
    void OperateExit(T sender);
}