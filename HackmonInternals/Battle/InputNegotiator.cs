namespace HackmonInternals.Battle;

public abstract class InputNegotiator
{
    public abstract bool InputsReady { get; protected set; }
    
    public abstract TurnInputs GetAllInputs();

    public virtual void PlayerAInput()
    {
    }

    public virtual void PlayerBInput()
    {
    }
}