using System;
using System.Reflection;
using Terraria;

namespace NakedDash
{
    public static class PlayerExtentions
    {
        private static MethodInfo DoCommonDashHandleCache = null;
        public static void DoCommonDashHandle( Player playerInstance, out int dir, out bool dashing )
        {
            if ( DoCommonDashHandleCache == null )
            {
                Type playerType = playerInstance.GetType();
                string methodName = "DoCommonDashHandle";
                BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;
                DoCommonDashHandleCache = playerType.GetMethod(methodName, bindingFlags);
            }

            object[] parameters = { null, null, null };
            DoCommonDashHandleCache.Invoke(playerInstance, parameters);
            dir = (int)parameters[ 0 ];
            dashing = (bool)parameters[ 1 ];
        }
    }
}
