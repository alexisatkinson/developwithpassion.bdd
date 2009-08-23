using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace developwithpassion.bdd.core
{
    public interface Observations<SUT> {
        void tear_down();
        void reset();
        void doing(Action because_behaviour);
        void doing<T>(Func<IEnumerable<T>> behaviour);
        InterfaceType container_dependency<InterfaceType>() where InterfaceType : class;
        InterfaceType container_dependency<InterfaceType>(InterfaceType instance) where InterfaceType : class;
        object an_item_of(Type type);
        InterfaceType an<InterfaceType>() where InterfaceType : class;
        void add_pipeline_behaviour(PipelineBehaviour pipeline_behaviour);
        void add_pipeline_behaviour(Action context, Action teardown);
        ChangeValueInPipeline change(Expression<Func<object>> static_expression);
        void before_all_observations();
        void after_all_observations();
        TestState<SUT> test_state { get; }
        Exception exception_thrown_by_the_sut { get; set; }
        SystemUnderTestDependencyBuilder system_under_test_dependency_builder { get;}
        Contract build_sut<Contract, Class>();
    }
}