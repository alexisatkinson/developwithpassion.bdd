using developwithpassion.bdd.contexts;
using developwithpassion.bdd.core.commands;
using developwithpassion.bdd.core.extensions;

namespace developwithpassion.bdd.core
{
    public class TearDownCommand<SUT> : Command
    {
        TestState<SUT> test_state;
        DelegateController delegate_controller;

        public TearDownCommand(TestState<SUT> test_state, DelegateController delegate_controller)
        {
            this.test_state = test_state;
            this.delegate_controller = delegate_controller;
        }

        public void run()
        {
            delegate_controller.run_block<after_each_observation>();
            test_state.run_teardown_pipeline();
            test_state.dependencies.Clear();
        }
    }
}