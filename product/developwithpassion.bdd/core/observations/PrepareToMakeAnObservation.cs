using developwithpassion.bdd.contexts;
using developwithpassion.bdd.core.commands;

namespace developwithpassion.bdd.core.observations
{
    public class PrepareToMakeAnObservation<SUT> : Command
    {
        TestState<SUT> test_state;
        DelegateController controller;


        public PrepareToMakeAnObservation(TestState<SUT> test_state, DelegateController controller)
        {
            this.test_state = test_state;
            this.controller = controller;
        }

        public void run()
        {
            controller.run_block<context>();
            test_state.run_startup_pipeline();
            test_state.build_sut();
            controller.run_block<after_the_sut_has_been_created>();
            controller.run_block<because>();
        }
    }
}