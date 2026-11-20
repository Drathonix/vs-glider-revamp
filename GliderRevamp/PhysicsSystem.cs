using System;
using System.Collections.Generic;
using System.Text;
using Vintagestory.API.Common.Entities;

namespace GliderRevamp;

public interface IPhysicsSystem
{
    public bool Calculate(float dt, Entity entity, EntityPos pos, EntityControls controls);
    public bool ShouldApplyGravity(float dt, Entity entity, EntityPos pos, EntityControls controls);
}
