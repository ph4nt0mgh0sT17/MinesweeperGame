using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Mikrite.Core.DependencyInjection;

namespace MinesweeperGame.DependencyInjection
{
    /// <summary>
    /// The dependency injection provider that only provides the used services as static.
    /// </summary>
    internal static class DependencyInjectionProvider
    {
        /// <summary>
        /// Retrieves the <see cref="ILogger"/> from the Mikrite Dependency Injection Provider
        /// </summary>
        public static ILogger Logger => MikriteProvider.RetrieveService<ILogger>();
    }
}
