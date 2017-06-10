//using System;

//namespace EffectiveMemoryManagement
//{
//    static class ClosureComplex
//    {
//        static void Main(string[] args)
//        {
//            var tasks = new FluentTask();
//            var string1 = "toBeCaptured1";
//            var string2 = "toBeCaptured2";
//            tasks.AddTask(() =>
//            {
//                var temp = string1 + "some cool information";
//            }, () =>
//            {
//                var temp = string2 + "some great information";
//            });
//        }
//    }

//    public class FluentTask
//    {
//        Action _taskAction;
//        Action _onComplete;

//        public FluentTask AddTask(Action taskAction, Action onComplete)
//        {
//            _taskAction = taskAction;
//            _onComplete = onComplete;
//            return this;
//        }
//    }
//}