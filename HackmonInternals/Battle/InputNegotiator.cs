namespace HackmonInternals.Battle;

public abstract class InputNegotiator
{
    public abstract bool InputsReady { get; protected set; }
    
    public abstract List<TurnInput> GetAllInputs();

    public virtual void PlayerAInput()
    {
    }

    public virtual void PlayerBInput()
    {
    }
}