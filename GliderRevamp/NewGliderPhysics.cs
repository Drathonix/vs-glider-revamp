using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;

namespace GliderRevamp;

public class NewGliderPhysics: IPhysicsSystem
{
    public bool Calculate(float dt, Entity entity, EntityPos pos, EntityControls controls)
    {
        if (!controls.Gliding)
        {
            return false;
        }
        Turn(dt, entity, pos, controls);
        double drag = GliderRevampModSystem.EVENTS.InvokeCalculateDragCoefficient(entity, pos, ModConfig.Instance.DragCoefficiency);
        double surfaceArea = GliderRevampModSystem.EVENTS.InvokeCalculateSurfaceArea(entity, ModConfig.Instance.WingSurfaceArea);
        double density = GetAirDensity(entity, pos);
        pos.Motion.Add(
            (ComputeLiftVector(pos, density, surfaceArea)
        ).Add(
            ComputeDragVector(pos, density, drag)
        ).Add(
            ComputeWeightVector(pos)
        )*dt);
        // Doing this for compatibility reasons.
        controls.GlideSpeed = pos.Motion.Length();
        return true;
    }

    private void Turn(float dt, Entity entity, EntityPos pos, EntityControls controls)
    {
        if (controls.Right)
        {
            pos.Roll += 0.1F;
        }
        else if (controls.Left)
        {
            pos.Roll -= 0.1F;
        }
    }
    private static readonly Vec3d GRAVITY = new (0, GlobalConstants.GravityPerSecond, 0);
    public static Vec3d ComputeWeightVector(EntityPos pos)//, double airDensity, double minDragStrength)
    {
        /*Vec3d orientation = new Vec3d(pos.Roll, pos.Yaw, pos.Pitch).Normalize();
        Vec3d directionVector = GRAVITY.Clone();
        double angleRads = Math.Acos(orientation.Dot(directionVector)) / (orientation.Length() * directionVector.Length());
        double perpendicularity = angleRads / double.Pi;

        Vec3d drag =
        drag = new Vec3d(drag.X * drag.X, drag.Y * drag.Y, drag.Z * drag.Z) / 2;*/
        return GRAVITY.Clone();
    }

    private static readonly Vec3d UP = new (0, 1, 0);

    /// <summary>
    /// Lift is perpendicular to the plane of air flow.
    /// <param name="pos"></param>
    /// </summary>
    
    public static Vec3d ComputeLiftVector(EntityPos pos, double airDensity, double surfaceArea)
    {
        //lift = coefficient x(density of the air) x(velocity ^ 2 / 2) x(wing area)
        Vec3d orientation = new Vec3d(pos.Roll, pos.Yaw, pos.Pitch).Normalize();
        Vec3d lift = pos.Motion;
        lift = new Vec3d(lift.X * lift.X, lift.Y * lift.Y, lift.Z * lift.Z) / 2;
        Vec3d perpendicular = orientation.Cross(UP).Normalize();
        return perpendicular * airDensity * surfaceArea * lift.Length(); 
    }



    /// <summary>
    /// Drag is opposite of the current motion vector. It is also affected by the orientation of the wings.
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public static Vec3d ComputeDragVector(EntityPos pos, double airDensity, double minDragStrength)
    {
        Vec3d orientation = new Vec3d(pos.Roll, pos.Yaw, pos.Pitch).Normalize();
        Vec3d directionVector = pos.Motion.Clone().Normalize();
        double angleRads = Math.Acos(orientation.Dot(directionVector)) / (orientation.Length() * directionVector.Length());
        double perpendicularity = angleRads/double.Pi;
        Vec3d drag = pos.Motion;
        drag = new Vec3d(drag.X*drag.X,drag.Y*drag.Y,drag.Z*drag.Z) / 2;
        return drag.Mul(-double.Max(minDragStrength, perpendicularity)) * airDensity;
    }

    /// <summary>
    /// Returns [0-pi] when aimed down and [-pi-0] when aimed up.
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public static double GetPitchDirection(EntityPos pos)
    {
        // Down is 2pi, Up is 0.
        return pos.Pitch - double.Pi;
    }

    /// <summary>
    /// Returns [0-1] when aimed down and [-1-0] when aimed up.
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public static double GetPitchFrac(EntityPos pos)
    {
        return GetPitchDirection(pos) / double.Pi;
    }

    /// <summary>
    /// The density of the air. Currently assumes that this is unaffected by the world state and just the entity's height.
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="pos"></param>
    /// <returns>A number between 0-1 with 0 representing no air present</returns>
    public static double GetAirDensity(Entity entity, EntityPos pos)
    {
        var dropoffStart = entity.World.SeaLevel * ModConfig.Instance.AirDensityDropOff;
        if (pos.Y <= dropoffStart)
        {
            return 1D;
        }
        return (ModConfig.Instance.AirDensityDropOffMax - (pos.Y - dropoffStart)) / ModConfig.Instance.AirDensityDropOffMax;
    }

    /// <summary>
    /// Currently returns a constant. I may change this to be affected by inventory and other stats.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static double GetMass(Entity entity)
    {
        return 1D; 
    }

    /// <summary>
    /// Gets the arbitrary mass of the entity's world. Used for gravity calculations. Does nothing for now but this provides a way to account for other dimensions in the future. 
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static double GetWorldMass(Entity entity)
    {
        return 1D;
    }

    public static double GetSpeed(EntityPos pos)
    {
        return pos.Motion.Length();
    }

    public bool ShouldApplyGravity(float dt, Entity entity, EntityPos pos, EntityControls controls)
    {
        return !controls.Gliding;
    }
}
