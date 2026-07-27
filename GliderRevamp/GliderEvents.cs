using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Util;
using static GliderRevamp.GliderEvents;

namespace GliderRevamp;

public class GliderEvents
{
    public delegate bool TryStartGlideHandler(ModSystemGliding system, EntityPlayer entity);
    public static event TryStartGlideHandler TryStartGlide;

    public delegate float CalculateDragCoefficientHandler(Entity entity, EntityPos pos, float drag);
    private static event CalculateDragCoefficientHandler CalculateDragCoefficient;

    public delegate float CalculateClimbCoefficientHandler(Entity entity, EntityPos pos, float climb);
    private static event CalculateClimbCoefficientHandler CalculateClimbCoefficient;

    public delegate float CalculateStallSpeedHandler(Entity entity, EntityPos pos, float stall);
    private static event CalculateStallSpeedHandler CalculateStallSpeed;

    public delegate float CalculateActivationSpeedHandler(Entity entity, EntityPos pos, float activation);
    private static event CalculateActivationSpeedHandler CalculateActivationSpeed;

    public delegate float CalculateTurnRateHandler(Entity entity, EntityPos pos, float turnrate);
    private static event CalculateTurnRateHandler CalculateTurnRate;

    public delegate float CalculateTerminalVelocityHandler(Entity entity, EntityPos pos, float velocity);
    private static event CalculateTerminalVelocityHandler CalculateTerminalVelocity;

    public delegate bool BeforeGliderPhysicsCalculationsHandler(PModulePlayerInAir pModule, float dt, Entity entity, EntityPos pos, EntityControls controls);
    private static event BeforeGliderPhysicsCalculationsHandler BeforePhysicsCalculations;

    public static void RegisterBeforeGliderPhysicsCalculations(BeforeGliderPhysicsCalculationsHandler del, int priority = int.MaxValue){
        // I POSSESS GREAT AND TERRIFYING POWER! HAHA!
        EventExtensions.AddWithPriority(del, priority, 
            d => BeforePhysicsCalculations += d, 
            d => BeforePhysicsCalculations -= d);
    }

    public static void RegisterCalculateDragCoefficient(CalculateDragCoefficientHandler del, int priority = int.MaxValue)
    {
        EventExtensions.AddWithPriority(del, priority,
            d => CalculateDragCoefficient += d,
            d => CalculateDragCoefficient -= d);
    }

    public static void RegisterCalculateClimbCoefficient(CalculateClimbCoefficientHandler del, int priority = int.MaxValue)
    {
        EventExtensions.AddWithPriority(del, priority,
            d => CalculateClimbCoefficient += d,
            d => CalculateClimbCoefficient -= d);
    }

    public static void RegisterCalculateStallSpeed(CalculateStallSpeedHandler del, int priority = int.MaxValue)
    {
        EventExtensions.AddWithPriority(del, priority,
            d => CalculateStallSpeed += d,
            d => CalculateStallSpeed -= d);
    }

    public static void RegisterCalculateActivationSpeed(CalculateActivationSpeedHandler del, int priority = int.MaxValue)
    {
        EventExtensions.AddWithPriority(del, priority,
            d => CalculateActivationSpeed += d,
            d => CalculateActivationSpeed -= d);
    }

    public static void RegisterCalculateTurnRate(CalculateTurnRateHandler del, int priority = int.MaxValue)
    {
        EventExtensions.AddWithPriority(del, priority,
            d => CalculateTurnRate += d,
            d => CalculateTurnRate -= d);
    }

    public static void RegisterCalculateTerminalVelocity(CalculateTerminalVelocityHandler del, int priority = int.MaxValue)
    {
        EventExtensions.AddWithPriority(del, priority,
            d => CalculateTerminalVelocity += d,
            d => CalculateTerminalVelocity -= d);
    }

    internal static float InvokeCalculateActivationSpeed(Entity entity, EntityPos pos, float activation)
    {
        if (CalculateActivationSpeed != null) foreach (var item in CalculateActivationSpeed?.GetInvocationList())
        {
            activation = ((CalculateActivationSpeedHandler)item).Invoke(entity, pos, activation);
        }
        return activation;
    }
    internal static float InvokeCalculateStallSpeed(Entity entity, EntityPos pos, float stall)
    {
        if (CalculateStallSpeed != null) foreach (var item in CalculateStallSpeed?.GetInvocationList())
        {
            stall = ((CalculateStallSpeedHandler)item).Invoke(entity, pos, stall);
        }
        return stall;
    }
    internal static float InvokeCalculateDragCoefficient(Entity entity, EntityPos pos, float drag)
    {
        if (CalculateDragCoefficient != null) foreach (var item in CalculateDragCoefficient?.GetInvocationList())
        {
            drag = ((CalculateDragCoefficientHandler)item).Invoke(entity, pos, drag);
        }
        return drag;
    }

    internal static float InvokeCalculateClimbCoefficient(Entity entity, EntityPos pos, float climb)
    {
        if (CalculateClimbCoefficient != null) foreach (var item in CalculateClimbCoefficient?.GetInvocationList())
        {
            climb = ((CalculateClimbCoefficientHandler)item).Invoke(entity, pos, climb);
        }
        return climb;
    }

    internal static float InvokeCalculateTurnRate(Entity entity, EntityPos pos, float rate)
    {
        if (CalculateTurnRate != null) foreach (var item in CalculateTurnRate?.GetInvocationList())
        {
            rate = ((CalculateTurnRateHandler)item).Invoke(entity, pos, rate);
        }
        return rate;
    }
    internal static float InvokeCalculateTerminalVelocity(Entity entity, EntityPos pos, float velocity)
    {
        if (CalculateTerminalVelocity != null) foreach (var item in CalculateTerminalVelocity?.GetInvocationList())
        {
            velocity = ((CalculateTerminalVelocityHandler)item).Invoke(entity, pos, velocity);
        }
        return velocity;
    }
    internal static bool InvokeBeforePhysicsCalculations(PModulePlayerInAir pModule, float dt, Entity entity, EntityPos pos, EntityControls controls)
    {
        return BeforePhysicsCalculations.InvokeUntil(pModule, dt, entity, pos, controls);
    }
    internal static bool InvokeTryStartGlide(ModSystemGliding system, EntityPlayer entity)
    {
        return TryStartGlide.InvokeUntil(system, entity);
    }

    internal static void Init()
    {
        TryStartGlide += CustomGliderPhysics.CanGlide;
        RegisterBeforeGliderPhysicsCalculations(CustomGliderPhysics.Calculate);
    }

}
public static class EventExtensions
{
    private static readonly Dictionary<Type, PriorityQueue<Delegate, int>> priorityTable = [];

    extension(Delegate del)
    {
        public bool InvokeUntil(params object[] args)
        {
            if(del != null) foreach (var subDelegate in del?.GetInvocationList())
            {
                bool result = (bool)subDelegate.DynamicInvoke(args);
                if (!result)
                {
                    return false;
                }
            }
            return true;
        }
    }
    public static void AddWithPriority<T>(T del, int priority, Action<T> plus, Action<T> minus) where T : Delegate
    {
        var queue = priorityTable.GetValueOrDefault(typeof(T), new PriorityQueue<Delegate, int>());
        var newQueue = new PriorityQueue<Delegate, int>();
        foreach (var item in queue.UnorderedItems)
        {
            minus.Invoke((T)item.Element);
            newQueue.Enqueue(item.Element, item.Priority);
        }
        queue.Enqueue(del, priority);
        newQueue.Enqueue(del, priority);
        while (queue.Count > 0)
        {
            plus.Invoke((T)queue.Dequeue());
        }
        priorityTable[typeof(T)] = newQueue;
    }
}
