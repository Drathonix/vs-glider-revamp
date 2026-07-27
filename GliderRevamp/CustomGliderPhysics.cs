using GliderRevamp.Patches;
using System;
using System.Collections.Generic;
using System.Text;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;

namespace GliderRevamp;

public class CustomGliderPhysics
{
    private static Vec3d RotateTowards(Vec3d fromDir, Vec3d toDir, double maxRadians)
    {
        fromDir = fromDir.Normalize();
        toDir = toDir.Normalize();

        var dot = GameMath.Clamp(fromDir.Dot(toDir), -1, 1);
        var angle = Math.Acos(dot);
        if (angle < 1e-6) return toDir;

        var t = Math.Min(1.0, maxRadians / angle);

        // Slerp on the unit sphere
        var sinAngle = Math.Sin(angle);
        var a = Math.Sin((1 - t) * angle) / sinAngle;
        var b = Math.Sin(t * angle) / sinAngle;

        var blended = fromDir * a + toDir * b;
        return blended.Normalize();
    }

    public static bool Calculate(PModulePlayerInAir pModule, float dt, Entity entity, EntityPos pos, EntityControls controls)
    {
        if (!controls.Gliding)
        {
            return false;
        }

        var config = ModConfig.Instance;

        var v = pos.Motion;
        var speed = v.Length();
        if (speed < GliderEvents.InvokeCalculateStallSpeed(entity,pos,ModConfig.Instance.StallSpeedMs) / 60f || config.DisableGlider)
        {
            controls.Gliding = false;
            controls.GlideSpeed = 0;
            return true;
        }

        if (controls.GlideSpeed <= 0)
        {
            controls.GlideSpeed = speed;
        }

        var vDir = v.Normalize();
        var viewDir = pos.GetViewVector().ToVec3d().Normalize();

        var turnRateRadPerSec = config.TurnRate * (float)Math.PI / 180f;
        var maxTurn = turnRateRadPerSec * dt;

        var newDir = RotateTowards(vDir, viewDir, maxTurn);

        var energy = controls.GlideSpeed;

        // Apply lift.
        energy -= GliderEvents.InvokeCalculateClimbCoefficient(entity,pos,config.ClimbCoefficiency) * v.Y * dt;

        // Apply drag.
        energy -= GliderEvents.InvokeCalculateDragCoefficient(entity,pos,config.DragCoefficiency) * Math.Max(speed * speed, 0.15f) * dt;

        // Limit new speed to terminal velocity.
        energy = GameMath.Clamp(energy, 0, config.TerminalVelocityMs / 60f);

        controls.GlideSpeed = energy;

        pos.Motion = newDir * energy;

        return true;
    }
    public static bool CanGlide(ModSystemGliding system, EntityPlayer entity)
    {
        if (ModConfig.Instance.DisableGlider)
        {
            return false;
        }
        var speedMs = entity.Pos.Motion.Length() * 60f;
        var activationSpeed = GliderEvents.InvokeCalculateActivationSpeed(entity, entity.Pos, ModConfig.Instance.ActivationSpeedMs);
        var upwardSpeed = entity.Pos.Motion.Y; // Prevent activation when jumping.

        return !(speedMs < activationSpeed || upwardSpeed > -3f / 60f);
    }

}
