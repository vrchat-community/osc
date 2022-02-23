---
title: "OSC as Input Controller"
excerpt: ""
slug: "osc-as-input-controller"
category: "620dac4c9751e40020be7447"
parent-doc: "620d7b41be3f830099d5a460"
gh-wiki-slug: "Input"
---
# OSC as Input Controller
We support **a lot** of different controllers in VRChat - keyboard + mouse, gamepad, a plethora of VR trackers, etc. We're opening up that further by enabling you to control _most_ of the inputs over OSC as well! With this, you could create a program to control VRChat with your head, feet, brainwaves, a musical sequencer or just about anything else!

# Addresses
Each input watches for data at `/input/Name` like `/input/Jump`.
There are two types of inputs:
1. **Axes** expect a float from -1 to 1 to control things like movement. They expect to reset to 0 when not in use - otherwise a 'MoveForward' message left at '1' will continue to move you forward forever!
2. **Buttons ** expect an int of 1 for 'pressed' and 0 for 'released'. They will not function correctly without resetting to 0 first - sending `/input/Jump 1` and then `/input/Jump 1` will only result in a single jump.

A good way to get started with experiments is to controls to TouchOSC to test them out and see how they work in the client. Some will only work in Desktop or VR modes, and others may not do much at all. You can report controls that don't work the way you'd expect, or request additional controls, in our [discussions area (COMING SOON)](#).

This TouchOSC demo has a control for Vertical & Horizontal Movement, and a button for Jump: [vrc-input.tosc](https://github.com/vrchat-community/osc/raw/main/files/touch-osc/vrc-input.tosc)

# Supported Inputs
<details>
<summary>Axes</summary>


`/input/Vertical` : Move forwards (1) or Backwards (-1)

`/input/Horizontal` : Move right (1) or left (-1)

`/input/LookHorizontal` : Look Left and Right. Smooth in Desktop, VR will do a snap-turn when the value is 1 if Comfort Turning is on.

`/input/UseAxisRight` : Use held item - not sure if this works

`/input/GrabAxisRight` : Grab item - not sure if this works

`/input/MoveHoldFB` : Move a held object forwards (1) and backwards (-1)

`/input/SpinHoldCwCcw` : Spin a held object Clockwise or Counter-Clockwise

`/input/SpinHoldUD` :  Spin a held object Up or Down

`/input/SpinHoldLR` : Spin a held object Left or Right

</details>

<details>

<summary>Buttons</summary>

`/input/MoveForward` : Move forward while this is 1.

`/input/MoveBackward` : Move backwards while this is 1.

`/input/MoveLeft` : Strafe left while this is 1.

`/input/MoveRight` : Strafe right while this is 1.

`/input/LookLeft` : Turn to the left while this is 1. Smooth in Desktop, VR will do a snap-turn if Comfort Turning is on.

`/input/LookRight` : Turn to the right while this is 1. Smooth in Desktop, VR will do a snap-turn if Comfort Turning is on.

`/input/Jump` : Jump if the world supports it.

`/input/Run` : Walk faster if the world supports it.

`/input/ComfortLeft` : Snap-Turn to the left - VR Only.

`/input/ComfortRight` : Snap-Turn to the right - VR Only.

`/input/DropRight` : Drop the item held in your right hand - VR Only.

`/input/UseRight` : Use the item highlighted by your right hand - VR Only.

`/input/GrabRight` : Grab the item highlighted by your right hand - VR Only.

`/input/DropLeft` : Drop the item held in your left hand - VR Only.

`/input/UseLeft` : Use the item highlighted by your left hand - VR Only.

`/input/UseRight` : Grab the item highlighted by your right hand - VR Only.

`/input/GrabLeft` : Grab the item highlighted by your left hand - VR Only.

`/input/PanicButton` Turn on Safe Mode.

`/input/QuickMenuToggleLeft` : Toggle QuickMenu On/Off. Will toggle upon receiving '1' if it's currently '0'. 

`/input/QuickMenuToggleRight` Toggle QuickMenu On/Off. Will toggle upon receiving '1' if it's currently '0'.

`/input/Voice` : Toggle Voice - the action will depend on whether "Toggle Voice" is turned on in your Settings. If so, then changing from 0 to 1 will toggle the state of mute. If "Toggle Voice" is turned off, then it functions like Push-To-Mute - 1 is muted, 0 is unmuted.

</details>