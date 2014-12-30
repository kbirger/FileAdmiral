using Ninject.Extensions.Factory;
using Ninject.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FileAdmiral.IoC
{
    public class UseFirstArgumentAsNameInstanceProvider : StandardInstanceProvider
    {
        protected override string GetName(System.Reflection.MethodInfo methodInfo, object[] arguments)
        {
            return (string)arguments[0];
        }

        /// <summary>
        /// Gets the constructor arguments that shall be passed with the instance request.
        /// </summary>
        /// <param name="methodInfo">The method info of the method that was called on the factory.</param><param name="arguments">The arguments that were passed to the factory.</param>
        /// <returns>
        /// The constructor arguments that shall be passed with the instance request.
        /// </returns>
        protected override IConstructorArgument[] GetConstructorArguments(MethodInfo methodInfo, object[] arguments)
        {
            return base.GetConstructorArguments(methodInfo, arguments).Skip(1).ToArray(); ;
        }
    }
}
