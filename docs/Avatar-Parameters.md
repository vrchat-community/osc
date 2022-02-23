---
title: "OSC Avatar Parameters"
excerpt: ""
slug: "osc-avatar-parameters"
category: "620dac4c9751e40020be7447"
parent-doc: "620d7b41be3f830099d5a460"
gh-wiki-slug: "Avatar-Parameters"
---
# Overview
This API lets you drive parameters on your Avatar, and send parameters out to another OSC application.

# Avatar Changes
When a new Avatar is loaded by the local player with OSC enabled, a message will be sent to `/avatar/change` with the ID of the avatar. If an OSC config is generated or loaded, the absolute path to that config on the user's local drive will be sent to `/avatar/config`.
**Please note** that the config file system is a stop-gap to allow for some customization until we can integrate a proper in-client UI for OSC, and may be removed at some point.

# Avatar Parameters & Config Files
The general idea is that incoming values at the address `/avatar/parameters/name` will set the value of a matching parameter's name. So `/avatar/parameters/VRCEmote` with an Integer value will set the default VRCEmote parameter if you have it, and your avatar will start waving, dancing, etc. Here's a very simple TouchOSC doc that does exactly that: [vrc-emote.tosc](https://github.com/vrchat-community/osc/raw/main/files/touch-osc/vrc-emote.tosc )

To enable you to do more with your Avatar Parameters, we auto-generate config files that can be edited for customization. Note that configs are not saved to disk using Build & Test - so you can use OSC but you won't see the config until you publish.

When you load into a Published Avatar, your local storage is checked for an OSC config with that id (in `~\AppData\LocalLow\VRChat\VRChat\OSC\{userId}\Avatars\{avatarName}_{avatarId}.json`).
Here's what a real full path looks like:
```
C:\Users\Momo\AppData\LocalLow\VRChat\VRChat\OSC\usr_9381c776-ce11-4def-9331-6ffeacce027e\Avatars\PurpleMomo_avtr_9d58037b-23c7-4c9c-adbd-b1338178cd81.json
```

If found, the given input addresses will have their messages drive Avatar parameters as they come in, and the output addresses will have send messages with parameter values as they change.

If not found, a config is automatically generated and loaded, listening and sending every possible message.

**Example Config File Snippet**

```
{
    "id" : "avtr_9d58037b-23c7-4c9c-adbd-b1338178cd81",
    "name" : "PurpleMomo",
    "parameters" : [
        {
            "name" : "Face",
            "input" : {
                "address" : "/avatar/parameters/Face",
                "type" : "Int"
            },
            "output" : {
                "address" : "/ableton/trackselect",
                "type" : "Float"
            }
        },
        {
            "name" : "VelocityZ",
            "output" : {
                "address" : "/avatar/parameters/VelocityZ",
                "type" : "Float"
            }
        }
    ]
}
```

The avatar's name is included for readability, but only the id is used (and actually just the ID in the filename). Note that the input member is left blank for VelocityZ above, since it's input-only. Also, I've changed the output address and type for Face to show how a config change can help you directly control another application - in this case, changing the Face parameter will send values to Ableton Live through the address "/ableton/trackselect", and Ableton expects a Float so we ask VRChat to convert it on the fly.

Full Config: [example-avatar-config.json](https://github.com/vrchat-community/osc/raw/main/files/avatar-parameters/configs/example-avatar-config.json)
Here's a breakdown of each member of the array:

- name: **string** which must match the name of an Avatar Parameter
- input
    - address: **string** which will be listened to for incoming data
    - type: **string** of "Int", "Bool", or "Float" *only*, which is the expected type of the incoming data
- output
    - address: **string** which will be used for outgoing OSC messages, sending the new value of the parameter as it changes
    - type: **string** of "Int", "bool", or "Float" *only*, which is the type that will be sent out, regardless of the parameter's actual type

In the default generated config, the types and addresses will match exactly, but you can change the *input* types and addresses if you expect different data coming in, and change the *output* types and addresses if you want to send different data out. In this way, the config gives you a great way to interact with other systems without requiring a third-party program (however - for more advanced setups, you can use such a program to do further manipulation, scaling and combining data, etc.)