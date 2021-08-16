# Project Air Ride -- Development diary
_A recreation of the Kirby Air Ride game mechanics_

## Introduction
[2021-08-08]

I'm not sure if anyone is interested in this development log/diary, but I hope at least I will be.
Maybe I can make it into a good guide on how to make a game project, who knows?
I will discuss here concept ideas for the project, its goals and some technical stuff.

Anyway, this is a study project on recreating the movement mechanics of Kirby Air Ride, a game very dear to me.
My goal is to try and build the game step-by-step, begining from the movement of the machines on ground, flying, braking, charging and boosting.
Machines are the "ships" from the original game. I will continue to call the ones in this projects machines as well (please don't sue me, Nintendo).

For a second phase, I want to split development into two fronts:
(i) fine-tune some different machines, as well as allow them to have power-ups and status bonuses and
(ii) make some animations and effects, like rolling the machine when turning, spinning it around or tossing it up when damaged and adding effects when it boosts, brakes and glides.

Allowing for multiplayer would be awesome, too.
I will look into that once I have a playable sandbox!

At least, for now, that's what I want to achieve with this project.

I started this project a number of weeks ago but did not document it, so I will first describe what I did until today, then follow with the actual development diary.

## Test level
First thing I did when I started this project was creating a place to have the machine running about for testing.
I used the Pro Builder extension for Unity and it is quite easy to use, especially for prototyping terrain or simple objects.
I created a box with some different shaped ramps to test the machine's movement and a machine that could kinda look like a warp star from Kirby.

That was some time ago.
Back then I was building the code in a rather convoluted way, or at least it seemed so.
I scrapped it and restarted the project with only the 3D models I created with Pro Builder.

!(The test level and the test machine, named _Cobalt Star_.)[https://github.com/LucRosset/Air-Ride/tree/main/Development%20Log/Test%20level%202021-08-14.png]

A week ago I started implementing Unity's new input system control scheme, instead of using the classic input manager.
Unfortunately, Nintendo Switch Pro Controllers are not currently working with it, at least not as of this writing.

After coding the controls, I started playing with the physics engine to set up the machine's movement, as well as setting up the camera.
I went for a Cinemachine virtual camera to make the camera follow the player from behind.
It worked quite well, after some time learning what each option, slider and checkbox did.

At first I tried implementing movement dynamically, that is, inputting force and torque to control the movement.
It worked... well, not great.
A friend suggested me to do it kinematically, that is, setting up speeds and changing them during runtime based on acceleration values.
I was reluctant, but today (finally, caught up!) it turned out great!

## Acceleration, top speed and turning/drifting
To accelerate, I first tried using the `Vector3.MoveTowards` method, which worked perfectly.
I'd set the velocity to be moved towards the `transform.forward * topSpeed` and the machine would move and brake smoothly!

To turn, I used the `Vector3.RotateTowards` method, which is similar to `MoveTowards`, to change the machine's facing and the velocity's direction.
It worked wonderfully, but it was very stiff...

In the original game, there was some drift to the turns, more in some machines and less in others.
That made controlling the machines more difficult, but also very rewarding and I wanted to recreate that.

Here is how I recreated the machine's turning drift:

First, I realised that `RotateTowards` was also able to do what `MoveTowards` did, but setting a maximum direction delta and a maximum magnitude delta, so I joined the linear acceleration with the turning into a single `RotateTowards` operation.

But, before setting this acceleration and turn, I would rotate the machine's facing and created a separate maximum turning delta.
Now there are `facingTurnSpeed` and `turnSpeed`.
The first turns the machine's model (and camera) and sets the goal direction for the acceleration and turning, but if the model's turning speed is higher than the velocity's turning speed, the machine is not able to go exactly wher it is facing.
Drift achieved!

The larger the difference between turning deltas, the more a machine drifts.
Now the ground movement can be quite well tuned, but I still want to implement more details, like changing the turning speeds when braking.
There is also still some work to do on syncing up the speed direction when the machine is slow.
Maybe I'll do a more complex function for the `turnSpeed` instead of having it constant.

## Code architecture
Right now, I only have a ground movement script for everything, but I also created a `MovementState` class to switch between movement schemes depending on the machine's state.
I guess the next thing will be to set the machine to fall and to glide.

[2021-08-14]

I haven't done this yet, but I plan to separate the methods that accelerate, brake and rotate the machine from the main movement script.
This makes each movement script more consise and the parameters of methods can be swapped more easily and in a modular way.
This will also make each machine a huge script container, since each movement script has several other scripts and there is at least two movement scripts: grounded and gliding/jumping.
Oh well, I will start doing that once I finish the ground movement.

Yeah, that would also make it muuuuch easier to implement machines with different mechanics, like using the boost charge to accelerate, not being able to turn while accelerating etc.
_Boost_, by the way, is something that I feel like is very machine-specific.
Some may add velocity to the current velocity, some may replace the velocity for another, some may keep adding velocity for some amount of time...
Separating the movement modules will really make it easier to customize machines.

## Acceleration, top speed and turning/drifting
I have changed the acceleration to linearly interpolate towards the machine's facing (`Vector3.MoveTowards` instead of `Vector3.RotateTowards`) when the angle between the current velocity and the target velocity is larger than a threshold.
I set the threshold to 30 degrees and it ended up correcting the problem of velocity vector taking too long to reach the machine's facing after a sharp turn or brake.

Added a very simple boost method: when the brake button is pressed, a gauge starts filling up at a specific rate.
When the brake is released, a boost method is executed and, for now, it's just adding to the velocity (with `Vector3.ClampMagnitude` to avoid huge boosts).
Also added a velocity vector gizmo and a simple boost gauge.

Next, I will modularize the movement script, as I mentined.
After that, add pitch and machine orientation to the mix, then switch to flight/glide.

[2021-08-15]

Modularizing takes a lot of effort, between restructuring some parameters and variables and the fear that having to scrap your work because it is not working as it was before.
For now, I only created an Accelerate, Boost and Turn modules.
There will probably be a Pitch module as well.

The gliding or flying movement will probably have some some refactoring involved as well to fit with everything.
But I am optimistic that, the way I've made the acceleration method, it will integrate nicely.

## Gravity is still not good...
The gravity needs to be reworked.
Right now it's basically a constant velocity down... :| It should carry over from the previous (fixed) update.

[Later]

Gravity in the ground movement fixed through (what I believe is) a simple concept:
Whenever a method accelerates the machine _forward_ (that is, towards the `transform.forward`), it first decomposes the velocity into tangential and normal (in respect to the ground).
The tangential velocity is transformed, then the normal velocity is added back.
That seems to solve having the forward acceleration and turning immediately correcting the gravity.

## Nonrigid camera follow
I swapped the cinemachine camera aim to a _Composer_, which allows some angle changes and, most interestingly, adding some elasticity to the camera distance.
I found that having some sudden distancing of the machine from the camera when it boosts forward is very satisfying!
