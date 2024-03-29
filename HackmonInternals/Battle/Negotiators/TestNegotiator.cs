namespace HackmonInternals.Battle.Negotiators
{
    public sealed class TestNegotiator : InputNegotiator
    {
        public override bool InputsReady
        {
            get => true;
            protected set => _ = value;
        }

        public override List<TurnInput> GetAllInputs()
        {
            return new();
        }
    }
}