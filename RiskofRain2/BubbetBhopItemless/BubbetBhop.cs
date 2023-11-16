using BepInEx.Configuration;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using System;
using System.Reflection;
using UnityEngine;

namespace BubbetBhopItemless
{
    public static class BubbetBhop
    {
        public static ConfigEntry<float> BaseAirControl;

        public static ConfigEntry<int> ScalingFunction;

        public static ConfigEntry<float> ScalingFunctionValA;

        private static Func<float, float, int, float, float>[] ScalingFuncs =
            [
                ( float @base, float added, int count, float _ ) => @base + (float)(count - 1) * added,
                ( float @base, float added, int count, float arg1 ) => @base + Mathf.Pow((float)count, arg1) * added,
                ( float @base, float added, int count, float arg1 ) => arg1 / ((float)count - added) + @base,
            ];
        public static void Init( ConfigFile config )
        {

            BaseAirControl = config.Bind<float>("General", "Base Air Control", 3f, "Base air control, more = easier to turn and still gain speed.");
            ScalingFunction = config.Bind<int>("Advanced", "Scaling Function", 0, "Changes the scaling function of the bunny feet: 0 = linear(base + (count-1)*added), 1 = power law (base + Mathf.Pow(count, arg1) * added), 2 = Rectangular Hyperbola (arg1 / (count-added) + base)");
            ScalingFunctionValA = config.Bind<float>("Advanced", "Scaling Function Arg1", 0.5f, "Changes the first arbitrary value in scaling function: ex: power law's exponent (arg1)");

            IL.RoR2.Projectile.ProjectileGrappleController.GripState.FixedUpdateBehavior += GripState_FixedUpdateBehavior;
            IL.EntityStates.GenericCharacterMain.ApplyJumpVelocity += GenericCharacterMain_ApplyJumpVelocity;
            IL.RoR2.CharacterMotor.PreMove += CharacterMotor_PreMove;
        }

        private static void GripState_FixedUpdateBehavior( ILContext il )
        {
            ILCursor ilcursor = new ILCursor(il);
            Func<Instruction, bool>[] array = new Func<Instruction, bool>[ 3 ];
            array[ 0 ] = ( Instruction x ) => ILPatternMatchingExt.MatchCall<Vector3>(x, "op_Multiply");
            array[ 1 ] = ( Instruction x ) => ILPatternMatchingExt.MatchLdcI4(x, 1);
            array[ 2 ] = ( Instruction x ) => ILPatternMatchingExt.MatchLdcI4(x, 1);
            ilcursor.GotoNext(array);
            ilcursor.Index += 2;
            ilcursor.Remove();
            ilcursor.Emit(OpCodes.Ldc_I4_0);
        }

        private static void CharacterMotor_PreMove( ILContext il )
        {
            ILCursor ilcursor3 = new ILCursor(il);
            Func<Instruction, bool>[] array4 = new Func<Instruction, bool>[ 6 ];
            array4[ 0 ] = ( Instruction x ) => ILPatternMatchingExt.MatchLdarg(x, 0);
            array4[ 1 ] = ( Instruction x ) => ILPatternMatchingExt.MatchLdfld<RoR2.CharacterMotor>(x, "velocity");
            array4[ 2 ] = ( Instruction x ) => ILPatternMatchingExt.MatchLdloc(x, 2);
            array4[ 3 ] = ( Instruction x ) => ILPatternMatchingExt.MatchLdloc(x, 0);
            array4[ 4 ] = ( Instruction x ) => ILPatternMatchingExt.MatchLdarg(x, 1);
            array4[ 5 ] = ( Instruction x ) => ILPatternMatchingExt.MatchMul(x);
            ilcursor3.GotoNext(array4);
            ilcursor3.Index += 7;
            ilcursor3.Emit(OpCodes.Ldarg_0);
            ilcursor3.Emit(OpCodes.Ldfld, typeof(RoR2.CharacterMotor).GetRuntimeField("velocity"));
            ilcursor3.Emit(OpCodes.Ldloc_2);
            ilcursor3.Emit(OpCodes.Ldarg_1);
            ilcursor3.Emit(OpCodes.Ldarg_0);
            ilcursor3.Emit(OpCodes.Ldarg_0);
            ilcursor3.Emit(OpCodes.Ldfld, typeof(RoR2.CharacterMotor).GetField("body", BindingFlags.Instance | BindingFlags.NonPublic));
            ilcursor3.Emit(OpCodes.Ldarg_0);
            ilcursor3.Emit(OpCodes.Ldfld, typeof(RoR2.CharacterMotor).GetField("disableAirControlUntilCollision", BindingFlags.Instance | BindingFlags.Public));
            ilcursor3.EmitDelegate<NewMove>(new NewMove(NewMoveMeth));
        }

        private static void GenericCharacterMain_ApplyJumpVelocity( ILContext il )
        {
            Func<Vector3, RoR2.CharacterMotor, RoR2.CharacterBody, Vector3> func = delegate ( Vector3 vector, RoR2.CharacterMotor cm, RoR2.CharacterBody cb )
            {
                Vector3 vector2 = vector + Vector3.down * vector.y;
                Vector3 vector3 = cm.velocity + Vector3.down * cm.velocity.y;
                if ( vector3.sqrMagnitude > vector2.sqrMagnitude )
                {
                    vector2 = vector3;
                }
                vector2.y = vector.y;
                return vector2;
            };
            ILCursor ilcursor2 = new ILCursor(il);
            Func<Instruction, bool>[] array2 = new Func<Instruction, bool>[ 3 ];
            array2[ 0 ] = ( Instruction x ) => ILPatternMatchingExt.MatchLdarg(x, 0);
            array2[ 1 ] = ( Instruction x ) => ILPatternMatchingExt.MatchLdloc(x, 0);
            array2[ 2 ] = ( Instruction x ) => ILPatternMatchingExt.MatchStfld<CharacterMotor>(x, "velocity");
            ilcursor2.GotoNext(array2);
            ilcursor2.Index += 2;
            ilcursor2.Emit(OpCodes.Ldarg_0);
            ilcursor2.Emit(OpCodes.Ldarg_1);
            ilcursor2.EmitDelegate<Func<Vector3, CharacterMotor, CharacterBody, Vector3>>(func);
            Func<Instruction, bool>[] array3 = new Func<Instruction, bool>[ 3 ];
            array3[ 0 ] = ( Instruction x ) => ILPatternMatchingExt.MatchLdarg(x, 0);
            array3[ 1 ] = ( Instruction x ) => ILPatternMatchingExt.MatchLdloc(x, 2);
            array3[ 2 ] = ( Instruction x ) => ILPatternMatchingExt.MatchStfld<CharacterMotor>(x, "velocity");
            ilcursor2.GotoNext(array3);
            ilcursor2.Index += 2;
            ilcursor2.Emit(OpCodes.Ldarg_0);
            ilcursor2.Emit(OpCodes.Ldarg_1);
            ilcursor2.EmitDelegate<Func<Vector3, CharacterMotor, CharacterBody, Vector3>>(func);
        }

        private static Vector3 NewMoveMeth( Vector3 velocityFromMoveTowards, Vector3 velocityOld, Vector3 target, float deltaTime, CharacterMotor self, CharacterBody body, bool disableAirControlUntilCollision )
        {
            Vector3 vector2;
            if ( !disableAirControlUntilCollision && !self.Motor.GroundingStatus.IsStableOnGround )
            {
                Vector3 vector = target;
                if ( !self.isFlying )
                {
                    vector.y = 0f;
                }
                Vector3 normalized = vector.normalized;
                float num = self.walkSpeed * normalized.magnitude;
                vector2 = Accelerate(velocityOld, normalized, num, ScalingFuncs[ ScalingFunction.Value ](BaseAirControl.Value, 1.5f, 1, ScalingFunctionValA.Value), self.acceleration, deltaTime);
            }
            else
            {
                vector2 = velocityFromMoveTowards;
            }
            return vector2;
        }

        private static Vector3 Accelerate( Vector3 velocity, Vector3 wishdir, float wishspeed, float speedLimit, float acceleration, float deltaTime )
        {
            if ( speedLimit > 0f && wishspeed > speedLimit )
            {
                wishspeed = speedLimit;
            }
            float num = Vector3.Dot(velocity, wishdir);
            float num2 = wishspeed - num;
            Vector3 vector;
            if ( num2 <= 0f )
            {
                vector = velocity;
            }
            else
            {
                float num3 = acceleration * deltaTime * wishspeed;
                if ( num3 > num2 )
                {
                    num3 = num2;
                }
                vector = velocity + wishdir * num3;
            }
            return vector;
        }
        private delegate Vector3 NewMove( Vector3 velocityFromMoveTowards, Vector3 velocityOld, Vector3 target, float deltaTime, CharacterMotor self, CharacterBody body, bool disableAirControlUntilCollision );
    }
}
