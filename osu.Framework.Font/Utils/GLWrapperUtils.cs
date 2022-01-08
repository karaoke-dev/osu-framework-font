// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using System.Reflection;
using osu.Framework.Graphics.OpenGL;

namespace osu.Framework.Utils
{
    public static class GLWrapperUtils
    {
        /// <summary>
        /// use reflection to call ScheduleDisposal in <see cref="GLWrapper"/>
        /// </summary>
        /// <param name="disposalAction"></param>
        /// <param name="target"></param>
        /// <typeparam name="T"></typeparam>
        /// <exception cref="NullReferenceException"></exception>
        public static void ScheduleDisposal<T>(Action<T> disposalAction, T target)
        {
            var prop = typeof(GLWrapper).GetRuntimeMethods().FirstOrDefault(x => x.Name == "ScheduleDisposal");
            if (prop == null)
                throw new NullReferenceException();

            MethodInfo generic = prop.MakeGenericMethod(typeof(T));
            generic.Invoke(prop, new object[] { disposalAction, target });
        }
    }
}
