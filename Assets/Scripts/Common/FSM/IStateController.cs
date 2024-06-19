namespace Treasure.Common
{
    public interface IStateController 
    {
        void ChangeToNextState(int nextState);
        void DelayChangeToNextState(int nextState, int delayTime);
    }
}