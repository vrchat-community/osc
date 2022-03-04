---
title: "OSC Resources"
excerpt: ""
slug: "osc-resources"
category: "620dac4c9751e40020be7447"
parent-doc: "620d7b41be3f830099d5a460"
gh-wiki-slug: "Resources"
---

# Tools and Projects for OSC Development and Use

## WARNING: USE THESE AT YOUR OWN RISK!
It is INHERENTLY DANGEROUS to run code that someone else has written. Most of the projects below are open-source so you can verify that they are safe to use. If possible, read through the code and compile it yourself instead of using compiled binaries (exe files, etc).

## Submissions
If you would like submit your own project for consideration, please post it to [Show and Tell](https://github.com/vrchat-community/osc/discussions/categories/show-and-tell). To be considered, your project should be documented, open-source, include a license, and the project page must not violate or suggest violations of [VRChat's TOS](https://hello.vrchat.com/legal), or link to any pages that do. We will update this list weekly, adding and removing projects. 

---

# The List

## Utilities
* [AV3 Emulator](https://github.com/lyuma/Av3Emulator): Avatar Emulator with OSC Support (Unity Editor)
* [TouchOSC](https://hexler.net/touchosc): Scriptable OSC Interface Application (App)
* [Protokol](https://hexler.net/protokol): App to monitor and log OSC (App)

## Libraries
* [OscCore](https://github.com/stella3d/OscCore): A performance-oriented OSC library for Unity, what we use within VRChat
* [VRCOSCGUI](https://github.com/YABam/VRCOSCGUI): OSCSender for VRChat based on Plugins (C#, C++, C)
* [phorcys](https://github.com/kb10uy/phorcys): Open Sound Control (OSC) implementation and VRChat OSC API tools written in Rust! (Rust)
* [VRC_OSCLib](https://github.com/Irisl0/VRC_OSCLib): Library for using OSC in VRChat (Python)
* [OSCLib-for-ESP8266](https://github.com/stahlnow/OSCLib-for-ESP8266): Send OSC from microcontrollers for DIY hardware (Arduino)

## Heart Rate
* [HRPResence](https://github.com/Naraenda/HRPresence): Windows GATT heartrate monitor tool that pushes BPM to OpenSoundControl (OSC) for VRChat and DiscordRPC. (C#)
* [miband-heartrate-osc](https://github.com/mkc1370/miband-heartrate-osc): Enable and monitor heartrate with Mi Band device on Windows 10. (C#)
* [pulsoid-to-vrchat-osc](https://github.com/Sonic853/pulsoid-to-vrchat-osc): Send Heart Rate to VRChat over OSC via Pulsoid (NodeJS)
* [hr-osc](https://github.com/kamyu1537/hr-osc): Send Heart Rate from Stromno to Avatar (Go)
* [vrc-osc-miband-hrm](https://github.com/vard88508/vrc-osc-miband-hrm): Sends data from Mi Band / Amazfit HR Monitor to Avatar (NodeJS)
* [BluetoothHeartRateOSC](https://github.com/AkaiMage/BluetoothHeartRateOSC): Bluetooth Heart Rate reading program that writes to VRChat OSC (C#)

## Face Tracking
* [OpenSeeFace](https://github.com/emilianavt/OpenSeeFace): Robust realtime face and facial landmark tracking on CPU with Unity integration (Python)
* [VSeeFace](https://www.vseeface.icu/) Standalone program with Face and Hand tracking, adding some direct VRChat Avatar messages (App)
* [VRChat-MotionOSC](https://github.com/rogeraabbccdd/VRChat-MotionOSC): Webcam to Avatar Parameters (NodeJS)

## IRL Control

* [HardwareStatToVRChat](https://github.com/Nifty255/HardwareStat2VRChat): Use OSC to show your CPU and RAM usage on your VRChat avatar! (Go, NodeJS)
* [vrcwatch](https://github.com/mezum/vrcwatch): Sends time over OSC (Python)
* [mqtt2osc](https://github.com/asleeponduty/mqtt2osc): Subscribes to a MQTT topic and publishes to an OSC topic (Python)
* [vrc-osc-audio-controls](https://github.com/uzair-ashraf/vrc-osc-audio-controls): Control system audio playback via OSC (Go)

## Hand Tracking

* [leapmotion-osc](https://github.com/adeleine1412/leapmotion-osc): Leap Motion Controller finger tracking for VRChat OSC (NodeJS)

## Misc
* [TwitchVrcAvatarOSC](https://github.com/Killers0992/TwitchVrcAvatarOSC): Twitch bot to manipulate avatar
* [OSCKeyboard](https://github.com/ShadowForests/OSCKeyboard): Send Windows keyboard input to Avatar keyboard (Python)
* [vrc-worldobject](https://github.com/seanedwards/vrc-worldobject): Create world space props that are network-synced for late joiners. (TouchOSC, C#)