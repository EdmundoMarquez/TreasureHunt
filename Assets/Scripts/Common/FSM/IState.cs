namespace Treasure.Common
{
    public interface IState
    {
        void Awake();
        void Init();
        void Tick();
        void FixedTick();
        void Stop();
    }
}
