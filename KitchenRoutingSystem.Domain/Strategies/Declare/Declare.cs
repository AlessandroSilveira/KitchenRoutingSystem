namespace KitchenRoutingSystem.Domain.Strategies.Declare
{
    public class Declare
    {
        private readonly IDeclare _declare;

        public Declare(IDeclare declare)
        {
            _declare = declare;
        }

        public void DeclareQueue()
        {
            _declare.Declare(this);
        }
    }
}
