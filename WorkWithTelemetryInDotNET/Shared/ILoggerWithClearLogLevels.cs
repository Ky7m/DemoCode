using System;
using JetBrains.Annotations;

namespace Shared;

[PublicAPI]
public interface ILoggerWithClearLogLevels
{
    void InDevelopmentMode(string message);
    void InProductionMode(string message);
    void ToInvestigateTomorrow(Exception exception);
    void WakeMeInTheMiddleOfTheNight(Exception exception);
}