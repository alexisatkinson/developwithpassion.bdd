using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using developwithpassion.bdd.containers;
using developwithpassion.bdd.mbunit;
using developwithpassion.bdd.mbunit.standard.observations;

namespace developwithpassion.bdd.core
{
    public class ObservationContext<SUT> : Observations<SUT>
    {
        public TestState<SUT> test_state { get; private set; }
        ObservationCommandFactory observation_command_factory;
        MockFactory mock_factory;


        public ObservationContext(TestState<SUT> test_state_implementation, ObservationCommandFactory observation_command_factory, MockFactory mock_factory)
        {
            this.test_state = test_state_implementation;
            this.observation_command_factory = observation_command_factory;
            this.mock_factory = mock_factory;
        }


        public void tear_down()
        {
            observation_command_factory.create_teardown_command().run();
        }

        public void reset()
        {
            observation_command_factory.create_reset_command().run();
            observation_command_factory.create_prepare_observations_command().run();
        }

        public void doing(Action because_behaviour)
        {
            test_state.behaviour_performed_in_because = because_behaviour;
        }

        public void doing<T>(Func<IEnumerable<T>> behaviour)
        {
            doing(() => behaviour().force_traversal());
        }

        public Exception exception_thrown_by_the_sut
        {
            get
            {
                return test_state.exception_thrown_while_the_sut_performed_its_work ??
                       (test_state.exception_thrown_while_the_sut_performed_its_work =
                        get_exception_throw_by(test_state.behaviour_performed_in_because));
            }
            set { test_state.exception_thrown_while_the_sut_performed_its_work = value; }
        }

        Exception get_exception_throw_by(Action because_behaviour)
        {
            return because_behaviour.get_exception();
        }

        public InterfaceType container_dependency<InterfaceType>() where InterfaceType : class
        {
            return container_dependency(an<InterfaceType>());
        }

        public InterfaceType container_dependency<InterfaceType>(InterfaceType instance) where InterfaceType : class
        {
            UnitTestContainer.add_implementation_of(instance);
            return instance;
        }

        public object an_item_of(Type type)
        {
            return mock_factory.create_stub(type);
        }

        public InterfaceType an<InterfaceType>() where InterfaceType : class
        {
            return mock_factory.create_stub<InterfaceType>();
        }

        public void add_pipeline_behaviour(PipelineBehaviour pipeline_behaviour)
        {
            test_state.pipeline_behaviours.Add(pipeline_behaviour);
        }

        public void add_pipeline_behaviour(Action context, Action teardown)
        {
            add_pipeline_behaviour(new PipelineBehaviour(context, teardown));
        }

        public ChangeValueInPipeline change(Expression<Func<object>> static_expression)
        {
            return new ChangeValueInPipeline(add_pipeline_behaviour, static_expression);
        }

        public void before_all_observations()
        {
            observation_command_factory.create_before_all_observations_command().run();
        }

        public void after_all_observations()
        {
            observation_command_factory.create_after_all_observations_command().run();
        }
    }
}