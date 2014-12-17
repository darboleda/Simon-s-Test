namespace Canal.Engine.Physics
{
    public interface IMovementLimiter<TVector, TModel>
    {
        bool LimitMovement(ref TVector delta, ref TModel model);
    }
}
