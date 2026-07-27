# Glider Revamp

## Drathon's Fork Features
1. Updated to the most recent game version.
2. Custom events for glide physics calculations for developers.
3. Fixes a vanilla bug that causes glider animation stutters.

## Core functionality

An attempt to enhance the glider physics system.
The mod replaces Vintage Story's vanilla glider mechanics with a simple and probably funner flight model that gives players more control and realistic gliding behavior. While decreasing the incentive of building tower freeways.

## Key Features

### Core Mechanics
- **Speed-Based Flight Dynamics**: Players must maintain a minimum speed to keep gliding. Below the stall speed, the glider deactivates and the player free falls, preventing passive hovering.
- **Energy Management System**: Climbing, turning, and air resistance all drain speed. This prevents infinite flying and makes gliding a skill-based activity requiring careful energy management.
- **Terminal Velocity Cap**: Maximum achievable speed prevents unrealistic velocities and balances gameplay.

### Customizable Settings

**Climb Coefficiency** (default: 0.2)
- Controls energy loss when climbing upward
- Example: 0.2 means climbing 1 meter costs 0.2 m/s of speed, and descending 1 meter adds 0.2 m/s of speed.
- Higher values make climbing and descending more impactful on speed, while lower values allow for more forgiving altitude changes and smooth glide.

**Turn Rate** (default: 90 degrees/second)
- Controls how quickly the glider can change the direction
- Example: 90°/s means a 180° turn takes 2 seconds
- Lower values create slower, more graceful turns; higher values allow snappier maneuvers

**Drag Coefficiency** (default: 0.1)
- Air resistance that slows the glider over time
- Example: 0.1 means 10% of speed² is lost to drag
- Balances maximum achievable distances and prevents tower highways
- Higher values make it harder to maintain speed

**Stall Speed** (default: 4.0 m/s)
- Minimum speed required to maintain a glide
- Below this speed, the glider automatically deactivates and the player free falls
- Sets the baseline for minimum speed management

**Activation Speed** (default: 8.0 m/s)
- Minimum speed required to initially activate the glider
- Prevents accidental glider activation during normal jumping or falling
- Typically, set higher than stall speed to provide a buffer

**Terminal Velocity** (default: 40.0 m/s)
- Maximum speed the glider can achieve
- Prevents players from reaching unrealistic velocities
- Creates a natural speed ceiling for balanced gameplay

### Quality of Life Features
- **Speed Display**: Optional HUD element showing current glide speed in m/s while flying
- **Activation Gate**: Glider only activates when moving fast enough, preventing awkward early-flight situations
- **Fully Configurable**: All physics parameters can be adjusted without code changes through ConfigLib

### Roadmap

- [x] Implement FOV changes based on speed for enhanced immersion
- [x] Add sound effects that vary with speed and maneuvers
- [ ] Add roll animation and make the player face flight trajectory for visual feedback
